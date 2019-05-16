using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.CRM.Service.CRMService.Modle;
using System.IO;
using ETS.Framework;
using ETS2.PM.Service.Meituan.Model;
using ETS2.PM.Service.Meituan.Data;
using ETS2.CRM.Service.CRMService.Data;
using Newtonsoft.Json;
using ETS2.VAS.Service.VASService.Data;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.WebApp.Meituan_jk
{
    public partial class orderRefundedMessage : System.Web.UI.Page
    {
    /// <summary>
    /// 美团已退款消息(基本不会出现这种情况，防范美团不经退款审核退款而造成用户票已经退但是仍然可以用的情况)
    /// 美团对如下三种退款不会走商家退款审核：美团客服和商家核实可以退款的客服操作退款、商家审核退款申请超过三个工作日美团自动退款、商家支付结果未知美团多次通过订单查询也没查询到支付成功时退款。
    /// 对于上述三种退款，美团会通过该接口告知商家系统商，也会通过邮件形式告知商家人员。请系统商及时撤销支付凭证避免用户既得到了退款又能入园，因此建议商家对接该接口。
    /// 支付结果商家明确返回“支付失败”的这种情况，不会通过该接口告知商家。
    /// 三种退款通知type对应在“已退款消息类型”映射表中。
    /// 1	用户未消费美团客服操作退款
    /// 2	支付状态未知，未查询到支付成功，美团自动退款
    /// 3	商家退款审核超时，美团自动退款
    /// </summary> 
        private Agent_company agentinfo;
        readonly string _requestParam;

        public orderRefundedMessage()
        {
            _requestParam = GetRequestStreamString();
        }

        public string GetRequestStreamString()
        {
            StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Request.InputStream);
            return reader.ReadToEnd();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            #region 记入日志表Meituan_reqlog
            string reqip = CommonFunc.GetRealIP();
            Meituan_reqlog mlog = new Meituan_reqlog
            {
                id = 0,
                reqstr = _requestParam,
                subtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                respstr = "",
                resptime = "",
                code = "",
                describe = "",
                req_type = "",
                sendip = reqip,
                stockagentcompanyid = 0
            };
            int logid = new Meituan_reqlogData().EditReqlog(mlog);
            mlog.id = logid;
            #endregion


            string date = System.Web.HttpContext.Current.Request.Headers.Get("Date");
            string PartnerId = System.Web.HttpContext.Current.Request.Headers.Get("PartnerId");
            string Authorization = System.Web.HttpContext.Current.Request.Headers.Get("Authorization");
            string requestMethod = System.Web.HttpContext.Current.Request.HttpMethod;
            string URI = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;
            //authorization 形式: "MWS" + " " + client + ":" + sign;
            string mtSign = Authorization.Substring(Authorization.IndexOf(":") + 1);

            mlog.req_type = URI;

            agentinfo = new AgentCompanyData().GetAgentCompanyByMeituanPartnerId(PartnerId);
            #region 验证是否已经配置美团合作商信息
            if (agentinfo == null)
            {
                var response = new MtpApiResponse();
                response.partnerId = int.Parse(PartnerId);
                response.code = 300;
                response.describe = "分销尚未配置美团合作商信息(合作商PartnerId:" + PartnerId + ")";

                string json = JsonConvert.SerializeObject(response);

                //把处理结果录入日志
                mlog.respstr = json;
                mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                mlog.code = response.code.ToString();
                mlog.describe = response.describe;
                new Meituan_reqlogData().EditReqlog(mlog);

                LogHelper.RecordSelfLog("Error", "meituan", response.describe);

                Response.Write(json);
                return;
            }
            #endregion

            mlog.stockagentcompanyid = agentinfo.Id;

            #region 签名验证
            string beforeSign = requestMethod + " " + URI + "\n" + date;
            string afterSign = new MeiTuanInter(agentinfo.mt_partnerId, agentinfo.mt_secret, agentinfo.mt_client).GetSign(beforeSign);
            //判断签名是否正确
            if (afterSign != mtSign)
            {
                var response = new MtpApiResponse();
                response.partnerId = int.Parse(PartnerId);
                response.code = 300;
                response.describe = "签名验证失败";

                string json = JsonConvert.SerializeObject(response);

                //把处理结果录入日志
                mlog.respstr = json;
                mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                mlog.code = response.code.ToString();
                mlog.describe = response.describe;
                new Meituan_reqlogData().EditReqlog(mlog);

                LogHelper.RecordSelfLog("Error", "meituan", "拉取产品 签名错误 mtSign-" + mtSign + "  meSign-" + afterSign);

                Response.Write(json);
                return;
            }
            #endregion

            string actionResult = GetDealNoticeSend(mlog);

            Response.Write(actionResult);
        }

        private string GetDealNoticeSend(Meituan_reqlog mlog)
        {
            var response = new MtpApiResponse();
            response.partnerId = agentinfo.mt_partnerId.ConvertTo<int>(0);
            try
            {
                var data = JsonConvert.DeserializeObject<MtpOrderRefundedMessageRequest>(_requestParam.Replace("operator", "operator1"));
                if (data.body == null)
                {
                    response.code = 300;
                    response.describe = "Body数据解析失败";
                }
                else
                {
                    MtpOrderRefundedMessageRequestBody body = data.body;
                    /*
                     * 把电子票作废，数量清零 
                     * 1.根据美团订单号得到系统订单号
                     * 2.根据系统订单号得到电子票
                     * 3.把电子票作废
                     */
                    int sysOrderid = new Meituan_reqlogData().GetSysorderidByMtorderid(body.orderId.ToString());
                    if (sysOrderid == 0)
                    {
                        //把这次操作计入日志文档中
                        LogHelper.RecordSelfLog("Error", "美团重要错误记录", "1--美团订单号：" + body.orderId + ",系统订单号:" + body.partnerOrderId + ",退款流水号:" + body.refundSerialNo + ",凭证码：" + String.Join(",", body.voucherList) + ",单张门票退款金额:" + body.refundPrice + ",已退款消息类型:" + body.refundMessageType + ",退款原因:" + body.reason + ",退款时间:" + body.refundTime + ",退款份数:"+body.count);
                    }
                    else 
                    {
                        string pno = new SendEticketData().HuoQuEticketPno(sysOrderid);
                        if (pno == "")
                        {
                            //把这次操作计入日志文档中
                            LogHelper.RecordSelfLog("Error", "美团重要错误记录", "2--美团订单号：" + body.orderId + ",系统订单号:" + body.partnerOrderId + ",退款流水号:" + body.refundSerialNo + ",凭证码：" + String.Join(",", body.voucherList) + ",单张门票退款金额:" + body.refundPrice + ",已退款消息类型:" + body.refundMessageType + ",退款原因:" + body.reason + ",退款时间:" + body.refundTime + ",退款份数:" + body.count);
                        }
                        else 
                        {
                            try
                            {
                                string[] pnoarr = pno.Split(',');
                                for (var i = 0; i < pnoarr.Length; i++)
                                {
                                    if (pnoarr[i] != "")
                                    {
                                        //清空电子票数量
                                        int r = new B2bEticketData().ClearPnoNum(pnoarr[i]);
                                    }
                                }
                            }
                            catch { }

                            //把这次操作计入日志文档中
                            LogHelper.RecordSelfLog("Error", "美团重要错误记录", "3--美团订单号：" + body.orderId + ",系统订单号:" + body.partnerOrderId + ",退款流水号:" + body.refundSerialNo + ",凭证码：" + String.Join(",", body.voucherList) + ",单张门票退款金额:" + body.refundPrice + ",已退款消息类型:" + body.refundMessageType + ",退款原因:" + body.reason + ",退款时间:" + body.refundTime + ",退款份数:" + body.count);
                        }

                       
                    }

                    
                }
                response.code = 200;
                response.describe = "successful";
                response.partnerId = int.Parse(agentinfo.mt_partnerId);
            }
            catch (Exception ex)
            {
                response.code = 300;
                response.describe = "异常错误";

            }
            string json = JsonConvert.SerializeObject(response);
            #region 把处理结果录入数据库
            mlog.respstr = json;
            mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            mlog.code = response.code.ToString();
            mlog.describe = response.describe;
            new Meituan_reqlogData().EditReqlog(mlog);
            #endregion

            return json;
        }
    }
}
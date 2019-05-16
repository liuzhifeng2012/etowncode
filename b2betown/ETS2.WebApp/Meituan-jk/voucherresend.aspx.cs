using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ETS.Framework;
using ETS2.PM.Service.Meituan.Model;
using ETS2.PM.Service.Meituan.Data;
using Newtonsoft.Json;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.VAS.Service.VASService.Data;

namespace ETS2.WebApp.Meituan_jk
{
    /// <summary>
    /// 重新发送电子门-新接口已经取消
    /// </summary>
    public partial class voucherresend : System.Web.UI.Page
    {
        private Agent_company agentinfo;
        readonly string _requestParam;

        public voucherresend()
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
                var response = new VoucherResponse();
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
                var response = new VoucherResponse();
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

            string actionResult = Getorderresend(mlog);

            Response.Write(actionResult);

             
        }

        ///     重发电子码
        public string Getorderresend(Meituan_reqlog mlog)
        {
            return "";
            //var response = new VoucherResponse();
            //response.partnerId = int.Parse(agentinfo.mt_partnerId);
            //try
            //{
            //    var data = JsonConvert.DeserializeObject<VoucherRequest>(_requestParam);
            //    if (data.body == null)
            //    {
            //        response.code = 300;
            //        response.describe = "Body数据解析失败";
            //        return EditMTlog_Err(response, mlog);
            //    }
            //    else
            //    {
            //        VoucherRequestBody body = data.body;

            //        string mtorderid = body.bookOrderId;
            //        string ordernum = body.partnerOrderId;

            //        mlog.mtorderid = body.bookOrderId;
            //        mlog.ordernum = body.partnerOrderId;

            //        //根据机构号获得机构(分销商)信息
            //        Agent_company agentcompany = new AgentCompanyData().GetAgentCompanyByName("美团旅游");
            //        if (agentcompany == null)
            //        {
            //            response.code = 300;
            //            response.describe = "不存在 美团旅游 分销账户";
            //            return EditMTlog_Err(response, mlog);
            //        }
            //        int organization = agentcompany.Id;

            //        //判断分销商查询订单是否是 自己发送的接口订单
            //        Agent_requestlog mrequestlogg = new Agent_requestlogData().GetAgent_addorderlogByReq_seq(organization.ToString(), mtorderid);
            //        if (mrequestlogg == null)
            //        {
            //            response.code = 300;
            //            response.describe = "当前查询的订单不存在";
            //            return EditMTlog_Err(response, mlog);
            //        }
            //        if (mrequestlogg.Is_dealsuc == 0)
            //        {
            //            response.code = 300;
            //            response.describe = "当前查询的订单不存在";
            //            return EditMTlog_Err(response, mlog);
            //        }
            //        if (mtorderid.Trim() == "")
            //        {
            //            response.code = 300;
            //            response.describe = "美团订单号 不可为空";
            //            return EditMTlog_Err(response, mlog);
            //        }

            //        string pno = "";

            //        B2b_order morder = new B2bOrderData().GetOrderById(ordernum.ConvertTo<int>(0));
            //        if (morder == null)
            //        {
            //            response.code = 300;
            //            response.describe = "订单查询失败";
            //            return EditMTlog_Err(response, mlog);
            //        }
            //        pno = morder.Pno;

            //        if (morder.Bindingagentorderid > 0)
            //        {
            //            pno = new B2bOrderData().GetPnoByOrderId(morder.Bindingagentorderid);
            //        }

            //        if (pno == "")
            //        {
            //            response.code = 300;
            //            response.describe = "电子码查询失败";
            //            return EditMTlog_Err(response, mlog);
            //        }

            //        var vasmodel = new SendEticketData().SendEticket(ordernum.ConvertTo<int>(0), 2);//重发电子码
            //        if (vasmodel == "OK")
            //        {

            //            response.code = 200;
            //            response.describe = "success";
            //            response.body = new VoucherResponseBody
            //            {
            //                voucherType = (int)Meituan_voucherType.Message,
            //                voucher = pno
            //            };
            //            return EditMTlog_Err(response, mlog);
            //        }
            //        else
            //        {
            //            response.code = 300;
            //            response.describe = "重发电子码失败";
            //            return EditMTlog_Err(response, mlog);
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    response.code = 300;
            //    response.describe = "查询产品异常";

            //    return EditMTlog_Err(response, mlog);
            //}

        }
        //编辑日志
        public string EditMTlog_Err(VoucherResponse response, Meituan_reqlog mlog)
        {
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
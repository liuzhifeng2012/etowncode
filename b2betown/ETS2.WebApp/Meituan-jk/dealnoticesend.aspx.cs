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
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.WebApp.Meituan_jk
{
    public partial class dealnoticesend : System.Web.UI.Page
    {
        /// <summary>
        /// 产品编审状态通知
        /// </summary> 
        private Agent_company agentinfo;
        readonly string _requestParam;

        public dealnoticesend()
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
                var data = JsonConvert.DeserializeObject<MtpDealSendNoticeRequest>(_requestParam);
                if (data.body == null)
                {
                    response.code = 300;
                    response.describe = "Body数据解析失败";
                }
                else
                {
                    List<MtpDealSendNoticeRequestBody> body = data.body;
                    foreach(MtpDealSendNoticeRequestBody nboy in body)
                    {
                        string partnerDealId = nboy.partnerDealId;
                        int status = nboy.status;
                        int checkStatus = nboy.checkStatus;
                        int msRatioCheckStatus = nboy.msRatioCheckStatus;

                        //产品上线状态：默认未上单
                        int groupbuystatus = 0;
                        if(status==1)
                        {
                            groupbuystatus = 1;//已上单
                        }
                        //产品上线状态描述
                        string groupbuystatusdesc = EnumUtils.GetName((Meituan_OnlineStatus)status) + ",审核状态:" + EnumUtils.GetName((Meituan_CheckStatus)checkStatus);
                        int r = new B2b_com_pro_groupbuystocklogData().UpGroupbuystatus(partnerDealId.ConvertTo<int>(0),agentinfo.Id,groupbuystatus,groupbuystatusdesc);
                       
                        response.code = 200;
                        response.describe = "successful";
                        response.partnerId = int.Parse(agentinfo.mt_partnerId);
                    }
                }
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.PM.Service.LMM.Data;
using ETS2.PM.Service.LMM.Model;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using System.IO;
using ETS2.CRM.Service.CRMService.Data;
using Newtonsoft.Json;
using ETS.JsonFactory;
using System.Xml;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.VAS.Service.VASService.Data;
using ETS2.PM.Service.WL.Data;
using System.Runtime.Remoting.Messaging;


namespace ETS2.WebApp.lvmama
{
    public partial class sms_resend : System.Web.UI.Page
    {
       
        private Agent_company agentinfo;
        string uid = "";
        string password = "";
        readonly string _requestParam;

        public sms_resend()
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


            #region 记入日志表LVmama_reqlog
            string reqip = CommonFunc.GetRealIP();
            Lvmama_reqlog mlog = new Lvmama_reqlog
            {
                id = 0,
                reqstr = _requestParam,
                subtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                respstr = "",
                resptime = "",
                code = "",
                describe = "",
                req_type = "sms_resend",
                sendip = reqip,
                stockagentcompanyid = 0
            };
            int logid = new lvmama_reqlogData().EditReqlog(mlog);
            mlog.id = logid;
            #endregion

            var data = JsonConvert.DeserializeObject<sms_resendmodel>(_requestParam);
            if (data == null)
            {
                return;
            }
            try
            {

                uid = data.uid;
                password = data.password;


                string timestamp = data.timestamp;
                string sign = data.sign;
                string extId = data.extId;


                agentinfo = new AgentCompanyData().GetAgentCompanyByLvmamaPartnerId(uid);

                LVMAMA_Data lvmamadata = new LVMAMA_Data(agentinfo.Lvmama_uid, agentinfo.Lvmama_password, agentinfo.Lvmama_Apikey);


                #region 验证是否已经配置驴妈妈合作商信息
                if (agentinfo == null)
                {
                    var response = new apply_codeRefund();
                    response.uid = uid;
                    response.orderId = "0";
                    response.status = "2";
                    response.msg = "分销尚未配置驴妈妈商信息(uid:" + uid + ")";

                    string json = JsonConvert.SerializeObject(response);

                    //把处理结果录入日志
                    mlog.respstr = "";
                    mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    mlog.code = response.status;
                    mlog.describe = response.msg;
                    new lvmama_reqlogData().EditReqlog(mlog);
                    LogHelper.RecordSelfLog("Error", "lvmama", response.msg);
                    Response.Write(json);
                    return;
                }
                #endregion


                mlog.stockagentcompanyid = agentinfo.Id;
                mlog.ordernum = extId;


                #region 签名验证
                string Md5Sign = lvmamadata.sms_resend_codemd5(data);
                string afterSign = lvmamadata.lumamasign(Md5Sign, agentinfo.Lvmama_Apikey);

                //判断签名是否正确
                if (afterSign != sign)
                {
                    var response = new apply_codeRefund();
                    response.uid = uid;
                    response.orderId = extId;
                    response.status = "2";
                    response.msg = "签名认证失败";

                    string json = JsonConvert.SerializeObject(response);

                    //把处理结果录入日志
                    mlog.respstr = "";
                    mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    mlog.code = response.status;
                    mlog.describe = response.msg;
                    new lvmama_reqlogData().EditReqlog(mlog);

                    LogHelper.RecordSelfLog("Error", "lvmama", "签名错误 lvmamamSign-" + sign + "  meSign-" + afterSign);

                    Response.Write(json);
                    return;
                }
                #endregion

                string actionResult = Smsresend(mlog, agentinfo);

                Response.Write(actionResult);
            }
            catch (Exception ex)
            {
                // Response.Write(ex);
            }
        }


        //     重发，
        public string Smsresend(Lvmama_reqlog mlog, Agent_company agentinfo)
        {
            var response = new backRefund();
            response.uid = agentinfo.Lvmama_uid;
            try
            {
                int organization = agentinfo.Id;
                #region 把分销商发送过来的请求记入数据库日志表2  agent_requestlog
                Agent_requestlog reqlog = new Agent_requestlog()
                {
                    Id = 0,
                    Organization = organization,
                    Encode_requeststr = "",
                    Decode_requeststr = _requestParam,
                    Request_time = DateTime.Now,
                    Encode_returnstr = "",
                    Decode_returnstr = "",
                    Return_time = DateTime.Parse("1970-01-01 00:00:00"),
                    Errmsg = "",
                    Request_type = "sms_resend",
                    Req_seq = "",
                    Ordernum = "",
                    Is_dealsuc = 0,
                    Is_second_receivereq = 0,
                    Request_ip = CommonFunc.GetRealIP()
                };
                int reqlogid = new Agent_requestlogData().Editagent_reqlog(reqlog);
                reqlog.Id = reqlogid;
                #endregion


                var data = JsonConvert.DeserializeObject<sms_resendmodel>(_requestParam);
                if (data.uid == "")
                {
                    response.uid = agentinfo.Lvmama_uid;
                    response.status = "1";
                    response.msg = "数据解析失败";
                    return EditLvmamalog_Order(response, mlog, reqlog);

                }
                else
                {

                   string uid = data.uid;
                   string password = data.password;
                   string sign = data.sign;
                   string timestamp = data.timestamp;
                   string extId = data.extId;

                   var vasmodel = new SendEticketData().SendEticket(extId.ConvertTo<int>(0), 2);//重发电子码

                   if (vasmodel == "OK")
                   {
                       response.status = "0";
                       response.msg = "重发成功";
                       return EditLvmamalog_Order(response, mlog, null);
                   }
                   else {

                       response.status = "14";
                       response.msg = vasmodel;
                       return EditLvmamalog_Order(response, mlog, null);
                   }
                 
                }
            }
            catch (Exception ex)
            {
                response.status = "2";
                response.msg = "异常错误";

                return EditLvmamalog_Order(response, mlog, null);
            }

        }


        //编辑日志
        public string EditLvmamalog_Order(backRefund response, Lvmama_reqlog mlog, Agent_requestlog reqlog)
        {
            string json = JsonConvert.SerializeObject(response);

            #region 把处理结果录入数据库
            mlog.respstr = json;
            mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            mlog.code = response.status;
            mlog.describe = response.msg;
            new lvmama_reqlogData().EditReqlog(mlog);
            #endregion

            #region  把处理结果录入分销接口通用日志表
            if (reqlog != null)
            {
                reqlog.Errmsg = json;
                reqlog.Decode_returnstr = json;
                reqlog.Encode_returnstr = "";
                reqlog.Return_time = DateTime.Now;

                new Agent_requestlogData().Editagent_reqlog(reqlog);
            }
            #endregion

            return json;
        }

    }
}
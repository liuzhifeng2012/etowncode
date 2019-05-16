using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;
using ETS2.PM.Service.LMM.Model;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace ETS2.PM.Service.LMM.Data
{
    public class LVMAMA_Data
    {
        public string uid;
        public string password;
        public string signKey;


        public LVMAMA_Data(string uid, string password, string signKey)
        {
            this.uid = uid;
            this.password = password;
            this.signKey = signKey;

            //this.uid = "lvmama1234";
            //this.password = "85a61c849";
            //this.signKey = "b373a69b5e004cd3a9928d10afb1ea3e";
             
        }


        #region 给驴妈妈发送废票通知
        public backRefund discardcodecallbacksend(discardcodecallbackmodel requestData, int agentcompanyid)
        {

            string url = "http://114.80.83.165/vst_passport/lvmamacallback/discardcodecallback.do";

            #region  记入日志表
            string reqip = CommonFunc.GetRealIP();
            Lvmama_reqlog mlog = new Lvmama_reqlog
            {
                id = 0,
                reqstr = JsonConvert.SerializeObject(requestData),
                subtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                respstr = "",
                resptime = "",
                code = "",
                describe = "",
                req_type = "discardcodecallback",
                sendip = reqip,
                stockagentcompanyid = agentcompanyid
            };
            int logid = new lvmama_reqlogData().EditReqlog(mlog);
            mlog.id = logid;
            #endregion


            backRefund result = new backRefund
            {
                IsSuccess = false,
            };
            try
            {
                var responseStr = DoRequest(url, JsonConvert.SerializeObject(requestData));
                if (string.IsNullOrEmpty(responseStr))
                {
                    result.msg = "返回数据为空";
                }
                else
                {
                    backRefund responseBody = (backRefund)JsonConvert.DeserializeObject(responseStr, typeof(backRefund));
                    if (responseBody.status == "0")
                    {
                        result.IsSuccess = true;
                        result.msg = JsonConvert.SerializeObject(responseBody);
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.msg = responseBody.msg;
                    }
                    //记录在日志表
                    mlog.code = responseBody.status;
                    mlog.describe = responseBody.msg;
                }
            }
            catch (Exception ex)
            {
                result.msg = "异常" + ex.Message;
            }

            #region  记录在日志表
            mlog.req_type = "discardcodecallback";
            mlog.respstr = result.msg;
            mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            new lvmama_reqlogData().EditReqlog(mlog);
            #endregion

            return result;
        }

        public discardcodecallbackmodel discardcodecall_json(string serialNo, string uid, string password, string status, string sign, string msg)
        {

            if (status == "0")
            {
                status = "APPROVE";
            }
            else {
                status = "REJECT";
            }

            string prochangetxt = "{" +
                              "\"msg\":\"\"," +
                              "\"serialNo\":\""+serialNo+"\"," +
                              "\"uid\":\""+uid+"\"," +
                              "\"password\":\"" + password + "\"," +
                              "\"status\":\"" + status + "\"," +
                              "\"sign\":\"" + sign + "\"," +
                              "\"msg\":\"" + msg + "\"" +
                          "}";
            return (discardcodecallbackmodel)JsonConvert.DeserializeObject(prochangetxt, typeof(discardcodecallbackmodel));
       
        
        }
        #endregion



        #region 给驴妈妈发送下单审核通知
        public backRefund asyConsumeNotify(asynnoticecallback requestData, int agentcompanyid)
        {

            string url = "http://114.80.83.165/vst_passport/lvmamacallback/asynnoticecallback.do";

            #region  记入日志表
            string reqip = CommonFunc.GetRealIP();
            Lvmama_reqlog mlog = new Lvmama_reqlog
            {
                id = 0,
                reqstr = JsonConvert.SerializeObject(requestData),
                subtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                respstr = "",
                resptime = "",
                code = "",
                describe = "",
                req_type = "",
                sendip = reqip,
                stockagentcompanyid = agentcompanyid
            };
            int logid = new lvmama_reqlogData().EditReqlog(mlog);
            mlog.id = logid;
            #endregion


            backRefund result = new backRefund
            {
                IsSuccess = false,
            };
            try
            {
                var responseStr = DoRequest(url, JsonConvert.SerializeObject(requestData));
                if (string.IsNullOrEmpty(responseStr))
                {
                    result.msg = "返回数据为空";
                }
                else
                {
                    backRefund responseBody = (backRefund)JsonConvert.DeserializeObject(responseStr, typeof(backRefund));
                    if (responseBody.status == "0")
                    {
                        result.IsSuccess = true;
                        result.msg = JsonConvert.SerializeObject(responseBody);
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.msg = responseBody.msg;
                    }
                    //记录在日志表
                    mlog.code = responseBody.status;
                    mlog.describe = responseBody.msg;
                }
            }
            catch (Exception ex)
            {
                result.msg = "异常" + ex.Message;
            }

            #region  记录在日志表
            mlog.req_type = "asynnoticecallback";
            mlog.respstr = result.msg;
            mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            new lvmama_reqlogData().EditReqlog(mlog);
            #endregion

            return result;
        }

        public asynnoticecallback asyConsumeNotify_json(string serialNo, string uid, string password, string status, string sign, string authCode, string codeURL, string orderId)
        {
            string prochangetxt = "{" +
                              "\"msg\":\"\"," +
                              "\"serialNo\":\"" + serialNo + "\"," +
                              "\"uid\":\"" + uid + "\"," +
                              "\"password\":\"" + password + "\"," +
                              "\"status\":\"" + status + "\"," +
                              "\"sign\":\"" + sign + "\"," +
                              "\"authCode\":\"" + authCode + "\"," +
                              "\"codeURL\":\"" + codeURL + "\"," +
                              "\"orderId\":\"" + orderId + "\"" +
                          "}";
            return (asynnoticecallback)JsonConvert.DeserializeObject(prochangetxt, typeof(asynnoticecallback));


        }
        #endregion


        #region 给驴妈妈发送核销通知
        public backRefund useConsumeNotify(usedticketscallback requestData, int agentcompanyid)
        {


            string url = "http://114.80.83.165/vst_passport/lvmamacallback/usedticketscallback.do";
                          

            #region  记入日志表
            string reqip = CommonFunc.GetRealIP();
            Lvmama_reqlog mlog = new Lvmama_reqlog
            {
                id = 0,
                reqstr = JsonConvert.SerializeObject(requestData),
                subtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                respstr = "",
                resptime = "",
                code = "",
                describe = "",
                mtorderid = requestData.serialNo,
                req_type = "",
                sendip = reqip,
                stockagentcompanyid = agentcompanyid
            };
            int logid = new lvmama_reqlogData().EditReqlog(mlog);
            mlog.id = logid;
            #endregion


            backRefund result = new backRefund
            {
                IsSuccess = false,
            };
            try
            {
                var responseStr = DoRequest(url, JsonConvert.SerializeObject(requestData));
                if (string.IsNullOrEmpty(responseStr))
                {
                    result.msg = "返回数据为空";
                }
                else
                {
                    backRefund responseBody = (backRefund)JsonConvert.DeserializeObject(responseStr, typeof(backRefund));
                    if (responseBody.status == "0" )
                    {
                        result.IsSuccess = true;
                        result.msg = JsonConvert.SerializeObject(responseBody);
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.msg = responseBody.msg;
                    }
                    //记录在日志表
                    mlog.code = responseBody.status;
                    mlog.describe = responseBody.msg;
                }
            }
            catch (Exception ex)
            {
                result.msg = "异常" + ex.Message;
            }

            #region  记录在日志表
            mlog.req_type = "usedticketscallback";
            mlog.respstr = result.msg;
            mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            new lvmama_reqlogData().EditReqlog(mlog);
            #endregion

            return result;
        }


        public usedticketscallback usedticketscallback_json(string serialNo, string uid, string password, string status, string sign, string usedTime, string usedCount)
        {
            string prochangetxt = "{" +
                              "\"msg\":\"\"," +
                              "\"serialNo\":\""+serialNo+"\"," +
                              "\"uid\":\""+uid+"\"," +
                              "\"password\":\"" + password + "\"," +
                              "\"status\":\"" + status + "\"," +
                              "\"sign\":\"" + sign + "\"," +
                              "\"usedCount\":\"" + usedCount + "\"," +
                              "\"usedTime\":\"" + usedTime + "\"" +
                             
                          "}";
            return (usedticketscallback)JsonConvert.DeserializeObject(prochangetxt, typeof(usedticketscallback));
       
        
        }
        #endregion



        //MD5加密下单
        public string lumamamd5(apply_codemodel m)
        {
            string s = m.num + m.password + m.serialNo + m.settlePrice + m.supplierGoodsId + m.timestamp + m.uid + m.visitTime;
            string s_temp = EncryptionHelper.ToMD5ToLower(s, "UTF-8");
            return s_temp;
        }

        //核销通知
        public string usedticketscallbackmd5(usedticketscallback m)
        {
            string s = m.msg + m.password + m.serialNo + m.status + m.uid + m.usedCount + m.usedTime;
            string s_temp = EncryptionHelper.ToMD5ToLower(s, "UTF-8");
            return s_temp;
        }

        //审核
        public string asynnoticecallbackmd5(asynnoticecallback m)
        {
            string s = m.authCode + m.codeURL + m.msg + m.orderId + m.password + m.serialNo + m.status + m.uid;
            string s_temp = EncryptionHelper.ToMD5ToLower(s, "UTF-8");
            return s_temp;
        }

        //废单加密
        public string discard_codemd5(discard_codemodel m)
        {
            string s = m.extId + m.password + m.timestamp + m.uid;
            string s_temp = EncryptionHelper.ToMD5ToLower(s, "UTF-8");
            return s_temp;
        }

        //废单通知加密
        public string discardcall_codemd5(discardcodecallbackmodel m)
        {
            string s = m.msg + m.password + m.serialNo + m.status + m.uid;
            string s_temp = EncryptionHelper.ToMD5ToLower(s, "UTF-8");
            return s_temp;
        }

        //重发短信加密
        public string sms_resend_codemd5(sms_resendmodel m)
        {
            string s = m.extId + m.password + m.timestamp + m.uid ;
            string s_temp = EncryptionHelper.ToMD5ToLower(s, "UTF-8");
            return s_temp;
        }
        

        public string lumamasign(string md5str,string key)
        {
            string s_temp = EncryptionHelper.ToMD5ToLower(md5str + key, "UTF-8");
            return s_temp;
        }


        public string DoRequest(string url, string data)
        {
            DateTime date = DateTime.Now;
            return PostResponse(url, data, date, 200000);
        }

        public static string GetCurrentDateStr()
        {
            return DateTime.Now.GetDateTimeFormats('r')[0];
        }
        public string PostResponse(string url, string postData, DateTime date, int Timeout)
        {

            //HttpRequestBase
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Date = date;
            var indexOf = url.IndexOf("/", 10, StringComparison.Ordinal);

            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8 ";
            request.Timeout = Timeout;
            request.ServicePoint.Expect100Continue = false;
            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = bytes.Length;
            HttpWebResponse response = null;
            try
            {
                request.GetRequestStream().Write(bytes, 0, bytes.Length);
                response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                {
                    return ex.Message;
                }
                WebResponse wr = ex.Response;
                Stream st = wr.GetResponseStream();
                StreamReader sr = new StreamReader(st, System.Text.Encoding.UTF8);
                string sError = sr.ReadToEnd();
                sr.Close();
                st.Close();
                return sError;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
        }



    }
}

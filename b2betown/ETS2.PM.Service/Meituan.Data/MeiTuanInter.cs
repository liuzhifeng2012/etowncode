using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using ETS.Framework;
using ETS2.PM.Service.Meituan.Model;
using ETS2.PM.Service.PMService.Modle.Enum;

namespace ETS2.PM.Service.Meituan.Data
{
    public class MeiTuanInter
    {
        public string mt_partnerId;
        public string mt_secret;
        public string mt_client;

        //public MeiTuanInter()
        //{
        //    this.mt_partnerId = "1122";
        //    this.mt_secret = "fd9094915faf06472766b4956d6659e8";
        //    this.mt_client = "lvyou_langyashan";
        //}
        public MeiTuanInter(string partnerid,string secret,string client) {
            this.mt_partnerId = partnerid;
            this.mt_secret = secret;
            this.mt_client = client;
        }

        #region 给美团发送产品变化通知 

        public ReturnResult ProductChangedNotify(DealChangeNotice requestData, int agentcompanyid)
        {
            //测试地址
            //string url = "http://lvyou.meituan.com/rhone-doc/lv/deal/change/notice";
            ////正式地址
            //string url = "http://lvyou.meituan.com/rhone/lv/deal/change/notice";
            string url = "http://lvyou.meituan.com/rhone/mtp/api/deal/change/notice";
            #region  记入日志表
            string reqip = CommonFunc.GetRealIP();
            Meituan_reqlog mlog = new Meituan_reqlog
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
            int logid = new Meituan_reqlogData().EditReqlog(mlog);
            mlog.id = logid;
            #endregion

            ReturnResult result = new ReturnResult
            {
                IsSuccess = false,
            };
            try
            {
                var responseStr = DoRequest(url, JsonConvert.SerializeObject(requestData));
                if (string.IsNullOrEmpty(responseStr))
                {
                    result.Message = "返回数据为空";
                }
                else
                {
                    PushResponseBody responseBody = (PushResponseBody)JsonConvert.DeserializeObject(responseStr, typeof(PushResponseBody));
                    if (responseBody.code == "200" && responseBody.describe == "success")
                    {
                        result.IsSuccess = true;
                        result.Message = JsonConvert.SerializeObject(responseBody);
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = responseBody.describe;
                    }

                    //记录在日志表
                    mlog.code = responseBody.code;
                    mlog.describe = responseBody.describe;
                }
            }
            catch (Exception ex)
            {
                result.Message = "异常" + ex.Message;
            }

            #region  记录在日志表
            mlog.req_type = "/rhone/lv/deal/change/notice";
            mlog.respstr = result.Message;
            mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            new Meituan_reqlogData().EditReqlog(mlog);
            #endregion

            return result;
        }
        #endregion

        #region 给美团发送验证通知
        public ReturnResult ConsumeNotify(OrderConsumeNotice requestData,int agentcompanyid)
        {
            //测试地址
            //string url = "http://lvyou.meituan.com/rhone-doc/lv/order/consume/notice";
            ////正式地址
            //string url = "http://lvyou.meituan.com/rhone/lv/order/consume/notice";

            string url = "http://lvyou.meituan.com/rhone/mtp/api/order/consume/notice";

            #region  记入日志表
            string reqip = CommonFunc.GetRealIP();
            Meituan_reqlog mlog = new Meituan_reqlog
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
            int logid = new Meituan_reqlogData().EditReqlog(mlog);
            mlog.id = logid;
            #endregion


            ReturnResult result = new ReturnResult
            {
                IsSuccess = false,
            };
            try
            {
                var responseStr = DoRequest(url, JsonConvert.SerializeObject(requestData));
                if (string.IsNullOrEmpty(responseStr))
                {
                    result.Message = "返回数据为空";
                }
                else
                {
                    PushResponseBody responseBody = (PushResponseBody)JsonConvert.DeserializeObject(responseStr, typeof(PushResponseBody));
                    if (responseBody.code == "200" && responseBody.describe == "success")
                    {
                        result.IsSuccess = true;
                        result.Message = JsonConvert.SerializeObject(responseBody);
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = responseBody.describe;
                    }
                    //记录在日志表
                    mlog.code = responseBody.code;
                    mlog.describe = responseBody.describe;
                }
            }
            catch (Exception ex)
            {
                result.Message = "异常" + ex.Message;
            }

            #region  记录在日志表
            mlog.req_type = "/rhone/lv/order/consume/notice";
            mlog.respstr = result.Message;
            mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            new Meituan_reqlogData().EditReqlog(mlog);
            #endregion

            return result;
        }
        #endregion

        #region  给美团发送通知-已经注释
       
       
        //public ReturnResult ProductAdd(string url, DealResponse product, Meituan_reqlog mlog)
        //{
        //    ReturnResult result = new ReturnResult
        //    {
        //        IsSuccess = false,
        //    };
        //    try
        //    {
        //        var responseStr = DoRequest(url, JsonConvert.SerializeObject(product));
        //        if (string.IsNullOrEmpty(responseStr))
        //        {
        //            result.Message = "返回数据为空";
        //        }
        //        else
        //        {
        //            PushResponseBody responseBody = (PushResponseBody)JsonConvert.DeserializeObject(responseStr, typeof(PushResponseBody)); ;
        //            if (responseBody.code == "200" && responseBody.describe == "success")
        //            {
        //                result.IsSuccess = true;
        //                result.Message = JsonConvert.SerializeObject(responseBody);
        //            }
        //            else
        //            {
        //                result.IsSuccess = false;
        //                result.Message = responseBody.describe;
        //            }

        //            //记录在日志表
        //            mlog.code = responseBody.code;
        //            mlog.describe = responseBody.describe;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Message = "异常" + ex.Message;
        //    }

        //    #region  记录在日志表
        //    mlog.req_type = "/rhone/lv/deal/push";
        //    mlog.respstr = result.Message;
        //    mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //    new Meituan_reqlogData().EditReqlog(mlog);
        //    #endregion

        //    return result;
        //}
        //public ReturnResult PoiAdd(string url, PoiResponse product, Meituan_reqlog mlog)
        //{
        //    ReturnResult result = new ReturnResult
        //    {
        //        IsSuccess = false,
        //    };
        //    try
        //    {
        //        var responseStr = DoRequest(url, JsonConvert.SerializeObject(product));
        //        if (string.IsNullOrEmpty(responseStr))
        //        {
        //            result.Message = "返回数据为空";
        //        }
        //        else
        //        {
        //            PushResponseBody responseBody = (PushResponseBody)JsonConvert.DeserializeObject(responseStr, typeof(PushResponseBody));
        //            if (responseBody.code == "200" && responseBody.describe == "success")
        //            {
        //                result.IsSuccess = true;
        //                result.Message = JsonConvert.SerializeObject(responseBody);
        //            }
        //            else
        //            {
        //                result.IsSuccess = false;
        //                result.Message = responseBody.describe;
        //            }

        //            //记录在日志表
        //            mlog.code = responseBody.code;
        //            mlog.describe = responseBody.describe;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Message = "异常" + ex.Message;
        //    }

        //    #region  记录在日志表
        //    mlog.req_type = "/rhone/lv/poi/push";
        //    mlog.respstr = result.Message;
        //    mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //    new Meituan_reqlogData().EditReqlog(mlog);
        //    #endregion

        //    return result;
        //}
        //public ReturnResult RefundNotify(string url, OrderCancelResponse requestData, Meituan_reqlog mlog)
        //{
        //    ReturnResult result = new ReturnResult
        //    {
        //        IsSuccess = false,
        //    };
        //    try
        //    {
        //        var responseStr = DoRequest(url, JsonConvert.SerializeObject(requestData));
        //        if (string.IsNullOrEmpty(responseStr))
        //        {
        //            result.Message = "返回数据为空";
        //        }
        //        else
        //        {
        //            PushResponseBody responseBody = (PushResponseBody)JsonConvert.DeserializeObject(responseStr, typeof(PushResponseBody));
        //            if (responseBody.code == "200" && responseBody.describe == "success")
        //            {
        //                result.IsSuccess = true;
        //                result.Message = JsonConvert.SerializeObject(responseBody);
        //            }
        //            else
        //            {
        //                result.IsSuccess = false;
        //                result.Message = responseBody.describe;
        //            }
        //            //记录在日志表
        //            mlog.code = responseBody.code;
        //            mlog.describe = responseBody.describe;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Message = "异常" + ex.Message;
        //    }

        //    #region  记录在日志表
        //    mlog.req_type = "/rhone/lv/order/refund/notice";
        //    mlog.respstr = result.Message;
        //    mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //    new Meituan_reqlogData().EditReqlog(mlog);
        //    #endregion

        //    return result;
        //}
        #endregion

        public string DoRequest(string url, string data)
        {
            DateTime date = DateTime.Now;
            return PostResponse(url, data, date, mt_partnerId, mt_client, 200000);
        }
        public string GetSign(string data)
        {
            Encoding encode = Encoding.UTF8;
            byte[] byteData = encode.GetBytes(data);
            byte[] byteKey = encode.GetBytes(mt_secret);
            HMACSHA1 hmac = new HMACSHA1(byteKey);
            CryptoStream cs = new CryptoStream(Stream.Null, hmac, CryptoStreamMode.Write);
            cs.Write(byteData, 0, byteData.Length);
            cs.Close();
            return Convert.ToBase64String(hmac.Hash);
        }
        public static WebHeaderCollection GetHttpRequestHeader(string partnerId, string date, string sign)
        {
            var header = new WebHeaderCollection();
            header.Add("PartnerId", partnerId);
            header.Add("Date", date);
            header.Add("Authorization", sign);
            return header;
        }
        public static string GetCurrentDateStr()
        {
            return DateTime.Now.GetDateTimeFormats('r')[0];
        }
        public string PostResponse(string url, string postData, DateTime date, string partnerId, string client, int Timeout)
        {

            //HttpRequestBase
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Headers.Add("PartnerId", partnerId);
            request.Date = date;
            var indexOf = url.IndexOf("/", 10, StringComparison.Ordinal);
            //测试环境
            string stringToSign = "POST" + " " + url.Substring(indexOf).Replace("rhone-doc", "rhone") + "\n" + request.Headers.Get("Date");
            ////正式环境需要替换为下面内容
            //string stringToSign = "POST" + " " + url.Substring(indexOf) + "\n" + request.Headers.Get("Date");
            string sign = GetSign(stringToSign);
            string authorization = "MWS" + " " + client + ":" + sign;
            request.Headers.Add("Authorization", authorization);
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

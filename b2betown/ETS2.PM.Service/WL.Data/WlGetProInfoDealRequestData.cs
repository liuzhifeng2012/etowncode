using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.WL.Model;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.WL.Data.Internal;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Framework;
using Newtonsoft.Json;
using System.Net;
using System.Security.Cryptography;
using System.IO;

namespace ETS2.PM.Service.WL.Data
{
    public class WlGetProInfoDealRequestData
    { 
        
        public string wl_partnerId;
        public string wl_usekey;
        public string wl_url;


        public WlGetProInfoDealRequestData(string partnerid, string usekey)
        {
            this.wl_partnerId = partnerid;
            this.wl_usekey = usekey;
            this.wl_url = "http://124.65.121.42:12345/";
            //this.wl_partnerId = "2312";
            //this.wl_usekey = "E60C5FA69F937CF81F1A67F738BF5508";
        }


        #region 拉取产品
        public ReturnResult ProductChangedNotify(WlGetProInfoDealRequest requestData, int comid)
        {

            string url = wl_url + "wl.trip.order.get";
            #region  记入日志表
            string reqip = CommonFunc.GetRealIP();
            WL_reqlog mlog = new WL_reqlog
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
                orderid = "0",
                stockagentcompanyid = comid
            };
            int logid = new WL_reqlogData().EditReqlog(mlog);
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

                    mlog.req_type = "wl.trip.order.get";
                    mlog.respstr = responseStr;
                    mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    new WL_reqlogData().EditReqlog(mlog);



                    WlDealResponse responseBody = (WlDealResponse)JsonConvert.DeserializeObject(responseStr, typeof(WlDealResponse));
                    if (responseBody.code == 200)
                    {
                        result.IsSuccess = true;
                        result.Message = JsonConvert.SerializeObject(responseBody);

                        //写入产品数据
                        using (var sql = new SqlHelper())
                        {
                            try
                            {
                                var internalData = new InternalWlGetProInfoDealRequest(sql);

                                //更新返回后先对所有产品线进行下线，对上线的产品进行标注
                                var closewlpro = internalData.UPCloseWlProDeal(comid);

                                ////循环对所有下载的数据依次插入
                                for (int i = 0; i < responseBody.body.Count; i++)
                                {
                                    responseBody.body[i].id = 0;//临时都做为新的记录进行插入
                                    responseBody.body[i].comid = comid;//商户id
                                    responseBody.body[i].state = 1;//商户id
                                    //对已拉取过的产品进行更新而不是插入
                                    var pro_temp=internalData.SelectproidgetWlProDeal(responseBody.body[i].proID, responseBody.body[i].comid);
                                    if (pro_temp != null) {
                                        responseBody.body[i].id = pro_temp.id;
                                    }


                                    int wlpro_id = internalData.InsertorUpdateWlProDeal(responseBody.body[i]);//插入只插入主要数据，后期更新
                                }
                            }
                            catch
                            {
                                throw;
                            }
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = responseBody.describe;
                    }

                    //记录在日志表
                    mlog.code = responseBody.code.ToString();
                    mlog.describe = responseBody.describe;
                }
            }
            catch (Exception ex)
            {
                result.Message = "异常" + ex.Message;
            }

            #region  记录在日志表
            //mlog.req_type = "wl.trip.order.get";
            //mlog.respstr = result.Message;
            //mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            new WL_reqlogData().EditReqlog(mlog);
            #endregion

            return result;
        }
        #endregion

        #region 拉取产品json代码
        public WlGetProInfoDealRequest productchangedNotify_json(int s1)
        {
            string prochangetxt = "{" +
                             "\"partnerId\":"+s1+"," +
                             "\"body\": {" +
                                 "\"method\":\"multi\"," +
                                 "\"currentPage\":1," +
                                  "\"pageSize\": 20" +
                               "}" +
                           "}";
            return  (WlGetProInfoDealRequest)JsonConvert.DeserializeObject(prochangetxt, typeof(WlGetProInfoDealRequest));
        }
        #endregion


        #region 创建订单
        public ReturnResult wlOrderCreateRequest_data(wlOrderCreateRequest requestData, int comid)
        {

            string url = wl_url+"wl.trip.order.create";
            #region  记入日志表
            string reqip = CommonFunc.GetRealIP();
            WL_reqlog mlog = new WL_reqlog
            {
                id = 0,
                reqstr = JsonConvert.SerializeObject(requestData),
                subtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                respstr = "",
                resptime = "",
                code = "",
                describe = "",
                req_type = "",
                orderid = requestData.body.partnerOrderId,
                sendip = reqip,
                stockagentcompanyid = comid
            };
            int logid = new WL_reqlogData().EditReqlog(mlog);
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

                    mlog.req_type = "wl.trip.order.create";
                    mlog.respstr = responseStr;
                    mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    new WL_reqlogData().EditReqlog(mlog);

                    wlOrderCreateResponse WlDealResponse = (wlOrderCreateResponse)JsonConvert.DeserializeObject(responseStr, typeof(wlOrderCreateResponse));
                    if (WlDealResponse.code == 200 )
                    {
                        result.IsSuccess = true;
                        result.Message = JsonConvert.SerializeObject(WlDealResponse);

                        //创建订单
                        string wlid = WlDealResponse.body.wlOrderId;
                        string orderid = requestData.body.partnerOrderId;

                        requestData.body.comid = comid;

                        using (var sql = new SqlHelper())
                        {
                            try
                            {
                                var internalData = new InternalWlGetProInfoDealRequest(sql);
                                int wl_id = internalData.InsertorUpdateWlOrderCreate(requestData,WlDealResponse);//一次性插入万龙id

                            }
                            catch
                            {
                                throw;
                            }
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = WlDealResponse.describe;
                    }

                    //记录在日志表
                    mlog.code = WlDealResponse.code.ToString();
                    mlog.describe = WlDealResponse.describe;
                    mlog.mtorderid = WlDealResponse.body.wlOrderId;
                }
            }
            catch (Exception ex)
            {
                result.Message = "异常" + ex.Message;
            }

            #region  记录在日志表
            //mlog.req_type = "wl.trip.order.get";
            //mlog.respstr = result.Message;
            //mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            new WL_reqlogData().EditReqlog(mlog);
            #endregion

            return result;
        }
        #endregion

        #region 创建订单 /
        public wlOrderCreateRequest wlOrderCreateRequest_json(int partnerId, string name, string mobile, string partnerOrderId, string partnerDealId, string wlDealId, double buyPrice, double unitPrice, double totalPrice, int quantity, string travelDate)
        {
    
            //创建订单
            string prochangetxt = "{" +
                         "\"partnerId\":" + partnerId + "," +
                         "\"body\": {" +
                              "\"contactPerson\": {" +
                                    "\"name\":\"" + name + "\"," +
                                    "\"mobile\":\"" + mobile + "\"" +
                               "}," +
                               "\"visitors\": [{" +
                                    "\"name\":\"" + name + "\"," +
                                    "\"mobile\":\"" + mobile + "\"" +
                               "}]," +
                               "\"partnerOrderId\":\"" + partnerOrderId + "\"," +
                               "\"partnerDealId\":\"" + partnerDealId + "\"," +
                               "\"wlDealId\":\"" + wlDealId + "\"," +
                               "\"buyPrice\":" + buyPrice + "," +
                               "\"unitPrice\":" + unitPrice + " ," +
                               "\"totalPrice\":" + totalPrice + " ," +
                               "\"quantity\":" + quantity + " ," +
                               "\"travelDate\": \"" + travelDate + "\"" +
                           "}" +
                       "}";

            return (wlOrderCreateRequest)JsonConvert.DeserializeObject(prochangetxt, typeof(wlOrderCreateRequest));
        }
        #endregion

        #region 支付通知
        public ReturnResult wlOrderPayRequest_data(wlOrderPayRequest requestData, int comid)
        {

            string url = wl_url+"wl.trip.order.pay";
            #region  记入日志表
            string reqip = CommonFunc.GetRealIP();
            WL_reqlog mlog = new WL_reqlog
            {
                id = 0,
                reqstr = JsonConvert.SerializeObject(requestData),
                subtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                respstr = "",
                resptime = "",
                code = "",
                describe = "",
                req_type = "",
                orderid = requestData.body.partnerOrderId,
                sendip = reqip,
                stockagentcompanyid = comid
            };
            int logid = new WL_reqlogData().EditReqlog(mlog);
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

                    mlog.req_type = "wl.trip.order.pay";
                    mlog.respstr = responseStr;
                    mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    new WL_reqlogData().EditReqlog(mlog);

                    wlOrderPayResponse wlOrderPayResponse = (wlOrderPayResponse)JsonConvert.DeserializeObject(responseStr, typeof(wlOrderPayResponse));
                    if (wlOrderPayResponse.code == 200)
                    {
                        result.IsSuccess = true;
                        result.Message = "支付成功";

                        //完成支付
                        using (var sql = new SqlHelper())
                        {
                            try
                            {
                                var internalData = new InternalWlGetProInfoDealRequest(sql);
                                int wl_id = internalData.UpdateWlOrderPaySC(wlOrderPayResponse);//一次性插入万龙id
                            }
                            catch
                            {
                                throw;
                            }
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = wlOrderPayResponse.describe;
                    }

                    //记录在日志表
                    mlog.code = wlOrderPayResponse.code.ToString();
                    mlog.describe = wlOrderPayResponse.describe;
                    mlog.mtorderid = wlOrderPayResponse.body.wlOrderId;
                }
            }
            catch (Exception ex)
            {
                result.Message = "异常" + ex.Message;
            }

            #region  记录在日志表
            //mlog.req_type = "wl.trip.order.get";
            //mlog.respstr = result.Message;
            //mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            new WL_reqlogData().EditReqlog(mlog);
            #endregion

            return result;
        }
        #endregion

         #region 支付通知json
        public wlOrderPayRequest wlOrderPayRequest_json(int partnerId, string partnerOrderId, string wlOrderId)
        {
            //支付通知
            string prochangetxt = "{" +
                         "\"partnerId\":" + partnerId + "," +
                         "\"body\": {" +
                               "\"partnerOrderId\":\"" + partnerOrderId + "\"," +
                               "\"wlOrderId\":\"" + wlOrderId + "\"" +

                           "}" +
                       "}";
            return (wlOrderPayRequest)JsonConvert.DeserializeObject(prochangetxt, typeof(wlOrderPayRequest));
        }
         #endregion

        #region 核销通知
        public string wlhexiaotongzhi_json(int code, string describe)
        {
            //支付通知
            string prochangetxt = "{" +
                         "\"code\":" + code + "," +
                         "\"describe\":\"" + describe + "\"" +
                       "}";
            return prochangetxt;
            //return (wlhexiaoreturn)JsonConvert.DeserializeObject(prochangetxt, typeof(wlhexiaoreturn));
        }
        #endregion


        #region 退款 orderStatus= 1为创建订单成功 2为创建订单失败 4为支付成功 5为支付中 不能明确支付状态时使用该状态 6为支付失败
        public ReturnResult wlOrderRefundRequest_data(wlOrderRefundRequest requestData, int comid, int orderStatus)
        {

            string url = wl_url+"wl.trip.order.refund";
            #region  记入日志表
            string reqip = CommonFunc.GetRealIP();
            WL_reqlog mlog = new WL_reqlog
            {
                id = 0,
                reqstr = JsonConvert.SerializeObject(requestData),
                subtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                respstr = "",
                resptime = "",
                code = "",
                describe = "",
                req_type = "",
                orderid = requestData.body.partnerOrderId,
                sendip = reqip,
                stockagentcompanyid = comid
            };
            int logid = new WL_reqlogData().EditReqlog(mlog);
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

                    mlog.req_type = "wl.trip.order.refund";
                    mlog.respstr = responseStr;
                    mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    new WL_reqlogData().EditReqlog(mlog);

                    wlOrderRefundResponse wlOrderRefundResponse = (wlOrderRefundResponse)JsonConvert.DeserializeObject(responseStr, typeof(wlOrderRefundResponse));
                    if (wlOrderRefundResponse.code == 200)
                    {
                        result.IsSuccess = true;
                        result.Message = "退款返回";

                        //完成支付
                        using (var sql = new SqlHelper())
                        {
                            try
                            {
                                var internalData = new InternalWlGetProInfoDealRequest(sql);
                                int wl_id = internalData.UpdateWlOrderBack(wlOrderRefundResponse, orderStatus, requestData);//一次性插入万龙id
                            }
                            catch
                            {
                                throw;
                            }
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = wlOrderRefundResponse.describe;
                    }

                    //记录在日志表
                    mlog.code = wlOrderRefundResponse.code.ToString();
                    mlog.describe = wlOrderRefundResponse.describe;
                    mlog.mtorderid = wlOrderRefundResponse.body.wlOrderId;
                }
            }
            catch (Exception ex)
            {
                result.Message = "异常" + ex.Message;
            }

            #region  记录在日志表
            //mlog.req_type = "wl.trip.order.get";
            //mlog.respstr = result.Message;
            //mlog.resptime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            new WL_reqlogData().EditReqlog(mlog);
            #endregion

            return result;
        }
        #endregion

        #region 退款json
        public wlOrderRefundRequest wlOrderRefundRequest_json(int partnerId, string wlOrderId, string wlDealId, string partnerOrderId, string partnerDealId, string voucherList, int refundQuantity, double unitPrice, double refundPrice, double refundFee)
        {
            //退款
            string prochangetxt = "{" +
                         "\"partnerId\":" + partnerId + "," +
                         "\"body\": {" +
                                "\"wlOrderId\":\"" + wlOrderId + "\"," +
                                "\"wlDealId\":\"" + wlDealId + "\"," +
                                "\"partnerRefundId\":\"" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "\"," +//退款流水号
                                "\"partnerOrderId\":\"" + partnerOrderId + "\"," +
                                "\"partnerDealId\":\"" + partnerDealId + "\"," +
                                "\"voucherList\":[\"" + voucherList + "\"]," +
                                "\"refundQuantity\":" + refundQuantity + "," +
                                "\"unitPrice\":" + unitPrice + "," +
                                "\"refundPrice\":" + refundPrice + "," +
                                "\"refundFee\":" + refundFee + "," +
                               "\"refundTime\":\"" + DateTime.Now.ToString("yyyy-MM-dd- HH:mm:ss") + "\"" +
                           "}" +
                       "}";
            return (wlOrderRefundRequest)JsonConvert.DeserializeObject(prochangetxt, typeof(wlOrderRefundRequest));
        }
        #endregion

        #region 查询单笔订单
        public wl_order_model SearchWlOrderData(int comid, int id, string wlorderid, int orderid)
        {

            using (var helper = new SqlHelper())
            {
                var r = new InternalWlGetProInfoDealRequest(helper).SearchWlOrder( comid,  id,  wlorderid,  orderid);
                return r;
            }
        }
        #endregion


        #region 核销修改数据
        public int UpdateWlOrderPaySC(wl_order_model m)
        {

            using (var helper = new SqlHelper())
            {
                var r = new InternalWlGetProInfoDealRequest(helper).UpdateWlOrderPaySC(m);
                return r;
            }
        }
        #endregion



        #region 核销日志数据
        public int InsertWlUseLog(int comid, string wlorderid, int usedQuantity=0, int partnerId=0, int quantity=0,int orderid=0,int proid=0)
        {

            using (var helper = new SqlHelper())
            {
                var r = new InternalWlGetProInfoDealRequest(helper).InsertWlUseLog(comid,  wlorderid,  usedQuantity,  partnerId,  quantity,orderid,proid);
                return r;
            }
        }
        #endregion


        #region 查询日志
        public List<wl_use_log_model> InterfaceUsePageList(int comid, int pageindex, int pagesize, out int totalcount, string key, string startime, string endtime)
        {

            using (var helper = new SqlHelper())
            {
                List<wl_use_log_model> r = new InternalWlGetProInfoDealRequest(helper).InterfaceUsePageList(comid, pageindex, pagesize, out totalcount, key, startime, endtime);
                return r;
            }
        }
        #endregion



        #region 查询所有产品
        public List<WlDealResponseBody> SelectallgetWlProDealData(string proID, int comid, out int totalcount)
        {

            using (var helper = new SqlHelper())
            {
                List<WlDealResponseBody> r = new InternalWlGetProInfoDealRequest(helper).SelectallgetWlProDeal(proID, comid, out totalcount);
                return r;
            }
        }
        #endregion

        #region 查询单个产品
        public WlDealResponseBody SelectonegetWlProDealData(string wlDealId,int comid)
        {

            using (var helper = new SqlHelper())
            {
                WlDealResponseBody r = new InternalWlGetProInfoDealRequest(helper).SelectonegetWlProDeal(wlDealId,comid);
                return r;
            }
        }
        #endregion

        #region 为核销单独设定值用过wl订单id查询订单
        public wl_order_model getWlOrderidData(string wlorderId)
        {

            using (var helper = new SqlHelper())
            {
                wl_order_model r = new InternalWlGetProInfoDealRequest(helper).getWlOrderidData(wlorderId);
                return r;
            }
        }
        #endregion

        public string DoRequest(string url, string data)
        {
            DateTime date = DateTime.Now;
            return PostResponse(url, data, date, wl_partnerId, wl_usekey, 200000);
        }



        public List<WlDealResponseBody> WLProPageList(string comid, int pageindex, int pagesize, int prostate, out int totalcount, string key = "")
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalWlGetProInfoDealRequest(helper).WLProPageList(comid, pageindex, pagesize, prostate, out totalcount,  key);

                return list;
            }
        }



        #region 万龙签名
        public static string GetSigntest(string text, string key){
           Encoding encode = Encoding.GetEncoding("utf-8");
           byte[] byteData = encode.GetBytes(text);
           byte[] byteKey = encode.GetBytes(key);
           HMACSHA1 hmac = new HMACSHA1(byteKey);
           CryptoStream cs = new CryptoStream(Stream.Null, hmac, CryptoStreamMode.Write);
           cs.Write(byteData, 0, byteData.Length);
           cs.Close();
           string ret = Convert.ToBase64String(hmac.Hash);
          return ret;
        }
        #endregion



        public string PostResponse(string url, string postData, DateTime date, string partnerId, string usekey, int Timeout)
        {

            //HttpRequestBase
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Headers.Add("partnerId", partnerId);
            request.Date = date;
            var indexOf = url.IndexOf("/", 10, StringComparison.Ordinal);
            //测试环境
            string stringToSign = "POST" + " " + url.Substring(indexOf).Replace("rhone-doc", "rhone") + "\n" + request.Headers.Get("Date");
            ////正式环境需要替换为下面内容
            //string stringToSign = "POST" + " " + url.Substring(indexOf) + "\n" + request.Headers.Get("Date");
            string sign = GetSigntest(stringToSign, usekey);
            string authorization = "MWS" + " " + partnerId + ":" + sign;
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

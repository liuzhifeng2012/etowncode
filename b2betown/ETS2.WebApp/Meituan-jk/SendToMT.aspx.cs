using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.PM.Service.PMService.Modle.Enum;
using Newtonsoft.Json;
using ETS.Framework;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.PM.Service.WL.Data;
using ETS2.PM.Service.WL.Model;
using ETS.JsonFactory;
using ETS2.PM.Service.PMService.Modle;
using ETS2.VAS.Service.VASService.Data;
using System.Net;
using System.IO;
using System.Text;
using ETS.Data.SqlHelper;
using System.Threading;
using ETS2.PM.Service.Meituan.Model;

namespace ETS2.WebApp.Meituan_jk
{
    /// <summary>
    /// 弃用--直接在产品上下架 和 后台验证电子码 操作就可以发送通知
    /// </summary>
    public partial class SendToMT : System.Web.UI.Page
    {
        private Agent_company agentinfo;

        private SqlHelper sqlHelper;

        protected void Page_Load(object sender, EventArgs e)
        {

          
            ////-1 需要替换为测试分销商id
            //agentinfo = new AgentCompanyData().GetAgentCompany(-1);

            //拉取产品
            //string prochangetxt = "{" +
            //             "\"partnerId\":2312," +
            //             "\"body\": {" +
            //                 "\"method\":\"multi\"," +
            //                 "\"currentPage\":1," +
            //                  "\"pageSize\": 20" +
            //               "}" +
            //           "}";

            //创建订单
            //string prochangetxt = "{" +
            //             "\"partnerId\":2312," +
            //             "\"body\": {" +
            //                  "\"contactPerson\": {" +
            //                        "\"name\":\"李星海\"," +
            //                        "\"mobile\":\"13488761102\"" +
            //                   "}," +
            //                   "\"visitors\": [{" +
            //                        "\"name\":\"李星海\"," +
            //                        "\"mobile\":\"13488761102\"" +
            //                   "}]," +
            //                   "\"partnerOrderId\":\"90000013\"," +
            //                   "\"partnerDealId\":\"50000001\"," +
            //                   "\"wlDealId\":\"5bed3fcc73c37c31f8d8909c\"," +
            //                   "\"buyPrice\":0.01," +
            //                   "\"unitPrice\":1 ," +
            //                   "\"totalPrice\":2 ," +
            //                   "\"quantity\":2 ," +
            //                   "\"travelDate\": \"2018-12-07\"" +
            //               "}" +
            //           "}";
            //string prochangetxt = "{\"contacts\":{\"idNum\":\"441400198002244166\",\"idType\":\"ID_CARD\",\"mobile\":\"13764517990\",\"name\":\"测试下单\"},\"num\":\"1\",\"password\":\"a6b50913\",\"serialNo\":\"2018122592819013\",\"settlePrice\":\"0.01\",\"sign\":\"3fe30eee43a6f04d62be36dc69d7a28b\",\"supplierGoodsId\":\"12811\",\"timestamp\":\"20181225135505\",\"travellerList\":[{\"idNum\":\"441400198002244166\",\"idType\":\"ID_CARD\",\"mobile\":\"13764517990\",\"name\":\"测试下单\"}],\"uid\":\"lvmama8527\",\"visitTime\":\"20181226000000\"}";
            //TextBox1.Text = prochangetxt;


            //支付通知
           // string prochangetxt = "{" +
           //              "\"partnerId\":2312," +
           //              "\"body\": {" +
           //                    "\"partnerOrderId\":\"90000013\"," +
           //                    "\"wlOrderId\":\"15441535059641023\"" +

           //                "}" +
           //            "}";
           //var t= wlOrderPayRequest_data();
            //退款
            //string prochangetxt = "{" +
            //             "\"partnerId\":2312," +
            //             "\"body\": {" +
            //                    "\"wlOrderId\":\"15441535059641023\"," +
            //                    "\"wlDealId\":\"5bed3fcc73c37c31f8d8909c\"," +
            //                    "\"partnerRefundId\":\"" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "\"," +//退款流水号
            //                    "\"partnerOrderId\":\"90000012\"," +
            //                    "\"partnerDealId\":\"50000001\"," +
            //                    "\"voucherList\":[\"15441535059641023\"]," +
            //                    "\"refundQuantity\":2," +
            //                    "\"unitPrice\":0.01," +
            //                    "\"refundPrice\":0.02," +
            //                    "\"refundFee\":0.00," +
            //                   "\"refundTime\":\"" + DateTime.Now.ToString("yyyy-MM-dd- HH:mm:ss") + "\"" +
            //               "}" +
            //           "}";

            //TextBox1.Text = prochangetxt;


            ////核销




            string prochangetxt = "{" +
                         "\"partnerId\":2312," +
                         "\"body\": {" +
                                "\"wlOrderId\":\"15464225396069730\"," +
                                "\"usedQuantity\":1," +
                                "\"quantity\":1," +
                                "\"refundedQuantity\":0," +
                           "}" +
                       "}";

            TextBox1.Text = prochangetxt;


            //string consumetext = "{" +
            //              "\"code\": 200," +
            //              "\"describe\": \"消费通知\"," +
            //              "\"partnerId\":" + agentinfo.mt_partnerId + "," +
            //              "\"body\": {" +
            //                "\"bookOrderId\": \"美团订单号\"," +
            //                "\"partnerOrderId\": \"易城订单号\"," +
            //                "\"orderStatus\": 0," +
            //                "\"orderQuantity\": 0," +
            //                "\"usedQuantity\": 0," +
            //                "\"refundQuantity\": 0," +
            //                "\"isConsumed\": false," +//凭证码是否消费	
            //                "\"voucher\": \"string\"" +//订单凭证码
            //              "}" +
            //            "}";
            //string url = "http://test.wlski.net/wl.trip.order.create";
            //var indexOf = url.IndexOf("/", 10, StringComparison.Ordinal);
            ////string stringToSign = "POST" + " " + url.Substring(indexOf).Replace("rhone-doc", "rhone").Replace("/", "") + "\n" + request.Headers.Get("Date");
            //////正式环境需要替换为下面内容
            ////string stringToSign = "POST" + " " + url.Substring(indexOf) + "\n" + request.Headers.Get("Date");
            //string usekey = "E60C5FA69F937CF81F1A67F738BF5508";
            //string stringToSign = "POST wl.trip.order.get \nWed, 06 May 2015 10:34:20 GMT";
            //string sign = WlGetProInfoDealRequestData.GetSign(stringToSign, usekey);

            
                        B2b_order order = new B2b_order()
                        {
                            ignoredabatime = 0,
                            M_b2b_order_hotel = null,
                            Id = 0,
                            Pro_id = 12809,
                            Speciid = 0,
                            Order_type = 1,
                            U_name = "李星海",
                            U_id = 0,
                            U_phone = "13488761102",
                            U_idcard="",
                            U_num = 1,
                            U_subdate = DateTime.Now,
                            Payid = 0,
                            Pay_price =1,
                            Cost = 1,
                            Profit = 0,
                            Order_state = (int)OrderStatus.WaitPay,//
                            Pay_state = (int)PayStatus.NotPay,//支付模块未做出来前先设置为已支付
                            Send_state = (int)SendCodeStatus.NotSend,
                            Ticketcode = 0,//电子码未创建，支付后产生码赋值
                            Rebate = 0,//  利润返佣金额暂时定为0
                            Ordercome = "",//订购来源 暂时定为空
                            U_traveldate = DateTime.Now,
                            Dealer = "自动",
                            Comid = 112,
                            Pno = "",
                            Openid = "",
                            Ticketinfo = "",

                            Integral1 = 0,//积分
                            Imprest1 = 0,//预付款
                            Agentid = 0,     //分销ID
                            Warrantid = 0,   //授权ID
                            Warrant_type = 1,  //支付类型分销 1出票扣款 2验码扣款
                            Buyuid = 0,
                            Tocomid = 0,
                            pickuppoint = "",
                            dropoffpoint = "",
                            Order_remark = "",
                            Deliverytype = 0,
                            Province = "",
                            City = "",
                            Address = "",
                            Code = "",
                            channelcoachid = 0,
                            //baoxiannames = baoxiannames,
                            //baoxianpinyinnames = baoxianpinyinnames,
                            //baoxianidcards = baoxianidcards,
                            //autosuccess = autosuccess,
                            //submanagename = submanagename,
                            //yuyuepno = yuyuepno,
                            //travelnames = travelnames,
                            //travelphones = travelphones,
                            //serverid = sid,
                            //payserverpno = payserverpno,

                            travelidcards = "",
                            travelnations = "",
                            travelremarks = ""
                        };

                        int orderid = 0;
                        //var   data = OrderJsonData.InsertOrUpdate(order, out orderid);
                        //var PayReturnSendEticket = new PayReturnSendEticketData();
                        //var s = PayReturnSendEticket.chulidingdan(298283, 1);
                       


            //string consumetext = sign;
                       TextBox2.Text = "";
        }


        #region 支付通知
        public ReturnResult wlOrderPayRequest_data()
        {

           // string url = "http://localhost:14395/wljk/notice.ashx";

            //B2b_company commanage = B2bCompanyData.GetAllComMsg(112);
            //WlGetProInfoDealRequestData wldata = new WlGetProInfoDealRequestData(commanage.B2bcompanyinfo.wl_PartnerId, commanage.B2bcompanyinfo.wl_userkey);
            //var createwlorder = wldata.wlOrderPayRequest_json(11, "22", "33");//

            //支付通知
            //string prochangetxt = "{" +
            //             "\"partnerId\":2132," +
            //             "\"body\": {" +
            //                   "\"wlOrderId\":\"15442393747832888\"," +
            //                   "\"usedQuantity\":1," +
            //                   "\"quantity\":1," +
            //                   "\"refundedQuantity \":0" +
            //               "}" +
            //           "}";
            //var createwlorder = (wlhexiao)JsonConvert.DeserializeObject(prochangetxt, typeof(wlhexiao));



            //var responseStr = wldata.DoRequest(url, JsonConvert.SerializeObject(createwlorder));
            //string action = "1";
            return null;
        }
        #endregion



        //产品变化通知
        protected void Button1_Click(object sender, EventArgs e)
        {
            //deal变化通知接口    
            //DealChangeNotice mrequest = (DealChangeNotice)JsonConvert.DeserializeObject(TextBox1.Text.Trim(), typeof(DealChangeNotice));
            //ReturnResult r = new MeiTuanInter(agentinfo.mt_partnerId,agentinfo.mt_secret,agentinfo.mt_client).ProductChangedNotify(mrequest,agentinfo.Id);

            //拉取产品
            //WlGetProInfoDealRequest mrequest = (WlGetProInfoDealRequest)JsonConvert.DeserializeObject(TextBox1.Text.Trim(), typeof(WlGetProInfoDealRequest));
            //var r = new WlGetProInfoDealRequestData("2312", "E60C5FA69F937CF81F1A67F738BF5508").ProductChangedNotify(mrequest, 112);


            //创建订单
            //wlOrderCreateRequest mrequest = (wlOrderCreateRequest)JsonConvert.DeserializeObject(TextBox1.Text.Trim(), typeof(wlOrderCreateRequest));
            //var r = new WlGetProInfoDealRequestData("2312", "E60C5FA69F937CF81F1A67F738BF5508").wlOrderCreateRequest_data(mrequest, 112);

            //支付
            //wlOrderPayRequest mrequest = (wlOrderPayRequest)JsonConvert.DeserializeObject(TextBox1.Text.Trim(), typeof(wlOrderPayRequest));
            //var r = new WlGetProInfoDealRequestData("2312", "E60C5FA69F937CF81F1A67F738BF5508").wlOrderPayRequest_data(mrequest, 112);

            //美团订单退款
            //string s = "{\"partnerId\":13857,\"body\":{\"orderId\":267462905905413,\"refundId\":\"267462905905413-20181226090557024\",\"partnerOrderId\":\"305566\",\"partnerDealId\":\"12885\",\"voucherList\":[\"15456624941871613\"],\"refundQuantity\":2,\"unitPrice\":290.0,\"refundPrice\":580.0,\"refundFee\":0.0,\"refundTime\":\"2018-12-26 09:05:57\"}}";
            //OrderCancelRequest mrequest = (OrderCancelRequest)JsonConvert.DeserializeObject(s, typeof(OrderCancelRequest));
            //var r = DoRequest("http://localhost:14395/Meituan-jk/orderrefund.aspx", JsonConvert.SerializeObject(mrequest));
            //Label1.Text = r;
            
            //美团查询订单
            //string s = "{\"partnerId\":13857,\"body\":{\"orderId\":269402503946483,\"partnerOrderId\":\"314418\"}}";
            //OrderQueryRequest mrequest = (OrderQueryRequest)JsonConvert.DeserializeObject(s, typeof(OrderQueryRequest));
            //var r = DoRequest("http://localhost:14395/Meituan-jk/orderquery.aspx", JsonConvert.SerializeObject(mrequest));
            //Label1.Text =  r;


            //美团补核销记录


            //string sql = @"select vouchers,usedQuantity,quantity from wl_OrderCreate as a left join b2b_order as b on a.orderid=b.id where b.agentid=6714 and usedQuantity>0 ";
            //int i = 1;
            //using (var helper = new SqlHelper())
            //{

            //    var cmd = helper.PrepareTextSqlCommand(sql);
            //    using (var reader = cmd.ExecuteReader())
            //    {

            //        while (reader.Read())
            //        {
            //            string prochangetxt = "{" +
            //                         "\"partnerId\":2312," +
            //                         "\"body\": {" +
            //                                "\"wlOrderId\":\"" + reader.GetValue<string>("vouchers") + "\"," +
            //                                "\"usedQuantity\":" + reader.GetValue<int>("usedQuantity") + "," +
            //                                "\"quantity\":" + reader.GetValue<int>("quantity") + "," +
            //                                "\"refundedQuantity\":0," +
            //                           "}" +
            //                       "}";
                        
            //            //wlhexiao mrequest = (wlhexiao)JsonConvert.DeserializeObject(prochangetxt, typeof(wlhexiao));
            //            //Thread.Sleep(2 * 1000);
            //            //var r = DoRequest("http://localhost:14395/wljk/notice.ashx", JsonConvert.SerializeObject(mrequest));
            //            //Label1.Text =  "第"+i+"个"+reader.GetValue<string>("vouchers") +r;

            //            i = i + 1;
            //        }

            //    }
            //}


            //批量补发wl核销
//            string[] numbers = new string[] { "15474756618389245",
//"15475022144385829",
//}; //不定长
//            int j=0;
//            foreach (string i in numbers)
//            {
//                // Console.WriteLine(i.ToString() + "\r");

//                j = j + 1;
//                string prochangetxt = "{\"partnerId\":2312,\"body\":{\"wlOrderId\":" + i + ",\"quantity\":1,\"usedQuantity\":1,\"refundedQuantity\":0}}";

//                wlhexiao mrequest1 = (wlhexiao)JsonConvert.DeserializeObject(prochangetxt, typeof(wlhexiao));
//                Thread.Sleep(2 * 1000);
//                var r = DoRequest("http://localhost:14395/wljk/notice.ashx", JsonConvert.SerializeObject(mrequest1));
//                Label1.Text += j + ":" + i + r + "r,";
//            }


        }

        string wl_partnerId = "";
        string wl_usekey = "";
        public string DoRequest(string url, string data)
        {
            DateTime date = DateTime.Now;
            return PostResponse(url, data, date, wl_partnerId, wl_usekey, 200000);
        }






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
            //string sign = GetSigntest(stringToSign, usekey);
            //string authorization = "MWS" + " " + partnerId + ":" + sign;
            //request.Headers.Add("Authorization", authorization);
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
        //消费通知
        protected void Button2_Click(object sender, EventArgs e)
        {
            //消费通知接口

            //OrderConsumeNotice mrequest = (OrderConsumeNotice)JsonConvert.DeserializeObject(TextBox2.Text.Trim(), typeof(OrderConsumeNotice));

            //ReturnResult r = new MeiTuanInter(agentinfo.mt_partnerId, agentinfo.mt_secret, agentinfo.mt_client).ConsumeNotify(mrequest,agentinfo.Id);

            //Label1.Text = r.Message;

        }
        //protected void LinkButton1_Click(object sender, EventArgs e)
        //{
        //    t_caozuo.Text = "http://lvyou.meituan.com/rhone/lv/deal/change/notice";

        //    string s = "{" +
        //                  "\"code\": 200," +
        //                  "\"describe\": \"string\"," +
        //                  "\"partnerId\":" +new MeiTuanInter().mt_partnerId + "," +
        //                  "\"body\": [" +
        //                    "{" +
        //                      "\"partnerDealId\": \"2503\"," +
        //                      "\"status\": " + (int)Meituan_ProStatus.Shangjia + "" +
        //                    "}" +
        //                  "]" +
        //                "}";
        //    t_xml.Text = s;
        //}
        //protected void LinkButton2_Click(object sender, EventArgs e)
        //{
        //    t_caozuo.Text = "http://lvyou.meituan.com/rhone/lv/deal/push";

        //    var response = new DealResponse();
        //    List<string> productIdList = new List<string> { "2503" };

        //    response.code = 200;
        //    response.describe = "success";
        //    response.partnerId = int.Parse(new MeiTuanInter().mt_partnerId);
        //    response.totalSize = productIdList.Count;
        //    //response.body = new List<DealResponseBody>();


        //    #region 获取产品列表
        //    Agent_company agentcompany = new AgentCompanyData().GetAgentCompanyByName("美团旅游");

        //    int totalcount = 0;
        //    List<DealResponseBody> list = new DealResponseData().GetDealResponseBody(out totalcount,agentcompany.Id, "multi", null, productIdList, new MeiTuanInter().mt_partnerId, new MeiTuanInter().mt_client);
        //    foreach (DealResponseBody rbody in list)
        //    {
        //        var imgurl = FileSerivce.GetImgUrl(int.Parse(rbody.pics.urls[0]));

        //        rbody.pics.urls[0] = imgurl;
        //    }
        //    response.body = list;
        //    #endregion
        //    string json = JsonConvert.SerializeObject(response);
        //    t_xml.Text = json;
        //}
        //protected void LinkButton3_Click(object sender, EventArgs e)
        //{
        //    t_caozuo.Text = "http://lvyou.meituan.com/rhone/lv/poi/push";

        //    var response = new PoiResponse();
        //    List<string> poiIdList = new List<string> { "371" };

        //    response.code = 200;
        //    response.describe = "success";
        //    response.partnerId = int.Parse(new MeiTuanInter().mt_partnerId);
        //    response.totalSize = poiIdList.Count;
        //    //response.body = new List<DealResponseBody>();


        //    #region 获取产品列表
        //    int totalcount = 0;
        //    List<PoiResponseBody> list = new PoiResponseData().GetPoiResponseBody(out totalcount, "multi", poiIdList, new MeiTuanInter().mt_partnerId, new MeiTuanInter().mt_client);
        //    foreach (PoiResponseBody rbody in list)
        //    {
        //        var imgurl = FileSerivce.GetImgUrl(int.Parse(rbody.img.urls[0]));

        //        rbody.img.urls[0] = imgurl;
        //    }
        //    response.body = list;
        //    #endregion

        //    string json = JsonConvert.SerializeObject(response);
        //    t_xml.Text = json;
        //}
        //protected void LinkButton4_Click(object sender, EventArgs e)
        //{
        //    t_caozuo.Text = "http://lvyou.meituan.com/rhone/lv/order/consume/notice";

        //    string s = "{" +
        //                  "\"code\": 200," +
        //                  "\"describe\": \"string\"," +
        //                  "\"partnerId\": " + new MeiTuanInter().mt_partnerId + "," +
        //                  "\"body\": {" +
        //                    "\"bookOrderId\": \"美团订单号\"," +
        //                    "\"partnerOrderId\": \"易城订单号\"," +
        //                    "\"orderStatus\": 0," +
        //                    "\"orderQuantity\": 0," +
        //                    "\"usedQuantity\": 0," +
        //                    "\"refundQuantity\": 0," +
        //                    "\"isConsumed\": false," +//凭证码是否消费	
        //                    "\"voucher\": \"string\"" +//订单凭证码
        //                  "}" +
        //                "}";
        //    t_xml.Text = s;
        //}
        //protected void LinkButton5_Click(object sender, EventArgs e)
        //{
        //    t_caozuo.Text = "http://lvyou.meituan.com/rhone/lv/order/refund/notice";

        //    string s = "{" +
        //                  "\"code\": 0," +
        //                  "\"describe\": \"string\"," +
        //                  "\"partnerId\": 0," +
        //                  "\"body\": {" +
        //                    "\"bookOrderId\": \"string\"," +
        //                    "\"partnerOrderId\": \"string\"," +
        //                    "\"orderStatus\": 0," +
        //                    "\"autoRefund\": 0," +
        //                    "\"refundDes\": \"string\"," +
        //                    "\"refundType\": 0," +
        //                    "\"refundPrice\": 0," +
        //                    "\"refundRefactorage\": 0" +
        //                  "}" +
        //                "}";
        //    t_xml.Text = s;
        //}


        ////确定
        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //if (t_xml.Text.Trim() == "")
        //{
        //    Label1.Text = "请填写发送内容";
        //    return;
        //}
        //string url = t_caozuo.Text.Trim();
        //string content = t_xml.Text.Trim();

        //#region  记入日志表
        //string reqip = CommonFunc.GetRealIP();
        //Meituan_reqlog mlog = new Meituan_reqlog
        //{
        //    id = 0,
        //    reqstr = content,
        //    subtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        //    respstr = "",
        //    resptime = "",
        //    code = "",
        //    describe = "",
        //    req_type = "",
        //    sendip = reqip
        //};
        //int logid = new Meituan_reqlogData().EditReqlog(mlog);
        //mlog.id = logid;
        //#endregion

        ////产品推送接口
        //if (url == "http://lvyou.meituan.com/rhone/lv/deal/push")
        //{
        //    DealResponse mrequest = (DealResponse)JsonConvert.DeserializeObject(content, typeof(DealResponse));

        //    ReturnResult r = new MeiTuanInter().ProductAdd(url, mrequest, mlog);

        //    Label1.Text = r.Message;
        //}
        ////poi推送接口
        //if (url == "http://lvyou.meituan.com/rhone/lv/poi/push")
        //{
        //    PoiResponse mrequest = (PoiResponse)JsonConvert.DeserializeObject(content, typeof(PoiResponse));

        //    ReturnResult r = new MeiTuanInter().PoiAdd(url, mrequest, mlog);

        //    Label1.Text = r.Message;
        //}
        ////deal变化通知接口    
        //if (url == "http://lvyou.meituan.com/rhone/lv/deal/change/notice")
        //{
        //    DealChangeNotice mrequest = (DealChangeNotice)JsonConvert.DeserializeObject(content, typeof(DealChangeNotice));

        //    ReturnResult r = new MeiTuanInter().ProductChangedNotify(url, mrequest, mlog);

        //    Label1.Text = r.Message;
        //}


        ////退款通知接口
        //if (url == "http://lvyou.meituan.com/rhone/lv/order/refund/notice")
        //{
        //    OrderCancelResponse mrequest = (OrderCancelResponse)JsonConvert.DeserializeObject(content, typeof(OrderCancelResponse));

        //    ReturnResult r = new MeiTuanInter().RefundNotify(url, mrequest, mlog);

        //    Label1.Text = r.Message;
        //}
        ////消费通知接口
        //if (url == "http://lvyou.meituan.com/rhone/lv/order/consume/notice")
        //{
        //    OrderConsumeNotice mrequest = (OrderConsumeNotice)JsonConvert.DeserializeObject(content, typeof(OrderConsumeNotice));

        //    ReturnResult r = new MeiTuanInter().ConsumeNotify(url, mrequest, mlog);

        //    Label1.Text = r.Message;
        //}


        //}
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Security.Cryptography;
using ETS2.PM.Service.Meituan.Data;
using ETS2.PM.Service.Meituan.Model;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.Meituan_jk
{
    public partial class MTSendToMe : System.Web.UI.Page
    {
        private Agent_company agentinfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            //6313(易城专属测试分销) 需要替换为测试分销商id
            agentinfo = new AgentCompanyData().GetAgentCompany(6313);
        }
        /// <summary>
        /// 拉取账户余额
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton8_Click(object sender, EventArgs e)
        {
            string s = "{" +
                          "\"partnerId\": " + agentinfo.mt_partnerId + "," +
                          "\"body\": {" +
                            "\"partnerDealIds\": [\"1000_100\",\"2000_200\"]" +
                          "}" +
                        "}";
            //string[] arr = new string[] { "zhangsan", "lisi" }; 
            //string s = JsonConvert.SerializeObject(new {aa=arr });
            t_xml.Text = s;

        }
        /// <summary>
        /// 拉取poi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            string s = "{" +
                          "\"partnerId\": " + agentinfo.mt_partnerId + "," +
                          "\"body\": {" +
                            "\"method\": \"page\"," +
                            "\"currentPage\": 1," +
                            "\"pageSize\": 10," +
                            "\"partnerPoiId\": [" +
                              "\"1114\"" +
                            "]" +
                          "}" +
                        "}";
            t_xml.Text = s;

        }
        /// <summary>
        /// 拉取deal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            string s = "{" +
                          "\"partnerId\": " + agentinfo.mt_partnerId + "," +
                          "\"body\": {" +
                            "\"method\": \"page\"," +
                            "\"currentPage\": 1," +
                            "\"pageSize\": 10," +
                            "\"partnerDealIds\": [" +
                              "\"1114\"" +
                            "]" +
                          "}" +
                        "}";
            t_xml.Text = s;

        }

        /// <summary>
        /// 订单创建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton5_Click(object sender, EventArgs e)
        {
            string s = "{" +
                        "\"partnerId\": " + agentinfo.mt_partnerId + "," +
                        "\"body\": {" +
                          "\"orderId\": 1453361297221," +//美团订单
                          "\"partnerDealId\": \"12113\"," + //第三方dealId	
                          "\"mtDealId\": 111111," +//美团DealId
                          "\"buyPrice\": 1080.00," +//商品结算单价
                          "\"unitPrice\": 1199.00," +//商品单价(美团售价)	
                          "\"totalPrice\": 2398.00," +//订单总金额
                          "\"quantity\": 2," +//购买数量
                          "\"visitors\": [{" +
                            "\"name\":\"张三\"," +
                            "\"pinyin\":\"zhangsan\"," +
                            "\"mobile\":\"13522401000\"," +
                            "\"address\":\"\"," +
                            "\"postCode\":\"\"," +
                            "\"email\":\"\"," +
                            "\"credentials\":{\"0\":\"130181198307077635\"}," +
                            "\"gender\":1" +
                          "}]," +
                           "\"travelDate\": \"2016-11-30\"" +//预约游玩日期
                        "}" +
                      "}";
            t_xml.Text = s;

        }
        /// <summary>
        /// 订单支付
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            string s = "{" +
                         "\"partnerId\": " + agentinfo.mt_partnerId + "," +
                         "\"body\": {" +
                           "\"orderId\": 1453361297221," +//美团订单
                           "\"partnerOrderId\": \"\"" + //合作方订单ID
                         "}" +
                       "}";
            t_xml.Text = s;
        }
        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            string s = "{" +
                        "\"partnerId\": " + agentinfo.mt_partnerId + "," +
                        "\"body\": {" +
                          "\"orderId\": 1453361297221," +//美团订单
                          "\"partnerOrderId\": \"\"" + //合作方订单ID
                        "}" +
                      "}";
            t_xml.Text = s;
        }

        /// <summary>
        /// 订单退款
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton6_Click(object sender, EventArgs e)
        {
            string s = "{\"partnerId\": " + agentinfo.mt_partnerId + "," +
            "\"body\": " +
            "{" +
                "\"orderId\": 1453361297221," +
                "\"refundId\": \"1453361297221-01\", " +
                "\"partnerOrderId\": \"211602\", " +//合作方订单ID
                "\"partnerDealId\": \"12113\"," +//合作方产品号
                "\"voucherList\": [ \"910632001657\" ]," +//要退的票对应的凭证码列表
                "\"refundQuantity\": 1," +//退款票数
                "\"unitPrice\":1080," +//美团售卖单价
                "\"refundPrice\": 1080, " +//美团需退还用户总金额
                "\"refundFee\": 0," +//本次退款合作方收取的手续费
                "\"refundTime\": \""+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\" }" +//用户第一次发起退款申请的时间
            "}";

            t_xml.Text = s;
        }
        /// <summary>
        /// 拉取价格日历
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton7_Click(object sender, EventArgs e)
        {
            string s = "{" +
                         "\"partnerId\": " + agentinfo.mt_partnerId + "," +
                         "\"body\": {" +
                           "\"partnerDealId\": \"12113\"," +
                           "\"startTime\": \"2016-11-22\"," +
                           "\"endTime\": \"2016-11-30\"" +

                         "}" +
                       "}";
            t_xml.Text = s;

        }

        /// <summary>
        /// 合作方心跳测试接口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton9_Click(object sender, EventArgs e)
        {
            string s = "{" +
                          "\"partnerId\": " + agentinfo.mt_partnerId+
                       "}";
            
            t_xml.Text = s;

        }
        /// <summary>
        /// 产品编审状态通知接口测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton10_Click(object sender, EventArgs e)
        {
            string s = "{" +
                          "\"partnerId\": " + agentinfo.mt_partnerId + "," +
                          "\"body\": [{" +
                              "\"partnerDealId\":\"" + 12113 + "\"," +
                              "\"status\": 1," +
                              "\"checkStatus\": 1 ," +
                              "\"msRatioCheckStatus\": 1" +
                          "},{" +
                              "\"partnerDealId\":\"" + 12114 + "\"," +
                              "\"status\": 1," +
                              "\"checkStatus\": 1 ," +
                              "\"msRatioCheckStatus\": 1" +
                          "},{" +
                              "\"partnerDealId\":\"" + 12115 + "\"," +
                              "\"status\": 0," +
                              "\"checkStatus\": 0 ," +
                              "\"msRatioCheckStatus\": 0" +
                          "}]" +
                        "}"; 
            t_xml.Text = s;

        }
        /// <summary>
        /// 美团已退款消息通知接口测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton11_Click(object sender, EventArgs e)
        {
            string s = "{" +
                          "\"partnerId\": " + agentinfo.mt_partnerId + "," +
                          "\"body\": {" +
                                "\"orderId\": 1453361297221," +
                                "\"partnerOrderId\": \"211602\"," +
                                "\"refundSerialNo\": \"1453361297221-00\"," +
                                "\"voucherList\": [\"910632001657\"]," +
                                "\"voucherType\": 2," +
                                "\"refundPrice\": 1080," +
                                "\"refundMessageType\": 1," +
                                "\"operator\": \"aaaaa\"," +
                                "\"reason\": \"sssss\"," +
                                "\"refundTime\": \""+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\"," +
                                "\"count\": 1," +
                          "}" +
                        "}";
          
            t_xml.Text = s;

        }


        #region 模拟美团向合作方发送查询账户余额请求
        public ReturnResult SimulationMTGetDeal(string url, MtpBalanceRequestBody body)
        {
            ReturnResult result = new ReturnResult
            {
                IsSuccess = false,
            };
            try
            {
                MtpBalanceRequest requestData = new MtpBalanceRequest
                {
                    partnerId = agentinfo.mt_partnerId.ConvertTo<int>(0),
                    body = body
                };

                var responseStr = new MeiTuanInter(agentinfo.mt_partnerId, agentinfo.mt_secret, agentinfo.mt_client).DoRequest(url, JsonConvert.SerializeObject(requestData));
                if (string.IsNullOrEmpty(responseStr))
                {
                    result.Message = "返回数据为空";
                }
                else
                {
                    MtpBalanceResponse responseBody = (MtpBalanceResponse)JsonConvert.DeserializeObject(responseStr, typeof(MtpBalanceResponse));
                    if (responseBody.code == 200)
                    {
                        result.IsSuccess = true;
                        result.Message = JsonConvert.SerializeObject(responseBody);
                    }
                    else
                    {
                        result.Message = responseBody.describe;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = "异常" + ex.Message;
            }
            return result;
        }
        protected void Button8_Click(object sender, EventArgs e)
        {
            if (t_xml.Text.Trim() == "")
            {
                Label1.Text = "请填写发送内容";
                return;
            }


            MtpBalanceRequest mrequest = (MtpBalanceRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(MtpBalanceRequest));

            ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/balanceget.aspx", mrequest.body);



            Label1.Text = r.Message;

        }
        #endregion

        #region 模拟美团向合作方发送抓取Poi请求
        public ReturnResult SimulationMTGetDeal(string url, PoiRequestBody body)
        {
            ReturnResult result = new ReturnResult
            {
                IsSuccess = false,
            };
            try
            {
                PoiRequest requestData = new PoiRequest
                {
                    partnerId = agentinfo.mt_partnerId,
                    body = body
                };

                var responseStr = new MeiTuanInter(agentinfo.mt_partnerId, agentinfo.mt_secret, agentinfo.mt_client).DoRequest(url, JsonConvert.SerializeObject(requestData));
                if (string.IsNullOrEmpty(responseStr))
                {
                    result.Message = "返回数据为空";
                }
                else
                {
                    PoiResponse responseBody = (PoiResponse)JsonConvert.DeserializeObject(responseStr, typeof(PoiResponse));
                    if (responseBody.code == 200)
                    {
                        result.IsSuccess = true;
                        result.Message = JsonConvert.SerializeObject(responseBody);
                    }
                    else
                    {
                        result.Message = responseBody.describe;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = "异常" + ex.Message;
            }
            return result;
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (t_xml.Text.Trim() == "")
            {
                Label1.Text = "请填写发送内容";
                return;
            }

            PoiRequest mrequest = (PoiRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(PoiRequest));

            ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/poiget.aspx", mrequest.body);


            Label1.Text = r.Message;

        }
        #endregion


        #region 模拟美团向合作方发送抓取deal请求
        public ReturnResult SimulationMTGetDeal(string url, DealRequestBody body)
        {
            ReturnResult result = new ReturnResult
            {
                IsSuccess = false,
            };
            try
            {
                DealRequest requestData = new DealRequest
                {
                    partnerId = agentinfo.mt_partnerId,
                    body = body
                };

                var responseStr = new MeiTuanInter(agentinfo.mt_partnerId, agentinfo.mt_secret, agentinfo.mt_client).DoRequest(url, JsonConvert.SerializeObject(requestData));
                if (string.IsNullOrEmpty(responseStr))
                {
                    result.Message = "返回数据为空";
                }
                else
                {
                    DealResponse responseBody = (DealResponse)JsonConvert.DeserializeObject(responseStr, typeof(DealResponse));
                    if (responseBody.code == 200)
                    {
                        result.IsSuccess = true;
                        result.Message = JsonConvert.SerializeObject(responseBody);
                    }
                    else
                    {
                        result.Message = responseBody.describe;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = "异常" + ex.Message;
            }
            return result;
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (t_xml.Text.Trim() == "")
            {
                Label1.Text = "请填写发送内容";
                return;
            }


            DealRequest mrequest = (DealRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(DealRequest));

            ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/dealget.aspx", mrequest.body);



            Label1.Text = r.Message;

        }
        #endregion
        #region 模拟美团向合作方拉取价格日历
        public ReturnResult SimulationMTGetDeal(string url, PriceRequestBody body)
        {
            ReturnResult result = new ReturnResult
            {
                IsSuccess = false,
            };
            try
            {
                PriceRequest requestData = new PriceRequest
                {
                    partnerId = agentinfo.mt_partnerId,
                    body = body
                };

                var responseStr = new MeiTuanInter(agentinfo.mt_partnerId, agentinfo.mt_secret, agentinfo.mt_client).DoRequest(url, JsonConvert.SerializeObject(requestData));
                if (string.IsNullOrEmpty(responseStr))
                {
                    result.Message = "返回数据为空";
                }
                else
                {
                    PriceResponse responseBody = (PriceResponse)JsonConvert.DeserializeObject(responseStr, typeof(PriceResponse));
                    if (responseBody.code == 200)
                    {
                        result.IsSuccess = true;
                        result.Message = JsonConvert.SerializeObject(responseBody);
                    }
                    else
                    {
                        result.Message = responseBody.describe;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = "异常" + ex.Message;
            }
            return result;
        }
        protected void Button7_Click(object sender, EventArgs e)
        {
            if (t_xml.Text.Trim() == "")
            {
                Label1.Text = "请填写发送内容";
                return;
            }
            #region 注释内容
            //PoiRequest mrequest = (PoiRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(PoiRequest));

            //ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/poiget.aspx", mrequest.body);

            //DealRequest mrequest = (DealRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(DealRequest));

            //ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/dealget.aspx", mrequest.body);

            //OrderPayRequest mrequest = (OrderPayRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(OrderPayRequest));

            //ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/orderpay.aspx", mrequest.body);

            //VoucherRequest mrequest = (VoucherRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(VoucherRequest));

            //ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/voucherresend.aspx", mrequest.body);

            //OrderQueryRequest mrequest = (OrderQueryRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(OrderQueryRequest));

            //ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/orderquery.aspx", mrequest.body);

            //OrderCancelRequest mrequest = (OrderCancelRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(OrderCancelRequest));

            //ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/orderrefund.aspx", mrequest.body);
            #endregion
            PriceRequest mrequest = (PriceRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(PriceRequest));

            ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/priceget.aspx", mrequest.body);


            Label1.Text = r.Message;

        }
        #endregion

        #region 模拟美团向合作方创建订单请求
        public ReturnResult SimulationMTGetDeal(string url, OrderCreateRequestBody body)
        {
            ReturnResult result = new ReturnResult
            {
                IsSuccess = false,
            };
            try
            {
                OrderCreateRequest requestData = new OrderCreateRequest
                {
                    partnerId = agentinfo.mt_partnerId.ConvertTo<int>(0),
                    body = body
                };

                var responseStr = new MeiTuanInter(agentinfo.mt_partnerId, agentinfo.mt_secret, agentinfo.mt_client).DoRequest(url, JsonConvert.SerializeObject(requestData));
                if (string.IsNullOrEmpty(responseStr))
                {
                    result.Message = "返回数据为空";
                }
                else
                {
                    OrderCreateResponse responseBody = (OrderCreateResponse)JsonConvert.DeserializeObject(responseStr, typeof(OrderCreateResponse));
                    if (responseBody.code == 200)
                    {
                        result.IsSuccess = true;
                        result.Message = JsonConvert.SerializeObject(responseBody);
                    }
                    else
                    {
                        result.Message = responseBody.describe;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = "异常" + ex.Message;
            }
            return result;
        }
        protected void Button5_Click(object sender, EventArgs e)
        {
            if (t_xml.Text.Trim() == "")
            {
                Label1.Text = "请填写发送内容";
                return;
            }


            OrderCreateRequest mrequest = (OrderCreateRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(OrderCreateRequest));

            ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/ordercreate.aspx", mrequest.body);


            Label1.Text = r.Message;

        }
        #endregion

        #region 模拟美团向合作方发送提单支付请求
        public ReturnResult SimulationMTGetDeal(string url, OrderPayRequestBody body)
        {
            ReturnResult result = new ReturnResult
            {
                IsSuccess = false,
            };
            try
            {
                OrderPayRequest requestData = new OrderPayRequest
                {
                    partnerId = agentinfo.mt_partnerId,
                    body = body
                };

                var responseStr = new MeiTuanInter(agentinfo.mt_partnerId, agentinfo.mt_secret, agentinfo.mt_client).DoRequest(url, JsonConvert.SerializeObject(requestData));
                if (string.IsNullOrEmpty(responseStr))
                {
                    result.Message = "返回数据为空";
                }
                else
                {
                    OrderPayResponse responseBody = (OrderPayResponse)JsonConvert.DeserializeObject(responseStr, typeof(OrderPayResponse));
                    if (responseBody.code == 200)
                    {
                        result.IsSuccess = true;
                        result.Message = JsonConvert.SerializeObject(responseBody);
                    }
                    else
                    {
                        result.Message = responseBody.describe;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = "异常" + ex.Message;
            }
            return result;
        }
        protected void Button3_Click(object sender, EventArgs e)
        {
            if (t_xml.Text.Trim() == "")
            {
                Label1.Text = "请填写发送内容";
                return;
            }

            OrderPayRequest mrequest = (OrderPayRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(OrderPayRequest));

            ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/orderpay.aspx", mrequest.body);



            Label1.Text = r.Message;

        }
        #endregion
        #region 模拟美团向合作方查询请求
        public ReturnResult SimulationMTGetDeal(string url, OrderQueryRequestBody body)
        {
            ReturnResult result = new ReturnResult
            {
                IsSuccess = false,
            };
            try
            {
                OrderQueryRequest requestData = new OrderQueryRequest
                {
                    partnerId = agentinfo.mt_partnerId,
                    body = body
                };

                var responseStr = new MeiTuanInter(agentinfo.mt_partnerId, agentinfo.mt_secret, agentinfo.mt_client).DoRequest(url, JsonConvert.SerializeObject(requestData));
                if (string.IsNullOrEmpty(responseStr))
                {
                    result.Message = "返回数据为空";
                }
                else
                {
                    OrderQueryResponse responseBody = (OrderQueryResponse)JsonConvert.DeserializeObject(responseStr, typeof(OrderQueryResponse));
                    if (responseBody.code == 200 )
                    {
                        result.IsSuccess = true;
                        result.Message = JsonConvert.SerializeObject(responseBody);
                    }
                    else
                    {
                        result.Message = responseBody.describe;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = "异常" + ex.Message;
            }
            return result;
        }
        protected void Button4_Click(object sender, EventArgs e)
        {
            if (t_xml.Text.Trim() == "")
            {
                Label1.Text = "请填写发送内容";
                return;
            }


            OrderQueryRequest mrequest = (OrderQueryRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(OrderQueryRequest));

            ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/orderquery.aspx", mrequest.body);

            Label1.Text = r.Message;

        }
        #endregion

        #region 模拟美团向合作方退款请求
        public ReturnResult SimulationMTGetDeal(string url, OrderCancelRequestBody body)
        {
            ReturnResult result = new ReturnResult
            {
                IsSuccess = false,
            };
            try
            {
                OrderCancelRequest requestData = new OrderCancelRequest
                {
                    partnerId = agentinfo.mt_partnerId,
                    body = body
                };

                var responseStr = new MeiTuanInter(agentinfo.mt_partnerId, agentinfo.mt_secret, agentinfo.mt_client).DoRequest(url, JsonConvert.SerializeObject(requestData));
                if (string.IsNullOrEmpty(responseStr))
                {
                    result.Message = "返回数据为空";
                }
                else
                {
                    OrderCancelResponse responseBody = (OrderCancelResponse)JsonConvert.DeserializeObject(responseStr, typeof(OrderCancelResponse));
                    if (responseBody.code == 200)
                    {
                        result.IsSuccess = true;
                        result.Message = JsonConvert.SerializeObject(responseBody);
                    }
                    else
                    {
                        result.Message = responseBody.describe;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = "异常" + ex.Message;
            }
            return result;
        }
        protected void Button6_Click(object sender, EventArgs e)
        {
            if (t_xml.Text.Trim() == "")
            {
                Label1.Text = "请填写发送内容";
                return;
            }
            #region 注释内容
            //PoiRequest mrequest = (PoiRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(PoiRequest));

            //ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/poiget.aspx", mrequest.body);

            //DealRequest mrequest = (DealRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(DealRequest));

            //ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/dealget.aspx", mrequest.body);

            //OrderPayRequest mrequest = (OrderPayRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(OrderPayRequest));

            //ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/orderpay.aspx", mrequest.body);

            //VoucherRequest mrequest = (VoucherRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(VoucherRequest));

            //ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/voucherresend.aspx", mrequest.body);

            //OrderQueryRequest mrequest = (OrderQueryRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(OrderQueryRequest));

            //ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/orderquery.aspx", mrequest.body);
            #endregion
            OrderCancelRequest mrequest = (OrderCancelRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(OrderCancelRequest));

            ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/orderrefund.aspx", mrequest.body);


            Label1.Text = r.Message;

        }
        #endregion


        #region 模拟美团向合作方发送心跳接口请求
        public ReturnResult SimulationMTGetDeal(string url)
        {
            ReturnResult result = new ReturnResult
            {
                IsSuccess = false,
            };
            try
            {
                MtpApiRequest requestData = new MtpApiRequest
                {
                    partnerId = agentinfo.mt_partnerId.ConvertTo<int>(0)  
                };

                var responseStr = new MeiTuanInter(agentinfo.mt_partnerId, agentinfo.mt_secret, agentinfo.mt_client).DoRequest(url, JsonConvert.SerializeObject(requestData));
                if (string.IsNullOrEmpty(responseStr))
                {
                    result.Message = "返回数据为空";
                }
                else
                {
                    MtpApiResponse responseBody = (MtpApiResponse)JsonConvert.DeserializeObject(responseStr, typeof(MtpApiResponse));
                    if (responseBody.code == 200)
                    {
                        result.IsSuccess = true;
                        result.Message = JsonConvert.SerializeObject(responseBody);
                    }
                    else
                    {
                        result.Message = responseBody.describe;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = "异常" + ex.Message;
            }
            return result;
        }
        protected void Button9_Click(object sender, EventArgs e)
        {
            if (t_xml.Text.Trim() == "")
            {
                Label1.Text = "请填写发送内容";
                return;
            }


            MtpApiRequest mrequest = (MtpApiRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(MtpApiRequest));

            ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/checkAlive.aspx");



            Label1.Text = r.Message;

        }
        #endregion

        #region 模拟美团向合作方发送产品编审状态通知请求
        public ReturnResult SimulationMTGetDeal(string url, List<MtpDealSendNoticeRequestBody> body)
        {
            ReturnResult result = new ReturnResult
            {
                IsSuccess = false,
            };
            try
            {
                MtpDealSendNoticeRequest requestData = new MtpDealSendNoticeRequest
                {
                    partnerId = agentinfo.mt_partnerId.ConvertTo<int>(0),
                    body = body
                };

                var responseStr = new MeiTuanInter(agentinfo.mt_partnerId, agentinfo.mt_secret, agentinfo.mt_client).DoRequest(url, JsonConvert.SerializeObject(requestData));
                if (string.IsNullOrEmpty(responseStr))
                {
                    result.Message = "返回数据为空";
                }
                else
                {
                    MtpApiResponse responseBody = (MtpApiResponse)JsonConvert.DeserializeObject(responseStr, typeof(MtpApiResponse));
                    if (responseBody.code == 200)
                    {
                        result.IsSuccess = true;
                        result.Message = JsonConvert.SerializeObject(responseBody);
                    }
                    else
                    {
                        result.Message = responseBody.describe;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = "异常" + ex.Message;
            }
            return result;
        }
        protected void Button10_Click(object sender, EventArgs e)
        {
            if (t_xml.Text.Trim() == "")
            {
                Label1.Text = "请填写发送内容";
                return;
            }


            MtpDealSendNoticeRequest mrequest = (MtpDealSendNoticeRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(MtpDealSendNoticeRequest));

            ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/dealnoticesend.aspx",mrequest.body);



            Label1.Text = r.Message;

        }
        #endregion

        #region 模拟美团向合作方发送美团已退款消息请求
        public ReturnResult SimulationMTGetDeal(string url, MtpOrderRefundedMessageRequestBody body)
        {
            ReturnResult result = new ReturnResult
            {
                IsSuccess = false,
            };
            try
            {
                MtpOrderRefundedMessageRequest requestData = new MtpOrderRefundedMessageRequest
                {
                    partnerId = agentinfo.mt_partnerId.ConvertTo<int>(0),
                    body = body
                };

                var responseStr = new MeiTuanInter(agentinfo.mt_partnerId, agentinfo.mt_secret, agentinfo.mt_client).DoRequest(url, JsonConvert.SerializeObject(requestData));
                if (string.IsNullOrEmpty(responseStr))
                {
                    result.Message = "返回数据为空";
                }
                else
                {
                    MtpApiResponse responseBody = (MtpApiResponse)JsonConvert.DeserializeObject(responseStr, typeof(MtpApiResponse));
                    if (responseBody.code == 200)
                    {
                        result.IsSuccess = true;
                        result.Message = JsonConvert.SerializeObject(responseBody);
                    }
                    else
                    {
                        result.Message = responseBody.describe;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = "异常" + ex.Message;
            }
            return result;
        }
        protected void Button11_Click(object sender, EventArgs e)
        {
            if (t_xml.Text.Trim() == "")
            {
                Label1.Text = "请填写发送内容";
                return;
            }


            MtpOrderRefundedMessageRequest mrequest = (MtpOrderRefundedMessageRequest)JsonConvert.DeserializeObject(t_xml.Text.Trim(), typeof(MtpOrderRefundedMessageRequest));

            ReturnResult r = SimulationMTGetDeal("http://localhost:1111/meituan-jk/orderRefundedMessage.aspx", mrequest.body);



            Label1.Text = r.Message;

        }
        #endregion
    }
}
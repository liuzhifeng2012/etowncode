using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using System.Text;
using System.Collections.Specialized;
using System.Xml;
using Newtonsoft.Json;

using System.Xml.Serialization;
using System.IO;
using ETS2.PM.Service.Qunar_Ms.QMRequestDataSchema;
using ETS2.PM.Service.Qunar_Ms.Model;
using ETS2.PM.Service.Qunar_Ms.Data;
using ETS.JsonFactory;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.PM.Service.Qunar_Ms.QMResponseDataSchema;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Modle;

namespace ETS2.WebApp.Qunar_Ms
{
    /// <summary>
    /// Notice 的摘要说明
    /// </summary>
    public class Notice : IHttpHandler
    {
        private static object lockobj = new object();
        public void ProcessRequest(HttpContext context)
        {
            lock (lockobj)
            {
                context.Response.ContentType = "text/plain";
                string method = "";
                string requestParam = "";
                SortedDictionary<string, string> dicArray = CommonFunc.GetRequestPost();
                if (dicArray.Count > 0)
                {
                    string paramstr = CommonFunc.CreateLinkString(dicArray);
                    //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarlog" + DateTime.Now.ToString("yyyyMMdd") + ".txt", paramstr);

                    method = dicArray["method"];
                    requestParam = dicArray["requestParam"];

                }
                else
                {
                    method = "testAlive";
                    requestParam = "Are you alive?";
                }
                //接口心跳检测
                if (method == "testAlive")
                {
                    if (requestParam == "Are you alive?")
                    {
                        context.Response.Write("alive");
                        return;
                    }
                    context.Response.Write("alive");
                    return;
                }
                //method = "noticeOrderRefundedByQunar";
                //requestParam = "{\"data\":\"PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiIHN0YW5kYWxvbmU9InllcyI/PjxyZXF1ZXN0IHhtbG5zPSJodHRwOi8vcGlhby5xdW5hci5jb20vMjAxMy9RTWVucGlhb1JlcXVlc3RTY2hlbWEiPjxoZWFkZXI+PGFwcGxpY2F0aW9uPlF1bmFyLk1lbnBpYW8uQWdlbnQ8L2FwcGxpY2F0aW9uPjxwcm9jZXNzb3I+U3VwcGxpZXJEYXRhRXhjaGFuZ2VQcm9jZXNzb3I8L3Byb2Nlc3Nvcj48dmVyc2lvbj52Mi4wLjA8L3ZlcnNpb24+PGJvZHlUeXBlPk5vdGljZU9yZGVyUmVmdW5kZWRCeVF1bmFyUmVxdWVzdEJvZHk8L2JvZHlUeXBlPjxjcmVhdGVVc2VyPlF1bmFyLk1lbnBpYW8uQWdlbnQ8L2NyZWF0ZVVzZXI+PGNyZWF0ZVRpbWU+MjAxNS0wNS0yNSAxMTo0MzowOTwvY3JlYXRlVGltZT48c3VwcGxpZXJJZGVudGl0eT5USUFOR1VJPC9zdXBwbGllcklkZW50aXR5PjwvaGVhZGVyPjxib2R5IHhzaTp0eXBlPSJOb3RpY2VPcmRlclJlZnVuZGVkQnlRdW5hclJlcXVlc3RCb2R5IiB4bWxuczp4c2k9Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvWE1MU2NoZW1hLWluc3RhbmNlIj48b3JkZXJJbmZvPjxwYXJ0bmVyb3JkZXJJZD43OTUxMDwvcGFydG5lcm9yZGVySWQ+PHJlZnVuZFNlcT44OTc3OTU8L3JlZnVuZFNlcT48b3JkZXJRdWFudGl0eT4zPC9vcmRlclF1YW50aXR5PjxyZWZ1bmRRdWFudGl0eT4xPC9yZWZ1bmRRdWFudGl0eT48b3JkZXJQcmljZT42MDA8L29yZGVyUHJpY2U+PHJlZnVuZFJlYXNvbj7ooYznqIvlj5jljJY8L3JlZnVuZFJlYXNvbj48cmVmdW5kRXhwbGFpbj5kZGQ8L3JlZnVuZEV4cGxhaW4+PHJlZnVuZEp1ZGdlTWFyaz5zc3NhPC9yZWZ1bmRKdWRnZU1hcms+PG9yZGVyUmVmdW5kQ2hhcmdlPjA8L29yZGVyUmVmdW5kQ2hhcmdlPjwvb3JkZXJJbmZvPjwvYm9keT48L3JlcXVlc3Q+\",\"securityType\":\"MD5\",\"signed\":\"5FDB48D2495A8C31F6DD3D3565E8DB05\"}";

                #region 去哪请求供应商接口 数据记录
                requestParam = "{\"root\":" + requestParam + "}";
                XmlDocument doc1 = JsonConvert.DeserializeXmlNode(requestParam);
                XmlElement root = doc1.DocumentElement;
                string data = root.SelectSingleNode("data").InnerText;
                string securityType = root.SelectSingleNode("securityType").InnerText;
                string signed = root.SelectSingleNode("signed").InnerText;

                //base64解密
                data = Encoding.UTF8.GetString(EncryptionExtention.FromBase64(data));

                Qunar_ms_requestlog rlog = new Qunar_ms_requestlog
                {
                    id = 0,
                    method = method,
                    requestParam = requestParam,
                    base64data = root.SelectSingleNode("data").InnerText,
                    securityType = securityType,
                    signed = signed,
                    frombase64data = data,
                    bodyType = "",
                    createUser = "",
                    supplierIdentity = "",
                    createTime = DateTime.Now,
                    qunar_orderId = "",
                    msg = ""
                };
                int rlogid = new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);

                rlog.id = rlogid;
                #endregion

                XmlDocument xr = new XmlDocument();
                xr.LoadXml(data);
                XmlNamespaceManager nsMgr = new XmlNamespaceManager(xr.NameTable);
                nsMgr.AddNamespace("ns", "http://piao.qunar.com/2013/QMenpiaoRequestSchema");

                string supplierIdentity = xr.SelectSingleNode("/ns:request/ns:header/ns:supplierIdentity", nsMgr).InnerText;

                #region 根据去哪用户名得到用户秘钥

                B2b_company company_qunar = new B2bCompanyData().GetqunarbyQunarname(supplierIdentity);
                if (company_qunar == null)
                {
                    rlog.msg = rlog.msg + "(去哪用户:" + supplierIdentity + ")去哪标识信息获取失败";
                    new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                    return;
                }
                if (company_qunar.isqunar == 0 || company_qunar.qunar_pass == "")
                {
                    rlog.msg = rlog.msg + "(去哪用户:" + supplierIdentity + ")去哪标识信息状态有误 或 信息填写不完善";
                    new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                    return;
                }


                #endregion
                string signkey = company_qunar.qunar_pass;//去哪密码

                //对签名进行验证 
                string self_singed = EncryptionHelper.ToMD5(signkey + root.SelectSingleNode("data").InnerText, "utf-8");
                if (self_singed == signed)//签名相同
                {
                    #region 统一读取 header消息
                    string bodyType = xr.SelectSingleNode("/ns:request/ns:header/ns:bodyType", nsMgr).InnerText;
                    string createUser = xr.SelectSingleNode("/ns:request/ns:header/ns:createUser", nsMgr).InnerText; ;
                    supplierIdentity = xr.SelectSingleNode("/ns:request/ns:header/ns:supplierIdentity", nsMgr).InnerText; ;//去哪用户名
                    DateTime createTime = xr.SelectSingleNode("/ns:request/ns:header/ns:createTime", nsMgr).InnerText.ConvertTo<DateTime>(DateTime.Now);

                    rlog.bodyType = bodyType;
                    rlog.createUser = createUser;
                    rlog.supplierIdentity = supplierIdentity;

                    new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);

                    #endregion


                    #region 创建订单(支付前下单)
                    if (method == "createOrderForBeforePaySync")
                    {
                        //xml转化为对象
                        data = data.Replace("http://www.w3.org/2001/XMLSchema-instance", "http://piao.qunar.com/2013/QMenpiaoRequestSchema");
                        CreateOrderForBeforePaySyncRequestBodyrequest mrequest = new CreateOrderForBeforePaySyncRequestBodyrequest();

                        mrequest = XmlHelper.XmlDeserialize<CreateOrderForBeforePaySyncRequestBodyrequest>(data, Encoding.UTF8);
                        RequestHeader header = mrequest.header;

                        #region 支付前下单订单信息
                        CreateOrderForBeforePaySyncRequestBody createOrderForBeforePaySyncRequestBody = mrequest.body;


                        CreateOrderForBeforePaySyncRequestBodyorderInfo createOrderForBeforePaySyncRequestBodyorderInfo = createOrderForBeforePaySyncRequestBody.orderInfo;
                        string qunar_orderId = createOrderForBeforePaySyncRequestBodyorderInfo.orderId;
                        string orderQuantity = createOrderForBeforePaySyncRequestBodyorderInfo.orderQuantity;
                        string orderPrice = createOrderForBeforePaySyncRequestBodyorderInfo.orderPrice;//总价
                        string orderCashBackMoney = createOrderForBeforePaySyncRequestBodyorderInfo.orderCashBackMoney;
                        string orderStatus = createOrderForBeforePaySyncRequestBodyorderInfo.orderStatus;
                        string orderRemark = createOrderForBeforePaySyncRequestBodyorderInfo.orderRemark;
                        string orderSource = createOrderForBeforePaySyncRequestBodyorderInfo.orderSource;
                        string eticketNo = createOrderForBeforePaySyncRequestBodyorderInfo.eticketNo;

                        //判断通知日志表中是否含有当前去哪订单 的支付前下单通知，含有的话标注为 “重复通知”
                        bool ishasnotice = new Qunar_ms_requestlogData().IsHasNotice(qunar_orderId, method);

                        rlog.qunar_orderId = qunar_orderId;
                        if (ishasnotice)
                        {
                            TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "logid:" + rlog.id + ",createOrderForBeforePaySync:重复通知");
                        }
                        new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);


                        //订单表(b2b_order)已经含有去哪订单，有则按重复通知不在处理
                        bool IsHasSuc = new Qunar_ms_requestlogData().IsHasSuc("createOrderForBeforePaySync", qunar_orderId);
                        if (IsHasSuc)
                        {
                            int Rparterorderid = new B2bOrderData().GetParterOrderId(qunar_orderId);

                            string RorderStatus = "PREPAY_ORDER_NOT_PAYED";

                            string RresponseParam = GetCreateOrderForBeforePaySyncResponseBodyresponse("1000", "成功", Rparterorderid, RorderStatus, signkey);
                            context.Response.Write(RresponseParam);

                            rlog.msg = "suc";
                            new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                            return;
                        }



                        CreateOrderForBeforePaySyncRequestBodyorderInfoproduct mproduct = createOrderForBeforePaySyncRequestBodyorderInfo.product;
                        string resourceId = mproduct.resourceId;
                        string productName = mproduct.productName;
                        string visitDate = mproduct.visitDate;
                        string sellPrice = mproduct.sellPrice;//单价
                        string cashBackMoney = mproduct.cashBackMoney;

                        CreateOrderForBeforePaySyncRequestBodyorderInfocontactPerson mcontactPerson = createOrderForBeforePaySyncRequestBodyorderInfo.contactPerson;
                        string name = mcontactPerson.name;
                        string namePinyin = mcontactPerson.namePinyin;
                        string mobile = mcontactPerson.mobile;
                        string email = mcontactPerson.email;
                        string address = mcontactPerson.address;
                        string zipCode = mcontactPerson.zipCode;

                        //判断是否含有对去哪 请求的详细记录
                        bool ishasrequest = new Qunar_CreateOrderForBeforePaySyncData().Ishasrequest(qunar_orderId);

                        if (ishasrequest == false)
                        {
                            int qunar_CreateOrderForBeforePaySyncid = new Qunar_CreateOrderForBeforePaySyncData().InsQunar_CreateOrderForBeforePaySync(createOrderForBeforePaySyncRequestBody);
                            if (qunar_CreateOrderForBeforePaySyncid == 0)
                            {
                                //把意外信息保存到txt中
                                TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "logid:" + rlog.id + ",qunar_CreateOrderForBeforePaySyncid:ins出错");
                            }
                        }

                        #endregion

                        #region 支付前下单 游玩人信息
                        //判断是否含有 游玩人 信息
                        bool ishasvisitperson = new Qunar_CreateOrderForBeforePaySyncvisitpersonData().Ishasvisitperson(qunar_orderId);

                        CreateOrderForBeforePaySyncRequestBodyorderInfovisitPerson mvisitPerson = createOrderForBeforePaySyncRequestBodyorderInfo.visitPerson;
                        CreateOrderForBeforePaySyncRequestBodyorderInfovisitPersonpersonCollection mpersonCollection = mvisitPerson.personCollection;
                        foreach (CreateOrderForBeforePaySyncRequestBodyorderInfovisitPersonperson mperson in mpersonCollection)
                        {
                            string name1 = mperson.name;
                            string namePinyin1 = mperson.namePinyin;
                            string credentials = mperson.credentials;
                            string credentialsType = mperson.credentialsType;
                            string defined1Value = mperson.defined1Value;
                            string defined2Value = mperson.defined2Value;

                            if (ishasvisitperson == false)
                            {
                                int qunar_visitperson = new Qunar_CreateOrderForBeforePaySyncvisitpersonData().InsQunar_visitperson(mperson, qunar_orderId);
                                if (qunar_visitperson == 0)
                                {
                                    //把意外信息保存到txt中
                                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "logid:" + rlog.id + ",qunar_visitperson:ins出错");
                                }
                            }
                        }
                        #endregion

                        #region  创建直销订单，返回通知
                        decimal cost = 0;
                        decimal profit = 0;
                        int comid = 0;




                        B2b_com_pro pro = new B2bComProData().GetProById(resourceId);
                        if (pro != null)
                        {
                            if (pro.Server_type == 10)//服务类型：旅游大巴
                            {
                                cost = 0;
                                profit = 0;
                            }
                            else
                            {
                                cost = pro.Agentsettle_price;
                                profit = pro.Advise_price - cost;
                            }
                            comid = pro.Com_id;
                            if (company_qunar.ID != comid)
                            {
                                rlog.msg = rlog.msg + "绑定产品在商户中不存在";
                                new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                                return;
                            }


                            //判断直销价格和 去哪价格是否相同
                            decimal adviseprice = pro.Advise_price * 100;
                            decimal qunarprice = decimal.Parse(sellPrice);
                            if (adviseprice != qunarprice)
                            {
                                string err_response = GetErrResponse(bodyType, "20025", "产品价格已变化", signkey);
                                context.Response.Write(err_response);

                                rlog.msg = rlog.msg + ",产品价格已变化";
                                new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                                return;
                            }

                            decimal totalprice = adviseprice * int.Parse(orderQuantity);
                            if (totalprice != decimal.Parse(orderPrice))
                            {
                                string err_response = GetErrResponse(bodyType, "20025", "产品价格已变化", signkey);
                                context.Response.Write(err_response);

                                rlog.msg = rlog.msg + ",产品价格已变化";
                                new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                                return;
                            }
                        }
                        else
                        {
                            string err_response = GetErrResponse(bodyType, "12001", "产品不存在", signkey);
                            context.Response.Write(err_response);

                            rlog.msg = rlog.msg + ",产品不存在";
                            new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                            return;
                        }
                        B2b_order order = new B2b_order()
                        {
                            M_b2b_order_hotel = null,
                            Id = 0,
                            Pro_id = resourceId.ConvertTo<int>(0),
                            Order_type = 1,
                            U_name = name,
                            U_id = 0,
                            U_phone = mobile,
                            U_num = orderQuantity.ConvertTo<int>(0),
                            U_subdate = DateTime.Now,
                            Payid = 0,
                            Pay_price = sellPrice.ConvertTo<decimal>(0) / 100,
                            Cost = 0,
                            Profit = profit,
                            Order_state = (int)OrderStatus.WaitPay,//
                            Pay_state = (int)PayStatus.NotPay,//支付模块未做出来前先设置为已支付
                            Send_state = (int)SendCodeStatus.NotSend,
                            Ticketcode = 0,//电子码未创建，支付后产生码赋值
                            Rebate = 0,//  利润返佣金额暂时定为0
                            Ordercome = "",//订购来源 暂时定为空
                            U_traveldate = visitDate.ConvertTo<DateTime>(DateTime.Now),
                            Dealer = "自动",
                            Comid = comid,
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
                            Order_remark = orderRemark,
                            Deliverytype = 0,
                            Province = "",
                            City = "",
                            Address = address,
                            Code = "",

                        };
                        int parterorderid = 0;
                        try
                        {
                            data = OrderJsonData.InsertOrUpdate(order, out parterorderid);
                        }
                        catch
                        {
                            string err_response = GetErrResponse(bodyType, "1", "创建订单出错", signkey);
                            context.Response.Write(err_response);


                            rlog.msg = rlog.msg + ",创建直销订单出错";
                            new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                            return;
                        }
                        #endregion

                        if (parterorderid > 0)
                        {
                            string nreturnstr = "{\"root\":" + data + "}";
                            XmlDocument ndoc1 = JsonConvert.DeserializeXmlNode(nreturnstr);
                            string type = ndoc1.SelectSingleNode("root/type").InnerText;
                            string msg = ndoc1.SelectSingleNode("root/msg").InnerText;
                            if (type == "100")
                            {
                                orderStatus = "PREPAY_ORDER_NOT_PAYED";
                                string responseParam = GetCreateOrderForBeforePaySyncResponseBodyresponse("1000", "成功", parterorderid, orderStatus, signkey);
                                context.Response.Write(responseParam);

                                string r_data = GetFromBase64(responseParam);
                                int insqunar_response = new Qunar_CreateOrderForBeforePaySyncData().InsQunar_CreateOrderForBeforePaySync_ret(qunar_orderId, parterorderid, orderStatus, "", responseParam + "---" + r_data);
                                if (insqunar_response == 0)
                                {
                                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "logid:" + rlog.id + ",createOrderForBeforePaySync insqunar_response:err");
                                }
                                //把去哪订单号加入 订单表
                                int upb2borderid = new B2bOrderData().InsertQunar_Orderid(parterorderid, rlog.qunar_orderId);
                                if (upb2borderid == 0)
                                {
                                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "logid:" + rlog.id + ",createOrderForBeforePaySync upb2borderid:err");
                                }

                                //标注通知处理成功
                                rlog.msg = "suc";
                                rlog.qunar_orderId = qunar_orderId;
                                new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);

                                return;
                            }
                            else
                            {
                                string err_response = GetErrResponse(bodyType, "1", msg, signkey);
                                context.Response.Write(err_response);

                                int insqunar_response = new Qunar_CreateOrderForBeforePaySyncData().InsQunar_CreateOrderForBeforePaySync_ret(qunar_orderId, parterorderid, orderStatus, "", err_response);
                                if (insqunar_response == 0)
                                {
                                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "logid:" + rlog.id + ",createOrderForBeforePaySync insqunar_response:err");
                                }


                                //标注通知处理失败
                                rlog.msg = msg;
                                rlog.qunar_orderId = qunar_orderId;
                                new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);

                                return;
                            }

                        }
                        else
                        {
                            string nreturnstr = "{\"root\":" + data + "}";
                            XmlDocument ndoc1 = JsonConvert.DeserializeXmlNode(nreturnstr);
                            string msg = ndoc1.SelectSingleNode("root/msg").InnerText;

                            string err_response = GetErrResponse(bodyType, "1", msg, signkey);
                            context.Response.Write(err_response);


                            rlog.msg = rlog.msg + ",创建直销订单出错(" + msg + ")";
                            new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                            return;
                        }

                    }
                    #endregion

                    #region Qunar获取订单信息
                    if (method == "getOrderByQunar")
                    {
                        #region body主体消息
                        string partnerOrderId = xr.SelectSingleNode("/ns:request/ns:body/ns:partnerOrderId", nsMgr).InnerText;
                        int insqunar_request = new Qunar_GetOrderByQunarData().InsQunar_GetOrderByQunar_request(partnerOrderId);
                        if (insqunar_request == 0)
                        {
                            //把意外信息保存到txt中
                            TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "logid:" + rlog.id + ",insqunar_request:ins出错");
                        }
                        #endregion

                        #region  处理
                        //获得去哪订单
                        string qunar_orderid = new Qunar_CreateOrderForBeforePaySyncData().GetQunarOrderId(partnerOrderId);
                        if (qunar_orderid == "")
                        {
                            string err_response = GetErrResponse(bodyType, "13001", "订单不存在", signkey);
                            context.Response.Write(err_response);

                            rlog.msg = "去哪订单不存在(原订单" + partnerOrderId + ")";
                            rlog.qunar_orderId = qunar_orderid;
                            new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                            return;
                        }
                        B2b_order morder = new B2bOrderData().GetOrderById(partnerOrderId.ConvertTo<int>(0));

                        if (morder != null)
                        {
                            if (morder.Bindingagentorderid > 0)
                            {
                                string orderstatus = "";
                                //预付：预订失败 PREPAY_ORDER_BOOL_FAILED
                                if (morder.qunar_orderid == "")
                                {
                                    orderstatus = "PREPAY_ORDER_BOOL_FAILED";
                                }
                                else
                                {
                                    morder = new B2bOrderData().GetOrderById(morder.Bindingagentorderid);

                                    //预付:初始订单 PREPAY_ORDER_INIT


                                    //预付:预订成功，待支付 PREPAY_ORDER_NOT_PAYED
                                    if (morder.Order_state == (int)OrderStatus.WaitPay)
                                    {
                                        orderstatus = "PREPAY_ORDER_NOT_PAYED";
                                    }

                                    //预付:已付款，出票中 

                                    //预付：出票成功 PREPAY_ORDER_PRINT_SUCCESS 
                                    else if (morder.Order_state == (int)OrderStatus.HasSendCode)
                                    {
                                        orderstatus = "PREPAY_ORDER_PRINT_SUCCESS";
                                    }
                                    //预付：订单已取消 PREPAY_ORDER_CANCEL
                                    else if (morder.Order_state == (int)OrderStatus.InvalidOrder)
                                    {
                                        orderstatus = "PREPAY_ORDER_CANCEL";
                                    }
                                    //预付：出票失败 PREPAY_ORDER_PRINT_FAILED
                                    else
                                    {
                                        orderstatus = "PREPAY_ORDER_PRINT_FAILED";
                                    }
                                }
                                string orderQuantity = morder.U_num.ToString();
                                string eticketNo = morder.Pno;

                                string eticketSended = "FALSE";
                                //电子码发送状态:1未发送2已发送3发送中
                                if (morder.Send_state == 2)
                                {
                                    eticketSended = "TRUE";
                                }
                                //获得订单已消费票数 
                                int total_consumenum = 0;
                                string pnostr = morder.Pno;

                                string[] pnoarr = pnostr.Split(',');
                                for (int i = 0; i < pnoarr.Length; i++)
                                {
                                    if (pnoarr[i] != "")
                                    {
                                        total_consumenum += new B2bOrderData().GetHasConsumeNumByPno(pnoarr[i]);
                                    }
                                }
                                //消费信息
                                string consumeInfo = "已消费" + total_consumenum + "张";

                                string responseParam = GetQunar_OrderByQunarResponse("1000", "成功", signkey, partnerOrderId, orderstatus, orderQuantity, eticketNo, eticketSended, total_consumenum, consumeInfo);
                                context.Response.Write(responseParam);

                                string r_data = GetFromBase64(responseParam);
                                int insqunar_response = new Qunar_GetOrderByQunarData().InsQunar_GetOrderByQunar_response(partnerOrderId, orderstatus, orderQuantity, eticketNo, eticketSended, total_consumenum, consumeInfo, responseParam + "---" + r_data);
                                if (insqunar_response == 0)
                                {
                                    //把意外信息保存到txt中
                                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "logid:" + rlog.id + ",insqunar_response:err");
                                }

                                //标注通知处理成功
                                rlog.msg = "suc";
                                rlog.qunar_orderId = qunar_orderid;
                                new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);

                                return;
                            }
                            else
                            {
                                string orderstatus = "";

                                //预付:初始订单 PREPAY_ORDER_INIT
                                //预付：预订失败 PREPAY_ORDER_BOOL_FAILED
                                if (morder.qunar_orderid == "")
                                {
                                    orderstatus = "PREPAY_ORDER_BOOL_FAILED";
                                }
                                //预付:预订成功，待支付 PREPAY_ORDER_NOT_PAYED
                                else
                                {
                                    if (morder.Order_state == (int)OrderStatus.WaitPay)
                                    {
                                        orderstatus = "PREPAY_ORDER_NOT_PAYED";
                                    }

                                        //预付:已付款，出票中 

                                        //预付：出票成功 PREPAY_ORDER_PRINT_SUCCESS 
                                    else if (morder.Order_state == (int)OrderStatus.HasSendCode)
                                    {
                                        orderstatus = "PREPAY_ORDER_PRINT_SUCCESS";
                                    }
                                    //预付：订单已取消 PREPAY_ORDER_CANCEL
                                    else if (morder.Order_state == (int)OrderStatus.InvalidOrder)
                                    {
                                        orderstatus = "PREPAY_ORDER_CANCEL";
                                    }
                                    //预付：出票失败 PREPAY_ORDER_PRINT_FAILED
                                    else
                                    {
                                        orderstatus = "PREPAY_ORDER_PRINT_FAILED";
                                    }
                                }
                                string orderQuantity = morder.U_num.ToString();
                                string eticketNo = morder.Pno;

                                string eticketSended = "FALSE";
                                //电子码发送状态:1未发送2已发送3发送中
                                if (morder.Send_state == 2)
                                {
                                    eticketSended = "TRUE";
                                }
                                //获得订单已消费票数 
                                int total_consumenum = 0;
                                string pnostr = morder.Pno;

                                string[] pnoarr = pnostr.Split(',');
                                for (int i = 0; i < pnoarr.Length; i++)
                                {
                                    if (pnoarr[i] != "")
                                    {
                                        total_consumenum += new B2bOrderData().GetHasConsumeNumByPno(pnoarr[i]);
                                    }
                                }
                                //消费信息
                                string consumeInfo = "已消费" + total_consumenum + "张";

                                string responseParam = GetQunar_OrderByQunarResponse("1000", "成功", signkey, partnerOrderId, orderstatus, orderQuantity, eticketNo, eticketSended, total_consumenum, consumeInfo);
                                context.Response.Write(responseParam);

                                string r_data = GetFromBase64(responseParam);
                                int insqunar_response = new Qunar_GetOrderByQunarData().InsQunar_GetOrderByQunar_response(partnerOrderId, orderstatus, orderQuantity, eticketNo, eticketSended, total_consumenum, consumeInfo, responseParam + "---" + r_data);
                                if (insqunar_response == 0)
                                {
                                    //把意外信息保存到txt中
                                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "logid:" + rlog.id + ",insqunar_response:err");
                                }

                                //标注通知处理成功
                                rlog.msg = "suc";
                                rlog.qunar_orderId = qunar_orderid;
                                new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);

                                return;

                            }

                        }
                        else
                        {
                            //订单不存在
                            string err_response = GetErrResponse(bodyType, "13001", "订单不存在", signkey);
                            context.Response.Write(err_response);

                            rlog.msg = "订单不存在";
                            rlog.qunar_orderId = qunar_orderid;
                            new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                            return;
                        }
                        #endregion
                    }
                    #endregion

                    #region 支付订单（用于支付前下单）
                    if (method == "payOrderForBeforePaySync")
                    {
                        #region body主体消息
                        string partnerOrderId = xr.SelectSingleNode("/ns:request/ns:body/ns:orderInfo/ns:partnerOrderId", nsMgr).InnerText;
                        string orderStatus = xr.SelectSingleNode("/ns:request/ns:body/ns:orderInfo/ns:orderStatus", nsMgr).InnerText;
                        string orderPrice = xr.SelectSingleNode("/ns:request/ns:body/ns:orderInfo/ns:orderPrice", nsMgr).InnerText;
                        string paymentSerialno = xr.SelectSingleNode("/ns:request/ns:body/ns:orderInfo/ns:paymentSerialno", nsMgr).InnerText;
                        string eticketNo = "";
                        if (xr.SelectSingleNode("/ns:request/ns:body/ns:orderInfo/ns:eticketNo", nsMgr) != null)
                        {
                            eticketNo = xr.SelectSingleNode("/ns:request/ns:body/ns:orderInfo/ns:eticketNo", nsMgr).InnerText;
                        }
                        //判断重复：如果已经有当前支付订单，则不再录入
                        bool ishasrequest = new Qunar_payOrderForBeforePaySyncData().Ishasrequest(partnerOrderId);
                        if (ishasrequest)
                        {
                            TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "logid:" + rlog.id + ",payOrderForBeforePaySync:重复通知");
                        }


                        //获得去哪订单
                        string qunar_orderid = new Qunar_CreateOrderForBeforePaySyncData().GetQunarOrderId(partnerOrderId);
                        if (qunar_orderid == "")
                        {
                            string err_response = GetErrResponse(bodyType, "13001", "订单不存在", signkey);
                            context.Response.Write(err_response);

                            rlog.msg = "去哪订单不存在(原订单" + partnerOrderId + ")";
                            rlog.qunar_orderId = qunar_orderid;
                            new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                            return;
                        }
                        #region 判断重复：如果已经成功处理了订单则不再处理
                        bool ishassuc = new Qunar_ms_requestlogData().IsHasSuc("payOrderForBeforePaySync", qunar_orderid);
                        if (ishassuc)
                        {

                            B2b_order rmodelb2border = new B2bOrderData().GetOrderById(partnerOrderId.ConvertTo<int>(0));
                            //根据订单号得到电子票 
                            string Rpno = new B2bOrderData().GetPnoByOrderId(int.Parse(partnerOrderId));
                            if (rmodelb2border.Bindingagentorderid > 0)
                            {
                                Rpno = new B2bOrderData().GetPnoByOrderId(rmodelb2border.Bindingagentorderid);
                            }

                            string RorderStatus = "PREPAY_ORDER_PRINT_SUCCESS";
                            string RresponseParam = GetpayOrderForBeforePaySyncresponse("1000", "suc", int.Parse(partnerOrderId), RorderStatus, Rpno, signkey);
                            context.Response.Write(RresponseParam);

                            rlog.msg = rlog.msg + "suc";
                            rlog.qunar_orderId = qunar_orderid;
                            new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                            return;
                        }
                        #endregion

                        int insqunar_request = new Qunar_payOrderForBeforePaySyncData().InsQunar_payOrderForBeforePaySync_request(partnerOrderId, orderStatus, orderPrice, paymentSerialno, eticketNo);
                        if (insqunar_request == 0)
                        {
                            TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "logid:" + rlog.id + ",payOrderForBeforePaySync insqunar_request:err");

                        }
                        #endregion

                        #region 处理
                        B2b_order modelb2border = new B2bOrderData().GetOrderById(partnerOrderId.ConvertTo<int>(0));
                        if (modelb2border != null)
                        {
                            B2b_pay modelb2pay = new B2bPayData().GetPayByoId(int.Parse(partnerOrderId));
                            if (modelb2pay == null)
                            {
                                //向支付表b2b_pay插入一条记录
                                B2b_pay eticket = new B2b_pay()
                                {
                                    Id = 0,
                                    Oid = int.Parse(partnerOrderId),
                                    Pay_com = "qunar",
                                    Pay_name = modelb2border.U_name,
                                    Pay_phone = modelb2border.U_phone,
                                    Total_fee = decimal.Parse(orderPrice) / 100,
                                    Trade_no = "",
                                    Trade_status = "trade_pendpay",
                                    Uip = "",
                                    comid = modelb2border.Comid
                                };
                                int payid = new B2bPayData().InsertOrUpdate(eticket);
                            }

                            string retunstr = new PayReturnSendEticketData().PayReturnSendEticket(paymentSerialno, int.Parse(partnerOrderId), decimal.Parse(orderPrice) / 100, "TRADE_SUCCESS", "");

                            //根据订单号得到电子票 
                            string pno = new B2bOrderData().GetPnoByOrderId(int.Parse(partnerOrderId));
                            if (modelb2border.Bindingagentorderid > 0)
                            {
                                pno = new B2bOrderData().GetPnoByOrderId(modelb2border.Bindingagentorderid);
                            }

                            if (pno == "")
                            {
                                string err_response = GetErrResponse(bodyType, "1", "意外错误", signkey);
                                context.Response.Write(err_response);

                                rlog.msg = "根据订单号得到电子票:err--" + pno;
                                rlog.qunar_orderId = qunar_orderid;
                                new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                                return;
                            }
                            orderStatus = "PREPAY_ORDER_PRINT_SUCCESS";
                            string responseParam = GetpayOrderForBeforePaySyncresponse("1000", "suc", int.Parse(partnerOrderId), orderStatus, pno, signkey);
                            context.Response.Write(responseParam);

                            string r_data = GetFromBase64(responseParam);
                            int insqunar_response = new Qunar_payOrderForBeforePaySyncData().InsQunar_payOrderForBeforePaySync_response(partnerOrderId, orderStatus, pno, responseParam + "---" + r_data);
                            if (insqunar_response == 0)
                            {
                                TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "logid:" + rlog.id + ",payOrderForBeforePaySync insqunar_response:err");
                            }

                            //标注通知处理成功
                            rlog.msg = "suc";
                            rlog.qunar_orderId = qunar_orderid;
                            new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                            return;
                        }
                        else
                        {
                            //不存在订单
                            string err_response = GetErrResponse(bodyType, "1", "意外错误", signkey);
                            context.Response.Write(err_response);

                            rlog.msg += "logid:" + rlog.id + ",不存在订单";
                            rlog.qunar_orderId = qunar_orderid;
                            new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                            return;
                        }
                        #endregion
                    }
                    #endregion

                    #region Qunar退款通知
                    if (method == "noticeOrderRefundedByQunar")
                    {
                        #region body主体消息
                        string partnerorderId = xr.SelectSingleNode("/ns:request/ns:body/ns:orderInfo/ns:partnerorderId", nsMgr).InnerText;
                        string refundSeq = xr.SelectSingleNode("/ns:request/ns:body/ns:orderInfo/ns:refundSeq", nsMgr).InnerText;
                        string orderQuantity = xr.SelectSingleNode("/ns:request/ns:body/ns:orderInfo/ns:orderQuantity", nsMgr).InnerText;
                        string refundQuantity = xr.SelectSingleNode("/ns:request/ns:body/ns:orderInfo/ns:refundQuantity", nsMgr).InnerText;
                        string orderPrice = xr.SelectSingleNode("/ns:request/ns:body/ns:orderInfo/ns:orderPrice", nsMgr).InnerText;
                        string refundReason = xr.SelectSingleNode("/ns:request/ns:body/ns:orderInfo/ns:refundReason", nsMgr).InnerText;
                        string refundExplain = xr.SelectSingleNode("/ns:request/ns:body/ns:orderInfo/ns:refundExplain", nsMgr).InnerText;
                        string refundJudgeMark = xr.SelectSingleNode("/ns:request/ns:body/ns:orderInfo/ns:refundJudgeMark", nsMgr).InnerText;
                        string orderRefundCharge = xr.SelectSingleNode("/ns:request/ns:body/ns:orderInfo/ns:orderRefundCharge", nsMgr).InnerText;

                        //判断重复：如果已经有当前退款通知，则不再录入
                        bool ishasrequest = new Qunar_noticeOrderRefundedByQunarData().Ishasrequest(partnerorderId);
                        if (ishasrequest)
                        {
                            TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "logid:" + rlog.id + ",noticeOrderRefundedByQunar ishasrequest:重复通知");
                        }


                        //获得去哪订单
                        string qunar_orderid = new Qunar_CreateOrderForBeforePaySyncData().GetQunarOrderId(partnerorderId);
                        if (qunar_orderid == "")
                        {
                            string err_response = GetErrResponse(bodyType, "13001", "去哪订单不存在", signkey);
                            context.Response.Write(err_response);

                            rlog.msg = rlog.msg + ",去哪订单不存在";
                            rlog.qunar_orderId = "";
                            new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                            return;
                        }
                        //#region 判断重复：如果已经成功处理了订单则不再处理(去哪支持一个订单多次退，所以此部去掉)
                        //bool ishassuc = new Qunar_ms_requestlogData().IsHasSuc("noticeOrderRefundedByQunar", qunar_orderid);
                        //if (ishassuc)
                        //{
                        //    string Rmessage = "suc";
                        //    string RresponseParam = GetnoticeOrderRefundedByQunarresponse("1000", "suc", Rmessage, signkey);
                        //    context.Response.Write(RresponseParam);

                        //    rlog.msg = "suc";
                        //    rlog.qunar_orderId = qunar_orderid;
                        //    new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                        //    return;
                        //}
                        //#endregion

                        int insqunar_request = new Qunar_noticeOrderRefundedByQunarData().Insrequest(partnerorderId, refundSeq, orderQuantity, refundQuantity, orderPrice, refundReason, refundExplain, refundJudgeMark, orderRefundCharge);
                        if (insqunar_request == 0)
                        {
                            TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "logid:" + rlog.id + ",noticeOrderRefundedByQunar insqunar_request:err");
                        }
                        #endregion

                        #region  处理
                        B2b_order morder = new B2bOrderData().GetOrderById(int.Parse(partnerorderId));
                        //判断b订单是否在20160121前由于去哪儿录入产品号错误 产生的错误订单中(147613,147849,148717,148813,148815,148819,148840,148846,148863,148889,149251,149779,149906,150088,150116,150207,150391,150412,150467,150482,150501,150515,150526,150529,150530,150533,150535,150587,150599,150661,150837,150866,151408,151538,151777,151870,152052,154026,154751,155122,155858,156112,156419,160826,162089,163317)，
                        //如果在的话用b订单号，否则用a订单号，这个判断需要保留,直到狼牙山上面的46个订单都处理完成后才可删除，谨记!!! by xiaoliu
                        int[] intqud = { 147613, 147849, 148717, 148813, 148815, 148819, 148840, 148846, 148863, 148889, 149251, 149779, 149906, 150088, 150116, 150207, 150391, 150412, 150467, 150482, 150501, 150515, 150526, 150529, 150530, 150533, 150535, 150587, 150599, 150661, 150837, 150866, 151408, 151538, 151777, 151870, 152052, 154026, 154751, 155122, 155858, 156112, 156419, 160826, 162089, 163317 };
                        if (intqud.Contains(int.Parse(partnerorderId)))
                        {
                            //需要判断订单 是否为 导入产品的订单
                            int initorderid = new B2bOrderData().Getinitorderid(int.Parse(partnerorderId));
                            if (initorderid > 0)
                            {
                                morder = new B2bOrderData().GetOrderById(initorderid);
                            }
                        }


                        var paydate = new B2bPayData();
                        decimal Total_fee = 0;

                        if (morder != null)
                        {
                            ////等把狼牙山去哪儿账户的财务弄清楚后，加限制意义不大，因为只是一个通知过程，返回失败也不起作用
                            //if(morder.Comid!=company_qunar.ID)
                            //{
                            //    rlog.msg = rlog.msg + ",去哪订单在商户下不存在";
                            //    rlog.qunar_orderId = qunar_orderid;
                            //    new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                            //    return;
                            //}

                            //判断b订单是否在20160121前由于去哪儿录入产品号错误 产生的错误订单中(147613,147849,148717,148813,148815,148819,148840,148846,148863,148889,149251,149779,149906,150088,150116,150207,150391,150412,150467,150482,150501,150515,150526,150529,150530,150533,150535,150587,150599,150661,150837,150866,151408,151538,151777,151870,152052,154026,154751,155122,155858,156112,156419,160826,162089,163317)，
                            //如果在的话用b订单号，否则用a订单号，这个判断需要保留,直到狼牙山上面的46个订单都处理完成后才可删除，谨记!!! by xiaoliu
                            if (!intqud.Contains(int.Parse(partnerorderId)))
                            {
                                Total_fee = paydate.GetPayByoId(morder.Id) == null ? 0 : paydate.GetPayByoId(morder.Id).Total_fee;

                                if (decimal.Parse(orderPrice) / 100 <= Total_fee)
                                {
                                    decimal refundprice = morder.Pay_price * decimal.Parse(refundQuantity);
                                    morder.Ticket = refundprice / 100;
                                }
                                else
                                {
                                    string err_response = GetErrResponse(bodyType, "15001", "金额有误，退款金额不能大于支付金额", signkey);
                                    context.Response.Write(err_response);


                                    rlog.msg = "金额有误，退款金额不能大于支付金额";
                                    rlog.qunar_orderId = qunar_orderid;
                                    new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);

                                    return;
                                }
                            }
                        }
                        else
                        {
                            string err_response = GetErrResponse(bodyType, "13001", "订单不存在", signkey);
                            context.Response.Write(err_response);


                            rlog.msg = "订单不存在";
                            rlog.qunar_orderId = qunar_orderid;
                            new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                            return;
                        }
                        string quitstr = OrderJsonData.DirectSell_Refundticket(morder, int.Parse(refundQuantity));
                        quitstr = "{\"root\":" + quitstr + "}";
                        XmlDocument xxd = JsonConvert.DeserializeXmlNode(quitstr);
                        string type = xxd.SelectSingleNode("root/type").InnerText;
                        string msg = xxd.SelectSingleNode("root/msg").InnerText;

                        string code = "";
                        string describe = "";
                        string rmessage = "";
                        if (type == "100")
                        {
                            code = "1000";
                            describe = "suc";
                            rmessage = "suc";

                            string responseParam = GetnoticeOrderRefundedByQunarresponse(code, describe, rmessage, signkey);
                            context.Response.Write(responseParam);

                            //标注通知处理成功
                            rlog.msg = "suc";
                            rlog.qunar_orderId = qunar_orderid;
                            new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);

                            string r_data = GetFromBase64(responseParam);
                            int insqunar_response = new Qunar_noticeOrderRefundedByQunarData().Insresponse(partnerorderId, rmessage, responseParam + "---" + r_data);
                            if (insqunar_response == 0)
                            {
                                TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "logid:" + rlog.id + ",noticeOrderRefundedByQunar insqunar_response:err");
                            }
                        }
                        else
                        {
                            code = "15003";
                            describe = msg;
                            rmessage = msg;

                            string responseParam = GetErrResponse(bodyType, code, describe, signkey);
                            context.Response.Write(responseParam);

                            //标注通知处理失败
                            rlog.msg = msg;
                            rlog.qunar_orderId = qunar_orderid;
                            new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                        }



                        #endregion
                    }
                    #endregion

                    //#region 同步订单（用于支付前下单）-取消订单；去哪订单信息变动(主要是取票人信息 和 游玩人信息) pushOrderForBeforePaySync
                    //if (method == "pushOrderForBeforePaySync")
                    //{
                    //    #region body主体消息


                    //    string partnerorderId = xr.SelectSingleNode("/ns:request/ns:body/ns:partnerorderId", nsMgr).InnerText;
                    //    string c_mobile = xr.SelectSingleNode("/ns:request/ns:body/ns:cancelReason/ns:mobile", nsMgr).InnerText;

                    //    //获得去哪订单
                    //    string qunar_orderid = new Qunar_CreateOrderForBeforePaySyncData().GetQunarOrderId(partnerorderId);
                    //    if (qunar_orderid == "")
                    //    {
                    //        string err_response = GetErrResponse(bodyType, "13001", "订单不存在", signkey);
                    //        context.Response.Write(err_response);


                    //        rlog.msg = "获得去哪订单err";
                    //        rlog.qunar_orderId = "";
                    //        new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                    //        return;
                    //    }
                    //    #region 取消订单(标注为超时订单)
                    //    if (xr.SelectSingleNode("/ns:request/ns:body/ns:orderStatus", nsMgr).InnerText != "")//取消订单 
                    //    {

                    //        string orderStatus = xr.SelectSingleNode("/ns:request/ns:body/ns:orderStatus", nsMgr).InnerText;
                    //        string cancelreason = "";
                    //        if (xr.SelectSingleNode("/ns:request/ns:body/ns:cancelReason", nsMgr) != null)
                    //        {
                    //            cancelreason = xr.SelectSingleNode("/ns:request/ns:body/ns:cancelReason", nsMgr).InnerText;
                    //        }
                    //        //录入请求信息
                    //        int insrequest = new Qunar_pushOrderForBeforePaySyncData().InsRequest_cancel(partnerorderId, orderStatus, cancelreason, c_mobile);
                    //        if (insrequest == 0)
                    //        {
                    //            TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "logid:" + rlog.id + ",pushOrderForBeforePaySync insrequest:err");
                    //        }
                    //        //预付：订单已取消
                    //        if (orderStatus == "PREPAY_ORDER_CANCEL")
                    //        {
                    //            //直销订单状态
                    //            int orderstatu = new B2bOrderData().GetOrderState(partnerorderId);

                    //            if (orderstatu == (int)OrderStatus.InvalidOrder)
                    //            {
                    //                string responseparam = GetpushOrderForBeforePaySyncresponse("1000", "suc", partnerorderId, orderStatus, signkey);
                    //                context.Response.Write(responseparam);

                    //                rlog.msg = "suc";
                    //                rlog.qunar_orderId = qunar_orderid;
                    //                new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                    //                return;
                    //            }

                    //            //取消直销订单(设置超时订单)-----------------
                    //            bool cancelOrder = new B2bOrderData().CancelOrder(partnerorderId);

                    //            if (cancelOrder)
                    //            {
                    //                string responseparam = GetpushOrderForBeforePaySyncresponse("1000", "suc", partnerorderId, orderStatus, signkey);
                    //                int insresponse = new Qunar_pushOrderForBeforePaySyncData().InsResponse(partnerorderId, "", responseparam, insrequest);

                    //                context.Response.Write(responseparam);

                    //                rlog.msg = "suc";
                    //                rlog.qunar_orderId = qunar_orderid;
                    //                new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                    //                return;
                    //            }
                    //            else
                    //            {
                    //                string err_response = GetErrResponse(bodyType, "1", "取消直销订单出错", signkey);
                    //                context.Response.Write(err_response);


                    //                rlog.msg = "订单不存在";
                    //                rlog.qunar_orderId = qunar_orderid;
                    //                new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                    //                return;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            string err_response = GetErrResponse(bodyType, "1", "未知订单状态", signkey);
                    //            context.Response.Write(err_response);


                    //            rlog.msg = "未知订单状态";
                    //            rlog.qunar_orderId = qunar_orderid;
                    //            new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                    //            return;
                    //        }

                    //    }
                    //    #endregion
                    //    #region 取票人信息/游玩人信息修改
                    //    else //取票人信息/游玩人信息修改
                    //    {
                    //        string cancelReason = xr.SelectSingleNode("/ns:request/ns:body/ns:cancelReason", nsMgr).InnerText;
                    //        //联系人信息
                    //        XmlNode contactPerson = xr.SelectSingleNode("/ns:request/ns:body/ns:contactPerson", nsMgr);
                    //        string c_name = contactPerson.SelectSingleNode("/ns:name", nsMgr).InnerText;
                    //        string c_namePinyin = contactPerson.SelectSingleNode("/ns:namePinyin", nsMgr).InnerText;
                    //        c_mobile = contactPerson.SelectSingleNode("/ns:mobile", nsMgr).InnerText;
                    //        string c_email = contactPerson.SelectSingleNode("/ns:email", nsMgr).InnerText;
                    //        string c_address = contactPerson.SelectSingleNode("/ns:address", nsMgr).InnerText;
                    //        string c_zipCode = contactPerson.SelectSingleNode("/ns:zipCode", nsMgr).InnerText;
                    //        //订单备注
                    //        string orderRemark = xr.SelectSingleNode("/ns:request/ns:body/ns:orderRemark", nsMgr).InnerText;

                    //        //录入请求信息
                    //        int insrequest1 = new Qunar_pushOrderForBeforePaySyncData().InsRequest_contactperson(partnerorderId, c_name, c_namePinyin, c_mobile, c_email, c_address, c_zipCode, orderRemark);
                    //        if (insrequest1 == 0)
                    //        {
                    //            TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "logid:" + rlog.id + ",pushOrderForBeforePaySync insrequest1:err");
                    //        }

                    //        #region  出游人信息
                    //        if (xr.SelectSingleNode("/ns:request/ns:body/ns:visitPerson", nsMgr) != null)
                    //        {
                    //            XmlNode visitPerson = xr.SelectSingleNode("/ns:request/ns:body/ns:visitPerson", nsMgr);
                    //            XmlNodeList v_personlist = visitPerson.SelectNodes("/ns:person", nsMgr);
                    //            foreach (XmlNode xn in v_personlist)
                    //            {
                    //                string v_name = xn.SelectSingleNode("/ns:name", nsMgr).InnerText;
                    //                string v_namePinyin = xn.SelectSingleNode("/ns:namePinyin", nsMgr).InnerText;
                    //                string v_credentials = xn.SelectSingleNode("/ns:credentials", nsMgr).InnerText;
                    //                string v_credentialsType = xn.SelectSingleNode("/ns:credentialsType", nsMgr).InnerText;
                    //                string v_defined1Value = xn.SelectSingleNode("/ns:defined1Value", nsMgr).InnerText;
                    //                string v_defined2Value = xn.SelectSingleNode("/ns:defined2Value", nsMgr).InnerText;

                    //                //录入请求信息
                    //                int insrequest2 = new Qunar_pushOrderForBeforePaySyncvisitpersonData().InsRequest(v_name, v_namePinyin, v_credentials, v_credentialsType, v_defined1Value, v_defined2Value, insrequest1);
                    //                if (insrequest2 == 0)
                    //                {
                    //                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "logid:" + rlog.id + ",pushOrderForBeforePaySync insrequest2:err");
                    //                }
                    //            }
                    //        }
                    //        #endregion

                    //        //修改订单中预订人信息
                    //        int uporder = new B2bOrderData().UpdateOrder_contactperson(partnerorderId, c_name, c_mobile);
                    //        if (uporder > 0)
                    //        {
                    //            string responseparam = GetpushOrderForBeforePaySyncresponse("1000", "suc", partnerorderId, "", signkey);

                    //            string r_data = GetFromBase64(responseparam);
                    //            int insresponse = new Qunar_pushOrderForBeforePaySyncData().InsResponse(partnerorderId, "", responseparam+"---"+r_data, insrequest1);
                    //            context.Response.Write(responseparam);

                    //            rlog.msg = "suc";
                    //            rlog.qunar_orderId = qunar_orderid;
                    //            new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                    //            return;
                    //        }
                    //        else
                    //        {
                    //            string err_response = GetErrResponse(bodyType, "1", "修改取票人录入请求信息失败", signkey);
                    //            context.Response.Write(err_response);

                    //            rlog.msg = rlog.msg + ",修改取票人录入请求信息失败";
                    //            rlog.qunar_orderId = qunar_orderid;
                    //            new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                    //            return;
                    //        }

                    //    }
                    //    #endregion

                    //    #endregion
                    //}
                    //#endregion

                    //#region 订单已返现通知
                    //if (method == "noticeOrderAlreadyCashBack")
                    //{

                    //    string partnerorderId = xr.SelectSingleNode("/ns:request/ns:body/ns:orderInfo/ns:partnerorderId", nsMgr).InnerText;
                    //    string orderCashBackMoney = xr.SelectSingleNode("/ns:request/ns:body/ns:orderInfo/ns:refundSeq", nsMgr).InnerText;

                    //    //判断重复：如果已经有通知，则不再录入
                    //    bool ishasrequest = new Qunar_noticeOrderAlreadyCashBackData().Ishasrequest(partnerorderId);
                    //    if (ishasrequest)
                    //    {
                    //        TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "logid:" + rlog.id + ",noticeOrderAlreadyCashBack ishasrequest:重复通知");
                    //    }

                    //    //获得去哪订单
                    //    string qunar_orderid = new Qunar_CreateOrderForBeforePaySyncData().GetQunarOrderId(partnerorderId);
                    //    if (qunar_orderid == "")
                    //    {
                    //        string err_response = GetErrResponse(bodyType, "13001", "订单不存在", signkey);
                    //        context.Response.Write(err_response);


                    //        rlog.msg = "获得去哪订单err";
                    //        rlog.qunar_orderId = "";
                    //        new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                    //        return;
                    //    }
                    //    //录入请求信息
                    //    int insrequest = new Qunar_noticeOrderAlreadyCashBackData().Insrequest(partnerorderId, orderCashBackMoney);
                    //    if (insrequest == 0)
                    //    {
                    //        TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\qunarerr.txt", "logid:" + rlog.id + ",noticeOrderAlreadyCashBack insrequest:err");
                    //    }

                    //    //把去哪返现金额录入订单表
                    //    int upqunarcashback = new B2bOrderData().Upqunarcashback(partnerorderId, orderCashBackMoney);
                    //    if (upqunarcashback > 0)
                    //    {
                    //        string message = "收到";
                    //        string responseparam = GetnoticeOrderAlreadyCashBackresponse("1000", "suc", message, signkey);

                    //        string r_data = GetFromBase64(responseparam);
                    //        int insresponse = new Qunar_noticeOrderAlreadyCashBackData().InsResponse(partnerorderId, message, responseparam+"---"+r_data);

                    //        context.Response.Write(responseparam);

                    //        rlog.msg = "suc";
                    //        rlog.qunar_orderId = qunar_orderid;
                    //        new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                    //        return;
                    //    }
                    //    else
                    //    {
                    //        string err_response = GetErrResponse(bodyType, "1", "把去哪返现金额录入订单表err", signkey);
                    //        context.Response.Write(err_response);


                    //        rlog.msg = "把去哪返现金额录入订单表err";
                    //        rlog.qunar_orderId = qunar_orderid;
                    //        new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);
                    //        return;
                    //    }


                    //}
                    //#endregion
                }
                else
                {
                    rlog.msg = rlog.msg + ",签名验证出错";
                    new Qunar_ms_requestlogData().EditQunar_ms_requestlog(rlog);

                    context.Response.Write("签名验证出错");
                }


            }
        }

        private string GetFromBase64(string responseParam)
        {
            string d = "{\"root\":" + responseParam + "}";
            XmlDocument doc1c = JsonConvert.DeserializeXmlNode(d);
            XmlElement rootc = doc1c.DocumentElement;
            string datac = rootc.SelectSingleNode("data").InnerText;
            //string securityTypec = rootc.SelectSingleNode("securityType").InnerText;
            //string signedc = rootc.SelectSingleNode("signed").InnerText;

            //base64解密
            datac = Encoding.UTF8.GetString(EncryptionExtention.FromBase64(datac));
            return datac;
        }

        private string GetnoticeOrderAlreadyCashBackresponse(string code, string describe, string message, string signkey)
        {
            string responseserxml = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                            "<response xsi:schemaLocation=\"http://piao.qunar.com/2013/QMenpiaoResponseSchema QMResponseDataSchema-2.0.1.xsd\" xmlns=\"http://piao.qunar.com/2013/QMenpiaoResponseSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                                                "<header>" +
                                                    "<application>Qunar.Menpiao.Agent</application>" +
                                                    "<processor>SupplierDataExchangeProcessor</processor>" +
                                                    "<version>v2.0.1</version>" +
                                                    "<bodyType>NoticeOrderAlreadyCashBackResponseBody</bodyType>" +
                                                    "<createUser>SupplierSystemName</createUser>" +
                                                    "<createTime>{0}</createTime>" +
                                                    "<code>{1}</code>" +
                                                    "<describe>{2}</describe>" +
                                                "</header>" +
                                                "<body xsi:type=\"NoticeOrderAlreadyCashBackResponseBody\">" +
                                                    "<message>{3}</message>" +
                                                "</body>" +
                                            "</response>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), code, describe, message);
            responseserxml = EncryptionExtention.ToBase64(responseserxml);

            string responsesecurityType = "MD5";
            string responsesigned = EncryptionHelper.ToMD5(signkey + responseserxml, "utf-8");

            string responseParam = "{\"data\":\"" + responseserxml + "\",\"signed\":\"" + responsesigned + "\",\"securityType\":\"" + responsesecurityType + "\"}";
            return responseParam;
        }

        private string GetErrResponse(string bodyType, string code, string describe, string signkey)
        {
            string responseserxml = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                     "<response xsi:schemaLocation=\"http://piao.qunar.com/2013/QMenpiaoResponseSchema QMResponseDataSchema-2.0.1.xsd\" xmlns=\"http://piao.qunar.com/2013/QMenpiaoResponseSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                                      "<header>" +
                                          "<application>Qunar.Menpiao.Agent</application>" +
                                          "<processor>SupplierDataExchangeProcessor</processor>" +
                                          "<version>v2.0.1</version>" +
                                          "<bodyType>{0}</bodyType>" +
                                          "<createUser>SupplierSystemName</createUser>" +
                                          "<createTime>{1}</createTime>" +
                                          "<code>{2}</code>" +
                                          "<describe>{3}</describe>" +
                                      "</header>" +
                                      "<body xsi:type=\"{4}\">" +
                                      "</body>" +
                                  "</response>", bodyType, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), code, describe, bodyType);
            responseserxml = EncryptionExtention.ToBase64(responseserxml);

            string responsesecurityType = "MD5";
            string responsesigned = EncryptionHelper.ToMD5(signkey + responseserxml, "utf-8");

            string responseParam = "{\"data\":\"" + responseserxml + "\",\"signed\":\"" + responsesigned + "\",\"securityType\":\"" + responsesecurityType + "\"}";
            return responseParam;
        }

        private string GetpushOrderForBeforePaySyncresponse(string code, string describe, string partnerorderId, string orderStatus, string signkey)
        {
            string responseserxml = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                    "<!--Sample XML file generated by XMLSpy v2013 (http://www.altova.com)-->" +
                                    "<response xsi:schemaLocation=\"http://piao.qunar.com/2013/QMenpiaoResponseSchema QMResponseDataSchema-2.0.1.xsd\" xmlns=\"http://piao.qunar.com/2013/QMenpiaoResponseSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                                        "<header>" +
                                            "<application>Qunar.Menpiao.Agent</application>" +
                                            "<processor>SupplierDataExchangeProcessor</processor>" +
                                            "<version>v2.0.1</version>" +
                                            "<bodyType>PushOrderForBeforePaySyncResponseBody</bodyType>" +
                                            "<createUser>SupplierSystemName</createUser>" +
                                            "<createTime>{0}</createTime>" +
                                            "<code>{1}</code>" +
                                            "<describe>{2}</describe>" +
                                        "</header>" +
                                        "<body xsi:type=\"PushOrderForBeforePaySyncResponseBody\">" +
                                            "<orderInfo>" +
                                                "<partnerorderId>{3}</partnerorderId>" +
                                                "<orderStatus>{4}</orderStatus>" +
                                            "</orderInfo>" +
                                        "</body>" +
                                    "</response>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), code, describe, partnerorderId, orderStatus);
            responseserxml = EncryptionExtention.ToBase64(responseserxml);

            string responsesecurityType = "MD5";
            string responsesigned = EncryptionHelper.ToMD5(signkey + responseserxml, "utf-8");

            string responseParam = "{\"data\":\"" + responseserxml + "\",\"signed\":\"" + responsesigned + "\",\"securityType\":\"" + responsesecurityType + "\"}";
            return responseParam;
        }

        private string GetnoticeOrderRefundedByQunarresponse(string code, string describe, string Rmessage, string signkey)
        {
            string responseserxml = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                        "<response xsi:schemaLocation=\"http://piao.qunar.com/2013/QMenpiaoResponseSchema QMResponseDataSchema-2.0.1.xsd\" xmlns=\"http://piao.qunar.com/2013/QMenpiaoResponseSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                                            "<header>" +
                                                "<application>Qunar.Menpiao.Agent</application>" +
                                                "<processor>SupplierDataExchangeProcessor</processor>" +
                                                "<version>v2.0.1</version>" +
                                                "<bodyType>NoticeOrderRefundedByQunarResponseBody</bodyType>" +
                                                "<createUser>SupplierSystemName</createUser>" +
                                                "<createTime>{0}</createTime>" +
                                                "<code>{1}</code>" +
                                                "<describe>{2}</describe>" +
                                            "</header>" +
                                            "<body xsi:type=\"NoticeOrderRefundedByQunarResponseBody\">" +
                                                "<message>{3}</message>" +
                                            "</body>" +
                                        "</response>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), code, describe, Rmessage);
            responseserxml = EncryptionExtention.ToBase64(responseserxml);

            string responsesecurityType = "MD5";
            string responsesigned = EncryptionHelper.ToMD5(signkey + responseserxml, "utf-8");

            string responseParam = "{\"data\":\"" + responseserxml + "\",\"signed\":\"" + responsesigned + "\",\"securityType\":\"" + responsesecurityType + "\"}";
            return responseParam;
        }
        /// <summary>
        /// 支付订单（用于支付前下单） 返回
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        /// <param name="p_3"></param>
        /// <param name="orderStatus"></param>
        /// <param name="pno"></param>
        /// <param name="signkey"></param>
        /// <returns></returns>
        private string GetpayOrderForBeforePaySyncresponse(string code, string describe, int partnerOrderId, string orderStatus, string pno, string signkey)
        {
            string responseserxml = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                            "<response xsi:schemaLocation=\"http://piao.qunar.com/2013/QMenpiaoResponseSchema QMResponseDataSchema-2.0.1.xsd\" xmlns=\"http://piao.qunar.com/2013/QMenpiaoResponseSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                                "<header>" +
                                    "<application>Qunar.Menpiao.Agent</application>" +
                                    "<processor>PayOrderForBeforePaySyncResponseBody</processor>" +
                                    "<version>v2.0.1</version>" +
                                    "<bodyType>SendOrderEticketResponseBody</bodyType>" +
                                    "<createUser>SupplierSystemName</createUser>" +
                                    "<createTime>{0}</createTime>" +
                                       "<code>{1}</code>" +
                                       "<describe>{2}</describe>" +
                                   "</header>" +
                                   "<body xsi:type=\"PayOrderForBeforePaySyncResponseBody\">" +
                                       "<orderInfo>" +
                                             "<partnerorderId>{3}</partnerorderId>" +
                                            "<orderStatus>{4}</orderStatus>" +
                                            "<eticketNo>{5}</eticketNo>" +
                                       "</orderInfo>" +
                                   "</body>" +
                               "</response>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), code, describe, partnerOrderId, orderStatus, pno);
            responseserxml = EncryptionExtention.ToBase64(responseserxml);

            string responsesecurityType = "MD5";
            string responsesigned = EncryptionHelper.ToMD5(signkey + responseserxml, "utf-8");

            string responseParam = "{\"data\":\"" + responseserxml + "\",\"signed\":\"" + responsesigned + "\",\"securityType\":\"" + responsesecurityType + "\"}";
            return responseParam;
        }
        /// <summary>
        ///  qunar获取订单信息 返回
        /// </summary>
        /// <param name="signkey"></param>
        /// <param name="partnerOrderId"></param>
        /// <param name="orderstatus"></param>
        /// <param name="orderQuantity"></param>
        /// <param name="eticketNo"></param>
        /// <param name="eticketSended"></param>
        /// <param name="total_consumenum"></param>
        /// <param name="consumeInfo"></param>
        /// <returns></returns>
        private string GetQunar_OrderByQunarResponse(string code, string describe, string signkey, string partnerOrderId, string orderstatus, string orderQuantity, string eticketNo, string eticketSended, int total_consumenum, string consumeInfo)
        {
            string responseserxml = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                               "<response xsi:schemaLocation=\"http://piao.qunar.com/2013/QMenpiaoResponseSchema QMResponseDataSchema-2.0.1.xsd\" xmlns=\"http://piao.qunar.com/2013/QMenpiaoResponseSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                                   "<header>" +
                                       "<application>Qunar.Menpiao.Agent</application>" +
                                       "<processor>SupplierDataExchangeProcessor</processor>" +
                                       "<version>v2.0.1</version>" +
                                       "<bodyType>GetOrderByQunarResponseBody</bodyType>" +
                                       "<createUser>SupplierSystemName</createUser>" +
                                       "<createTime>{0}</createTime>" +
                                          "<code>{1}</code>" +
                                          "<describe>{2}</describe>" +
                                      "</header>" +
                                      "<body xsi:type=\"GetOrderByQunarResponseBody\">" +
                                          "<orderInfo>" +
                                                "<partnerorderId>{3}</partnerorderId>" +
                                                "<orderStatus>{4}</orderStatus>" +
                                                "<orderQuantity>{5}</orderQuantity>" +
                                                "<eticketNo>{6}</eticketNo>" +
                                                "<eticketSended>{7}</eticketSended>" +
                                                "<useQuantity>{8}</useQuantity>" +
                                                "<consumeInfo>{9}</consumeInfo>" +
                                          "</orderInfo>" +
                                      "</body>" +
                                  "</response>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), code, describe, partnerOrderId, orderstatus, orderQuantity, eticketNo, eticketSended, total_consumenum, consumeInfo);
            responseserxml = EncryptionExtention.ToBase64(responseserxml);

            string responsesecurityType = "MD5";
            string responsesigned = EncryptionHelper.ToMD5(signkey + responseserxml, "utf-8");

            string responseParam = "{\"data\":\"" + responseserxml + "\",\"signed\":\"" + responsesigned + "\",\"securityType\":\"" + responsesecurityType + "\"}";
            return responseParam;
        }
        /// <summary>
        /// 创建订单（支付前）  返回
        /// </summary>
        /// <param name="code"></param>
        /// <param name="describe"></param>
        /// <param name="parterorderid"></param>
        /// <param name="orderStatus"></param>
        /// <param name="signkey"></param>
        /// <returns></returns>
        private string GetCreateOrderForBeforePaySyncResponseBodyresponse(string code, string describe, int parterorderid, string orderStatus, string signkey)
        {
            #region 暂时先用笨方法实现，此种方法暂时注释掉
            //CreateOrderForBeforePaySyncResponseBodyresponse mresponse = new CreateOrderForBeforePaySyncResponseBodyresponse();
            //mresponse.header = new ResponseHeader
            //{
            //    application = "Qunar.Menpiao.Agent",
            //    processor = "SupplierDataExchangeProcessor",
            //    version = "v2.0.1",
            //    bodyType = "CreateOrderForBeforePaySyncResponseBody",
            //    createUser = "SupplierSystemName",
            //    createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            //    code = code,
            //    describe = describe,
            //};

            //mresponse.body = new CreateOrderForBeforePaySyncResponseBody
            //{
            //    orderInfo = new PM.Service.Qunar_Ms.QMResponseDataSchema.orderInfo
            //    {
            //        partnerorderId = parterorderid.ToString(),
            //        orderStatus = orderStatus
            //    }
            //};
            ////response data数据
            //string responseserxml = XmlHelper.XmlSerialize(mresponse, Encoding.UTF8);
            #endregion
            string responseserxml = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                 "<response xsi:schemaLocation=\"http://piao.qunar.com/2013/QMenpiaoResponseSchema QMResponseDataSchema-2.0.1.xsd\" xmlns=\"http://piao.qunar.com/2013/QMenpiaoResponseSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                                     "<header>" +
                                         "<application>Qunar.Menpiao.Agent</application>" +
                                         "<processor>SupplierDataExchangeProcessor</processor>" +
                                         "<version>v2.0.1</version>" +
                                         "<bodyType>CreateOrderForBeforePaySyncResponseBody</bodyType>" +
                                         "<createUser>SupplierSystemName</createUser>" +
                                         "<createTime>{0}</createTime>" +
                                         "<code>{1}</code>" +
                                         "<describe>{2}</describe>" +
                                        "</header>" +
                                        "<body xsi:type=\"CreateOrderForBeforePaySyncResponseBody\">" +
                                            "<orderInfo>" +
                                                "<partnerorderId>{3}</partnerorderId>" +
                                                "<orderStatus>{4}</orderStatus>" +
                                            "</orderInfo>" +
                                        "</body>" +
                                    "</response>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), code, describe, parterorderid, orderStatus);
            responseserxml = EncryptionExtention.ToBase64(responseserxml);

            string responsesecurityType = "MD5";
            string responsesigned = EncryptionHelper.ToMD5(signkey + responseserxml, "utf-8");

            string responseParam = "{\"data\":\"" + responseserxml + "\",\"signed\":\"" + responsesigned + "\",\"securityType\":\"" + responsesecurityType + "\"}";
            return responseParam;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public static class XmlHelper
    {
        private static void XmlSerializeInternal(Stream stream, object o, Encoding encoding)
        {
            if (o == null)
                throw new ArgumentNullException("o");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            XmlSerializer serializer = new XmlSerializer(o.GetType());

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineChars = "\r\n";
            settings.Encoding = encoding;
            settings.IndentChars = "    ";

            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, o);
                writer.Close();
            }
        }

        /// <summary>
        /// 将一个对象序列化为XML字符串
        /// </summary>
        /// <param name="o">要序列化的对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>序列化产生的XML字符串</returns>
        public static string XmlSerialize(object o, Encoding encoding)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializeInternal(stream, o, encoding);

                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 将一个对象按XML序列化的方式写入到一个文件
        /// </summary>
        /// <param name="o">要序列化的对象</param>
        /// <param name="path">保存文件路径</param>
        /// <param name="encoding">编码方式</param>
        public static void XmlSerializeToFile(object o, string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                XmlSerializeInternal(file, o, encoding);
            }
        }

        /// <summary>
        /// 从XML字符串中反序列化对象
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="s">包含对象的XML字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserialize<T>(string s, Encoding encoding)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("s");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(encoding.GetBytes(s)))
            {
                using (StreamReader sr = new StreamReader(ms, encoding))
                {
                    return (T)mySerializer.Deserialize(sr);
                }
            }
        }

        /// <summary>
        /// 读入一个文件，并按XML的方式反序列化对象。
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserializeFromFile<T>(string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            string xml = File.ReadAllText(path, encoding);
            return XmlDeserialize<T>(xml, encoding);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS.JsonFactory;
using System.Web.SessionState;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.VAS.Service.VASService.Data;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle.Enum;
using Newtonsoft.Json;
using ETS2.WeiXin.Service.WinXinService.BLL;
using ETS2.Member.Service.MemberService.Data;
using System.Xml;
using ETS2.VAS.Service.VASService.Modle;
using WxPayAPI;
using Com.Tenpay.WxpayApi.sysprogram.model;
using Com.Tenpay.WxpayApi.sysprogram.data;
using Com.Alipiay.app_code2.SysProgram.model;
using Com.Alipiay.app_code2.SysProgram.data;
using Com.Alipiay.app_code2;
using ETS2.PM.Service.Taobao_Ms.Model;
using ETS2.PM.Service.Taobao_Ms.Data;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;


namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// OrderHandler 的摘要说明
    /// </summary>
    public class OrderHandler : IHttpHandler, IRequiresSessionState
    {
        private static object lockobj = new object();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string oper = context.Request["oper"].ConvertTo<string>("");
            if (oper != "")
            {
                //处理订单
                if (oper == "HasFinOrder")
                {
                    int orderid = context.Request["orderid"].ConvertTo<int>(0);
                    string data = OrderJsonData.HasFinOrder(orderid);
                    context.Response.Write(data); 
                }
                //得到需要提交使用日期的订单的列表
                if (oper == "getNeedvisitdatePaysucorderlist")
                {
                    DateTime gooutdate = context.Request["gooutdate"].ConvertTo<DateTime>(DateTime.Now);
                    int paystate = context.Request["paystate"].ConvertTo<int>(2);
                    int proid = context.Request["proid"].ConvertTo<int>(0);
                    string orderstate = context.Request["orderstate"].ConvertTo<string>("");


                    string data = OrderJsonData.getNeedvisitdatePaysucorderlist(gooutdate,proid,paystate,orderstate);
                    context.Response.Write(data); 
                }
                //需要提交使用日期的产品的销售统计
                if (oper == "needvisitdateordercountbyday")
                {
                    DateTime startdate = context.Request["startdate"].ConvertTo<DateTime>(DateTime.Now);
                    DateTime enddate = context.Request["enddate"].ConvertTo<DateTime>(DateTime.Now);
                    int servertype = context.Request["servertype"].ConvertTo<int>(1);
                    int comid = context.Request["comid"].ConvertTo<int>(0);


                    string data = OrderJsonData.needvisitdateordercountbyday(startdate, enddate, servertype, comid);
                    context.Response.Write(data); 
                }
                if (oper == "checkPaystate")
                {
                    int orderid = context.Request["orderid"].ConvertTo<int>(0);

                    string date = OrderJsonData.checkPaystate(orderid);
                    context.Response.Write(date);
                }
                //本机测试时需要的随机时间编号
                if (oper == "getrandomtimestr")
                {
                    string timerandomstr = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    context.Response.Write("{\"type\":\"100\",\"msg\":\"" + timerandomstr + "\"}");
                    return;
                }
                if (oper == "getcompanylist")
                {
                    //是否是通过接口售票的商家
                    string isapicompany = context.Request["isapicompany"].ConvertTo<string>("0,1");

                    string date = OrderJsonData.Getcompanylist(isapicompany);
                    context.Response.Write(date);
                }
                if (oper == "getagentlist")
                {
                    //是否是通过接口出票的分销商
                    string isapiagent = context.Request["isapiagent"].ConvertTo<string>("0,1");

                    string date = OrderJsonData.Getagentlist(isapiagent);
                    context.Response.Write(date);
                }
                //补发通知
                if (oper == "reissueNotice")
                {
                    int noticelogid = context.Request["noticelogid"].ConvertTo<int>(0);

                    string date = OrderJsonData.reissueNotice(noticelogid);
                    context.Response.Write(date);
                }
                //通过验证id得到通知列表
                if (oper == "getNoticesByYzlogid")
                {
                    int b2b_etcket_logid = context.Request["b2b_etcket_logid"].ConvertTo<int>(0);

                    string date = OrderJsonData.getNoticesByYzlogid(b2b_etcket_logid);
                    context.Response.Write(date);
                }
                //得到验证通知日志列表
                if (oper == "yznoticeloglist")
                {
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(12);
                    string startime = context.Request["startime"].ConvertTo<string>("");
                    string key = context.Request["key"].ConvertTo<string>("");
                    int agentcomid = context.Request["agentcomid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string date = OrderJsonData.Yznoticeloglist(pageindex, pagesize, startime, key, agentcomid, comid);
                    context.Response.Write(date);
                }
                //给送车人发送乘车人名单列表
                if (oper == "SendTrvalNamelist")
                {
                    //根据订单状态查询 旅游大巴订单子表 --乘客表
                    string gooutdate = context.Request["gooutdate"].ConvertTo<string>("");
                    int proid = context.Request["proid"].ConvertTo<int>(0);
                    int paystate = context.Request["paystate"].ConvertTo<int>(0);
                    string orderstate = context.Request["orderstate"].ConvertTo<string>("");
                    string telphone = context.Request["telphone"].ConvertTo<string>("");
                    string firststationtime = context.Request["firststationtime"].ConvertTo<string>("");

                    string date = OrderJsonData.SendTrvalNamelist(gooutdate, proid, paystate, orderstate, telphone, firststationtime);
                    context.Response.Write(date);
                }
                //重置旅游大巴群发提醒短信
                if (oper == "travelbusQunfaRemindSmsReset")
                {
                    string gooutdate = context.Request["gooutdate"].ConvertTo<string>("");
                    int proid = context.Request["proid"].ConvertTo<int>(0);
                    ;

                    string date = OrderJsonData.travelbusQunfaRemindSmsReset(gooutdate, proid);
                    context.Response.Write(date);
                }
                //得到退押金日志列表
                if (oper == "getyajinrefundLoglist")
                {
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(1);
                    int refundstate = context.Request["refundstate"].ConvertTo<int>(-1);
                    string key = context.Request["key"].ConvertTo<string>("");

                    string date = OrderJsonData.GetyajinrefundLoglist(pageindex, pagesize, refundstate, key);
                    context.Response.Write(date);
                }
                //得到旅游大巴预览提醒短信
                if (oper == "getPreviewRemindSms")
                {
                    string gooutdate = context.Request["gooutdate"].ConvertTo<string>("");
                    int proid = context.Request["proid"].ConvertTo<int>(0);

                    string telphone = context.Request["telphone"].ConvertTo<string>("");
                    string licenceplate = context.Request["licenceplate"].ConvertTo<string>("");


                    string date = OrderJsonData.getPreviewRemindSms(gooutdate, proid, licenceplate, telphone);
                    context.Response.Write(date);
                }
                //旅游大巴给支付成功客人 群发提醒短信 
                if (oper == "travelbusQunfaRemindSms")
                {
                    //根据订单状态查询 旅游大巴订单子表 --乘客表
                    string gooutdate = context.Request["gooutdate"].ConvertTo<string>("");
                    int proid = context.Request["proid"].ConvertTo<int>(0);
                    int paystate = context.Request["paystate"].ConvertTo<int>(0);
                    string orderstate = context.Request["orderstate"].ConvertTo<string>("");
                    string telphone = context.Request["telphone"].ConvertTo<string>("");
                    string licenceplate = context.Request["licenceplate"].ConvertTo<string>("");
                    string kerentype = context.Request["kerentype"].ConvertTo<string>("");
                    string againphone = context.Request["againphone"].ConvertTo<string>("");

                    string date = OrderJsonData.travelbusQunfaRemindSms(gooutdate, proid, paystate, orderstate, licenceplate, telphone, kerentype, againphone);
                    context.Response.Write(date);
                }
                //旅游大巴上车标注(2个操作:验证电子码；旅游大巴订单附属表中是否上车标识)
                if (oper == "travelbusonboardtag")
                {
                    var orderbusrideid = context.Request["orderbusrideid"].ConvertTo<int>(0);//旅游大巴订单附属表id
                    var orderid = context.Request["orderid"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var idcard = context.Request["idcard"].ConvertTo<string>("");
                    var name = context.Request["name"].ConvertTo<string>("");
                    var traveltime = context.Request["traveltime"].ConvertTo<string>("");

                    string r = OrderJsonData.Travelbusonboardtag(orderid, idcard, name, traveltime, comid, orderbusrideid);
                    context.Response.Write(r);
                    return;

                }
                if (oper == "GetPnoByOrderid")
                {
                    var orderid = context.Request["orderid"].ConvertTo<int>(0);
                    //根据订单id得到电子码
                    string pno = new B2bOrderData().GetPnoByOrderId(orderid);
                    if (pno != "")
                    {
                        context.Response.Write("{\"type\":\"100\",\"msg\":\"" + pno + "\"}");
                        return;
                    }
                    else
                    {
                        context.Response.Write("{\"type\":\"1\",\"msg\":\"根据订单获取电子票信息失败\"}");
                        return;
                    }
                }
                //旅游大巴改期
                if (oper == "changetraveldate")
                {
                    //var comid = context.Request["comid"].ConvertTo<int>(0);
                    var orderid = context.Request["id"].ConvertTo<int>(0);
                    //var proid = context.Request["proid"].ConvertTo<int>(0);
                    //var num = context.Request["num"].ConvertTo<int>(0);
                    var testpro = context.Request["testpro"].ConvertTo<string>("");//备注
                    var traveldate = context.Request["traveldate"].ConvertTo<string>("");
                    var oldtraveldate = context.Request["oldtraveldate"].ConvertTo<string>("");

                    string data = OrderJsonData.Changetraveldate(orderid, testpro, traveldate, oldtraveldate);
                    context.Response.Write(data);
                    return;
                }
                //慧择网保单批量查询
                if (oper == "GethzinsorderSearch")
                {
                    string orderidstr = context.Request["oidstr"].ConvertTo<string>("");
                    string data = OrderJsonData.GethzinsorderSearch(orderidstr);
                    context.Response.Write(data);
                }
                if (oper == "Hzins_detail")
                {
                    int orderid = context.Request["orderid"].ConvertTo<int>(0);
                    string data = OrderJsonData.GetHzins_detail(orderid);
                    context.Response.Write(data);
                }
                if (oper == "taobaosendcoderet")
                {
                    int selforderid = context.Request["selforderid"].ConvertTo<int>(0);
                    Taobao_send_noticelog log = new Taobao_send_noticelogData().GetSendNoticeBySelfOrderid(selforderid.ToString());
                    if (log == null)
                    {
                        context.Response.Write(JsonConvert.SerializeObject(new { type = 1, msg = "获取淘宝发码通知出错" }));
                    }
                    else
                    {
                        string r = TaobaoSendRetApi(log.order_id, log.token, log.num.ToString(), selforderid);
                        if (r == "1")
                        {
                            context.Response.Write(JsonConvert.SerializeObject(new { type = 100, msg = "淘宝发码成功" }));
                        }
                        else
                        {
                            context.Response.Write(JsonConvert.SerializeObject(new { type = 1, msg = "淘宝发码失败，请稍候重试(" + r + ")" }));
                        }
                    }
                    //context.Response.Write(data);
                }
                if (oper == "getposlogbyid")
                {
                    int logid = context.Request["logid"].ConvertTo<int>(0);
                    string data = OrderJsonData.GetPosLogById(logid);
                    context.Response.Write(data);
                }
                if (oper == "posloglist")
                {
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(0);
                    string startime = context.Request["startime"].ConvertTo<string>("");
                    string key = context.Request["key"].ConvertTo<string>("");

                    string data = OrderJsonData.GetPosLogList(pageindex, pagesize, startime, key);
                    context.Response.Write(data);
                }
                
                //直销订单直接退款
                if (oper == "quitorderfee")
                {
                    int orderid = context.Request["orderid"].ConvertTo<int>(0);
                    int quit_num = context.Request["quit_num"].ConvertTo<int>(0);
                    decimal quit_fee = context.Request["quit_fee"].ConvertTo<decimal>(0);
                    decimal totalfee = context.Request["total_fee"].ConvertTo<decimal>(0);
                    string quit_Reason = context.Request["quit_Reason"].ConvertTo<string>("");
                    string quit_info = context.Request["quit_info"].ConvertTo<string>("");

                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int userid = context.Request["userid"].ConvertTo<int>(0);
                    int proid = context.Request["proid"].ConvertTo<int>(0);


                    //退票操作
                    string data1 = OrderJsonData.QuitOrder(comid, orderid, proid, quit_num, quit_Reason + "-" + quit_info);
                    data1 = "{\"root\":" + data1 + "}";
                    XmlDocument xxd = JsonConvert.DeserializeXmlNode(data1);
                    string type1 = xxd.SelectSingleNode("root/type").InnerText;
                    string msg1 = xxd.SelectSingleNode("root/msg").InnerText;
                    if (type1 == "100")
                    {
                        #region 用户订单退款(微信支付 并且 已经开通微信自动退款的商户直销订单 退款则自动把款项退给用户，不需要在总账户后台处理；使用其他支付方式 或者 还没有开通微信支付自动退款的商户 则还需要进入总账户后台处理，并且需要手动退款给客户)
                        string refundstr = OrderJsonData.OrderRefundManage(orderid, quit_num, quit_fee, totalfee, quit_Reason, quit_info);
                        context.Response.Write(refundstr);
                        return;
                        #endregion
                    }
                    else
                    {
                        context.Response.Write("{\"type\":\"1\",\"msg\":\"" + msg1 + "\"}");
                    }

                }

                //直销用户自助退款
                if (oper == "userquitorder")
                {
                    int orderid = context.Request["orderid"].ConvertTo<int>(0);
                    int quit_num = context.Request["quit_num"].ConvertTo<int>(0);
                    string quit_Reason = "用户自助退票";
                    string quit_info = "自助退款";

                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int AccountId = 0;
                    int proid = context.Request["proid"].ConvertTo<int>(0);

                    //读取用户登录信息
                    if (context.Session["AccountId"] != null)
                    {
                        //先判断Session
                        AccountId = int.Parse(context.Request.Cookies["AccountId"].Value);
                    }
                    else
                    {
                        string data3 = "{\"type\":1,\"msg\":\"账户不匹配,请重新登录\"}";
                        context.Response.Write(data3);
                        return;
                    }


                    var order_a = new B2bOrderData().GetOrderById(orderid);
                    if (order_a == null)
                    {
                        context.Response.Write("{\"type\":\"1\",\"msg\":\"直销退款失败\"}");
                        return;
                    }
                    if (quit_num > order_a.U_num) {
                        context.Response.Write("{\"type\":\"1\",\"msg\":\"退票数量不符\"}");
                        return;
                    }
                    if (AccountId != order_a.U_id)
                    {
                        context.Response.Write("{\"type\":\"1\",\"msg\":\"退票用户不符\"}");
                        return;
                    }
                    if (comid != order_a.Comid)
                    {
                        context.Response.Write("{\"type\":\"1\",\"msg\":\"退票账户不符\"}");
                        return;
                    }


                    //退票操作，当用户直销订单需要单独对微信进行退款
                    string data1 = OrderJsonData.QuitOrder(comid, orderid, proid, quit_num, quit_Reason + "-" + quit_info);
                    data1 = "{\"root\":" + data1 + "}";
                    XmlDocument xxd = JsonConvert.DeserializeXmlNode(data1);
                    string type1 = xxd.SelectSingleNode("root/type").InnerText;
                    string msg1 = xxd.SelectSingleNode("root/msg").InnerText;
                    if (type1 == "100")
                    {
                        #region 用户订单退款(微信支付 并且 已经开通微信自动退款的商户直销订单 退款则自动把款项退给用户，不需要在总账户后台处理；使用其他支付方式 或者 还没有开通微信支付自动退款的商户 则还需要进入总账户后台处理，并且需要手动退款给客户)

                        
                        B2b_pay msucpay = new B2bPayData().GetSUCCESSPayById(order_a.Id);
                        if (msucpay != null)
                        {

                            decimal quitfee = quit_num * order_a.Pay_price;//退款金额，没有计算快递费，积分等问题
                            if (quitfee > msucpay.Total_fee)
                            {
                                quitfee = msucpay.Total_fee;
                            }
                            string refundstr = OrderJsonData.OrderRefundManage(order_a.Id, order_a.U_num, quitfee, msucpay.Total_fee, "微信自动退款", "微信自动退款");
                            context.Response.Write(refundstr);
                            return;
                        }
                        return;
                        #endregion
                    }
                    else
                    {
                        context.Response.Write("{\"type\":\"1\",\"msg\":\"" + msg1 + "\"}");
                    }

                }
                if (oper == "getorderdetail")
                {
                    var orderid = context.Request["orderid"].ConvertTo<int>(0);

                    string data = OrderJsonData.Getorderdetail(orderid);
                    context.Response.Write(data);
                }
                //录入糯米交易号
                if (oper == "insnuomi_dealid")
                {
                    var orderid = context.Request["orderid"].ConvertTo<int>(0);
                    var nuomidealid = context.Request["nuomidealid"].ConvertTo<int>(0);
                    string data = OrderJsonData.Insnuomi_dealid(orderid, nuomidealid);
                    context.Response.Write(data);
                }
                if (oper == "uporderstate")
                {
                    var orderid = context.Request["orderid"].ConvertTo<int>(0);
                    var orderstate = context.Request["orderstate"].ConvertTo<int>(0);
                    string data = OrderJsonData.Uporderstate(orderid, orderstate);
                    context.Response.Write(data);
                }
                if (oper == "deladdr")
                {
                    var addrid = context.Request["addrid"].ConvertTo<int>(0);
                    string data = OrderJsonData.Deladdr(addrid);
                    context.Response.Write(data);
                }
                if (oper == "getagentaddrbyid")
                {
                    var addrid = context.Request["addrid"].ConvertTo<int>(0);
                    string data = OrderJsonData.Getagentaddrbyid(addrid);
                    context.Response.Write(data);
                }
                if (oper == "editagentaddr")
                {
                    var addrid = context.Request["addrid"].ConvertTo<int>(0);
                    var agentid = context.Request["agentid"].ConvertTo<int>(0);
                    string u_name = context.Request["u_name"].ConvertTo<string>("");
                    var u_phone = context.Request["u_phone"].ConvertTo<string>("");
                    var province = context.Request["province"].ConvertTo<string>("");
                    string city = context.Request["city"].ConvertTo<string>("");
                    var address = context.Request["address"].ConvertTo<string>("");
                    var txtcode = context.Request["txtcode"].ConvertTo<string>("");

                    string data = OrderJsonData.SaveAddress(addrid, agentid, u_name, u_phone, province, city,
address, txtcode);
                    context.Response.Write(data);
                }
                //计算购物车运费
                if (oper == "getshopcartexpressfee")
                {
                    var proidstr = context.Request["proidstr"].ConvertTo<string>("");
                    var numstr = context.Request["numstr"].ConvertTo<string>("");
                    string citystr = context.Request["citystr"].ConvertTo<string>("");


                    string data = OrderJsonData.GetShopCartExpressfee(proidstr, citystr, numstr);
                    context.Response.Write(data);

                }
                if (oper == "gettravelbus")
                {
                    int busid = context.Request["busid"].ConvertTo<int>(0);

                    string date = OrderJsonData.Gettravelbus(busid);
                    context.Response.Write(date);
                }
                if (oper == "CloseOrder")
                {
                    int proid = context.Request["proid"].ConvertTo<int>(0);
                    string daydate = context.Request["daydate"].ConvertTo<string>("");
                    int userid = context.Request["userid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);


                    string date = OrderJsonData.CloseOrder(proid, daydate, userid, comid);
                    context.Response.Write(date);

                }
                if (oper == "Gettravelbusorder_sendbusBylogid")
                {
                    int logid = context.Request["logid"].ConvertTo<int>(0);

                    string date = OrderJsonData.Gettravelbusorder_sendbusBylogid(logid);
                    context.Response.Write(date);
                }

                if (oper == "edittravelbusorder_operlog")
                {
                    var operlogid = context.Request["operlogid"].ConvertTo<int>(0);
                    var proid = context.Request["proid"].ConvertTo<int>(0);
                    var proname = context.Request["proname"].ConvertTo<string>("");
                    var gooutdate = context.Request["gooutdate"].ConvertTo<string>("");
                    var operremark = context.Request["operremark"].ConvertTo<string>("");
                    var bustotal = context.Request["bustotal"].ConvertTo<int>(0);
                    var busids = context.Request["busids"].ConvertTo<string>("");
                    var travelbus_model = context.Request["travelbus_model"].ConvertTo<string>("");
                    var seatnum = context.Request["seatnum"].ConvertTo<string>("");
                    var licenceplate = context.Request["licenceplate"].ConvertTo<string>("");
                    var drivername = context.Request["drivername"].ConvertTo<string>("");
                    var driverphone = context.Request["driverphone"].ConvertTo<string>("");
                    var userid = context.Request["userid"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var issavebus = context.Request["issavebus"].ConvertTo<string>("");

                    string date = OrderJsonData.Edittravelbusorder_operlog(operlogid, proid, proname, gooutdate, operremark, bustotal, busids, travelbus_model, seatnum, licenceplate, drivername, driverphone, userid, comid, issavebus);
                    context.Response.Write(date);
                }
                if (oper == "travelbusorderdetail")
                {
                    //根据订单状态查询 旅游大巴订单
                    string gooutdate = context.Request["gooutdate"].ConvertTo<string>("");
                    int proid = context.Request["proid"].ConvertTo<int>(0);
                    int order_state = context.Request["order_state"].ConvertTo<int>(0);


                    string date = OrderJsonData.Travelbusorderdetail(gooutdate, proid, order_state);
                    context.Response.Write(date);
                }
                if (oper == "travelbusordertravalerdetail")
                {
                    //根据订单状态查询 旅游大巴订单--乘客信息
                    string gooutdate = context.Request["gooutdate"].ConvertTo<string>("");
                    int proid = context.Request["proid"].ConvertTo<int>(0);
                    int order_state = context.Request["order_state"].ConvertTo<int>(0);


                    string date = OrderJsonData.travelbusordertravalerdetail(gooutdate, proid, order_state);
                    context.Response.Write(date);
                }
                if (oper == "travelbusorderdetailBypaystate")
                {
                    //根据订单状态查询 旅游大巴订单
                    string gooutdate = context.Request["gooutdate"].ConvertTo<string>("");
                    int proid = context.Request["proid"].ConvertTo<int>(0);
                    int paystate = context.Request["paystate"].ConvertTo<int>(0);


                    string date = OrderJsonData.travelbusorderdetailBypaystate(gooutdate, proid, paystate);
                    context.Response.Write(date);
                }
                if (oper == "travelbustravelerlistBypaystate")
                {
                    //根据订单状态查询 旅游大巴订单子表 --乘客表
                    string gooutdate = context.Request["gooutdate"].ConvertTo<string>("");
                    int proid = context.Request["proid"].ConvertTo<int>(0);
                    int paystate = context.Request["paystate"].ConvertTo<int>(0);
                    string orderstate = context.Request["orderstate"].ConvertTo<string>("");

                    string date = OrderJsonData.travelbustravelerlistBypaystate(gooutdate, proid, paystate, orderstate);
                    context.Response.Write(date);
                }

                if (oper == "travelbusorderdetailByiscloseteam")
                {
                    //根据是否截团查询 旅游大巴订单(如果已经截团，则查询 处理过 的订单；如果没有截团，则查询支付成功的订单)
                    string gooutdate = context.Request["gooutdate"].ConvertTo<string>("");
                    int proid = context.Request["proid"].ConvertTo<int>(0);
                    int iscloseteam = context.Request["iscloseteam"].ConvertTo<int>(0);

                    string date = OrderJsonData.travelbusorderdetailByiscloseteam(gooutdate, proid, iscloseteam);
                    context.Response.Write(date);
                }
                if (oper == "travelbusordercountbyday")
                {
                    DateTime startdate = context.Request["startdate"].ConvertTo<DateTime>(DateTime.Now);
                    DateTime enddate = context.Request["enddate"].ConvertTo<DateTime>(DateTime.Now);
                    int servertype = context.Request["servertype"].ConvertTo<int>(10);
                    int comid = context.Request["comid"].ConvertTo<int>(0);


                    string data = OrderJsonData.travelbusordercountbyday(startdate, enddate, servertype, comid);
                    context.Response.Write(data);

                }
                if (oper == "Getb2bcomprobytraveldate")
                {
                    DateTime daydate = context.Request["daydate"].ConvertTo<DateTime>(DateTime.Now);
                    int servertype = context.Request["servertype"].ConvertTo<int>(10);
                    int comid = context.Request["comid"].ConvertTo<int>(0);


                    string data = OrderJsonData.Getb2bcomprobytraveldate(daydate, servertype, comid);
                    context.Response.Write(data);
                }
                if (oper == "GetNeedvisitdateProByTraveldate")
                {
                    DateTime daydate = context.Request["daydate"].ConvertTo<DateTime>(DateTime.Now);
                    int servertype = context.Request["servertype"].ConvertTo<int>(1);
                    int comid = context.Request["comid"].ConvertTo<int>(0);


                    string data = OrderJsonData.GetNeedvisitdateProByTraveldate(daydate, servertype, comid);
                    context.Response.Write(data);
                }
                if (oper == "Recharge")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var proid = 0;
                    var Speciid = 0;
                    var userid = context.Request["userid"].ConvertTo<int>(0);
                    var payorder = context.Request["payorder"].ConvertTo<int>(0);//判断是否为直接支付订单

                    var price = context.Request["price"].ConvertTo<decimal>(0);
                    var u_name = context.Request["u_name"].ConvertTo<string>("");

                    var u_phone = context.Request["u_phone"].ConvertTo<string>("");

                    var order_remark = context.Request["remark"].ConvertTo<string>("");

                    var Pno = context.Request["pno"].ConvertTo<string>("");//加密的 预约验证码，自动成功
                    if (Pno != "")
                    {
                        Pno = EncryptionHelper.EticketPnoDES(Pno, 1);//解密
                    }

                    B2b_crm user = new B2bCrmData().Readuser(userid, comid);
                    if (user != null)
                    {
                        if (u_name != "")
                        {
                            u_name = user.Name;
                        }
                        if (u_phone != null)
                        {
                            u_phone = user.Phone;
                        }
                    }


                    var sid = context.Request["sid"].ConvertTo<string>("");//选择服务已,隔开（增加服务金额，及押金）

                    if (sid != "")
                    {
                        int bixuzhifuyajin = 0;
                        decimal serverprice_temp = 0;
                        var id_arr = sid.Split(',');
                        for (int i = 0; i < id_arr.Count(); i++)
                        {
                            if (id_arr[i].Trim() != "")
                            {
                                var Rentserver_temp = new RentserverData().Rentserverbyid(int.Parse(id_arr[i].Trim()), comid);
                                if (Rentserver_temp != null)
                                {
                                    serverprice_temp += Rentserver_temp.saleprice + Rentserver_temp.serverDepositprice;
                                }
                            }
                        }

                        price = price + serverprice_temp;

                        if (price == 0)
                        {
                            context.Response.Write("{\"type\":\"1\",\"msg\":\"购买金额错误\"}");
                            return;
                        }

                        //通过码查询产品
                        B2b_com_pro m = new B2bComProData().Getprobypno(Pno);
                        if (m != null)
                        {   //通过产品查询所可以购买的服务
                            int rpnum = 0;
                            var Rentserverbyproid_temp = new RentserverData().Rentserverpagelist(1, 100, m.Com_id, out rpnum, m.Id);
                            if (Rentserverbyproid_temp != null)
                            {

                                for (int i = 0; i < Rentserverbyproid_temp.Count; i++)
                                {
                                    if (Rentserverbyproid_temp[i].mustselect != 0) //查看必须购买的是否购买了
                                    {
                                        int bixugoumai = 0;
                                        for (int j = 0; j < id_arr.Count(); j++)//对提交的购买服务id，循环
                                        {
                                            if (id_arr[j].Trim() != "")
                                            {
                                                if (int.Parse(id_arr[j].Trim()) == Rentserverbyproid_temp[i].id)
                                                {//购买提交服务id必须包含
                                                    bixugoumai = 1;
                                                }
                                            }
                                        }//循环结束，如果购买服务里没有包含则下面就 请选择上必须支付的押金服务
                                        if (bixugoumai == 0)
                                        {
                                            bixuzhifuyajin = 1;
                                        }
                                    }
                                }
                            }



                            if (bixuzhifuyajin == 1)
                            {
                                context.Response.Write("{\"type\":\"1\",\"msg\":\"请选择上必须支付的押金服务\"}");
                                return;
                            }

                        }
                    }
                    else {

                        context.Response.Write("{\"type\":\"1\",\"msg\":\"请选择服务\"}");
                        return;
                    }



                    B2b_order order = new B2b_order()
                    {
                        Id = 0,
                        Pro_id = proid,
                        Speciid = Speciid,
                        Order_type = 2,
                        U_name = u_name,
                        U_id = userid,
                        U_phone = u_phone,
                        U_num = 1,
                        U_subdate = DateTime.Now,
                        Backtickettime = DateTime.Now,
                        Payid = 0,
                        Pay_price = price,
                        Cost = 0,
                        Profit = 0,
                        Order_state = (int)OrderStatus.WaitPay,//
                        Pay_state = (int)PayStatus.NotPay,//支付模块未做出来前先设置为已支付
                        Send_state = (int)SendCodeStatus.NotSend,
                        Ticketcode = 0,//电子码未创建，支付后产生码赋值
                        Rebate = 0,//  利润返佣金额暂时定为0
                        Ordercome = "",//订购来源 暂时定为空
                        U_traveldate = DateTime.Now,
                        Dealer = "自动",
                        Comid = comid,
                        Openid = "",
                        Pno = Pno,
                        serverid = sid,
                        Ticketinfo = "",
                        Order_remark = order_remark,
                        payorder =payorder
                    };
                    string data = "";
                    try
                    {
                        data = OrderJsonData.InsertRecharge(order);
                    }
                    catch
                    {
                        data = "";
                    }

                    context.Response.Write(data);

                }

                if (oper == "H5Recharge")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var proid = 0;
                    var Speciid = 0;
                    var openid = context.Request["openid"].ConvertTo<string>("");
                    var price = context.Request["price"].ConvertTo<decimal>(0);

                    B2bCrmData b2b_crm = new B2bCrmData();
                    if (openid != "")
                    {
                        B2b_crm user = b2b_crm.b2b_crmH5(openid, comid);
                        if (user != null)
                        {
                            if (user != null)
                            {

                                B2b_order order = new B2b_order()
                                {
                                    Id = 0,
                                    Pro_id = proid,
                                    Speciid = Speciid,
                                    Order_type = 2,
                                    U_name = user.Name,
                                    U_id = user.Id,
                                    U_phone = user.Phone,
                                    U_num = 1,
                                    U_subdate = DateTime.Now,
                                    Payid = 0,
                                    Pay_price = price,
                                    Cost = 0,
                                    Profit = 0,
                                    Order_state = (int)OrderStatus.WaitPay,//
                                    Pay_state = (int)PayStatus.NotPay,//支付模块未做出来前先设置为已支付
                                    Send_state = (int)SendCodeStatus.NotSend,
                                    Ticketcode = 0,//电子码未创建，支付后产生码赋值
                                    Rebate = 0,//  利润返佣金额暂时定为0
                                    Ordercome = "",//订购来源 暂时定为空
                                    U_traveldate = DateTime.Now,
                                    Dealer = "自动",
                                    Comid = comid,

                                    Openid = ""
                                };
                                string data = "";
                                try
                                {
                                    data = OrderJsonData.InsertRecharge(order);
                                }
                                catch
                                {
                                    data = "";
                                }

                                context.Response.Write(data);
                            }
                        }

                    }

                }

                if (oper == "B2bcrmreader")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var uid = context.Request["uid"].ConvertTo<int>(0);
                    string data = OrderJsonData.B2bcrmreader(comid, uid);

                    context.Response.Write(data);
                }

                if (oper == "gettotaldate")
                {
                    var comid = context.Request["comid"];
                    var startdate = context.Request["startdate"];
                    var enddate = context.Request["enddate"];
                    string data = OrderJsonData.GetTotalDate(comid, startdate, enddate);

                    context.Response.Write(data);


                }
                if (oper == "getsalecount")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var startdate = context.Request["startdate"];
                    var enddate = context.Request["enddate"];
                    var key = context.Request["key"].ConvertTo<string>("");
                    string data = OrderJsonData.SaleCountPageList(comid, pageindex, pagesize, startdate, enddate, key);

                    context.Response.Write(data);
                }
                //综合查询订单 userid 是判断是否为渠道 非用户ID
                if (oper == "getorderlist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var order_state = context.Request["order_state"].ConvertTo<int>(0);
                    var ordertype = context.Request["ordertype"].ConvertTo<int>(0);

                    int userid = context.Request["userid"].ConvertTo<int>(0);
                    var servertype = context.Request["servertype"].ConvertTo<int>(0);

                    var begindate = context.Request["beginDate"].ConvertTo<string>("");
                    var enddate = context.Request["endDate"].ConvertTo<string>("");
                    var datetype = context.Request["datetype"].ConvertTo<int>(0);


                    string data = OrderJsonData.OrderPageList(comid, pageindex, pagesize, key, order_state, ordertype, userid, 0, servertype, begindate, enddate, datetype);

                    context.Response.Write(data);
                }

                //综合查询订单 userid 是判断是否为渠道 非用户ID
                if (oper == "getyuyueorderlist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var order_state = context.Request["order_state"].ConvertTo<int>(0);
                    var ordertype = context.Request["ordertype"].ConvertTo<int>(0);

                    int userid = context.Request["userid"].ConvertTo<int>(0);
                    var servertype = context.Request["servertype"].ConvertTo<int>(0);

                    var begindate = context.Request["beginDate"].ConvertTo<string>("");
                    var enddate = context.Request["endDate"].ConvertTo<string>("");
                    var datetype = context.Request["datetype"].ConvertTo<int>(0);


                    string data = OrderJsonData.yuyueOrderPageList(comid, pageindex, pagesize, key, order_state, ordertype, userid, 0, servertype, begindate, enddate, datetype);

                    context.Response.Write(data);
                }

                //综合查询订单 userid 是判断是否为渠道 非用户ID
                if (oper == "getcartorderlist")
                {
                    var comid = context.Request["comid"];
                    var cartid = context.Request["cartid"].ConvertTo<int>(0);

                    string data = OrderJsonData.OrderCartPageList(comid, cartid);

                    context.Response.Write(data);
                }

                //订单统计
                if (oper == "getordercount")
                {
                    var comid = context.Request["comid"];
                    var startime = context.Request["startime"].ConvertTo<string>("");
                    var endtime = context.Request["endtime"].ConvertTo<string>("");
                    var searchtype = context.Request["searchtype"].ConvertTo<int>(1);

                    int userid = context.Request["userid"].ConvertTo<int>(0);


                    if (startime == "" || endtime == "")
                    {
                        var data1 = "{\"type\":1,\"msg\":\"时间错误\"}";
                        context.Response.Write(data1);
                        return;
                    }

                    string data = OrderJsonData.OrderCountList(comid, startime, endtime, searchtype, userid);

                    context.Response.Write(data);
                }

                //订单财务确认
                if (oper == "orderfinset")
                {
                    var comid = context.Request["comid"];
                    var startdate = context.Request["startdate"].ConvertTo<string>("");
                    var enddate = context.Request["enddate"].ConvertTo<string>("");
                    var mangefinset = context.Request["mangefinset"].ConvertTo<int>(1);

                    var key = context.Request["key"].ConvertTo<string>("");


                    if (startdate == "" || enddate == "")
                    {
                        var data1 = "{\"type\":1,\"msg\":\"时间错误\"}";
                        context.Response.Write(data1);
                        return;
                    }

                    string data = OrderJsonData.Orderfinset(comid, startdate, enddate, mangefinset, key);

                    context.Response.Write(data);
                }

                //订单财务确认
                if (oper == "orderfinset_pro_list")
                {
                    var comid = context.Request["comid"];
                    var startdate = context.Request["startdate"].ConvertTo<string>("");
                    var enddate = context.Request["enddate"].ConvertTo<string>("");
                    var mangefinset = context.Request["mangefinset"].ConvertTo<int>(1);

                    var key = context.Request["key"].ConvertTo<string>("");
                    var submanagename = context.Request["submanagename"].ConvertTo<string>("");

                    if (startdate == "" || enddate == "")
                    {
                        var data1 = "{\"type\":1,\"msg\":\"时间错误\"}";
                        context.Response.Write(data1);
                        return;
                    }

                    string data = OrderJsonData.Orderfinset_pay_price_list(comid, startdate, enddate, mangefinset, key, submanagename);

                    context.Response.Write(data);
                }

                if (oper == "orderfinset_quren")
                {
                    var comid = context.Request["comid"];
                    var startdate = context.Request["startdate"].ConvertTo<string>("");
                    var enddate = context.Request["enddate"].ConvertTo<string>("");
                    var mangefinset = context.Request["mangefinset"].ConvertTo<int>(1);
                    var submanagename = context.Request["submanagename"].ConvertTo<string>("");
                    var key = "";
                    if (startdate == "" || enddate == "")
                    {
                        var data1 = "{\"type\":1,\"msg\":\"时间错误\"}";
                        context.Response.Write(data1);
                        return;
                    }

                    string data = OrderJsonData.orderfinset_quren(comid, startdate, enddate, mangefinset, key, submanagename);

                    context.Response.Write(data);
                }
                

                //查询用户订单userid 是用户id
                if (oper == "getuserorderlist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var order_state = context.Request["order_state"].ConvertTo<int>(0);
                    var ordertype = context.Request["ordertype"].ConvertTo<int>(0);

                    int userid = context.Request["userid"].ConvertTo<int>(0);
                    string data = OrderJsonData.OrderPageList(comid, pageindex, pagesize, key, order_state, ordertype, 0, userid);

                    context.Response.Write(data);
                }

                //分销订单列表
                if (oper == "getagentorderlist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var account = context.Request["account"].ConvertTo<string>("");
                    var order_state = context.Request["order_state"].ConvertTo<int>(0);

                    var beginDate = context.Request["beginDate"].ConvertTo<string>("");
                    var endDate = context.Request["endDate"].ConvertTo<string>("");
                    var servertype = context.Request["servertype"].ConvertTo<int>(0);

                    string data = OrderJsonData.AgentOrderPageList(comid.ToString(), agentid, account, pageindex, pagesize, key, order_state, beginDate, endDate, servertype);

                    context.Response.Write(data);
                }

                //分销销售统计
                if (oper == "getagentordercount")
                {
                    var comid = context.Request["comid"];
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var startime = context.Request["startime"].ConvertTo<string>("");
                    var endtime = context.Request["endtime"].ConvertTo<string>("");


                    string data = OrderJsonData.AgentOrderCount(comid, agentid, pageindex, pagesize, key, startime, endtime);

                    context.Response.Write(data);
                }
                //分销销售统计
                if (oper == "clientagentordercount")
                {
                    var comid = context.Request["comid"];
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var startime = context.Request["startime"].ConvertTo<string>("");
                    var endtime = context.Request["endtime"].ConvertTo<string>("");


                    string data = OrderJsonData.AgentOrderCount(comid, agentid, pageindex, pagesize, key, startime, endtime);

                    context.Response.Write(data);
                }

                //按订单，分销销售统计
                if (oper == "getIdagentordercount")
                {
                    var comid = context.Request["comid"];
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    int orderid = context.Request["orderid"].ConvertTo<int>(0);
                    string data = OrderJsonData.GetIdAgentOrderCount(comid, agentid, orderid);

                    context.Response.Write(data);
                }

                //分销倒码整体退票
                if (oper == "EticketOrderVoid")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    int orderid = context.Request["orderid"].ConvertTo<int>(0);
                    string data = OrderJsonData.EticketOrderVoid(comid, agentid, orderid);

                    context.Response.Write(data);
                }

                //处理退票
                if (oper == "getticketlist")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var order_state = context.Request["order_state"].ConvertTo<int>(17);
                    var endstate = 0;
                    string data = OrderJsonData.TicketPageList(pageindex, pagesize, key, order_state, endstate);

                    context.Response.Write(data);
                }
                //处理退票Upticket
                if (oper == "Upticket")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var remarks = context.Request["remarks"].ConvertTo<string>("");
                    var price = context.Request["price"].ConvertTo<decimal>(0);

                    string data = "";

                    B2b_order oldorder = new B2bOrderData().GetOrderById(id);

                    var paydate = new B2bPayData();
                    decimal Total_fee = 0;

                    if (oldorder != null)
                    {
                        if (oldorder.Order_state == 17 || oldorder.Order_state == 18)
                        {
                            Total_fee = paydate.GetPayByoId(oldorder.Id) == null ? 0 : paydate.GetPayByoId(oldorder.Id).Total_fee;
                            oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退票
                            oldorder.Ticketinfo = remarks;
                            oldorder.Backtickettime = DateTime.Now;
                            oldorder.Ticket = price;

                            //退款金额不能等于0
                            if (price == 0)
                            {
                                data = "{\"type\":1,\"msg\":\"退款金额不能等于0\"}";
                                context.Response.Write(data);
                                return;
                            }

                            //如果有退款金额 ，退款金额不能大于收款的金额
                            if (Total_fee != 0)
                            {
                                if (price > Total_fee)
                                {
                                    data = "{\"type\":1,\"msg\":\"金额有误，退款金额不能大于支付金额\"}";
                                    context.Response.Write(data);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            data = "{\"type\":1,\"msg\":\"订单状态不正确，只有 申请了退票的直销订单才能进一步退款!\"}";
                            context.Response.Write(data);
                            return;

                        }
                    }
                    data = OrderJsonData.Upticket(oldorder);

                    context.Response.Write(data);
                    //context.Response.Write("{\"type\":100,\"msg\":\"dd\",\"finaceback\":\"\",\"eticket\":\"\"}");
                }
                //标注处理
                if (oper == "orderyoufang")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var state = context.Request["state"].ConvertTo<int>(0);
                    string data = "";
                    B2b_order modelb2border = new B2bOrderData().GetOrderById(id);
                    if (modelb2border != null)
                    {

                        B2b_com_pro pro_t = new B2bComProData().GetProById(modelb2border.Pro_id.ToString());
                        if (pro_t != null)
                        {
                            if (state == 0)
                            {
                                data = OrderJsonData.UporderPaystate(id, "qx", pro_t.bookpro_bindphone);

                                modelb2border.Order_state = (int)OrderStatus.Hotecannel;//订房取消订单
                                modelb2border.Backtickettime = DateTime.Now;
                                data = OrderJsonData.InsertRecharge(modelb2border);

                            }
                            else
                            {
                                data = OrderJsonData.UporderPaystate(id, "qr", pro_t.bookpro_bindphone);


                                //确认订单后直接 给结算了 只针对订房
                                var data_temp = OrderJsonData.agentorder_shoudongchuli(id);

                            }

                        }
                        else
                        {
                            data = "{\"type\":1,\"msg\":\"产品错误\"}";
                        }
                    }
                    else
                    {

                        data = "{\"type\":1,\"msg\":\"订单错误\"}";
                    }
                    context.Response.Write(data);

                }




                //标注处理
                if (oper == "orderfin")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var state = context.Request["state"].ConvertTo<int>(0);

                    B2b_order oldorder = new B2bOrderData().GetOrderById(id);
                    B2b_com_pro pro = new B2b_com_pro();
                    string data = "";
                    if (oldorder != null)
                    {
                        pro = new B2bComProData().GetProById(oldorder.Pro_id.ToString());
                        if (pro == null)
                        {
                            data = "{\"type\":1,\"msg\":\"产品错误\"}";
                        }
                        else
                        {
                            if (pro.Server_type != 1)
                            {

                                if (pro.Server_type == 9)
                                {
                                    string qustte = "";
                                    if (state == 0)
                                    {
                                        qustte = "qx";
                                    }
                                    else
                                    {
                                        qustte = "qr";
                                    }



                                    data = OrderJsonData.UporderPaystate(id, qustte, pro.bookpro_bindphone);
                                }
                                else
                                {
                                    if (state == 0)
                                    {
                                        oldorder.Order_state = (int)OrderStatus.Hotecannel;//订房取消订单
                                        oldorder.Backtickettime = DateTime.Now;
                                    }
                                    else
                                    {

                                        oldorder.Order_state = (int)OrderStatus.HasFin;//已处理
                                        oldorder.Backtickettime = DateTime.Now;
                                    }


                                    try
                                    {
                                        //酒店先退票
                                        if (pro.Server_type == 9)
                                        {
                                            if (state == 0)
                                            {
                                                string data_temp = OrderJsonData.QuitOrder(oldorder.Comid, id, pro.Id, oldorder.U_num, "酒店房满自动退票");
                                            }
                                        }


                                        data = OrderJsonData.InsertRecharge(oldorder);
                                    }
                                    catch
                                    {
                                        data = "{\"type\":1,\"msg\":\"操作错误\"}";
                                    }

                                    if (pro.Server_type == 9)
                                    {
                                        if (state == 0)//酒店发送房满作废消息
                                        {
                                            new Weixin_tmplmsgManage().WxTmplMsg_OrderStatusChange(id);//--状态变更

                                        }
                                        else
                                        {//酒店发送确认成功消息
                                            new Weixin_tmplmsgManage().WxTmplMsg_OrderHotelConfirm(id);//--酒店确认
                                        }
                                    }
                                }

                            }
                            else
                            {
                                data = "{\"type\":1,\"msg\":\"产品类型错误\"}";
                            }
                        }
                    }
                    else
                    {
                        data = "{\"type\":1,\"msg\":\"订单错误\"}";
                    }

                    context.Response.Write(data);
                }


                //标注已发码
                if (oper == "orderstatefin")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);

                    B2b_order oldorder = new B2bOrderData().GetOrderById(id);
                    B2b_com_pro pro = new B2b_com_pro();
                    string data = "";
                    if (oldorder != null)
                    {
                        pro = new B2bComProData().GetProById(oldorder.Pro_id.ToString());
                        if (pro == null)
                        {
                            data = "{\"type\":1,\"msg\":\"产品错误\"}";
                        }
                        else
                        {
                            if (pro.Server_type != 1)
                            {
                                oldorder.Order_state = (int)OrderStatus.HasSendCode;//已发码
                                oldorder.Backtickettime = DateTime.Now;

                                try
                                {
                                    data = OrderJsonData.InsertRecharge(oldorder);
                                }
                                catch
                                {
                                    data = "{\"type\":1,\"msg\":\"操作错误\"}";
                                }
                            }
                            else
                            {
                                data = "{\"type\":1,\"msg\":\"产品类型错误\"}";
                            }
                        }
                    }
                    else
                    {
                        data = "{\"type\":1,\"msg\":\"订单错误\"}";
                    }

                    context.Response.Write(data);
                }


                //分销退票
                if (oper == "getticket")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var proid = context.Request["proid"].ConvertTo<int>(0);
                    var num = context.Request["num"].ConvertTo<int>(0);
                    var testpro = context.Request["testpro"].ConvertTo<string>("");//备注

                    if (num == 0)
                    {
                        context.Response.Write("{\"type\":\"1\",\"msg\":\"传递退票数量不可为0\"}");
                        return;
                    }
                    string bookpro_bindphone = "";
                    string data = "";
                    int server_type = 0;
                    var prodata = new B2bComProData().GetProById(proid.ToString());
                    if (prodata != null)
                    {
                        server_type = prodata.Server_type;
                        bookpro_bindphone = prodata.bookpro_bindphone;
                    }

                    //退票判断
                    if (server_type == 9 || server_type == 10 || server_type == 11 || server_type == 13 || server_type == 2)
                    {

                         data = "{\"type\":1,\"msg\":\"订单因为需要商家确认，请联系客服人员退订！\"}";
                         context.Response.Write(data);
                         return;
                    }

                    if (server_type == 9)
                    {

                        data = OrderJsonData.UporderPaystate(id, "qx", bookpro_bindphone);
                    }
                    else
                    {
                        data = OrderJsonData.QuitOrder(comid, id, proid, num, testpro);

                        //订单是B单，则需要查询A单，A单是直销订单 
                        var order_a = new B2bOrderData().GetOldorderBybindingId(id);
                        if (order_a != null)
                        {
                            if (order_a.Agentid == 0)
                            {
                                string data1 = "{\"root\":" + data + "}";
                                XmlDocument xxd = JsonConvert.DeserializeXmlNode(data1);
                                string type1 = xxd.SelectSingleNode("root/type").InnerText;
                                string msg1 = xxd.SelectSingleNode("root/msg").InnerText;
                                if (type1 == "100")
                                {
                                    #region 用户订单退款(微信支付 并且 已经开通微信自动退款的商户直销订单 退款则自动把款项退给用户，不需要在总账户后台处理；使用其他支付方式 或者 还没有开通微信支付自动退款的商户 则还需要进入总账户后台处理，并且需要手动退款给客户)
                                    B2b_pay msucpay = new B2bPayData().GetSUCCESSPayById(order_a.Id);
                                    if (msucpay != null)
                                    {
                                        decimal quitfee = num * order_a.Pay_price;
                                        if (quitfee > msucpay.Total_fee)
                                        {
                                            quitfee = msucpay.Total_fee;
                                        }
                                        string refundstr = OrderJsonData.OrderRefundManage(order_a.Id, num, quitfee, msucpay.Total_fee, "微信自动退款", "微信自动退款");
                                        //context.Response.Write(refundstr);
                                        //return;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    context.Response.Write("{\"type\":\"1\",\"msg\":\"" + msg1 + "\"}");
                                    return;
                                }
                            }
                        }

                    }
                    context.Response.Write(data);
                    return;

                }


                //商户退票
                if (oper == "com-getticket")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var proid = context.Request["proid"].ConvertTo<int>(0);
                    var num = context.Request["num"].ConvertTo<int>(0);
                    var testpro = context.Request["testpro"].ConvertTo<string>("");//备注

                    if (num == 0)
                    {
                        context.Response.Write("{\"type\":\"1\",\"msg\":\"传递退票数量不可为0\"}");
                        return;
                    }
                    string bookpro_bindphone = "";
                    string data = "";
                    int server_type = 0;
                    var prodata = new B2bComProData().GetProById(proid.ToString());
                    if (prodata != null)
                    {
                        server_type = prodata.Server_type;
                        bookpro_bindphone = prodata.bookpro_bindphone;
                    }

                    if (server_type == 9)
                    {

                        data = OrderJsonData.UporderPaystate(id, "qx", bookpro_bindphone);
                    }
                    else
                    {
                        data = OrderJsonData.QuitOrder(comid, id, proid, num, testpro);

                        //订单是B单，则需要查询A单，A单是直销订单 
                        var order_a = new B2bOrderData().GetOldorderBybindingId(id);
                        if (order_a != null)
                        {
                            if (order_a.Agentid == 0)
                            {
                                string data1 = "{\"root\":" + data + "}";
                                XmlDocument xxd = JsonConvert.DeserializeXmlNode(data1);
                                string type1 = xxd.SelectSingleNode("root/type").InnerText;
                                string msg1 = xxd.SelectSingleNode("root/msg").InnerText;
                                if (type1 == "100")
                                {
                                    #region 用户订单退款(微信支付 并且 已经开通微信自动退款的商户直销订单 退款则自动把款项退给用户，不需要在总账户后台处理；使用其他支付方式 或者 还没有开通微信支付自动退款的商户 则还需要进入总账户后台处理，并且需要手动退款给客户)
                                    B2b_pay msucpay = new B2bPayData().GetSUCCESSPayById(order_a.Id);
                                    if (msucpay != null)
                                    {
                                        decimal quitfee = order_a.U_num * order_a.Pay_price;
                                        if (quitfee > msucpay.Total_fee)
                                        {
                                            quitfee = msucpay.Total_fee;
                                        }
                                        string refundstr = OrderJsonData.OrderRefundManage(order_a.Id, order_a.U_num, quitfee, msucpay.Total_fee, "微信自动退款", "微信自动退款");
                                        //context.Response.Write(refundstr);
                                        //return;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    context.Response.Write("{\"type\":\"1\",\"msg\":\"" + msg1 + "\"}");
                                    return;
                                }
                            }
                        }

                    }
                    context.Response.Write(data);
                    return;

                }


                //确认倒码那生成excel
                if (oper == "createdaoma")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var proid = context.Request["proid"].ConvertTo<int>(0);
                    var confirmstate = context.Request["confirmstate"].ConvertTo<int>(0);
                    var testpro = context.Request["testpro"].ConvertTo<string>("");//备注
                    string data = "";
                    try
                    {
                        //读取订单
                        B2b_order oldorder = new B2bOrderData().GetOrderById(id);
                        if (oldorder != null)
                        {
                            if (confirmstate == 1)
                            {
                                data = OrderJsonData.CreateDaoma(comid, id, proid, confirmstate, testpro);
                            }
                            else
                            {
                                oldorder.Order_state = (int)OrderStatus.InvalidOrder;//作废
                                oldorder.Ticketinfo = testpro;
                                oldorder.Backtickettime = DateTime.Now;
                                data = OrderJsonData.InsertRecharge(oldorder);
                            }

                        }
                        else
                        {
                            data = "{\"type\":1,\"msg\":\"订单错误\"}";
                        }

                    }
                    catch
                    {
                        data = "{\"type\":1,\"msg\":\"操作错误\"}";
                    }
                    context.Response.Write(data);

                }



                if (oper == "sendticketsms")
                {
                    var comid = context.Request["comid"];
                    var oid = context.Request["oid"].ConvertTo<int>(0);
                    string data = OrderJsonData.SendTicketSms(comid, oid);

                    context.Response.Write(data);
                }
                if (oper == "restticketsms")
                {
                    var comid = context.Request["comid"];
                    var oid = context.Request["oid"].ConvertTo<int>(0);
                    string data = OrderJsonData.RestTicketSms(comid, oid);

                    context.Response.Write(data);
                }
                if (oper == "guoqi_biaozhu")
                {
                    var comid = context.Request["comid"];
                    var oid = context.Request["oid"].ConvertTo<int>(0);
                    string data = OrderJsonData.guoqi_biaozhu(comid, oid);

                    context.Response.Write(data);
                }
                
                if (oper == "weborder")
                {   //网站提交订单，使用预付款和积分
                    var comid = 0;
                    var proid = context.Request["proid"];
                    var speciid = context.Request["speciid"].ConvertTo<int>(0);
                    var ordertype = context.Request["ordertype"];
                    var payprice = context.Request["payprice"];
                    string sid = context.Request["sid"].ConvertTo<string>("");
                    decimal cost = 0;
                    decimal profit = 0;

                    var u_num = context.Request["u_num"];
                    var u_name = context.Request["u_name"];
                    var u_phone = context.Request["u_phone"];
                    var u_idcard = context.Request["u_idcard"].ConvertTo<string>("");
                    DateTime u_traveldate = context.Request["u_traveldate"].ConvertTo<DateTime>(DateTime.Now);

                    var Integral = context.Request["Integral"].ConvertTo<decimal>(0);
                    var Imprest = context.Request["Imprest"].ConvertTo<decimal>(0);

                    //新增乘车人信息
                    var travelnames = context.Request["travelnames"].ConvertTo<string>("");
                    var travelidcards = context.Request["travelidcards"].ConvertTo<string>("");
                    var travelnations = context.Request["travelnations"].ConvertTo<string>("");
                    var travel_pickuppoints = context.Request["travel_pickuppoints"].ConvertTo<string>("");
                    var travel_dropoffpoints = context.Request["travel_dropoffpoints"].ConvertTo<string>("");

                    var travelphones = context.Request["travelphones"].ConvertTo<string>("");
                    var travelremarks = context.Request["travelremarks"].ConvertTo<string>("");

                    var order_remark = context.Request["order_remark"].ConvertTo<string>("");

                    decimal Integral_user = 0;
                    decimal Imprest_user = 0;
                    string data = "";

                    var uid = context.Request["uid"].ConvertTo<int>(0);//用户ID

                    //新增被保险人信息
                    var baoxiannames = context.Request["baoxiannames"].ConvertTo<string>("");
                    var baoxianpinyinnames = context.Request["baoxianpinyinnames"].ConvertTo<string>("");
                    var baoxianidcards = context.Request["baoxianidcards"].ConvertTo<string>("");
                    var payserverpno = context.Request["payserverpno"].ConvertTo<string>("");

                    //获取验证码
                    string getcode = context.Request["getcode"].ConvertTo<string>("");
                    string initcode = "";
                    int orderid = 0;
                    string errinfo = "OK";

                    //if (getcode != "")
                    //{
                    //        if (context.Session["SomeValidateCode"] != null)
                    //        {
                    //            initcode = context.Session["SomeValidateCode"].ToString();
                    //        }
                    //        if (getcode != initcode)
                    //        {
                    //            errinfo = "err";
                    //            data = "{\"type\":1,\"msg\":\"验证码错误\"}";
                    //        }
                    //}
                    lock (lockobj)
                    {
                        //如果验证码错误，返回
                        if (errinfo == "OK")
                        {

                            B2b_com_pro pro = new B2bComProData().GetProById(proid.ToString());
                            if (pro != null)
                            {
                                if (pro.Server_type == 10)//旅游大巴
                                {
                                    cost = 0;
                                    profit = 0;
                                    //旅游大巴 赠送保险，保险资料=旅游大巴提交资料
                                    baoxiannames = travelnames;
                                    baoxianpinyinnames = "";
                                    baoxianidcards = travelidcards;
                                }
                                else
                                {
                                    cost = pro.Agentsettle_price;
                                    profit = pro.Advise_price - cost;
                                    payprice = pro.Advise_price.ToString();


                                    //如果是多规格产品，没有选择选择规格，不能提交订单
                                    if (pro.Manyspeci == 1)
                                    {
                                        if (speciid == 0)
                                        {
                                            data = "{\"type\":1,\"msg\":\"未选择具体规格，请选择规格重新提交\"}";
                                            context.Response.Write(data);
                                            return;

                                        }
                                    }

                                    if (speciid != 0)
                                    { //如果带规格，读取规格价格

                                        B2b_com_pro prospeciid = new B2bComProData().GetProspeciidById(proid.ToString(), speciid);
                                        if (prospeciid != null)
                                        {

                                            cost = prospeciid.Agentsettle_price;
                                            profit = prospeciid.Advise_price - cost;
                                            payprice = prospeciid.Advise_price.ToString("0.00");

                                            //如果库存
                                            if (prospeciid.Ispanicbuy == 1)
                                            {
                                                if (prospeciid.Limitbuytotalnum < Int32.Parse(u_num))
                                                {
                                                    data = "{\"type\":1,\"msg\":\"库存票不足，请电话订购或联系商家\"}";
                                                    context.Response.Write(data);
                                                    return;
                                                }
                                            }

                                        }
                                    }


                                }
                                comid = pro.Com_id;
                                if (pro.Pro_state == 0)
                                {
                                    data = "{\"type\":1,\"msg\":\"产品已暂停\"}";
                                    context.Response.Write(data);
                                    return;
                                }

                                if (pro.Pro_number != 0)
                                {
                                    if (int.Parse(u_num) > pro.Pro_number)
                                    {
                                        data = "{\"type\":1,\"msg\":\"此产品限购 " + pro.Pro_number + "张，请重新提交订单！\"}";
                                        context.Response.Write(data);
                                        return;
                                    }
                                }

                                if (pro.Pro_end < DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
                                {
                                    data = "{\"type\":1,\"msg\":\"产品已过期\"}";
                                    context.Response.Write(data);
                                    return;
                                }
                                //库存票检查库存数量，不足则不让提交订单
                                if (pro.Source_type == 2)
                                {
                                    int kucunpiaoshuliang = new B2bComProData().ProSEPageCount_UNUse(pro.Id);
                                    if (kucunpiaoshuliang < Int32.Parse(u_num))
                                    {
                                        data = "{\"type\":1,\"msg\":\"库存票不足，请电话订购或联系商家\"}";
                                        context.Response.Write(data);
                                        return;
                                    }
                                }



                            }

                            B2bCrmData b2b_crm = new B2bCrmData();
                            if (uid == 0)
                            {//没用户ID的预订款和积分为0
                                Integral = 0;
                                Imprest = 0;
                            }
                            if (Integral > 0)
                            {
                                //读取用户积分
                                var b2b_model = b2b_crm.Readuser(uid, comid);
                                if (b2b_model != null)
                                {
                                    Integral_user = b2b_model.Integral;
                                }
                                //判断用户实际积分 要大于等于要使用的积分
                                if (Integral_user < Integral)
                                {
                                    Integral = Integral_user;
                                }

                                //使用的积分如果大于等于订单金额
                                if (Integral >= int.Parse(u_num) * int.Parse(payprice))
                                {
                                    Integral = int.Parse(u_num) * int.Parse(payprice);
                                    Imprest = 0;//当积分足以支付订单，使用预付款直接为0
                                }
                                else
                                {//小于订单金额
                                    //使用积分不用做处理，按以上使用积分计算
                                }
                            }

                            if (Imprest > 0)
                            { //使用预付款大于0
                                //读取预付款记录
                                var b2b_model = b2b_crm.Readuser(uid, comid);
                                if (b2b_model != null)
                                {
                                    Imprest_user = b2b_model.Imprest;
                                }

                                //判断用户实际积分 要大于等于要使用的积分
                                if (Imprest_user < Imprest)
                                {
                                    Imprest = Imprest_user;
                                }

                                //使用的预付款 大于 订单金额-积分（如果使用了则减未使用则-0）
                                if (Imprest >= (int.Parse(u_num) * int.Parse(payprice)) - Integral)
                                {
                                    Imprest = (int.Parse(u_num) * int.Parse(payprice)) - Integral;//直接按实际订单所需使用预付款
                                }
                                else
                                {
                                    //使用预付款不予处理，安指定预付款处理
                                }
                            }

                            if (sid != "")
                            {
                                decimal serverprice_temp = 0;
                                var id_arr = sid.Split(',');
                                for (int i = 0; i < id_arr.Count(); i++)
                                {
                                    if (id_arr[i].Trim() != "")
                                    {
                                        var Rentserver_temp = new RentserverData().Rentserverbyid(int.Parse(id_arr[i].Trim()), comid);
                                        if (Rentserver_temp != null)
                                        {
                                            serverprice_temp += Rentserver_temp.saleprice + Rentserver_temp.serverDepositprice;
                                        }
                                    }
                                }

                                payprice = (decimal.Parse(payprice) + serverprice_temp).ToString("0.00");
                            }

                            B2b_order order = new B2b_order()
                            {
                                Id = 0,
                                Pro_id = proid.ConvertTo<int>(0),
                                Speciid = speciid,
                                Order_type = ordertype.ConvertTo<int>(1),
                                U_name = u_name,
                                U_id = uid,
                                U_phone = u_phone,
                                U_idcard = u_idcard,
                                U_num = u_num.ConvertTo<int>(0),
                                U_subdate = DateTime.Now,
                                Payid = 0,
                                Pay_price = payprice.ConvertTo<decimal>(0),
                                Cost = cost,
                                Profit = profit,
                                Order_state = (int)OrderStatus.WaitPay,//
                                Pay_state = (int)PayStatus.NotPay,//支付模块未做出来前先设置为已支付
                                Send_state = (int)SendCodeStatus.NotSend,
                                Ticketcode = 0,//电子码未创建，支付后产生码赋值
                                Rebate = 0,//  利润返佣金额暂时定为0
                                Ordercome = "",//订购来源 暂时定为空
                                U_traveldate = u_traveldate,
                                Dealer = "自动",
                                Comid = comid,
                                Pno = "",
                                Openid = "",
                                Ticketinfo = "",

                                Integral1 = Integral,//积分
                                Imprest1 = Imprest,//预付款
                                Agentid = 0,     //分销ID
                                Warrantid = 0,   //授权ID
                                Warrant_type = 1,  //支付类型分销 1出票扣款 2验码扣款

                                pickuppoint = travel_pickuppoints,
                                dropoffpoint = travel_dropoffpoints,

                                Order_remark = order_remark,
                                baoxiannames = baoxiannames,
                                baoxianpinyinnames = baoxianpinyinnames,
                                baoxianidcards = baoxianidcards,
                                travelnames = travelnames,
                                travelphones = travelphones,
                                serverid = sid,
                                payserverpno = payserverpno,

                                travelidcards = travelidcards,
                                travelnations = travelnations,

                                travelremarks = travelremarks
                            };

                            try
                            {
                                data = OrderJsonData.InsertOrUpdate(order, out orderid);

                                #region 已注释
                                ////判断提交订单是否成功，成功录入订单子表(订单乘车人信息表)
                                //if (orderid > 0)
                                //{
                                //    int servertype = new B2bComProData().GetProServer_typeById(proid);
                                //    if (servertype == 10)
                                //    {
                                //        //对订单查询如果导入产品产生订单，插入乘车人插入b订单
                                //        var b2borderinfo = new B2bOrderData().GetOrderById(orderid);
                                //        if (b2borderinfo != null)
                                //        {
                                //            if (b2borderinfo.Bindingagentorderid != 0)
                                //            {
                                //                orderid = b2borderinfo.Bindingagentorderid;
                                //                var b2borderinfo_B = new B2bOrderData().GetOrderById(orderid);
                                //                if (b2borderinfo_B != null)
                                //                {
                                //                    comid = b2borderinfo_B.Comid;
                                //                    proid = b2borderinfo_B.Pro_id.ToString();
                                //                }
                                //                b2borderinfo = b2borderinfo_B;
                                //            }

                                //        }
                                //        for (int i = 1; i <= u_num.ConvertTo<int>(0); i++)
                                //        {
                                //            string travelname = travelnames.Split(',')[i - 1];
                                //            string travelidcard = travelidcards.Split(',')[i - 1];
                                //            string travelnation = "";
                                //            if (travelnations != "")
                                //            {
                                //                travelnation = travelnations.Split(',')[i - 1];
                                //            }
                                //            string travelphone = travelphones.Split(',')[i - 1];
                                //            string travelremark = "";
                                //            if (travelremarks != "")
                                //            {
                                //                travelremark = travelremarks.Split(',')[i - 1];
                                //            }
                                //            string travel_pickuppoint = travel_pickuppoints;
                                //            string travel_dropoffpoint = travel_dropoffpoints;

                                //            int rt = new B2bOrderData().Insertb2b_order_busNamelist(orderid, travelname, travelidcard, travelnation, u_name, u_phone, DateTime.Now, u_num, u_traveldate.ToString("yyyy-MM-dd HH:mm:ss"), comid, b2borderinfo.Agentid, proid.ConvertTo<int>(0), travel_pickuppoint, travel_dropoffpoint, travelphone, travelremark);

                                //        }
                                //    }
                                //}
                                #endregion

                            }
                            catch
                            {
                                data = "";
                            }
                        }
                        context.Response.Write(data);
                    }
                }


                if (oper == "sohuorder")
                {   //网站提交订单，使用预付款和积分
                    var comid = 0;

                    var ordertype = context.Request["ordertype"];
                    var payprice = context.Request["payprice"];

                    decimal cost = 0;
                    decimal profit = 0;

                    var u_num = context.Request["u_num"];
                    var u_name = context.Request["u_name"];
                    var u_phone = context.Request["u_phone"];

                    var proid = context.Request["proid"].ConvertTo<int>(0); ;
                    var Integral = context.Request["Integral"].ConvertTo<decimal>(0);
                    var Imprest = context.Request["Imprest"].ConvertTo<decimal>(0);
                    var u_traveldate = context.Request["u_traveldate"].ConvertTo<DateTime>(); ;
                    string data = "";

                    var uid = context.Request["uid"].ConvertTo<int>(0);//用户ID

                    //获取验证码
                    string getcode = context.Request["getcode"].ConvertTo<string>("");
                    string initcode = "";

                    string errinfo = "OK";

                    if (getcode != "")
                    {
                        if (context.Session["SomeValidateCode"] != null)
                        {
                            initcode = context.Session["SomeValidateCode"].ToString();
                        }
                        if (getcode != initcode)
                        {
                            errinfo = "err";
                            data = "{\"type\":1,\"msg\":\"验证码错误\"}";
                        }
                    }
                    else
                    {
                        errinfo = "err";
                        data = "{\"type\":1,\"msg\":\"请填写验证码\"}";
                    }

                    //如果验证码错误，返回
                    if (errinfo == "OK")
                    {
                        //读取产品
                        B2b_com_pro pro = new B2bComProData().GetProById(proid.ToString());
                        if (pro != null)
                        {
                            cost = pro.Agentsettle_price;
                            profit = pro.Advise_price - cost;
                            payprice = pro.Advise_price.ToString();
                            comid = pro.Com_id;
                        }

                        //读取団期价格
                        B2b_com_LineGroupDate linemode = new B2b_com_LineGroupDateData().GetLineDayGroupDate(u_traveldate, proid);
                        if (linemode != null)
                        {
                            payprice = linemode.Menprice.ToString();
                        }


                        B2bCrmData b2b_crm = new B2bCrmData();
                        if (uid == 0)
                        {//没用户ID的预订款和积分为0
                            Integral = 0;
                            Imprest = 0;
                        }

                        B2b_order order = new B2b_order()
                        {
                            Id = 0,
                            Pro_id = proid,
                            Order_type = ordertype.ConvertTo<int>(1),
                            U_name = u_name,
                            U_id = uid,
                            U_phone = u_phone,
                            U_num = u_num.ConvertTo<int>(0),
                            U_subdate = DateTime.Now,
                            Payid = 0,
                            Pay_price = payprice.ConvertTo<decimal>(0),
                            Cost = cost,
                            Profit = profit,
                            Order_state = (int)OrderStatus.WaitPay,//
                            Pay_state = (int)PayStatus.NotPay,//支付模块未做出来前先设置为已支付
                            Send_state = (int)SendCodeStatus.NotSend,
                            Ticketcode = 0,//电子码未创建，支付后产生码赋值
                            Rebate = 0,//  利润返佣金额暂时定为0
                            Ordercome = "sohu",//订购来源 暂时定为空
                            U_traveldate = u_traveldate,
                            Dealer = "自动",
                            Comid = comid,
                            Pno = "",
                            Openid = "",
                            Ticketinfo = "",

                            Integral1 = Integral,//积分
                            Imprest1 = Imprest,//预付款
                            Agentid = 0,     //分销ID
                            Warrantid = 0,   //授权ID
                            Warrant_type = 1  //支付类型分销 1出票扣款 2验码扣款
                        };

                        try
                        {
                            int orderid = 0;
                            data = OrderJsonData.InsertOrUpdate(order, out orderid);
                        }
                        catch
                        {
                            data = "";
                        }
                    }
                    context.Response.Write(data);

                }


                if (oper == "lineorder")
                {   //线路订单
                    var comid = 0;

                    var ordertype = context.Request["ordertype"];
                    var payprice = context.Request["payprice"];

                    decimal cost = 0;
                    decimal profit = 0;

                    var u_num = context.Request["u_num"];//成人数量
                    var budget_child = context.Request["budget_child"];//儿童数量
                    var ticket_info = "成人:" + u_num + "儿童:" + budget_child;//暂时当备注用

                    var u_name = context.Request["u_name"];
                    var u_phone = context.Request["u_phone"];
                    var openid = context.Request["openid"].ConvertTo<string>("");


                    var proid = context.Request["proid"].ConvertTo<int>(0);//线路ID
                    var Integral = context.Request["Integral"].ConvertTo<decimal>(0);
                    var Imprest = context.Request["Imprest"].ConvertTo<decimal>(0);
                    var u_traveldate = context.Request["u_traveldate"].ConvertTo<DateTime>();//出行日期
                    string data = "";

                    var uid = context.Request["uid"].ConvertTo<int>(0);//用户ID

                    //获取验证码
                    string getcode = context.Request["getcode"].ConvertTo<string>("");
                    string initcode = "";

                    string errinfo = "OK";

                    //if (getcode != "")//暂时不需要验证码
                    //{
                    //    if (context.Session["SomeValidateCode"] != null)
                    //    {
                    //        initcode = context.Session["SomeValidateCode"].ToString();
                    //    }
                    //    if (getcode != initcode)
                    //    {
                    //        errinfo = "err";
                    //        data = "{\"type\":1,\"msg\":\"验证码错误\"}";
                    //    }
                    //}
                    //else
                    //{
                    //    errinfo = "err";
                    //    data = "{\"type\":1,\"msg\":\"请填写验证码\"}";
                    //}

                    lock (lockobj)
                    {


                        //string accountt = context.Request.Cookies["AccountId"].Value;
                        //如果验证码错误，返回
                        if (errinfo == "OK")
                        {
                            //读取产品
                            B2b_com_pro pro = new B2bComProData().GetProById(proid.ToString());
                            if (pro != null)
                            {
                                cost = pro.Agentsettle_price;
                                profit = pro.Advise_price - cost;
                                payprice = pro.Advise_price.ToString();
                                comid = pro.Com_id;
                            }

                            if (pro.Ispanicbuy == 1)
                            {//抢购产品判断微信端登陆，或页面登陆
                                //判断用户是否微信端登录
                                if (context.Request.Cookies["AccountId"] == null)
                                {
                                    context.Response.Write("{\"type\":\"1\",\"msg\":\"请微信端登录\"}");
                                    return;
                                }
                            }


                            //读取団期价格//根据实际选择的団期报价
                            B2b_com_LineGroupDate linemode = new B2b_com_LineGroupDateData().GetLineDayGroupDate(u_traveldate, proid);
                            if (linemode != null)
                            {
                                payprice = linemode.Menprice.ToString();
                            }


                            B2bCrmData b2b_crm = new B2bCrmData();
                            if (uid == 0)
                            {//没用户ID的预订款和积分为0
                                Integral = 0;
                                Imprest = 0;
                            }

                            B2b_order order = new B2b_order()
                            {
                                Id = 0,
                                Pro_id = proid,
                                Order_type = 1,
                                U_name = u_name,
                                U_id = uid,
                                U_phone = u_phone,
                                U_num = u_num.ConvertTo<int>(0),
                                U_subdate = DateTime.Now,
                                Payid = 0,
                                Pay_price = payprice.ConvertTo<decimal>(0),
                                Cost = cost,
                                Profit = profit,
                                Order_state = (int)OrderStatus.WaitPay,//
                                Pay_state = (int)PayStatus.NotPay,//支付模块未做出来前先设置为已支付
                                Send_state = (int)SendCodeStatus.NotSend,
                                Ticketcode = 0,//电子码未创建，支付后产生码赋值
                                Rebate = 0,//  利润返佣金额暂时定为0
                                Ordercome = "H5",//订购来源 暂时定为空
                                U_traveldate = u_traveldate,
                                Dealer = "自动",
                                Comid = comid,
                                Pno = "",
                                Openid = openid,
                                Ticketinfo = ticket_info,

                                Integral1 = Integral,//积分
                                Imprest1 = Imprest,//预付款
                                Agentid = 0,     //分销ID
                                Warrantid = 0,   //授权ID
                                Warrant_type = 1,  //支付类型分销 1出票扣款 2验码扣款

                                Child_u_num = budget_child.ConvertTo<int>(0)
                            };

                            int orderid = 0;
                            data = OrderJsonData.InsertOrUpdate(order, out orderid);

                        }
                        context.Response.Write(data);

                    }

                }

                if (oper == "coopcount")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var ordercome = "sohu";
                    string data = OrderJsonData.CoopOrderCount(comid, ordercome);

                    context.Response.Write(data);
                }
                if (oper == "coopcountlist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var ordercome = "sohu";

                    string data = OrderJsonData.CoopOrderPageList(comid, pageindex, pagesize, ordercome);

                    context.Response.Write(data);

                }
                if (oper == "coopcountverlist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var ordercome = "sohu";

                    string data = OrderJsonData.CoopVerOrderPageList(comid, pageindex, pagesize, ordercome);

                    context.Response.Write(data);

                }


                if (oper == "agentaddcart")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var proid = context.Request["proid"].ConvertTo<int>(0);
                    var speciid = context.Request["speciid"].ConvertTo<int>(0);
                    var cartid = context.Request["cartid"].ConvertTo<int>(0);
                    var agentid = context.Request["agentid"].ConvertTo<int>(0);//分销id
                    var userid = context.Request["userid"].ConvertTo<string>("");//用户ID或临时ID
                    var u_num = context.Request["u_num"].ConvertTo<int>(1);

                    string data = "";


                    lock (lockobj)
                    {
                        //当用户ID 有效时，按用户ID 执行
                        if (userid == "" || userid == "0")
                        {
                            if (context.Session["Agentid"] != null)
                            {
                                //分销账户信息
                                var Agentid_temp = Int32.Parse(context.Session["Agentid"].ToString());
                                var Account_temp = context.Session["Account"].ToString();

                                if (agentid != Agentid_temp)
                                {
                                    data = "{\"type\":1,\"msg\":\"分销账户不匹配,请重新登录\"}";
                                    context.Response.Write(data);
                                    return;
                                }

                            }
                            else
                            {
                                data = "{\"type\":1,\"msg\":\"分销账户不匹配,请重新登录\"}";
                                context.Response.Write(data);
                                return;
                            }
                        }


                        if (comid == 0)
                        {
                            data = "{\"type\":1,\"msg\":\"商户信息错误,请重新提交\"}";
                            context.Response.Write(data);
                            return;
                        }
                        if (proid == 0)
                        {
                            data = "{\"type\":1,\"msg\":\"产品信息错误,请重新提交\"}";
                            context.Response.Write(data);
                            return;
                        }
                        if (u_num == 0)
                        {
                            data = "{\"type\":1,\"msg\":\"订购数量错误,请重新提交\"}";
                            context.Response.Write(data);
                            return;
                        }

                        B2b_com_pro pro = new B2bComProData().GetProById(proid.ToString());
                        if (pro != null)
                        {

                            //如果是多规格产品，没有选择选择规格，不能提交订单
                            if (pro.Manyspeci == 1)
                            {
                                if (speciid == 0)
                                {
                                    data = "{\"type\":1,\"msg\":\"未选择具体规格，请选择规格重新提交\"}";
                                    context.Response.Write(data);
                                    return;

                                }
                            }

                        }

                        B2b_order order = new B2b_order()
                        {
                            Pro_id = proid,//没有产品
                            Speciid = speciid,//规格
                            Cartid = cartid,
                            Agentid = agentid,//为用户充值
                            Comid = comid,
                            U_num = u_num,
                            Openid = userid //使用OPENID 临时使用为 userid 因为存在超长数字等，做为字符存储

                        };
                        try
                        {
                            //当用户ID 有效时，按用户ID 执行

                            data = OrderJsonData.InsertCart(order);

                        }
                        catch
                        {
                            data = "";
                        }
                        context.Response.Write(data);

                    }
                }

                if (oper == "agentdelcart")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var cartid = context.Request["cartid"].ConvertTo<string>("");
                    var userid = context.Request["userid"].ConvertTo<string>("");//用户ID或临时ID
                    var agentid = context.Request["agentid"].ConvertTo<int>(0);//分销id

                    string data = "";


                    lock (lockobj)
                    {
                        //当用户ID 有效时，按用户ID 执行
                        if (userid == "" || userid == "0")
                        {
                            if (context.Session["Agentid"] != null)
                            {
                                //分销账户信息
                                var Agentid_temp = Int32.Parse(context.Session["Agentid"].ToString());
                                var Account_temp = context.Session["Account"].ToString();

                                if (agentid != Agentid_temp)
                                {
                                    data = "{\"type\":1,\"msg\":\"分销账户不匹配,请重新登录\"}";
                                    context.Response.Write(data);
                                    return;
                                }

                            }
                            else
                            {

                                data = "{\"type\":1,\"msg\":\"分销账户不匹配,请重新登录\"}";
                                context.Response.Write(data);
                                return;
                            }
                        }

                        if (comid == 0)
                        {
                            data = "{\"type\":1,\"msg\":\"商户信息错误,请重新提交\"}";
                            context.Response.Write(data);
                            return;
                        }
                        if (cartid == "")
                        {
                            data = "{\"type\":1,\"msg\":\"产品信息错误,请重新提交\"}";
                            context.Response.Write(data);
                            return;
                        }


                        B2b_order order = new B2b_order()
                        {

                            Agentid = agentid,//为用户充值
                            Comid = comid,
                            Openid = userid //使用OPENID 临时使用为 userid 因为存在超长数字等，做为字符存储
                        };
                        try
                        {
                            data = OrderJsonData.DelCart(order, cartid);
                        }
                        catch
                        {
                            data = "";
                        }
                        context.Response.Write(data);

                    }
                }


                if (oper == "agentreducecart")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var proid = context.Request["proid"].ConvertTo<int>(0);
                    var Speciid = context.Request["speciid "].ConvertTo<int>(0);
                    var userid = context.Request["userid"].ConvertTo<string>("");//用户ID或临时ID
                    var agentid = context.Request["agentid"].ConvertTo<int>(0);//分销id
                    var u_num = context.Request["u_num"].ConvertTo<int>(1);
                    var cartid = context.Request["cartid"].ConvertTo<int>(0);
                    string data = "";


                    lock (lockobj)
                    {
                        //当用户ID 有效时，按用户ID 执行
                        if (userid == "" || userid == "0")
                        {
                            if (context.Session["Agentid"] != null)
                            {
                                //分销账户信息
                                var Agentid_temp = Int32.Parse(context.Session["Agentid"].ToString());
                                var Account_temp = context.Session["Account"].ToString();

                                if (agentid != Agentid_temp)
                                {
                                    data = "{\"type\":1,\"msg\":\"分销账户不匹配,请重新登录\"}";
                                    context.Response.Write(data);
                                    return;
                                }

                            }
                            else
                            {

                                data = "{\"type\":1,\"msg\":\"分销账户不匹配,请重新登录\"}";
                                context.Response.Write(data);
                                return;
                            }
                        }

                        if (comid == 0)
                        {
                            data = "{\"type\":1,\"msg\":\"商户信息错误,请重新提交\"}";
                            context.Response.Write(data);
                            return;
                        }
                        if (u_num == 0)
                        {
                            data = "{\"type\":1,\"msg\":\"订购数量错误,请重新提交\"}";
                            context.Response.Write(data);
                            return;
                        }

                        B2b_order order = new B2b_order()
                        {
                            Pro_id = proid,//没有产品
                            Speciid = Speciid,
                            Cartid = cartid,
                            Agentid = agentid,//为用户充值
                            Comid = comid,
                            U_num = u_num,
                            Openid = userid //使用OPENID 临时使用为 userid 因为存在超长数字等，做为字符存储

                        };
                        try
                        {
                            data = OrderJsonData.UpCartNum(order);
                        }
                        catch
                        {
                            data = "";
                        }
                        context.Response.Write(data);

                    }
                }
                if (oper == "agentsearchcartcount")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var agentid = context.Request["agentid"].ConvertTo<int>(0);//分销id

                    string data = "";


                    lock (lockobj)
                    {
                        if (context.Session["Agentid"] != null)
                        {
                            //分销账户信息
                            var Agentid_temp = Int32.Parse(context.Session["Agentid"].ToString());
                            var Account_temp = context.Session["Account"].ToString();

                            if (agentid != Agentid_temp)
                            {
                                data = "{\"type\":1,\"msg\":\"分销账户不匹配,请重新登录\"}";
                                context.Response.Write(data);
                                return;
                            }

                        }
                        else
                        {

                            data = "{\"type\":1,\"msg\":\"分销账户不匹配,请重新登录\"}";
                            context.Response.Write(data);
                            return;
                        }


                        if (comid == 0)
                        {
                            data = "{\"type\":1,\"msg\":\"商户信息错误,请重新提交\"}";
                            context.Response.Write(data);
                            return;
                        }


                        try
                        {
                            data = OrderJsonData.SearchCartCount(comid, agentid);
                        }
                        catch
                        {
                            data = "";
                        }
                        context.Response.Write(data);

                    }
                }
                if (oper == "agentsearchcart")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var agentid = context.Request["agentid"].ConvertTo<int>(0);//分销id
                    var userid = context.Request["userid"].ConvertTo<string>("");//分销id
                    string data = "";


                    lock (lockobj)
                    {

                        if (userid == "" || userid == "0")
                        {

                            if (context.Session["Agentid"] != null)
                            {
                                //分销账户信息
                                var Agentid_temp = Int32.Parse(context.Session["Agentid"].ToString());
                                var Account_temp = context.Session["Account"].ToString();

                                if (agentid != Agentid_temp)
                                {
                                    data = "{\"type\":1,\"msg\":\"分销账户不匹配,请重新登录\"}";
                                    context.Response.Write(data);
                                    return;
                                }

                            }
                            else
                            {

                                data = "{\"type\":1,\"msg\":\"分销账户不匹配,请重新登录\"}";
                                context.Response.Write(data);
                                return;
                            }
                        }


                        if (comid == 0)
                        {
                            data = "{\"type\":1,\"msg\":\"商户信息错误,请重新提交\"}";
                            context.Response.Write(data);
                            return;
                        }

                        B2b_order order = new B2b_order()
                        {

                            Agentid = agentid,//为用户充值
                            Comid = comid,
                            Openid = userid,
                        };
                        try
                        {

                            data = OrderJsonData.SearchCart(order);

                        }
                        catch
                        {
                            data = "";
                        }
                        context.Response.Write(data);

                    }
                }

                if (oper == "agentcartlist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var agentid = context.Request["agentid"].ConvertTo<int>(0);//分销id
                    var proid = context.Request["proid"].ConvertTo<string>("");//
                    var cartid = context.Request["cartid"].ConvertTo<string>("");//
                    var speciid = context.Request["speciid"].ConvertTo<string>("");//
                    string data = "";


                    lock (lockobj)
                    {
                        if (context.Session["Agentid"] != null)
                        {
                            //分销账户信息
                            var Agentid_temp = Int32.Parse(context.Session["Agentid"].ToString());
                            var Account_temp = context.Session["Account"].ToString();

                            if (agentid != Agentid_temp)
                            {
                                data = "{\"type\":1,\"msg\":\"分销账户不匹配,请重新登录\"}";
                                context.Response.Write(data);
                                return;
                            }

                        }
                        else
                        {

                            data = "{\"type\":1,\"msg\":\"分销账户不匹配,请重新登录\"}";
                            context.Response.Write(data);
                            return;
                        }


                        if (comid == 0)
                        {
                            data = "{\"type\":1,\"msg\":\"商户信息错误,请重新提交\"}";
                            context.Response.Write(data);
                            return;
                        }

                        B2b_order order = new B2b_order()
                        {

                            Agentid = agentid,//为用户充值
                            Comid = comid,
                        };
                        try
                        {
                            data = OrderJsonData.SearchCartList(order, proid, cartid);
                        }
                        catch
                        {
                            data = "{\"type\":1,\"msg\":\"异常错误！\"}";
                        }
                        context.Response.Write(data);

                    }
                }

                if (oper == "usercartlist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var userid = context.Request["userid"].ConvertTo<string>("");//分销id
                    var proid = context.Request["proid"].ConvertTo<string>("");//
                    var speciid = context.Request["speciid"].ConvertTo<string>("");//
                    string data = "";


                    lock (lockobj)
                    {
                        if (userid == "" || userid == "0")
                        {

                            data = "{\"type\":1,\"msg\":\"用户信息丢失，请刷新重新加入购物车\"}";
                            context.Response.Write(data);
                            return;
                        }


                        if (comid == 0)
                        {
                            data = "{\"type\":1,\"msg\":\"商户信息错误,请重新提交\"}";
                            context.Response.Write(data);
                            return;
                        }

                        B2b_order order = new B2b_order()
                        {

                            Openid = userid,
                            Comid = comid,
                        };
                        try
                        {
                            data = OrderJsonData.SearchUserCartList(order, proid, speciid);
                        }
                        catch
                        {
                            data = "{\"type\":1,\"msg\":\"异常错误！\"}";
                        }
                        context.Response.Write(data);

                    }
                }

                if (oper == "userorderprolist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var userid = context.Request["userid"].ConvertTo<int>(0);//yonghuID
                    var cartid = context.Request["cartid"].ConvertTo<int>(0);//购物城订单ID
                    var proid = context.Request["proid"].ConvertTo<string>("");//产品 ID
                    string data = "";


                    lock (lockobj)
                    {

                        if (comid == 0)
                        {
                            data = "{\"type\":1,\"msg\":\"商户信息错误,请重新提交\"}";
                            context.Response.Write(data);
                            return;
                        }


                        try
                        {
                            data = OrderJsonData.SearchUserOrderList(comid, cartid, proid);
                        }
                        catch
                        {
                            data = "{\"type\":1,\"msg\":\"异常错误！\"}";
                        }
                        context.Response.Write(data);

                    }
                }


                if (oper == "agentorder")
                {
                    //网站提交订单，使用预付款和积分
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var proid = context.Request["proid"].ConvertTo<int>(0);
                    var speciid = context.Request["speciid"].ConvertTo<int>(0);
                    var ordertype = context.Request["ordertype"];//1.出票2.充值

                    var u_childnum = context.Request["u_childnum"].ConvertTo<int>(0); //儿童数量
                    var u_num = (context.Request["u_num"].ConvertTo<int>(0) + context.Request["u_childnum"].ConvertTo<int>(0)).ToString();//总的提单数量

                    var u_name = context.Request["u_name"];
                    var u_phone = context.Request["u_phone"];
                    var u_idcard = context.Request["u_idcard"].ConvertTo<string>("");
                    var u_traveldate = context.Request["u_traveldate"];
                    var u_traveldate_out = context.Request["u_traveldate_out"];
                    var agentid = context.Request["agentid"].ConvertTo<int>(0);//用户ID

                    //新增乘车人信息
                    var travelnames = context.Request["travelnames"].ConvertTo<string>("");
                    var travelidcards = context.Request["travelidcards"].ConvertTo<string>("");
                    var travelnations = context.Request["travelnations"].ConvertTo<string>("");
                    var travel_pickuppoints = context.Request["travel_pickuppoints"].ConvertTo<string>("");
                    var travel_dropoffpoints = context.Request["travel_dropoffpoints"].ConvertTo<string>("");
                    var travelphones = context.Request["travelphones"].ConvertTo<string>("");
                    var travelremarks = context.Request["travelremarks"].ConvertTo<string>("");



                    var order_remark = context.Request["order_remark"].ConvertTo<string>("");


                    //新增实物运费
                    var deliverytype = context.Request["deliverytype"].ConvertTo<int>(0);
                    var province = context.Request["province"].ConvertTo<string>("");
                    var city = context.Request["city"].ConvertTo<string>("");
                    var address = context.Request["address"].ConvertTo<string>("");
                    var txtcode = context.Request["txtcode"].ConvertTo<string>("");

                    var saveaddress = context.Request["saveaddress"].ConvertTo<int>(0);//是否保存为常用地址，1为保存 0为不保存
                    int channelcoachid = context.Request["channelcoachid"].ConvertTo<int>(0);
                    //新增被保险人信息
                    var baoxiannames = context.Request["baoxiannames"].ConvertTo<string>("");
                    var baoxianpinyinnames = context.Request["baoxianpinyinnames"].ConvertTo<string>("");
                    var baoxianidcards = context.Request["baoxianidcards"].ConvertTo<string>("");





                    lock (lockobj)
                    {
                        int servertype = new B2bComProData().GetProServer_typeById(proid.ToString());
                        //旅游大巴产品 则被保险人信息=乘车人信息
                        if (servertype == 10)
                        {
                            baoxiannames = travelnames;
                            baoxianpinyinnames = "";
                            baoxianidcards = travelidcards;
                        }

                        B2b_order_hotel b2b_order_hotel = null;
                        if (servertype == 9)
                        {//服务类型为酒店客房，则给订单扩展表-酒店订单表添加数据
                            DateTime start_date = context.Request["start_date"].ConvertTo<DateTime>();
                            DateTime end_date = context.Request["end_date"].ConvertTo<DateTime>();
                            int bookdaynum = context.Request["bookdaynum"].ConvertTo<int>(0);
                            string lastarrivaltime = context.Request["lastarrivaltime"].ConvertTo<string>("");
                            string fangtai = context.Request["fangtai"].ConvertTo<string>("");

                            b2b_order_hotel = new B2b_order_hotel()
                            {
                                Id = 0,
                                Orderid = 0,
                                Start_date = start_date,
                                End_date = end_date,
                                Bookdaynum = bookdaynum,
                                Lastarrivaltime = lastarrivaltime,
                                Fangtai = fangtai
                            };
                        }



                        string agentaccount = "";//分销登录账号

                        string data = "";
                        //分销子分销判断授权金额
                        if (context.Session["Agentid"] != null)
                        {
                            //账户信息
                            var Agentid_temp = Int32.Parse(context.Session["Agentid"].ToString());
                            var Account_temp = context.Session["Account"].ToString();

                            if (agentid != Agentid_temp)
                            {
                                data = "{\"type\":1,\"msg\":\"分销账户不匹配,请重新登录\"}";
                                context.Response.Write(data);
                                return;
                            }

                            Agent_regiinfo agentinfo = AgentCompanyData.GetAgentAccountByUid(Account_temp, agentid);
                            if (agentinfo == null)
                            {
                                data = "{\"type\":1,\"msg\":\"分销账户信息丢失,请重新登录\"}";
                                context.Response.Write(data);
                                return;
                            }
                            else
                            {
                                agentaccount = Account_temp;
                            }
                        }
                        else
                        {

                            if (context.Request.Cookies["Agentid"] != null)
                            {
                                string accountmd5 = "";
                                string Account = "";
                                int Agentid_temp = int.Parse(context.Request.Cookies["Agentid"].Value);
                                if (context.Request.Cookies["AgentKey"] != null)
                                {
                                    accountmd5 = context.Request.Cookies["AgentKey"].Value;
                                }
                                if (context.Request.Cookies["Account"] != null)
                                {
                                    Account = context.Request.Cookies["Account"].Value;
                                }

                                var agentdata = AgentCompanyData.GetAgentByid(Agentid_temp);
                                if (agentdata != null)
                                {
                                    var returnmd5 = EncryptionHelper.ToMD5(Account + "lixh1210" + Agentid_temp, "UTF-8");
                                    if (returnmd5 == accountmd5)
                                    {
                                        context.Session["Agentid"] = Agentid_temp;
                                        context.Session["Account"] = Account;
                                    }
                                    else
                                    {

                                        data = "{\"type\":1,\"msg\":\"分销账户错误,请重新登录\"}";
                                        context.Response.Write(data);
                                        return;
                                    }

                                }
                                else
                                {

                                    data = "{\"type\":1,\"msg\":\"分销账户错误,请重新登录\"}";
                                    context.Response.Write(data);
                                    return;
                                }


                            }
                            else
                            {
                                data = "{\"type\":1,\"msg\":\"分销账户错误,请重新登录\"}";
                                context.Response.Write(data);
                                return;
                            }
                        }


                        //新添加条件字段:isInterfaceSub(是否是电子票接口提交的订单:0.否;1.是)
                        int isInterfaceSub = 0;
                        int orderid = 0;
                        data = OrderJsonData.AgentOrder(agentid, proid.ToString(), ordertype, u_num, u_name, u_phone, u_traveldate, agentaccount, isInterfaceSub, out orderid, 0, 1, travel_pickuppoints, travel_dropoffpoints, order_remark, deliverytype, province, city, address, txtcode, 0, 0, u_childnum, 0, speciid, baoxiannames, baoxianpinyinnames, baoxianidcards, channelcoachid, b2b_order_hotel, 0, travelnames, travelidcards, travelnations, travelphones, travelremarks,u_idcard);

                        //对常用地址保存，只要提交成功，
                        if (saveaddress == 1)
                        {
                            if (orderid > 0)
                            {
                                var savedata = OrderJsonData.SaveAddress(agentid, u_name, u_phone, province, city, address, txtcode);
                            }
                        }



                        if (orderid > 0)
                        {
                            #region 已经注释
                            //if (servertype == 10)
                            //{
                            //    #region  录入订单子表(订单乘车人信息表)
                            //    //对订单查询如果导入产品产生订单，插入乘车人插入b订单
                            //    var b2borderinfo = new B2bOrderData().GetOrderById(orderid);
                            //    if (b2borderinfo != null)
                            //    {
                            //        if (b2borderinfo.Bindingagentorderid != 0)
                            //        {
                            //            orderid = b2borderinfo.Bindingagentorderid;
                            //            var b2borderinfo_B = new B2bOrderData().GetOrderById(orderid);
                            //            if (b2borderinfo_B != null)
                            //            {
                            //                comid = b2borderinfo_B.Comid;
                            //                proid = b2borderinfo_B.Pro_id;
                            //            }
                            //            b2borderinfo = b2borderinfo_B;
                            //        }

                            //    }
                            //    if (travelnames != "")
                            //    {
                            //        for (int i = 1; i <= u_num.ConvertTo<int>(0); i++)
                            //        {
                            //            string travelname = travelnames.Split(',')[i - 1];
                            //            string travelidcard = travelidcards.Split(',')[i - 1];
                            //            string travelnation = travelnations.Split(',')[i - 1];
                            //            string travelphone = travelphones.Split(',')[i - 1];
                            //            string travelremark = travelremarks.Split(',')[i - 1];
                            //            string travel_pickuppoint = travel_pickuppoints;
                            //            string travel_dropoffpoint = travel_dropoffpoints;


                            //            int rt = new B2bOrderData().Insertb2b_order_busNamelist(orderid, travelname, travelidcard, travelnation, u_name, u_phone, DateTime.Now, u_num, u_traveldate, comid, b2borderinfo.Agentid, proid, travel_pickuppoint, travel_dropoffpoint, travelphone, travelremark);
                            //        }
                            //    }
                            //    #endregion
                            //    #region 赠送保险
                            //    OrderJsonData.ZengsongBaoxian(orderid);
                            //    #endregion
                            //}
                            #endregion

                            //得到订单状态
                            int dorderstatus = new B2bOrderData().GetOrderState(orderid.ToString());
                            if (dorderstatus == 2 || dorderstatus == 4)
                            {
                                #region 赠送保险
                                OrderJsonData.ZengsongBaoxian(orderid);
                                #endregion
                            }

                            //订单如果为充值订单，则 分销提醒短信状态修改
                            if (ordertype == "2")
                            {
                                //设置分销充值提醒短信通知为 处理成功
                                int r = new Agent_RechargeRemindSmsData().UpremindSmsState(agentid, comid, 1);
                            }
                        }


                        context.Response.Write(data);
                    }
                }

                if (oper == "agentorderpay")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var data = OrderJsonData.agentorder_shoudongchuli(id);
                    context.Response.Write(data);
                }


                if (oper == "agentcartorder")
                {
                    //网站提交订单，使用预付款和积分
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var proid = context.Request["proid"].ConvertTo<string>("");
                    var speciid = context.Request["speciid"].ConvertTo<string>("");
                    var ordertype = context.Request["ordertype"].ConvertTo<string>("1");//1.出票2.充值
                    var u_num = context.Request["u_num"].ConvertTo<string>("");
                    var u_name = context.Request["u_name"];
                    var u_phone = context.Request["u_phone"];
                    var u_traveldate = context.Request["u_traveldate"];
                    var agentid = context.Request["agentid"].ConvertTo<int>(0);//用户ID
                    var payprice = context.Request["payprice"].ConvertTo<decimal>(0);//用户ID



                    //新增乘车人信息
                    var travelnames = context.Request["travelnames"].ConvertTo<string>("");
                    var travelidcards = context.Request["travelidcards"].ConvertTo<string>("");
                    var travelnations = context.Request["travelnations"].ConvertTo<string>("");
                    var travel_pickuppoints = context.Request["travel_pickuppoints"].ConvertTo<string>("");
                    var travel_dropoffpoints = context.Request["travel_dropoffpoints"].ConvertTo<string>("");
                    var travelphones = context.Request["travelphones"].ConvertTo<string>("");
                    var travelremarks = context.Request["travelremarks"].ConvertTo<string>("");



                    var order_remark = context.Request["order_remark"].ConvertTo<string>("");


                    //新增实物运费
                    var deliverytype = context.Request["deliverytype"].ConvertTo<int>(0);
                    var province = context.Request["province"].ConvertTo<string>("");
                    var city = context.Request["city"].ConvertTo<string>("");
                    var address = context.Request["address"].ConvertTo<string>("");
                    var txtcode = context.Request["txtcode"].ConvertTo<string>("");

                    var saveaddress = context.Request["saveaddress"].ConvertTo<int>(0);//是否保存为常用地址，1为保存 0为不保存


                    lock (lockobj)
                    {
                        string agentaccount = "";//分销登录账号

                        string data = "";
                        //分销子分销判断授权金额
                        if (context.Session["Agentid"] != null)
                        {
                            //账户信息
                            var Agentid_temp = Int32.Parse(context.Session["Agentid"].ToString());
                            var Account_temp = context.Session["Account"].ToString();

                            if (agentid != Agentid_temp)
                            {
                                data = "{\"type\":1,\"msg\":\"分销账户不匹配,请重新登录\"}";
                                context.Response.Write(data);
                                return;
                            }

                            Agent_regiinfo agentinfo = AgentCompanyData.GetAgentAccountByUid(Account_temp, agentid);
                            if (agentinfo == null)
                            {
                                data = "{\"type\":1,\"msg\":\"分销账户信息丢失,请重新登录\"}";
                                context.Response.Write(data);
                                return;
                            }
                            else
                            {
                                agentaccount = Account_temp;
                            }
                        }
                        else
                        {

                            if (context.Request.Cookies["Agentid"] != null)
                            {
                                string accountmd5 = "";
                                string Account = "";
                                int Agentid_temp = int.Parse(context.Request.Cookies["Agentid"].Value);
                                if (context.Request.Cookies["AgentKey"] != null)
                                {
                                    accountmd5 = context.Request.Cookies["AgentKey"].Value;
                                }
                                if (context.Request.Cookies["Account"] != null)
                                {
                                    Account = context.Request.Cookies["Account"].Value;
                                }

                                var agentdata = AgentCompanyData.GetAgentByid(Agentid_temp);
                                if (agentdata != null)
                                {
                                    var returnmd5 = EncryptionHelper.ToMD5(Account + "lixh1210" + Agentid_temp, "UTF-8");
                                    if (returnmd5 == accountmd5)
                                    {
                                        context.Session["Agentid"] = Agentid_temp;
                                        context.Session["Account"] = Account;
                                    }
                                    else
                                    {

                                        data = "{\"type\":1,\"msg\":\"分销账户错误,请重新登录\"}";
                                        context.Response.Write(data);
                                        return;
                                    }

                                }
                                else
                                {

                                    data = "{\"type\":1,\"msg\":\"分销账户错误,请重新登录\"}";
                                    context.Response.Write(data);
                                    return;
                                }

                            }
                            else
                            {
                                data = "{\"type\":1,\"msg\":\"分销账户错误,请重新登录\"}";
                                context.Response.Write(data);
                                return;
                            }
                        }





                        //新添加条件字段:isInterfaceSub(是否是电子票接口提交的订单:0.否;1.是)
                        int isInterfaceSub = 0;
                        int orderid = 0;

                        int shopcartid = 0;//购物车订单号
                        decimal expressfee = 0;//快递费

                        //购物车暂时只有实物，如果有电子票的需要调整，大巴，旅游的不支持

                        if (proid == "")
                        {
                            data = "{\"type\":1,\"msg\":\"产品信息错误,请重新提交\"}";
                            context.Response.Write(data);
                            return;
                        }

                        if (u_num == "")
                        {
                            data = "{\"type\":1,\"msg\":\"订购数量,请重新提交\"}";
                            context.Response.Write(data);
                            return;
                        }

                        var proid_arr = proid.Split(',');
                        var speciid_arr = speciid.Split(',');
                        var u_num_arr = u_num.Split(',');

                        if (proid_arr.Count() != u_num_arr.Count())
                        {
                            data = "{\"type\":1,\"msg\":\"产品与订购数量不匹配,请重新提交\"}";
                            context.Response.Write(data);
                            return;
                        }

                        //计算总快递费用
                        string feedetail = "";
                        expressfee = new B2b_delivery_costData().Getdeliverycost_ShopCart(proid, city, u_num, out feedetail);
                        if (expressfee == -1)
                        {
                            data = "{\"type\":1,\"msg\":\"快递费计算错误\"}";
                            context.Response.Write(data);
                            return;
                        }

                        //根据数量或重量平均分配快递费

                        var expressfee_arr = feedetail.Split(',');
                        if (proid_arr.Count() != expressfee_arr.Count())
                        {
                            data = "{\"type\":1,\"msg\":\"产品与快递费不匹配,请重新提交\"}";
                            context.Response.Write(data);
                            return;
                        }


                        //检查预付款 是否 足够支付所有订单，如果不足则直接跳出
                        //读取分销商信息
                        Agent_company agentinfo_temp = AgentCompanyData.GetAgentWarrant(agentid, comid);
                        if (agentinfo_temp == null)
                        {
                            data = "{\"type\":1,\"msg\":\"账户信息错误\"}";
                            context.Response.Write(data);
                            return;
                        }
                        if (deliverytype == 4)
                        {
                            expressfee = 0;
                        }


                        //每笔订单金额必须不能超出预付款金额 payprice 传递过来总支付金额，实际金额 按提交是算这只是做一次验证是否金额够支付
                        if ((agentinfo_temp.Imprest + agentinfo_temp.Credit) < (payprice + expressfee) && agentinfo_temp.Warrant_type == 1 && int.Parse(ordertype) == 1)
                        {
                            decimal paymoney = payprice + expressfee - agentinfo_temp.Imprest;

                            data = "{\"type\":1,\"msg\":\"yfkbz\",\"money\":\"" + paymoney + "\"}";
                            context.Response.Write(data);
                            return;
                        }

                        //生成购物车订单
                        B2b_order cartorderinfo = new B2b_order()
                        {
                            Order_type = ordertype.ConvertTo<int>(1),
                            U_name = u_name,
                            U_phone = u_phone,
                            Pay_price = payprice,
                            Comid = comid,
                            Pno = "",
                            Openid = "",
                            Integral1 = 0,   //积分
                            Imprest1 = 0,    //预付款
                            Agentid = agentid,     //分销ID
                            Province = province,
                            City = city,
                            Address = address,
                            Code = txtcode,
                            Express = expressfee
                        };
                        shopcartid = new B2bOrderData().CartInsertOrUpdate(cartorderinfo);


                        for (int i = 0; i < proid_arr.Count(); i++)
                        {
                            //计算快递费，如果 自取的 不算快递费
                            decimal express_temp = decimal.Parse(expressfee_arr[i]);
                            if (deliverytype == 4)
                            {
                                express_temp = 0;
                            }

                            data = OrderJsonData.AgentOrder(agentid, proid_arr[i], ordertype, u_num_arr[i], u_name, u_phone, u_traveldate, agentaccount, isInterfaceSub, out orderid, 0, 1, travel_pickuppoints, travel_dropoffpoints, order_remark, deliverytype, province, city, address, txtcode, shopcartid, express_temp, 0, 0, int.Parse(speciid_arr[i]));

                            B2b_order orderdelcart = new B2b_order()
                            {
                                Pro_id = int.Parse(proid_arr[i]),
                                Speciid = int.Parse(speciid_arr[i]),
                                Comid = comid,
                                Agentid = agentid,     //分销ID
                            };

                            var redata = new B2bOrderData().DelCart(orderdelcart, ""); ;
                        }
                        //对常用地址保存，只要提交成功，
                        if (saveaddress == 1)
                        {
                            if (orderid > 0)
                            {
                                var savedata = OrderJsonData.SaveAddress(agentid, u_name, u_phone, province, city, address, txtcode);
                            }
                        }

                        context.Response.Write(data);
                    }
                }

                if (oper == "sumprice")
                {

                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var proid = context.Request["proid"].ConvertTo<string>("");
                    var cartid = context.Request["cartid"].ConvertTo<string>("");
                    var Speciid = context.Request["Speciid"].ConvertTo<string>("");
                    var userid = context.Request["userid"].ConvertTo<string>("");//用户ID或临时ID
                    var u_num = context.Request["u_num"].ConvertTo<int>(1);

                    if (cartid != "")
                    {//去掉右侧的 ，
                        if (cartid.Substring(cartid.Length - 1, 1) == ",")
                        {
                            cartid = cartid.Substring(0, cartid.Length - 1);
                        }
                    }


                    string data = "";


                    lock (lockobj)
                    {
                        //当用户ID 有效时，按用户ID 执行
                        if (userid == "" || userid == "0")
                        {
                            data = "{\"type\":1,\"msg\":\"账户信息丢失,请重新刷新后提交\"}";
                            context.Response.Write(data);
                            return;
                        }


                        if (comid == 0)
                        {
                            data = "{\"type\":1,\"msg\":\"商户信息错误,请重新提交\"}";
                            context.Response.Write(data);
                            return;
                        }
                        if (cartid == "")
                        {
                            data = "{\"type\":1,\"msg\":\"产品信息错误,请重新提交\"}";
                            context.Response.Write(data);
                            return;
                        }

                        try
                        {
                            //统计

                            data = OrderJsonData.SumCartPrice(comid, userid, cartid);

                        }
                        catch
                        {
                            data = "";
                        }
                        context.Response.Write(data);
                    }
                }

                if (oper == "agentRecharge")
                {//
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var proid = context.Request["proid"];
                    var ordertype = context.Request["ordertype"];
                    var handlid = context.Request["handlid"].ConvertTo<int>(0);
                    decimal payprice = context.Request["payprice"].ConvertTo<decimal>(0);
                    decimal cost = 0;
                    decimal profit = 0;

                    var u_num = 1;
                    var u_name = context.Request["u_name"];
                    var u_phone = context.Request["u_phone"];
                    var u_traveldate = DateTime.Now;


                    var agentid = context.Request["agentid"].ConvertTo<int>(0);//分销ID
                    int warrantid = 0;
                    int Warrant_type = 0;
                    int Warrant_level = 0;
                    Agent_company agentwarrantinfo = AgentCompanyData.GetAgentWarrant(agentid, comid);
                    if (agentwarrantinfo != null)
                    {
                        warrantid = agentwarrantinfo.Warrantid;
                        Warrant_type = agentwarrantinfo.Warrant_type;
                        Warrant_level = agentwarrantinfo.Warrant_level;
                    }

                    B2b_order order = new B2b_order()
                    {
                        Id = 0,
                        Pro_id = 0,//没有产品
                        Speciid = 0,
                        Order_type = 2,//为用户充值
                        U_name = u_name,
                        U_id = 0,
                        U_phone = u_phone,
                        U_num = u_num,
                        U_subdate = DateTime.Now,
                        Payid = 0,
                        Pay_price = payprice,
                        Cost = cost,
                        Profit = profit,
                        Order_state = (int)OrderStatus.WaitPay,//
                        Pay_state = (int)PayStatus.NotPay,//支付模块未做出来前先设置为已支付
                        Send_state = (int)SendCodeStatus.NotSend,
                        Ticketcode = 0,//电子码未创建，支付后产生码赋值
                        Rebate = 0,//  利润返佣金额暂时定为0
                        Ordercome = "",//订购来源 暂时定为空
                        U_traveldate = u_traveldate,
                        Dealer = "自动",
                        Comid = comid,
                        Pno = "",
                        Openid = "",
                        Ticketinfo = "",
                        Integral1 = 0,   //积分
                        Imprest1 = 0,    //预付款
                        Agentid = agentid,     //分销ID
                        Warrantid = warrantid,   //授权ID
                        Warrant_type = Warrant_type,  //支付类型分销 1出票扣款 2验码扣款
                        Bindingagentorderid = 0,
                        Handlid = handlid

                    };
                    string data = "";
                    try
                    {
                        int orderid = 0;
                        data = OrderJsonData.InsertOrUpdate(order, out orderid);
                    }
                    catch
                    {
                        data = "";
                    }
                    context.Response.Write(data);
                }

                if (oper == "createorder")//微信提交订单，使用预付款和积分
                {
                    var ignoredabatime = context.Request["ignoredabatime"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var proid = context.Request["proid"];
                    var speciid = context.Request["speciid"].ConvertTo<int>(0);
                    var ordertype = context.Request["ordertype"];
                    var payprice = context.Request["payprice"];

                    var sid = context.Request["sid"].ConvertTo<string>("");//选择服务已,隔开（增加服务金额，及押金）

                    decimal cost = 0;
                    decimal profit = 0;

                    var u_num = context.Request["u_num"];
                    var u_name = context.Request["u_name"];
                    var u_phone = context.Request["u_phone"];
                    var u_idcard = context.Request["u_idcard"].ConvertTo<string>("");
                    var u_traveldate = context.Request["u_traveldate"].ConvertTo<string>(DateTime.Now.ToString("yyyy-MM-dd"));

                    var openid = context.Request["openid"].ConvertTo<string>("");

                    var Integral = context.Request["Integral"].ConvertTo<decimal>(0);
                    var Imprest = context.Request["Imprest"].ConvertTo<decimal>(0);

                    //新增乘车人信息
                    var travelnames = context.Request["travelnames"].ConvertTo<string>("");
                    var travelidcards = context.Request["travelidcards"].ConvertTo<string>("");
                    var travelnations = context.Request["travelnations"].ConvertTo<string>("");
                    var travelphones = context.Request["travelphones"].ConvertTo<string>("");
                    var travelremarks = context.Request["travelremarks"].ConvertTo<string>("");

                    var travel_pickuppoints = context.Request["travel_pickuppoints"].ConvertTo<string>("");
                    var travel_dropoffpoints = context.Request["travel_dropoffpoints"].ConvertTo<string>("");

                    var order_remark = context.Request["order_remark"].ConvertTo<string>("");
                    decimal Integral_user = 0;
                    decimal Imprest_user = 0;
                    string data = "";

                    var tocomid = context.Request["tocomid"].ConvertTo<int>(0);
                    var buyuid = context.Request["buyuid"].ConvertTo<int>(0);

                    int uid = context.Request["uid"].ConvertTo<int>(0);

                    var deliverytype = context.Request["deliverytype"].ConvertTo<int>(0);
                    string province = context.Request["province"].ConvertTo<string>("");//省市
                    string city = context.Request["city"].ConvertTo<string>("");//城市
                    string address = context.Request["address"].ConvertTo<string>("");//配送地址
                    string code = context.Request["code"].ConvertTo<string>("");//邮编
                    decimal express = context.Request["express"].ConvertTo<decimal>(0);//快递费
                    int channelcoachid = context.Request["channelcoachid"].ConvertTo<int>(0);

                    //新增被保险人信息
                    var baoxiannames = context.Request["baoxiannames"].ConvertTo<string>("");

                    var payserverpno = context.Request["payserverpno"].ConvertTo<string>("");

                    var baoxianpinyinnames = context.Request["baoxianpinyinnames"].ConvertTo<string>("");
                    var baoxianidcards = context.Request["baoxianidcards"].ConvertTo<string>("");
                    var yuyuepno = context.Request["pno"].ConvertTo<string>("");//加密的 预约验证码，自动成功
                    int autosuccess = context.Request["autosuccess"].ConvertTo<int>(0);
                    string submanagename = "";
                    if (autosuccess == 1)
                    { //自动成功，必须检查 是否为 管理员登陆权限
                        if (UserHelper.ValidateLogin())
                        {
                            B2b_company_manageuser user = UserHelper.CurrentUser();
                            B2b_company company = UserHelper.CurrentCompany;
                            var comid_temp = company.ID;//获取登陆账户公司id
                            submanagename = user.Accounts;
                            if (comid != comid_temp)
                            {
                                data = "{\"type\":1,\"msg\":\"" + comid + comid_temp + "登陆账户商户不符，请重新登录\"}";
                                context.Response.Write(data);
                                return;
                            }
                        }
                        else
                        {
                            data = "{\"type\":1,\"msg\":\"登陆账户信息错误，请从新登陆\"}";
                            context.Response.Write(data);
                            return;
                        }

                    }


                    lock (lockobj)
                    {

                        if (u_num.ConvertTo<int>(0) <= 0)
                        {
                            data = "{\"type\":1,\"msg\":\"订购数量出错\"}";
                            context.Response.Write(data);
                            return;
                        }



                        B2b_com_pro pro = new B2bComProData().GetProById(proid.ToString());
                        if (pro != null)
                        {
                            if (pro.Server_type == 10)//服务类型：旅游大巴
                            {
                                cost = 0;
                                profit = 0;
                                //旅游大巴 赠送保险，保险资料=旅游大巴提交资料
                                baoxiannames = travelnames;
                                baoxianpinyinnames = "";
                                baoxianidcards = travelidcards;
                            }
                            else
                            {
                                cost = pro.Agentsettle_price;
                                profit = pro.Advise_price - cost;
                            }
                            comid = pro.Com_id;
                            payprice = pro.Advise_price.ToString("0.00");

                            //如果是多规格产品，没有选择选择规格，不能提交订单
                            if (pro.Manyspeci == 1)
                            {
                                if (speciid == 0)
                                {
                                    data = "{\"type\":1,\"msg\":\"未选择具体规格，请选择规格重新提交\"}";
                                    context.Response.Write(data);
                                    return;

                                }
                            }
                            if (speciid != 0)
                            { //如果带规格，读取规格价格

                                B2b_com_pro prospeciid = new B2bComProData().GetProspeciidById(proid.ToString(), speciid);
                                if (prospeciid != null)
                                {

                                    cost = prospeciid.Agentsettle_price;
                                    profit = prospeciid.Advise_price - cost;
                                    payprice = prospeciid.Advise_price.ToString("0.00");

                                    //如果库存
                                    if (prospeciid.Ispanicbuy == 1)
                                    {
                                        if (prospeciid.Limitbuytotalnum < Int32.Parse(u_num))
                                        {
                                            data = "{\"type\":1,\"msg\":\"库存票不足，请电话订购或联系商家\"}";
                                            context.Response.Write(data);
                                            return;
                                        }
                                    }

                                }
                            }






                            if (pro.Ispanicbuy == 1)
                            {//抢购产品判断微信端登陆，或页面登陆
                                //判断用户是否微信端登录
                                //if (context.Request.Cookies["AccountId"] == null)
                                //{
                                //    context.Response.Write("{\"type\":\"1\",\"msg\":\"抢购产品请在微信端订购\"}");
                                //    return;
                                //}
                            }

                            if (pro.Pro_state == 0)
                            {
                                data = "{\"type\":1,\"msg\":\"产品已暂停\"}";
                                context.Response.Write(data);
                                return;
                            }

                            if (pro.Pro_end < DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
                            {
                                data = "{\"type\":1,\"msg\":\"产品已过期\"}";
                                context.Response.Write(data);
                                return;
                            }
                            //库存票检查库存数量，不足则不让提交订单
                            if (pro.Source_type == 2)
                            {
                                int kucunpiaoshuliang = new B2bComProData().ProSEPageCount_UNUse(pro.Id);
                                if (kucunpiaoshuliang < Int32.Parse(u_num))
                                {
                                    data = "{\"type\":1,\"msg\":\"库存票不足，请电话订购或联系商家\"}";
                                    context.Response.Write(data);
                                    return;
                                }
                            }


                        }

                        B2bCrmData b2b_crm = new B2bCrmData();
                        if (openid == "")//如果没微信号使用预付款与积分为0直接
                        {
                            Integral = 0;
                            Imprest = 0;
                        }

                        if (Integral > 0)
                        {
                            //读取用户积分
                            var b2b_model = b2b_crm.b2b_crmH5(openid, comid);
                            if (b2b_model != null)
                            {
                                Integral_user = b2b_model.Integral;
                            }
                            //判断用户实际积分 要大于等于要使用的积分
                            if (Integral_user < Integral)
                            {
                                Integral = Integral_user;
                            }

                            //使用的积分如果大于等于订单金额
                            if (Integral >= int.Parse(u_num) * decimal.Parse(payprice))
                            {
                                Integral = int.Parse(u_num) * decimal.Parse(payprice);
                                Imprest = 0;//当积分足以支付订单，使用预付款直接为0
                            }
                            else
                            {//小于订单金额
                                //使用积分不用做处理，按以上使用积分计算
                            }
                        }

                        if (Imprest > 0)
                        { //使用预付款大于0
                            //读取预付款记录
                            var b2b_model = b2b_crm.b2b_crmH5(openid, comid);
                            if (b2b_model != null)
                            {
                                Imprest_user = b2b_model.Imprest;
                            }

                            //判断用户实际积分 要大于等于要使用的积分
                            if (Imprest_user < Imprest)
                            {
                                Imprest = Imprest_user;
                            }

                            //使用的预付款 大于 订单金额-积分（如果使用了则减未使用则-0）
                            if (Imprest >= (int.Parse(u_num) * decimal.Parse(payprice)) - Integral)
                            {
                                Imprest = (int.Parse(u_num) * decimal.Parse(payprice)) - Integral;//直接按实际订单所需使用预付款
                            }
                            else
                            {
                                //使用预付款不予处理，安指定预付款处理
                            }
                        }


                        B2b_order_hotel b2b_order_hotel = null;
                        if (pro != null)
                        {
                            if (pro.Server_type == 9)
                            {//服务类型为酒店客房，则给订单扩展表-酒店订单表添加数据
                                DateTime start_date = context.Request["start_date"].ConvertTo<DateTime>();
                                DateTime end_date = context.Request["end_date"].ConvertTo<DateTime>();
                                int bookdaynum = context.Request["bookdaynum"].ConvertTo<int>(0);
                                string lastarrivaltime = context.Request["lastarrivaltime"].ConvertTo<string>("");
                                string fangtai = context.Request["fangtai"].ConvertTo<string>("");

                                b2b_order_hotel = new B2b_order_hotel()
                                {
                                    Id = 0,
                                    Orderid = 0,
                                    Start_date = start_date,
                                    End_date = end_date,
                                    Bookdaynum = bookdaynum,
                                    Lastarrivaltime = lastarrivaltime,
                                    Fangtai = fangtai
                                };

                                //订购单价（多天的房价）
                                decimal Menprice = new B2b_com_LineGroupDateData().Gethotelallprice(pro.Id, b2b_order_hotel.Start_date, b2b_order_hotel.End_date);//单人次价格
                                //入住天数
                                int danum = (b2b_order_hotel.End_date - b2b_order_hotel.Start_date).Days;//订购天数

                                if (Menprice != 0)
                                {
                                    payprice = (Menprice).ToString("0.00");
                                    profit = 0;//订房无法计算毛利，因为每天日历并没有填写成本
                                }



                            }

                        }


                        //如果附带服务 则把服务
                        if (payprice == null)
                        {
                            payprice = "0";
                        }

                        if (sid != "")
                        {
                            decimal serverprice_temp = 0;
                            var id_arr = sid.Split(',');
                            for (int i = 0; i < id_arr.Count(); i++)
                            {
                                if (id_arr[i].Trim() != "")
                                {
                                    var Rentserver_temp = new RentserverData().Rentserverbyid(int.Parse(id_arr[i].Trim()), comid);
                                    if (Rentserver_temp != null)
                                    {
                                        serverprice_temp += Rentserver_temp.saleprice + Rentserver_temp.serverDepositprice;
                                    }
                                }
                            }

                            //如果选择服务，必须判断是否购买了必须购买的押金服务
                            if (pro != null)
                            {   //通过产品查询所可以购买的服务
                                int rpnum = 0;
                                var Rentserverbyproid_temp = new RentserverData().Rentserverpagelist(1, 100, pro.Com_id, out rpnum, pro.Id);
                                if (Rentserverbyproid_temp != null)
                                {

                                    for (int i = 0; i < Rentserverbyproid_temp.Count; i++)
                                    {
                                        if (Rentserverbyproid_temp[i].mustselect != 0) //查看必须购买的是否购买了
                                        {
                                            int bixugoumai = 0;
                                            for (int j = 0; j < id_arr.Count(); j++)//对提交的购买服务id，循环
                                            {
                                                if (id_arr[j].Trim() != "")
                                                {
                                                    if (int.Parse(id_arr[j].Trim()) == Rentserverbyproid_temp[i].id)
                                                    {//购买提交服务id必须包含
                                                        bixugoumai = 1;
                                                    }
                                                }
                                            }//循环结束，如果购买服务里没有包含则下面就 请选择上必须支付的押金服务
                                            if (bixugoumai == 0)
                                            {
                                                context.Response.Write("{\"type\":\"1\",\"msg\":\"您订购的产品，下面的选择有必须点选的服务，请返回重新选择！\"}");
                                                return;
                                            }
                                        }
                                    }
                                }
                            }



                            payprice = (decimal.Parse(payprice) + serverprice_temp).ToString("0.00");

                            if (payprice == "" || payprice == "0" || payprice == null)
                            {
                                context.Response.Write("{\"type\":\"1\",\"msg\":\"购买金额错误\"}");
                                return;

                            }
                        }
                        else
                        {
                            if (pro == null)
                            {//如果是不含服务，又没有产品 则为错误，要么购买产品，要么购买服务

                                context.Response.Write("{\"type\":\"1\",\"msg\":\"订购产品错误\"}");
                                return;
                            }

                        }






                        B2b_order order = new B2b_order()
                        {
                            ignoredabatime = ignoredabatime,
                            M_b2b_order_hotel = b2b_order_hotel,
                            Id = 0,
                            Pro_id = proid.ConvertTo<int>(0),
                            Speciid = speciid,
                            Order_type = ordertype.ConvertTo<int>(1),
                            U_name = u_name,
                            U_id = uid,
                            U_phone = u_phone,
                            U_idcard=u_idcard,
                            U_num = u_num.ConvertTo<int>(0),
                            U_subdate = DateTime.Now,
                            Payid = 0,
                            Pay_price = payprice.ConvertTo<decimal>(0),
                            Cost = cost,
                            Profit = profit,
                            Order_state = (int)OrderStatus.WaitPay,//
                            Pay_state = (int)PayStatus.NotPay,//支付模块未做出来前先设置为已支付
                            Send_state = (int)SendCodeStatus.NotSend,
                            Ticketcode = 0,//电子码未创建，支付后产生码赋值
                            Rebate = 0,//  利润返佣金额暂时定为0
                            Ordercome = "",//订购来源 暂时定为空
                            U_traveldate = DateTime.Parse(u_traveldate),
                            Dealer = "自动",
                            Comid = comid,
                            Pno = "",
                            Openid = openid,
                            Ticketinfo = "",

                            Integral1 = Integral,//积分
                            Imprest1 = Imprest,//预付款
                            Agentid = 0,     //分销ID
                            Warrantid = 0,   //授权ID
                            Warrant_type = 1,  //支付类型分销 1出票扣款 2验码扣款
                            Buyuid = buyuid,
                            Tocomid = tocomid,
                            pickuppoint = travel_pickuppoints,
                            dropoffpoint = travel_dropoffpoints,
                            Order_remark = order_remark,
                            Deliverytype = deliverytype,
                            Province = province,
                            City = city,
                            Address = address,
                            Code = code,
                            channelcoachid = channelcoachid,
                            baoxiannames = baoxiannames,
                            baoxianpinyinnames = baoxianpinyinnames,
                            baoxianidcards = baoxianidcards,
                            autosuccess = autosuccess,
                            submanagename = submanagename,
                            yuyuepno = yuyuepno,
                            travelnames = travelnames,
                            travelphones = travelphones,
                            serverid = sid,
                            payserverpno = payserverpno,

                            travelidcards = travelidcards,
                            travelnations = travelnations,
                            travelremarks = travelremarks
                        };

                        try
                        {
                            int orderid = 0;
                            data = OrderJsonData.InsertOrUpdate(order, out orderid);

                            #region 已注释
                            ////判断提交订单是否成功，成功录入订单子表(订单乘车人信息表)
                            //if (orderid > 0)
                            //{
                            //    int servertype = new B2bComProData().GetProServer_typeById(proid);
                            //    if (servertype == 10)
                            //    {
                            //        if (travelnames != "")
                            //        {
                            //            //对订单查询如果导入产品产生订单，插入乘车人插入b订单
                            //            var b2borderinfo = new B2bOrderData().GetOrderById(orderid);
                            //            if (b2borderinfo != null)
                            //            {
                            //                if (b2borderinfo.Bindingagentorderid != 0)
                            //                {
                            //                    orderid = b2borderinfo.Bindingagentorderid;
                            //                    var b2borderinfo_B = new B2bOrderData().GetOrderById(orderid);
                            //                    if (b2borderinfo_B != null)
                            //                    {
                            //                        comid = b2borderinfo_B.Comid;
                            //                        proid = b2borderinfo_B.Pro_id.ToString();
                            //                    }
                            //                    b2borderinfo = b2borderinfo_B;
                            //                }

                            //            }
                            //            for (int i = 1; i <= u_num.ConvertTo<int>(0); i++)
                            //            {
                            //                string travelname = travelnames.Split(',')[i - 1];
                            //                string travelidcard = travelidcards.Split(',')[i - 1];
                            //                string travelnation = "";
                            //                if (travelnations != "")
                            //                {
                            //                    travelnation = travelnations.Split(',')[i - 1];
                            //                }
                            //                string travelphone = travelphones.Split(',')[i - 1];
                            //                string travelremark = "";
                            //                if (travelremarks != "")
                            //                {
                            //                    travelremark = travelremarks.Split(',')[i - 1];
                            //                }
                            //                string travel_pickuppoint = travel_pickuppoints;
                            //                string travel_dropoffpoint = travel_dropoffpoints;



                            //                int rt = new B2bOrderData().Insertb2b_order_busNamelist(orderid, travelname, travelidcard, travelnation, u_name, u_phone, DateTime.Now, u_num, u_traveldate, comid, b2borderinfo.Agentid, proid.ConvertTo<int>(0), travel_pickuppoint, travel_dropoffpoint, travelphone, travelremark);

                            //            }
                            //        }
                            //    }
                            //}
                            #endregion
                        }
                        catch
                        {
                            data = "";
                        }
                        context.Response.Write(data);

                    }
                }



                if (oper == "cartcreateorder")
                {

                    var comid = 0;
                    var proid = context.Request["proid"];
                    var speciid = context.Request["speciid"];
                    var ordertype = context.Request["ordertype"];
                    var payprice = context.Request["payprice"];

                    decimal cost = 0;
                    decimal profit = 0;

                    var u_num = context.Request["u_num"];
                    var u_name = context.Request["u_name"];
                    var u_phone = context.Request["u_phone"];
                    var u_traveldate = context.Request["u_traveldate"];

                    var openid = context.Request["openid"].ConvertTo<string>("");

                    var Integral = context.Request["Integral"].ConvertTo<decimal>(0);
                    var Imprest = context.Request["Imprest"].ConvertTo<decimal>(0);

                    //新增乘车人信息
                    var travelnames = context.Request["travelnames"].ConvertTo<string>("");
                    var travelidcards = context.Request["travelidcards"].ConvertTo<string>("");
                    var travelnations = context.Request["travelnations"].ConvertTo<string>("");
                    var travelphones = context.Request["travelphones"].ConvertTo<string>("");
                    var travelremarks = context.Request["travelremarks"].ConvertTo<string>("");

                    var travel_pickuppoints = context.Request["travel_pickuppoints"].ConvertTo<string>("");
                    var travel_dropoffpoints = context.Request["travel_dropoffpoints"].ConvertTo<string>("");

                    var order_remark = context.Request["order_remark"].ConvertTo<string>("");

                    decimal Integral_user = 0;
                    decimal Imprest_user = 0;
                    string data = "";

                    var tocomid = context.Request["tocomid"].ConvertTo<int>(0);
                    var buyuid = context.Request["buyuid"].ConvertTo<int>(0);

                    int uid = context.Request["uid"].ConvertTo<int>(0);

                    var deliverytype = context.Request["deliverytype"].ConvertTo<int>(0);
                    string province = context.Request["province"].ConvertTo<string>("");//省市
                    string city = context.Request["city"].ConvertTo<string>("");//城市
                    string address = context.Request["address"].ConvertTo<string>("");//配送地址
                    string code = context.Request["code"].ConvertTo<string>("");//邮编
                    decimal express = context.Request["express"].ConvertTo<decimal>(0);//快递费
                    string userid = context.Request["userid"].ConvertTo<string>("");
                    string[] useIntegral_arr;
                    string[] useImprest_arr;

                    lock (lockobj)
                    {
                        proid = proid.Replace("_", ",");

                        var proid_d = proid.Split(',')[0];//获取第一个产品


                        B2b_com_pro pro = new B2bComProData().GetProById(proid_d);
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

                        }

                        B2bCrmData b2b_crm = new B2bCrmData();
                        if (openid == "")//如果没微信号使用预付款与积分为0直接
                        {
                            Integral = 0;
                            Imprest = 0;
                        }

                        if (Integral > 0)
                        {
                            //读取用户积分
                            var b2b_model = b2b_crm.b2b_crmH5(openid, comid);
                            if (b2b_model != null)
                            {
                                Integral_user = b2b_model.Integral;
                            }
                            //判断用户实际积分 要大于等于要使用的积分
                            if (Integral_user < Integral)
                            {
                                Integral = Integral_user;
                            }

                            //使用的积分如果大于等于订单金额
                            if (Integral >= int.Parse(u_num) * decimal.Parse(payprice))
                            {
                                Integral = int.Parse(u_num) * decimal.Parse(payprice);
                                Imprest = 0;//当积分足以支付订单，使用预付款直接为0
                            }
                            else
                            {//小于订单金额
                                //使用积分不用做处理，按以上使用积分计算
                            }
                        }

                        if (Imprest > 0)
                        { //使用预付款大于0
                            //读取预付款记录
                            var b2b_model = b2b_crm.b2b_crmH5(openid, comid);
                            if (b2b_model != null)
                            {
                                Imprest_user = b2b_model.Imprest;
                            }

                            //判断用户实际积分 要大于等于要使用的积分
                            if (Imprest_user < Imprest)
                            {
                                Imprest = Imprest_user;
                            }

                            //使用的预付款 大于 订单金额-积分（如果使用了则减未使用则-0）
                            if (Imprest >= (int.Parse(u_num) * decimal.Parse(payprice)) - Integral)
                            {
                                Imprest = (int.Parse(u_num) * decimal.Parse(payprice)) - Integral;//直接按实际订单所需使用预付款
                            }
                            else
                            {
                                //使用预付款不予处理，安指定预付款处理
                            }
                        }




                        int shopcartid = 0;//购物车订单号
                        decimal expressfee = 0;//快递费


                        var proid_arr = proid.Split(',');
                        var speciid_arr = speciid.Split(',');
                        var u_num_arr = u_num.Split(',');


                        //计算总快递费用
                        string feedetail = "";
                        expressfee = new B2b_delivery_costData().Getdeliverycost_ShopCart(proid, city, u_num, out feedetail);
                        if (expressfee == -1)
                        {
                            data = "{\"type\":1,\"msg\":\"快递费计算错误\"}";
                            context.Response.Write(data);
                            return;
                        }

                        //根据数量或重量平均分配快递费
                        var expressfee_arr = feedetail.Split(',');
                        if (proid_arr.Count() != expressfee_arr.Count())
                        {
                            data = "{\"type\":1,\"msg\":\"产品与快递费不匹配,请重新提交\"}";
                            context.Response.Write(data);
                            return;
                        }


                        if (proid_arr.Count() != u_num_arr.Count())
                        {
                            data = "{\"type\":1,\"msg\":\"产品与订购数量不匹配,请重新提交\"}";
                            context.Response.Write(data);
                            return;
                        }
                        for (int i = 0; i < u_num_arr.Count(); i++)
                        {
                            if (int.Parse(u_num_arr[i]) <= 0)
                            {
                                data = "{\"type\":1,\"msg\":\"订购数量出错\"}";
                                context.Response.Write(data);
                                return;
                            }
                        }


                        #region 列出产品单价 根据金额，分配各订单使用预付款及积分数
                        var proprice = "";
                        var useIntegral = "";
                        var useImprest = "";



                        var Integral_temp = Integral;
                        var Imprest_temp = Imprest;

                        for (int i = 0; i < proid_arr.Count(); i++)
                        {
                            B2bComProData prodata = new B2bComProData();
                            var proinfo = prodata.GetProById(proid_arr[i]);
                            if (proinfo != null)
                            {

                                if (int.Parse(speciid_arr[i]) != 0)
                                { //如果带规格，读取规格价格

                                    B2b_com_pro prospeciid = new B2bComProData().GetProspeciidById(proid_arr[i].ToString(), int.Parse(speciid_arr[i]));
                                    if (prospeciid != null)
                                    {

                                        cost = prospeciid.Agentsettle_price;
                                        profit = prospeciid.Advise_price - cost;
                                        //payprice = prospeciid.Advise_price.ToString("0.00");
                                        proprice += prospeciid.Advise_price + ",";
                                    }
                                }
                                else
                                {

                                    proprice += proinfo.Advise_price + ",";
                                }
                            }





                            if (Integral_temp > 0)//当使用积分大于0
                            {
                                //但产品金额的占此次购物比例计算
                                var usein = decimal.Round(((proinfo.Advise_price * decimal.Parse(u_num_arr[i])) / decimal.Parse(payprice) * Integral_temp), 0);
                                if (Integral_temp > usein)//当剩余积分大于此笔订单积分时，直接扣减剩余积分，否则 使用积分等于剩余积分
                                {
                                    Integral_temp = Integral_temp - usein;
                                }
                                else
                                {//否则 使用积分等于剩余积分,剩余积分清0
                                    usein = Integral_temp;
                                    Integral_temp = 0;
                                }

                                //防止因四舍五入时，剩余了积分 当最后一笔订单还有剩余积分，则 把剩余积分加到使用积分里并清0，理论山积分都为整数
                                if (i == proid_arr.Count() - 1)
                                {
                                    if (Integral_temp > 0)
                                    {
                                        usein = usein + Integral_temp;
                                        Integral_temp = 0;
                                    }
                                }
                                useIntegral += usein + ",";
                            }

                            if (Imprest_temp > 0)
                            {
                                var usein = decimal.Round(((proinfo.Advise_price * decimal.Parse(u_num_arr[i])) / decimal.Parse(payprice) * Imprest_temp), 2);
                                if (Imprest_temp > usein)
                                {
                                    Imprest_temp = Imprest_temp - usein;
                                }
                                else
                                {
                                    usein = Imprest_temp;
                                    Imprest_temp = 0;
                                }

                                if (i == proid_arr.Count() - 1)
                                {
                                    if (Imprest_temp > 0)
                                    {
                                        usein = usein + Imprest_temp;
                                        Imprest_temp = 0;
                                    }
                                }
                                useImprest += usein + ",";
                            }

                        }
                        proprice = proprice.Substring(0, proprice.Length - 1);


                        useIntegral_arr = useIntegral.Split(',');
                        if (Integral > 0)
                        {
                            if (proid_arr.Count() != useIntegral_arr.Count())
                            {
                                data = "{\"type\":1,\"msg\":\"产品与使用积分异常,请重新提交\"}";
                                context.Response.Write(data);
                                return;
                            }
                        }

                        useImprest_arr = useImprest.Split(',');
                        if (Imprest > 0)
                        {
                            if (proid_arr.Count() != useImprest_arr.Count())
                            {
                                data = "{\"type\":1,\"msg\":\"产品与使用预付款异常,请重新提交\"}";
                                context.Response.Write(data);
                                return;
                            }
                        }

                        var proprice_arr = proprice.Split(',');
                        if (proid_arr.Count() != proprice_arr.Count())
                        {
                            data = "{\"type\":1,\"msg\":\"产品与产品价格异常,请重新提交\"}";
                            context.Response.Write(data);
                            return;
                        }
                        #endregion





                        B2bCrmData crmdata = new B2bCrmData();
                        #region 创建或读取用户id
                        if (openid == "")//判断是否有微信号，没有微信号检查手机
                        {//查看手机是否有账户
                            B2b_crm crmm = crmdata.GetSjKeHu(u_phone, comid);
                            if (crmm != null)
                            {
                                //通取用户ID
                                uid = crmm.Id;
                            }
                            else
                            {//没有的话创建新账户
                                B2b_crm crm = new B2b_crm()
                                {
                                    Id = 0,
                                    Com_id = comid,
                                    Name = u_name,
                                    Sex = "0",
                                    Phone = u_phone,
                                    Email = "",
                                    Weixin = "",
                                    Laiyuan = "",
                                    Regidate = DateTime.Now,
                                    Age = 0
                                };


                                string cardcode = MemberCardData.CreateECard(1, comid);//创建卡号并插入活动,1:网站；2:微信

                                crm.Idcard = cardcode.ConvertTo<decimal>(0);
                                uid = crmdata.InsertOrUpdate(crm);
                            }
                        }
                        else
                        {
                            //通过微信获取用户ID
                            B2b_crm weixinuserinfo = null;
                            var weixincrm = crmdata.GetB2bCrm(openid, comid, out weixinuserinfo);
                            if (weixincrm == "OK")
                            {
                                if (weixinuserinfo != null)
                                {
                                    uid = weixinuserinfo.Id;
                                }
                            }
                            else
                            {
                                //如果微信号查询不到账户的异常，按手机读取账户或创建账户
                                B2b_crm crmm = crmdata.GetSjKeHu(u_phone, comid);
                                if (crmm != null)
                                {
                                    //通取用户ID
                                    uid = crmm.Id;
                                }
                                else
                                {
                                    B2b_crm crm = new B2b_crm()
                                    {
                                        Id = 0,
                                        Com_id = comid,
                                        Name = u_name,
                                        Sex = "0",
                                        Phone = u_phone,
                                        Email = "",
                                        Weixin = "",
                                        Laiyuan = "",
                                        Regidate = DateTime.Now,
                                        Age = 0
                                    };

                                    string cardcode = MemberCardData.CreateECard(1, comid);//创建卡号并插入活动,1:网站；2:微信

                                    crm.Idcard = cardcode.ConvertTo<decimal>(0);
                                    int u_id = crmdata.InsertOrUpdate(crm);
                                    uid = u_id;
                                }
                            }

                        }
                        #endregion


                        //生成购物车订单
                        B2b_order cartorderinfo = new B2b_order()
                        {
                            Order_type = ordertype.ConvertTo<int>(1),
                            U_name = u_name,
                            U_phone = u_phone,
                            Pay_price = decimal.Parse(payprice) + expressfee,
                            Comid = comid,
                            Pno = "",
                            Openid = "",
                            U_id = uid,
                            Integral1 = 0,   //积分
                            Imprest1 = 0,    //预付款
                            Agentid = 0,     //分销ID
                            Province = province,
                            City = city,
                            Address = address,
                            Code = code,
                            Express = expressfee
                        };
                        shopcartid = new B2bOrderData().CartInsertOrUpdate(cartorderinfo);



                        try
                        {
                            int orderid = 0;

                            for (int i = 0; i < proid_arr.Count(); i++)
                            {
                                var Integral_t = 0;
                                if (Integral > 0)
                                {
                                    Integral_t = int.Parse(useIntegral_arr[i]);
                                }
                                decimal Imprest_t = 0;
                                if (Imprest > 0)
                                {
                                    Imprest_t = decimal.Parse(useImprest_arr[i]);
                                }




                                B2b_order order = new B2b_order()
                                {
                                    Id = 0,
                                    Pro_id = proid_arr[i].ConvertTo<int>(0),
                                    Speciid = speciid_arr[i].ConvertTo<int>(0),
                                    Order_type = ordertype.ConvertTo<int>(1),
                                    U_name = u_name,
                                    U_id = uid,
                                    U_phone = u_phone,
                                    U_num = u_num_arr[i].ConvertTo<int>(0),
                                    U_subdate = DateTime.Now,
                                    Payid = 0,
                                    Pay_price = proprice_arr[i].ConvertTo<decimal>(0),
                                    Cost = cost,
                                    Profit = profit,
                                    Order_state = (int)OrderStatus.WaitPay,//
                                    Pay_state = (int)PayStatus.NotPay,//
                                    Send_state = (int)SendCodeStatus.NotSend,
                                    Ticketcode = 0,//电子码未创建，支付后产生码赋值
                                    Rebate = 0,//  利润返佣金额暂时定为0
                                    Ordercome = "",//订购来源 暂时定为空
                                    U_traveldate = DateTime.Parse(u_traveldate),
                                    Dealer = "自动",
                                    Comid = comid,
                                    Pno = "",
                                    Openid = openid,
                                    Ticketinfo = "",
                                    Integral1 = Integral_t,//积分
                                    Imprest1 = Imprest_t,//预付款
                                    Agentid = 0,     //分销ID
                                    Warrantid = 0,   //授权ID
                                    Warrant_type = 1,  //支付类型分销 1出票扣款 2验码扣款
                                    Buyuid = buyuid,
                                    Tocomid = tocomid,
                                    pickuppoint = travel_pickuppoints,
                                    dropoffpoint = travel_dropoffpoints,
                                    Order_remark = order_remark,
                                    Deliverytype = deliverytype,
                                    Province = province,
                                    City = city,
                                    Address = address,
                                    Code = code,
                                    Express = decimal.Parse(expressfee_arr[i]),
                                    Shopcartid = shopcartid,
                                };

                                data = OrderJsonData.InsertOrUpdate(order, out orderid);


                                //
                                B2b_order orderdelcart = new B2b_order()
                                {
                                    Pro_id = int.Parse(proid_arr[i]),
                                    Speciid = int.Parse(speciid_arr[i]),
                                    Comid = comid,
                                    Openid = userid,//分销ID
                                };

                                var redata = new B2bOrderData().DelUserCart(orderdelcart, ""); ;
                            }
                        }
                        catch
                        {
                            data = "";
                        }
                        context.Response.Write(data);// 按最后一笔订单 进行跳转，实际在支付时判断如果是 购物车订单则 再汇总数据。

                    }


                }


                if (oper == "ConsumerOrderPageList")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    string openid = context.Request["openid"].ConvertTo<string>("");
                    int AccountId = 0;
                    //读取用户登录信息
                    if (context.Session["AccountId"] != null)
                    {
                        //先判断Session
                        AccountId = int.Parse(context.Request.Cookies["AccountId"].Value);
                    }
                    else
                    {
                        string data1 = "{\"type\":1,\"msg\":\"账户不匹配,请重新登录\"}";
                        context.Response.Write(data1);
                        return;
                    }

                    string data = OrderJsonData.ConsumerOrderPageList(openid, pageindex, pagesize, AccountId);
                    context.Response.Write(data);

                }


                if (oper == "ConsumerOrderbyorderid")
                {
                    var orderid = context.Request["orderid"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    string openid = context.Request["openid"].ConvertTo<string>("");
                    int AccountId = 0;
                    //读取用户登录信息
                    if (context.Session["AccountId"] != null)
                    {
                        //先判断Session
                        AccountId = int.Parse(context.Request.Cookies["AccountId"].Value);
                    }
                    else
                    {
                        string data1 = "{\"type\":1,\"msg\":\"账户不匹配,请重新登录\"}";
                        context.Response.Write(data1);
                        return;
                    }

                    string data = OrderJsonData.ConsumerOrderbyorderid(comid, orderid, AccountId);
                    context.Response.Write(data);

                }

                if (oper == "yudingOrderPageList")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    string openid = context.Request["openid"].ConvertTo<string>("");
                    int AccountId = 0;
                    //读取用户登录信息
                    if (context.Session["AccountId"] != null)
                    {
                        //先判断Session
                        AccountId = int.Parse(context.Request.Cookies["AccountId"].Value);
                    }
                    else
                    {
                        string data1 = "{\"type\":1,\"msg\":\"账户不匹配,请重新登录\"}";
                        context.Response.Write(data1);
                        return;
                    }

                    string data = OrderJsonData.ClientOrderquerenPageList(openid, pageindex, pagesize, AccountId, 12);
                    context.Response.Write(data);

                }

                if (oper == "getexpressfee")
                {
                    var proid = context.Request["proid"].ConvertTo<int>(0);
                    var num = context.Request["num"].ConvertTo<int>(1);
                    string city = context.Request["city"].ConvertTo<string>("");


                    string data = OrderJsonData.GetExpressfee(proid, city, num);
                    context.Response.Write(data);

                }
                if (oper == "confirexpress")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    string expresscom = context.Request["expresscom"].ConvertTo<string>("");
                    string expresscode = context.Request["expresscode"].ConvertTo<string>("");
                    string expresstext = context.Request["expresstext"].ConvertTo<string>("");


                    string data = OrderJsonData.ConfirExpress(comid, id, expresscom, expresscode, expresstext);
                    context.Response.Write(data);

                }

                if (oper == "confircartexpress")
                {
                    var id = context.Request["id"].ConvertTo<string>("");
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    string expresscom = context.Request["expresscom"].ConvertTo<string>("");
                    string expresscode = context.Request["expresscode"].ConvertTo<string>("");
                    string expresstext = context.Request["expresstext"].ConvertTo<string>("");



                    var proid_arr = id.Split(',');
                    string data = "";
                    for (int i = 0; i < proid_arr.Count(); i++)
                    {
                        if (proid_arr[i] != "")
                        {
                            data = OrderJsonData.ConfirExpress(comid, int.Parse(proid_arr[i]), expresscom, expresscode, expresstext);
                        }
                    }
                    context.Response.Write(data);

                }


                if (oper == "GetProductByOrderId")
                {
                    int orderid = context.Request["orderid"].ConvertTo<int>(0);
                    string openid = context.Request["openid"].ConvertTo<string>("");

                    int AccountId = 0;
                    //读取用户登录信息
                    if (context.Session["AccountId"] != null)
                    {
                        //先判断Session
                        AccountId = int.Parse(context.Request.Cookies["AccountId"].Value);
                    }
                    else
                    {
                        string data1 = "{\"type\":1,\"msg\":\"账户不匹配,请重新登录\"}";
                        context.Response.Write(data1);
                        return;
                    }


                    string data = OrderJsonData.GetProductByOrderId(orderid, openid, AccountId);
                    context.Response.Write(data);
                }
                if (oper == "Reservation_edit")
                {
                    int reservationid = context.Request["reservationid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string name = context.Request["name"].ConvertTo<string>("");
                    string phone = context.Request["phone"].ConvertTo<string>();
                    string title = context.Request["title"].ConvertTo<string>();
                    int number = context.Request["number"].ConvertTo<int>(0);
                    int wxmaterialid = context.Request["wxmaterialid"].ConvertTo<int>(0);
                    DateTime checkindate = context.Request["checkindate"].ConvertTo<DateTime>();
                    DateTime checkoutdate = context.Request["checkoutdate"].ConvertTo<DateTime>();
                    int roomtypeid = context.Request["roomtypeid"].ConvertTo<int>(0);
                    decimal totalprice = context.Request["totalprice"].ConvertTo<decimal>();
                    string lastercheckintime = context.Request["lastercheckintime"].ConvertTo<string>();

                    int uid = context.Request["uid"].ConvertTo<int>(0);

                    string data = OrderJsonData.CreateHotelOrder(reservationid, comid, name, phone, title, number, wxmaterialid, checkindate, checkoutdate, roomtypeid, totalprice, lastercheckintime, uid);
                    context.Response.Write(data);
                }


                if (oper == "addresspagelist")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    string key = context.Request["key"].ConvertTo<string>("");
                    var agentid = context.Request["agentid"].ConvertTo<int>(0);

                    string data = OrderJsonData.SaveAddressPageList(agentid, pageindex, pagesize, key);
                    context.Response.Write(data);
                }

                if (oper == "deladdress")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);


                    string data = OrderJsonData.DelSaveAddress(id, agentid);
                    context.Response.Write(data);
                }

                if (oper == "subevaluate")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int star = context.Request["star"].ConvertTo<int>(0);
                    string openid = context.Request["openid"].ConvertTo<string>("");
                    int uid = context.Request["uid"].ConvertTo<int>(0);
                    int oid = context.Request["oid"].ConvertTo<int>(0);
                    int evatype = context.Request["evatype"].ConvertTo<int>(0);
                    string area = context.Request["area"].ConvertTo<string>("");

                    string data = OrderJsonData.Subevaluate(comid, oid, uid, star, evatype, area);
                    context.Response.Write(data);
                }

                if (oper == "evaluatePageList")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    string openid = context.Request["openid"].ConvertTo<string>("");
                    int uid = context.Request["uid"].ConvertTo<int>(0);
                    int oid = context.Request["oid"].ConvertTo<int>(0);
                    int evatype = context.Request["evatype"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int channelid = context.Request["channelid"].ConvertTo<int>(0);

                    string data = OrderJsonData.EvaluatePageList(comid, oid, uid, channelid, evatype, pageindex, pagesize);
                    context.Response.Write(data);

                }

                if (oper == "evaluatePageinfo")
                {

                    int id = context.Request["id"].ConvertTo<int>(0);
                    int evatype = context.Request["evatype"].ConvertTo<int>(0);

                    string data = OrderJsonData.EvaluatePageinfo(id, evatype);
                    context.Response.Write(data);

                }


                if (oper == "GetorderPaystate")
                {

                    int id = context.Request["id"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = OrderJsonData.GetorderPaystate(comid, id);
                    context.Response.Write(data);

                }

                //查询用户订单userid 是用户id
                if (oper == "getjiaolianyuyueorder")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var channelid = context.Request["channelid"].ConvertTo<int>(0);
                    var uid = context.Request["uid"].ConvertTo<int>(0);
                    var startime = context.Request["startime"].ConvertTo<string>("");
                    var endtime = context.Request["endtime"].ConvertTo<string>("");

                    if (startime != "")
                    {
                        if (endtime == "")
                        {
                            endtime = startime;
                        }

                        if (DateTime.Parse(endtime) < DateTime.Parse(startime))
                        {
                            endtime = startime;
                        }

                        //结束日期加1天
                        if (endtime != "")
                        {
                            endtime = DateTime.Parse(endtime).AddDays(1).ToString("yyyy-MM-dd");
                        }
                    }
                    else
                    {
                        endtime = "";
                    }





                    string data = OrderJsonData.ConsumerOrderPageList("", pageindex, pagesize, 0, 0, channelid, startime, endtime);
                    context.Response.Write(data);

                }



                //查询用户订单userid 是用户id
                if (oper == "querenpay")
                {
                    var orderid = context.Request["orderid"].ConvertTo<int>(0);
                    var caozuo = context.Request["caozuo"].ConvertTo<string>("");
                    var mobile = context.Request["mobile"].ConvertTo<string>("");

                    var data = OrderJsonData.UporderPaystate(orderid, caozuo, mobile);//处理订单


                    context.Response.Write(data);

                }
                 //查询产品订单销售统计
                if (oper == "prosaleordercount")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var begindate = context.Request["begindate"].ConvertTo<string>("");
                    var enddate = context.Request["enddate"].ConvertTo<string>("");
                    var projectid = context.Request["projectid"].ConvertTo<int>(0);
                    var productid = context.Request["productid"].ConvertTo<int>(0);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var orderstate = context.Request["orderstate"].ConvertTo<string>("");

                    var data = OrderJsonData.prosaleordercount(comid,begindate,enddate,projectid,productid,key,orderstate);//处理订单


                    context.Response.Write(data);

                }
                


                //bus预约
                if (oper == "yuyueyanzheng")
                {
                    string logindata = "";

                    int comid = 0;
                    var pno = context.Request["pno"].ConvertTo<string>("");
                    var getcode = context.Request["getcode"].ConvertTo<string>("");

                    try
                    {

                        if (context.Session["SomeValidateCode"] == null)
                        {
                            logindata = "{\"type\":1,\"msg\":\"验证码错误\"}";
                            context.Response.Write(logindata);
                            return;
                        }
                        else
                        {
                            string initcode = context.Session["SomeValidateCode"].ToString();
                            if (getcode != initcode)
                            {
                                context.Session["SomeValidateCode"] = null;
                                logindata = "{\"type\":1,\"msg\":\"验证码错误\"}";
                                context.Response.Write(logindata);
                                return;
                            }
                        }


                        var prodata = new B2bEticketData();
                        var eticketinfo = prodata.GetEticketDetail(pno);

                        if (eticketinfo == null)
                        {
                            logindata = "{\"type\":1,\"msg\":\"预约码错误\"}";
                            context.Response.Write(logindata);
                            return;
                        }

                        comid = eticketinfo.Com_id;

                        var busdata = new Bus_FeeticketData();
                        var buspnolist = busdata.BusSearchpnobyproid(comid, eticketinfo.Pro_id);
                        if (buspnolist == 0)
                        {
                            logindata = "{\"type\":1,\"msg\":\"预约码错误\"}";
                            context.Response.Write(logindata);
                            return;
                        }
                        else
                        {
                            pno = EncryptionHelper.EticketPnoDES(pno, 0);//加密
                            logindata = "{\"type\":100,\"msg\":\"" + pno + "\"}";
                        }

                        context.Response.Write(logindata);

                    }
                    catch (Exception e)
                    {
                        logindata = "{\"type\":1,\"msg\":\"验证错误，请重新操作\"}";
                        context.Response.Write(logindata);
                        return;
                    }

                }


                //g通过买购买服务
                if (oper == "buyserver")
                {
                    string logindata = "";

                    int comid = 0;
                    var pno = context.Request["pno"].ConvertTo<string>("");
                    var getcode = context.Request["getcode"].ConvertTo<string>("");
                    var buytype = context.Request["buytype"].ConvertTo<string>("");
                    var pno1 = context.Request["pno1"].ConvertTo<string>("");



                    try
                    {

                        if (context.Session["SomeValidateCode"] == null)
                        {
                            logindata = "{\"type\":1,\"msg\":\"验证码错误\"}";
                            context.Response.Write(logindata);
                            return;
                        }
                        else
                        {
                            string initcode = context.Session["SomeValidateCode"].ToString();
                            if (getcode != initcode)
                            {
                                context.Session["SomeValidateCode"] = null;
                                logindata = "{\"type\":1,\"msg\":\"验证码错误\"}";
                                context.Response.Write(logindata);
                                return;
                            }
                        }



                        if (buytype == "1") {

                            if (pno1 == "")
                            {
                                logindata = "{\"type\":1,\"msg\":\"请输入卡号\"}";
                                context.Response.Write(logindata);
                                return;
                            }
                            //根据卡号得到芯片号
                            string cardchipid = new RentserverData().GetChipidByPrintNo(pno1);
                            if (cardchipid == "")
                            {
                                logindata = "{\"type\":1,\"msg\":\"查询失败,请通过短信码购买！\"}";
                                context.Response.Write(logindata);
                                return;
                            }


                            var entserver_User = new RentserverData().SearchRentserver_User(0, cardchipid);
                            if (entserver_User == null)
                            {
                                logindata = "{\"type\":1,\"msg\":\"查询失败！\"}";
                                context.Response.Write(logindata);
                                return;
                            }

                            if (entserver_User.endtime <DateTime.Now)
                            {
                                logindata = "{\"type\":1,\"msg\":\"卡已过有效期！\"}";
                                context.Response.Write(logindata);
                                return;
                            }

                            if (entserver_User.serverstate==2)
                            {
                                logindata = "{\"type\":1,\"msg\":\"卡已归还结束服务！\"}";
                                context.Response.Write(logindata);
                                return;
                            }

                            var eticketonfotemp =  new B2bEticketData().GetEticketByID(entserver_User.eticketid.ToString());
                            if (eticketonfotemp == null)
                            {
                                logindata = "{\"type\":1,\"msg\":\"查询失败,电子票匹配失败，请通过短信码购买！\"}";
                                context.Response.Write(logindata);
                                return;
                            }
                            pno = eticketonfotemp.Pno;
                        }


                        var prodata = new B2bEticketData();
                        var eticketinfo = prodata.GetEticketDetail(pno);

                        if (eticketinfo == null)
                        {
                            logindata = "{\"type\":1,\"msg\":\"没有查询到此短信码\"}";
                            context.Response.Write(logindata);
                            return;
                        }

                        comid = eticketinfo.Com_id;

                        pno = EncryptionHelper.EticketPnoDES(pno, 0);//加密
                        logindata = "{\"type\":100,\"msg\":\"" + pno + "\"}";

                        context.Response.Write(logindata);

                    }
                    catch (Exception e)
                    {
                        logindata = "{\"type\":1,\"msg\":\"验证错误，请重新操作\"}";
                        context.Response.Write(logindata);
                        return;
                    }

                }

                 //g通过买购买服务
                if (oper == "wxpay_saoma")
                {
                    var orderid = context.Request["orderid"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    try
                    {

                    B2b_finance_paytype model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(comid);

                    if (model != null)
                    {
                        //商家微信支付的所有参数都存在
                        if (model.Wx_appid != "" && model.Wx_appkey != "" && model.Wx_partnerid != "" && model.Wx_paysignkey != "")
                        { }
                        else
                        {
                            model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(106);
                        }
                    }
                    else
                    {
                        model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(106);
                    }


                    string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + model.Wx_appid + "&redirect_uri=http://shop" + model.Com_id + ".etown.cn/wxpay/haipayback.aspx?product_id=oid" + orderid + "&response_type=code&scope=snsapi_base&state=123#wechat_redirect";

                     var data = "{\"type\":100,\"msg\":\"" + url + "\"}";
                        context.Response.Write(data);
                        return;
                    }
                    catch
                    {
                        var data = "{\"type\":1,\"msg\":\"验证错误，请重新操作\"}";
                        context.Response.Write(data);
                        return;
                    
                    }


                }
            }
        }

        #region 回调淘宝发码接口(调用3次，间隔为10分钟)

        public string TaobaoSendRetApi(string order_id, string token, string num, int orderid)
        {
            string CodemerchantId = "727429491";
            string tb_returl = "http://gw.api.taobao.com/router/rest";
            string session = "61017227c9b25cd5e74e3daf09f1471cfaa3f87cd1d5a16727429491";
            string appkey = "23139679";//开放平台证书权限管理App Key
            string appsecret = "adde2a4100288166bbee8df66c127d42";//开放平台证书权限管理App Secret
            ////调用发码成功回调接口 记录的次数，超过3次 ，不在回调
            //int invokenum = new Taobao_send_noticeretlogData().GetInvokeNum(order_id);
            //if (invokenum > 0)
            //{
            //    ////关闭计时器
            //    //SendRettimer1.Dispose();

            //    if (invokenum >= 3)
            //    {
            //        return;
            //    }
            //}

            //根据订单号判断是否含有 绑定订单号，含有的话直接赋值绑定订单号
            int bindorderid = new B2bOrderData().GetBindOrderIdByOrderid(orderid);
            if (bindorderid > 0)
            {
                orderid = bindorderid;
            }

            //根据订单id得到电子码
            string pno = new B2bOrderData().GetPnoByOrderId(orderid);


            //调用发码成功回调接口记录记入数据库
            Taobao_send_noticeretlog mretlog = new Taobao_send_noticeretlog
            {
                id = 0,
                order_id = order_id,
                codemerchant_id = CodemerchantId,
                token = token,
                verify_codes = pno + ":" + num,
                qr_images = "",
                ret_code = "",
                ret_time = DateTime.Now
            };
            int loggid = new Taobao_send_noticeretlogData().Editsendnoticeretlog(mretlog);
            mretlog.id = loggid;

            //合作方发码，确认发送成功后再调用淘宝API: taobao.vmarket.eticket.send
            string url = tb_returl;

            ITopClient client = new DefaultTopClient(url, appkey, appsecret);
            VmarketEticketSendRequest req = new VmarketEticketSendRequest();
            req.OrderId = long.Parse(order_id);
            req.VerifyCodes = pno + ":" + num;
            req.Token = token;
            req.CodemerchantId = long.Parse(CodemerchantId);
            req.QrImages = "";
            VmarketEticketSendResponse response = client.Execute(req, session);

            mretlog.ret_code = response.Body;
            new Taobao_send_noticeretlogData().Editsendnoticeretlog(mretlog);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(response.Body);
            XmlElement root = doc.DocumentElement;
            if (root.SelectSingleNode("ret_code") != null)
            {
                string ret_code = root.SelectSingleNode("ret_code").InnerText;

                if (ret_code == "1")//回调成功 
                {
                    mretlog.ret_code = ret_code;
                    new Taobao_send_noticeretlogData().Editsendnoticeretlog(mretlog);
                    return ret_code;
                }
                else
                {
                    //???(暂时没有做)如果调用失败，建议尝试重新调用 直到调用成功或者收到响应中的sub_code是isv.eticket-order-status-error:invalid-order-status或者isv.eticket-send-error:code-alreay-send为止
                    ////创建计时器，间隔10 分钟重新回调 
                    //TaskTimerPara taskpara = new TaskTimerPara();
                    //taskpara.para = para;
                    //taskpara.orderid = orderid;

                    //SendRettimer1 = new System.Threading.Timer(Tick, taskpara, 600000, 600000);
                    return ret_code;
                }
            }
            else
            {


                string sub_code = root.SelectSingleNode("sub_code").InnerText;
                if (sub_code == "isv.eticket-order-status-error:invalid-order-status" || sub_code == "isv.eticket-send-error:code-alreay-send")
                {
                    //不用处理了 
                    return sub_code;
                }
                else
                {
                    //???(暂时没有做)如果调用失败，建议尝试重新调用 直到调用成功或者收到响应中的sub_code是isv.eticket-order-status-error:invalid-order-status或者isv.eticket-send-error:code-alreay-send为止
                    ////创建计时器，间隔10 分钟重新回调 
                    //TaskTimerPara taskpara = new TaskTimerPara();
                    //taskpara.para = para;
                    //taskpara.orderid = orderid;

                    //SendRettimer1 = new System.Threading.Timer(Tick, taskpara, 600000, 600000);
                    return sub_code;
                }
            }

            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log20150422.txt", response.Body); 
            //context.Response.Write(response.Body);  
        }
        #endregion
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
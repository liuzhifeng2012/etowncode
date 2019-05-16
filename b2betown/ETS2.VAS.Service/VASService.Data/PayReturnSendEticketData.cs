using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Common.Business;
using ETS2.WeiXin.Service.WinXinService.BLL;
using Com.Tenpay;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS.Framework;
using System.Web.Script.Serialization;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;

namespace ETS2.VAS.Service.VASService.Data
{
    public class PayReturnSendEticketData
    {

        #region 支付异步返回后，完成订单并发送电子码
        public string PayReturnSendEticket(string trade_no, int order_no, decimal total_fee, string trade_status, string wxtransaction_id = "")
        {


            if (trade_no == null || total_fee == null || order_no == null || trade_status == null)
            {

                return trade_no.ToString() + ";" + total_fee.ToString() + ";" + order_no.ToString() + ";" + trade_status.ToString();
            }



            //查询支付，并修改为支付成功
            B2bPayData datapay = new B2bPayData();
            B2b_pay modelb2pay = datapay.GetPayByoId(order_no);
            if (modelb2pay != null)
            {
                #region 支付原状态 为没有支付成功
                if (modelb2pay.Trade_status != "TRADE_SUCCESS")//判断原状态是否为不成功
                {




                    #region 支付金额 和订单金额相符
                    if (modelb2pay.Total_fee == total_fee)//判断支付金额是否相同
                    {

                        #region ------------------------------------------------获取订单产品信息，并修改支付表支付状态------------------------------------------------------------------
                        modelb2pay.Trade_no = trade_no;
                        modelb2pay.Trade_status = "TRADE_SUCCESS";
                        modelb2pay.Wxtransaction_id = wxtransaction_id;
                        int payid = datapay.InsertOrUpdate(modelb2pay);//修改支付状态

                        //根据订单id得到订单信息
                        B2bOrderData dataorder = new B2bOrderData();
                        B2b_order modelb2border = dataorder.GetOrderById(order_no);

                        if (modelb2border == null)
                        {
                            return "没有查询到此笔订单";
                        }

                        string Pro_name = "";//产品名称
                        B2b_com_pro modelcompro = null;//产品详情

                        //订单类型为 普通订单
                        if (modelb2border.Order_type == 1)
                        {
                            //根据产品id得到产品信息
                            B2bComProData datapro = new B2bComProData();
                            modelcompro = datapro.GetProById(modelb2border.Pro_id.ToString());
                            if (modelcompro != null)
                            {
                                Pro_name = modelcompro.Pro_name;
                            }
                        }
                        else if (modelb2border.Order_type == 2)
                        { //订单类型为 充值订单

                            Pro_name = "预付款充值";
                        }
                        else
                        {
                            return "订单类型错误！";
                        }
                        #endregion//------------------------------------------------获取订单产品信息，并修改支付表支付状态结束------------------------------------------------------------------

                        #region ------------------------------------------------财务处理------------------------------------------------------------------
                        //得到商家信息,账户余额
                        B2b_company modelcom = B2bCompanyData.GetCompany(modelb2border.Comid);
                        //获得新总金额
                        decimal Over_money = modelcom.Imprest + total_fee;



                        //得到支付方式，如果是支付到自己的支付宝账户则需要增加退出记录
                        B2bFinanceData datefin = new B2bFinanceData();
                        B2b_finance_paytype modelfin = datefin.FinancePayType(modelb2border.Comid);
                        int Paytype_int = 1;//支付款到 易城=1 支付到自己=2

                        if (modelfin != null)
                        {
                            Paytype_int = modelfin.Paytype;
                        }

                        //财务支付来源
                        var pay_com = modelb2pay.Pay_com;
                        var Money_come = "支付宝";//默认支付来源

                        if (pay_com == "wx")
                        {
                            Money_come = "微信支付";
                        }

                        if (pay_com == "mtenpay")
                        {
                            Money_come = "财付通支付";
                        }
                        #region --------20150520 去哪支付 是支付到了商家自己 by xiaoliu
                        if (pay_com == "qunar")
                        {
                            Money_come = "去哪商家支付";
                            Paytype_int = 2;
                        }
                        #endregion





                        //增加财务记录
                        B2bFinanceData Financed = new B2bFinanceData();
                        B2b_Finance Financeinfo = new B2b_Finance()
                        {

                            Id = 0,
                            Com_id = modelb2border.Comid,
                            Agent_id = 0,           //分销编号（默认为0）
                            Eid = 0,                //易城账户（默认为0，0=没有交易，1=和易城交易）
                            Order_id = order_no,           //订单号（默认为0）
                            Servicesname = Pro_name + "[" + order_no + "]",       //交易名称/内容
                            SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                            Money = total_fee,              //金额
                            Payment = 0,            //收支(0=收款,1=支出)
                            Payment_type = "直销收款",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点）
                            Money_come = Money_come,         //资金来源（网上支付,银行收款等）
                            Over_money = Over_money,       //余额（根据商家，分销，易城，编号显示相应余额）
                            Paytype = Paytype_int

                        };
                        int finaceid = Financed.InsertOrUpdate(Financeinfo);

                        bool issetfinancepaytype = false;//判断是否为商户自己的微信支付
                        //如果是支付到商户自己则做一笔支出平账，其中是 Paytype_int 未使用，无效,增加去哪，如果是去哪支付都做一笔支出
                        if (Paytype_int == 2 || pay_com == "wx" || pay_com == "qunar")
                        {

                            //判断支付订单是否为 支付到商户自己的微信账户
                            if (modelb2pay.comid != 106 && pay_com == "wx")
                            {
                                issetfinancepaytype = true;
                            }

                            //B2b_finance_paytype model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(modelb2border.Comid);
                            //if (model != null)
                            //{
                            //    //商家微信支付的所有参数都存在，则
                            //    if (model.Wx_appid != "" && model.Wx_appkey != "" && model.Wx_partnerid != "" && model.Wx_paysignkey != "")
                            //    {
                            //        issetfinancepaytype = true;
                            //    }
                            //}
                            //如果商家开通了自己的微信支付则支出一笔做平，其中 106商户是易城自己的排除
                            if ((issetfinancepaytype == true && modelb2border.Comid != 106) || pay_com == "qunar")
                            {
                                //如果是支付到商户的支付宝则增加一笔支出操作与上面做平
                                B2b_Finance Financebackinfo = new B2b_Finance()
                                {
                                    Id = 0,
                                    Com_id = modelb2border.Comid,
                                    Agent_id = 0,           //分销编号（默认为0）
                                    Eid = 0,                //易城账户（默认为0，0=没有交易，1=和易城交易）
                                    Order_id = order_no,           //订单号（默认为0）
                                    Servicesname = Pro_name,       //交易名称/内容
                                    SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                                    Money = 0 - total_fee,              //金额
                                    Payment = 1,            //收支(0=收款,1=支出)
                                    Payment_type = "转-" + Money_come,       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点）
                                    Money_come = Money_come,         //资金来源（网上支付,银行收款等）
                                    Over_money = Over_money - total_fee       //余额（根据商家，分销，易城，编号显示相应余额）
                                };
                                int finacebackid = Financed.InsertOrUpdate(Financebackinfo);
                                Over_money = Over_money - total_fee;
                            }
                        }

                        //-----------------------------------------------支付/易城服务费手续费 开始------------
                        decimal shouxufei = 0;//计算费率 ，包括 手续费和服务费两种，其中，
                        decimal zifukoujianfeilv = decimal.Parse("0.006");
                        if (modelb2border.Order_type == 2)
                        {
                            if (issetfinancepaytype == true)//如果商户自己的微信支付
                            {
                                //如果商户未自己的支付平台，扣减指定指定手续费率，如果 最后手续费为小于0则等于0
                                shouxufei = modelcom.Fee - zifukoujianfeilv;
                                if (shouxufei < 0)
                                {
                                    shouxufei = 0;
                                }
                            }
                            else
                            {
                                //如果是充值订单只收取支付手续费
                                shouxufei = modelcom.Fee;
                            }
                        }
                        else
                        {
                            if (issetfinancepaytype == true)//如果商户自己的微信支付，收取服务费，支付手续费扣减自付费率并不能小于0
                            {
                                //如果商户未自己的支付平台，扣减指定指定手续费率，如果 最后手续费为小于0则等于0
                                decimal payshouxufei_temp = modelcom.Fee - zifukoujianfeilv;
                                if (payshouxufei_temp < 0)
                                {
                                    payshouxufei_temp = 0;
                                }
                                shouxufei = modelcom.ServiceFee + payshouxufei_temp;
                            }
                            else if (pay_com == "qunar")
                            {//独立判断，如果去哪订单 只收取服务费，因没有支付
                                shouxufei = modelcom.ServiceFee;
                            }
                            else
                            {
                                shouxufei = modelcom.Fee + modelcom.ServiceFee;
                            }
                        }

                        B2b_Finance Financebackinfo_1 = new B2b_Finance()
                        {
                            Id = 0,
                            Com_id = modelb2border.Comid,
                            Agent_id = 0,           //分销编号（默认为0）
                            Eid = 0,                //易城账户（默认为0，0=没有交易，1=和易城交易）
                            Order_id = order_no,           //订单号（默认为0）
                            Servicesname = Pro_name + "[" + order_no + "]",       //交易名称/内容
                            SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                            Money = decimal.Round(0 - (total_fee * shouxufei), 2),              //金额
                            Payment = 1,            //收支(0=收款,1=支出)
                            Payment_type = "手续费",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点）
                            Money_come = Money_come,         //资金来源（网上支付,银行收款等）
                            Over_money = decimal.Round(Over_money - (total_fee * shouxufei), 2)       //余额（根据商家，分销，易城，编号显示相应余额）
                        };
                        int finacebackid_1 = Financed.InsertOrUpdate(Financebackinfo_1);
                        //-----------------------------------------------支付/易城服务费手续费 结束------------

                        //推荐人返佣
                        try
                        {
                            if (modelcompro != null)
                            {
                                if (modelcompro.isrebate == 1)
                                {

                                    Member_Channel modelchannel = new MemberChannelData().GetChannelDetail(modelb2border.recommendchannelid);
                                    if (modelchannel != null)
                                    {
                                        //在职的 内部门市员工/合作商家员工（除默认渠道人）外可以获得返佣
                                        if (modelchannel.Issuetype == 0 || modelchannel.Issuetype == 1)
                                        {
                                            if (modelchannel.Whetherdefaultchannel == 0 && modelchannel.Runstate == 1)
                                            {
                                                decimal rebatemoney = 0;
                                                decimal ordermoney = modelb2border.Pay_price * modelb2border.U_num;

                                                B2b_order bindagentorder = new B2bOrderData().GetOrderById(modelb2border.Bindingagentorderid);
                                                #region 导入产品：直销售卖价格-分销价格
                                                if (bindagentorder != null)
                                                {
                                                    rebatemoney = ordermoney - bindagentorder.Pay_price * bindagentorder.U_num;

                                                    if (rebatemoney > 0)
                                                    {
                                                        //获得渠道人的返佣余额
                                                        decimal channelrebatemoney = new Member_channel_rebatelogData().Getrebatemoney(modelb2border.recommendchannelid);
                                                        //返佣记录
                                                        Member_channel_rebatelog rebatelog = new Member_channel_rebatelog
                                                        {
                                                            id = 0,
                                                            channelid = modelb2border.recommendchannelid,
                                                            orderid = modelb2border.Id,
                                                            payment = 1,
                                                            payment_type = "返佣进账",
                                                            proid = modelcompro.Id.ToString(),
                                                            proname = modelcompro.Pro_name,
                                                            subdatetime = DateTime.Now,
                                                            ordermoney = ordermoney,
                                                            rebatemoney = rebatemoney,
                                                            over_money = decimal.Round(channelrebatemoney + rebatemoney, 2),
                                                            comid = modelb2border.Comid
                                                        };
                                                        //增加返佣记录 同时增加渠道人的返佣金额
                                                        new Member_channel_rebatelogData().Editrebatelog(rebatelog);
                                                        new Member_channel_rebatelogData().Editchannelrebate(rebatelog.channelid, rebatelog.over_money);
                                                    }
                                                }
                                                #endregion
                                                #region 自己产品(现阶段只是票务产品 和 实物产品):直销售卖价格-三级分销价格
                                                else
                                                {
                                                    if (modelcompro.Server_type == 1 || modelcompro.Server_type == 11)
                                                    {
                                                        //产品是单规格产品
                                                        if (modelcompro.Manyspeci == 0)
                                                        {
                                                            if (modelcompro.Agent3_price > 0)
                                                            {
                                                                rebatemoney = ordermoney - modelcompro.Agent3_price * modelb2border.U_num;
                                                            }
                                                        }
                                                        //产品是多规格产品,需要到产品规格表中查询一级分销价
                                                        if (modelcompro.Manyspeci == 1)
                                                        {
                                                            B2b_com_pro prospeciid = new B2bComProData().GetProspeciidById(modelcompro.Id.ToString(), modelb2border.Speciid);
                                                            if (prospeciid != null)
                                                            {
                                                                if (prospeciid.Agent3_price > 0)
                                                                {
                                                                    rebatemoney = ordermoney - prospeciid.Agent3_price * modelb2border.U_num;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                rebatemoney = 0;
                                                            }
                                                        }

                                                        if (rebatemoney > 0)
                                                        {
                                                            //获得渠道人的返佣余额
                                                            decimal channelrebatemoney = new Member_channel_rebatelogData().Getrebatemoney(modelb2border.recommendchannelid);
                                                            //返佣记录
                                                            Member_channel_rebatelog rebatelog = new Member_channel_rebatelog
                                                            {
                                                                id = 0,
                                                                channelid = modelb2border.recommendchannelid,
                                                                orderid = modelb2border.Id,
                                                                payment = 1,
                                                                payment_type = "返佣进账",
                                                                proid = modelcompro.Id.ToString(),
                                                                proname = modelcompro.Pro_name,
                                                                subdatetime = DateTime.Now,
                                                                ordermoney = ordermoney,
                                                                rebatemoney = rebatemoney,
                                                                over_money = decimal.Round(channelrebatemoney + rebatemoney, 2),
                                                                comid = modelb2border.Comid
                                                            };
                                                            //增加返佣记录 同时增加渠道人的返佣金额
                                                            new Member_channel_rebatelogData().Editrebatelog(rebatelog);
                                                            new Member_channel_rebatelogData().Editchannelrebate(rebatelog.channelid, rebatelog.over_money);
                                                        }
                                                    }
                                                }
                                                #endregion

                                            }
                                        }
                                    }

                                }
                            }
                        }
                        catch { }

                        #endregion  ------------------------------------------------财务处理------------------------------------------------------------------





                        #region ------------------------------------------------订单处理------------------------------------------------------------------
                        string retstr = "";
                        if (modelb2border.Shopcartid == 0)
                        {
                            var order = chulidingdan(order_no, total_fee);

                            retstr = order;
                            return retstr;
                        }
                        else
                        {
                            var ordercartlist = dataorder.shopcartorder(modelb2border.Shopcartid);
                            if (ordercartlist != null)
                            {
                                decimal ordersum = 0;
                                for (int i = 0; i < ordercartlist.Count; i++)
                                {
                                    try
                                    {
                                        //实际支付的金额等于 单价*数量 - 积分- 预付款 +快递费
                                        ordersum = ordercartlist[i].Pay_price * ordercartlist[i].U_num - ordercartlist[i].Integral1 - ordercartlist[i].Imprest1 + ordercartlist[i].Express;
                                        var order = chulidingdan(ordercartlist[i].Id, ordersum);
                                        retstr += "ordercartlist[" + i + "].Id:" + ordercartlist[i].Id + ", ordersum:" + ordersum + "--" + order;
                                    }
                                    catch { }

                                }
                                return retstr;
                            }
                            else
                            {
                                return "获得购物车中订单为null";
                            }
                        }


                        #endregion  --------------------------------------------订单处理结束------------------------------------------------------------------







                    }
                    #endregion
                    #region 支付金额与订单金额不相符
                    else
                    {
                        Wxdelivernotify(modelb2pay, 0, "支付金额不符");
                        return "支付金额不符！";
                    }
                    #endregion
                }
                #endregion
                #region 支付原状态 为支付成功
                else
                {
                    return "发送失败，订单状态显示已完成！";
                }
                #endregion
            }
            else
            {//没有此支付的时候返回0
                return "没有查询到此笔支付记录！";
            }

            return "未知错误，请联系管理员！";

        }
        #endregion


        #region 绑定导入产品分销订单再客服支付成功后进行，分销订单扣款并发送码 orderid 绑定分销的订单号是相关订单号
        public string BindingAgentOrderSend(int orderid)
        {
            string dikou = "";
            B2bOrderData dataorder = new B2bOrderData();
            B2b_order order = dataorder.GetOrderById(orderid);
            B2b_com_pro pro_t = new B2bComProData().GetProById(order.Pro_id.ToString());
            Agent_company agentinfo = AgentCompanyData.GetAgentWarrant(order.Agentid, order.Comid);
            //计算分销余额
            decimal overmoney = agentinfo.Imprest - order.Pay_price * order.U_num;


            if (order == null)
            {
                dikou = "没有查询到此笔订单";
                return dikou;
            }
            if (order.Order_type == 1)//1为订单处理
            {
                //扣减子分销授权金额
                if (order.Agentsunid != 0)
                {
                    decimal jine = order.Pay_price * order.U_num;//扣减金额
                    var agentsunfin = AgentCompanyData.WriteAgentSunMoney(jine, order.Agentsunid);
                }

                if (order.Warrant_type == 1)//1.出票扣款 2.验码扣款
                {
                    //分销商财务扣款
                    Agent_Financial Financialinfo = new Agent_Financial
                    {
                        Id = 0,
                        Com_id = order.Comid,
                        Agentid = order.Agentid,
                        Warrantid = order.Warrantid,
                        Order_id = order.Id,
                        Servicesname = pro_t.Pro_name + "[" + orderid + "]",
                        SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                        Money = 0 - order.Pay_price * order.U_num,
                        Payment = 1,            //收支(0=收款,1=支出)
                        Payment_type = "分销扣款",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点,分销扣款）
                        Money_come = "分销账户",         //资金来源（网上支付,银行收款，分销账户等）
                        Over_money = overmoney
                    };
                    var fin = AgentCompanyData.AgentFinancial(Financialinfo);

                    order.Pay_state = 2;
                    order.Order_state = 2;
                    //修改此订单的支付状态为“支付成功” ,订单状态为“已付款”
                    dataorder.InsertOrUpdate(order);



                    #region 酒店客房产品当客户 支付了，给绑定人发送订单确认通知
                    if (pro_t.Server_type == 9)
                    {

                        if (pro_t.Source_type != 4)
                        {//导入产品不发送订房信息
                            //获取订单房
                            order.M_b2b_order_hotel = new B2b_order_hotelData().GetHotelOrderByOrderId(order.Id);

                            //获取项目名称
                            string projectname = new B2b_com_projectData().GetProjectNameByid(pro_t.Projectid);

                            //短信通知
                            var querenduanxin = pro_t.bookpro_bindname + "你好 客户预订：" + projectname + pro_t.Pro_name + " 姓名:" + order.U_name + " (" + order.U_phone + ") ，" + order.U_num + "间，入住时间:" + order.M_b2b_order_hotel.Start_date.ToString("yyyy-MM-dd") + " 离店时间： " + order.M_b2b_order_hotel.End_date.ToString("yyyy-MM-dd") + " ,订单号:" + order.Id + ",请立即回复短信确认：(1)留房请回复 qr" + order.Id + " ，(2)满房取消请回复 qx" + order.Id + "。";

                            var msg = "";
                            var sendback = SendSmsHelper.SendSms(pro_t.bookpro_bindphone, querenduanxin, order.Comid, out msg);

                            string Returnmd5_temp = EncryptionHelper.ToMD5(order.ToString() + "lixh1210", "UTF-8");
                            var querenduanxin_weixin = "有客户预订房间：\n\n " + projectname + pro_t.Pro_name + " \n\n客户:" + order.U_name + " (" + order.U_phone + ") 入住时间:" + order.U_traveldate.ToString("yyyy-MM-dd hh:mm") + " 离店时间：" + order.M_b2b_order_hotel.End_date.ToString("yyyy-MM-dd") + " \n\n点击下面链接进行确认。\n http://shop" + pro_t.Com_id + ".etown.cn/h5/Confirmyuyue.aspx?id=" + order.Id + "&md5=" + Returnmd5_temp;

                            //对绑定顾问发送微信客服通道通知
                            CustomerMsg_Send.SendWxkefumsg(order.Id, 1, querenduanxin_weixin, order.Comid);//给绑定顾问发送
                        }

                       
                    }
                    #endregion
                    else
                    {//非酒店发送带脑子码，酒店先不发送电子吗，发送有问题

                        dikou = new SendEticketData().SendEticket(orderid, 1);
                    }
                    return dikou;
                }
                else
                {

                    order.Pay_state = 2; //对于验码时扣款，此笔订单应该如何支付状态应该如何处理。
                    order.Order_state = 2;
                    //修改此订单的支付状态为“支付成功” ,订单状态为“已付款”
                    dataorder.InsertOrUpdate(order);
                    dikou = new SendEticketData().SendEticket(orderid, 1);
                    return dikou;

                    //return "订单已成功提交,此订单验码扣款";
                }
            }
            else
            {//成功提交充值订单
                return "订单提交成功";
            }
        }
        #endregion




        #region 对已支付成功的未出票的，直接成功
        public string chulidingdan(int order_no, decimal total_fee)
        {
            B2bOrderData dataorder = new B2bOrderData();
            B2b_order modelb2border = dataorder.GetOrderById(order_no);
            B2bPayData datapay = new B2bPayData();
            B2b_pay modelb2pay = datapay.GetPayByoId(order_no);
            B2b_com_pro modelcompro = null;//产品详情
            string Pro_name = "";
            //订单类型为 普通订单
            if (modelb2border.Order_type == 1)
            {
                //根据产品id得到产品信息
                B2bComProData datapro = new B2bComProData();
                modelcompro = datapro.GetProById(modelb2border.Pro_id.ToString());
                if (modelcompro != null)
                {
                    Pro_name = modelcompro.Pro_name;


                    if(modelb2border.Order_state ==23)//如果超时订单
                    {
                        //查询产品是否为抢购，
                        if (modelcompro.Ispanicbuy == 0) {
                            modelb2border.Order_state = 1;
                        }
                    }

                }
            }




            #region ------------------------------------------------订单处理------------------------------------------------------------------
            #region 订单状态正常则执行
            if (modelb2border.Order_state == 1)
            {//如果订单状态正常则执行

                #region 支付金额<订单金额时，需要判断用户应用 积分、预付款情况
                //如果使用积分或预付款则 进行扣款
                if ((modelb2border.Integral1 + modelb2border.Imprest1) > 0)
                {
                    #region 读取账户信息
                    //如果订单里没有openID则直接跳出
                    if (modelb2border.Openid == "" && modelb2border.U_id == 0)
                    {
                        return "此订单支付金额不等于订单金额，同时未查到此订单的账户信息";
                    }

                    //读取用户信息,优先根据openid，如果没有OPENid则按用户UID读取
                    var crmdata = new B2bCrmData();
                    var b2b_crm = crmdata.b2b_crmH5(modelb2border.Openid, modelb2border.Comid);
                    if (b2b_crm == null)
                    {
                        b2b_crm = crmdata.Readuser(modelb2border.U_id, modelb2border.Comid);
                        if (b2b_crm == null)
                        {
                            return "未查到此订单的账户信息";
                        }
                    }
                    #endregion

                    #region 用户使用积分 预付款
                    decimal Imprest = b2b_crm.Imprest;//用户预付款金额
                    decimal Integral = b2b_crm.Integral;//用户积分金额

                    decimal chae = (modelb2border.Pay_price * modelb2border.U_num + modelb2border.Express) - total_fee;//支付金额与订单差额
                    decimal Integral1 = modelb2border.Integral1;//此订单使用 积分或积分
                    decimal Imprest1 = modelb2border.Imprest1;//此订单使用 预付款

                    if (chae == Integral1 + Imprest1)//预付款+积分 = 订单金额-支付金额
                    {
                        if (Integral1 > 0)//当使用积分
                        {
                            if (Integral1 > Integral)
                            {//使用积分要小于或等于用户积分
                                return "用户使用积分金额不符";
                            }
                        }

                        if (Imprest1 > 0)//当使用预付款
                        {
                            if (Imprest1 > Imprest)
                            {//使用积分要小于或等于用户预付款
                                return "用户使用预付款金额不符";
                            }
                        }


                        if (Integral1 > 0)
                        {   //抵扣积分
                            MemberIntegralData intdate = new MemberIntegralData();
                            Member_Integral Intinfo = new Member_Integral()
                            {
                                Id = b2b_crm.Id,
                                Comid = modelb2border.Comid,
                                Acttype = "reduce_integral",           //操作类型
                                Money = 0 - Integral1,              //交易金额
                                Admin = "订单使用",
                                Ip = "",
                                Ptype = 2,
                                Oid = 0,
                                Remark = "",
                                OrderId = order_no,
                                OrderName = "订单使用"
                            };
                            var prointegral = intdate.InsertOrUpdate(Intinfo);

                            if (b2b_crm.Weixin != "")
                            {
                                new Weixin_tmplmsgManage().WxTmplMsg_CrmConsume(b2b_crm.Com_id, b2b_crm.Weixin, "名称", "使用会员积分", "会员卡号", b2b_crm.Idcard.ToString(), DateTime.Now.ToString(), "订单:" + order_no + " 使用积分" + Integral1 + ",抵扣" + Integral1 + "元,如有疑问，请致电客服。");

                            }

                        }
                        if (Imprest1 > 0)
                        {//扣除预付款
                            MemberImprestData impdate = new MemberImprestData();
                            Member_Imprest Impinfo = new Member_Imprest()
                            {
                                Id = b2b_crm.Id,
                                Comid = modelb2border.Comid,
                                Acttype = "reduce_imprest",        //操作类型
                                Money = 0 - Imprest1,              //交易金额
                                Admin = "订单使用",
                                Ip = "",
                                Ptype = 2,
                                Oid = 0,
                                Remark = "",
                                OrderId = order_no,
                                OrderName = "订单使用"
                            };
                            var proImprest = impdate.InsertOrUpdate(Impinfo);
                            if (b2b_crm.Weixin != "")
                            {
                                new Weixin_tmplmsgManage().WxTmplMsg_CrmConsume(b2b_crm.Com_id, b2b_crm.Weixin, "名称", "使用预付款", "会员卡号", b2b_crm.Idcard.ToString(), DateTime.Now.ToString(), "订单:" + order_no + " 使用" + Integral1 + "元预付款,如有疑问，请致电客服。");
                            }
                        }
                    }
                    #endregion

                    #region 用户预付款+积分 ！= 订单金额-支付金额，返回错误信息
                    else
                    {//预付款+积分 ！= 订单金额-支付金额

                        return "支付金额+预付款等与订单金额及金额不符";
                    }
                    #endregion

                }
                #endregion


                modelb2border.Pay_state = (int)PayStatus.HasPay;
                modelb2border.Order_state = (int)OrderStatus.HasPay;
                modelb2border.Backtickettime = DateTime.Now;
                //修改此订单的支付状态为“支付成功” ,订单状态为“已付款”
                dataorder.InsertOrUpdate(modelb2border);


                //当支付成功了，则直接对导入产品的分销订单进行扣款和发码
                if (modelcompro != null)
                {
                    if (modelcompro.Source_type == 4)
                    {

                        var agentod = BindingAgentOrderSend(modelb2border.Bindingagentorderid);
                    }
                }



                //读取用户信息,优先根据openid，如果没有OPENid则按用户UID读取 一下代码需要优化，查询两次
                var crmdataa = new B2bCrmData();
                var b2b_crm_temp = crmdataa.b2b_crmH5(modelb2border.Openid, modelb2border.Comid);
                if (b2b_crm_temp == null)
                {
                    b2b_crm_temp = crmdataa.Readuser(modelb2border.U_id, modelb2border.Comid);
                }

                if (b2b_crm_temp != null)//有用户信息才执行继续送积分
                {
                    if (modelcompro != null)
                    {
                        if (modelcompro.Pro_Integral > 0)
                        {    //产品加积分+
                            MemberIntegralData intdate = new MemberIntegralData();
                            Member_Integral Intinfo = new Member_Integral()
                            {
                                Id = b2b_crm_temp.Id,
                                Comid = modelb2border.Comid,
                                Acttype = "add_integral",           //操作类型
                                Money = modelcompro.Pro_Integral,              //交易金额
                                Admin = "订单赠送积分",
                                Ip = "",
                                Ptype = 1,
                                Oid = 0,
                                Remark = "",
                                OrderId = order_no,
                                OrderName = "订单赠送积分"
                            };
                            var prointegral = intdate.InsertOrUpdate(Intinfo);
                            //订单赠送等积分
                            B2bcrm_dengjifenlog djflog = new B2bcrm_dengjifenlog
                            {
                                id = 0,
                                crmid = b2b_crm_temp.Id,
                                dengjifen = modelcompro.Pro_Integral,
                                ptype = 1,
                                opertor = "订单赠送等积分",
                                opertime = DateTime.Now,
                                orderid = order_no,
                                ordername = "订单赠送等积分",
                                remark = "订单赠送等积分"
                            };
                            new B2bCrmData().Adjust_dengjifen(djflog, b2b_crm_temp.Id, modelb2border.Comid, modelcompro.Pro_Integral);


                            //微信消息模板
                            if (b2b_crm_temp.Weixin != "")
                            {
                                new Weixin_tmplmsgManage().WxTmplMsg_CrmIntegralReward(b2b_crm_temp.Com_id, b2b_crm_temp.Weixin, "您好，积分已经打入您的账户", b2b_crm_temp.Idcard.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "活动赠送", Intinfo.Money.ToString(), b2b_crm_temp.Integral.ToString(), "如有疑问，请致电客服。");
                            }

                        }
                    }
                }

                //微信模板消息-订单状态变更,此处有问题,需要参数 '@msgid'
                //new Weixin_tmplmsgManage().WxTmplMsg_OrderStatusChange(modelb2border.Id);





            }
            #endregion
            #region 订单状态非正常，只修改支付状态
            else
            {//订单状态非正常，只修改支付状态
                modelb2border.Pay_state = (int)PayStatus.HasPay;
                modelb2border.Backtickettime = DateTime.Now;
                //异常订单，只标注 支付成功，订单状态不变
                dataorder.InsertOrUpdate(modelb2border);

                if (modelb2border.Order_state == 23)
                {
                    //如果订单还是超时订单，给服务商发信息，操作退款提示
                    string msg = "";
                    string Phone = "13488761102";
                    var company = B2bCompanyData.GetCompany(modelb2border.Comid);
                    if (company != null) {

                        Phone = company.B2bcompanyinfo.Phone;
                    }

                    string duanxinmsg = "客户订单（" + modelb2border.Id + "）超时支付了,订单处理失败，请后台进行对订单进行退票处理，如需要其他操作请联系管理员！";
                    var sendback = SendSmsHelper.SendSms(Phone, duanxinmsg, modelb2border.Comid, out msg);
                }


            }
            #endregion
            #endregion

            #region------------------------------------------对其他商户转来的订单，进行返点----------------------------------
            if (modelb2border.Tocomid != 0 && modelb2border.Tocomid != 106)
            { //对来路商户判断，不能为0，也不能为自己106商户。
                if (modelcompro != null)
                {
                    if (modelcompro.Agent3_price != 0)//读取给商户同行价
                    { //返点价格不能为0，为0则不返 
                        //计算 要返点金额,用总支付金额（销售价*数量）-（同行价*数量）
                        decimal fandian_money = total_fee - (modelcompro.Agent3_price * modelb2border.U_num);
                        string toMoney_come = "会员购门票返点";
                        if (fandian_money >= 0)
                        { //返点金额必须大于或等于0，小于暂时不操作，当使用积分，时规则再完善

                            //读取来路商户信息
                            B2b_company modelTocomdata = B2bCompanyData.GetCompany(modelb2border.Tocomid);
                            decimal toOver_money = modelTocomdata.Imprest + fandian_money;

                            //根据用户读取渠道商,以后可以不用
                            B2bCrmData tocrmdata = new B2bCrmData();
                            var tocomdata = tocrmdata.GetMemberChannelcompanyByB2bCrmId(modelb2border.Buyuid);
                            if (tocomdata != "")
                            {
                                toMoney_come = tocomdata + "-会员购门票返点";
                            }

                            //获取渠道/门市ID
                            int Channelid = 0;
                            var tocomdataid = tocrmdata.GetMemberChannelcompanyIdByB2bCrmId(modelb2border.Buyuid);
                            if (tocomdataid != 0)
                            {
                                Channelid = tocomdataid;
                            }


                            //订单来路商户返点
                            B2bFinanceData Financed1 = new B2bFinanceData();
                            B2b_Finance Financeinfo1 = new B2b_Finance()
                            {

                                Id = 0,
                                Com_id = modelb2border.Tocomid,
                                Agent_id = 0,           //分销编号（默认为0）
                                Eid = 0,                //易城账户（默认为0，0=没有交易，1=和易城交易）
                                Order_id = order_no,           //订单号（默认为0）
                                Servicesname = Pro_name + "[" + order_no + "]",       //交易名称/内容
                                SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                                Money = fandian_money,              //金额
                                Payment = 0,            //收支(0=收款,1=支出)
                                Payment_type = "商家返点",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点）
                                Money_come = toMoney_come,         //资金来源（网上支付,银行收款等）
                                Over_money = toOver_money,       //余额（根据商家，分销，易城，编号显示相应余额）
                                Paytype = 1,
                                Channelid = Channelid,
                                Paychannelstate = 0     //
                            };
                            int finaceid1 = Financed1.InsertOrUpdate(Financeinfo1);


                            //重新得到商家信息,账户余额
                            B2b_company modelcom2 = B2bCompanyData.GetCompany(modelb2border.Comid);
                            //获得新总金额
                            decimal Over_money2 = modelcom2.Imprest - fandian_money;

                            //106商户支付返点利润
                            B2bFinanceData Financed2 = new B2bFinanceData();
                            B2b_Finance Financeinfo2 = new B2b_Finance()
                            {

                                Id = 0,
                                Com_id = modelb2border.Comid,
                                Agent_id = 0,           //分销编号（默认为0）
                                Eid = 0,                //易城账户（默认为0，0=没有交易，1=和易城交易）
                                Order_id = order_no,           //订单号（默认为0）
                                Servicesname = Pro_name + "[" + order_no + "]",       //交易名称/内容
                                SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                                Money = 0 - fandian_money,              //金额
                                Payment = 1,  //收支(0=收款,1=支出)
                                Payment_type = "商家返点",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点）
                                Money_come = "会员购门票返点",         //资金来源（网上支付,银行收款等）
                                Over_money = Over_money2,       //余额（根据商家，分销，易城，编号显示相应余额）
                                Paytype = 1
                            };
                            int finaceid2 = Financed2.InsertOrUpdate(Financeinfo2);


                        }
                    }
                }

            }
            #endregion


            //增加客服通知
            try
            {
                //对顾问发送
                SendEticketData.SendWeixinKfMsg(order_no);
            }
            catch { }


            #region -------------------------------------Order_type == 1普通订单处理-------------------------------------------------------------
            if (modelb2border.Order_type == 1)//只有普通订单才有电子码发送流程
            {

                //产品支付类型为微信支付，则向微信平台发送发货通知-暂时先不判断产品类型
                //if (modelb2pay.Pay_com == "wx")
                //{
                //    Wxdelivernotify(modelb2pay, 1, "ok");
                //}


                //发送电子码：服务类型为1(电子票),8(当地游) 发送电子码；2(跟团游),9(酒店) 不发送电子码,10大巴发送电子票,实物订单也产生电子码（判断如果自提的发送，快递的不发送电子码）
                if (modelcompro.Server_type == 1 || modelcompro.Server_type == 8 || modelcompro.Server_type == 10 || modelcompro.Server_type == 11 || modelcompro.Server_type == 12 || modelcompro.Server_type == 13 || modelcompro.Server_type == 14)
                {
                    var sendeticketdate = new SendEticketData();
                    var vasmodel = sendeticketdate.SendEticket(order_no, 1);// 1=发电子码（会生成电子码）,2=重发电子码

                }

                #region 酒店客房产品当客户 支付了，给绑定人发送订单确认通知
                if (modelcompro.Server_type==9)
                {
                    if (modelcompro.Source_type != 4)
                    {//导入产品不发送订房信息
                        //获取订单房
                        modelb2border.M_b2b_order_hotel = new B2b_order_hotelData().GetHotelOrderByOrderId(modelb2border.Id);

                        //获取项目名称
                        string projectname = new B2b_com_projectData().GetProjectNameByid(modelcompro.Projectid);

                        //短信通知
                        var querenduanxin = modelcompro.bookpro_bindname + "你好 客户预订：" + projectname + modelcompro.Pro_name + " 姓名:" + modelb2border.U_name + " (" + modelb2border.U_phone + ") ，" + modelb2border.U_num + "间，入住时间:" + modelb2border.M_b2b_order_hotel.Start_date.ToString("yyyy-MM-dd") + " 离店时间： " + modelb2border.M_b2b_order_hotel.End_date.ToString("yyyy-MM-dd") + " ,订单号:" + modelb2border.Id + ",请立即回复短信确认：(1)留房请回复 qr" + modelb2border.Id + " ，(2)满房取消请回复 qx" + modelb2border.Id + "。";

                        var msg = "";
                        var sendback = SendSmsHelper.SendSms(modelcompro.bookpro_bindphone, querenduanxin, modelb2border.Comid, out msg);

                        string Returnmd5_temp = EncryptionHelper.ToMD5(modelb2border.ToString() + "lixh1210", "UTF-8");
                        var querenduanxin_weixin = "有客户预订房间：\n\n " + projectname + modelcompro.Pro_name + " \n\n客户:" + modelb2border.U_name + " (" + modelb2border.U_phone + ") 入住时间:" + modelb2border.U_traveldate.ToString("yyyy-MM-dd hh:mm") + " 离店时间：" + modelb2border.M_b2b_order_hotel.End_date.ToString("yyyy-MM-dd") + " \n\n点击下面链接进行确认。\n http://shop" + modelcompro.Com_id + ".etown.cn/h5/Confirmyuyue.aspx?id=" + modelb2border.Id + "&md5=" + Returnmd5_temp;

                        //对绑定顾问发送微信客服通道通知
                        CustomerMsg_Send.SendWxkefumsg(modelb2border.Id, 1, querenduanxin_weixin, modelb2border.Comid);//给绑定顾问发送
                    }

                }
               #endregion


                //订单支付成功，发送微信订单模板消息
                new Weixin_tmplmsgManage().WxTmplMsg_OrderSendCodeSuc(modelb2border.Id);
            }
            #endregion

            #region -------------------------------------Order_type == 2 分销充值订单处理---------------------------------------------------
            else if (modelb2border.Order_type == 2)
            {



                if (modelb2border.Agentid == 0)
                {//为用户充值



                    if (modelb2border.serverid != "" || modelb2border.payorder==1)
                    {//购买服务

                        if (modelb2border.payorder == 1)//直接支付
                        {
                            //直接支付直接成功不做任何处理
                        }
                        else
                        {
                            if (modelb2border.Pno != "")
                            { //针对电子码 进行 购买押金及服务
                                //插入购买的服务
                                var inserver = SendEticketData.InsertEticetServerDepositbyorder(modelb2border, modelb2border.Pno);
                            }
                        }



                    }
                    else
                    {//充值

                        MemberImprestData impdate = new MemberImprestData();
                        Member_Imprest Impinfo = new Member_Imprest()
                        {
                            Id = modelb2border.U_id,
                            Comid = modelb2border.Comid,
                            Acttype = "add_imprest",           //操作类型
                            Money = total_fee,               //交易金额
                            Admin = "在线充值",
                            Ip = "",
                            Ptype = 1,
                            Oid = order_no,
                            Remark = "在线充值预付款",
                            OrderId = 0,
                            OrderName = ""
                        };
                        var pro = impdate.InsertOrUpdate(Impinfo);
                        if (pro != 0)
                        {
                            modelb2border.Send_state = (int)SendCodeStatus.HasSend;
                            modelb2border.Order_state = (int)OrderStatus.HasRecharge;
                            modelb2border.Ticketcode = pro;
                            modelb2border.Backtickettime = DateTime.Now;
                            //充值，订单状态修改
                            dataorder.InsertOrUpdate(modelb2border);




                            //发送消息
                            B2bCrmData prodata = new B2bCrmData();
                            var list = prodata.Readuser(Impinfo.Id, Impinfo.Comid);
                            SendSmsHelper.GetMember_sms(list.Phone, list.Name, list.Idcard.ToString(), list.Password1, Impinfo.Money, "充预付款", modelb2border.Comid);//发送短信


                            //微信消息模板
                            if (list.Weixin != "")
                            {
                                new Weixin_tmplmsgManage().WxTmplMsg_CrmRecharge(list.Com_id, list.Weixin, "您好，已成功进行会员卡充值", "会员卡号", list.Idcard.ToString(), Impinfo.Money.ToString() + "元", "充值成功", "如有疑问，请致电客服。");
                            }
                        }
                    }

                }
                else
                { //分销充值
                    var agentinfo = AgentCompanyData.GetAgentWarrant(modelb2border.Agentid, modelb2border.Comid);
                    if (agentinfo != null)
                    {
                        decimal overmoney = agentinfo.Imprest + total_fee;
                        int Payment = 0;
                        string Servicesname = "分销在线充值";
                        string Payment_type = "分销充值";
                        //分销商财务扣款
                        Agent_Financial Financialinfo = new Agent_Financial
                        {
                            Id = 0,
                            Com_id = modelb2border.Comid,
                            Agentid = modelb2border.Agentid,
                            Warrantid = agentinfo.Warrantid,
                            Order_id = order_no,
                            Servicesname = Servicesname,
                            SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                            Money = total_fee,
                            Payment = Payment,            //收支(0=收款,1=支出)
                            Payment_type = Payment_type,       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点,分销扣款）
                            Money_come = "网上支付",         //资金来源（网上支付,银行收款，分销账户等）
                            Over_money = overmoney
                        };
                        var fin = AgentCompanyData.AgentFinancial(Financialinfo);

                        if (fin != 0)
                        {

                            modelb2border.Send_state = (int)SendCodeStatus.HasSend;
                            modelb2border.Order_state = (int)OrderStatus.HasRecharge;
                            modelb2border.Ticketcode = fin;
                            modelb2border.Backtickettime = DateTime.Now;
                            //充值，订单状态修改
                            dataorder.InsertOrUpdate(modelb2border);
                        }


                    }
                }
            }
            #endregion

            #region ------------------------------------Order_type=其他 暂时无法处理--------------------------------
            else
            {
                return "订单类型错误！";
            }
            #endregion




            return "OK";
        }
        #endregion




        #region 对已支付成功的未出票的，直接成功
        public string ShougongReturnSendEticket(int order_no)
        {

            string trade_no = "";
            decimal total_fee = 0;
            string trade_status = "TRADE_SUCCESS";

            //查询支付，并修改为支付成功
            B2bPayData datapay = new B2bPayData();
            B2b_pay modelb2pay = datapay.GetPayByoId(order_no);
            if (modelb2pay != null)
            {
                if (modelb2pay.Trade_status == "TRADE_SUCCESS")
                {
                    total_fee = modelb2pay.Total_fee;
                    trade_no = modelb2pay.Trade_no;


                    //根据订单id得到订单信息
                    B2bOrderData dataorder = new B2bOrderData();
                    B2b_order modelb2border = dataorder.GetOrderById(order_no);

                    if (modelb2border == null)
                    {
                        return "没有查询到此笔订单";
                    }


                    //订单必须未处理才能自动处理,已成功订单都通过其他发送
                    if (modelb2border.Order_state == 1)
                    {
                        //判断支付金额+预付款+积分 是否等于订单金额
                        if (total_fee < modelb2border.Pay_price * modelb2border.U_num)
                        {

                            //如果订单里没有openID则直接跳出
                            if (modelb2border.Openid == "" && modelb2border.U_id == 0)
                            {
                                return "此订单支付金额不等于订单金额，同时未查到此订单的账户信息";
                            }

                            //读取用户信息,优先根据openid，如果没有OPENid则按用户UID读取
                            var crmdata = new B2bCrmData();
                            var b2b_crm = crmdata.b2b_crmH5(modelb2border.Openid, modelb2border.Comid);
                            if (b2b_crm == null)
                            {
                                b2b_crm = crmdata.Readuser(modelb2border.U_id, modelb2border.Comid);
                                if (b2b_crm == null)
                                {
                                    return "未查到此订单的账户信息";
                                }
                            }

                            decimal Imprest = b2b_crm.Imprest;//用户预付款金额
                            decimal Integral = b2b_crm.Integral;//用户积分金额

                            decimal chae = (modelb2border.Pay_price * modelb2border.U_num) - total_fee;//支付金额与订单差额
                            decimal Integral1 = modelb2border.Integral1;//此订单使用 积分或积分
                            decimal Imprest1 = modelb2border.Imprest1;//此订单使用 预付款

                            if (chae == Integral1 + Imprest1)//预付款+积分 = 订单金额-支付金额
                            {
                                if (Integral1 > 0)//当使用积分
                                {
                                    if (Integral1 > Integral)
                                    {//使用积分要小于或等于用户积分
                                        return "用户使用积分金额不符";
                                    }
                                }

                                if (Imprest1 > 0)//当使用预付款
                                {
                                    if (Imprest1 > Imprest)
                                    {//使用积分要小于或等于用户预付款
                                        return "用户使用预付款金额不符";
                                    }
                                }


                                if (Integral1 > 0)
                                {    //抵扣积分
                                    MemberIntegralData intdate = new MemberIntegralData();
                                    Member_Integral Intinfo = new Member_Integral()
                                    {
                                        Id = b2b_crm.Id,
                                        Comid = modelb2border.Comid,
                                        Acttype = "reduce_integral",           //操作类型
                                        Money = 0 - Integral1,              //交易金额
                                        Admin = "订单使用",
                                        Ip = "",
                                        Ptype = 2,
                                        Oid = 0,
                                        Remark = "",
                                        OrderId = order_no,
                                        OrderName = "订单使用"
                                    };
                                    var prointegral = intdate.InsertOrUpdate(Intinfo);
                                }
                                if (Imprest1 > 0)
                                {//扣除预付款
                                    MemberImprestData impdate = new MemberImprestData();
                                    Member_Imprest Impinfo = new Member_Imprest()
                                    {
                                        Id = b2b_crm.Id,
                                        Comid = modelb2border.Comid,
                                        Acttype = "reduce_imprest",        //操作类型
                                        Money = 0 - Imprest1,              //交易金额
                                        Admin = "订单使用",
                                        Ip = "",
                                        Ptype = 2,
                                        Oid = 0,
                                        Remark = "",
                                        OrderId = order_no,
                                        OrderName = "订单使用"
                                    };
                                    var proImprest = impdate.InsertOrUpdate(Impinfo);
                                }
                            }
                            else
                            {//预付款+积分 ！= 订单金额-支付金额

                                return "支付金额+预付款等与订单金额及金额不符";
                            }


                        }


                        //---------------新增1begin--------------//
                        modelb2border.Pay_state = (int)PayStatus.HasPay;
                        modelb2border.Order_state = (int)OrderStatus.HasPay;
                        modelb2border.Backtickettime = DateTime.Now;
                        //修改此订单的支付状态为“支付成功” ,订单状态为“已付款”
                        dataorder.InsertOrUpdate(modelb2border);


                        //读取用户信息,优先根据openid，如果没有OPENid则按用户UID读取 一下代码需要优化，查询两次
                        var crmdataa = new B2bCrmData();
                        var b2b_crm_temp = crmdataa.b2b_crmH5(modelb2border.Openid, modelb2border.Comid);
                        if (b2b_crm_temp == null)
                        {
                            b2b_crm_temp = crmdataa.Readuser(modelb2border.U_id, modelb2border.Comid);
                        }

                        if (b2b_crm_temp != null)//有用户信息才执行继续送积分
                        {
                            B2b_com_pro b2b_pro = new B2bComProData().GetProById(modelb2border.Pro_id.ToString());
                            if (b2b_pro != null)
                            {
                                if (b2b_pro.Pro_Integral > 0)
                                {    //产品加积分++
                                    MemberIntegralData intdate = new MemberIntegralData();
                                    Member_Integral Intinfo = new Member_Integral()
                                    {
                                        Id = b2b_crm_temp.Id,
                                        Comid = modelb2border.Comid,
                                        Acttype = "add_integral",           //操作类型
                                        Money = b2b_pro.Pro_Integral,              //交易金额
                                        Admin = "订单赠送积分",
                                        Ip = "",
                                        Ptype = 1,
                                        Oid = 0,
                                        Remark = "",
                                        OrderId = order_no,
                                        OrderName = "订单赠送积分"
                                    };
                                    var prointegral = intdate.InsertOrUpdate(Intinfo);
                                    //订单赠送等积分
                                    B2bcrm_dengjifenlog djflog = new B2bcrm_dengjifenlog
                                    {
                                        id = 0,
                                        crmid = b2b_crm_temp.Id,
                                        dengjifen = b2b_pro.Pro_Integral,
                                        ptype = 1,
                                        opertor = "订单赠送等积分",
                                        opertime = DateTime.Now,
                                        orderid = order_no,
                                        ordername = "订单赠送等积分",
                                        remark = "订单赠送等积分"
                                    };
                                    new B2bCrmData().Adjust_dengjifen(djflog, b2b_crm_temp.Id, modelb2border.Comid, b2b_pro.Pro_Integral);
                                }
                            }
                        }



                        //---------------新增1end--------------//
                        string Pro_name = "";//产品名称
                        //订单类型为 普通订单
                        if (modelb2border.Order_type == 1)
                        {
                            //根据产品id得到产品信息
                            B2bComProData datapro = new B2bComProData();
                            B2b_com_pro modelcompro = datapro.GetProById(modelb2border.Pro_id.ToString());
                            if (modelcompro != null)
                            {
                                Pro_name = modelcompro.Pro_name;
                            }

                        }
                        else if (modelb2border.Order_type == 2)
                        { //订单类型为 充值订单

                            Pro_name = "预付款充值";
                        }
                        else
                        {
                            return "订单类型错误！";
                        }

                        //得到商家信息,账户余额
                        B2b_company modelcom = B2bCompanyData.GetCompany(modelb2border.Comid);
                        //获得新总金额
                        decimal Over_money = modelcom.Imprest + total_fee;



                        //得到支付方式，如果是支付到自己的支付宝账户则需要增加退出记录
                        B2bFinanceData datefin = new B2bFinanceData();
                        B2b_finance_paytype modelfin = datefin.FinancePayType(modelb2border.Comid);
                        int Paytype_int = 1;//支付款到 易城=1 支付到自己=2

                        if (modelfin != null)
                        {
                            Paytype_int = modelfin.Paytype;
                        }



                        //财务支付来源
                        var pay_com = modelb2pay.Pay_com;
                        var Money_come = "支付宝";//默认支付来源

                        if (pay_com == "wx")
                        {
                            Money_come = "微信支付";
                        }



                        //增加财务记录
                        B2bFinanceData Financed = new B2bFinanceData();
                        B2b_Finance Financeinfo = new B2b_Finance()
                        {

                            Id = 0,
                            Com_id = modelb2border.Comid,
                            Agent_id = 0,           //分销编号（默认为0）
                            Eid = 0,                //易城账户（默认为0，0=没有交易，1=和易城交易）
                            Order_id = order_no,           //订单号（默认为0）
                            Servicesname = Pro_name + "[" + order_no + "]",       //交易名称/内容
                            SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                            Money = total_fee,              //金额
                            Payment = 0,            //收支(0=收款,1=支出)
                            Payment_type = "直销收款",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点）
                            Money_come = Money_come,         //资金来源（网上支付,银行收款等）
                            Over_money = Over_money,       //余额（根据商家，分销，易城，编号显示相应余额）
                            Paytype = Paytype_int

                        };
                        int finaceid = Financed.InsertOrUpdate(Financeinfo);



                        if (Paytype_int == 2)
                        { //如果是支付到商户的支付宝则增加一笔支出操作与上面做平
                            B2b_Finance Financebackinfo = new B2b_Finance()
                            {
                                Id = 0,
                                Com_id = modelb2border.Comid,
                                Agent_id = 0,           //分销编号（默认为0）
                                Eid = 0,                //易城账户（默认为0，0=没有交易，1=和易城交易）
                                Order_id = order_no,           //订单号（默认为0）
                                Servicesname = Pro_name,       //交易名称/内容
                                SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                                Money = 0 - total_fee,              //金额
                                Payment = 1,            //收支(0=收款,1=支出)
                                Payment_type = "提现-商家支付宝",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点）
                                Money_come = Money_come,         //资金来源（网上支付,银行收款等）
                                Over_money = Over_money - total_fee       //余额（根据商家，分销，易城，编号显示相应余额）
                            };
                            int finacebackid = Financed.InsertOrUpdate(Financebackinfo);
                        }
                        else
                        {//支付到易城收相应的手续费------------

                            B2b_Finance Financebackinfo = new B2b_Finance()
                            {
                                Id = 0,
                                Com_id = modelb2border.Comid,
                                Agent_id = 0,           //分销编号（默认为0）
                                Eid = 0,                //易城账户（默认为0，0=没有交易，1=和易城交易）
                                Order_id = order_no,           //订单号（默认为0）
                                Servicesname = Pro_name + "[" + order_no + "]",       //交易名称/内容
                                SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                                Money = decimal.Round(0 - (total_fee * modelcom.Fee), 2),              //金额
                                Payment = 1,            //收支(0=收款,1=支出)
                                Payment_type = "手续费",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点）
                                Money_come = Money_come,         //资金来源（网上支付,银行收款等）
                                Over_money = decimal.Round(Over_money - (total_fee * modelcom.Fee), 2)       //余额（根据商家，分销，易城，编号显示相应余额）
                            };
                            int finacebackid = Financed.InsertOrUpdate(Financebackinfo);

                        }

                        if (modelb2border.Order_type == 1)//只有普通订单才有电子码发送流程
                        {
                            //发送电子码
                            var sendeticketdate = new SendEticketData();
                            var vasmodel = sendeticketdate.SendEticket(order_no, 1);// 1=发电子码（会生成电子码）,2=重发电子码
                            if (vasmodel != null)
                            {
                                if (vasmodel == "OK")
                                {
                                    return "OK";
                                }
                                else
                                {
                                    return vasmodel;
                                }

                            }
                        }
                        else if (modelb2border.Order_type == 2)
                        { //为用户充值

                            MemberImprestData impdate = new MemberImprestData();
                            Member_Imprest Impinfo = new Member_Imprest()
                            {
                                Id = modelb2border.U_id,
                                Comid = modelb2border.Comid,
                                Acttype = "add_imprest",           //操作类型
                                Money = total_fee,               //交易金额
                                Admin = "在线充值",
                                Ip = "",
                                Ptype = 1,
                                Oid = order_no,
                                Remark = "在线充值预付款",
                                OrderId = 0,
                                OrderName = ""
                            };
                            var pro = impdate.InsertOrUpdate(Impinfo);
                            if (pro != 0)
                            {
                                modelb2border.Send_state = (int)SendCodeStatus.HasSend;
                                modelb2border.Order_state = (int)OrderStatus.HasRecharge;
                                modelb2border.Ticketcode = pro;
                                //充值，订单状态修改
                                dataorder.InsertOrUpdate(modelb2border);



                                B2bCrmData prodata = new B2bCrmData();
                                var list = prodata.Readuser(Impinfo.Id, Impinfo.Comid);
                                SendSmsHelper.GetMember_sms(list.Phone, list.Name, list.Idcard.ToString(), list.Password1, Impinfo.Money, "充预付款", modelb2border.Comid);//发送短信
                            }
                        }
                        else
                        {
                            return "订单类型错误！";
                        }
                    }
                }
            }
            else
            {//没有此支付的时候返回0
                return "没有查询到此笔支付记录！";
            }

            return "未知错误，请联系管理员！";

        }
        #endregion

        /// <summary>
        /// 发送发货通知
        /// </summary>
        /// <param name="modelb2pay"></param>
        /// <param name="deliver_status"></param>
        /// <param name="deliver_msg"></param>
        public void Wxdelivernotify(B2b_pay modelb2pay, int deliver_status, string deliver_msg)
        {
            try
            {
                if (modelb2pay == null)
                {
                    return;
                }
                string transid = modelb2pay.Wxtransaction_id;
                string out_trade_no = modelb2pay.Oid.ToString();
                B2b_order morder = new B2bOrderData().GetOrderById(modelb2pay.Oid);
                if (morder == null)
                {
                    return;
                }
                int comid = morder.Comid;
                int uid = morder.U_id;
                string openid = "";
                if (uid > 0)
                {
                    openid = new B2bCrmData().GetWeiXinByCrmid(uid);
                }
                else
                {
                    return;
                }


                #region 微信支付参数
                string Wx_appid = "";
                string Wx_partnerid = "";
                string Wx_partnerkey = "";
                string Wx_paysignkey = "";

                //根据产品判断商家是否含有自己的微信支付:a.含有的话支付到商家；b.没有的话支付到平台的微信公众号账户中
                B2b_finance_paytype model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(comid);

                if (model != null)
                {
                    //商家支付类型为“2-支付到商户”并且微信支付的所有参数都存在
                    if (model.Wx_appid != "" && model.Wx_partnerid != "" && model.Wx_partnerkey != "" && model.Wx_paysignkey != "")
                    {
                        Wx_appid = model.Wx_appid;
                        Wx_partnerid = model.Wx_partnerid;
                        Wx_partnerkey = model.Wx_partnerkey;
                        Wx_paysignkey = model.Wx_paysignkey;
                    }
                }
                #endregion

                WxPayHelper wxPayHelper = new WxPayHelper();
                ////先设置基本信息
                //wxPayHelper.SetAppId(Wx_appid);//AppID
                //wxPayHelper.SetAppKey(Wx_paysignkey);//PaySignKey
                //wxPayHelper.SetPartnerKey(Wx_partnerkey);//商户秘钥                     
                //wxPayHelper.SetSignType("SHA1");

                DateTime nowtime = DateTime.Now;
                int deliver_timestamp = new WeiXinManage().ConvertDateTimeInt(nowtime);
                Dictionary<string, string> paras = new Dictionary<string, string>();
                paras.Add("appid", Wx_appid);
                //paras.Add("appkey", Wx_paysignkey);
                wxPayHelper.SetAppKey(Wx_paysignkey);//PaySignKey
                paras.Add("openid", openid);
                paras.Add("transid", transid);
                paras.Add("out_trade_no", out_trade_no);
                paras.Add("deliver_timestamp", deliver_timestamp.ToString());
                paras.Add("deliver_status", deliver_status.ToString());
                paras.Add("deliver_msg", deliver_msg);


                string app_signature = wxPayHelper.GetBizSign(paras);
                string requestxml = "{" +
                                         "\"appid\" : \"" + Wx_appid + "\"," +
                                         "\"openid\" : \"" + openid + "\"," +
                                         "\"transid\" : \"" + transid + "\"," +
                                         "\"out_trade_no\" : \"" + out_trade_no + "\"," +
                                         "\"deliver_timestamp\" : \"" + deliver_timestamp + "\"," +
                                         "\"deliver_status\" : \"" + deliver_status + "\"," +
                                         "\"deliver_msg\" : \"" + deliver_msg + "\"," +
                                         "\"app_signature\" : \"" + app_signature + "\"," +
                                         "\"sign_method\" : \"sha1\"" +
                                      "}";


                WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(comid);
                WXAccessToken accesstoken = WeiXinManage.GetAccessToken(basic.Comid, basic.AppId, basic.AppSecret);
                if (accesstoken == null)
                {
                    return;
                }
                string requesturl = "https://api.weixin.qq.com/pay/delivernotify?access_token=" + accesstoken.ACCESS_TOKEN;

                Wxdelivernotify m_d = new Wxdelivernotify
                {
                    Id = 0,
                    Out_trade_no = out_trade_no,
                    Appid = Wx_appid,
                    Openid = openid,
                    Transid = transid,
                    Deliver_timestamp = deliver_timestamp.ToString(),
                    Timeformat = nowtime,
                    Deliver_status = deliver_status,
                    Deliver_msg = deliver_msg,
                    Requestxml = requestxml,
                    Responsexml = "",
                    Errcode = "",
                    Errmsg = "",
                    Comid = comid,
                };
                int notifyid = new WxdelivernotifyData().EditWxdelivernotify(m_d);
                m_d.Id = notifyid;

                string responsexml = new GetUrlData().HttpPost(requesturl, requestxml);
                JavaScriptSerializer ser = new JavaScriptSerializer();
                OuterClass1 foo = ser.Deserialize<OuterClass1>(responsexml);

                m_d.Errcode = foo.errcode;
                m_d.Errmsg = foo.errmsg;
                m_d.Responsexml = responsexml;
                new WxdelivernotifyData().EditWxdelivernotify(m_d);
            }
            catch (Exception e)
            {

            }

        }
    }
    public class OuterClass1
    {
        public string errcode;
        public string errmsg;
    }
}

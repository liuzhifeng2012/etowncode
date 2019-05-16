using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Modle;
using ETS.JsonFactory;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.PM.Service.PMService.Data;




namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// FinanceHandler 的摘要说明
    /// </summary>
    public class FinanceHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string oper = context.Request["oper"].ConvertTo<string>("");
            if (oper != "")
            {
                if (oper == "selhotelprojectlist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = FinanceJsonData.Selhotelprojectlist(comid);
                    context.Response.Write(data);
                }

                if (oper == "selhotelproductlist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var projectid = context.Request["projectid"].ConvertTo<int>(0);
                    string data = FinanceJsonData.Selhotelproductlist(comid, projectid);
                    context.Response.Write(data);
                }
                if (oper == "selprojectlist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = FinanceJsonData.Selprojectlist(comid);
                    context.Response.Write(data);
                }
                if (oper == "HotelOrderlist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var begindate = context.Request["begindate"].ConvertTo<string>("");
                    var enddate = context.Request["enddate"].ConvertTo<string>("");
                    var productid = context.Request["productid"].ConvertTo<int>(0);
                    var orderstate = context.Request["orderstate"].ConvertTo<string>("");
                    string data = FinanceJsonData.HotelOrderlist(comid, begindate, enddate, productid, orderstate);
                    context.Response.Write(data);
                }
               
                if (oper == "HotelOrderStat")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var begindate = context.Request["begindate"].ConvertTo<string>("");
                    var enddate = context.Request["enddate"].ConvertTo<string>("");
                    var projectid = context.Request["projectid"].ConvertTo<int>(0);
                    var productid = context.Request["productid"].ConvertTo<int>(0);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var orderstate = context.Request["orderstate"].ConvertTo<string>("");
                    string data = FinanceJsonData.HotelOrderStat(comid, begindate, enddate, projectid, productid, key, orderstate);
                    context.Response.Write(data);
                }
                if (oper == "FinanceStat")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var begindate = context.Request["begindate"].ConvertTo<string>("");
                    var enddate = context.Request["enddate"].ConvertTo<string>("");
                    var projectid = context.Request["projectid"].ConvertTo<int>(0);
                    var productid = context.Request["productid"].ConvertTo<int>(0);
                    string data = FinanceJsonData.FinanceStat(comid, begindate, enddate, projectid, productid);
                    context.Response.Write(data);
                }
                if (oper == "Selpayment_type")
                {
                    string data = FinanceJsonData.Selpayment_type();

                    context.Response.Write(data);
                }
                if (oper == "Selmoney_come")
                {
                    string data = FinanceJsonData.Selmoney_come();

                    context.Response.Write(data);
                }
                if (oper == "issetfinancepaytype")
                {

                    var comid = context.Request["comid"].ConvertTo<int>(0);

                    var data = "{\"type\":\"1\",\"msg\":\"\"}";
                    if (comid == 0)
                    {
                        data = "{\"type\":\"1\",\"msg\":\"传参数失败\"}";
                    }
                    B2b_finance_paytype model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(comid);

                    if (model != null)
                    {
                        //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "c");
                        //商家微信支付的所有参数都存在
                        if (model.Wx_appid != "" && model.Wx_appkey != "" && model.Wx_partnerid != "" && model.Wx_paysignkey != "")
                        {
                            data = "{\"type\":\"100\",\"msg\":\"\"}";
                        }
                        else
                        {
                            data = "{\"type\":\"1\",\"msg\":\"微信财务参数设置不完全\"}";
                        }
                    }
                    else
                    {
                        data = "{\"type\":\"1\",\"msg\":\"微信财务参数没有设置\"}";
                    }
                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", data);
                    context.Response.Write(data);
                }
                if (oper == "Financelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");
                    string data = FinanceJsonData.FinancePageList(comid, pageindex, pagesize, key);

                    context.Response.Write(data);
                }
                if (oper == "Financecount")
                {
                    var comid = context.Request["comid"];
                    var stardate = context.Request["stardate"].ConvertTo<string>(DateTime.Now.ToString("yyyy-MM-dd"));
                    var enddate = context.Request["enddate"].ConvertTo<string>(DateTime.Now.ToString("yyyy-MM-dd"));
                    string data = FinanceJsonData.Financecount(comid, stardate, enddate);

                    context.Response.Write(data);
                }

                if (oper == "ChannelFlist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);
                    var key = context.Request["key"].ConvertTo<string>("");
                    string data = FinanceJsonData.ChannelFPageList(comid, pageindex, pagesize, key, channelcompanyid);

                    context.Response.Write(data);
                }
                if (oper == "ChannelFcount")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);
                    string data = FinanceJsonData.GetChannelFinanceCount(comid, channelcompanyid);

                    context.Response.Write(data);
                }

                if (oper == "MasterFinancelist")
                {
                    var comid = "999999";
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");
                    string data = FinanceJsonData.FinancePageList(comid, pageindex, pagesize, key);

                    context.Response.Write(data);
                }

                //获取所有财务记录，并获取 支付宝财务通对账号
                if (oper == "ComFinancelist")
                {
                    var comid = "999999";
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var oid = context.Request["oid"].ConvertTo<int>(0);

                    var payment_type = context.Request["payment_type"].ConvertTo<string>("");
                    var money_come = context.Request["money_come"].ConvertTo<string>("");
                    var starttime = context.Request["starttime"].ConvertTo<string>("");
                    var endtime = context.Request["endtime"].ConvertTo<string>("");


                    string data = FinanceJsonData.ComFinancePageList(comid, pageindex, pagesize, key, oid, payment_type, money_come, starttime, endtime);

                    context.Response.Write(data);
                }


                if (oper == "WithdrawConf")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int id = context.Request["id"].ConvertTo<int>(0);
                    string remarks = context.Request["remarks"].ConvertTo<string>("");

                    int printscreen = context.Request["printscreen"].ConvertTo<int>(0);

                    string data = "";
                    try
                    {
                        data = FinanceJsonData.WithdrawConf(id, comid, remarks, printscreen);

                    }
                    catch
                    {
                        data = "";
                    }
                    context.Response.Write(data);
                }
                if (oper == "getpaytype")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = "";//获取公司基本信息和扩展信息
                    try
                    {
                        data = FinanceJsonData.FinancePayType(comid);

                    }
                    catch
                    {
                        data = "";
                    }
                    context.Response.Write(data);
                }

                //修改支付方式
                if (oper == "editpaytype")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int act = context.Request["act"].ConvertTo<int>(0);
                    int paytype = Int32.Parse(context.Request["paytype"]);

                    B2b_Finance findate = new B2b_Finance()
                    {
                        Id = id,
                        Com_id = comid,
                        Act = act,
                        Paytype = paytype

                    };

                    var data = FinanceJsonData.ModifyFinancePayType(findate);
                    context.Response.Write(data);

                }

                //提现
                if (oper == "Withdraw")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    decimal money = 0 - context.Request["money"].ConvertTo<decimal>(0);//提现直接为负数
                    string bank_account = context.Request["bank_account"];
                    string bank_card = context.Request["bank_card"];
                    string bank_name = context.Request["bank_name"];

                    //得到商家信息,账户余额
                    B2b_company modelcom = B2bCompanyData.GetCompany(comid);
                    decimal Over_money = modelcom.Imprest + money;

                    B2b_Finance findate = new B2b_Finance()
                    {
                        Id = id,
                        Com_id = comid,
                        Money = money,
                        Money_come = bank_account,
                        Servicesname = bank_account + ":" + bank_card + ":" + bank_name,
                        Payment = 1,
                        SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),
                        Payment_type = "商家提现",
                        Over_money = Over_money
                    };

                    var data = FinanceJsonData.ModifyFinanceWithdraw(findate);
                    context.Response.Write(data);

                }
                //修改支付银行状态
                if (oper == "Upbanktype")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int type = context.Request["type"].ConvertTo<int>(0);
                    var data = "";
                    B2bFinanceData fdate = new B2bFinanceData();
                    B2b_finance_paytype com = fdate.FinancePayType(comid);
                    if (com != null)
                    {
                        com.Uptype = type;
                        data = FinanceJsonData.ModifyFinancePayBank(com);
                    }
                    context.Response.Write(data);
                }

                //修改支付银行
                if (oper == "editpaybank")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int paytype = Int32.Parse(context.Request["paytype"]);
                    string bank_account = context.Request["bank_account"];
                    string bank_card = context.Request["bank_card"];
                    string bank_name = context.Request["bank_name"];
                    string alipay_account = context.Request["alipay_account"];
                    string alipay_id = context.Request["alipay_id"];
                    string alipay_key = context.Request["alipay_key"];
                    bool type = context.Request["type"].ConvertTo<bool>(false);
                    int Uptype = 0;
                    if (type == true)
                    {
                        Uptype = 1;
                    }

                    B2b_finance_paytype findate = new B2b_finance_paytype()
                    {
                        Id = id,
                        Com_id = comid,
                        Bank_account = bank_account,
                        Bank_card = bank_card,
                        Bank_name = bank_name,
                        Alipay_account = alipay_account,
                        Alipay_id = alipay_id,
                        Alipay_key = alipay_key,
                        Paytype = paytype,
                        Uptype = Uptype,


                    };

                    var data = FinanceJsonData.ModifyFinancePayBank(findate);
                    context.Response.Write(data);

                }

                //修改微信支付设置
                if (oper == "editpaywx")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string appid = context.Request["appid"];
                    string appkey = context.Request["appkey"];
                    string paysignkey = context.Request["paysignkey"];
                    string partnerid = context.Request["partnerid"];
                    string partnerkey = context.Request["partnerkey"];

                    string wx_SSLCERT_PATH = context.Request["wx_SSLCERT_PATH"].ConvertTo<string>("");
                    string wx_SSLCERT_PASSWORD = context.Request["wx_SSLCERT_PASSWORD"].ConvertTo<string>("");

                    B2b_finance_paytype findate2 = new B2b_finance_paytype()
                    {
                        Id = id,
                        Com_id = comid,
                        Wx_appid = appid,
                        Wx_appkey = appkey,
                        Wx_partnerid = partnerid,
                        Wx_paysignkey = paysignkey,
                        Wx_partnerkey = partnerkey,
                        wx_SSLCERT_PATH = wx_SSLCERT_PATH,
                        wx_SSLCERT_PASSWORD = wx_SSLCERT_PASSWORD
                    };

                    var data = FinanceJsonData.ModifyFinancePayWX(findate2);
                    context.Response.Write(data);

                }



                if (oper == "integrallist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    string key = context.Request["key"].ConvertTo<string>("");

                    string data = FinanceJsonData.IntegralList(pageindex, pagesize, comid,key);

                    context.Response.Write(data);
                    //context.Response.Write("Hello World");
                }

                if (oper == "integralcount")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = FinanceJsonData.IntegralCount(comid);
                    context.Response.Write(data);
                    //context.Response.Write("Hello World");
                }


                if (oper == "imprestlist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    string key = context.Request["key"].ConvertTo<string>("");

                    string data = FinanceJsonData.ImprestList(pageindex, pagesize, comid,key);

                    context.Response.Write(data);
                    //context.Response.Write("Hello World");
                }

                if (oper == "imprestcount")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = FinanceJsonData.ImprestCount(comid);
                    context.Response.Write(data);
                    //context.Response.Write("Hello World");
                }

                if (oper == "shougongqueren")
                {
                    var trade_no = context.Request["trade_no"].ConvertTo<string>("");
                    var order_no = context.Request["order_no"].ConvertTo<int>(0);
                    var total_fee = context.Request["total_fee"].ConvertTo<decimal>(0);
                    string data = FinanceJsonData.shougongqueren(trade_no, order_no, total_fee);
                    context.Response.Write(data);
                    //context.Response.Write("Hello World");
                }


            }
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
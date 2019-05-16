using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Collections.Generic;
using Com.Alipay;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Common.Business;
using ETS.JsonFactory;

/// <summary>
/// 功能：服务器异步通知页面
/// 版本：3.2
/// 日期：2011-03-11
/// 说明：
/// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
/// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
/// 
/// ///////////////////页面功能说明///////////////////
/// 创建该页面文件时，请留心该页面文件中无任何HTML代码及空格。
/// 该页面不能在本机电脑测试，请到服务器上做测试。请确保外部可以访问该页面。
/// 该页面调试工具请使用写文本函数logResult。
/// 如果没有收到该页面返回的 success 信息，支付宝会在24小时内按一定的时间策略重发通知
/// TRADE_FINISHED(表示交易已经成功结束，并不能再对该交易做后续操作);
/// TRADE_SUCCESS(表示交易已经成功结束，可以对该交易做后续操作，如：分润、退款等);
/// </summary>

namespace ETS2.WebApp.UI.VASUI.alipay
{
    public partial class notify_url : System.Web.UI.Page
    {
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> sPara = GetRequestPost();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request["notify_id"], Request["sign"]);

                if (verifyResult)//验证成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码

                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表
                    string trade_no = Request["trade_no"];         //支付宝交易号
                    int order_no = Int32.Parse(Request["out_trade_no"]);     //获取订单号
                    decimal total_fee = decimal.Parse(Request["total_fee"]);       //获取总金额
                    string subject = Request["subject"];           //商品名称、订单名称
                    string body = Request.Form["body"];                 //商品描述、订单备注、描述
                    string buyer_email = Request["buyer_email"];   //买家支付宝账号
                    string trade_status = Request["trade_status"]; //交易状态

                    if (trade_status == "TRADE_FINISHED" || trade_status == "TRADE_SUCCESS")
                    {
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序
                        #region
                        //查询支付，并修改为支付成功
                        //B2bPayData datapay = new B2bPayData();
                        //B2b_pay modelb2pay = datapay.GetPayByoId(order_no);

                        //if (modelb2pay.Trade_status != "TRADE_SUCCESS")//判断原状态是否为不成功
                        //{
                        //    if (modelb2pay.Total_fee == total_fee)//判断支付金额是否相同
                        //    {
                        //        //B2b_pay payinfo = new B2b_pay()
                        //        //{
                        //        //    Id = modelb2pay.Id,
                        //        //    Trade_no = trade_no,
                        //        //    Trade_status = "TRADE_SUCCESS",
                        //        //};

                        //        modelb2pay.Trade_no = trade_no;
                        //        modelb2pay.Trade_status = "TRADE_SUCCESS";
                        //        int payid = datapay.InsertOrUpdate(modelb2pay);//修改支付状态


                        //        //根据订单id得到订单信息
                        //        B2bOrderData dataorder = new B2bOrderData();
                        //        B2b_order modelb2border = dataorder.GetOrderById(order_no);

                        //        //---------------新增1begin--------------//
                        //        modelb2border.Pay_state = (int)PayStatus.HasPay;
                        //        modelb2border.Order_state = (int)OrderStatus.HasPay;
                        //        //修改此订单的支付状态为“支付成功” ,订单状态为“已付款”
                        //        dataorder.InsertOrUpdate(modelb2border);
                        //        //---------------新增1end--------------//

                        //        //根据产品id得到产品信息
                        //        B2bComProData datapro = new B2bComProData();
                        //        B2b_com_pro modelcompro = datapro.GetProById(modelb2border.Pro_id.ToString());

                        //        //得到商家信息,账户余额
                        //        B2b_company modelcom = B2bCompanyData.GetCompany(modelcompro.Com_id);
                        //        decimal Over_money = modelcom.Imprest + total_fee;

                        //        //得到支付方式，如果是支付到自己的支付宝账户则需要增加退出记录
                        //        B2bFinanceData datefin = new B2bFinanceData();
                        //        B2b_Finance modelfin = datefin.FinancePayType(modelcompro.Com_id);
                        //        int Paytype_int = 1;//支付款到 易城=1 支付到自己=2

                        //        if (modelfin != null)
                        //        {
                        //            Paytype_int = modelfin.Paytype;
                        //        }


                        //        //增加财务记录
                        //        B2bFinanceData Financed = new B2bFinanceData();
                        //        B2b_Finance Financeinfo = new B2b_Finance()
                        //        {

                        //            Id = 0,
                        //            Com_id = modelcompro.Com_id,
                        //            Agent_id = 0,           //分销编号（默认为0）
                        //            Eid = 0,                //易城账户（默认为0，0=没有交易，1=和易城交易）
                        //            Order_id = modelcompro.Id,           //订单号（默认为0）
                        //            Servicesname = modelcompro.Pro_name,       //交易名称/内容
                        //            SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                        //            Money = total_fee,              //金额
                        //            Payment = 0,            //收支(0=收款,1=支出)
                        //            Payment_type = "直销收款",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点）
                        //            Money_come = "支付宝",         //资金来源（网上支付,银行收款等）
                        //            Over_money = Over_money,       //余额（根据商家，分销，易城，编号显示相应余额）
                        //            Paytype = Paytype_int

                        //        };
                        //        int finaceid = Financed.InsertOrUpdate(Financeinfo);


                        //        //如果是支付到商户的支付宝则增加一笔支出操作与上面做平
                        //        if (Paytype_int == 2)
                        //        {
                        //            B2b_Finance Financebackinfo = new B2b_Finance()
                        //            {
                        //                Id = 0,
                        //                Com_id = modelcompro.Com_id,
                        //                Agent_id = 0,           //分销编号（默认为0）
                        //                Eid = 0,                //易城账户（默认为0，0=没有交易，1=和易城交易）
                        //                Order_id = modelcompro.Id,           //订单号（默认为0）
                        //                Servicesname = modelcompro.Pro_name,       //交易名称/内容
                        //                SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                        //                Money = 0 - total_fee,              //金额
                        //                Payment = 1,            //收支(0=收款,1=支出)
                        //                Payment_type = "提现-商家支付宝",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点）
                        //                Money_come = "支付宝",         //资金来源（网上支付,银行收款等）
                        //                Over_money = Over_money - total_fee       //余额（根据商家，分销，易城，编号显示相应余额）
                        //            };
                        //            int finacebackid = Financed.InsertOrUpdate(Financebackinfo);

                        //        }

                        //        //---------------------------生成电子票并发送-------------------------------
                        //        //如果订单“已付款”则执行发码操作，否则跳过
                        //        if ((int)modelb2border.Order_state == (int)OrderStatus.HasPay)
                        //        {

                        //            //生成电子码
                        //            int u_num = modelb2border.U_num;
                        //            int comid = modelcompro.Com_id;
                        //            RandomCodeData datarandomcode = new RandomCodeData();
                        //            RandomCode modelrandomcode = datarandomcode.GetRandomCode();//得到未用随机码对象


                        //            //设置取出的电子码状态为1（已使用）
                        //            modelrandomcode.State = 1;
                        //            int randomcodeid = datarandomcode.InsertOrUpdate(modelrandomcode);
                        //            string eticketcode = "9" + comid.ToString() + modelrandomcode.Code.ToString();
                        //            string sendstr = "";

                        //            //录入电子票列表
                        //            B2bEticketData eticketdata = new B2bEticketData();
                        //            B2b_eticket eticket = new B2b_eticket()
                        //            {

                        //                Id = 0,
                        //                Com_id = comid,
                        //                Pro_id = modelcompro.Id,
                        //                Agent_id = 0,//直销
                        //                Pno = eticketcode,
                        //                E_type = (int)EticketCodeType.ShuZiMa,
                        //                Pnum = modelb2border.U_num,
                        //                Use_pnum = modelb2border.U_num,
                        //                E_proname = modelcompro.Pro_name,
                        //                E_face_price = modelcompro.Face_price,
                        //                E_sale_price = modelcompro.Advise_price,
                        //                E_cost_price = modelcompro.Agentsettle_price,
                        //                V_state = (int)EticketCodeStatus.NotValidate,
                        //                Send_state = (int)EticketCodeSendStatus.NotSend,
                        //                Subdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                        //            };
                        //            int eticketid = eticketdata.InsertOrUpdate(eticket);
                        //            if (eticketid > 0)
                        //            {
                        //                //生成电子码短信，稍后可单独写类或写到数据库中
                        //                //diveticketcode.InnerText = "电子码生成成功:" + eticketcode;
                        //                sendstr = "感谢您订购" + modelcompro.Pro_name + modelb2border.U_num + "张" + ",电子码:" + eticketcode + " 有效期至:" + modelcompro.Pro_end.ToString("yyyy-MM-dd") + "二维码:http://" + RequestUrl + "/qr.ashx?p=" + eticketcode + "【微旅行】";

                        //                //电子票发送日志表，创建发送记录
                        //                B2bEticketSendLogData eticketsnedlog = new B2bEticketSendLogData();
                        //                B2b_eticket_send_log eticketlog = new B2b_eticket_send_log()
                        //                {
                        //                    Id = 0,
                        //                    Eticket_id = eticketid,
                        //                    Pnotext = sendstr,
                        //                    Phone = modelb2border.U_phone,
                        //                    Sendstate = 0,
                        //                    Sendtype = 1,
                        //                    Senddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                        //                };
                        //                int insertsendEticketid = eticketsnedlog.InsertOrUpdate(eticketlog);


                        //                //发送电子码
                        //                string msg = "";
                        //                int sendstate = 0;//发送状态 1=成功，2=失败 0=未发送
                        //                int sendback = SendSmsHelper.SendSms(modelb2border.U_phone, sendstr, out msg);
                        //                if (sendback > 0)
                        //                {
                        //                    //labelmsg.InnerText = "发送成功" + sendback;
                        //                    sendstate = 1;
                        //                    //修改电子票发送日志表的发码状态为成功
                        //                    B2b_eticket_send_log eticketlogup = new B2b_eticket_send_log()
                        //                    {
                        //                        Id = insertsendEticketid,
                        //                        Sendstate = 1,
                        //                        Senddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                        //                    };
                        //                    int upsendEticket = eticketsnedlog.InsertOrUpdate(eticketlogup);
                        //                    //-----------------新增2 begin-------------------------//
                        //                    modelb2border.Send_state = (int)SendCodeStatus.HasSend;
                        //                    modelb2border.Order_state = (int)OrderStatus.HasSendCode;
                        //                    modelb2border.Ticketcode = eticketid;
                        //                    //修改订单中发码状态为“已发码”，订单状态为"已发码"，电子码id输入
                        //                    dataorder.InsertOrUpdate(modelb2border);
                        //                    //------------------新增2 end---------------------------//
                        //                }
                        //                else
                        //                {
                        //                    //labelmsg.InnerText = "发送错误" + msg;

                        //                    sendstate = 2;
                        //                }

                        //                //记录短信日志表
                        //                B2bSmsMobileSendDate smsmobilelog = new B2bSmsMobileSendDate();
                        //                B2b_smsmobilesend smslog = new B2b_smsmobilesend()
                        //                {
                        //                    Mobile = modelb2border.U_phone,
                        //                    Content = sendstr,
                        //                    Flag = sendstate,
                        //                    Text = msg,
                        //                    Delaysendtime = "",
                        //                    Realsendtime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        //                    Smsid = sendback,
                        //                    Sendeticketid = insertsendEticketid
                        //                };
                        //                int insertsendmobileid = smsmobilelog.InsertOrUpdate(smslog);

                        //            }
                        //            else
                        //            {
                        //                //diveticketcode.InnerText = "电子码生成ERROR";

                        //            }

                        //        }



                        //    }

                        //}
                        #endregion


                        string retunstr = new PayReturnSendEticketData().PayReturnSendEticket(trade_no, order_no, total_fee, trade_status);

                        //对分销充值订单支付返回，对绑定订单进行处理确认
                        var handdata = OrderJsonData.agentorderpay_Hand(order_no);

                    }
                   

                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                    Response.Write("success");  //请不要修改或删除
                   
                    #region 赠送保险
                    OrderJsonData.ZengsongBaoxian(order_no);
                    #endregion

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else//验证失败
                {
                    Response.Write("fail");
                }
            }
            else
            {
                Response.Write("无通知参数");
            }
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }


        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestGet()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;



        }
    }

}
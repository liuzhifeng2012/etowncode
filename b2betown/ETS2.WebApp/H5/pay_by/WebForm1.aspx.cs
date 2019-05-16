using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using ComH5.Alipay;
using ETS.Framework;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Modle;


namespace ETS2.WebApp.H5.pay_by
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        #region 辨别商家参数
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        public int comid = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            int orderid = Request["out_trade_no"].ConvertTo<int>(0);
            //根据订单id得到订单信息
            B2bOrderData dataorder = new B2bOrderData();
            B2b_order modelb2border = dataorder.GetOrderById(orderid);

            //根据产品id得到产品信息
            B2bComProData datapro = new B2bComProData();
            B2b_com_pro modelcompro = datapro.GetProById(modelb2border.Pro_id.ToString());

            if (modelb2border != null  )
            {
                //支付宝网关地址
                string GATEWAY_NEW = "http://wappaygw.alipay.com/service/rest.htm?";

                ////////////////////////////////////////////调用授权接口alipay.wap.trade.create.direct获取授权码token////////////////////////////////////////////

                //返回格式
                string format = "xml";
                //必填，不需要修改

                //返回格式
                string v = "2.0";
                //必填，不需要修改

                //请求号
                string req_id = DateTime.Now.ToString("yyyyMMddHHmmss");
                //必填，须保证每次请求都是唯一

                //req_data详细信息

                //服务器异步通知页面路径 
                //string notify_url = "";
                string notify_url = "http://" + RequestUrl + "/h5/pay_by/notify_url.aspx";
                //需http://格式的完整路径，不允许加?id=123这类自定义参数

                //页面跳转同步通知页面路径
                //string call_back_url = "http://127.0.0.1:64704/WS_WAP_PAYWAP-CSHARP-UTF-8/call_back_url.aspx";
                string call_back_url = "http://" + RequestUrl + "/h5/pay_by/call_back_url.aspx";
                //需http://格式的完整路径，不允许加?id=123这类自定义参数

                //操作中断返回地址
                //string merchant_url = "http://v.etown.cn";
                string merchant_url = "http://" + RequestUrl + "/h5/Orderlist.aspx";
                //用户付款中途退出返回商户的地址。需http://格式的完整路径，不允许加?id=123这类自定义参数

                //卖家支付宝帐户
                string seller_email = "wesley@etown.cn";
                //必填

                //商户订单号
                string out_trade_no = modelb2border.Id.ToString();
                //商户网站订单系统中唯一订单号，必填

                //订单名称
                string subject = "";
                if (modelb2border.Order_type == 1)
                {
                    subject = modelcompro.Pro_name;
                }
                if (modelb2border.Order_type == 2)
                {
                    if (modelb2border.serverid == "")
                    {
                        subject = "预付款充值";
                    }
                    else {
                        subject = "购买服务于押金";
                    }
                }
                //必填

                //付款金额
                string total_fee = (modelb2border.Pay_price * modelb2border.U_num+modelb2border.Express-modelb2border.Integral1-modelb2border.Imprest1).ToString();
                //必填


                if (modelb2border.Child_u_num > 0)//如果是旅游包含儿童的
                {
                    total_fee = (Decimal.Parse(total_fee) + modelb2border.Child_u_num * (modelb2border.Pay_price - modelcompro.Childreduce)).ToString();
                }


                //如果是购物车订单重新写金额
                if (modelb2border.Shopcartid != 0) {

                    total_fee = dataorder.GetCartOrderMoneyById(orderid).ToString("0.00"); ;
                
                }


                //请求业务参数详细
                string req_dataToken = "<direct_trade_create_req><notify_url>" + notify_url + "</notify_url><call_back_url>" + call_back_url + "</call_back_url><seller_account_name>" + seller_email + "</seller_account_name><out_trade_no>" + out_trade_no + "</out_trade_no><subject>" + subject + "</subject><total_fee>" + total_fee + "</total_fee><merchant_url>" + merchant_url + "</merchant_url></direct_trade_create_req>";
                //必填

                //把请求参数打包成数组
                Dictionary<string, string> sParaTempToken = new Dictionary<string, string>();
                sParaTempToken.Add("partner", Config.Partner);
                sParaTempToken.Add("_input_charset", Config.Input_charset.ToLower());
                sParaTempToken.Add("sec_id", Config.Sign_type.ToUpper());
                sParaTempToken.Add("service", "alipay.wap.trade.create.direct");
                sParaTempToken.Add("format", format);
                sParaTempToken.Add("v", v);
                sParaTempToken.Add("req_id", req_id);
                sParaTempToken.Add("req_data", req_dataToken);

                //建立请求
                string sHtmlTextToken = Submit.BuildRequest(GATEWAY_NEW, sParaTempToken);
                //URLDECODE返回的信息
                Encoding code = Encoding.GetEncoding(Config.Input_charset);
                sHtmlTextToken = HttpUtility.UrlDecode(sHtmlTextToken, code);

                //解析远程模拟提交后返回的信息
                Dictionary<string, string> dicHtmlTextToken = Submit.ParseResponse(sHtmlTextToken);

                //获取token
                string request_token = dicHtmlTextToken["request_token"];

                ////////////////////////////////////////////根据授权码token调用交易接口alipay.wap.auth.authAndExecute////////////////////////////////////////////


                //业务详细
                string req_data = "<auth_and_execute_req><request_token>" + request_token + "</request_token></auth_and_execute_req>";
                //必填

                //把请求参数打包成数组
                Dictionary<string, string> sParaTemp = new Dictionary<string, string>();
                sParaTemp.Add("partner", Config.Partner);
                sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
                sParaTemp.Add("sec_id", Config.Sign_type.ToUpper());
                sParaTemp.Add("service", "alipay.wap.auth.authAndExecute");
                sParaTemp.Add("format", format);
                sParaTemp.Add("v", v);
                sParaTemp.Add("req_data", req_data);







                //写入支付数据库,先判定是否有此订单支付
                //根据订单id得到订单信息
                B2bPayData datapay = new B2bPayData();
                B2b_pay modelb2pay = datapay.GetPayByoId(orderid);


                if (modelb2pay == null)
                {
                    B2b_pay eticket = new B2b_pay()
                    {
                        Id = 0,
                        Oid = orderid,
                        Pay_com = "malipay",
                        Pay_name = modelb2border.U_name,
                        Pay_phone = modelb2border.U_phone,
                        Total_fee = decimal.Parse(total_fee),
                        Trade_no = "",
                        Trade_status = "trade_pendpay",
                        Uip = "",
                        comid = 106
                    };
                    int payid = datapay.InsertOrUpdate(eticket);
                }
                else
                {
                    //对已完成支付的，再次提交支付，跳转到订单也或显示此订单已支付
                    if (modelb2pay.Trade_status != "TRADE_SUCCESS")
                    {
                        //防止金额有所改动
                        modelb2pay.comid = 106;
                        modelb2pay.Pay_com = "malipay";
                        modelb2pay.Total_fee = decimal.Parse(total_fee);
                        datapay.InsertOrUpdate(modelb2pay);
                    }
                }












                //建立请求
                string sHtmlText = Submit.BuildRequest(GATEWAY_NEW, sParaTemp, "get", "确认");
                Response.Write(sHtmlText);
            }
        }
    }
}
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
using ComH5.Alipay;
using ETS2.VAS.Service.VASService.Data;
using ETS2.PM.Service.PMService.Data;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.JsonFactory;
using ETS.Framework;

namespace ETS2.WebApp.H5
{
    public partial class call_back_url : System.Web.UI.Page
    {
        #region 辨别商家参数
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        #endregion
        public string title = "";
        public string phone = "";
        public string comname = "";
        public int comid = 0;
        public int order_type = 0;
        public bool bo = false;//是否是手机端


        public int orderid = 0;
        public string md5 = "";
        public int servertype = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            bo = detectmobilebrowser.HttpUserAgent(u);


            Dictionary<string, string> sPara = GetRequestGet();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.VerifyReturn(sPara, Request.QueryString["sign"]);

                string trade_no = Request["trade_no"];         //支付宝交易号
                int order_no = Int32.Parse(Request["out_trade_no"]);     //获取订单号
                orderid = order_no;
                decimal total_fee = 0;       //获取总金额
                if (verifyResult)//验证成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码


                    //获取访问的商户COMID
                    if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式，则从多微信商户基本信息表中获取comid
                    {
                        comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl));
                    }
                    else
                    {

                        B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestUrl);
                        if (companyinfo != null)
                        {
                            comid = companyinfo.Com_id;
                        }
                    }



                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中页面跳转同步通知参数列表

                    ////商户订单号
                    //string out_trade_no = Request.QueryString["out_trade_no"];

                    ////支付宝交易号
                    //string trade_no = Request.QueryString["trade_no"];

                    B2bPayData datapay = new B2bPayData();
                    B2b_pay modelb2pay = datapay.GetPayByoId(order_no);

                    md5 = EncryptionHelper.ToMD5(order_no.ToString() + "lixh1210", "UTF-8");

                    B2b_order orderdate = new B2bOrderData().GetOrderById(order_no);
                    if (orderdate != null)
                    {
                        order_type = orderdate.Order_type;

                        var saleset = B2bCompanySaleSetData.GetDirectSellByComid(orderdate.Comid.ToString());
                        if (saleset != null)
                        {
                            phone = saleset.Service_Phone;
                        }

                        var comdata = B2bCompanyData.GetCompany(orderdate.Comid);
                        if (saleset != null)
                        {
                            comname = comdata.Com_name;
                        }


                        //根据产品id得到产品信息
                        B2bComProData datapro = new B2bComProData();
                        B2b_com_pro modelcompro = datapro.GetProById(orderdate.Pro_id.ToString(), orderdate.Speciid, orderdate.channelcoachid);

                        if (modelcompro != null)
                        {
                            servertype = modelcompro.Server_type;
                        }


                    }

                    if (modelb2pay != null)
                    {
                        total_fee = modelb2pay.Total_fee;
                    }

                    ////交易状态
                    string result = Request.QueryString["result"];


                    //判断是否在商户网站中已经做过了这次通知返回的处理
                    //如果没有做过处理，那么执行商户的业务程序
                    //如果有做过处理，那么不执行商户的业务程序

                    //打印页面
                    //Response.Write("验证成功<br />");订单支付 成功！
                    string retunstr = new PayReturnSendEticketData().PayReturnSendEticket(trade_no, order_no, total_fee, result);
                    title = "订单支付 成功！";

                    //对分销充值订单支付返回，对绑定订单进行处理确认
                    var handdata = OrderJsonData.agentorderpay_Hand(order_no);

                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else//验证失败
                {
                    //Response.Write("验证失败");
                    title = "订单支付 出现错误";
                }
            }
            else
            {
                //Response.Write("无返回参数");
                title = "订单 没有 支付";
            }
        }

        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public Dictionary<string, string> GetRequestGet()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
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
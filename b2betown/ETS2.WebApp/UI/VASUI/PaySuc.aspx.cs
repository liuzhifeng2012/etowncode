using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.Common.Business;
using Com.Alipay;
using System.Collections.Specialized;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.JsonFactory;

namespace ETS2.WebApp.UI.VASUI
{
    public partial class PaySuc : System.Web.UI.Page
    {

        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        protected void Page_Load(object sender, EventArgs e)
        {

            SortedDictionary<string, string> sPara = GetRequestGet();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.QueryString["notify_id"], Request.QueryString["sign"]);

                if (verifyResult)//验证成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码

                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中页面跳转同步通知参数列表
                    string trade_no = Request.QueryString["trade_no"];              //支付宝交易号
                    int order_no = Int32.Parse(Request.QueryString["out_trade_no"]);	        //获取订单号
                    decimal total_fee = decimal.Parse(Request.QueryString["total_fee"]);           //获取总金额
                    string subject = Request.QueryString["subject"];                //商品名称、订单名称
                    string body = Request.QueryString["body"];                      //商品描述、订单备注、描述
                    string buyer_email = Request.QueryString["buyer_email"];        //买家支付宝账号
                    string trade_status = Request.QueryString["trade_status"];      //交易状态

                    if (Request.QueryString["trade_status"] == "TRADE_FINISHED" || Request.QueryString["trade_status"] == "TRADE_SUCCESS")
                    {

                        string retunstr = new PayReturnSendEticketData().PayReturnSendEticket(trade_no, order_no, total_fee, trade_status);

                        //对分销充值订单支付返回，对绑定订单进行处理确认
                        var handdata = OrderJsonData.agentorderpay_Hand(order_no);

                        diveticketcode.InnerText = ("您已成功支付" + total_fee.ToString() + "元！");

                    }
                    else
                    {
                        diveticketcode.InnerText = ("支付返回错误：" + Request.QueryString["trade_status"]);
                    }
                }
                else
                {
                    //支付宝验证失败，应该是微信支付返回成功
                    int out_trade_no = Request["out_trade_no"].ConvertTo<int>(0);
                    B2b_pay wxpay = new B2bPayData().GetSUCCESSPayById(out_trade_no);
                    if (wxpay != null)
                    {
                        diveticketcode.InnerText = ("您已成功支付" + wxpay.Total_fee + "元！");
                    }
                }

            }
            else
            {
                diveticketcode.InnerText = ("无返回参数");
            }
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


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
using System.Xml;
using ComH5.Alipay;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS.JsonFactory;
 

/// <summary>
/// 功能：服务器异步通知页面
/// 版本：3.3
/// 日期：2012-07-10
/// 说明：
/// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
/// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
/// 
/// ///////////////////页面功能说明///////////////////
/// 创建该页面文件时，请留心该页面文件中无任何HTML代码及空格。
/// 该页面不能在本机电脑测试，请到服务器上做测试。请确保外部可以访问该页面。
/// 该页面调试工具请使用写文本函数logResult。
/// 如果没有收到该页面返回的 success 信息，支付宝会在24小时内按一定的时间策略重发通知
/// </summary>
public partial class notify_url : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {


        Dictionary<string, string> sPara = GetRequestPost();

        if (sPara.Count > 0)//判断是否有带返回参数
        {
            Notify aliNotify = new Notify();
            bool verifyResult = aliNotify.VerifyNotify(sPara, Request.Form["sign"]);

          

            if (verifyResult)//验证成功
            {
                

                //XML解析notify_data数据
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(sPara["notify_data"]);
                    //商户订单号
                    string order_no = xmlDoc.SelectSingleNode("/notify/out_trade_no").InnerText;
                    //支付宝交易号
                    string trade_no = xmlDoc.SelectSingleNode("/notify/trade_no").InnerText;
                    //交易状态
                    string trade_status = xmlDoc.SelectSingleNode("/notify/trade_status").InnerText;
                    decimal total_fee = decimal.Parse(xmlDoc.SelectSingleNode("/notify/total_fee").InnerText);

                    if (trade_status == "TRADE_FINISHED" || trade_status == "TRADE_SUCCESS")
                    {
                      

                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序

                        //注意：
                        //该种交易状态只在两种情况下出现
                        //1、开通了普通即时到账，买家付款成功后。
                        //2、开通了高级即时到账，从该笔交易成功时间算起，过了签约时的可退款时限（如：三个月以内可退款、一年以内可退款等）后。
                        //B2bPayData datapay = new B2bPayData();
                        //B2b_pay modelb2pay = datapay.GetPayByoId(Int32.Parse(order_no));
                        //if (modelb2pay != null)
                        //{
                        //    total_fee = modelb2pay.Total_fee;
                        //}

                        string retunstr = new PayReturnSendEticketData().PayReturnSendEticket(trade_no, Int32.Parse(order_no), total_fee, trade_status);

                        Response.Write("success");  //请不要修改或删除

                        #region 赠送保险
                        OrderJsonData.ZengsongBaoxian(Int32.Parse(order_no));
                        #endregion
                    }
                    else
                    {
                        Response.Write(trade_status);
                     
                    }

                }
                catch (Exception exc)
                {
                    Response.Write(exc.ToString());
                   
                }



                //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

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
    public Dictionary<string, string> GetRequestPost()
    {
        int i = 0;
        Dictionary<string, string> sArray = new Dictionary<string, string>();
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
}

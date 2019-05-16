using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using System.Xml;
using Com.Alipay;
using ETS2.VAS.Service.VASService.Data;
using Com.Tenpay.TenpayApp;
using ETS2.Common.Business;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS.JsonFactory;

namespace ETS2.WebApp.wxpaytest
{
    public partial class backpaynotice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ResponseHandler resHandler = new ResponseHandler(Context);
            resHandler.init();

            string RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();

            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            //bool bo = detectmobilebrowser.HttpUserAgent(u);//判断是否是手机访问
            //if (bo)
            //{
            if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式
            {
                //先通过正则表达式获取COMid
                int comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl).ToString());
                B2b_finance_paytype modelfinance = new B2b_finance_paytypeData().GetFinancePayTypeByComid(comid);
                resHandler.setKey(modelfinance.Wx_partnerkey, modelfinance.Wx_paysignkey);
            }
            else 
            {
                Response.Write("获取商家支付参数失败");
                return;
            }
            //}




            //判断签名
            if (resHandler.isTenpaySign())
            {

                if (resHandler.isWXsign())
                {
                    //商户在收到后台通知后根据通知ID向财付通发起验证确认，采用后台系统调用交互模式
                    string notify_id = resHandler.getParameter("notify_id");
                    //取结果参数做业务处理
                    string out_trade_no = resHandler.getParameter("out_trade_no");
                    //财付通订单号
                    string transaction_id = resHandler.getParameter("transaction_id");
                    //金额,以分为单位
                    string total_fee = resHandler.getParameter("total_fee");
                    //如果有使用折扣券，discount有值，total_fee+discount=原请求的total_fee
                    string discount = resHandler.getParameter("discount");
                    //支付结果
                    string trade_state = resHandler.getParameter("trade_state");

                    //即时到账
                    if ("0".Equals(trade_state))
                    {
                        //------------------------------
                        //处理业务开始
                        //------------------------------
                        string retunstr = new PayReturnSendEticketData().PayReturnSendEticket(notify_id, int.Parse(out_trade_no), decimal.Parse(total_fee)/100, "TRADE_SUCCESS", transaction_id);


                        //对分销充值订单支付返回，对绑定订单进行处理确认
                        var handdata = OrderJsonData.agentorderpay_Hand(int.Parse(out_trade_no));


                        //处理数据库逻辑
                        //注意交易单不要重复处理
                        //注意判断返回金额

                        //------------------------------
                        //处理业务完毕
                        //------------------------------

                        //给财付通系统发送成功信息，财付通系统收到此结果后不再进行后续通知
                        Response.Write("success 后台通知成功");
                    }
                    else
                    {
                        Response.Write("支付失败");
                        return;
                    }
                    //回复服务器处理成功
                    Response.Write("success");
                }

                else
                {//SHA1签名失败
                    Response.Write("fail -SHA1 failed");
                    Response.Write(resHandler.getDebugInfo());
                    return;
                }
            }

            else
            {//md5签名失败
                Response.Write("fail -md5 failed");
                Response.Write(resHandler.getDebugInfo());
                return;
            }
        }
    }
}
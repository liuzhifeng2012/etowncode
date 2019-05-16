using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.Tenpay.Tenpay;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS.Framework;

namespace ETS2.WebApp.tenpay
{
    public partial class payReturnUrl : System.Web.UI.Page
    {
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();

        public string title = "";
        public string phone = "";
        public string comname = "";
        public int comid = 0;
        

        protected void Page_Load(object sender, EventArgs e)
        {
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


            #region 获得财付通支付参数 tenpay_id,tenpay_key
            //根据产品判断商家是否含有自己的财付通支付:
            //a.含有的话支付到商家；
            //b.没有的话支付到平台财付通账户(易城账户，公司id=106)
            string tenpay_id = "";
            string tenpay_key = "";
            B2b_finance_paytype model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(comid);
            if (model != null)
            {
                //商家财付通支付的所有参数都存在
                if (model.Tenpay_id != "" && model.Tenpay_key != "")
                {
                    tenpay_id = model.Tenpay_id;
                    tenpay_key = model.Tenpay_key;
                }
                else
                {
                    model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(106);
                    if (model == null)
                    {
                        Response.Write("商户财付通信息设置不完全！");
                        Response.End();
                        return;
                    }
                    else
                    {
                        //商家财付通支付的所有参数都存在
                        if (model.Tenpay_id != "" && model.Tenpay_key != "")
                        {
                            tenpay_id = model.Tenpay_id;
                            tenpay_key = model.Tenpay_key;
                        }
                        else
                        {
                            Response.Write("商户财付通信息设置不完全！！");
                            Response.End();
                            return;
                        }
                    }
                }
            }
            else
            {
                model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(106);
                if (model == null)
                {
                    Response.Write("商户财付通信息设置不完全！");
                    Response.End();
                    return;
                }
                else
                {
                    //商家财付通支付的所有参数都存在
                    if (model.Tenpay_id != "" && model.Tenpay_key != "")
                    {
                        tenpay_id = model.Tenpay_id;
                        tenpay_key = model.Tenpay_key;
                    }
                    else
                    {
                        Response.Write("商户财付通信息设置不完全！！");
                        Response.End();
                        return;
                    }
                }
            }

            #endregion
          

            //创建ResponseHandler实例
            ResponseHandler resHandler = new ResponseHandler(Context);
            resHandler.setKey(tenpay_key);

            //判断签名
            if (resHandler.isTenpaySign())
            {

                ///通知id
                string notify_id = resHandler.getParameter("notify_id");
                //商户订单号
                string out_trade_no = resHandler.getParameter("out_trade_no");
                //财付通订单号
                string transaction_id = resHandler.getParameter("transaction_id");
                //金额,以分为单位
                string total_fee = resHandler.getParameter("total_fee");
                //如果有使用折扣券，discount有值，total_fee+discount=原请求的total_fee
                string discount = resHandler.getParameter("discount");
                //支付结果
                string trade_state = resHandler.getParameter("trade_state");
                //交易模式，1即时到账，2中介担保
                string trade_mode = resHandler.getParameter("trade_mode");

                if ("1".Equals(trade_mode))
                {       //即时到账 
                    if ("0".Equals(trade_state))
                    {

                       
                        B2bPayData datapay = new B2bPayData();
                        B2b_pay modelb2pay = datapay.GetPayByoId(out_trade_no.ConvertTo<int>(0));

                        B2b_order orderdate = new B2bOrderData().GetOrderById(out_trade_no.ConvertTo<int>(0));
                        if (orderdate != null)
                        {
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
                        }

                        if (modelb2pay != null)
                        {
                            total_fee = modelb2pay.Total_fee.ToString();
                        }

                        string retunstr = new PayReturnSendEticketData().PayReturnSendEticket(notify_id, out_trade_no.ConvertTo<int>(), total_fee.ConvertTo<decimal>(), "TRADE_SUCCESS");
                        title = "订单支付 成功！";

                        //Response.Write("即时到帐付款成功");
                        //给财付通系统发送成功信息，财付通系统收到此结果后不再进行后续通知

                    }
                    else
                    {
                        title = "即时到账支付失败";
                        //Response.Write("即时到账支付失败");
                    }

                }
                else if ("2".Equals(trade_mode))
                {    //中介担保
                    if ("0".Equals(trade_state))
                    {



                        title = "中介担保付款成功";
                        //Response.Write("中介担保付款成功");
                        //给财付通系统发送成功信息，财付通系统收到此结果后不再进行后续通知

                    }
                    else
                    {
                        title = "trade_state=" + trade_state;
                        //Response.Write("trade_state=" + trade_state);
                    }
                }
            }
            else
            {
                title = "认证签名失败";
                //Response.Write("认证签名失败");
            }

            //获取debug信息,建议把debug信息写入日志，方便定位问题
            string debuginfo = resHandler.getDebugInfo();
            //Response.Write("<br/>debuginfo:" + debuginfo + "<br/>");

        }
    }
}
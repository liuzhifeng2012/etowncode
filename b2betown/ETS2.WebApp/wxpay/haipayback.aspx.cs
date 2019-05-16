using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WxPayAPI;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.VAS.Service.VASService.Data;
using ETS.Framework;
using ETS2.Common.Business;
using ETS.JsonFactory;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using Newtonsoft.Json;
using Com.Tenpay.TenpayAppV3;

namespace ETS2.WebApp.wxpay
{
    public partial class haipayback : System.Web.UI.Page
    {
        /// <summary>
        /// 单为星海扫码付款做的(实现功能：微信支付统一下单，然后跳转到原生支付付款页面)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static object lockobj = new object();
        protected void Page_Load(object sender, EventArgs e)
        {
            lock (lockobj)
            {
                string RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();
                if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式
                {
                    //先通过正则表达式获取COMid
                    int comid = Domain_def.Domain_Huoqu(RequestUrl).ToString().ConvertTo<int>(0);
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
                    #region  把支付结果通知数据记录一下
                    try
                    {
                        WxPayConfig config = new WxPayConfig
                        {
                            APPID = model.Wx_appid,
                            APPSECRET = model.Wx_appkey,
                            KEY = model.Wx_paysignkey,
                            MCHID = model.Wx_partnerid,
                            IP = CommonFunc.GetRealIP(),
                            SSLCERT_PATH = model.wx_SSLCERT_PATH,
                            SSLCERT_PASSWORD = model.wx_SSLCERT_PASSWORD,
                            PROXY_URL = "",
                            LOG_LEVENL = 3,//日志级别
                            REPORT_LEVENL = 0,//上报信息配置  
                            NOTIFY_URL = "http://shop" + comid + ".etown.cn/wxpay/backpaynotice.aspx"
                        };
                        Log.Error(this.GetType().ToString(), "进入统一下单页面");
                        ProcessNotify(config, model);

                    }
                    catch (Exception ex)
                    {
                        Log.Error(this.GetType().ToString(), "微信支付通知意外错误(" + ex.Message + ")");
                        Response.Write("<xml><return_code>FAIL</return_code><return_msg>微信支付通知意外错误</return_msg></xml>");
                        return;
                    }
                    #endregion

                }
                else
                {
                    Log.Error(this.GetType().ToString(), "微信支付通知参数格式错误");
                    Response.Write("<xml><return_code>FAIL</return_code><return_msg>微信支付通知参数格式错误</return_msg></xml>");
                    return;
                }
            }
        }

        public void ProcessNotify(WxPayConfig config, B2b_finance_paytype model)
        {
            string product_id = Request["product_id"]; //订单id


            #region 根据code获取用户openid
            string code = Request.QueryString["code"];

            Log.Error(this.GetType().ToString(), product_id + "  ||   " + code);

            string openid = "";
            string url =
                string.Format(
                    "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",
                    config.APPID, config.APPSECRET, code);
            string returnStr = HttpUtil.Send("", url);

            var obj = JsonConvert.DeserializeObject<ModelOpenID>(returnStr);
            if (obj.openid == null)
            {
                url = string.Format(
                    "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}",
                    config.APPID, obj.refresh_token);
                returnStr = HttpUtil.Send("", url);
                obj = JsonConvert.DeserializeObject<ModelOpenID>(returnStr);
                openid = obj.openid;
            }
            else
            {
                openid = obj.openid;
            }
            Log.Info(this.GetType().ToString(), openid);
            #endregion


            //调统一下单接口，获得下单结果 

            WxPayData unifiedOrderResult = new WxPayData();
            try
            {
                unifiedOrderResult = UnifiedOrder(openid, product_id, config);
            }
            catch (Exception ex)//若在调统一下单接口时抛异常，立即返回结果给微信支付后台
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "统一下单失败");
                Log.Error(this.GetType().ToString(), "UnifiedOrder failure : " + res.ToXml(config), config);
                Response.Write(res.ToXml(config));
                Response.End();
            }

            //若下单失败，则立即返回结果给微信支付后台
            if (!unifiedOrderResult.IsSet("appid") || !unifiedOrderResult.IsSet("mch_id") || !unifiedOrderResult.IsSet("prepay_id"))
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "统一下单失败");
                Log.Error(this.GetType().ToString(), "UnifiedOrder failure.. : " + res.ToXml(config), config);
                Response.Write(res.ToXml(config));
                Response.End();
            }
            ////Log.Info(this.GetType().ToString(), "UnifiedOrder code_url:" + unifiedOrderResult.GetValue("code_url").ToString(), config);
            ////统一下单成功,则打开返回的预支付交易链接code_url
            //Response.Redirect(unifiedOrderResult.GetValue("code_url").ToString());
            Response.Write("<script type='text/javascript'>location.href='" + unifiedOrderResult.GetValue("code_url").ToString() + "';</script>");
        }

        private WxPayData UnifiedOrder(string openId, string productId, WxPayConfig config)
        {
            #region  productid中包含oid，则传递过来的是订单号
            if (productId.IndexOf("oid") > -1)
            {
                B2b_order morder = new B2bOrderData().GetOrderById(productId.Substring(3).ConvertTo<int>(0));
                if (morder == null)
                {
                    throw new WxPayException("原生支付订单查询失败!");
                }
                //直接付款订单
                if (morder.Pro_id == 0)
                {
                    #region 统一下单
                    if (morder.Pay_price == 0)
                    {
                        throw new WxPayException("订单金额不可为0!");
                    }

                    //统一下单
                    WxPayData req = new WxPayData();
                    req.SetValue("body", "快速支付");
                    req.SetValue("attach", "");
                    req.SetValue("out_trade_no", productId.Substring(3));
                    req.SetValue("total_fee", (morder.Pay_price * 100).ToString("F0"));
                    req.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    req.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
                    req.SetValue("goods_tag", "");
                    req.SetValue("trade_type", "NATIVE");
                    req.SetValue("openid", openId);
                    req.SetValue("product_id", productId);
                    //Log.Error(this.GetType().ToString(), "UnifiedOrder failure:" + req.ToXml(config), config);
                    WxPayData result = WxPayApi.UnifiedOrder(req, config, 10);
                    return result;
                    #endregion
                }
                //一般订单
                else
                {
                    B2b_com_pro modelcompro = new B2bComProData().GetProById(morder.Pro_id.ToString());
                    if (modelcompro == null)
                    {
                        Log.Error(this.GetType().ToString(), "UnifiedOrder failure: 原生支付产品查询失败!", config);
                        throw new WxPayException("原生支付产品查询失败!");
                    }

                    #region 统一下单
                    string price = "0";//单价
                    string pricedetail = "";//价格描述
                    decimal p_totalprice1 = new B2bOrderData().GetOrderTotalPrice(modelcompro, morder, out price, out pricedetail);
                    string wxp_totalprice = (p_totalprice1 * 100).ToString("F0");
                    if (p_totalprice1 == 0 || price == "0")
                    {
                        throw new WxPayException("订单金额不可为0!");
                    }

                    //统一下单
                    WxPayData req = new WxPayData();
                    req.SetValue("body", modelcompro.Pro_name);
                    req.SetValue("attach", "");
                    req.SetValue("out_trade_no", productId.Substring(3));
                    req.SetValue("total_fee", wxp_totalprice);
                    req.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    req.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
                    req.SetValue("goods_tag", "");
                    req.SetValue("trade_type", "NATIVE");
                    req.SetValue("openid", openId);
                    req.SetValue("product_id", productId);
                    WxPayData result = WxPayApi.UnifiedOrder(req, config);
                    return result;
                    #endregion
                }
            }
            #endregion
            #region productid中包含pid，则传递过来的是产品号
            else if (productId.IndexOf("pid") > -1)
            {
                throw new WxPayException("暂时原生支付只是用于PC线上付款!");
            }
            #endregion
            #region productid中pid和oid 均不包含
            else
            {
                throw new WxPayException("暂时原生支付只是用于PC线上付款!!");
            }
            #endregion
        }
    }
}
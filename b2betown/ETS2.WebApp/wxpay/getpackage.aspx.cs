using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.Tenpay.TenpayApp;
using ETS2.Common.Business;
using WxPayAPI;
using ETS.Framework;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.VAS.Service.VASService.Data;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.JsonFactory;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.WebApp.wxpay
{
    public partial class getpackage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();
            if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式
            {
                //先通过正则表达式获取COMid
                int comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl).ToString());
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

                //if (model != null)
                //{
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

                Log.Info(this.GetType().ToString(), "进入支付回调URL：getpackage.aspx", config);
                ProcessNotify(config);

                //}
                //else 
                //{
                //    Log.Error(this.GetType().ToString(), "获取商家支付参数失败"); 
                //    Response.Write("<xml><return_code>FAIL</return_code><return_msg>获取商家支付参数失败</return_msg></xml>");
                //    return;
                //}
            }
            else
            {
                Log.Error(this.GetType().ToString(), "传递网址格式错误");
                Response.Write("<xml><return_code>FAIL</return_code><return_msg>传递网址格式错误</return_msg></xml>");
                return;
            }
        }

        public void ProcessNotify(WxPayConfig config)
        {
            WxPayData notifyData = new Notify(this).GetNotifyData(config);

            //检查openid和product_id是否返回
            if (!notifyData.IsSet("openid") || !notifyData.IsSet("product_id"))
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "回调数据异常");
                Log.Info(this.GetType().ToString(), "The data WeChat post is error : " + res.ToXml(config), config);
                Response.Write(res.ToXml(config));
                Response.End();
            }

            //调统一下单接口，获得下单结果
            string openid = notifyData.GetValue("openid").ToString();
            string product_id = notifyData.GetValue("product_id").ToString();

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
                Log.Error(this.GetType().ToString(), "UnifiedOrder failure : " + res.ToXml(config), config);
                Response.Write(res.ToXml(config));
                Response.End();
            }

            //统一下单成功,则返回成功结果给微信支付后台
            WxPayData data = new WxPayData();
            data.SetValue("return_code", "SUCCESS");
            data.SetValue("return_msg", "OK");
            data.SetValue("appid", config.APPID);
            data.SetValue("mch_id", config.MCHID);
            data.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
            data.SetValue("prepay_id", unifiedOrderResult.GetValue("prepay_id"));
            data.SetValue("result_code", "SUCCESS");
            data.SetValue("err_code_des", "OK");
            data.SetValue("sign", data.MakeSign(config));

            Log.Info(this.GetType().ToString(), "UnifiedOrder success , send data to WeChat : " + data.ToXml(config), config);
            Response.Write(data.ToXml(config));
            Response.End();
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
                B2b_com_pro modelcompro = new B2bComProData().GetProById(morder.Pro_id.ToString());
                if (modelcompro == null)
                {
                    throw new WxPayException("原生支付产品查询失败!");
                }

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
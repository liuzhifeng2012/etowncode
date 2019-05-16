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

namespace ETS2.WebApp.wxpay
{
    public partial class backpaynotice : System.Web.UI.Page
    {
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
                        };
                        Log.Info(this.GetType().ToString(), "支付结果通知页面收到的comid:" + comid);
                        ProcessNotify(config, model);
                        //ResultNotify resultNotify = new ResultNotify(this);
                        //resultNotify.ProcessNotify(config);
                    }
                    catch(Exception ex)
                    {
                        Log.Error(this.GetType().ToString(), "微信支付通知意外错误("+ex.Message+")");
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
            WxPayData notifyData = new Notify(this).GetNotifyData(config);

            Log.Info(this.GetType().ToString(), "The Pay ResultXml:" + notifyData.ToXml(config));

            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                Log.Error(this.GetType().ToString(), "The Pay result is error : " + res.ToXml(config), config);
                Response.Write(res.ToXml(config));
                Response.End();
            }

            string transaction_id = notifyData.GetValue("transaction_id").ToString();
            string notify_id = "1";
            string out_trade_no = notifyData.GetValue("out_trade_no").ToString();
            string total_fee = notifyData.GetValue("total_fee").ToString();

            //查询订单，判断订单真实性
            if (!QueryOrder(transaction_id, config))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                Log.Error(this.GetType().ToString(), "Order query failure : " + res.ToXml(config), config);
                Response.Write(res.ToXml(config));
                Response.End();
            }
            //查询订单成功
            else
            {
                //订单支付信息如果没有，则添加支付信息
                B2b_pay modelb2pay = new B2bPayData().GetPayByoId(out_trade_no.ConvertTo<int>(0));
                B2b_order modelb2border = new B2bOrderData().GetOrderById(out_trade_no.ConvertTo<int>(0));
                string Pay_name = "";
                string Pay_phone = "";
                if (modelb2border != null)
                {
                    Pay_name = modelb2border.U_name;
                    Pay_phone = modelb2border.U_phone;
                }

                #region 订单提交支付信息

                if (modelb2pay == null)
                {
                    B2b_pay eticket = new B2b_pay()
                    {
                        Id = 0,
                        Oid = out_trade_no.ConvertTo<int>(0),
                        Pay_com = "wx",
                        Pay_name = Pay_name,
                        Pay_phone = Pay_phone,
                        Total_fee = decimal.Parse(total_fee) / 100,
                        Trade_no = "",
                        Trade_status = "trade_pendpay",
                        Uip = "",
                        comid = model.Com_id
                    };
                    int payid = new B2bPayData().InsertOrUpdate(eticket);
                }
                else
                {
                    //对已完成支付的，再次提交支付，跳转到订单也或显示此订单已支付
                    if (modelb2pay.Trade_status != "TRADE_SUCCESS")
                    {
                        //防止金额有所改动
                        modelb2pay.comid = model.Com_id;
                        modelb2pay.Pay_com = "wx";
                        modelb2pay.Total_fee = decimal.Parse(total_fee) / 100;
                        new B2bPayData().InsertOrUpdate(modelb2pay);
                    }
                }
                #endregion
                //------------------------------
                //处理业务开始
                //------------------------------
                string retunstr = new PayReturnSendEticketData().PayReturnSendEticket(notify_id, int.Parse(out_trade_no), decimal.Parse(total_fee) / 100, "TRADE_SUCCESS", transaction_id);

                //对分销充值订单支付返回，对绑定订单进行处理确认
                var handdata = OrderJsonData.agentorderpay_Hand(int.Parse(out_trade_no));

                // 赠送保险
                OrderJsonData.ZengsongBaoxian(int.Parse(out_trade_no));

                //给财付通系统发送成功信息，财付通系统收到此结果后不再进行后续通知
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                Log.Info(this.GetType().ToString(), "order query success : " + res.ToXml(config), config);
                Response.Write(res.ToXml(config));
                Response.End();
            }
        }

        //查询订单
        private bool QueryOrder(string transaction_id, WxPayConfig config)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req, config);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                //支付成功
                if (res.GetValue("trade_state").ToString() == "SUCCESS")
                {
                    return true;
                }
                //支付失败
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Com.Tenpay.TenpayAppV3;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using ETS.Framework;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.VAS.Service.VASService.Data;
using ETS2.Common.Business;

namespace ETS2.WebApp.wxpay
{
    /// <summary>
    /// 根据Code获取OpenID等信息
    /// </summary>
    public class ModelOpenID
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }
    }
    public partial class payment : System.Web.UI.Page
    {
        public static string mchid = ""; //mchid
        public static string appId = ""; //appid
        public static string appsecret = ""; //appsecret
        public static string appkey = ""; //paysignkey(非appkey 在微信商户平台设置 (md5)111111111111) 
        public static string timeStamp = ""; //时间戳 
        public static string nonceStr = ""; //随机字符串 

        public static string code = "";     //微信端传来的code
        public static string prepayId = "";     //预支付ID
        public static string sign = "";     //为了获取预支付ID的签名
        public static string paySign = "";  //进行支付需要的签名
        public static string package = "";  //进行支付需要的包


        //public static string RequestUrl = "";//域名
        public string proname = "";
        public string u_name = "";
        public string u_mobile = "";
        public string travel_date = "";
        public int buy_num = 0;
        public string u_youxiaoqi = "";
        public decimal p_totalprice1 = 0;//订单实际金额
        public string p_totalprice = "";//订单实际金额,精确到两位小数
        public string wxp_totalprice = "";//微信支付用到的金额,以分为单位(money * 100).ToString()
        public int orderid = 0;
        public string price = "";

        public string comName = "";
        public int comid = 0;

        public string phone = "";

        public string orderstatus = "等待对方付款";//是否可以支付
        public int paystatus = 0;//支付状态

        public int servertype = 1;//服务类型(1.电子凭证2.跟团游8.当地游9.酒店客房)
        public string pricedetail = "";//价格详情(暂时只有用来描述旅游)
        public int cart = 0;//是否是购物车产品  大于0为购物车
        public int order_type = 1;//订单类型:1正常订单;2充值订单
        public string Returnmd5 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            bool bo = detectmobilebrowser.HttpUserAgent(u);

            if (Request.QueryString["code"] != null)
            {
                orderid = Request["orderid"].ConvertTo<int>(0);

                Returnmd5 = EncryptionHelper.ToMD5(orderid.ToString() + "lixh1210", "UTF-8");


                if (orderid != 0)
                {
                    //根据订单id得到订单信息
                    B2bOrderData dataorder = new B2bOrderData();
                    B2b_order modelb2border = dataorder.GetOrderById(orderid);

                    orderstatus = EnumUtils.GetName((OrderStatus)modelb2border.Order_state);
                    paystatus = modelb2border.Pay_state;//1未支付；2已支付 
                    order_type = modelb2border.Order_type;

                    //根据产品id得到产品信息
                    B2bComProData datapro = new B2bComProData();
                    B2b_com_pro modelcompro = datapro.GetProById(modelb2border.Pro_id.ToString(), modelb2border.Speciid, modelb2border.channelcoachid);

                    if (modelcompro != null)
                    {
                        servertype = modelcompro.Server_type;
                    }

                    cart = modelb2border.Shopcartid;//购物车

                    #region 正常订单
                    if (modelb2border.Order_type == 1)
                    {

                        if (modelcompro != null)
                        {
                            #region 不同服务类型订单金额
                            p_totalprice1 = new B2bOrderData().GetOrderTotalPrice(modelcompro, modelb2border, out price, out pricedetail);
                            if (p_totalprice1 == 0 || price == "0")
                            {
                                Response.Write("<script>alert('订单金额不可为0');</script>");
                                Response.End();
                                return;
                            }
                            #endregion

                            proname = modelcompro.Pro_name;
                            u_name = modelb2border.U_name.Substring(0, 1) + "**";
                            u_mobile = modelb2border.U_phone.Substring(0, 4) + "****" + modelb2border.U_phone.Substring(modelb2border.U_phone.Length - 3, 3);
                            travel_date = modelb2border.U_traveldate.ToString();
                            buy_num = modelb2border.U_num;
                            u_youxiaoqi = modelcompro.Pro_start.ToString() + " - " + modelcompro.Pro_end.ToString();

                            p_totalprice = CommonFunc.OperTwoDecimal(p_totalprice1.ToString());
                            wxp_totalprice = (p_totalprice1 * 100).ToString("F0");
                            price = modelb2border.Pay_price.ToString();

                            price = CommonFunc.OperTwoDecimal(price);
                            comid = modelcompro.Com_id;

                            var saleset = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
                            if (saleset != null)
                            {
                                phone = saleset.Service_Phone;
                            }
                        }
                    }
                    #endregion

                    #region 充值订单
                    if (modelb2border.Order_type == 2)
                    {
                        //ordertype=2 充值订单会传递过来值
                        comid = modelb2border.Comid;

                        p_totalprice1 = modelb2border.U_num * modelb2border.Pay_price;
                        p_totalprice = CommonFunc.OperTwoDecimal(p_totalprice1.ToString());
                        wxp_totalprice = (p_totalprice1 * 100).ToString("F0");

                        //获得商户电话
                        var saleset = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
                        if (saleset != null)
                        {
                            phone = saleset.Service_Phone;
                        }
                    }
                    #endregion

                    #region 统一获得商户名称 和 微信支付链接
                    if (comid != 0)
                    {
                        comName = B2bCompanyData.GetCompany(comid).Com_name;
                    }
                    #endregion


                    #region 写入支付数据库,先判定是否有此订单支付
                    B2bPayData datapay = new B2bPayData();
                    B2b_pay modelb2pay = datapay.GetPayByoId(orderid);

                    if (modelb2pay != null)
                    {
                        //对已完成支付的，再次提交支付，跳转到订单也或显示此订单已支付
                        if (modelb2pay.Trade_status == "TRADE_SUCCESS")
                        {
                            Response.Write("订单已经支付过！");
                            Response.End();
                            return;
                        }
                    }
                    #endregion

                    #region  微信支付
                    //根据产品判断商家是否含有自己的微信支付:a.含有的话支付到商家；b.没有的话支付到平台的微信公众号账户中
                    B2b_finance_paytype model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(comid);

                    if (model != null)
                    {
                        //商家微信支付的所有参数都存在
                        if (model.Wx_appid != "" && model.Wx_appkey != "" && model.Wx_partnerid != "" && model.Wx_paysignkey != "")
                        {
                            appId = model.Wx_appid;
                            appsecret = model.Wx_appkey;
                            appkey = model.Wx_paysignkey;
                            mchid = model.Wx_partnerid;
                        }
                        else
                        {
                            model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(106);
                            if (model.Wx_appid != "" && model.Wx_appkey != "" && model.Wx_partnerid != "" && model.Wx_paysignkey != "")
                            {
                                appId = model.Wx_appid;
                                appsecret = model.Wx_appkey;
                                appkey = model.Wx_paysignkey;
                                mchid = model.Wx_partnerid;
                            }
                        }
                    }
                    else
                    {
                        model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(106);
                        if (model.Wx_appid != "" && model.Wx_appkey != "" && model.Wx_partnerid != "" && model.Wx_paysignkey != "")
                        {
                            appId = model.Wx_appid;
                            appsecret = model.Wx_appkey;
                            appkey = model.Wx_paysignkey;
                            mchid = model.Wx_partnerid;
                        }
                    }



                    #region 获取用户openid
                    code = Request.QueryString["code"];
                    string url =
                        string.Format(
                            "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",
                            appId, appsecret, code);
                    string returnStr = HttpUtil.Send("", url);

                    var obj = JsonConvert.DeserializeObject<ModelOpenID>(returnStr);
                    if (obj.openid == null)
                    {
                        //WriteFile(Server.MapPath("") + "\\Log.txt", "code:" + code + "\r\n------------------------------\r\n returnStr:" + returnStr);

                        url = string.Format(
                            "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}",
                            appId, obj.refresh_token);
                        returnStr = HttpUtil.Send("", url);
                        obj = JsonConvert.DeserializeObject<ModelOpenID>(returnStr);
                    }


                    //WriteFile(Server.MapPath("") + "\\Log.txt", "access_token:" + obj.access_token + "\r\n" + "--openid:" + obj.openid);


                    //url = string.Format(
                    //    "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}",
                    //    obj.access_token, obj.openid);
                    //returnStr = HttpUtil.Send("", url);
                    //WriteFile(Server.MapPath("") + "\\Log.txt", returnStr);
                    #endregion


                    //创建支付应答对象
                    var packageReqHandler = new RequestHandler(Context);
                    //初始化
                    packageReqHandler.init();

                    timeStamp = TenpayUtil.getTimestamp();
                    nonceStr = TenpayUtil.getNoncestr();


                    //设置package订单参数
                    string productname = "";
                    if (modelb2border.Order_type == 1)
                    {
                        productname = modelcompro.Pro_name.Replace("\"", "").Replace("“", "").Replace("'", "").Replace("‘", "").Replace(";", "").Replace("；", "");

                        if (productname.Length > 50)
                        {
                            productname = productname.Substring(0, 50);
                        }
                    }
                    if (modelb2border.Order_type == 2)
                    {
                        productname = "预付款充值";
                    }
                    packageReqHandler.setParameter("body", productname); //商品信息 127字符
                    packageReqHandler.setParameter("appid", appId);
                    packageReqHandler.setParameter("mch_id", mchid);
                    packageReqHandler.setParameter("nonce_str", nonceStr.ToLower());
                    packageReqHandler.setParameter("notify_url", "http://shop" + comid + ".etown.cn/wxpay/backpaynotice.aspx");
                    packageReqHandler.setParameter("openid", obj.openid);
                    packageReqHandler.setParameter("out_trade_no", orderid.ToString()); //商家订单号
                    packageReqHandler.setParameter("spbill_create_ip", Page.Request.UserHostAddress); //用户的公网ip，不是商户服务器IP
                    packageReqHandler.setParameter("total_fee", wxp_totalprice); //商品金额,以分为单位(money * 100).ToString()
                    packageReqHandler.setParameter("trade_type", "JSAPI");

                    //获取package包
                    sign = packageReqHandler.CreateMd5Sign("key", appkey);
                    //WriteFile(Server.MapPath("") + "\\Log.txt", sign);
                    packageReqHandler.setParameter("sign", sign);

                    string data = packageReqHandler.parseXML();
                    
                    //WriteFile(Server.MapPath("") + "\\Log.txt", "package包签名：" + sign + "-----\r\n向统一支付接口发送的xml：" + data);

                    string prepayXml = HttpUtil.Send(data, "https://api.mch.weixin.qq.com/pay/unifiedorder");

                    //WriteFile(Server.MapPath("") + "\\Log.txt", "统一支付接口返回xml:" + prepayXml);

                    //获取预支付ID
                    var xdoc = new XmlDocument();
                    xdoc.LoadXml(prepayXml);
                    XmlNode xn = xdoc.SelectSingleNode("xml");
                    XmlNodeList xnl = xn.ChildNodes;
                    if (xnl.Count > 7)
                    {
                        prepayId = xnl[7].InnerText;
                        package = string.Format("prepay_id={0}", prepayId);
                        //WriteFile(Server.MapPath("") + "\\Log.txt", "预支付id:" + package);
                    }

                    //设置支付参数
                    var paySignReqHandler = new RequestHandler(Context);
                    paySignReqHandler.setParameter("appId", appId);
                    paySignReqHandler.setParameter("timeStamp", timeStamp);
                    paySignReqHandler.setParameter("nonceStr", nonceStr);
                    paySignReqHandler.setParameter("package", package);
                    paySignReqHandler.setParameter("signType", "MD5");
                    paySign = paySignReqHandler.CreateMd5Sign("key", appkey);


                    //WriteFile(Server.MapPath("") + "\\Log.txt", "支付签名" + paySign);
                    #endregion

                    #region 订单提交支付信息
                    if (modelb2pay == null)
                    {
                        B2b_pay eticket = new B2b_pay()
                        {
                            Id = 0,
                            Oid = orderid,
                            Pay_com = "wx",
                            Pay_name = modelb2border.U_name,
                            Pay_phone = modelb2border.U_phone,
                            Total_fee = p_totalprice1,
                            Trade_no = "",
                            Trade_status = "trade_pendpay",
                            Uip = "",
                            comid = model.Com_id
                        };
                        int payid = datapay.InsertOrUpdate(eticket);
                    }
                    else
                    {
                        //对已完成支付的，再次提交支付，跳转到订单也或显示此订单已支付
                        if (modelb2pay.Trade_status != "TRADE_SUCCESS")
                        {
                            //防止金额有所改动
                            modelb2pay.comid = model.Com_id;
                            modelb2pay.Pay_com = "wx";
                            modelb2pay.Total_fee = p_totalprice1;
                            datapay.InsertOrUpdate(modelb2pay);
                        }
                    }
                    #endregion
                }

            }
        }


    }
}

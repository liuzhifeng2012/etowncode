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

namespace ETS2.WebApp.wxpaytest
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

        public string order_status = "等待对方付款";//是否可以支付

        public int servertype = 1;//服务类型(1.电子凭证2.跟团游8.当地游9.酒店客房)
        public string pricedetail = "";//价格详情(暂时只有用来描述旅游)
        protected void Page_Load(object sender, EventArgs e)
        {
            //RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            bool bo = detectmobilebrowser.HttpUserAgent(u);

            if (Request.QueryString["code"] != null)
            {
                #region 获取用户openid
                code = Request.QueryString["code"];
                string url =
                    string.Format(
                        "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",
                        appId, appsecret, code);
                string returnStr = HttpUtil.Send("", url);

                var obj = JsonConvert.DeserializeObject<ModelOpenID>(returnStr);

                url = string.Format(
                    "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}",
                    appId, obj.refresh_token);
                returnStr = HttpUtil.Send("", url);
                obj = JsonConvert.DeserializeObject<ModelOpenID>(returnStr);

                WriteFile(Server.MapPath("") + "\\Log.txt", obj.access_token);
                WriteFile(Server.MapPath("") + "\\Log.txt", obj.openid);

                //url = string.Format(
                //    "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}",
                //    obj.access_token, obj.openid);
                //returnStr = HttpUtil.Send("", url);
                //WriteFile(Server.MapPath("") + "\\Log.txt", returnStr);
                #endregion
                ///////////////////////////////////////////////////////////////////////////////////////////////

                orderid = Request["orderid"].ConvertTo<int>(0);
                if (orderid != 0)
                {
                    //根据订单id得到订单信息
                    B2bOrderData dataorder = new B2bOrderData();
                    B2b_order modelb2border = dataorder.GetOrderById(orderid);

                    //根据产品id得到产品信息
                    B2bComProData datapro = new B2bComProData();
                    B2b_com_pro modelcompro = datapro.GetProById(modelb2border.Pro_id.ToString());

                    if (modelb2border != null && modelcompro != null)
                    {
                        #region 不同服务类型订单金额
                        servertype = modelcompro.Server_type;
                        //如果服务类型是“酒店客房”，则根据酒店扩展订单表中房态信息，获取支付金额
                        if (modelcompro.Server_type == 9)
                        {
                            B2b_order_hotel hotelorder = new B2b_order_hotelData().GetHotelOrderByOrderId(orderid);
                            if (hotelorder != null)
                            {
                                string fangtai = hotelorder.Fangtai;
                                DateTime start_data = hotelorder.Start_date;
                                DateTime end_data = hotelorder.End_date;
                                int bookdaynum = hotelorder.Bookdaynum;

                                decimal everyroomprice = 0;
                                string[] ftstr = fangtai.Split(',');
                                for (int i = 0; i < ftstr.Length; i++)
                                {
                                    if (ftstr[i].ConvertTo<decimal>(0) > 0)
                                    {
                                        everyroomprice += ftstr[i].ConvertTo<decimal>(0);
                                    }
                                }
                                price = everyroomprice.ToString();

                                p_totalprice1 = (modelb2border.U_num * everyroomprice - modelb2border.Integral1 - modelb2border.Imprest1);

                            }
                            else { }
                        }
                        else if (servertype == 2 || servertype == 8)//当地游；跟团游
                        {
                            string outdate = modelb2border.U_traveldate.ToString("yyyy-MM-dd");

                            //读取団期价格,根据实际选择的団期报价
                            B2b_com_LineGroupDate linemode = new B2b_com_LineGroupDateData().GetLineDayGroupDate(DateTime.Parse(outdate), modelcompro.Id);
                            if (linemode != null)
                            {
                                price = linemode.Menprice.ToString();
                                decimal childreduce = modelcompro.Childreduce;
                                decimal childprice = decimal.Parse(price) - childreduce;
                                if (childprice < 0)
                                {
                                    childprice = 0;
                                }
                                pricedetail = modelb2border.U_num + "成人," + modelb2border.Child_u_num + "儿童(成人" + price + "元/人，儿童" + childprice + "元/人)";

                                p_totalprice1 = (modelb2border.U_num * (linemode.Menprice) + (modelb2border.Child_u_num) * childprice - modelb2border.Integral1 - modelb2border.Imprest1);
                            }
                        }
                        else //票务
                        {
                            p_totalprice1 = (modelb2border.U_num * modelcompro.Advise_price - modelb2border.Integral1 - modelb2border.Imprest1);
                            price = modelcompro.Advise_price.ToString(); //modelb2border.Pay_price.ToString();
                            if (price == "0.00" || price == "0")
                            {
                                price = "";
                            }
                            else
                            {
                                price = CommonFunc.OperTwoDecimal(price);

                            }
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
                        if (comid != 0)
                        {
                            comName = B2bCompanyData.GetCompany(comid).Com_name;
                            if (bo == false)
                            {
                                Response.Redirect("http://shop" + comid + ".etown.cn");
                            }
                        }

                        if ((int)modelb2border.Order_state != (int)OrderStatus.WaitPay)
                        {
                            order_status = EnumUtils.GetName((OrderStatus)modelb2border.Order_state);
                            return;
                        }

                        var saleset = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
                        if (saleset != null)
                        {
                            phone = saleset.Service_Phone;
                        }

                        //写入支付数据库,先判定是否有此订单支付
                        B2bPayData datapay = new B2bPayData();
                        B2b_pay modelb2pay = datapay.GetPayByoId(orderid);


                        #region  微信支付
                        //根据产品判断商家是否含有自己的微信支付:a.含有的话支付到商家；b.没有的话支付到平台的微信公众号账户中
                        B2b_finance_paytype model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(modelcompro.Com_id);

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
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }



                        //创建支付应答对象
                        var packageReqHandler = new RequestHandler(Context);
                        //初始化
                        packageReqHandler.init();

                        timeStamp = TenpayUtil.getTimestamp();
                        nonceStr = TenpayUtil.getNoncestr();


                        //设置package订单参数
                        string productname = modelcompro.Pro_name.Replace("\"", "").Replace("“", "").Replace("'", "").Replace("‘", "").Replace(";", "").Replace("；", "");
                        packageReqHandler.setParameter("body", productname); //商品信息 127字符
                        packageReqHandler.setParameter("appid", appId);
                        packageReqHandler.setParameter("mch_id", mchid);
                        packageReqHandler.setParameter("nonce_str", nonceStr.ToLower());
                        packageReqHandler.setParameter("notify_url", "http://shop" + modelcompro.Com_id + ".etown.cn/wxpay/backpaynotice.aspx");
                        packageReqHandler.setParameter("openid", obj.openid);
                        packageReqHandler.setParameter("out_trade_no", orderid.ToString()); //商家订单号
                        packageReqHandler.setParameter("spbill_create_ip", Page.Request.UserHostAddress); //用户的公网ip，不是商户服务器IP
                        packageReqHandler.setParameter("total_fee", wxp_totalprice); //商品金额,以分为单位(money * 100).ToString()
                        packageReqHandler.setParameter("trade_type", "JSAPI");

                        //获取package包
                        sign = packageReqHandler.CreateMd5Sign("key", appkey);
                        WriteFile(Server.MapPath("") + "\\Log.txt", sign);
                        packageReqHandler.setParameter("sign", sign);

                        string data = packageReqHandler.parseXML();

                        WriteFile(Server.MapPath("") + "\\Log.txt", data);

                        string prepayXml = HttpUtil.Send(data, "https://api.mch.weixin.qq.com/pay/unifiedorder");

                        WriteFile(Server.MapPath("") + "\\Log.txt", prepayXml);

                        //获取预支付ID
                        var xdoc = new XmlDocument();
                        xdoc.LoadXml(prepayXml);
                        XmlNode xn = xdoc.SelectSingleNode("xml");
                        XmlNodeList xnl = xn.ChildNodes;
                        if (xnl.Count > 7)
                        {
                            prepayId = xnl[7].InnerText;
                            package = string.Format("prepay_id={0}", prepayId);
                            WriteFile(Server.MapPath("") + "\\Log.txt", package);
                        }

                        //设置支付参数
                        var paySignReqHandler = new RequestHandler(Context);
                        paySignReqHandler.setParameter("appId", appId);
                        paySignReqHandler.setParameter("timeStamp", timeStamp);
                        paySignReqHandler.setParameter("nonceStr", nonceStr);
                        paySignReqHandler.setParameter("package", package);
                        paySignReqHandler.setParameter("signType", "MD5");
                        paySign = paySignReqHandler.CreateMd5Sign("key", appkey);


                        WriteFile(Server.MapPath("") + "\\Log.txt", paySign);
                        #endregion

                        //订单提交支付信息
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
                                Uip = ""
                            };
                            int payid = datapay.InsertOrUpdate(eticket);
                        }
                        else
                        {
                            //对已完成支付的，再次提交支付，跳转到订单也或显示此订单已支付
                            if (modelb2pay.Trade_status != "TRADE_SUCCESS")
                            {
                                //防止金额有所改动
                                modelb2pay.Pay_com = "wx";
                                modelb2pay.Total_fee = p_totalprice1;
                                datapay.InsertOrUpdate(modelb2pay);
                            }
                        }

                    }
                }

            }
        }

        public static void WriteFile(string pathWrite, string content)
        {
            if (File.Exists(pathWrite))
            {
                //File.Delete(pathWrite);
            }
            File.AppendAllText(pathWrite, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + content + "\r\n----------------------------------------\r\n",
                Encoding.GetEncoding("utf-8"));
        }
    }
}
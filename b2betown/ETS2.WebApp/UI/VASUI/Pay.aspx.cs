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
using ETS.JsonFactory;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.VAS.Service.VASService.Data;
using WxPayAPI;
using Com.Tenpay.TenpayAppV3;
using System.Threading;
using System.Xml;
using Newtonsoft.Json;

namespace ETS2.WebApp.UI.VASUI
{
    public partial class Pay : System.Web.UI.Page
    {


        public string proname = "";
        public string u_name = "";
        public string u_mobile = "";
        public string travel_date = "";
        public int buy_num = 0;
        public string u_youxiaoqi = "";
        public string p_totalprice = "0";
        public int orderid = 0;
        public int comid = 0;

        public int ordertype = 0;
        public int Server_type = 0;
        public string stardate = "";
        public string enddate = "";
        public string tenpay_url = "";//财付通支付链接
        public int cart = 0;//是否为购物车订单
        public int viewtop = 1; //头部及左侧相关显示控制

        public string nativePayImgurl = "/Images/defaultThumb.png";//微信原生支付二维码 
        protected void Page_Load(object sender, EventArgs e)
        {
            //返回订单号
            orderid = Request["orderid"].ConvertTo<int>(0);
            #region 正常订单支付操作
            if (orderid != 0)
            {
                string u = Request.ServerVariables["HTTP_USER_AGENT"];
                bool bo = detectmobilebrowser.HttpUserAgent(u);
                if (bo)
                {
                    Response.Redirect("/h5/pay.aspx?orderid=" + orderid);
                }
                //根据订单id得到订单信息
                B2bOrderData dataorder = new B2bOrderData();
                B2b_order modelb2border = dataorder.GetOrderById(orderid);

                cart = modelb2border.Shopcartid;//不等于0则为购物车订单

                //根据产品id得到产品信息
                B2bComProData datapro = new B2bComProData();
                B2b_com_pro modelcompro = datapro.GetProById(modelb2border.Pro_id.ToString(), modelb2border.Speciid);

                if (modelcompro != null)
                {
                    Server_type = modelcompro.Server_type;
                    comid = modelcompro.Com_id;
                    //绿野 不显示头部
                    if (modelcompro.Com_id == 2553)
                    {
                        viewtop = 0;
                    }
                    string urljson = WeiXinJsonData.getNativePayQrcode(orderid, comid, "oid");
                    try
                    {
                        XmlDocument retdoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + urljson + "}");
                        XmlElement retroot = retdoc.DocumentElement;
                        string type = retroot.SelectSingleNode("type").InnerText;
                        string msg = retroot.SelectSingleNode("msg").InnerText;

                        nativePayImgurl = "/ui/pmui/eticket/showtcode.aspx?pno=" + msg;
                    }
                    catch 
                    { }
                }
                else
                {
                    //产品错误
                }

                //如果订单“未付款”现实支付及订单信息
                if ((int)modelb2border.Order_state == (int)OrderStatus.WaitPay)
                {

                    u_name = modelb2border.U_name.Substring(0, 1) + "**";
                    u_mobile = modelb2border.U_phone.Substring(0, 4) + "****" + modelb2border.U_phone.Substring(modelb2border.U_phone.Length - 3, 3);

                    ;
                    travel_date = modelb2border.U_traveldate.ToString();
                    buy_num = modelb2border.U_num;
                    p_totalprice = CommonFunc.OperTwoDecimal((modelb2border.U_num * modelb2border.Pay_price + modelb2border.Express - modelb2border.Integral1 - modelb2border.Imprest1).ToString());
                    if (modelb2border.Child_u_num > 0)//如果是旅游包含儿童的
                    {
                        p_totalprice = (Decimal.Parse(p_totalprice) + modelb2border.Child_u_num * (modelb2border.Pay_price - modelcompro.Childreduce)).ToString();
                    }
                    
                    ordertype = modelb2border.Order_type;
                    if (ordertype == 2)
                    {
                        proname = "预付款充值";
                        u_youxiaoqi = "";
                    }
                    else
                    {
                        proname = modelcompro.Pro_name;
                        if (modelcompro.Server_type == 10)
                        {//服务类型是：旅游大巴
                            u_youxiaoqi = modelb2border.U_traveldate.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            u_youxiaoqi = modelcompro.Pro_start.ToString() + " - " + modelcompro.Pro_end.ToString();
                        }
                    }


                    //如果是购物车订单再次处理
                    if (cart > 0)
                    {

                        proname = dataorder.GetCartOrderProById(orderid);
                        p_totalprice = dataorder.GetCartOrderMoneyById(orderid).ToString("0.00");
                        buy_num = 1;
                    }


                    tenpay_url = string.Format("/tenpay/payRequest.aspx?order_no={0}&product_name={1}&order_price={2}&remarkexplain={3}",
                        orderid, Server.UrlEncode(proname), Server.UrlEncode(p_totalprice), Server.UrlEncode(proname));

                    if (Server_type == 9)
                    {
                        //订房查询入住 及离店日期
                        var hoteldata = new B2b_order_hotelData();
                        var hotelmodel = hoteldata.GetHotelOrderByOrderId(orderid);

                        if (hotelmodel != null)
                        {
                            stardate = hotelmodel.Start_date.ToString("yyyy-MM-dd");
                            enddate = hotelmodel.End_date.ToString("yyyy-MM-dd");
                        }

                    }


                }

            }
            #endregion

        }
    }
}
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
using ETS2.CRM.Service.CRMService.Data;
using ETS.JsonFactory;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;


namespace ETS2.WebApp.H5
{
    public partial class pay : System.Web.UI.Page
    {
        public string proname = "";
        public string u_name = "";
        public string u_mobile = "";
        public string travel_date = "";
        public int buy_num = 0;
        public string pricedetail = "";

        public string u_youxiaoqi = "";
        public decimal p_totalprice = 0;
        public string p_totalpricedesc = "0";
        public int orderid = 0;
        public int agentorderid = 0;
        public string price = "";

        public string comName = "";

        public int comid = 0;
        public string phone = "";

        public int servertype = 1;//服务类型(1.电子凭证2.跟团游8.当地游9.酒店客房)

        public string ticketinfo = "";//线路备注
        public int travel_proid = 0;//旅游产品编号

        public int paystatus = 0;//支付状态
        public string orderstatus = "";//订单状态
        public string subtime = "";

        public string wxpaylinkurl = "";//微信支付链接url

        public int order_type = 1;//订单类型:1正常订单;2充值订单
        public int order_state = 0;
        public int Ispanicbuy = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            bool bo = detectmobilebrowser.HttpUserAgent(u);

            //返回订单号
            orderid = Request["orderid"].ConvertTo<int>(0);
            agentorderid = Request["agentorderid"].ConvertTo<int>(0);
            if (orderid != 0)
            {


                




                //根据订单id得到订单信息
                B2bOrderData dataorder = new B2bOrderData();
                B2b_order modelb2border = dataorder.GetOrderById(orderid);

                if (modelb2border != null) {

                    if (!bo)
                    {
                        Response.Redirect("/ui/vasui/pay.aspx?orderid=" + orderid + "&comid=" + modelb2border.Comid);

                    }
                }


                //取消超时订单
                B2b_com_pro pro_cannelorder = new B2b_com_pro();
                pro_cannelorder.Server_type = 0;
                int rs_cannelorder = new B2bComProData().CancelOvertimeOrder(pro_cannelorder);



                orderstatus = EnumUtils.GetName((OrderStatus)modelb2border.Order_state);

                order_state = modelb2border.Order_state;
                paystatus = modelb2border.Pay_state;//1未支付；2已支付 
                order_type = modelb2border.Order_type;
                subtime = modelb2border.U_subdate.ToString("yyyy/MM/dd hh:mm:ss");
                comid = modelb2border.Comid;

                #region 正常订单
                if (modelb2border.Order_type == 1)
                {
                    //根据产品id得到产品信息
                    B2bComProData datapro = new B2bComProData();
                    B2b_com_pro modelcompro = datapro.GetProById(modelb2border.Pro_id.ToString(),modelb2border.Speciid);

                    ////如果订单“未付款”显示支付及订单信息
                    //if ((int)modelb2border.Order_state == (int)OrderStatus.WaitPay)
                    //{ 
                    if (modelb2border.U_name != "")
                    {
                        u_name = modelb2border.U_name.Substring(0, 1) + "**";
                    }
                    else {
                        u_name = "**";
                    }

                    if (modelb2border.U_phone != "")
                    {
                        u_mobile = modelb2border.U_phone.Substring(0, 4) + "****" + modelb2border.U_phone.Substring(modelb2border.U_phone.Length - 3, 3);
                    }
                    else {
                        u_mobile = "****";
                    }


                    travel_date = modelb2border.U_traveldate.ToString();
                    buy_num = modelb2border.U_num;

                    if (modelcompro != null)
                    {
                        u_youxiaoqi = modelcompro.Pro_start.ToString() + " - " + modelcompro.Pro_end.ToString();
                        travel_proid = modelcompro.Travelproductid;
                        comid = modelcompro.Com_id;
                        proname = modelcompro.Pro_name;
                        servertype = modelcompro.Server_type;
                        Ispanicbuy = modelcompro.Ispanicbuy;
                    }

                    #region 如果服务类型是“酒店客房”，则根据酒店扩展订单表中房态信息，获取支付金额
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

                            p_totalprice = (modelb2border.U_num * everyroomprice - modelb2border.Integral1 - modelb2border.Imprest1);

                        }
                    }
                    #endregion
                    #region 当地游；跟团游；旅游大巴，获取支付金额
                    else if (servertype == 2 || servertype == 8 )//当地游；跟团游；旅游大巴
                    {
                        string outdate = modelb2border.U_traveldate.ToString("yyyy-MM-dd");

                        //读取団期价格,根据实际选择的団期报价
                        B2b_com_LineGroupDate linemode = new B2b_com_LineGroupDateData().GetLineDayGroupDate(DateTime.Parse(outdate), modelcompro.Id);
                        if (linemode != null)//当地游；跟团游
                        {
                            price = linemode.Menprice.ToString();
                            if (servertype == 2 || servertype == 8)
                            {
                                decimal childreduce = modelcompro.Childreduce;
                                decimal childprice = decimal.Parse(price) - childreduce;
                                if (childprice < 0)
                                {
                                    childprice = 0;
                                }
                                pricedetail = modelb2border.U_num + "成人," + modelb2border.Child_u_num + "儿童(成人" + price + "元/人，儿童" + childprice + "元/人)";

                                p_totalprice = (modelb2border.U_num * (linemode.Menprice) + (modelb2border.Child_u_num) * childprice - modelb2border.Integral1 - modelb2border.Imprest1);

                            }
                            else //旅游大巴:没有儿童减免
                            {

                                pricedetail = modelb2border.U_num + "人(" + price + "元/人)";

                                p_totalprice = (modelb2border.U_num * (linemode.Menprice) - modelb2border.Integral1 - modelb2border.Imprest1);
                            }
                        }
                    }
                    else if (servertype == 10) {

                        pricedetail = modelb2border.U_num + "人(" + modelb2border.Pay_price.ToString("0.00") + "元/人)";

                        p_totalprice = (modelb2border.U_num * (modelb2border.Pay_price) - modelb2border.Integral1 - modelb2border.Imprest1);
                    }




                    #endregion
                    #region 票务、实物，获取支付金额
                    else //票务
                    {
                        p_totalprice = (modelb2border.U_num * modelb2border.Pay_price - modelb2border.Integral1 - modelb2border.Imprest1);
                        price = modelb2border.Pay_price.ToString(); //modelb2border.Pay_price.ToString();
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
                    //}

                    #region  获得服务电话
                    var saleset = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
                    if (saleset != null)
                    {
                        phone = saleset.Service_Phone;
                    }

                    if (modelcompro != null)
                    {
                        //查询项目电话,如果有项目电话调取项目电话
                        var projectdata = new B2b_com_projectData();
                        var projectmodel = projectdata.GetProject(modelcompro.Projectid, comid);
                        if (projectmodel != null)
                        {
                            if (projectmodel.Mobile != "")
                            {
                                phone = projectmodel.Mobile;
                            }
                        }
                    }
                    #endregion

                }
                #endregion
                #region 充值订单
                if (modelb2border.Order_type == 2)
                {

                    if (modelb2border.serverid != "")
                    {
                        proname = "购买服务与押金";
                    }
                    else if (modelb2border.payorder==1)
                    {
                        proname = "快速支付";
                    }

                    else {
                        proname = "预付款充值";
                    }

                    //ordertype=2 充值订单会传递过来值
                    if (comid == 0)
                    {
                        comid = Request["comid"].ConvertTo<int>(0);
                    }
                    p_totalprice = (modelb2border.U_num * modelb2border.Pay_price) ;
                    //获得商户电话
                    var saleset = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
                    if (saleset != null)
                    {
                        phone = saleset.Service_Phone;
                    }
                }
                #endregion
                p_totalpricedesc = CommonFunc.OperTwoDecimal(p_totalprice.ToString());
               

                #region 统一获得商户名称 和 微信支付链接
                if (comid != 0)
                {
                    wxpaylinkurl = WeiXinJsonData.GetFollowOpenLinkUrlAboutPay(comid, "http://shop" + comid + ".etown.cn/wxpay/payment_" + orderid + "_1.aspx");

                    comName = B2bCompanyData.GetCompany(comid).Com_name;
                }
                #endregion
            }
        }


    }
}
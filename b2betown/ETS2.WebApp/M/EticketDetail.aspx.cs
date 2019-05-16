using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.WebApp.M
{
    public partial class EticketDetail : System.Web.UI.Page
    {
        public int orderid = 0;//订单id
        public string openid = "";//微信号

        public string pno = "0";//电子码辅助码

        public int sourcetype = 2;//电子码码来源：1，本系统生成；2，倒码过来的  (根据电子票号得到产品信息，主要是判断来源：1，本系统生成的电子码，可以查验票日志；2，倒码过来的，验票日志在别的系统上，不可查验证日志)
        public int servertype = 0;//产品类型 1=票务,11=实物，10=大巴，2=跟团游，8=当地游，9=酒店
        public int AccountId=0;

        protected void Page_Load(object sender, EventArgs e)
        {
            orderid = Request["orderid"].ConvertTo<int>(0);
            //从cookie中得到微信号
            if (Request.Cookies["openid"] != null)
            {
                openid = Request.Cookies["openid"].Value;
            }

            if (Session["AccountId"] != null)
            {
                //先判断Session
                AccountId = int.Parse(Request.Cookies["AccountId"].Value);
            }

            if (orderid != 0)
            {
                B2b_order order = new B2bOrderData().GetOrderById(orderid);
                if (order != null)
                { //查询订单首先判断是否为指定用户的否则直接跳出
                    if (AccountId == 0 || order.U_id != AccountId)
                    {
                        Response.Redirect("indexcard.aspx");//订购账户不匹配 跳出
                    }
                }
                else {

                    Response.Redirect("indexcard.aspx");//没有查询到订单 跳出
                }





                //根据订单信息得到产品详情
                B2b_com_pro proo = new B2bComProData().GetProByOrderID(orderid);
                if (proo != null)
                {
                    sourcetype = proo.Source_type;
                    servertype = proo.Server_type;
                    
                    if(order !=null)
                    {
                        if (sourcetype == 2 || sourcetype == 1)//倒码或系统生成码
                        {
                            pno = order.Pno;
                        }

                        if (sourcetype == 4) { //分销导入产品
                            //读取原产品
                            B2b_order oldorder = new B2bOrderData().GetOrderById(order.Bindingagentorderid);
                            if (oldorder != null) {
                                pno = oldorder.Pno;
                            }
                        }
                    }
                }
            }
        }
    }
}
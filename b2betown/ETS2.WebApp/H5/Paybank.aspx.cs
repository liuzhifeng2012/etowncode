using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.Common.Business;

namespace ETS2.WebApp.H5
{
    public partial class Paybank : System.Web.UI.Page
    {
        public int orderid = 0;
        public int bank = 0;
        public int comid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            orderid = Request["id"].ConvertTo<int>(0);
            bank = Request["bank"].ConvertTo<int>(0);
            if (orderid != 0)
            {
                string u = Request.ServerVariables["HTTP_USER_AGENT"];
                bool bo = detectmobilebrowser.HttpUserAgent(u);
                //根据订单id得到订单信息
                B2bOrderData dataorder = new B2bOrderData();
                B2b_order modelb2border = dataorder.GetOrderById(orderid);
                if (modelb2border != null) {
                    comid = modelb2border.Comid;
                    //if (bo == false)
                    //{
                    //    if (comid == 101)
                    //    {
                    //        Response.Redirect("http://vctrip.etown.cn/");
                    //    }
                    //    Response.Redirect("http://shop" + comid + ".etown.cn");
                    //}
                }
            }
        }
    }
}
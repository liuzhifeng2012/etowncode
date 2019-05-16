using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.VAS.Service.VASService.Data;

namespace ETS2.WebApp.H5
{
    public partial class backUrl : System.Web.UI.Page
    {
        public int orderid = 0;
        public int comid = 0;
        public string phone = "";
        public string title = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            orderid = Request["orderid"].ConvertTo<int>(0);
            string dikou = Request["dikou"].ConvertTo<string>("");
            if (dikou == "OK")
            {
                title = "订单支付 成功！";
            }
            else {
                title = dikou;
            }
            if (orderid != 0)
            {
                B2b_order orderdate = new B2bOrderData().GetOrderById(orderid);
                if (orderdate != null)
                {
                    var saleset = B2bCompanySaleSetData.GetDirectSellByComid(orderdate.Comid.ToString());
                    if (saleset != null)
                    {
                        phone = saleset.Service_Phone;
                    }
                }

            }
        }
    }
}
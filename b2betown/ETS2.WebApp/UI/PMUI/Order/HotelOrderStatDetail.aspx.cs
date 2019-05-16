using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.PMUI.Order
{
    public partial class HotelOrderStatDetail : System.Web.UI.Page
    {
        public int comid = 0;
        public string begindate = "";
        public string enddate = ""; 
        public int productid = 0; 
        public string orderstate = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comid = Request["comid"].ConvertTo<int>(0);
            begindate = Request["begindate"].ConvertTo<string>("");
            enddate = Request["enddate"].ConvertTo<string>(""); 
            productid = Request["productid"].ConvertTo<int>(0); 
            orderstate = Request["orderstate"].ConvertTo<string>("");
        }
    }
}
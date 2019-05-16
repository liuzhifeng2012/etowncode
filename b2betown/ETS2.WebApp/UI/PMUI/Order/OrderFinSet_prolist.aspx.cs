using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.PMUI.Order
{
    public partial class OrderFinSet_prolist : System.Web.UI.Page
    {
        public string comid = "";
        public string stardate = "";
        public string enddate = "";
        public string submanagename = "";



        protected void Page_Load(object sender, EventArgs e)
        {
            comid = Request["comid"].ConvertTo<string>("0");
            stardate = Request["stardate"].ConvertTo<string>("");
            enddate = Request["enddate"].ConvertTo<string>("");
            submanagename = Request["submanagename"].ConvertTo<string>("");


        }
    }
}
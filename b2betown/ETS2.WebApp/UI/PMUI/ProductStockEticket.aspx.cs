using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.PMUI
{
    public partial class ProductStockEticket : System.Web.UI.Page
    {
        public string proid = "";//产品ID
        public string statetype = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            proid = Request["proid"].ConvertTo<string>("");
            statetype = Request["statetype"].ConvertTo<string>("");

        }
    }
}
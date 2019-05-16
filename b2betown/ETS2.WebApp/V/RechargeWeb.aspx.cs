using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETS2.WebApp.V
{
    public partial class RechargeWeb : System.Web.UI.Page
    {
        public string comeurl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            comeurl = Request.Url.ToString();
        }
    }
}
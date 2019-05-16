using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.Account
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        public string staffid = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            staffid = Request["staffid"].ConvertTo<string>("0");
        }
    }
}

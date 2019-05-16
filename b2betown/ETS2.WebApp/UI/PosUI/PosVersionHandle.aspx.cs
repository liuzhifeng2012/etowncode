using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.PosUI
{
    public partial class PosVersionHandle : System.Web.UI.Page
    {
        public string posid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            posid = Request["posid"].ConvertTo<string>("");
        }
    }
}
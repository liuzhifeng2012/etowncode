using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.PermissionUI
{
    public partial class RoleManager : System.Web.UI.Page
    {
        public string groupid = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            groupid = Request["groupid"].ConvertTo<string>("0");
        }
    }
}
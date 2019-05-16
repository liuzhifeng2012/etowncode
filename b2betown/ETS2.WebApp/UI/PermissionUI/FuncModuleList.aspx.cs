using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.PermissionUI
{
    public partial class FuncModuleList : System.Web.UI.Page
    {
        public int rpage = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            rpage = Request["rpage"].ConvertTo<int>(1);

        }
    }
}
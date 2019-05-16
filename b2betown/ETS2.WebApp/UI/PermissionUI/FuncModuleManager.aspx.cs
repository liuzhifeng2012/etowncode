using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.PermissionUI
{
    public partial class FuncModuleManager : System.Web.UI.Page
    {
        public string actionid = "0";
        public string actioncolumnid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            actionid = Request["actionid"].ConvertTo<string>("0");
            actioncolumnid = Request["actioncolumnid"].ConvertTo<string>("0");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.PMUI
{
    public partial class AgentSalesCode : System.Web.UI.Page
    {
        public string kkey = "";//传入关键词
        protected void Page_Load(object sender, EventArgs e)
        {
            kkey = Request["kkey"].ConvertTo<string>("");
        }
    }
}
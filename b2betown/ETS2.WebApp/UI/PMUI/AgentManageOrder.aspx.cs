using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.PMUI
{
    public partial class AgentManageOrder : System.Web.UI.Page
    {
        public int agentid = 0;
        public string key = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            agentid = Request["agentid"].ConvertTo<int>(0);
            key = Request["key"].ConvertTo<string>("");
            if (agentid == 0)
            {
                Response.Redirect("AgentList.aspx");
            }
        }
    }
}
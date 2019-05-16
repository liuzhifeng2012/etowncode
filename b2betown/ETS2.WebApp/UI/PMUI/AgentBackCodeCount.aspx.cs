using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.PMUI
{
    public partial class AgentBackCodeCount : System.Web.UI.Page
    {
        public int agentid = 0;
        public int orderid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            agentid = Request["agentid"].ConvertTo<int>(0);
            orderid = Request["orderid"].ConvertTo<int>(0);
            if (agentid == 0 || orderid==0)//分销账户，订单ID必须都有值
            {
                //Response.Redirect("AgentBackCode.aspx");
            }
        }
    }
}
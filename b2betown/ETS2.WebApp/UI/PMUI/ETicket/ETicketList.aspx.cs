using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

namespace ETS2.WebApp.UI.PMUI.ETicket
{
    public partial class ETicketList : System.Web.UI.Page
    {
        public int eclass = -1;//查看范围
        public int proid = 0;//产品id
        public int jsid = 0;//结算id
        public int agentid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            eclass = Request["eclass"].ConvertTo<int>(-1);
            proid = Request["proid"].ConvertTo<int>(0);
            jsid=Request["jsid"].ConvertTo<int>(0);
            agentid = Request["agentid"].ConvertTo<int>(0);
        }
    }
}
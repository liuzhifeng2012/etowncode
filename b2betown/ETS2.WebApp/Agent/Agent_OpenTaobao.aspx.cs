using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETS2.WebApp.Agent
{
    public partial class Agent_OpenTaobao : System.Web.UI.Page
    {

        public int comid_temp = 0;
        public int Agentid = 0;
        public string Account = "";
        public int id = 0;
        public string company = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["Agentid"] != null)
            {
                //账户信息
                Agentid = Int32.Parse(Session["Agentid"].ToString());
                Account = Session["Account"].ToString();
            }
        }

    }
}
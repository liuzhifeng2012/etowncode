using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.CRM.Service.CRMService.Data;
namespace ETS2.WebApp.Agent
{
    public partial class Unwarrant_Supplier : System.Web.UI.Page
    {
        public int Agentid = 0;
        public string Account = "";
        public int Agentsort = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Agentid"] != null)
            {
                //账户信息
                var aget = Session["Agentid"].ToString();
                if (aget != "")
                {
                    Agentid = Int32.Parse(aget);
                    Account = Session["Account"].ToString();
                }
                if (Agentid != 0)
                {
                    var agentdata = AgentCompanyData.GetAgentByid(Agentid);
                    if (agentdata != null)
                    {
                        Agentsort = agentdata.Agentsort;
                    }
                }
                


            }

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.Agent
{
    public partial class Agent_Com_list : System.Web.UI.Page
    {
        public int Agentid = 0;
        public string Account = "";
        public int Agentsort = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Agentid"] != null)
            {
                try
                {
                    //账户信息
                    Agentid = Int32.Parse(Session["Agentid"].ToString());
                    Account = Session["Account"].ToString();

                    if (Agentid != 0)
                    {
                        var agentdata = AgentCompanyData.GetAgentByid(Agentid);
                        if (agentdata != null)
                        {
                            Agentsort = agentdata.Agentsort;
                        }
                    }
                }
                catch
                {
                Response.Redirect("/Agent/login.aspx");
                }



            }
        }
    }
}
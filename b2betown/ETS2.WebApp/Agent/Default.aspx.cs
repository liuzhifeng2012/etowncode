using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.Agent
{
    public partial class Default : System.Web.UI.Page
    {
        public int Agentid=0;
        public string Account="";
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
                if (Agentid !=0)
                {
                    var agentdata = AgentCompanyData.GetAgentByid(Agentid);
                    if (agentdata != null) {
                        Agentsort = agentdata.Agentsort;
                    }
                }
                //微信渠道
                if (Agentsort == 1) {
                    Response.Redirect("Agent_Com_list.aspx");
                }
                //项目账户
                if (Agentsort == 2)
                {
                    Response.Redirect("Project.aspx");
                }


            }
            
        }
    }
}
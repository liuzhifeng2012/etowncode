using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;

using ETS2.CRM.Service.CRMService.Data;
namespace ETS2.WebApp.Agent
{
    public partial class Agent_Com_Open : System.Web.UI.Page
    {
        public int Agentid = 0;
        public string Account = "";
        public int Agentsort = 1;
        public string comaccount = "";
        public string today = DateTime.Now.ToString("yyyy-MM-dd");
        protected void Page_Load(object sender, EventArgs e)
        {

            comaccount = Request["comaccount"].ConvertTo<string>("");
            if (Session["Agentid"] != null)
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
        }
    }
}
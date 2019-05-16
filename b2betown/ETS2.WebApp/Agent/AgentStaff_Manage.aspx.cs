using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.Agent
{
    public partial class AgentStaff_Manage : System.Web.UI.Page
    {
        public int Agentid = 0;
        public string Account = "";
        public int comid_temp = 0;
        public int Agentid_manage = 0;
        public int Id = 0;
 
        protected void Page_Load(object sender, EventArgs e)
        {

            Id = Request["id"].ConvertTo<int>(0);


            //判断登录账户是否为分销开户账户
            if (Session["Agentid"] != null)
            {
                Agentid = Int32.Parse(Session["Agentid"].ToString());

            }

            var agentdate = AgentCompanyData.GetAgentByid(Agentid);
            if (agentdate == null) {
                Response.Redirect("/Agent/login.aspx");
            }
            
            if (agentdate.AccountLevel != 0)
            {
                Response.Redirect("/Agent/login.aspx");
            }

        }
            

    }
}
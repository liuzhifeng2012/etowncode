using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.UI.PMUI
{
    public partial class AgentManage : System.Web.UI.Page
    {
        public int agentid = 0;
        public int comid = 0;//当前登录商家id
        protected void Page_Load(object sender, EventArgs e)
        {
            agentid = Request["agentid"].ConvertTo<int>(0);
            if (agentid == 0)
            {
                //Response.Redirect("AgentList.aspx");
            }

            B2b_company_manageuser user = UserHelper.CurrentUser();
            B2b_company company = UserHelper.CurrentCompany;

            comid = company.ID;
        }
    }
}
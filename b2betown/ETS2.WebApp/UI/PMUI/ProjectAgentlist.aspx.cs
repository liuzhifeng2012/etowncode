using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Common.Business;

namespace ETS2.WebApp.UI.PMUI
{
    public partial class ProjectAgentlist : System.Web.UI.Page
    {
        public int projectid = 0;
        public int comid = 0;
        public string projectname = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            projectid = Request["projectid"].ConvertTo<int>(0);
            B2b_company_manageuser user = UserHelper.CurrentUser();
            B2b_company company = UserHelper.CurrentCompany;
            comid = company.ID;

            if (projectid == 0)
            {

            }
            else {
                var projectdata = new B2b_com_projectData();
                var projectinfo = projectdata.GetProject(projectid, comid);
                if (projectinfo != null) {
                    projectname = projectinfo.Projectname;
                }
            }

        }
    }
}
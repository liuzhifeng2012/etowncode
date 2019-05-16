using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle.Enum;

namespace ETS2.WebApp.UI.PMUI
{
    public partial class project_finance : System.Web.UI.Page
    {
        public int projectid = 0;
        public string projectname = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            projectid = Request["projectid"].ConvertTo<int>(0);
            var prodata = new B2b_com_projectData();
            if (projectid != 0)
            {
                projectname = prodata.GetProjectNameByid(projectid);

            }

        }
    }
}
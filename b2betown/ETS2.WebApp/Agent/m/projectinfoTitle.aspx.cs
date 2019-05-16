using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.WebApp.Agent.m
{
    public partial class projectinfoTitle : System.Web.UI.Page
    {
        public int projectid = 0;//项目id
        public string projectname = "";//项目名称
        public string projectbrief = "";//项目精简介绍
        public string projectserviceintroduce = "";//项目服务介绍
        public int comid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //根据项目id得到项目信息
            projectid = Request["id"].ConvertTo<int>(0);
            B2b_com_project mod = new B2b_com_projectData().GetProject(projectid);
            if (mod != null)
            {
                comid = mod.Comid;
                projectname = mod.Projectname;
                projectbrief = mod.Briefintroduce;
                projectserviceintroduce = mod.Serviceintroduce;
            }

        }
    }
}
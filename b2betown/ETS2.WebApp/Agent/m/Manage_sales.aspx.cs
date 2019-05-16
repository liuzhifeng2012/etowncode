using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.PM.Service.PMService.Modle;
using ETS.Framework;
using ETS2.PM.Service.PMService.Data;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.Agent.m
{
    public partial class Manage_sales : System.Web.UI.Page
    {
        public int comid = 0;

        public string Wxfocus_url = "";
        public string Wxfocus_author = "";

        public int projectid = 0;//项目id
        public string projectname = "";//项目名称
        public string projectimgurl = "";//项目图片
        public string projectbrief = "";//项目精简介绍
        protected void Page_Load(object sender, EventArgs e)
        {

            projectid = Request["projectid"].ConvertTo<int>(0);
            comid = Request["comid"].ConvertTo<int>(0);
            if (comid > 0 & projectid > 0)
            {
                //根据项目id得到项目信息
                B2b_com_project mod = new B2b_com_projectData().GetProject(projectid, comid);
                if (mod != null)
                {
                    projectname = mod.Projectname;
                    projectimgurl = GetProjectImg(mod.Projectimg, mod.Id);
                    projectbrief = mod.Briefintroduce;

                }

                B2b_company companyinfo = B2bCompanyData.GetCompany(comid);
                if (companyinfo.B2bcompanyinfo != null)
                {
                    Wxfocus_url = companyinfo.B2bcompanyinfo.Wxfocus_url;
                    Wxfocus_author = companyinfo.B2bcompanyinfo.Wxfocus_author;

                }
            }
        }
        private static string GetProjectImg(int projectimg, int projectid)
        {
            if (projectimg == 0 || projectimg == 3962)
            {
                //得到项目下第一个在线产品图片做为项目图片
                int firstimg = new B2bComProData().GetFirstProImgInProjectId(projectid);
                return FileSerivce.GetImgUrl(firstimg);
            }
            else
            {
                return FileSerivce.GetImgUrl(projectimg);
            }

        }
    }
}
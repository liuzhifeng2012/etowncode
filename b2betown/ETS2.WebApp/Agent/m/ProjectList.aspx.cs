using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.PM.Service.PMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS.Framework; 

namespace ETS2.WebApp.Agent.m
{
    public partial class ProjectList : System.Web.UI.Page
    {
        public int comid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            comid = Request["comid"].ConvertTo<int>(0);
            if (comid > 0)
            {
                B2b_company company = new B2bCompanyData().GetCompanyBasicById(comid);
                //判断公司是否含有项目，不含有的话，添加默认项目
                int count = new B2b_com_projectData().GetProjectCountByComId(company.ID);
                if (count == 0)
                {
                    B2b_company_info companyinfo = new B2bCompanyInfoData().GetCompanyInfo(company.ID);
                    B2b_company_saleset saleset = B2bCompanySaleSetData.GetDirectSellByComid(company.ID.ToString());

                    B2b_com_project model = new B2b_com_project()
                    {
                        Id = 0,
                        Projectname = company.Com_name,
                        Projectimg = saleset.Logo.ConvertTo<int>(0),
                        Province = companyinfo.Province.ConvertTo<string>(""),
                        City = companyinfo.City,
                        Industryid = company.Com_type,
                        Briefintroduce = companyinfo.Scenic_intro,
                        Address = companyinfo.Scenic_address,
                        Mobile = companyinfo.Tel,
                        Coordinate = "",
                        Serviceintroduce = companyinfo.Serviceinfo,
                        Onlinestate = "1",
                        Comid = company.ID,
                        Createtime = DateTime.Now,
                        Createuserid = 0
                    };
                    int result = new B2b_com_projectData().EditProject(model);

                    //设置公司下产品的项目id都改为默认项目id
                    int result2 = new B2bComProData().UpProjectId(company.ID.ToString(), result);
                }
            }
        }
    }
}
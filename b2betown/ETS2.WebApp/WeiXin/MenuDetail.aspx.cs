using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.WebApp.WeiXin
{
    public partial class MenuDetail : System.Web.UI.Page
    {
        public string fathermenuid = "0";//父菜单id
        public string fathermenuname = "";//父菜单名称

        public string menuid = "0";//菜单id
        public bool whethervertiry = false;

        public int industryid = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            fathermenuid = Request["fathermenuid"].ConvertTo<string>("0");
            fathermenuname = Request["fathermenuname"].ConvertTo<string>("");
            menuid = Request["menuid"].ConvertTo<string>("0");

            if (UserHelper.ValidateLogin())
            {
                B2b_company company = UserHelper.CurrentCompany;
                if (company != null)
                {
                    int comid = company.ID;
                    whethervertiry = new WxRequestXmlData().JudgeWhetherRenZheng(comid);

                    var comdata = B2bCompanyData.GetCompany(company.ID);
                    if (comdata != null)
                    {
                        industryid = comdata.Com_type;
                    }
                }
            }
        }


    }
}
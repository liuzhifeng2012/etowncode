using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.WebApp.WeiXin
{
    public partial class MaterialSort : System.Web.UI.Page
    {
        public int comid = 0;//当前登录商家id
        public int userid = 0;//当前登录商户id
        public int promotetypeid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            promotetypeid = Request["promotetypeid"].ConvertTo<int>(0);
            if (UserHelper.ValidateLogin())
            {
                GetUser();
            }
            else
            {
                Response.Redirect("/Manage/index1.html");
            }
        }
        private void GetUser()
        {
            B2b_company_manageuser user = UserHelper.CurrentUser();
            B2b_company company = UserHelper.CurrentCompany;

            userid = user.Id;
            comid = company.ID;
        }
    }
}
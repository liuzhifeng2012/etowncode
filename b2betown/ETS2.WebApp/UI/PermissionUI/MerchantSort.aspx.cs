using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.WebApp.UI.PermissionUI
{
    public partial class MerchantSort : System.Web.UI.Page
    {
        public int comid = 0;//当前登录商家id
        public int userid = 0;//当前登录商户id
        //public int fathermenuid = 0;

        public int comstate = 1;//公司运营状态：1，开通；2，暂停
        protected void Page_Load(object sender, EventArgs e)
        {
            //fathermenuid = Request["fathermenuid"].ConvertTo<int>(0);
            if (UserHelper.ValidateLogin())
            {
                GetUser();
            }
            else
            {
                Response.Redirect("/Manage/index1.html");
            }
            comstate = Request["comstate"].ConvertTo<int>(1);
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
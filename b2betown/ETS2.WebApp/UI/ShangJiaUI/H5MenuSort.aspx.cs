using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.WebApp.UI.ShangJiaUI
{
    public partial class H5MenuSort : System.Web.UI.Page
    {
        public int comid = 0;//当前登录商家id
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserHelper.ValidateLogin())
            {
                GetUser();
            }
        }
        private void GetUser()
        {
            B2b_company_manageuser user = UserHelper.CurrentUser();
            B2b_company company = UserHelper.CurrentCompany;

            comid = company.ID;
        }
    }
}
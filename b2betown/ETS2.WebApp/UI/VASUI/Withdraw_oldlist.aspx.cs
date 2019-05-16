using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Common.Business;
using ETS.Framework;

namespace ETS2.WebApp.UI.VASUI
{
    public partial class Withdraw_oldlist : System.Web.UI.Page
    {
        public decimal imprest = 0;//预付款记录

        public string fileurl = "";
        protected void Page_Load(object sender, EventArgs e)
        {


            B2b_company company = UserHelper.CurrentCompany;
            imprest = company.Imprest;

            fileurl = AppSettings.CommonSetting.GetValue("FileUpload/FileUrl").ConvertTo<string>();
        }
    }
}
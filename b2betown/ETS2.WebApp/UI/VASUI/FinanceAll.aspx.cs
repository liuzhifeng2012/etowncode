using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Common.Business;

namespace ETS2.WebApp.UI.VASUI
{
    public partial class FinanceAll : System.Web.UI.Page
    {
        public decimal imprest = 0;//预付款记录
        protected void Page_Load(object sender, EventArgs e)
        {
          
          
            B2b_company company = UserHelper.CurrentCompany;
            imprest = company.Imprest;

        }
    }
}
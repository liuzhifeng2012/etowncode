using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.UI.CrmUI
{
    public partial class member_interest_manage : System.Web.UI.Page
    {
        public int crmid = 0;

        public int crmpageindex = 0;

        public string crminterest = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            crmid = Request["crmid"].ConvertTo<int>(0);

            crmpageindex = Request["crmpageindex"].ConvertTo<int>(0);

            int total = 0;
            IList<B2b_crm_interesttag> result = new B2b_crm_interesttagData().GetCrmInterest(out total, crmid);
            if (result != null) { 
                for (int i = 0; i< result.Count();i++ ){
                crminterest += result[i].Tagid.ToString() + ",";
             }
            }


        }
    }
}
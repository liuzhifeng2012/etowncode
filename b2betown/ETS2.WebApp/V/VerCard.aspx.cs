using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.JsonFactory;
namespace ETS2.WebApp.UI.MemberUI
{
    public partial class VerCard : System.Web.UI.Page
    {
        public string pno = "";
        public int validateticketlogid = 0;//验票日志id
        public string printname = "";
        public string comname = "";
        public string account_info = "";

        //用户信息
        public string AccountName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
                pno = Request["pno"].ConvertTo<string>("");
                validateticketlogid = Request["validateticketlogid"].ConvertTo<int>(0);
           
                B2b_company_manageuser user = UserHelper.CurrentUser();
                B2b_company company = UserHelper.CurrentCompany;
                comname = company.Scenic_name;
              
            //用户信息
                AccountName = user.Accounts;


                B2bCompanyData datecom = new B2bCompanyData ();
                B2b_company modledatecom = B2bCompanyData.GetAllComMsg(company.ID);

                printname = modledatecom.B2bcompanyinfo.Defaultprint;
                account_info = UserHelper.CurrentUser().Accounts;

        }
    }
}
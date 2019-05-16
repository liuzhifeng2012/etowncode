using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.UI.CrmUI
{
    public partial class Sms_Qunfa : System.Web.UI.Page
    {
        public string Phone = "";
        public string Name = "";
        public bool IsParentCompanyUser = true; //判断操作账户类型(总公司账户;门市账户)
        protected void Page_Load(object sender, EventArgs e)
        {
            Phone = Request["Phone"].ConvertTo<string>("");
            Name = Request["Name"].ConvertTo<string>("");
            if (Name != "")
            {
                Name = " " + Name;
            }

            int userid = UserHelper.CurrentUserId();
            IsParentCompanyUser = new B2bCompanyManagerUserData().IsParentCompanyUser(userid);
        }
    }
}
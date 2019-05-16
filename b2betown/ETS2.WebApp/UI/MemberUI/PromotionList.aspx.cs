using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.UI.MemberUI
{
    public partial class PromotionList : System.Web.UI.Page
    {
        public bool IsParentCompanyUser = true; //判断操作账户类型(总公司账户;门市账户)
        protected void Page_Load(object sender, EventArgs e)
        {
            int userid = UserHelper.CurrentUserId();
            IsParentCompanyUser = new B2bCompanyManagerUserData().IsParentCompanyUser(userid);
            //if (IsParentCompanyUser == false)
            //{
            //    Response.Redirect("/ui/MemberUI/ERNIERecordList.aspx");
            //}
        }
    }
}
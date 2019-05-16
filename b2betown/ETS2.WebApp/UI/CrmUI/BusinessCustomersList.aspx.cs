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
    public partial class BusinessCustomersList : System.Web.UI.Page
    {
        public string isactivate = "1";//是否激活，默认1激活
        public bool IsParentCompanyUser = true; //判断操作账户类型(总公司账户;门市账户)

        public int pageindex = 1;//页面加载时页数

        public string md5info = "";//用于下载会员信息
        public int comid = 0;//用于下载会员信息
        protected void Page_Load(object sender, EventArgs e)
        {
            isactivate = Request["isactivate"].ConvertTo<string>("1");
            int userid = UserHelper.CurrentUserId();
            IsParentCompanyUser = new B2bCompanyManagerUserData().IsParentCompanyUser(userid);

            pageindex = Request["pageindex"].ConvertTo<int>(1);

            int comid = UserHelper.CurrentCompany.ID;
            md5info = EncryptionHelper.ToMD5(comid.ToString() + "lixh1210", "UTF-8");
        }
    }
}
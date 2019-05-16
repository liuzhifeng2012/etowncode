using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.WeiXin.invitecode
{
    public partial class send : System.Web.UI.Page
    {
        public string phone = "";//手机号字符串
        public string qunfa = "no";//是否是群发，默认不是

        public string weixinname = "";//微信号

        public bool IsParentCompanyUser = true; //判断操作账户类型(总公司账户;门市账户)
        protected void Page_Load(object sender, EventArgs e)
        {
            qunfa = Request["qunfa"].ConvertTo<string>("no");
            phone = Request["phone"].ConvertTo<string>("");

            //获取公司微信号
            B2b_company com = B2bCompanyData.GetAllComMsg(UserHelper.CurrentCompany.ID);
            weixinname = com.B2bcompanyinfo.Weixinname;

            int userid = UserHelper.CurrentUserId();
            IsParentCompanyUser = new B2bCompanyManagerUserData().IsParentCompanyUser(userid);

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.WebApp.UI.MemberUI
{
    public partial class ChannelList : System.Web.UI.Page
    {
        public string statistics = "";

        public bool IsParentCompanyUser = true; //判断操作账户类型(总公司账户;门市账户)
        public string channelsource = "0";//渠道类型（0：内部门店；1：合作公司）
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                statistics = Request.QueryString["statistics"].ConvertTo<string>("");
                if (statistics == "")
                {
                    Response.Write("<script>alert('渠道单位不存在');location.href='Channelstatistics.aspx'</script>");
                }
                int userid = UserHelper.CurrentUserId();
                IsParentCompanyUser = new B2bCompanyManagerUserData().IsParentCompanyUser(userid);
                if (IsParentCompanyUser == false)//如果是门市账户
                {
                    B2b_company_manageuser user = B2bCompanyManagerUserData.GetUser(userid);
                    channelsource = user.Channelsource.ToString();
                }
            }

        }
    }
}
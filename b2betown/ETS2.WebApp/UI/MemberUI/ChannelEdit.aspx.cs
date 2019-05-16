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
    public partial class ChannelEdit : System.Web.UI.Page
    {
        public string channelid = "0";//渠道id
        public string channeltype = "out";//渠道类型

        public bool IsParentCompanyUser = true; //判断操作账户类型(总公司账户;门市账户)
        public string channelcompanyid = "0";//渠道公司id
        protected void Page_Load(object sender, EventArgs e)
        {
            channelid = Request["channelid"].ConvertTo<string>("0");
            if (channelid == "0")
            {
                Response.Redirect("/ui/MemberUI/Channelstatistics.aspx");
            }


            channeltype = Request["channeltype"].ConvertTo<string>("out");
            if (channeltype == "inner")
            {
                channeltype = "0";
            }
            else
            {
                channeltype = "1";
            }
            int userid = UserHelper.CurrentUserId();
            IsParentCompanyUser = new B2bCompanyManagerUserData().IsParentCompanyUser(userid);
            if (IsParentCompanyUser == false)//如果是门市账户
            {
                B2b_company_manageuser user = B2bCompanyManagerUserData.GetUser(userid);
                channelcompanyid = user.Channelcompanyid.ToString();
            }
        }
    }
}
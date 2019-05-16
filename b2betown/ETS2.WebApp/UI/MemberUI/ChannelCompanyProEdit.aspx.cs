using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using FileUpload.FileUpload.Entities.Enum;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Common.Business;

namespace ETS2.WebApp.UI.MemberUI
{
    public partial class ChannelCompanyProEdit : System.Web.UI.Page
    {
        public string channelcompanyid = "";
        public string channeltype = "";//渠道类型

        public bool IsParentCompanyUser = true; //判断操作账户类型(总公司账户;门市账户)
        protected void Page_Load(object sender, EventArgs e)
        {
            channelcompanyid = Request["channelcompanyid"].ConvertTo<string>("0");
            channeltype = Request["channeltype"].ConvertTo<string>("inner");


            IsParentCompanyUser = new B2bCompanyManagerUserData().IsParentCompanyUser(UserHelper.CurrentUserId());
        }
    }
}
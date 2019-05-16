using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Common.Business;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.CRM.Service.CRMService.Data;
using ETS.JsonFactory;
using ETS2.WeiXin.Service.WeiXinService.Model;
using System.Xml;
using Newtonsoft.Json;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;
namespace ETS2.WebApp.H5
{
    public partial class Archives : System.Web.UI.Page
    {
        public string openid = "";//微信号
        public int AccountId = 0;//会员id
        public B2b_crm userinfo = new B2b_crm();
        public int comid = 0;
        public string title = "";
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        public string userid = "0";//用户临时 Uid 或 实际Uid 

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
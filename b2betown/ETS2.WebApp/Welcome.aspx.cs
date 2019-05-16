using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Common.Business;

namespace ETS2.WebApp
{
    public partial class Welcome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //lable1.Text ="最后一天'"+ DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "'<br>上个月最后一天'" + DateTime.Now.AddDays(1 - DateTime.Now.Day).AddDays(-1).ToString("yyyy-MM-dd") + "'<br>下个月第一天'" + DateTime.Now.AddMonths(1).AddDays(1 - DateTime.Now.AddMonths(1).Day).ToString("yyyy-MM-dd")+"'";

            //判断当前用户，如果为“平台总账户",直接跳转到商户页面
            int userid = UserHelper.CurrentUserId();
            if (userid == 1035)
            {
                Response.Redirect("/ui/permissionui/SSort.aspx");
            }
            if (userid == 1053)
            {
                Response.Redirect("ui/pmui/CoopCount.aspx");
            }
        }
    }
}
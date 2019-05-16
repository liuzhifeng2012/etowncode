using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Permision.Service.PermisionService.Model;
using ETS2.Permision.Service.PermisionService.Data;

namespace ETS2.WebApp.Account
{
    public partial class Login : System.Web.UI.Page
    {
        public int comid = 0;//当前登录商家id
        protected void Page_Load(object sender, EventArgs e)
        {
            ActionInit();

            string RequestDomin = Request.ServerVariables["SERVER_NAME"].ToLower();
          

            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "1");

            //如果是微旅行，则直接跳转 当域名为微旅行，访问默认页面则直接跳转会员专区
            if (RequestDomin == "www.maikexing.com" )
            {
                comid = 1305;
            }

        }

        private void ActionInit()
        {
            int initaction = new Sys_ActionData().ActionInit();
            if (initaction == 0)
            {
                Response.Write("<script>alert('权限初始化失败');</script>");
            }
        }
    }
}

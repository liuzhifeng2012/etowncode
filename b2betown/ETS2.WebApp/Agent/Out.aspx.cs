using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETS2.WebApp.Agent
{
    public partial class Out : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Agentid"] = "";
            Session["Account"] = "";

            //退出时注销
            HttpCookie cookie = new HttpCookie("Agentid");     //实例化HttpCookie类并添加值
            cookie.Value = "";
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);

            cookie = new HttpCookie("Account");     //实例化HttpCookie类并添加值
            cookie.Value = "";
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);

            cookie = new HttpCookie("AgenKey");     //实例化HttpCookie类并添加值
            cookie.Value = "";
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);


            Response.Redirect("/Agent/login.aspx");
        }
    }
}
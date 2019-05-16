using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.Common.Business;

namespace ETS2.WebApp.Agent
{
    public partial class Login : System.Web.UI.Page
    {
        public string Email = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Email = Request["Email"].ConvertTo<string>("");

            string RequestDomin = Request.ServerVariables["SERVER_NAME"].ToLower();

            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            bool bo = detectmobilebrowser.HttpUserAgent(u);
            if (bo)
            {
                Response.Redirect("/agent/m/login.aspx");
            }
            else
            {
                if (RequestDomin == "agent.maikexing.com")//针对 麦客行，分销 手工跳转
                {

                    Response.Redirect("/Agent/page.html");

                }
            }

          
        }
    }
}
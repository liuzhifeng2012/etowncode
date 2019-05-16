using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.Agent.m
{
    public partial class register : System.Web.UI.Page
    {
        public string new_tel = "";
        public int comid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            new_tel = Request["new_tel"].ConvertTo<string>("");
            if(new_tel=="")
            {
                Response.Redirect("phoneverify.aspx");
            }
            HttpCookie cookie = Request.Cookies["openid"];
            if (cookie != null)
            {
                string openid = cookie.Value; 
                //根据openid得到comid
                if (openid != "")
                {
                    comid = new B2bCrmData().GetComidByOpenid(openid);
                }
            }

        }
    }
}
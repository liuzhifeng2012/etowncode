using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETS2.WebApp.M
{
    public partial class Out : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["AccountId"] = null;
            Session["AccountName"] = null;
            Session["AccountCard"] = null;

            if (Request.Cookies["AccountId"] != null)
            {
                HttpCookie mycookie;
                mycookie = Request.Cookies["AccountId"];
                TimeSpan ts = new TimeSpan(0, 0, 0, 0);//时间跨度 
                mycookie.Expires = DateTime.Now.Add(ts);//立即过期 
                Response.Cookies.Remove("AccountId");//清除 
                Response.Cookies.Add(mycookie);//写入立即过期的*/
                Response.Cookies["AccountId"].Expires = DateTime.Now.AddDays(-1);
            }
            if (Request.Cookies["AccountName"] != null)
            {
                HttpCookie mycookie = Request.Cookies["AccountName"];
                TimeSpan ts = new TimeSpan(0, 0, 0, 0);//时间跨度 
                mycookie.Expires = DateTime.Now.Add(ts);//立即过期 
                Response.Cookies.Remove("AccountName");//清除 
                Response.Cookies.Add(mycookie);//写入立即过期的*/
                Response.Cookies["AccountName"].Expires = DateTime.Now.AddDays(-1);
            }
            if (Request.Cookies["AccountKey"] != null)
            {

                HttpCookie mycookie = Request.Cookies["AccountKey"];
                TimeSpan ts = new TimeSpan(0, 0, 0, 0);//时间跨度 
                mycookie.Expires = DateTime.Now.Add(ts);//立即过期 
                Response.Cookies.Remove("AccountKey");//清除 
                Response.Cookies.Add(mycookie);//写入立即过期的*/
                Response.Cookies["AccountKey"].Expires = DateTime.Now.AddDays(-1);

            }

            HttpCookie aCookie; string cookieName; 
            int limit = Request.Cookies.Count; 
            for (int i = 0; i < limit; i++) 
            { 
                cookieName = Request.Cookies[i].Name;
                aCookie = new HttpCookie(cookieName); 
                aCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(aCookie); 
            }

           

            //HttpCookie cookie = new HttpCookie("AccountId", null);     //实例化HttpCookie类并添加值
           // Response.Cookies.Add(cookie); 
            //cookie = new HttpCookie("AccountName", null);     //实例化HttpCookie类并添加值
            ///Response.Cookies.Add(cookie);
            //cookie = new HttpCookie("AccountKey", null);     //实例化HttpCookie类并添加值
           // Response.Cookies.Add(cookie); 

            Response.Redirect("/m/");
        }
    }
}
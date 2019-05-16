using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle.Enum;

namespace ETS2.Common.Business
{
    /// <summary>
    /// 通用类
    /// </summary>
    public class UserHelper
    {

        private static readonly string COOKIE_Token = "EasytourUserToken";

        //private static EntryLogCaching caching = new EntryLogCaching();

        #region 获取当前登录用户信息 By:Xiaoxiong
        /// <summary>
        /// 获取当前登录用户的ID By:Xiaoxiong
        /// </summary>
        /// <returns>当前登录用户的ID</returns>
        public static B2b_company_manageuser CurrentUser(int userId = 0)
        {
            var identity = HttpContext.Current.User.Identity;
            if (!identity.IsAuthenticated)
            {
                HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl);
            }
            if (userId == 0 && !string.IsNullOrEmpty(identity.Name))
            {
                userId = int.Parse(identity.Name);
            }

            if (userId == 0)
            {
                if (!HttpContext.Current.Response.IsRequestBeingRedirected)
                {
                    HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl);
                }
                return null;
            }

            B2b_company_manageuser user = B2bCompanyManagerUserData.GetUser(userId);

            if (user == null || user.Id != userId || !VerifyToken(userId))
            {
                if (!HttpContext.Current.Response.IsRequestBeingRedirected)
                {
                    HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl);
                }

            }
            return user;
        }
        #region 验证用户是否已经登录
        public static bool ValidateLogin(int userid = 0)
        {
            var identity = HttpContext.Current.User.Identity;
            if (!identity.IsAuthenticated)
            {
                return false;
            }

            if (userid == 0)
            {
                userid = int.Parse(identity.Name);
            }
            B2b_company_manageuser userinfo = B2bCompanyManagerUserData.GetUser(userid);
            if (userinfo == null || userinfo.Id != userid || !VerifyToken(userid))
            {
                return false;
            }
            return true;
        }
        #endregion


        #region 用户注销 By:Xiaoxiong
        /// <summary>
        /// 用户注销
        /// </summary>
        public static void Logout(string dredirecturl="")
        {
            HttpContext context = HttpContext.Current;
            //唯一标识
            HttpCookie tokenCookie = new HttpCookie(COOKIE_Token, "");
            tokenCookie.Expires = DateTime.Now.AddDays(-1);
            context.Response.Cookies.Add(tokenCookie);
            context.Response.Cookies.Remove(COOKIE_Token);
            //登录Cookie
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            authCookie.Expires = DateTime.Now.AddYears(-1);
            context.Response.Cookies.Add(authCookie);
            //Asp.net
            HttpCookie aspNetCookie = new HttpCookie("ASP.NET_SessionId", "");
            aspNetCookie.Expires = DateTime.Now.AddYears(-1);
            context.Response.Cookies.Add(aspNetCookie);

            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();


            var RequestDomin = context.Request.ServerVariables["SERVER_NAME"].ToLower();

            int juststate = 0;
            if (context.Session["Agent_Com_Login"] != null)
            {
                int agentlogstate = Int32.Parse(context.Session["Agent_Com_Login"].ToString());
                if (agentlogstate >0)
                {
                    juststate = 1;
                    context.Session["Agent_Com_Login"] = null;
                }
            }

            if (dredirecturl!="")
            {
                context.Response.Redirect(dredirecturl);
            }
            else
            {
                if (juststate == 1)
                {
                    context.Response.Write("<script>window.opener=null;window.close();</script>");// 不会弹出询问

                }
                else
                {
                    //如果是微旅行，则直接跳转 当域名为微旅行，访问默认页面则直接跳转会员专区
                    if ((RequestDomin == "admin.vctrip.com" || RequestDomin == "shop.vctrip.com" || RequestDomin == "admin.etown.cn"))
                    {
                        context.Response.Redirect("/Manage/index1.html");
                    }
                    else if (RequestDomin == "shop.etown.cn")
                    {
                        context.Response.Redirect("/Manage/default.html");
                    }
                    else if (RequestDomin == "www.maikexing.com" || RequestDomin == "admin.maikexing.com" || RequestDomin == "agent.maikexing.com")
                    {
                        context.Response.Redirect("/agent/page.html");
                    }
                    else
                    {
                        context.Response.Redirect("/admin");
                    }
                }
            }

        }

        #endregion
        #region 用户直接注销，不带跳转 By:xiaoliu 不带跳转
        /// <summary>
        /// 用户直接注销，不带跳转
        /// </summary>
        public static void DirictLogout()
        {
            HttpContext context = HttpContext.Current;
            //唯一标识
            HttpCookie tokenCookie = new HttpCookie(COOKIE_Token, "");
            tokenCookie.Expires = DateTime.Now.AddDays(-1);
            context.Response.Cookies.Add(tokenCookie);
            context.Response.Cookies.Remove(COOKIE_Token);
            //登录Cookie
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            authCookie.Expires = DateTime.Now.AddYears(-1);
            context.Response.Cookies.Add(authCookie);
            //Asp.net
            HttpCookie aspNetCookie = new HttpCookie("ASP.NET_SessionId", "");
            aspNetCookie.Expires = DateTime.Now.AddYears(-1);
            context.Response.Cookies.Add(aspNetCookie);

            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();


        }

        #endregion
        /// <summary>
        /// 验证唯一标识
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private static bool VerifyToken(int userId)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[COOKIE_Token];
            if (cookie == null)
            {
                return false;
            }
            //EntryLog log = caching.Get(userId);
            //if (log == null)
            //{
            //    return false;
            //}
            Guid token;
            if (!Guid.TryParse(cookie.Value, out token))
            {
                return false;
            }
            //if (token != log.Token)
            //{
            //    return false;
            //}
            return true;
        }


        public static B2b_company CurrentCompany
        {
            get
            {
                var user = UserHelper.CurrentUser();
                if (user.Id <= 0)
                {
                    return null;
                }
                var company = B2bCompanyData.GetCompanyByUid(user.Id);//根据用户id得到商家基本信息
                if (company == null)
                {
                    return null;
                }
                return company;
            }
        }

        #region 用户登录 By:Xiaoxiong
        /// <summary>
        /// 用户登录
        /// </summary>                                     
        public static bool Entry(string username, string password, out string message, out int userId)
        {
            userId = 0;
            var msg = "";
            B2b_company_manageuser user = B2bCompanyManagerUserData.VerifyUser(username, password, out msg);
            if (user == null)
            {
                message = msg;
                return false;
            }


            userId = user.Id;
            B2b_company company = B2bCompanyData.GetCompany(user.Com_id);//根据公司id得到公司信息
            if (company == null)
            {
                message = "商家null";
                return false;
            }
            else
            {
                if (company.Com_state == (int)CompanyStatus.InBusiness)
                {
                     
                    
                    message = "";
                    return true;
                }
                else
                {
                    message = "商家状态不正确";
                    return false;
                }
            }


        }

        #endregion


        #region 设置cookie By:Xiaoxiong

        public static void SetCookie(int userId)
        {
            Guid token = Guid.NewGuid();
            HttpContext context = HttpContext.Current;
            ////保存登录信息
            //EntryLog log = EntryLogData.CreateOrUpdate(new EntryLog
            //{
            //    UserId = userId,
            //    LoginIp = context.Request.UserHostAddress,
            //    Token = token
            //});

            ////删除缓存
            //caching.Remove(userId);

            //设置唯一标识
            HttpCookie cookie = new HttpCookie(COOKIE_Token, token.ToString());
            //cookie.Expires = DateTime.Now.Add(FormsAuthentication.Timeout);
            cookie.Path = FormsAuthentication.FormsCookiePath;
            cookie.Domain = FormsAuthentication.CookieDomain;
            cookie.HttpOnly = true;

            context.Response.Cookies.Remove(COOKIE_Token);
            context.Response.Cookies.Add(cookie);

            FormsAuthentication.SetAuthCookie(userId.ToString(), true,FormsAuthentication.FormsCookiePath);
        }


        #endregion


        public static int CurrentUserId()
        {
            return CurrentUser().Id;
        }
        #endregion

        ///// <summary>
        ///// 验证买家和卖家是否相同用户或机构
        ///// </summary>
        ///// <param name="model"></param>
        //public static int CheckIsSameUser(int userId)
        //{
        //    int sameUserRole = 0;
        //    if (userId == UserHelper.CurrentUser().Id)//相同用户
        //    {
        //        sameUserRole = 1;
        //    }
        //    else if (CompanyBusiness.GetCompanyByUserId(userId).Id == UserHelper.CurrentCompany.Id)//相同机构
        //    {
        //        sameUserRole = 2;
        //    }

        //    return sameUserRole;
        //}
    }
}

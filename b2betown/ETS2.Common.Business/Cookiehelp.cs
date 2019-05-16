using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ETS2.Common.Business
{
    public static class Cookiehelp
    {

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue,DateTime expirestime)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value=HttpUtility.UrlEncode(strValue, Encoding.GetEncoding("UTF-8"));
            cookie.Expires = expirestime;
            // cookie.Value = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        
        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                //HttpUtility.UrlDecode(strName, Encoding.GetEncoding("UTF-8"));
                //return HttpContext.Current.Request.Cookies[strName].Value.ToString();
                return HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies[strName].Value.ToString(), Encoding.GetEncoding("UTF-8"));
            }
            return "";
        }
        /// <summary>
        /// 清除cookie
        /// </summary>
        /// <param name="strName"></param>
        public static void DeleteCookie(string strName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (HttpContext.Current.Request.Cookies != null && cookie != null)
            {
                cookie.Expires = DateTime.Today.AddDays(-1);
                //这句至关重要，需把过期的cookie告诉客户端： 
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
    }
}

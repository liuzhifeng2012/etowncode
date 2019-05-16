using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ETS2.Common.Business
{
    public class CookieHelper
    {

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value=HttpUtility.UrlEncode(strValue, Encoding.GetEncoding("UTF-8"));
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
    }
}

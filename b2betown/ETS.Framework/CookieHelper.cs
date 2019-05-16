using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Specialized;

namespace ETS.Framework
{
    public class CookieHelper
    {

        private static string key = "$%^&*&^%$%^&*)IJKMNGTFVB";

        public static void Set(string name, string value, TimeSpan expires, bool isEncrypt)
        {
            Set(name, (c) =>
            {
                if (isEncrypt)
                {
                    value = EncryptionHelper.Encrypt(value, key);
                }
                c.Value = value;
            }, expires);
        }

        public static void Set(string name, NameValueCollection values, TimeSpan expires)
        {
            Set(name, (c) =>
            {
                foreach (var item in values.AllKeys)
                {
                    c[item] = values[item];
                }
            }, expires);
        }

        private static void Set(string name, Action<HttpCookie> setValue, TimeSpan expires)
        {
            var context = HttpContext.Current;
            HttpCookie cookie = context.Request.Cookies.Get(name);
            if (cookie == null)
            {
                cookie = new HttpCookie(name);
            }
            setValue(cookie);
            if (expires != TimeSpan.Zero)
            {
                cookie.Expires = DateTime.UtcNow.Add(expires);
            }
            HttpContext.Current.Response.Cookies.Set(cookie);
        }



        public static string Get(string name, bool isEncrpt)
        {
            var request = HttpContext.Current.Request;
            if (request.Cookies[name] == null)
            {
                return null;
            }
            string value = request.Cookies[name].Value;
            if (isEncrpt)
            {
                value = EncryptionHelper.Decrypt(value, key);
            }
            return value;
        }

        public static NameValueCollection GetValues(string name)
        {
            var request = HttpContext.Current.Request;
            if (request.Cookies[name] == null)
            {
                return null;
            }
            return request.Cookies[name].Values;
        }
    }
}

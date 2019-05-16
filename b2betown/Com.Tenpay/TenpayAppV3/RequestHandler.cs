using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;

namespace Com.Tenpay.TenpayAppV3
{
    /**
    '签名工具类
     ============================================================================/// <summary>
    'api说明：
    'init();
    '初始化函数，默认给一些参数赋值。
    'setKey(key_)'设置商户密钥
    'createMd5Sign(signParams);字典生成Md5签名
    'genPackage(packageParams);获取package包
    'createSHA1Sign(signParams);创建签名SHA1
    'parseXML();输出xml
    'getDebugInfo(),获取debug信息
     * 
     * ============================================================================
     */

    public class RequestHandler
    {
        private string debugInfo;
        protected HttpContext httpContext;
        private string key;

        /** 请求的参数 */
        protected Hashtable parameters;

        public RequestHandler(HttpContext httpContext)
        {
            parameters = new Hashtable();

            this.httpContext = httpContext;
        }

        /** debug信息 */

        /** 初始化函数 */

        public virtual void init()
        {
        }

        /** 获取debug信息 */

        public String getDebugInfo()
        {
            return debugInfo;
        }

        /** 获取密钥 */

        public String getKey()
        {
            return key;
        }

        /** 设置密钥 */

        public void setKey(string key)
        {
            this.key = key;
        }

        /** 设置参数值 */

        public void setParameter(string parameter, string parameterValue)
        {
            if (parameter != null && parameter != "")
            {
                if (parameters.Contains(parameter))
                {
                    parameters.Remove(parameter);
                }

                parameters.Add(parameter, parameterValue);
            }
        }

        //创建package签名
        public virtual string CreateMd5Sign(string key, string value)
        {
            var sb = new StringBuilder();

            var akeys = new ArrayList(parameters.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                var v = (string) parameters[k];
                if (null != v && "".CompareTo(v) != 0
                    && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append(key + "=" + value);
            //return sb.ToString();


            string sign = MD5Util.GetMD5(sb.ToString(), getCharset()).ToUpper();

            return sign;
        }

        //输出XML
        public string parseXML()
        {
            var sb = new StringBuilder();
            sb.Append("<xml>");
            var akeys = new ArrayList(parameters.Keys);
            foreach (string k in akeys)
            {
                var v = (string) parameters[k];
                if (v == null) {
                    sb.Append("<" + k + "><![CDATA[]]></" + k + ">");

                    //WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\wxpay\\Log.txt", "parseXML方法出现null值，k为"+k);
                }
                else
                {
                    if (Regex.IsMatch(v, @"^[0-9.]$"))
                    {
                        sb.Append("<" + k + ">" + v + "</" + k + ">");
                    }
                    else
                    {
                        sb.Append("<" + k + "><![CDATA[" + v + "]]></" + k + ">");
                    }
                }
            }
            sb.Append("</xml>");
            return sb.ToString();
        }

        protected virtual string getCharset()
        {
            return httpContext.Request.ContentEncoding.BodyName;
        }

        public static void WriteFile(string pathWrite, string content)
        {
            if (File.Exists(pathWrite))
            {
                //File.Delete(pathWrite);
            }
            File.AppendAllText(pathWrite, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + content + "\r\n----------------------------------------\r\n",
                Encoding.GetEncoding("utf-8"));
        }

        //创建微信收货地址共享接口 签名
        public virtual string CreateAddrSign()
        {
            var sb = new StringBuilder();

            var akeys = new ArrayList(parameters.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                var v = (string)parameters[k];
                if (null != v && "".CompareTo(v) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            string sign = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sb.ToString().Substring(0, sb.ToString().Length - 1), "SHA1");
            //string sign = SHA1Util.getSha1(sb.ToString().Substring(0, sb.ToString().Length-1));

            return sign;
        }
    }
}
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Xml;
using System;
using System.Text;
using System.IO;

namespace Com.Tenpay.TenpayAppV3
{
    /** 
   '============================================================================
   'api说明：
   'getKey()/setKey(),获取/设置密钥
   'getParameter()/setParameter(),获取/设置参数值
   'getAllParameters(),获取所有参数
   'isTenpaySign(),是否正确的签名,true:是 false:否
   'isWXsign(),是否正确的签名,true:是 false:否
   ' * isWXsignfeedback判断微信维权签名
   ' *getDebugInfo(),获取debug信息
   '============================================================================
   */

    public class ResponseHandler
    {
        // appkey
        private string appkey;

        protected string content;

        //参与签名的参数列表
        protected HttpContext httpContext;
        protected Hashtable parameters;

        //xmlMap
        private Hashtable xmlMap;

        //获取页面提交的get和post参数
        public ResponseHandler(HttpContext httpContext)
        {
            parameters = new Hashtable();
            xmlMap = new Hashtable();

            this.httpContext = httpContext;
         
            NameValueCollection collection;
            //post data
            if (this.httpContext.Request.HttpMethod == "POST")
            {
                collection = this.httpContext.Request.Form;
            }
            else
            {
                //query string
                collection = this.httpContext.Request.QueryString;
            }

            foreach (string k in collection)
            {
                string v = collection[k];
                setParameter(k, v);
            }
 
            if (this.httpContext.Request.InputStream.Length > 0)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(this.httpContext.Request.InputStream);
                XmlNode root = xmlDoc.SelectSingleNode("xml");
                XmlNodeList xnl = root.ChildNodes;

                foreach (XmlNode xnf in xnl)
                {
                    xmlMap.Add(xnf.Name, xnf.InnerText);
                    this.setXmlNode(xnf.Name, xnf.InnerText);
                }
            }

        }

        public virtual void init()
        {
        }


        /** 获取密钥 */

        public string getKey()
        {
            return appkey;
        }

        /** 设置密钥 */

        public void setKey(string appkey)
        {
            this.appkey = appkey;

        }

        /** 获取参数值 */

        public string getParameter(string parameter)
        {
            var s = (string)parameters[parameter];
            return (null == s) ? "" : s;
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



        /** 获取Xml值 */

        public string getXmlNode(string xmlkey)
        {
            var s = (string)xmlMap[xmlkey];
            return (null == s) ? "" : s;
        }

        /** 设置xml值 */

        public void setXmlNode(string xmlkey, string xmlValue)
        {
            if (xmlkey != null && xmlkey != "")
            {
                if (xmlMap.Contains(xmlkey))
                {
                    xmlMap.Remove(xmlkey);
                }

                xmlMap.Add(xmlkey, xmlValue);
            }
        }
 

        //判断微信签名
        public virtual Boolean isWXsign()
        {
            StringBuilder sb = new StringBuilder();
            Hashtable signMap = new Hashtable();

            foreach (string k in xmlMap.Keys)
            {
                if (k != "sign")
                {
                    signMap.Add(k.ToLower(), xmlMap[k]);
                }
            }

            ArrayList akeys = new ArrayList(signMap.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)signMap[k];

                sb.Append(k + "=" + v + "&");

            }
            sb.Append("key=" + this.getKey());

            string sign = MD5Util.GetMD5(sb.ToString(), getCharset()).ToUpper();

            return sign.Equals(getXmlNode("sign"));

        }
 

        protected virtual string getCharset()
        {
            return httpContext.Request.ContentEncoding.BodyName;
        }
        
    }
}
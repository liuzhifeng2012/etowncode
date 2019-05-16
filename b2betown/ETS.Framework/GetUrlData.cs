using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Xml;

namespace ETS.Framework
{

    public class GetUrlData : System.Web.UI.Page
    {
        public GetUrlData()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region 模拟http 发送post或get请求 -常用

        public string HttpPost(string url, string data)
        {
            string returnData = null;
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(url);
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.ContentLength = buffer.Length;
                Stream postData = webReq.GetRequestStream();
                postData.Write(buffer, 0, buffer.Length);
                postData.Close();
                //HttpWebResponse webResp = (HttpWebResponse)webReq.GetResponse();
                HttpWebResponse webResp;
                try
                {
                    webResp = (HttpWebResponse)webReq.GetResponse();
                }
                catch (WebException ex)
                {
                    webResp = (HttpWebResponse)ex.Response;
                }


                Stream answer = webResp.GetResponseStream();
                StreamReader answerData = new StreamReader(answer);
                returnData = answerData.ReadToEnd();
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                return ex.Message;
            }
            return returnData.Trim() + "\n";




        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {   // 总是接受  
            return true;
        }
        public string HttpGet(string PageUrl)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                WebRequest request = WebRequest.Create(PageUrl);
                WebResponse response = request.GetResponse();
                Stream resStream = response.GetResponseStream();
                Encoding encode = Encoding.GetEncoding("utf-8");
                //Encoding encode = Encoding.GetEncoding("gb2312");
                StreamReader sr = new StreamReader(resStream, encode);
                string retstr = sr.ReadToEnd();
                resStream.Close();
                sr.Close();
                return retstr;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        #endregion


        #region 微博开放平台(http://open.t.sina.com.cn/wiki/%E5%BE%AE%E5%8D%9AAPI) 短链接
        public string ToShortUrl(string longurl)
        {
            try
            {
                string jsondata = new GetUrlData().HttpGet("https://api.weibo.com/2/short_url/shorten.json?source=2849184197&url_long=" + longurl);
                //{"urls":[{"result":true,"url_short":"http://t.cn/h5FGy","url_long":"http://www.cnblogs.com","type":0}]}
                XmlDocument retdoc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + jsondata + "}");
                XmlElement retroot = retdoc.DocumentElement;
                XmlNodeList xnodes = retroot.ChildNodes;
                string result = xnodes[0].SelectSingleNode("result").InnerText;
                string url_short = xnodes[0].SelectSingleNode("url_short").InnerText;
                return url_short;
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region 模拟http 发送Post请求 传递Json内容
        public string HttpPostJson(string url, string data)
        {
            string returnData = null;
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(url);
                webReq.Method = "POST";
                webReq.ContentType = "application/json";
                webReq.Accept = "application/json";
                webReq.ContentLength = buffer.Length;
                Stream postData = webReq.GetRequestStream();
                postData.Write(buffer, 0, buffer.Length);
                postData.Close();
                //HttpWebResponse webResp = (HttpWebResponse)webReq.GetResponse();
                HttpWebResponse webResp;
                try
                {
                    webResp = (HttpWebResponse)webReq.GetResponse();
                }
                catch (WebException ex)
                {
                    webResp = (HttpWebResponse)ex.Response;
                }


                Stream answer = webResp.GetResponseStream();
                StreamReader answerData = new StreamReader(answer);
                returnData = answerData.ReadToEnd();
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                return ex.Message;
            }
            return returnData.Trim() + "\n";




        }
        #endregion

        #region  模拟网络Post请求 备用1
        private const string sContentType = "application/x-www-form-urlencoded";
        private const string sUserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        public static string Send(string data, string url)
        {
            return Send(Encoding.GetEncoding("UTF-8").GetBytes(data), url);
        }

        public static string Send(byte[] data, string url)
        {
            Stream responseStream;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null)
            {

                throw new ApplicationException(string.Format("Invalid url string: {0}", url));
            }
            // request.UserAgent = sUserAgent; 
            request.ContentType = sContentType;
            request.Method = "POST";
            request.ContentLength = data.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
            try
            {
                responseStream = request.GetResponse().GetResponseStream();
            }
            catch (Exception exception)
            {

                throw exception;
            }
            string str = string.Empty;
            using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
            {
                str = reader.ReadToEnd();
            }
            responseStream.Close();
            return str;
        }
        #endregion

        #region 模拟网络Get Post请求 备用2
        /// <summary> 
        /// 通过POST方式发送数据 
        /// </summary> 
        /// <param name="Url">url</param> 
        /// <param name="postDataStr">Post数据</param> 
        /// <param name="cookie">Cookie容器</param> 
        /// <returns></returns> 
        public string SendDataByPost(string Url, string postDataStr, ref CookieContainer cookie)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            if (cookie.Count == 0)
            {
                request.CookieContainer = new CookieContainer();
                cookie = request.CookieContainer;
            }
            else
            {
                request.CookieContainer = cookie;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postDataStr.Length;
            //request.Timeout = 1000; 
            //request.ReadWriteTimeout = 3000; 
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("gb2312"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }


        /// <summary> 
        /// 通过GET方式发送数据 
        /// </summary> 
        /// <param name="Url">url</param> 
        /// <param name="postDataStr">GET数据</param> 
        /// <param name="cookie">Cookie容器</param> 
        /// <returns></returns> 
        public string SendDataByGET(string Url, string postDataStr, ref CookieContainer cookie)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            if (cookie.Count == 0)
            {
                request.CookieContainer = new CookieContainer();
                cookie = request.CookieContainer;
            }
            else
            {
                request.CookieContainer = cookie;
            }
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
        #endregion

        #region 模拟网络Get Post请求 备用3
        ///<summary> 
        ///采用get发送请求 
        ///</summary> 
        ///<param name="URL">url地址</param> 
        ///<param name="getdata">发送的数据</param> 
        ///<returns></returns> 
        public string zzget(string Url, string getdata, string type)
        {
            try
            {
                System.Net.WebRequest wReq = System.Net.WebRequest.Create(Url + (getdata == "" ? "" : "?") + getdata);
                // Get the response instance. 
                wReq.Method = "GET";
                wReq.ContentType = "text/html;charset=UTF-8";
                System.Net.WebResponse wResp = wReq.GetResponse();
                System.IO.Stream respStream = wResp.GetResponseStream();
                // Dim reader As StreamReader = New StreamReader(respStream) 
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding(type)))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (System.Exception ex)
            {
                //errorMsg = ex.Message; 
            }
            return "";
        }

        ///<summary> 
        ///采用post发送请求 
        ///</summary> 
        ///<param name="URL">url地址</param> 
        ///<param name="strPostdata">发送的数据</param> 
        ///<returns></returns> 
        public string zzpost(string URL, IDictionary<string, Object> strPostdata, string strEncoding)
        {

            //IDictionary<string, Object> idc = new Dictionary<string, object>(); 
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, Object> param in strPostdata)
            {
                data.Append(param.Key).Append("=");
                data.Append(param.Value.ToString());
                data.Append("&");
            }
            data.Remove(data.Length - 1, 1);
            Encoding encoding = Encoding.Default;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.CookieContainer = new CookieContainer();//少了这句就不能登录 
            request.Method = "post";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] buffer = encoding.GetBytes(data.ToString());
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            /* 
            request.ContentLength = data.Length; 
            Stream myRequestStream = request.GetRequestStream(); 
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312")); 
            myStreamWriter.Write(data); 
            myStreamWriter.Close(); 
            */
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding(strEncoding)))
            {
                return reader.ReadToEnd();
            }

        }

        #endregion




    }
}

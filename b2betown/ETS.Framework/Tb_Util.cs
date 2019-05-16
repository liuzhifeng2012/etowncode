﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Security.Cryptography;
using System.Web;


namespace ETS.Framework
{
    #region .net调用示例
    //using System;
    //using System.Collections.Generic;
    //using System.Text;
    //namespace APITest
    //{
    //    class Program
    //    {
    //        static void Main(string[] args)
    // {
    //      string appkey = "test";
    //      string secret = "test";
    //      string session = "test";

    //      IDictionary<string, string> param = new Dictionary<string, string>();
    //      param.Add("fields", "user_id,nick,type");
    //     // param.Add("nick", "sandbox_c_1");

    //      //Util.Post 集成了 系统参数 和 计算 sign的方法
    //      Console.WriteLine("返回结果：" + Tb_Util.Post("http://gw.api.tbsandbox.com/router/rest", :appkey, secret, "taobao.user.seller.get", session, param));
    //      Console.ReadKey();
    // }
    //    }
    //}
    #endregion

    /// <summary>
    /// 淘宝码商接口公共类
    /// </summary>
    public class Tb_Util
    {
        /// <summary>
        /// 给TOP请求签名 API v2.0
        /// </summary>
        /// <param name="parameters">所有字符型的TOP请求参数</param>
        /// <param name="secret">签名密钥</param>
        /// <returns>签名</returns>
        public static string CreateSign(IDictionary<string, string> parameters, string secret)
        {
            parameters.Remove("sign");

            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            StringBuilder query = new StringBuilder(secret);
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append(value);
                }
            }
            query.Append(secret);

            MD5 md5 = MD5.Create();
            //byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
            byte[] bytes = md5.ComputeHash(Encoding.GetEncoding("GBK").GetBytes(query.ToString()));

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                string hex = bytes[i].ToString("X");
                if (hex.Length == 1)
                {
                    result.Append("0");
                }
                result.Append(hex);
            }

            return result.ToString();
        }


        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public  static string PostData(IDictionary<string, string> parameters)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;

            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }

                    postData.Append(name);
                    postData.Append("=");
                    postData.Append(Uri.EscapeDataString(value));
                    hasParam = true;
                }
            }

            return postData.ToString();
        }


        /// <summary>
        /// TOP API POST 请求
        /// </summary>
        /// <param name="url">请求容器URL</param>
        /// <param name="appkey">AppKey</param>
        /// <param name="appSecret">AppSecret</param>
        /// <param name="method">API接口方法名</param>
        /// <param name="session">调用私有的sessionkey</param>
        /// <param name="param">请求参数</param>
        /// <returns>返回字符串</returns>
        public static string Post(string url, string appkey, string appSecret, string method, string session,

        IDictionary<string, string> param)
        {

            #region -----API系统参数----

            param.Add("app_key", appkey);
            param.Add("method", method);
            param.Add("session", session);
            param.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            param.Add("format", "xml");
            param.Add("v", "2.0");
            param.Add("sign_method", "md5");
            param.Add("sign", CreateSign(param, appSecret));

            #endregion

            string result = string.Empty;

            #region ---- 完成 HTTP POST 请求----

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.KeepAlive = true;
            req.Timeout = 300000;
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            byte[] postData = Encoding.UTF8.GetBytes(PostData(param));
            Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();
            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
            Stream stream = null;
            StreamReader reader = null;
            stream = rsp.GetResponseStream();
            reader = new StreamReader(stream, encoding);
            result = reader.ReadToEnd();
            if (reader != null) reader.Close();
            if (stream != null) stream.Close();
            if (rsp != null) rsp.Close();
            #endregion
            return Regex.Replace(result, @"[\x00-\x08\x0b-\x0c\x0e-\x1f]", "");
        }

         
    }
}

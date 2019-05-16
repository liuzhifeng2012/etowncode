using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using System.Xml;
using Newtonsoft.Json;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using System.Web;
using ETS.Framework;

namespace ETS2.WeiXin.Service.WinXinService.BLL
{
    public class WeixinVoiceUtil
    {
        // <summary>
        /// 获取jsapi_ticket
        /// jsapi_ticket是公众号用于调用微信JS接口的临时票据。
        /// 正常情况下，jsapi_ticket的有效期为7200秒，通过access_token来获取。
        /// 由于获取jsapi_ticket的api调用次数非常有限，频繁刷新jsapi_ticket会导致api调用受限，影响自身业务，开发者必须在自己的服务全局缓存jsapi_ticket 。
        /// </summary>
        /// <param name="access_token">BasicAPI获取的access_token,也可以通过TokenHelper获取</param>
        /// <returns></returns>
        public string GetTickect(string access_token, int comid)
        {
            try
            {
                if (HttpRuntime.Cache["jsapiticket" + comid] == null)
                {
                    var url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", access_token);
                    string jsonText = new GetUrlData().HttpGet(url);

                    XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + jsonText + "}");

                    XmlElement rootElement = doc.DocumentElement;
                    string jsTicket = rootElement.SelectSingleNode("ticket").InnerText;

                    //有效期7200秒，开发者必须在自己的服务全局缓存jsapi_ticket
                    HttpRuntime.Cache.Insert("jsapiticket" + comid, jsTicket, null, DateTime.Now.AddHours(2), System.Web.Caching.Cache.NoSlidingExpiration); //HttpRuntime.Cache与HttpContext.Current.Cache是同一对象,建议使用HttpRuntime.Cache

                    return jsTicket;
                }
                else
                {
                    return System.Web.HttpRuntime.Cache.Get("jsapiticket" + comid).ToString();
                }
            }
            catch 
            {
                return "";
            }

        }


        /// <summary>
        /// 创建随机字符串
        /// </summary>
        /// <returns></returns>
        public string CreatenNonce_str()
        {
            Random r = new Random();
            var sb = new StringBuilder();
            var length = strs.Length;
            for (int i = 0; i < 15; i++)
            {
                sb.Append(strs[r.Next(length - 1)]);
            }
            return sb.ToString();
        }
        public string[] strs = new string[]
        {
        "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
        "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
        };
        /// <summary>
        /// 创建时间戳
        /// </summary>
        /// <returns></returns>
        public long CreatenTimestamp()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
        /// <summary>
        /// 签名算法
        /// </summary>
        /// <param name="jsapi_ticket">jsapi_ticket</param>
        /// <param name="noncestr">随机字符串(必须与wx.config中的nonceStr相同)</param>
        /// <param name="timestamp">时间戳(必须与wx.config中的timestamp相同)</param>
        /// <param name="url">当前网页的URL，不包含#及其后面部分(必须是调用JS接口页面的完整URL)</param>
        /// <returns></returns>
        public string GetSignature(string jsapi_ticket, string noncestr, long timestamp, string url, out string string1)
        {
            var string1Builder = new StringBuilder();
            string1Builder.Append("jsapi_ticket=").Append(jsapi_ticket).Append("&")
             .Append("noncestr=").Append(noncestr).Append("&")
             .Append("timestamp=").Append(timestamp).Append("&")
             .Append("url=").Append(url.IndexOf("#") >= 0 ? url.Substring(0, url.IndexOf("#")) : url);
            string1 = string1Builder.ToString();
            return Com.Tenpay.SHA1Util.Sha1(string1);
        }

        /// <summary>
        /// 获取凭证（首先判断数据库是否存在凭证以及是否过期，如过期，重新获取）
        /// </summary>
        /// <returns></returns>
        public WXAccessToken GetAccessToken(int comid, string AppId, string AppSecret)
        {
            try
            {
                DateTime fitcreatetime = DateTime.Now.AddHours(-2);
                WXAccessToken token = new WXAccessTokenData().GetLaststWXAccessToken(fitcreatetime, comid);
                if (token == null)
                {
                    string geturl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppId + "&secret=" + AppSecret;
                    string jsonText = new GetUrlData().HttpGet(geturl);

                    XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + jsonText + "}");

                    XmlElement rootElement = doc.DocumentElement;
                    string access_token = rootElement.SelectSingleNode("access_token").InnerText;

                    //把获取到的凭证录入数据库中
                    token = new WXAccessToken()
                    {
                        Id = 0,
                        ACCESS_TOKEN = access_token,
                        CreateDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        Comid = comid

                    };
                    int edittoken = new WXAccessTokenData().EditAccessToken(token);
                }
                return token;
            }
            catch
            {
                return null;
            }
        }
    }
}

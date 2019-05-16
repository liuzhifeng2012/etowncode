using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using ETS.Framework;
using System.Web.Script.Serialization;

namespace ETS2.WeiXin.Service.WinXinService.BLL.WeiXinUploadDown
{
    /// <summary>
    /// 微信上传下载多媒体信息
    /// </summary>
    public class WxUploadDownManage
    {
        /// <SUMMARY> 
        /// 下载保存多媒体文件,返回多媒体下载状态
        /// </SUMMARY> 
        /// <PARAM name="ACCESS_TOKEN"></PARAM> 
        /// <PARAM name="MEDIA_ID"></PARAM> 
        /// <RETURNS></RETURNS> 
        public string GetMultimedia(string ACCESS_TOKEN, string MEDIA_ID, string savepath)
        {
            string issuc = "0";
            string content = string.Empty;
            string strpath = string.Empty;
            string stUrl = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=" + ACCESS_TOKEN + "&media_id=" + MEDIA_ID;

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(stUrl);

            req.Method = "GET";
            using (WebResponse wr = req.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();

                strpath = myResponse.ResponseUri.ToString();
                //TxtHelper.CheckLog("接收类别://" + myResponse.ContentType, "D:\\wxuploadlog.txt");
                WebClient mywebclient = new WebClient();
                ////savepath = System.Web.HttpContext.Current.Server.MapPath("image") + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next().ToString().Substring(0, 4) + ".jpg";
                //TxtHelper.CheckLog("路径://" + savepath, "D:\\wxuploadlog.txt");
                try
                {
                    mywebclient.DownloadFile(strpath, savepath);
                    issuc = "1";
                }
                catch (Exception ex)
                {
                    string errstr = ex.ToString();
                    issuc = "0";
                }

            }
            return issuc;
        }

        /// <SUMMARY> 
        /// 上传多媒体文件,返回 MediaId 
        /// </SUMMARY> 
        /// <PARAM name="ACCESS_TOKEN"></PARAM> 
        /// <PARAM name="Type"></PARAM> 
        /// <RETURNS></RETURNS> 
        public string UploadMultimedia(string ACCESS_TOKEN, string Type, string filepath)
        {
            string result = "";
            string wxurl = "http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token=" + ACCESS_TOKEN + "&type=" + Type;
            //string filepath = System.Web.HttpContext.Current.Server.MapPath("image") + "\\hemeng80.jpg"; //(本地服务器的地址)
            //string filepath = "D:\\site\\b2betown\\ETS2.WebApp\\UploadFile\\2014060012433370369.jpg";
            //TxtHelper.CheckLog("上传路径:" + filepath, "D:\\logg.txt");
            WebClient myWebClient = new WebClient();
            myWebClient.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                byte[] responseArray = myWebClient.UploadFile(wxurl, "POST", filepath);
                result = System.Text.Encoding.Default.GetString(responseArray, 0, responseArray.Length);
                //TxtHelper.CheckLog("上传result:" + result, "D:\\wxuploadlog.txt");
                UploadMM _mode = JsonConvert.DeserializeObject<UploadMM>(result);
                result = _mode.media_id;
                //TxtHelper.CheckLog("上传MediaId:" + result, "D:\\wxuploadlog.txt");
            }
            catch (Exception ex)
            {
                result = "";
                //TxtHelper.CheckLog("上传MediaId失败:" + ex.Message, "D:\\wxuploadlog.txt");
            }
            return result;
        }
        /// <summary>
        /// 上传图文消息素材(微信群发)，返回media_id
        /// </summary>
        /// <param name="articles"></param>
        /// <returns></returns>
        public string Uploadnews(string ACCESS_TOKEN, List<Wxarticle> articles, out string created_at)
        {

            if (articles == null)
            {
                created_at = "";
                return "";
            }
            if (articles.Count == 0)
            {
                created_at = "";
                return "";
            }
            string requesturl = "https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token=" + ACCESS_TOKEN;

            Wxarticles obj = new Wxarticles();
            obj.articles = articles;

            string json_param = JsonConvert.SerializeObject(obj);

            #region 
            //System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();

            //StringBuilder json_param = new StringBuilder();
            //jss.Serialize(obj, json_param);

            //string json_param = "{\"articles\": [";
            //foreach (Wxarticle m in articles)
            //{
            //    json_param += "{" +
            //            "\"thumb_media_id\":\"" + m.thumb_media_id + "\"," +
            //            "\"author\":\"" + m.author + "\"," +
            //             "\"title\":\"" + m.title + "\"," +
            //             "\"content_source_url\":\"" + m.content_source_url + "\"," +
            //             "\"content\":\"" + m.content + "\"," +
            //             "\"digest\":\"" + m.digest + "\"," +
            //            "\"show_cover_pic\":\"" + m.show_cover_pic + "\"" +
            //            "},";
            //}
            //json_param = json_param.Substring(0, json_param.Length - 1);
            //json_param += "]}";
            #endregion
            try
            {
                string ret = new GetUrlData().HttpPost(requesturl, json_param.ToString());
                JavaScriptSerializer ser = new JavaScriptSerializer();
                UploadMM foo = ser.Deserialize<UploadMM>(ret);
                created_at = foo.created_at;

                return foo.media_id;
            }
            catch 
            {
                created_at = "";
                return "";
            }
        }

        /// <summary>
        /// 微信图文群发
        /// </summary>
        /// <param name="tousernames"></param>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public string Wxqunfanews(string ACCESS_TOKEN, string[] tousernames, string media_id)
        {
            if (ACCESS_TOKEN == "")
            {
                return "ACCESS_TOKEN为空";
            }
            if (tousernames.Length <= 0)
            {
                return "请选择群发用户";
            }
            if (media_id == "")
            {
                return "请选择图文消息";
            }
            string requesturl = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token=" + ACCESS_TOKEN;

            string json_param = "{\"touser\":[";
            foreach (string m in tousernames)
            {
                if (m != "")
                {
                    json_param += "\"" + m + "\",";
                }
            }
            json_param = json_param.Substring(0, json_param.Length - 1);
            json_param += " ]," +
                            "\"mpnews\":{" +
                               "\"media_id\":\"" + media_id + "\"" +
                             "}," +
                             "\"msgtype\":\"mpnews\"" +
                            "}";

            string ret = new GetUrlData().HttpPost(requesturl, json_param);
            JavaScriptSerializer ser = new JavaScriptSerializer();
            WxQunfaMM foo = ser.Deserialize<WxQunfaMM>(ret);

            return foo.errcode;
        }


        /// <summary>
        /// 得到微信端返回消息实体
        /// </summary>
        /// <returns></returns>
        public Wxmessage GetWxMessage()
        {
            Wxmessage wx = new Wxmessage();
            StreamReader str = new StreamReader(System.Web.HttpContext.Current.Request.InputStream, System.Text.Encoding.UTF8);
            XmlDocument xml = new XmlDocument();
            xml.Load(str);
            wx.ToUserName = xml.SelectSingleNode("xml").SelectSingleNode("ToUserName").InnerText;
            wx.FromUserName = xml.SelectSingleNode("xml").SelectSingleNode("FromUserName").InnerText;
            wx.MsgType = xml.SelectSingleNode("xml").SelectSingleNode("MsgType").InnerText;
            if (wx.MsgType.Trim() == "text")
            {
                wx.Content = xml.SelectSingleNode("xml").SelectSingleNode("Content").InnerText;
            }
            if (wx.MsgType.Trim() == "event")
            {
                wx.EventName = xml.SelectSingleNode("xml").SelectSingleNode("Event").InnerText;
                wx.EventKey = xml.SelectSingleNode("xml").SelectSingleNode("EventKey").InnerText;
            }
            if (wx.MsgType.Trim() == "voice")
            {
                wx.Recognition = xml.SelectSingleNode("xml").SelectSingleNode("Recognition").InnerText;
            }
            if (wx.MsgType.Trim() == "image")
            {
                wx.MediaId = xml.SelectSingleNode("xml").SelectSingleNode("MediaId").InnerText;
            }

            return wx;
        }

    }
    /// <summary>
    /// 上传多媒体文件后返回信息实体类
    /// </summary>
    public class UploadMM
    {
        public string type { get; set; }
        public string media_id { get; set; }
        public string created_at { get; set; }
    }
    /// <summary>
    /// 群发消息后返回信息实体类
    /// </summary>
    public class WxQunfaMM
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public string msg_id { get; set; }
    }
}

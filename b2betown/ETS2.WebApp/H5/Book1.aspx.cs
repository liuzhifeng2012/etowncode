using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using System.Xml;
using Newtonsoft.Json;
using ETS2.WeiXin.Service.WinXinService.BLL;

namespace ETS2.WebApp.H5
{
    public partial class Book1 : System.Web.UI.Page
    {
        public string openid = "";//微信号
        public int uid = 0;//会员id
        public string weixinpass = "";//一次性密码
        public int comid = 0;
        public int clientuptypemark = 0;//下载 微信客户端上传类型标记

        public string appId = "";
        public long timestamp = 0;
        public string nonceStr = "";
        public string signature = "";//jsapi签名

        public int isrightwxset = 1;//微信接口设置信息是否正确
        protected void Page_Load(object sender, EventArgs e)
        {
            openid = Request["openid"].ConvertTo<string>("");
            uid = Request["uid"].ConvertTo<int>(0);
            weixinpass = Request["weixinpass"].ConvertTo<string>("");
            comid = Request["comid"].ConvertTo<int>(0);
            clientuptypemark = Request["clienttypemark"].ConvertTo<int>(0);

            #region  微信语音播放接口会用到
            string url = Request.Url.ToString();

            //根据传入参数openid、comid 得到 access_token、jsapi_ticket、noncestr、timestamp、 url（当前网页的URL，不包含#及其后面部分）
            WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(comid);
            if (basic != null)
            {
                appId = basic.AppId;
                timestamp = new WeixinVoiceUtil().CreatenTimestamp();
                nonceStr = new WeixinVoiceUtil().CreatenNonce_str();

                WXAccessToken maccesstoken = new WeixinVoiceUtil().GetAccessToken(comid, basic.AppId, basic.AppSecret);
                if (maccesstoken != null)
                {
                    string jsapi_ticket = new WeixinVoiceUtil().GetTickect(maccesstoken.ACCESS_TOKEN, comid);
                    if (jsapi_ticket == "")
                    {
                        isrightwxset = 0;
                    }
                    else
                    {
                        string beforesha1_signature = "";
                        signature = new WeixinVoiceUtil().GetSignature(jsapi_ticket, nonceStr, timestamp, url, out beforesha1_signature);

                    }
                }
                else
                {
                    isrightwxset = 0;
                }
            }
            else
            {
                isrightwxset = 0;
            }
            #endregion
            #region 注释掉
            //string str = "260,110";
            //int strnum = str.IndexOf(',', 0);
            //string a = str.Substring(0, strnum);
            //int d = str.Length;
            //string b = str;

            //string f = str.Substring(strnum+1, str.Length-strnum);

            //string c = a + b;
            #endregion
        } 
    }
}
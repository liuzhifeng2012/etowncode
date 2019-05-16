using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WinXinService.BLL;

namespace ETS2.WebApp.WeiXin
{
    public partial class WxZixunReplay : System.Web.UI.Page
    {
        public string userweixin = "";
        public string guwenweixin = "";
        public int comid = 0;

        public string appId = "";
        public long timestamp = 0;
        public string nonceStr = "";
        public string signature = "";//jsapi签名


        public int isrightwxset = 1;//微信接口设置信息是否正确

        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        protected void Page_Load(object sender, EventArgs e)
        {
            userweixin = Request["userweixin"].ConvertTo<string>("");
            guwenweixin = Request["guwenweixin"].ConvertTo<string>("");



            if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式，则从多微信商户基本信息表中获取comid
            {
                //先通过正则表达式获取COMid
                comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl).ToString());
            }
            if (comid == 0)//如果非标准格式，查询 是否有绑定的域名
            {
                var domaincomid = B2bCompanyData.GetComId(RequestUrl);
                if (domaincomid != null)
                {
                    comid = domaincomid.Com_id;
                } 
            }
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



        }
    }
}
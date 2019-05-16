using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.WeiXin.Service.WinXinService.BLL;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.WebApp.UI.PMUI.ETicket
{
    public partial class mETicketIndex : System.Web.UI.Page
    {
        public string appId = "";
        public long timestamp = 0;
        public string nonceStr = "";
        public string signature = "";//jsapi签名

        public int isrightwxset = 1;//微信接口设置信息是否正确

        public int comid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserHelper.ValidateLogin())
            {
                B2b_company company = UserHelper.CurrentCompany;
                comid = company.ID;
            }
            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "001");
            #region  微信扫描二维码，都用易城微信号的
            comid = 106;
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
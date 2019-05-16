using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Common.Business;
using FileUpload.FileUpload.Data;
using ETS.JsonFactory;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Member.Service.MemberService.Model;
using System.Xml;
using Newtonsoft.Json;

namespace ETS2.WebApp.H5.Hotel
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        #region 辨别商家参数
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        public int comid = 0;
        #endregion
        public string nowdate = "";//现在日期
        public string enddate = "";//离店日期

        public string phone = "";

        public string comName = "";

        public string headPortraitImgSrc = "";

        public string remark = "";
        public int comid_temp = 0;
        public int comidture = 0;

        //--------------新添加的公共参数---------------//
        public B2b_crm userinfo = new B2b_crm();

        public string openid = "";
        public string weixinpass = "";

        public string address = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            bool bo = detectmobilebrowser.HttpUserAgent(u);

            nowdate = DateTime.Now.ToString("yyyy-MM-dd");
            enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

            comid = Request["id"].ConvertTo<int>(0);
            if (comid == 0)
            {
                if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式，则从多微信商户基本信息表中获取comid
                {
                    comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl));
                }
                else
                {
                    //if (bo == false)
                    //{
                    //    Response.Redirect("http://vctrip.etown.cn/");
                    //}
                    B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestUrl);
                    if (companyinfo != null)
                    {
                        comid = companyinfo.Com_id;
                        //if (bo == false)
                        //{
                        //    if (comid == 101)
                        //    {
                        //        Response.Redirect("http://vctrip.etown.cn/");
                        //    }
                        //    Response.Redirect("http://shop" + comid + ".etown.cn");
                        //}
                    }
                }
            }

            //判断是否为平台。只有106,101
            if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式，则从多微信商户基本信息表中获取comid
            {
                comid_temp = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl));

            }
            else
            {
                B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestUrl);
                if (companyinfo != null)
                {
                    comid_temp = companyinfo.Com_id;
                }
            }

            if (comid_temp == 101 || comid_temp == 106)
            {
                comidture = 1;
            }

            if (comid != 0)
            {
                //if (bo == false)
                //{
                //    if (comid == 101)
                //    {
                //        Response.Redirect("http://vctrip.etown.cn/");
                //    }
                //    Response.Redirect("http://shop" + comid + ".etown.cn");
                //}

                B2b_company companyinfo = B2bCompanyData.GetCompany(comid);

                B2b_company_info info = new B2bCompanyInfoData().GetCompanyInfo(comid);
                if (companyinfo != null)
                {
                    comName = companyinfo.Com_name;
                    if (comName.Length >= 9)
                    {
                        comName = comName.Substring(0, 9) + "..";
                    }

                    remark = info.Scenic_intro;
                    if (remark.Length > 42)
                    {
                        remark = remark.Substring(0, 42) + "&nbsp;&nbsp;<a style=\"color:#1a9ed9;font-size:13px;\" href=\"HotelIntroduction.aspx?id=" + comid + "\">(查看全部)</a>";
                    }
                    address = info.Scenic_address;
                }

                B2b_company_saleset saleset = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
                if (saleset != null)
                {
                    phone = saleset.Service_Phone;
                    if (saleset.Smalllogo != null && saleset.Smalllogo != "")
                    {
                        headPortraitImgSrc = FileSerivce.GetImgUrl(saleset.Smalllogo.ConvertTo<int>(0));

                    }
                    
                }

            }


            //------------------------------新添加代码(主要目的就是获取微信号使用户处于登录状态以及判断用户转发时引发的微信号混乱问题)===========================

            string weixincode = Request["code"].ConvertTo<string>("");
            if (weixincode == "")//未认证微信
            {
                openid = Request["openid"].ConvertTo<string>("");
                weixinpass = Request["weixinpass"].ConvertTo<string>("");
                int questtype = 1;//1=一次性密码验证
                DealUserinfo1(openid, weixinpass, comid, questtype);//判断用户微信号(1,点击的转发链接进来的2，点击微信菜单进来的)；使用户处于登录状态（不包括点击转发链接的）
            }
            else//已认证微信
            {
                int questtype = 2;//1=微信授权验证
                DealUserinfo1("", weixincode, comid, questtype);//获取微信号；使用户处于登录状态
            }
        }

        public void DealUserinfo1(string openid, string weixinpass, int comid, int questtype)
        {

            if (Request.Cookies["AccountId"] != null)
            {
                string accountmd5 = "";
                int AccountId = int.Parse(Request.Cookies["AccountId"].Value);
                if (Request.Cookies["AccountKey"] != null)
                {
                    accountmd5 = Request.Cookies["AccountKey"].Value;
                }

                var data = CrmMemberJsonData.WeixinCookieLogin(AccountId.ToString(), accountmd5, comid, out userinfo);
                if (data != "OK")
                {
                    //当cookie错误无法登陆则清除所有COOKIE；
                    HttpCookie aCookie;
                    string cookieName;
                    int limit = Request.Cookies.Count;
                    for (int i = 0; i < limit; i++)
                    {
                        cookieName = Request.Cookies[i].Name;
                        aCookie = new HttpCookie(cookieName);
                        aCookie.Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies.Add(aCookie);
                    }

                    if (questtype == 1)
                    {
                        VerifyOneOffPass(openid, weixinpass);//重新验证一下是否是正确的一次性密码
                    }
                    else if (questtype == 2)
                    {
                        GetOpenId(weixinpass, comid);//微信授权验证
                    }
                }
                else
                {
                    //从cookie中得到微信号
                    if (Request.Cookies["openid"] != null)
                    {
                        openid = Request.Cookies["openid"].Value;
                    }

                    B2bCrmData dateuser = new B2bCrmData();
                    dateuser.WeixinConPass(openid, comid);//清空微信密码

                }
            }
            else
            {
                if (questtype == 1)
                {
                    VerifyOneOffPass(openid, weixinpass);//重新验证一下是否是正确的一次性密码
                }
                else if (questtype == 2)
                {
                    GetOpenId(weixinpass, comid);//微信授权验证
                }
            }
        }

        private void VerifyOneOffPass(string openid, string weixinpass)
        {
            if (openid != null && openid != "" && weixinpass != "" && weixinpass != null)
            {
                B2bCrmData dateuser = new B2bCrmData();
                string data = CrmMemberJsonData.WeixinLogin(openid, weixinpass, comid, out userinfo);

                if (data == "OK")
                {
                    HttpCookie cookie = new HttpCookie("AccountId");     //实例化HttpCookie类并添加值
                    cookie.Value = userinfo.Id.ToString();
                    cookie.Expires = DateTime.Now.AddDays(120);
                    Response.Cookies.Add(cookie);
                    var returnmd5 = EncryptionHelper.ToMD5(userinfo.Idcard.ToString() + userinfo.Id.ToString(), "UTF-8");
                    cookie = new HttpCookie("AccountKey");     //实例化HttpCookie类并添加值
                    cookie.Value = returnmd5;
                    cookie.Expires = DateTime.Now.AddDays(120);
                    Response.Cookies.Add(cookie);

                    cookie = new HttpCookie("openid");     //实例化HttpCookie类并添加值
                    cookie.Value = openid;
                    cookie.Expires = DateTime.Now.AddDays(120);
                    Response.Cookies.Add(cookie);
                }

                dateuser.WeixinConPass(openid, comid);//清空微信密码
            }
        }

        private void GetOpenId(string codee, int comid)
        {
            WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);


            if (basicc != null)
            {

                string st = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + basicc.AppId + "&secret=" + basicc.AppSecret + "&code=" + codee + "&grant_type=authorization_code";
                XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + new GetUrlData().HttpGet(st) + "}");

                XmlElement rootElement = doc.DocumentElement;

                string openid = rootElement.SelectSingleNode("openid").InnerText;

                //根据微信号获取用户信息，使用户处于登录状态
                string data = new B2bCrmData().GetB2bCrm(openid, comid, out userinfo);
                if (data == "OK")
                {
                    HttpCookie cookie = new HttpCookie("AccountId");     //实例化HttpCookie类并添加值
                    cookie.Value = userinfo.Id.ToString();
                    cookie.Expires = DateTime.Now.AddDays(120);
                    Response.Cookies.Add(cookie);
                    var returnmd5 = EncryptionHelper.ToMD5(userinfo.Idcard.ToString() + userinfo.Id.ToString(), "UTF-8");
                    cookie = new HttpCookie("AccountKey");     //实例化HttpCookie类并添加值
                    cookie.Value = returnmd5;
                    cookie.Expires = DateTime.Now.AddDays(120);
                    Response.Cookies.Add(cookie);

                    cookie = new HttpCookie("openid");     //实例化HttpCookie类并添加值
                    cookie.Value = openid;
                    cookie.Expires = DateTime.Now.AddDays(120);
                    Response.Cookies.Add(cookie);
                }
            }

        }

    }
}
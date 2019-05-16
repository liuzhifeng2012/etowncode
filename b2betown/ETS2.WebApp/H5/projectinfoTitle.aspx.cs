using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Common.Business;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;
using System.Xml;
using Newtonsoft.Json;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;
using ETS.JsonFactory;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.WebApp.H5
{
    public partial class projectinfoTitle : System.Web.UI.Page
    {
        public int comid = 0;
        public string title = "";
        public string article = "";
        public string merchant_intro = "";//商家介绍
        public string serviceinfo = "";
        public string Scenic_Takebus = "";
        public string Scenic_Drivingcar = "";
        public B2b_crm userinfo = new B2b_crm();

        public int projectid = 0;//项目id
        public string projectname = "";//项目名称
        public string projectbrief = "";//项目精简介绍
        public string projectserviceintroduce = "";//项目服务介绍

        protected void Page_Load(object sender, EventArgs e)
        {
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            bool bo = detectmobilebrowser.HttpUserAgent(u);


            if (comid == 0)
            {
                string RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();
                if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式，则从多微信商户基本信息表中获取comid
                {
                    //先通过正则表达式获取COMid
                    comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl).ToString());
                    if (comid == 0)
                    {
                        comid = new WeiXinBasicData().GetWeiXinBasicByDomain(RequestUrl).Comid;
                    }
                }
                else
                {
                    B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestUrl);
                    if (companyinfo != null)
                    {
                        comid = companyinfo.Com_id;
                    }
                }

            }
            //根据项目id得到项目信息
            projectid = Request["id"].ConvertTo<int>(0);
            B2b_com_project mod = new B2b_com_projectData().GetProject(projectid, comid);
            if (mod != null)
            {
                projectname = mod.Projectname;
                projectbrief = mod.Briefintroduce;
                projectserviceintroduce = mod.Serviceintroduce;
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
                B2b_company_info info = new B2bCompanyInfoData().GetCompanyInfo(comid);

                B2b_company com = B2bCompanyData.GetCompany(comid);
                if (com != null)
                {
                    title = com.Com_name;
                }
                if (info != null)
                {
                    article = info.Scenic_intro;
                    merchant_intro = info.Merchant_intro;
                    if (merchant_intro == "")//如果商家介绍为空的话，赋值景区介绍
                    {
                        merchant_intro = info.Scenic_intro;
                    }
                    serviceinfo = info.Serviceinfo;
                    Scenic_Takebus = info.Scenic_Takebus;
                    Scenic_Drivingcar = info.Scenic_Drivingcar;
                }
            }


            if (comid != 0)
            {
                //获取微信平台端code
                string weixincode = Request["code"].ConvertTo<string>("");

                if (weixincode != "")
                {
                    int questtype = 2;//1=微信授权验证
                    DealUserinfo1("", weixincode, comid, questtype);
                }
                else
                {
                    string openid = Request["openid"].ConvertTo<string>("");
                    string weixinpass = Request["weixinpass"].ConvertTo<string>("");
                    int questtype = 1;//1=一次性密码验证
                    DealUserinfo1(openid, weixinpass, comid, questtype);//判断用户微信号(1,点击的转发链接进来的2，点击微信菜单进来的)；使用户处于登录状态（不包括点击转发链接的）

                }
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
                        openid = GetOpenId(weixinpass, comid);//微信授权验证
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
                    if (userinfo != null)
                    {
                        Readuser(userinfo.Idcard, comid);//读取用户信息
                    }
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
                    {
                        openid = GetOpenId(weixinpass, comid);//微信授权验证
                    }
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
                    if (userinfo != null)
                    {
                        Readuser(userinfo.Idcard, comid);//读取用户信息
                    }
                }

                dateuser.WeixinConPass(openid, comid);//清空微信密码
            }
        }

        private string GetOpenId(string codee, int comid)
        {


            string openid = "";
            WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);
            if (basicc != null)
            {

                string st = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + basicc.AppId + "&secret=" + basicc.AppSecret + "&code=" + codee + "&grant_type=authorization_code";
                XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + new GetUrlData().HttpGet(st) + "}");

                XmlElement rootElement = doc.DocumentElement;

                openid = rootElement.SelectSingleNode("openid").InnerText;

                //根据微信号获取用户信息，使用户处于登录状态
                B2b_crm userinfo = new B2b_crm();
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
                    if (userinfo != null)
                    {
                        Readuser(userinfo.Idcard, comid);//读取用户信息
                    }
                }
            }
            return openid;
        }



        //读取用户信息
        private void Readuser(decimal idcard, int comid)
        {


            //Today = DateTime.Now.ToString("yyyy-MM-dd");
            B2bCrmData dateuser = new B2bCrmData();

            var userinfo = dateuser.GetB2bCrmByCardcode(idcard);
            if (userinfo != null)
            {

                //当读取用户信息的时候，判断是否有渠道转发信息
                if (Request.Cookies["ZF_ChanneId"] != null)
                {
                    int ZF_ChanneId = 0;
                    ZF_ChanneId = int.Parse(Request.Cookies["ZF_ChanneId"].Value);
                    if (ZF_ChanneId != 0)
                    { //有转发渠道ID
                        //在这判断 用户渠道是否为微信注册过来的
                        Member_Channel channel2 = new MemberChannelData().GetChannelByOpenId(userinfo.Weixin);
                        if (channel2 != null)
                        {
                            if (channel2.Issuetype == 4)
                            {
                                //如果为微信注册过来的 ，则修改会员渠道即可
                                int upchannel = new MemberCardData().upCardcodeChannel(userinfo.Idcard.ToString(), ZF_ChanneId);
                            }
                        }
                        else
                        {
                            //没有渠道的 ，则修改会员渠道即可
                            int upchannel = new MemberCardData().upCardcodeChannel(userinfo.Idcard.ToString(), ZF_ChanneId);
                        }

                        //清除Cookies
                        HttpCookie aCookie = new HttpCookie("ZF_ChanneId");
                        aCookie.Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies.Add(aCookie);
                    }

                }


                dateuser.WeixinConPass(userinfo.Weixin, comid);//只要包含SESSION登陆成功，清空微信密码
            }

        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Common.Business;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.CRM.Service.CRMService.Data;
using ETS.JsonFactory;
using ETS2.WeiXin.Service.WeiXinService.Model;
using System.Xml;
using Newtonsoft.Json;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;

namespace ETS2.WebApp.H5.order
{
    public partial class evaluate : System.Web.UI.Page
    {

        public string openid = "";//微信号
        public int AccountId = 0;//会员id
        public B2b_crm userinfo = new B2b_crm();
        public int comid = 0;
        public string title = "";
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        public string userid = "0";//用户临时 Uid 或 实际Uid 
        public int viewtype = 0;//显示类型，0为默认客户评价，1为 
        public int channelid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            bool bo = detectmobilebrowser.HttpUserAgent(u);


            if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式，则从多微信商户基本信息表中获取comid
            {
                comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl));
            }

            if (comid == 0)//如果非标准格式，查询 是否有绑定的域名
            {
                var domaincomid = B2bCompanyData.GetComId(RequestUrl);
                if (domaincomid != null)
                {
                    comid = domaincomid.Com_id;
                }
            }

            if (comid != 0)
            {
                var commodel = B2bCompanyData.GetCompany(comid);

                if (commodel != null)
                {
                    title = commodel.Com_name;
                }
            }


            //从cookie中得到微信号
            if (Request.Cookies["openid"] != null)
            {
                openid = Request.Cookies["openid"].Value;
            }
            if (Request.Cookies["AccountId"] != null)
            {
                AccountId = int.Parse(Request.Cookies["AccountId"].Value);
            }



            //获取微信平台端code
            string weixincode = Request["code"].ConvertTo<string>("");
            openid = Request["openid"].ConvertTo<string>("");
            string weixinpass = Request["weixinpass"].ConvertTo<string>("");

            viewtype = Request["type"].ConvertTo<int>(0);


            GetMemberCard(openid, weixincode, weixinpass, comid);//登陆或得到会员信息



            //最后判断登陆
            if (AccountId == 0)
            {

                Response.Redirect("/h5/order/login.aspx?come=/h5/order/evaluate.aspx");//非手机的跳转到V目录
            }

        }
        private void GetMemberCard(string openid, string weixincode, string weixinpass, int comid)
        {


            //如果SESSION有值，进行赋值
            if (openid == "" && Session["Openid"] != null)
            {
                openid = Session["Openid"].ToString();
            }

            //判断商家是否微信认证 
            if (weixincode != "")//进行过微信认证，微信认证登陆
            {
                //如果微信ID，递实现自动登陆     
                GetOpenId(weixincode, comid);
            }
            else if (openid != "" && weixinpass != "")
            {
                //最后判断传递过来的微信一次性密码
                VerifyOneOffPass(openid, weixinpass);
            }



            //判断登陆状态
            if (Session["AccountId"] != null)
            {
                //先判断Session
                AccountId = int.Parse(Session["AccountId"].ToString());
                B2bCrmData dateuser = new B2bCrmData();
                B2b_crm userinfo = dateuser.Readuser(AccountId, comid);
                if (userinfo != null)
                {
                    Readuser(userinfo.Idcard, comid);
                }
            }
            else
            {//再判断COOKIES

                if (Request.Cookies["AccountId"] != null)
                {
                    string accountmd5 = "";
                    AccountId = int.Parse(Request.Cookies["AccountId"].Value);
                    if (Request.Cookies["AccountKey"] != null)
                    {
                        accountmd5 = Request.Cookies["AccountKey"].Value;
                    }
                    B2b_crm userinfo;
                    var data = CrmMemberJsonData.WeixinCookieLogin(AccountId.ToString(), accountmd5, comid, out userinfo);
                    if (data == "OK")
                    {
                        Session["AccountId"] = userinfo.Id;
                        Session["AccountName"] = userinfo.Name;
                        Session["AccountCard"] = userinfo.Idcard;
                        Session["Com_id"] = comid;
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
                        cookie.Value = userinfo.Weixin;
                        cookie.Expires = DateTime.Now.AddDays(120);
                        Response.Cookies.Add(cookie);

                        if (userinfo != null)
                        {
                            Readuser(userinfo.Idcard, comid);//读取用户信息
                        }

                        Response.Redirect(Request.Url.ToString()); //登陆成功，刷新页面
                    }
                    else
                    {
                        //当cookie错误无法登陆则清除所有COOKIE
                        HttpCookie aCookie; string cookieName;
                        int limit = Request.Cookies.Count;
                        for (int i = 0; i < limit; i++)
                        {
                            cookieName = Request.Cookies[i].Name;
                            aCookie = new HttpCookie(cookieName);
                            aCookie.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(aCookie);
                        }

                        if (Request.Cookies["AccountId"] != null)
                        {
                            HttpCookie mycookie;
                            mycookie = Request.Cookies["AccountId"];
                            TimeSpan ts = new TimeSpan(0, 0, 0, 0);//时间跨度 
                            mycookie.Expires = DateTime.Now.Add(ts);//立即过期 
                            Response.Cookies.Remove("AccountId");//清除 
                            Response.Cookies.Add(mycookie);//写入立即过期的*/
                            Response.Cookies["AccountId"].Expires = DateTime.Now.AddDays(-1);
                        }
                        if (Request.Cookies["AccountName"] != null)
                        {
                            HttpCookie mycookie = Request.Cookies["AccountName"];
                            TimeSpan ts = new TimeSpan(0, 0, 0, 0);//时间跨度 
                            mycookie.Expires = DateTime.Now.Add(ts);//立即过期 
                            Response.Cookies.Remove("AccountName");//清除 
                            Response.Cookies.Add(mycookie);//写入立即过期的*/
                            Response.Cookies["AccountName"].Expires = DateTime.Now.AddDays(-1);
                        }
                        if (Request.Cookies["AccountKey"] != null)
                        {

                            HttpCookie mycookie = Request.Cookies["AccountKey"];
                            TimeSpan ts = new TimeSpan(0, 0, 0, 0);//时间跨度 
                            mycookie.Expires = DateTime.Now.Add(ts);//立即过期 
                            Response.Cookies.Remove("AccountKey");//清除 
                            Response.Cookies.Add(mycookie);//写入立即过期的*/
                            Response.Cookies["AccountKey"].Expires = DateTime.Now.AddDays(-1);
                        }


                        if (weixincode != "")
                        {//如果微信认证代码，微信认证登陆
                            GetOpenId(weixincode, comid);
                        }
                        else if (openid != "" && weixinpass != "")
                        {
                            VerifyOneOffPass(openid, weixinpass);
                        }



                    }

                }

            }
        }

        //微信一次性密码登陆
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

        //微信认证登陆
        private string GetOpenId(string codee, int comid)
        {
            WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);

            string openid = "";
            if (basicc != null)
            {

                string st = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + basicc.AppId + "&secret=" + basicc.AppSecret + "&code=" + codee + "&grant_type=authorization_code";
                XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + new GetUrlData().HttpGet(st) + "}");
                try
                {
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
                        cookie.Value = userinfo.Weixin;
                        cookie.Expires = DateTime.Now.AddDays(120);
                        Response.Cookies.Add(cookie);

                        if (userinfo != null)
                        {
                            Readuser(userinfo.Idcard, comid);//读取用户信息
                        }
                    }
                }
                catch
                {

                    //errlog += "{\"root\":" + new GetUrlData().HttpGet(st) + "}";
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
                AccountId = userinfo.Id;
                userid = userinfo.Id.ToString();//如果是登陆用户则读取用户的实际ID
                HttpCookie cookie = new HttpCookie("temp_userid");     //实例化HttpCookie类并添加值
                cookie.Value = userinfo.Id.ToString();
                cookie.Expires = DateTime.Now.AddDays(365);
                Response.Cookies.Add(cookie);

                var channeldata = new MemberChannelData();
                channelid = channeldata.GetChannelid(userinfo.Com_id, userinfo.Phone);

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
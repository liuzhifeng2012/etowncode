﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.Common.Business;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using FileUpload.FileUpload.Data;
using System.IO;
using ETS2.WeiXin.Service.WeiXinService.Model;
using System.Xml;
using Newtonsoft.Json;
using ETS.JsonFactory;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;

namespace ETS2.WebApp.H5.order
{
    public partial class Cart : System.Web.UI.Page
    {

        #region 辨别商家参数
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        public int comid = 0;
        #endregion
        public string comName = "";
        public string title = "";
        public string comlogo = "";
        public string key = "";
        public int proclass = 0;
        public int price = 0;

        public string Wxfocus_url = "";
        public string Wxfocus_author = "";

        public B2b_crm userinfo = null;

        public string openid = "";//微信号

        public int uid = 0;
        public string uip = "";

        public int buyuid = 0; //购买用户ID
        public int tocomid = 0;//来访商户COMID
        public string biaoti = "在线预订";
        public string weixinname = "";
        public string bannerimg = "";
        public string bannerlink = "";
        public string bannertitle = "";

        public string Scenic_intro = "";

        public List<B2b_company_menu> menulist;
        public int menutotalcount = 0;

        public string logoimg = "image/shop.png!60x60.jpg";
        public string userid = "0";//用户临时 Uid 或 实际Uid 



        public void Page_Load(object sender, EventArgs e)
        {
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            bool bo = detectmobilebrowser.HttpUserAgent(u);

            uid = Request["uid"].ConvertTo<int>(0);
            //获取IP地址
            uip = CommonFunc.GetRealIP();

            key = Request["key"].ConvertTo<string>("");

            if (key != "")
            {
                biaoti = key;
            }

            proclass = Request["class"].ConvertTo<int>(0);
            price = Request["price"].ConvertTo<int>(0);

            //获取随机用户ID
            if (Request.Cookies["temp_userid"] != null)
            {
                userid = Request.Cookies["temp_userid"].Value;
            }
            else
            {
                userid = Domain_def.HuoQu_Temp_UserId();
                //Response.Cookies("userid").val();

                HttpCookie cookie = new HttpCookie("temp_userid");     //实例化HttpCookie类并添加值
                cookie.Value = userid;
                cookie.Expires = DateTime.Now.AddDays(365);
                Response.Cookies.Add(cookie);
            }





            if (proclass != 0)
            {
                var prodata = new B2bComProData();
                var result = prodata.Proclassbyid(proclass);
                if (result != null)
                {
                    biaoti = result.Classname;
                }


            }



            buyuid = Request["buyuid"].ConvertTo<int>(0);
            tocomid = Request["tocomid"].ConvertTo<int>(0);


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


            if (comid != 0)
            {
                var commodel = B2bCompanyData.GetCompany(comid);

                if (commodel != null)
                {

                    if (commodel.B2bcompanyinfo != null)
                    {
                        Wxfocus_url = commodel.B2bcompanyinfo.Wxfocus_url;
                        Wxfocus_author = commodel.B2bcompanyinfo.Wxfocus_author;
                        weixinname = commodel.B2bcompanyinfo.Weixinname;
                        Scenic_intro = commodel.B2bcompanyinfo.Scenic_intro;

                    }

                    title = commodel.Com_name;
                }


                B2b_company_saleset pro = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
                if (pro != null)
                {
                    if (pro.Smalllogo != null && pro.Smalllogo != "")
                    {
                        comlogo = FileSerivce.GetImgUrl(pro.Smalllogo.ConvertTo<int>(0));
                    }
                }

                //获取微信平台端code
                string weixincode = Request["code"].ConvertTo<string>("");
                //获取微信号和一次性密码
                openid = Request["openid"].ConvertTo<string>("");
                string weixinpass = Request["weixinpass"].ConvertTo<string>("");

                //获得会员信息
                GetCrmInfo(weixincode, openid, weixinpass);

            }



            //获取BANNER，及logo
            if (comid != 0)
            {
                var imagedata = new B2bCompanyImageData();
                int totalcount = 0;
                List<B2b_company_image> list = imagedata.PageGetimageList(comid, 2, out totalcount);

                if (list != null)
                {
                    for (int i = 0; i < totalcount; i++)
                    {
                        bannerimg = FileSerivce.GetImgUrl(list[i].Imgurl);
                        bannerlink = list[i].Linkurl;
                        bannertitle = list[i].Title;
                    }
                }

                //根据公司id得到 直销设置
                B2b_company_saleset saleset = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
                if (saleset != null)
                {
                    logoimg = FileSerivce.GetImgUrl(saleset.Smalllogo.ConvertTo<int>(0));

                }


                //读取首页栏目,只读取前10个栏目，太多影响打开
                var shopmenudata = new B2bCompanyMenuData();
                menulist = shopmenudata.GetMenuList(comid, 1, 10, out menutotalcount, 1);
                if (menulist != null)
                {
                    for (int i = 0; i < menutotalcount; i++)
                    {
                        menulist[i].Imgurl_address = FileSerivce.GetImgUrl(menulist[i].Imgurl);
                    }
                }
            }



            //微信转发访问归属渠道
            if (uid != 0)//必须记录转发用户信息才能继续统计
            {

                //判断有转发人的渠道
                var crmdata = new B2bCrmData();
                var pro = crmdata.Readuser(uid, comid);//读取转发人用户信息
                string zhuanfa_phone = "";
                if (pro != null)
                {
                    zhuanfa_phone = pro.Phone;
                }

                if (zhuanfa_phone != "")
                { //转发人手机存在
                    MemberChannelData channeldata = new MemberChannelData();
                    var channeinfo = channeldata.GetPhoneComIdChannelDetail(zhuanfa_phone, comid);//查询渠道
                    if (channeinfo != null)
                    {
                        //转发人渠道记录COOKI
                        HttpCookie cookie = new HttpCookie("ZF_ChanneId");     //实例化HttpCookie类并添加值
                        cookie.Value = channeinfo.Id.ToString();
                        cookie.Expires = DateTime.Now.AddDays(120);
                        Response.Cookies.Add(cookie);
                    }
                }

            }



        }
        /// <summary>
        /// 获取会员信息分两种方式:a.根据客户端保存的cookie值获取 b.根据传递过来的参数获取
        /// </summary>
        /// <param name="weixincode"></param>
        /// <param name="openid"></param>
        /// <param name="weixinpass"></param>
        private void GetCrmInfo(string weixincode, string openid, string weixinpass)
        {
            if (Request.Cookies["AccountId"] != null)
            {
                string accountmd5 = "";
                int AccountId = int.Parse(Request.Cookies["AccountId"].Value);
                if (Request.Cookies["AccountKey"] != null)
                {
                    accountmd5 = Request.Cookies["AccountKey"].Value;
                }

                var data1 = CrmMemberJsonData.WeixinCookieLogin(AccountId.ToString(), accountmd5, comid, out userinfo);
                if (data1 == "OK")
                {
                    //从cookie中得到微信号
                    if (Request.Cookies["openid"] != null)
                    {
                        openid = Request.Cookies["openid"].Value;
                    }
                    if (userinfo != null)
                    {
                        Readuser(userinfo.Idcard, comid);//读取用户信息
                    }
                }
                else
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
                    //根据传递过来的参数获取会员信息
                    GetCrmByParam(weixincode, openid, weixinpass);
                }
            }
            else
            {
                //根据传递过来的参数获取会员信息
                GetCrmByParam(weixincode, openid, weixinpass);
            }
        }

        private void GetCrmByParam(string weixincode, string openid, string weixinpass)
        {
            if (weixincode != "")//商家已经进行过微信认证
            {
                GetOpenId(weixincode, comid);
            }
            else//商家没有进行过微信认证
            {

                VerifyOneOffPass(openid, weixinpass);
            }
        }
        private void VerifyOneOffPass(string openid, string weixinpass)
        {
            if (openid != "" && weixinpass != "")
            {
                B2bCrmData dateuser = new B2bCrmData();
                string data = CrmMemberJsonData.WeixinLogin(openid, weixinpass, comid, out userinfo);

                if (data == "OK")//正确的一次性密码
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
            new B2bCrmData().WeixinConPass(openid, comid);//清空微信密码
        }



        private void GetOpenId(string codee, int comid)
        {

            WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);
            if (basicc != null)
            {
                try
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
                catch
                {

                }
            }

        }



        //读取用户信息
        private void Readuser(decimal idcard, int comid)
        {


            //Today = DateTime.Now.ToString("yyyy-MM-dd");
            B2bCrmData dateuser = new B2bCrmData();

            var userinfo = dateuser.GetB2bCrmByCardcode(idcard);
            if (userinfo != null)
            {

                userid = userinfo.Id.ToString();//如果是登陆用户则读取用户的实际ID
                HttpCookie cookie = new HttpCookie("temp_userid");     //实例化HttpCookie类并添加值
                cookie.Value = userinfo.Id.ToString();
                cookie.Expires = DateTime.Now.AddDays(365);
                Response.Cookies.Add(cookie);

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
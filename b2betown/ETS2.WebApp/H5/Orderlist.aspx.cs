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

namespace ETS2.WebApp.H5
{
    public partial class Orderlist : System.Web.UI.Page
    {
        #region 辨别商家参数
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        public int comid = 0;
        #endregion

        public string comName = "";

        public string headPortraitImgSrc = "";

        public string remark = "";
        public int comid_temp = 0;
        public int comidture = 0;

        public string Wxfocus_url = "";
        public string Wxfocus_author = "";


        //--------------新添加的公共参数---------------//
        public B2b_crm userinfo = new B2b_crm();

        public string openid = "";
        public string weixinpass = "";

        public int projectid = 0;//项目id
        public string projectname = "";//项目名称
        public string projectimgurl = "";//项目图片
        public string projectbrief = "";//项目精简介绍


        public int uid = 0;
        public string uip = "";
        public int proclass = 0;

        public int buyuid = 0; //购买用户ID
        public int tocomid = 0;//来访商户COMID
        public string pno = "";
        public int usepnonum = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            Request.ValidateInput();  
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            //bool bo = detectmobilebrowser.HttpUserAgent(u);
            proclass = Request["proclass"].ConvertTo<int>(0);

            uid = Request["uid"].ConvertTo<int>(0);
            //获取IP地址
            uip = CommonFunc.GetRealIP();

            buyuid = Request["buyuid"].ConvertTo<int>(0);
            tocomid = Request["tocomid"].ConvertTo<int>(0);
            projectid = Request["id"].ConvertTo<int>(0);

            pno = Request["pno"].ConvertTo<string>("");

            string pno1 = EncryptionHelper.EticketPnoDES(pno, 1);//解密

            var B2bEticketdata = new B2bEticketData();
            var eticketinfo = B2bEticketdata.GetEticketDetail(pno1);

            if (eticketinfo != null)
            {
                comid = eticketinfo.Com_id;
                usepnonum = eticketinfo.Use_pnum;
            }


            if (comid == 0)
            {
                if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式，则从多微信商户基本信息表中获取comid
                {
                    comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl));
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


            //根据项目id得到项目信息
            if (projectid != 0)
            {
                B2b_com_project mod = new B2b_com_projectData().GetProject(projectid, comid);
                if(mod!=null)
                {
                    projectname = mod.Projectname;
                    projectimgurl = FileSerivce.GetImgUrl(mod.Projectimg);
                    projectbrief = mod.Briefintroduce;

                }

            
                B2bComProData prodata1 = new B2bComProData();
                var prohotel = prodata1.Selectpro_hotel(comid, projectid);
                if (prohotel == 1)
                {
                   // Response.Redirect("hotel/hotelshow.aspx?projectid=" + projectid + "&id=" + comid + "&uid =" + uid + "&buyuid=" + buyuid + "&tocomid=" + tocomid + " ");
                }
            }


            if (comid != 0)
            {



                B2b_company companyinfo = B2bCompanyData.GetCompany(comid);

                B2bCompanyInfoData info = new B2bCompanyInfoData();
                if (companyinfo != null)
                {
                    comName = companyinfo.Com_name;
                    if (comName.Length >= 9)
                    {
                        comName = comName.Substring(0, 9) + "..";
                    }

                    remark = info.GetCompanyInfo(comid).Scenic_intro + "&nbsp;&nbsp;<a style=\"color:#1a9ed9\" href=\"OrderinfoTitle.aspx?id=" + comid + "\">(查看全部)</a>";
                    if (remark.Length > 42)
                    {
                        remark = remark.Substring(0, 42) + "&nbsp;&nbsp;<a style=\"color:#1a9ed9\" href=\"OrderinfoTitle.aspx?id=" + comid + "\">(查看全部)</a>";
                    }


                    if (companyinfo.B2bcompanyinfo != null)
                    {
                        Wxfocus_url = companyinfo.B2bcompanyinfo.Wxfocus_url;
                        Wxfocus_author = companyinfo.B2bcompanyinfo.Wxfocus_author; ;

                    }
                }

                B2b_company_saleset saleset = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
                if (saleset != null)
                {
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

            {   int questtype = 2;//1=微信授权验证
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
                    else if (questtype == 2){
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
                    else if (questtype == 2){
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
                    if (userinfo != null)
                    {
                        Readuser(userinfo.Idcard, comid);//读取用户信息
                    }
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
                    if (userinfo != null)
                    {
                        Readuser(userinfo.Idcard, comid);//读取用户信息
                    }
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
                //Integral = userinfo.Integral.ToString() == "" ? "0" : userinfo.Integral.ToString();
                //Imprest = userinfo.Imprest.ToString() == "" ? "0" : userinfo.Imprest.ToString();

                //AccountWeixin = userinfo.Weixin;
                //AccountEmail = userinfo.Email;
                //Accountphone = userinfo.Phone;
                //AccountCard = userinfo.Idcard.ToString();
                //string a = AccountCard.Substring(0, 1);
                //if (a != null)
                //{
                //    fcard = int.Parse(a.ToString());
                //}


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
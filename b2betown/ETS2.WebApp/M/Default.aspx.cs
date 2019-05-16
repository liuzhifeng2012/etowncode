using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Member.Service.MemberService.Model;
using ETS.JsonFactory;
using ETS2.Common.Business;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS.Framework;
using FileUpload.FileUpload.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;
using System.Xml;
using Newtonsoft.Json;
using FileUpload.FileUpload.Entities;

namespace ETS2.WebApp.M
{
    public partial class Default : System.Web.UI.Page
    {

        ////判断如果是否为手机访问
        //if (detectmobilebrowser.HttpUserAgent(Request.ServerVariables["HTTP_USER_AGENT"]))
        //{
        //    if (Request["brow"]=="MO")//如果接收到传递手机访问则只手机版
        //    {
        //         Cookie.WriteCookie("Mobile_Brow_Set", "MO");
        //    }
        //
        //    //查看COOKIE 是否设定是否设定为手机访问
        //    if (Cookie.GetCookie("Mobile_Brow_Set") == "MO")
        //    {
        //    }
        //    else
        //    {
        //          Response.Redirect("/V/Default.aspx?brow=PC");
        //    }
        //}



        //public int AccountId = 0;
        //public string AccountName = "";
        //public string AccountCard = "";
        //public string AccountEmail = "";
        //public string AccountWeixin = "";
        //public string Accountphone = "";

        //public string RequestUrl = "";//访问网址通过访问网址判断商家，如果访问网址为空则跳转到登陆页
        //public int comid = 0;

        //public int channeltype = 0;//0为用户 1为渠道商
        //public int channelid = 0;//0为用户 1为渠道商


        //public decimal RebateConsume = 0;
        //public decimal RebateOpen = 0;
        //public int Opencardnum = 0;
        //public int Firstdealnum = 0;
        //public decimal Summoney = 0;

        //public string openid = "";//微信传递过来字符串

        public int comid = 0;//公司id
        //获得公司logo地址和公司名称
        public string comname = "";//公司名称
        public string comlogo = "";//公司logo地址
        public int channelcompanyid = 0;

        public B2b_crm userinfo = new B2b_crm();
        protected void Page_Load(object sender, EventArgs e)
        {
            //string u = Request.ServerVariables["HTTP_USER_AGENT"];
            //bool bo = detectmobilebrowser.HttpUserAgent(u);
            bool bo = true;

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
            if (comid != 0)
            {
                //根据公司id得到公司logo地址和公司名称
                comname = B2bCompanyData.GetCompany(comid).Com_name;
                B2b_company_saleset pro = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
                if (pro != null)
                {
                  
                    comlogo = FileSerivce.GetImgUrl(pro.Logo.ConvertTo<int>(0));
                }

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

                if (bo == false)
                {//|| comid == 106
                    if (comid == 101)
                    {
                        Response.Redirect("http://vctrip.etown.cn/");
                    }
                    Response.Redirect("http://shop" + comid + ".etown.cn");
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

            //根据微信号得到会员所在的门市信息
            Member_Channel_company menshi = new MemberChannelcompanyData().GetMenShiByJumpId(openid, comid);
            if (menshi != null)
            {
                if (menshi.Whetherdepartment == 0)
                {
                    //如果不是内部部门，显示门店列表
                    channelcompanyid = menshi.Id;
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
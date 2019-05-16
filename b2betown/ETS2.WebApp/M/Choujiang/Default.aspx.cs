using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Member.Service.MemberService.Model;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Common.Business;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;
using System.Xml;
using Newtonsoft.Json;
using ETS.JsonFactory;


namespace ETS2.WebApp.M.Choujiang
{
    public partial class Default : System.Web.UI.Page
    {
        public int actid = 0;
        public int actstate = 1;//是否有活动，没有互动则显示暂无活动
        public int Com_id = 0;
        public string Title = "";
        public int ERNIE_type = 0;     //摇奖类型，大转盘
        public DateTime ERNIE_star;
        public DateTime ERNIE_end;
        public int ERNIE_RateNum = 0; //摇奖基数
        public int ERNIE_Limit = 0;   //摇奖限定,每个账户一次，还是每天一次或多次
        public int Limit_Num = 0;    //摇奖限定次数
        public int Runstate = 0;
        public string Remark = "";
        public string openid = "";



        public string Award_title1 = "";
        public int Award_num1 = 0;
        public int Award_class1 = 0;
        public int Id1 = 0;

        public string Award_title2 = "";
        public int Award_num2 = 0;
        public int Award_class2 = 0;
        public int Id2 = 0;

        public string Award_title3 = "";
        public int Award_num3 = 0;
        public int Award_class3 = 0;
        public int Id3 = 0;

        public int choujiangcishu = 0;

        public int comid = 0;
        public string RequestUrl = "";
        public string weixincode = "";
        public string weixinpass = "";
        public string errlog = "";

        public int AccountId = 0;

        public string Today;
        public string Integral = "";
        public string Imprest = "";
        public string AccountWeixin = "";
        public string AccountEmail = "";
        public string Accountname = "";
        public string Accountphone = "";
        public string AccountCard = "";
        public int fcard = 0;
        public string shijianchuo = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            shijianchuo = DateTime.Now.ToString("yyyyMMddhhmmssfff");

            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            bool bo = detectmobilebrowser.HttpUserAgent(u);

            RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();
            try
            {

                //根据域名读取商户ID,如果没有绑定域名直接跳转后台
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
            catch
            {
                errlog += "1,";
            }


            if (comid != 0)
            {
                if (bo == false)
                {
                    if (comid == 101)
                    {
                        // Response.Redirect("http://vctrip.etown.cn/");
                    }
                    // Response.Redirect("http://shop" + comid + ".etown.cn");
                }



                //获取微信平台端code
                weixincode = Request["code"].ConvertTo<string>("");
                openid = Request["openid"].ConvertTo<string>("");
                weixinpass = Request["weixinpass"].ConvertTo<string>("");

                GetMemberCard(openid, weixincode, weixinpass, comid);//登陆或得到会员信息
            }


            //得到活动ID
            actid = Request["actid"].ConvertTo<int>(0);
            //非班车抽奖活动，则得到最新运行的上线活动id(现在只是班车抽奖传递ntime参数)
            if (actid == 0 && Request["ntime"].ConvertTo<string>("") == "")
            {
                //得到最新运行的，上线的活动ID
                actid = MemberERNIEData.ERNIETOPgetid(comid);
            }

            if (actid == 0)
            {
                actstate = 0;
            }
            else
            {
                actstate = 1;
            }


            //抽奖活动
            Member_ERNIE erniemodel = new Member_ERNIE();
            //奖项
            Member_ERNIE_Award Awardmodel = new Member_ERNIE_Award();

            var pro = MemberERNIEData.ERNIEGetActById(actid);
            if (pro != null)
            {
                Com_id = pro.Com_id;
                Title = pro.Title;
                ERNIE_type = pro.ERNIE_type;     //摇奖类型，大转盘
                ERNIE_star = pro.ERNIE_star;
                ERNIE_end = pro.ERNIE_end;
                ERNIE_RateNum = pro.ERNIE_RateNum; //摇奖基数
                ERNIE_Limit = pro.ERNIE_Limit;   //摇奖限定,每个账户一次，还是每天一次或多次
                Limit_Num = pro.Limit_Num;    //摇奖限定次数
                Runstate = pro.Runstate;
                Remark = pro.Remark;
            }
            //获取一等奖
            var Awardpro1 = MemberERNIEData.ERNIEAwardget(actid, 1);
            if (Awardpro1 != null)
            {
                Award_title1 = Awardpro1.Award_title;
                Award_num1 = Awardpro1.Award_num;
                Award_class1 = Awardpro1.Award_class;
                Id1 = Awardpro1.Id;
            }
            //获取二等奖
            var Awardpro2 = MemberERNIEData.ERNIEAwardget(actid, 2);
            if (Awardpro2 != null)
            {
                Award_title2 = Awardpro2.Award_title;
                Award_num2 = Awardpro2.Award_num;
                Award_class2 = Awardpro2.Award_class;
                Id1 = Awardpro2.Id;
            }
            //获取三等奖
            var Awardpro3 = MemberERNIEData.ERNIEAwardget(actid, 3);
            if (Awardpro3 != null)
            {
                Award_title3 = Awardpro3.Award_title;
                Award_num3 = Awardpro3.Award_num;
                Award_class3 = Awardpro3.Award_class;
                Id1 = Awardpro3.Id;
            }

            //读取活动信息
            var erniedate = MemberERNIEData.ERNIEGetActById(actid);
            if (erniedate != null)
            {
                if (erniedate.Runstate == 1 && erniedate.Online == 1 && erniedate.ERNIE_star < DateTime.Now && erniedate.ERNIE_end.AddDays(1) > DateTime.Now)
                {
                    var ERNIE_Limit = erniedate.ERNIE_Limit;//抽奖类型
                    var Limit_Num = erniedate.Limit_Num;//可抽奖次数
                    //读取用户信息
                    B2bCrmData crmmodel = new B2bCrmData();
                    B2b_crm memberinfo = crmmodel.b2b_crmH5(openid, erniedate.Com_id);
                    if (memberinfo != null)
                    {
                        if (memberinfo != null)
                        {
                            ERNIE_Record recordinfo = new ERNIE_Record();
                            recordinfo.ERNIE_openid = openid;
                            recordinfo.ERNIE_uid = 0;
                            recordinfo.ERNIE_id = actid;

                            var searchdate = MemberERNIEData.SearchChoujiang(recordinfo, ERNIE_Limit);//查询是否抽过奖
                            if (searchdate < Limit_Num)
                            {
                                if (AccountWeixin != "")//判断登陆状态并有微信号
                                {
                                    choujiangcishu = Limit_Num - searchdate;
                                }
                                else
                                {
                                    openid = "";
                                }

                            }
                        }
                    }
                }
            }


            string ntime = Request["ntime"].ConvertTo<string>("");
            string md5ntime = Request["md5ntime"].ConvertTo<string>("");

            string tmd5ntime = EncryptionHelper.ToMD5(ntime + "lixh1210", "UTF-8");
            if (ntime != "")
            {
                if (md5ntime == tmd5ntime)
                {
                    if (DateTime.Parse(ntime).ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                    { }
                    else
                    {
                        //非当日抽奖
                        actstate = 0;
                    }
                }
                else
                {
                    actstate = 0;
                }
            }

        }


        private void GetMemberCard(string openid, string weixincode, string weixinpass, int comid)
        {
            //if (openid != null && openid != "")
            //{
            //    //只要传递过来微信ID 直接SESSION
            //    Session["Openid"] = openid;
            //}

            //如果SESSION有值，进行赋值
            if (openid == "" && Session["Openid"] != null)
            {
                openid = Session["Openid"].ToString();
            }

            //判断登陆状态
            if (Session["AccountId"] != null)
            {//先判断Session
                AccountId = int.Parse(Request.Cookies["AccountId"].Value);
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
                        if (userinfo != null)
                        {
                            Integral = userinfo.Integral.ToString() == "" ? "0" : userinfo.Integral.ToString();
                            Imprest = userinfo.Imprest.ToString() == "" ? "0" : userinfo.Imprest.ToString();

                            AccountCard = userinfo.Idcard.ToString();
                            string a = AccountCard.Substring(0, 1);
                            if (a != null)
                            {
                                fcard = int.Parse(a.ToString());
                            }

                        }
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
                else
                {
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
                }
            }
        }

        //微信一次性密码登陆
        private void VerifyOneOffPass(string openid, string weixinpass)
        {
            if (openid != null && openid != "" && weixinpass != "" && weixinpass != null)
            {
                B2bCrmData dateuser = new B2bCrmData();
                B2b_crm userinfo = new B2b_crm();
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
                    errlog += "2,";
                    errlog += "{\"root\":" + new GetUrlData().HttpGet(st) + "}";
                }
            }
            return openid;
        }

        //读取用户信息
        private void Readuser(decimal idcard, int comid)
        {


            Today = DateTime.Now.ToString("yyyy-MM-dd");
            B2bCrmData dateuser = new B2bCrmData();

            var userinfo = dateuser.GetB2bCrmByCardcode(idcard);
            if (userinfo != null)
            {
                Integral = userinfo.Integral.ToString() == "" ? "0" : userinfo.Integral.ToString();
                Imprest = userinfo.Imprest.ToString() == "" ? "0" : userinfo.Imprest.ToString();

                AccountWeixin = userinfo.Weixin;
                AccountEmail = userinfo.Email;
                Accountname = userinfo.Name;
                Accountphone = userinfo.Phone;
                AccountCard = userinfo.Idcard.ToString();
                string a = AccountCard.Substring(0, 1);
                if (a != null)
                {
                    fcard = int.Parse(a.ToString());
                }


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
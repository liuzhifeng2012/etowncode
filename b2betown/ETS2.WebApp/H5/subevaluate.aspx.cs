using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS.JsonFactory;
using ETS.Framework;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Common.Business;
using ETS2.WeiXin.Service.WeiXinService.Data;
using FileUpload.FileUpload.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;
using System.Xml;
using Newtonsoft.Json;
using FileUpload.FileUpload.Entities;
using ETS2.VAS.Service.VASService.Data;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle.Enum;


namespace ETS2.WebApp.H5
{
    public partial class subevaluate : System.Web.UI.Page
    {
        public string weixinpass = "";//微信密码
        public string weixincode = "";

        //用户信息
        public B2b_crm userinfo = new B2b_crm();
        public string Today = "";

        public string Integral = "0";
        public string Imprest = "0";

        public string Integrallist = "";
        public string Imprestlist = "";
        public string Orderlist = "";


        public int AccountId = 0;
        public string AccountName = "";
        public string AccountCard = "";
        public string AccountEmail = "";
        public string AccountWeixin = "";
        public string Accountphone = "";

        public string RequestUrl = "";//访问网址通过访问网址判断商家，如果访问网址为空则跳转到登陆页
        public int comid = 0;

        public int channeltype = 0;//0为用户 1为渠道商
        public int channelid = 0;//0为用户 1为渠道商


        public decimal RebateConsume = 0;
        public decimal RebateOpen = 0;
        public int Opencardnum = 0;
        public int Firstdealnum = 0;
        public decimal Summoney = 0;

        public string openid = "";//微信传递过来字符串

        public int fcard;



        //获得公司logo地址和公司名称
        public string title = "";//公司名称
        public string logoimg = "";//公司logo地址

        //得到会员级别
        public int iscreate_b2bcrmlevel = 0;
        public string levelname = "";
        public string crmlevel = "";

        public string errlog = "";//错误日志


        public int oid = 0;      //订单ID
        public int channleid = 0;//渠道id
        public int uid = 0;//用户ID
        public int evatype = 0;// 留言类型，0=客户给教练评价，1=教练对客户的评定
        public string Pro_name = "";//产品名称

        protected void Page_Load(object sender, EventArgs e)
        {
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            bool bo = detectmobilebrowser.HttpUserAgent(u);

            oid = Request["oid"].ConvertTo<int>(0);
            evatype = Request["evatype"].ConvertTo<int>(0);

            RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();
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

            if (openid != null && openid != "")
            {
                //只要传递过来微信ID 直接SESSION
                Session["Openid"] = openid;
            }

            if (comid != 0)
            {

                //根据公司id得到公司logo地址和公司名称
                title = B2bCompanyData.GetCompany(comid).Com_name;
                B2b_company_saleset pro = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());

                if (pro != null)
                {

                    logoimg = FileSerivce.GetImgUrl(pro.Logo.ConvertTo<int>(0));
                }


                //获取微信平台端code
                weixincode = Request["code"].ConvertTo<string>("");
                openid = Request["openid"].ConvertTo<string>("");
                weixinpass = Request["weixinpass"].ConvertTo<string>("");

                GetMemberCard(openid, weixincode, weixinpass, comid);//登陆或得到会员信息


                //最后判断登陆
                if (AccountId == 0)
                {
                    Response.Redirect("/h5/order/login.aspx");
                }

                //读取订单
                var orderdata = new B2bOrderData();
                var orderinfo = orderdata.GetOrderById(oid);
                if (orderinfo != null) {
                    var prodata = new B2bComProData();
                    var proinfo = prodata.GetProById(orderinfo.Pro_id.ToString(),orderinfo.Speciid,orderinfo.channelcoachid);
                    if (proinfo != null) {
                        Pro_name=proinfo.Pro_name;
                        if (evatype == 1)
                        {
                            Pro_name = "教练为用户提交教学档案：" + Pro_name+ " 预约时间："+orderinfo.U_traveldate.ToString("yyyy-MM-dd HH:mm");
                        }
                        
                    }
                    

                    uid = orderinfo.U_id;
                    channelid = orderinfo.channelcoachid;

                }


                //权限不匹配 跳转会员登陆页面
                if (evatype == 0)
                {
                    //if (AccountId != uid)
                    //{
                    //    Response.Redirect("/h5/order/login.aspx");
                    //}
                }
                else {//如果是 教练评价 客户 
                    //查询渠道是否匹配，可能遇到不针对教练预约 ，暂时不做判定，这样 出现所有教练都有可能对客户进行评价，未指定教练，如何评价


                    
                
                
                }




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

                AccountId = userinfo.Id;
                Integral = userinfo.Integral.ToString() == "" ? "0" : userinfo.Integral.ToString();
                Imprest = userinfo.Imprest.ToString() == "" ? "0" : userinfo.Imprest.ToString();

                AccountWeixin = userinfo.Weixin;
                AccountEmail = userinfo.Email;
                Accountphone = userinfo.Phone;
                AccountCard = userinfo.Idcard.ToString();
                string a = AccountCard.Substring(0, 1);
                if (a != null)
                {
                    fcard = int.Parse(a.ToString());
                }

                dateuser.WeixinConPass(userinfo.Weixin, comid);//只要包含SESSION登陆成功，清空微信密码
            }


        }


    }
}
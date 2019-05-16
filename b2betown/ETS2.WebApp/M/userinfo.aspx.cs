using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.Common.Business;
using ETS.JsonFactory;
using ETS2.WeiXin.Service.WeiXinService.Data;

namespace ETS2.WebApp.M
{
    public partial class userinfo : System.Web.UI.Page
    {
        public int comid = 0;//公司id
        public string RequestUrl = "";
        public string openid = "";//微信号
        public string weixinpass = "";//微信密码

        //用户信息
        public int AccountId ;
        public string AccountName = "";
        public string AccountCard = "";
        public string Today = "";

        public string Accountphone = "";
        public string AccountWeixin = "";
        public DateTime AccountBirthday;
        public string Accountsex = "";

        public string year = "";
        public string month = "";
        public string day = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            bool bo = detectmobilebrowser.HttpUserAgent(u);

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

            //if (bo == false)
            //{
            //    if (comid == 101)
            //    {
            //        Response.Redirect("http://vctrip.etown.cn/");
            //    }
            //    Response.Redirect("http://shop" + comid + ".etown.cn");
            //}

            if (openid != null && openid != "")
            {
                //只要传递过来微信ID 直接SESSION
                Session["Openid"] = openid;
            }



            //判断登陆状态
            if (Session["AccountId"] != null)
            {//先判断Session
                AccountId = Int32.Parse(Session["AccountId"].ToString());
                AccountName = Session["AccountName"].ToString();
                AccountCard = Session["AccountCard"].ToString();
                Today = DateTime.Now.ToString("yyyy-MM-dd");
                //comid = int.Parse(Session["Com_id"].ToString());


                //AccountId = Int32.Parse(Session["AccountId"].ToString());
                B2bCrmData dateuser1 = new B2bCrmData();
                B2b_crm modeluser = dateuser1.Readuser(AccountId, comid);

                if (modeluser != null)
                {
                    openid = modeluser.Weixin;
                    Accountphone = modeluser.Phone;
                    AccountName = modeluser.Name;
                    AccountCard = modeluser.Idcard.ToString();
                    AccountBirthday = modeluser.Birthday;
                    Accountsex = modeluser.Sex;
                    year = AccountBirthday.Year.ToString();
                    month = AccountBirthday.Month.ToString();
                    day = AccountBirthday.Day.ToString();

                    comid = int.Parse(modeluser.Com_id.ToString());
                }



                B2bCrmData dateuser = new B2bCrmData();
                dateuser.WeixinConPass(openid, comid);//只要包含SESSION登陆成功，清空微信密码

            }
            else
            {//再判断COOKIES
                //comid = Request["comid"].ConvertTo<int>(0);
                if (Request.Cookies["AccountId"] != null && Request.Cookies["AccountKey"] != null)
                {
                    AccountId =Int32.Parse(Request.Cookies["AccountId"].Value);
                    string accountmd5 = Request.Cookies["AccountKey"].Value;
                    B2b_crm userinfo;

                    var data = CrmMemberJsonData.WeixinCookieLogin(AccountId.ToString(), accountmd5, comid, out userinfo);
                    if (data == "OK")
                    {
                        Session["AccountId"] = userinfo.Id;
                        Session["AccountName"] = userinfo.Name;
                        Session["AccountCard"] = userinfo.Idcard;
                        Session["Com_id"] = comid;
                        //Accountsex = userinfo.Sex;
                        openid = userinfo.Weixin;


                        HttpCookie cookie = new HttpCookie("AccountId", userinfo.Id.ToString());     //实例化HttpCookie类并添加值
                        cookie.Expires = DateTime.Now.AddDays(120);
                        Response.Cookies.Add(cookie);
                        cookie = new HttpCookie("AccountName", userinfo.Name.ToString());     //实例化HttpCookie类并添加值
                        cookie.Expires = DateTime.Now.AddDays(120);
                        Response.Cookies.Add(cookie);
                        var returnmd5 = EncryptionHelper.ToMD5(userinfo.Idcard.ToString() + userinfo.Id.ToString(), "UTF-8");
                        cookie = new HttpCookie("AccountKey", returnmd5);     //实例化HttpCookie类并添加值
                        cookie.Expires = DateTime.Now.AddDays(120);
                        Response.Cookies.Add(cookie);
                    }

                }
                
            }


        }
    }
}
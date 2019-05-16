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
using FileUpload.FileUpload.Data;
using FileUpload.FileUpload.Entities;


namespace ETS2.WebApp.M
{
    public partial class MemberH5 : System.Web.UI.MasterPage
    {

        public int comid = 0;//公司id
        public string RequestUrl = "";
        public string openid = "";//微信号
        public string weixinpass = "";//微信密码

        //用户信息
        public string AccountId = "";
        public string AccountName = "";
        public string AccountCard = "";
        public string Today = "";

        //获得公司logo地址和公司名称
        public string comname = "";//公司名称
        public string comlogo = "";//公司logo地址

        protected void Page_Load(object sender, EventArgs e)
        {

            comid = Request["comid"].ConvertTo<int>(0);
            openid = Request["openid"].ConvertTo<string>("");
            weixinpass = Request["weixinpass"].ConvertTo<string>("");

            RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();
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
            }


            if (openid != null && openid != "")
            {
                //只要传递过来微信ID 直接SESSION
                Session["Openid"] = openid;
            }



            //判断登陆状态
            if (Session["AccountId"] != null)
            {//先判断Session
                AccountId = Session["AccountId"].ToString();
                AccountName = Session["AccountName"].ToString();
                AccountCard = Session["AccountCard"].ToString();
                Today = DateTime.Now.ToString("yyyy-MM-dd");

                B2bCrmData dateuser = new B2bCrmData();
                dateuser.WeixinConPass(openid, comid);//只要包含SESSION登陆成功，清空微信密码

            }
            else
            {//再判断COOKIES
                if (Request.Cookies["AccountId"] != null && Request.Cookies["AccountKey"] != null)
                {
                    AccountId = Request.Cookies["AccountId"].Value;
                    string accountmd5 = Request.Cookies["AccountKey"].Value;
                    B2b_crm userinfo;

                    var data = CrmMemberJsonData.WeixinCookieLogin(AccountId, accountmd5, comid, out userinfo);
                    if (data == "OK")
                    {
                        Session["AccountId"] = userinfo.Id;
                        Session["AccountName"] = userinfo.Name;
                        Session["AccountCard"] = userinfo.Idcard;

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
                        Response.Redirect(Request.Url.ToString()); //登陆成功，刷新页面
                    }
                    else
                    {
                        //如果微信ID，递实现自动登陆
                        if (openid != null && openid != "" && weixinpass != "" && weixinpass != null)
                        {
                            //只要传递过来微信ID 直接SESSION
                            Session["Openid"] = openid;

                            B2bCrmData dateuser = new B2bCrmData();

                            data = CrmMemberJsonData.WeixinLogin(openid, weixinpass, comid, out userinfo);

                            if (data == "OK")
                            {
                                dateuser.WeixinConPass(openid, comid);//登陆成功，清空微信密码
                                Session["AccountId"] = userinfo.Id;
                                Session["AccountName"] = userinfo.Name;
                                Session["AccountCard"] = userinfo.Idcard;

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
                                Response.Redirect(Request.Url.ToString());

                            }
                        }


                    }

                }
                else
                {  //最后判断传递过来的微信一次性密码
                    //如果微信ID，递实现自动登陆
                    if (openid != null && openid != "" && weixinpass != "" && weixinpass != null)
                    {
                        //只要传递过来微信ID 直接SESSION
                        Session["Openid"] = openid;

                        B2bCrmData dateuser = new B2bCrmData();

                        B2b_crm userinfo = new B2b_crm();
                        var data = CrmMemberJsonData.WeixinLogin(openid, weixinpass, comid, out userinfo);

                        if (data == "OK")
                        {
                            dateuser.WeixinConPass(openid, comid);//登陆成功，清空微信密码
                            Session["AccountId"] = userinfo.Id;
                            Session["AccountName"] = userinfo.Name;
                            Session["AccountCard"] = userinfo.Idcard;

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
                            Response.Redirect(Request.Url.ToString());

                        }
                    }



                }
            }


        }
    }
}
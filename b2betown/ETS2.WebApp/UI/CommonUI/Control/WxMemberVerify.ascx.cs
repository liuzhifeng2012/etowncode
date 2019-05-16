using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.CRM.Service.CRMService.Data;
using ETS.JsonFactory;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Common.Business;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using System.Xml;
using Newtonsoft.Json;
using FileUpload.FileUpload.Entities;
using FileUpload.FileUpload.Data;

namespace ETS2.WebApp.UI.CommonUI.Control
{
    public partial class WxMemberVerify : System.Web.UI.UserControl
    {
        //---主要实现功能：获得会员信息和公司信息，以及实现会员的登录---
        public B2b_crm crminfo = null;//会员信息
        public B2b_company cominfo = null;//公司信息
        public string gongsilogo = "";//公司logo

        protected void Page_Load(object sender, EventArgs e)
        {

            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            bool bo = detectmobilebrowser.HttpUserAgent(u);//判断是否是手机访问


            string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();

            #region 如果符合shop101.etown.cn的格式,根据微信配置信息获得公司信息(基本信息和扩展信息)
            if (Domain_def.Domain_yanzheng(RequestUrl))
            {
                WeiXinBasic wxbasic = new WeiXinBasicData().GetWeiXinBasicByDomain(RequestUrl);

                #region 如果配置了微信接口信息，获得公司信息
                if (wxbasic != null)
                {
                    int comid = wxbasic.Comid;
                    cominfo = B2bCompanyData.GetAllComMsg(comid);


                    //判断访问路径来源:1.微信认证商家直接点击自定义菜单链接过来,微信端传递过来code ；
                    string weixincode = Request["code"].ConvertTo<string>("");
                    if (weixincode != "")
                    {
                        if (cominfo != null)
                        {
                            GetOpenId(weixincode, cominfo.ID);
                            GetComLogo(cominfo.ID);
                        }
                    }
                    //2.微信未认证商家点击链接过来,传递过来openid和一次性密码(a.正确的openid和一次性密码，则实现会员登录和获得会员信息所在公司信息;b.一次性密码错误或者没有传递则 通过客户端保存数据获得用户信息)
                    string openid = Request["openid"].ConvertTo<string>("");
                    string weixinpass = Request["weixinpass"].ConvertTo<string>("");
                    if (openid != "")
                    {
                        if (cominfo != null)
                        {
                            VerifyOneOffPass(openid, weixinpass, cominfo.ID);
                            GetComLogo(cominfo.ID);

                        }
                    }
                }
                #endregion
                #region 如果没有配置微信接口信息，不处理
                else
                {

                }
                #endregion
            }
            #endregion

            #region 如果不符合shop101.etown.cn的格式,不处理
            else
            {

            }
            #endregion
        }

        public void GetComLogo(int comid)
        {
            //根据公司id得到公司logo地址
            B2b_company_saleset pro = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
            if (pro != null)
            {
           
                gongsilogo = FileSerivce.GetImgUrl(pro.Logo.ConvertTo<int>(0));
            }

        }
        private void VerifyOneOffPass(string openid, string weixinpass, int comid)
        {
            if (openid != "" && weixinpass != "")
            {

                string data = CrmMemberJsonData.WeixinLogin(openid, weixinpass, comid, out crminfo);

                if (data == "OK")//正确的一次性密码
                {
                    Session["AccountId"] = crminfo.Id;
                    Session["AccountKey"] = crminfo.Name;
                    Session["openid"] = crminfo.Idcard;

                    HttpCookie cookie = new HttpCookie("AccountId");     //实例化HttpCookie类并添加值
                    cookie.Value = crminfo.Id.ToString();
                    cookie.Expires = DateTime.Now.AddDays(120);
                    Response.Cookies.Add(cookie);
                    var returnmd5 = EncryptionHelper.ToMD5(crminfo.Idcard.ToString() + crminfo.Id.ToString(), "UTF-8");
                    cookie = new HttpCookie("AccountKey");     //实例化HttpCookie类并添加值
                    cookie.Value = returnmd5;
                    cookie.Expires = DateTime.Now.AddDays(120);
                    Response.Cookies.Add(cookie);

                    cookie = new HttpCookie("openid");     //实例化HttpCookie类并添加值
                    cookie.Value = openid;
                    cookie.Expires = DateTime.Now.AddDays(120);
                    Response.Cookies.Add(cookie);
                }
                else //错误的一次性密码
                {
                    GetUserByclient(comid);//通过客户端保存数据获得用户信息
                }
            }
            else
            {
                GetUserByclient(comid);//通过客户端保存数据获得用户信息
            }

            new B2bCrmData().WeixinConPass(openid, comid);//清空微信密码
        }

        private void GetUserByclient(int comid)
        {
            #region 客户端没有保存数据cookie,session ,不处理
            if (Request.Cookies["AccountId"] == null && Session["AccountId"] == null)
            {

            }
            #endregion 客户端保存了数据cookie,session ,处理
            #region
            else
            {
                if (Session["AccountId"] != null)
                {
                    int crmid = Session["AccountId"].ToString().ConvertTo<int>(0);
                    if (crmid > 0)
                    {
                        crminfo = new B2bCrmData().GetB2bCrmById(crmid);
                    }
                }
                else
                {
                    if (Request.Cookies["AccountId"] != null)
                    {
                        string accountmd5 = "";
                        int AccountId = int.Parse(Request.Cookies["AccountId"].Value);
                        if (Request.Cookies["AccountKey"] != null)
                        {
                            accountmd5 = Request.Cookies["AccountKey"].Value;
                        }

                        var data1 = CrmMemberJsonData.WeixinCookieLogin(AccountId.ToString(), accountmd5, comid, out crminfo);
                        if (data1 != "OK")
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
                        }
                        else
                        {
                            //从cookie中得到accountid,accountkey,openid
                            string openid = Request.Cookies["openid"].Value;
                            string accountid = Request.Cookies["AccountId"].Value;
                            string accountkey = Request.Cookies["AccountKey"].Value;

                            int crmid = accountid.ConvertTo<int>(0);
                            if (crmid > 0)
                            {
                                crminfo = new B2bCrmData().GetB2bCrmById(crmid);
                            }

                        }
                    }
                }
            }
            #endregion

        }

        private void GetOpenId(string codee, int comid)
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

                string data = new B2bCrmData().GetB2bCrm(openid, comid, out crminfo);
                if (data == "OK")
                {
                    Session["AccountId"] = crminfo.Id;
                    Session["AccountKey"] = crminfo.Name;
                    Session["openid"] = crminfo.Idcard;


                    HttpCookie cookie = new HttpCookie("AccountId");     //实例化HttpCookie类并添加值
                    cookie.Value = crminfo.Id.ToString();
                    cookie.Expires = DateTime.Now.AddDays(120);
                    Response.Cookies.Add(cookie);
                    var returnmd5 = EncryptionHelper.ToMD5(crminfo.Idcard.ToString() + crminfo.Id.ToString(), "UTF-8");
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
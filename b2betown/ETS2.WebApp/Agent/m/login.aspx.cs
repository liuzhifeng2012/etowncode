using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using System.Xml;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using Newtonsoft.Json;
using ETS.JsonFactory;

namespace ETS2.WebApp.Agent.m
{
    public partial class login : System.Web.UI.Page
    {
        public string Email = "";
        public string openid = "";//微信号
        public int comid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Email = Request["Email"].ConvertTo<string>("");

            //获得comid：为0则进入登录页面；非0则进入下面流程
            string RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();
            comid = GeneralFunc.GetComid(RequestUrl);
            if (comid == 0)
            {
                return;
            }
            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "RequestUrl:" + RequestUrl);
            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "comid:" + comid); 

            //以下3个参数用于获取微信号，并且通过判断实现手机分销系统免登陆
            string code = Request["code"].ConvertTo<string>("");
            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "code:" + code); 

            string openid = Request["openid"].ConvertTo<string>("");
            string weixinpass = Request["weixinpass"].ConvertTo<string>("");

            //如果3个参数都为空，则进入登录页面
            if (code == "" && openid == "" && weixinpass == "")
            {
                return;
            }

            //如果以上3个参数中有参数传递过来，则进入手机分销系统免登陆判断
            if (code != "")//根据微信端code获取微信号
            {
                openid = GetOpenId(code, comid);
                //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "openid:" + openid); 

            }
            else//根据微信一次性密码获取微信号
            {
                if (openid != "" && weixinpass != "")//微信端发送来的请求
                {

                    bool isavailable = VerifyOneOffPass(openid, weixinpass);
                    if (isavailable == false)
                    {
                        //微信一次性密码失效，判断传递过来的openid 和 客户端cookie中openid 是否相同（防止转发）:相同直接免登陆进入手机分销后台；否则进入登录手机分销页面 
                        HttpCookie cookie = Request.Cookies["openid"];
                        if (cookie != null)
                        {
                            string cookieValue = cookie.Value;
                             if (cookieValue != openid)
                            {
                                openid = "";
                            } 
                        }
                        //else { openid = openid; } //微信一次性密,可以不操作
                    }
                    //else { openid = openid; } //传递过来的openid 和 客户端cookie中openid相符,可以不操作

                }
                else//非微信端发送来的请求，需要进入登录页面 
                {
                    openid = "";
                }
            }

            if (openid != "")//如果微信号 获取成功，则获取当前微信号 微信免登陆 对应的分销商:有则免登陆；没有则需要登录
            {
                Agent_company freelandingagentuser = new AgentCompanyData().GetFreeLandingAgentUserByOpenId(openid);
               
                if (freelandingagentuser != null)
                {
                    Session["Agentid"] = freelandingagentuser.Id;//分销公司id，并不是分销员工id
                    Session["Account"] = freelandingagentuser.Account;
                    //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "agentid:" + freelandingagentuser.Id + ",account:" + freelandingagentuser.Account); 


                    HttpCookie cookie = new HttpCookie("Agentid");     //实例化HttpCookie类并添加值
                    cookie.Value = freelandingagentuser.Id.ToString();//分销公司id，并不是分销员工id
                    cookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(cookie);


                    var returnmd5 = EncryptionHelper.ToMD5(freelandingagentuser.Account + "lixh1210" + freelandingagentuser.Id.ToString(), "UTF-8");
                    cookie = new HttpCookie("AgentKey");     //实例化HttpCookie类并添加值
                    cookie.Value = returnmd5;
                    cookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(cookie);

                    cookie = new HttpCookie("Account");     //分销账户
                    cookie.Value = freelandingagentuser.Account;
                    cookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(cookie);

                    Response.Redirect("default.aspx");
                }

            }



        }
        /// <summary>
        /// 验证微信一次性密码 
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="weixinpass"></param>
        private bool VerifyOneOffPass(string openid, string weixinpass)
        {
            if (openid != null && openid != "" && weixinpass != "" && weixinpass != null)
            {
                B2b_crm b2bcrm = new B2b_crm();

                B2bCrmData dateuser = new B2bCrmData();
                string data = CrmMemberJsonData.WeixinLogin(openid, weixinpass, comid, out b2bcrm);


                //清空微信一次性密码
                new B2bCrmData().WeixinConPass(openid, comid);

                if (data == "OK")
                {
                    HttpCookie cookie = new HttpCookie("AccountId");     //实例化HttpCookie类并添加值
                    cookie.Value = b2bcrm.Id.ToString();
                    cookie.Expires = DateTime.Now.AddDays(120);
                    Response.Cookies.Add(cookie);

                    var returnmd5 = EncryptionHelper.ToMD5(b2bcrm.Idcard.ToString() + b2bcrm.Id.ToString(), "UTF-8");
                    cookie = new HttpCookie("AccountKey");     //实例化HttpCookie类并添加值
                    cookie.Value = returnmd5;
                    cookie.Expires = DateTime.Now.AddDays(120);
                    Response.Cookies.Add(cookie);

                    cookie = new HttpCookie("openid");     //实例化HttpCookie类并添加值
                    cookie.Value = openid;
                    cookie.Expires = DateTime.Now.AddDays(120);
                    Response.Cookies.Add(cookie);

                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据微信端返回code 获取用户openid
        /// </summary>
        /// <param name="codee"></param>
        /// <param name="comid"></param>
        /// <returns></returns>
        private string GetOpenId(string codee, int comid)
        {
            string openidd = "";
            WeiXinBasic basicc = new WeiXinBasicData().GetWxBasicByComId(comid);
            if (basicc != null)
            {
                string st = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + basicc.AppId + "&secret=" + basicc.AppSecret + "&code=" + codee + "&grant_type=authorization_code";
                XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + new GetUrlData().HttpGet(st) + "}");

                XmlElement rootElement = doc.DocumentElement;

                openidd = rootElement.SelectSingleNode("openid").InnerText;

                //根据微信号获取用户信息，使用户处于登录状态
                B2b_crm b2bcrm = new B2b_crm();
                string data = new B2bCrmData().GetB2bCrm(openidd, comid, out b2bcrm);
                if (data == "OK")
                {
                    HttpCookie cookie = new HttpCookie("AccountId");     //实例化HttpCookie类并添加值
                    cookie.Value = b2bcrm.Id.ToString();
                    cookie.Expires = DateTime.Now.AddDays(120);
                    Response.Cookies.Add(cookie);

                    var returnmd5 = EncryptionHelper.ToMD5(b2bcrm.Idcard.ToString() + b2bcrm.Id.ToString(), "UTF-8");
                    cookie = new HttpCookie("AccountKey");     //实例化HttpCookie类并添加值
                    cookie.Value = returnmd5;
                    cookie.Expires = DateTime.Now.AddDays(120);
                    Response.Cookies.Add(cookie);

                    cookie = new HttpCookie("openid");     //实例化HttpCookie类并添加值
                    cookie.Value = openidd;
                    cookie.Expires = DateTime.Now.AddDays(120);
                    Response.Cookies.Add(cookie);



                }
            }
            return openidd;
        }


    }
}
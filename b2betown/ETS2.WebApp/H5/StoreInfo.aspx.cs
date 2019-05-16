using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.Common.Business;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using FileUpload.FileUpload.Entities;
using FileUpload.FileUpload.Data;
using ETS.Framework;
using ETS2.WeiXin.Service.WeiXinService.Model;
using System.Xml;
using Newtonsoft.Json;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;
using ETS.JsonFactory;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.WebApp.H5
{
    public partial class StoreInfo : System.Web.UI.Page
    {

        public int comid = 0;//公司id
        public string RequestUrl = "";//访问网址通过访问网址判断商家
        public B2b_crm userinfo = new B2b_crm();
        public  Member_Channel_company menshi = new Member_Channel_company();//门店信息
        public string menshiimgurl = "";//门市图片
        public string errlog = "";//记录错误信息
        public int menshiid = 0;//门市ID号
        public int type = 0;
        public string Province = "北京";
        public string City = "";

        public string comlogo = "";

        public string Wxfocus_url = "";
        public string Wxfocus_author = "";
        public string weixinname = "";
        public string Scenic_intro = "";
        public string title = "";//访问网址通过访问网址判断商家

        protected void Page_Load(object sender, EventArgs e)
        {

             RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();
                    //根据域名读取商户ID
             if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式，则从多微信商户基本信息表中获取comid
             {
                 //先通过正则表达式获取COMid
                 comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl).ToString());
             }


            #region 传递过来门市id,通过门市id获得门市信息
            menshiid = Request["menshiid"].ConvertTo<int>(0);
            type = Request["type"].ConvertTo<int>(0);

            if (menshiid > 0)
            {
                menshi = new MemberChannelcompanyData().GetChannelCompany(menshiid.ToString());
                if (menshi != null)
                {
                    menshiimgurl = FileSerivce.GetImgUrl(menshi.Companyimg);
                    comid = menshi.Com_id;

                    if (menshi.Province != "")
                    {
                        Province = menshi.Province;
                    }else{
                        //如果门市没有设置地址按 商户设定地址
                        var comdata = B2bCompanyData.GetCompany(comid);
                        if (comdata != null) {
                            Province = comdata.B2bcompanyinfo.Province;
                        }
                    }

                   
                }
                else
                {
  
                    errlog = "获得门店信息为空";
                }
            }
            #endregion
            #region 1.传递过来微信号openid(微信未认证商户)；2.根据网页授权接口获得openid(微信已认证商户)
            else
            {

                //非门市读取商户信息
                if (comid != 0)
                {
                    var cominfo = B2bCompanyData.GetCompany(comid);
                    if (cominfo != null)
                    {
                        
                        menshi.Companystate = 0;
                        menshi.Com_id = comid;
                        menshi.Issuetype = 0;
                        menshi.Companyname = cominfo.Com_name;
                        menshi.Whethercreateqrcode = false;
                        menshi.Companystate = 1;//默认渠道公司为开通状态
                        menshi.Whetherdepartment = 0;//默认 非内部部门
                        menshi.Bookurl = "";
                        menshi.Companyaddress = cominfo.B2bcompanyinfo.Scenic_address;
                        menshi.Companyphone = cominfo.B2bcompanyinfo.Tel;
                        menshi.CompanyCoordinate = 0;
                        menshi.CompanyLocate = cominfo.B2bcompanyinfo.Coordinate;
                        menshi.Companyimg = 0;
                        menshi.Companyintro = cominfo.B2bcompanyinfo.Scenic_intro;
                        menshi.Companyproject = String.Empty;
                        menshi.City = cominfo.B2bcompanyinfo.City;
                        menshi.Province = "";
                        menshi.Selectstate = 0;

                    }
                }


                #region 已认证微信商户，访问网址规则类似shop101.etown.cn,根据网页授权接口获得openid
                string weixincode = Request["code"].ConvertTo<string>("");
                if (weixincode != "")
                {
                    RequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();
                    //根据域名读取商户ID
                    if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式，则从多微信商户基本信息表中获取comid
                    {
                        //先通过正则表达式获取COMid
                        comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl).ToString());
                        if (comid == 0)
                        {
                            comid = new WeiXinBasicData().GetWeiXinBasicByDomain(RequestUrl).Comid;
                        }
                        string openid = GetOpenId_2(weixincode, comid);

                        GetMenshiDetail(openid, comid);
                    }
                    else
                    {
                        errlog = "公司id获取有误";
                    }

                }
                #endregion
                #region 未认证微信商户，访问网址不限制，需要传递参数openid
                else
                {
                    string openid = Request["openid"].ConvertTo<string>("");
                    //根据openid得到会员信息
                    B2b_crm crm = new B2bCrmData().GetB2bCrmByWeiXin(openid);
                    if (crm != null)
                    {
                        GetMenshiDetail(openid, crm.Com_id);
                        comid = crm.Com_id;
                    }
                    else
                    {
                        errlog = "根据微信号得到会员信息为空";
                    }
                }
                #endregion
            }
            #endregion


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
            }



        }
        #region 根据微信号得到用户信息，公司/门店信息
        private void GetMenshiDetail(string openid, int comid)
        {
            if (openid == "")
            {
                errlog = "未获得微信号";
            }
            else
            {
                ////判断用户身份:1.公司所属门店会员;2.公司会员
                //Member_Card card = new MemberCardData().GetMemberCardByOpenId(openid);
                //if (card != null)
                //{
                //    if (card.IssueCard > 0)//门店会员
                //    {
                //        menshi = new MemberChannelcompanyData().GetMenShiByOpenId(openid, comid);
                //        if (menshi != null)
                //        {
                //            menshiimgurl = FileSerivce.GetImgUrl(menshi.Companyimg);
                //        }
                //        else
                //        {
                //            errlog = "获得门店信息为空";
                //        }
                //    }
                //    else //公司会员
                //    {
                //        Response.Redirect("OrderinfoTitle.aspx?id=" + comid);
                //    }
                //}
                //else
                //{
                //    errlog = "未判断初用户身份";
                //}
            }
        }
        #endregion
        #region 微信认证商户，通过网页授权接口获得openid
        private string GetOpenId_2(string codee, int comid)
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
                    return openid;

                }
                catch
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        #endregion



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
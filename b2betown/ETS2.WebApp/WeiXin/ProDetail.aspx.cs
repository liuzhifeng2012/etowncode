using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS.Framework;
using ETS2.Common.Business;
using FileUpload.FileUpload.Data;
using ETS2.Member.Service.MemberService.Data;
using System.Net;
using System.Net.Sockets;
using ETS.JsonFactory;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Member.Service.MemberService.Model;
using ETS2.CRM.Service.CRMService.Modle;
using System.Xml;
using Newtonsoft.Json;

namespace ETS2.WebApp.WeiXin
{
    public partial class ProDetail : System.Web.UI.Page
    {
        protected int materialid = 0;
        public string phone_tel = "";
        public string fileUrl = AppSettings.CommonSetting.GetValue("FileUpload/FileUrl").ConvertTo<string>();//文件访问地址
        public string headPortraitImgSrc = "";

        public string title = "";
        public string thisday = "";
        public string article = "";
        public string phone = "";
        public string price = "";
        public string datetime = "";

        public string summary = "";
        public int id;
        public string nowdate = "";//现在日期

        //期
        public int percal = 0;
        public int peryear = 0;
        public int wxtype = 0;
        public int percalid = 0;

        public int comid = 0;

        public string Author = "";
        public string Articleurl = "";
        public int uid = 0;

        public string uip = "";
        public int forward = 0;

        B2b_crm userinfo = null;


        public string authorpayurl = ""; //作者关注链接
        protected void Page_Load(object sender, EventArgs e)
        {
            uid = Request["uid"].ConvertTo<int>(0);
            materialid = Request["materialid"].ConvertTo<int>(0);

            //获取IP地址
            uip = CommonFunc.GetRealIP();

            WxMaterial wxmaterial = new WxMaterialData().GetWxMaterial(materialid);
            nowdate = DateTime.Now.ToString("yyyy-MM-dd");

            string weixincode = Request["code"].ConvertTo<string>("");
            string openid = Request["openid"].ConvertTo<string>("");



            if (wxmaterial != null)
            {
                authorpayurl = wxmaterial.Authorpayurl;
                comid = wxmaterial.Comid;
                Author = wxmaterial.Author;

                id = wxmaterial.MaterialId;
                title = wxmaterial.Title;
                thisday = DateTime.Now.ToString("yyyy-MM-dd");
                article = wxmaterial.Article;
                Articleurl = wxmaterial.Articleurl;
                phone_tel = wxmaterial.Phone;
                phone = "客服电话：";
                price = wxmaterial.Price.ToString();
                datetime = wxmaterial.Operatime.ToString("yyyy-MM-dd");

                if (price == "0.00" || price == "0")
                {
                    price = "";
                }
                else
                {
                    price = price.IndexOf(".") != -1 ? price.Substring(0, price.IndexOf(".")) : price;
                    price = "￥" + price;
                }



                //期
                wxtype = wxmaterial.SalePromoteTypeid;
                percalid = wxmaterial.Periodicalid;
                periodical period = new WxMaterialData().selectperiodical(percalid);
                if (period != null)
                {
                    percal = period.Percal;
                    peryear = period.Peryear;
                }

                summary = wxmaterial.Summary;
                var identityFileUpload = new FileUploadData().GetFileById(wxmaterial.Imgpath.ToString().ConvertTo<int>(0));
                if (identityFileUpload != null)
                {
                    if (identityFileUpload.Relativepath != "")
                    {

                        headPortraitImgSrc = fileUrl + identityFileUpload.Relativepath;
                    }
                }
                else
                {
                    headPortraitImgSrc = "";
                }


                //微信转发访问统计
                if (uid != 0)//必须记录转发用户信息才能继续统计
                {

                    //查询是否有此cookies，有此cooki则证明已经访问过的用户
                    if (Request.Cookies["wxact" + materialid.ToString()] == null)
                    {

                        forward = new MemberForwardingData().Forwardingcount(uid, materialid, uip, comid);

                        if (forward > 0)
                        {
                            HttpCookie cookie = new HttpCookie("wxact" + materialid.ToString());     //实例化HttpCookie类并添加值
                            cookie.Value = "yes";
                            cookie.Expires = DateTime.Now.AddDays(365);
                            Response.Cookies.Add(cookie);
                        }

                    }


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

                            cookie = new HttpCookie("ZF_WxmaterialId");     //实例化HttpCookie类并添加值
                            cookie.Value = materialid.ToString();
                            cookie.Expires = DateTime.Now.AddDays(120);
                            Response.Cookies.Add(cookie);
                        }
                    }


                }
            }


            if (comid != 0)
            {

                if (weixincode != "")
                {
                    int questtype = 2;//1=微信授权验证
                    DealUserinfo1("", weixincode, comid, questtype);
                }
                else if (openid != "")
                {
                    string weixinpass = Request["weixinpass"].ConvertTo<string>("");
                    int questtype = 1;//1=一次性密码验证
                    DealUserinfo1(openid, weixinpass, comid, questtype);//判断用户微信号(1,点击的转发链接进来的2，点击微信菜单进来的)；使用户处于登录状态（不包括点击转发链接的）
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
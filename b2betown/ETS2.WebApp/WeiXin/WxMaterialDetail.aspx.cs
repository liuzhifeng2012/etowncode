using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FileUpload.FileUpload.Entities.Enum;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using FileUpload.FileUpload.Data;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.Common.Business;
using System.Net;
using System.Net.Sockets;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Member.Service.MemberService.Model;
using System.Xml;
using Newtonsoft.Json;
using ETS.JsonFactory;
using ETS2.WeiXin.Service.WeiXinService.Model.Enum;
using ETS2.WeiXin.Service.WinXinService.BLL;

namespace ETS2.WebApp.WeiXin
{
    public partial class WxMaterialDetail : System.Web.UI.Page
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

        public string summary = "";
        public string Articleurl = "";
        public int id;

        public int comid = 0;
        public int uid = 0;
        public string uip = "";
        public string comlogo = "";

        public string Author = "";
        public string datetime = "";
        B2b_crm userinfo = null;
        public string Wxfocus_url = "";
        public string Wxfocus_author = "";
        public string weixinname = "";
        public string Scenic_intro = "";
        public string Com_name = "";
        public string authorpayurl = "";//作者关注链接

        #region 以下语音播放接口会用到
        public string openid = "";
        //public int comid = 0;
        public int clientuptypemark = (int)Clientuptypemark.DownMaterialVoice;//下载 微信客户端上传类型标记
        //public int materialid = 0;//素材id

        //public string username = "";//顾问名称
        public string materialname = "";//素材名称 

        public string appId = "";
        public long timestamp = 0;
        public string nonceStr = "";
        public string signature = "";//jsapi签名

        public int isrightwxset = 1;//微信接口设置信息是否正确
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

            materialid = Request["materialid"].ConvertTo<int>(0);
            uid = Request["uid"].ConvertTo<int>(0);
            //获取IP地址
            uip = CommonFunc.GetRealIP();

            WxMaterial wxmaterial = new WxMaterialData().GetWxMaterial(materialid);

            string weixincode = Request["code"].ConvertTo<string>("");
            openid = Request["openid"].ConvertTo<string>("");


            #region 素材信息
            if (wxmaterial != null)
            {
                authorpayurl = wxmaterial.Authorpayurl;
                Author = wxmaterial.Author;
                datetime = wxmaterial.Operatime.ToString("yyyy-MM-dd");

                comid = wxmaterial.Comid;

                id = wxmaterial.MaterialId;
                title = wxmaterial.Title;
                thisday = DateTime.Now.ToString("yyyy-MM-dd");
                article = wxmaterial.Article;
                phone_tel = wxmaterial.Phone;
                Articleurl = wxmaterial.Articleurl;
                phone = "客服电话：";
                price = wxmaterial.Price.ToString();

                if (price == "0.00" || price == "0")
                {
                    price = "";
                }
                else
                {
                    price = price.IndexOf(".") != -1 ? price.Substring(0, price.IndexOf(".")) : price;
                    price = "￥" + price + "元-";
                }



                summary = wxmaterial.Summary;
                var identityFileUpload = new FileUploadData().GetFileById(wxmaterial.Imgpath.ToString().ConvertTo<int>(0));
                if (identityFileUpload != null)
                {
                    if (identityFileUpload.Relativepath != "")
                    {

                        headPortraitImgSrc = fileUrl + identityFileUpload.Relativepath;

                    }
                    else
                    {
                        headPortraitImgSrc = "";
                    }
                }
            }
            #endregion

            #region 微信转发访问统计
            if (uid != 0)//必须记录转发用户信息才能继续统计
            {
                //查询是否有此cookies，有此cooki则证明已经访问过的用户
                if (Request.Cookies["wxact" + materialid.ToString()] == null)
                {
                    var forward = new MemberForwardingData().Forwardingcount(uid, materialid, uip, comid);
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
            #endregion



            if (comid != 0)
            {
                #region  微信语音播放接口会用到
                string url = Request.Url.ToString();

                //根据传入参数openid、comid 得到 access_token、jsapi_ticket、noncestr、timestamp、 url（当前网页的URL，不包含#及其后面部分）
                WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(comid);
                if (basic != null)
                {
                    appId = basic.AppId;
                    timestamp = new WeixinVoiceUtil().CreatenTimestamp();
                    nonceStr = new WeixinVoiceUtil().CreatenNonce_str();

                    WXAccessToken maccesstoken = new WeixinVoiceUtil().GetAccessToken(comid, basic.AppId, basic.AppSecret);
                    if (maccesstoken != null)
                    {
                        string jsapi_ticket = new WeixinVoiceUtil().GetTickect(maccesstoken.ACCESS_TOKEN, comid);
                        if (jsapi_ticket == "")
                        {
                            isrightwxset = 0;
                        }
                        else
                        {
                            string beforesha1_signature = "";
                            signature = new WeixinVoiceUtil().GetSignature(jsapi_ticket, nonceStr, timestamp, url, out beforesha1_signature);

                        }
                    }
                    else
                    {
                        isrightwxset = 0;
                    }
                }
                else
                {
                    isrightwxset = 0;
                }
                #endregion



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
                    Com_name = commodel.Com_name;
                }


                B2b_company_saleset pro = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
                if (pro != null)
                {
                    if (pro.Smalllogo != null && pro.Smalllogo != "")
                    {
                        comlogo = FileSerivce.GetImgUrl(pro.Smalllogo.ConvertTo<int>(0));
                    }
                }




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
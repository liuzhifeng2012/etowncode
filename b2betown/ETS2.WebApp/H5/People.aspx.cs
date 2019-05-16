using System;
using System.Collections.Generic;
using System.Web;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Common.Business;
using ETS.Framework;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Member.Service.MemberService.Model;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using System.Xml;
using Newtonsoft.Json;
using ETS.JsonFactory;
using ETS2.WeiXin.Service.WeiXinService.Model.Enum;
using ETS2.WeiXin.Service.WinXinService.BLL;

namespace ETS2.WebApp.H5
{
    public partial class People : System.Web.UI.Page
    {

        public int MasterId = 0;//员工账户ID
        public int channelid = 0;//渠道ID
        public string MasterName = "";
        public string CompanyName = "";
        public string Tel = "";
        public int Viewtel = 1;
        public string GroupNames = "";
        public string GroupIds = "";
        public int CreateUserId = 0;
        public int EmployeState = 0;
        public string Job = "";
        public string Selfbrief = "";
        public string Headimgurl = "";
        public int Workingyears = 0;

        public string Workdays = "";
        public int WorkdaysView = 0;//是否为设定的工作日判断是否显示
        public string Workdaystime = "";
        public string Workendtime = "";
        public string Fixphone = "";
        public string Email = "";
        public string Homepage = "";
        public string Weibopage = "";
        public string QQ = "";
        public string Weixin = "";
        public string Selfhomepage_qrcordurl = "";

        public int? ChannelCompanyId;
        public string ChannelCompanyName = "";
        public int? Channelsource;
        Member_Channel_company ChannelCompany = null;

        public string zaixianzhuangtai = "未在线";

        public string daohang_html = "";
        public int firstdaohang = 0;
        public int firstmenuid = 0;
        public int isoutpro = 0;//是否是外部接口产品：0不是；1是
        public int comid = 0;

        B2b_company cominfo = null;
        public string linkguanzhu = "";
        public string author = "";
        public int yonghustate = 0; //0未关注(未登陆),1=已关注微信客服，2==已关注，非匹配顾问（或顾问未在线） 
        public B2b_crm userinfo = new B2b_crm();
        public string weixinopenid = "";

        public string come = "";

        public string new_weixinpass;//新的微信一次性密码
        public string title = "";
        public string comlogo = "";

        public string guwenweixin = "";//顾问微信

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

            MasterId = Request["MasterId"].ConvertTo<int>(0);
            come = Request["come"].ConvertTo<string>("");
            B2b_company_manageuser manageruser = B2bCompanyManagerUserData.GetUser(MasterId);
            if (manageruser != null)
            {

                MasterId = manageruser.Id;

                MasterName = manageruser.Employeename;
                CompanyName = B2bCompanyData.GetCompanyByUid(manageruser.Id).Com_name;
                Tel = manageruser.Tel;
                Viewtel = manageruser.Viewtel;
                comid = manageruser.Com_id;
                B2bCrmData crmdata = new B2bCrmData();
                var crmmodel = crmdata.GetB2bCrmByPhone(manageruser.Com_id, Tel);
                if (crmmodel != null)
                {
                    if (crmmodel.Weixin != "")
                    {
                        zaixianzhuangtai = "我在线上";
                        guwenweixin = crmmodel.Weixin;
                    }
                    else
                    {
                        zaixianzhuangtai = "未在线";
                    }
                }

                var commodel = B2bCompanyData.GetCompany(comid);

                if (commodel != null)
                {
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



                ChannelCompanyId = manageruser.Channelcompanyid;
                ChannelCompany = new MemberChannelcompanyData().GetCompanyById(manageruser.Channelcompanyid.ToString().ConvertTo<int>(0));

                if (ChannelCompany != null)
                {
                    ChannelCompanyName = ChannelCompany.Companyname;
                }

                //获取渠道ID
                MemberChannelData channeldata = new MemberChannelData();
                var channelmodel = channeldata.GetPhoneComIdChannelDetail(Tel, comid);
                if (channelmodel != null)
                {
                    channelid = channelmodel.Id;
                }




                Channelsource = manageruser.Channelsource;
                CreateUserId = manageruser.Createuserid;
                EmployeState = manageruser.Employeestate;
                Job = manageruser.Job;
                Selfbrief = manageruser.Selfbrief;
                if (Selfbrief != "" && Selfbrief != null)
                {
                    if (Selfbrief.Length > 21)
                    {
                        Selfbrief = Selfbrief.Substring(0, 21) + "...";
                    }
                }


                //Headimg=manageruser.Headimg;
                Headimgurl = FileSerivce.GetImgUrl(manageruser.Headimg);
                Workingyears = manageruser.Workingyears;
                Workdays = manageruser.Workdays;

                WorkdaysView = crmdata.WorkDay(Workdays);//判断是否在工作日内，1为在，0为不在




                Workdaystime = manageruser.Workdaystime;
                Workendtime = manageruser.Workendtime;
                Fixphone = manageruser.Fixphone;
                Email = manageruser.Email;
                Homepage = manageruser.Homepage;
                Weibopage = manageruser.Weibopage;
                QQ = manageruser.QQ;
                Weixin = manageruser.Weixin;
                Selfhomepage_qrcordurl = manageruser.Selfhomepage_qrcordurl;

                //获取公司信息（微信连接地址）
                cominfo = B2bCompanyData.GetAllComMsg(comid);
                if (cominfo != null)
                {
                    linkguanzhu = cominfo.B2bcompanyinfo.Wxfocus_url;
                    author = cominfo.B2bcompanyinfo.Wxfocus_author;
                }

                //导航
                var imagedata = new B2bCompanyMenuData();
                int totalcount = 0;

                List<B2b_company_menu> list = imagedata.GetconsultantList(manageruser.Com_id, 1, 10, out totalcount);
                if (list != null)
                {


                    for (int i = 0; i < list.Count; i++)
                    {
                        var imageurl = FileSerivce.GetImgUrl(list[i].Imgurl);



                        if (firstmenuid == 0)
                        {
                            firstdaohang = list[i].Linktype;
                            firstmenuid = list[i].Id;
                            isoutpro = list[i].Outdata;
                        }
                        if (daohang_html == "")
                        {
                            daohang_html += "<li class=\"curr\" menu-id=\"" + list[i].Id + "\"  data-id=\"" + list[i].Linktype + "\"  data-isoutpro=\"" + list[i].Outdata + "\"><!--<img src=\"" + imageurl + "\" height=\"60px\">--><span>" + list[i].Name + "</span></li>";
                        }
                        else
                        {
                            daohang_html += "<li class=\"\" menu-id=\"" + list[i].Id + "\"  data-id=\"" + list[i].Linktype + "\"   data-isoutpro=\"" + list[i].Outdata + "\"><!--<img src=\"" + imageurl + "\" height=\"60px\">--><span>" + list[i].Name + "</span></li>";
                        }
                        //daohang_Imgurl_address = FileSerivce.GetImgUrl(list.Imgurl);
                        //daohang_Imgurl = list.Imgurl;
                        ///daohang_Linkurl = list.Linkurl;
                        //daohang_Name = list.Name;
                        //daohang_Fonticon = list.Fonticon;
                    }



                }


            };


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



            //创建新的微信一次性密码
            new_weixinpass = new B2bCrmData().WeixinGetPass(weixinopenid, comid);


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
                weixinopenid = userinfo.Weixin;

                if (weixinopenid == "")
                {
                    yonghustate = 0;
                }
                else if (weixinopenid != "" && zaixianzhuangtai == "我在线上")
                {
                    MemberCardData carddata = new MemberCardData();
                    var cardmodel = carddata.GetCardByCardNumber(decimal.Parse(idcard.ToString()));
                    if (cardmodel != null)
                    {
                        if (cardmodel.IssueCard == channelid)
                        {
                            yonghustate = 1;
                        }
                        else
                        {
                            yonghustate = 2;
                        }

                    }
                    else
                    {
                        yonghustate = 2;
                    }
                }
                else
                {
                    yonghustate = 3;
                }


                dateuser.WeixinConPass(userinfo.Weixin, comid);//只要包含SESSION登陆成功，清空微信密码
            }


        }

    }
}
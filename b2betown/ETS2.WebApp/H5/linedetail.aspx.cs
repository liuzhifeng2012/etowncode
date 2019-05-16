using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Data;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Member.Service.MemberService.Data;
using ETS.JsonFactory;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;
using System.Xml;
using Newtonsoft.Json;
using ETS2.Member.Service.MemberService.Model;

namespace ETS2.WebApp.H5
{
    public partial class linedetail : System.Web.UI.Page
    {
        public int lineid = 0;
        public string pro_name = "";
        public int id;
        public int com_id=0;
        public int comid = 0;
        public int pro_state;
        public int server_type;
        public int pro_type;
        public int source_type;
        public string pro_Remark = String.Empty;
        public DateTime pro_start;
        public DateTime pro_end;
        public decimal face_price;
        public decimal advise_price;
        private decimal agent1_price;
        public decimal agent2_price;
        public decimal agent3_price;
        public decimal agentsettle_price;
        public int thatDay_can;
        public int thatday_can_day;
        public string service_Contain = String.Empty;
        public string service_NotContain = String.Empty;
        public string precautions = String.Empty;
        public int tuan_pro;
        public int zhixiao;
        public int agentsale;
        public DateTime createtime;
        public int createuserid;
        public string sms = String.Empty;
        public int imgurl;
        public int pro_number;
        public string pro_explain;
        public int totalpay;
        public int u_num;
        public decimal totalpay_price;
        public decimal totalprofit;
        public int tuipiao;
        public int tuipiao_guoqi;
        public int tuipiao_endday;
        public int proclass;
        public int projectid;
        public string imgaddress;

        public int travelproductid;//产品编号

        public string travelstarting;//出发地
        public string travelending;//目的地


        public int ispanicbuy = 0;
        public DateTime panic_begintime ;
        public DateTime panicbuy_endtime ;
        public int limitbuytotalnum = 0;

        public DateTime nowdate;
        public int shijiacha = 0;


        public string  companyname ;
        public string tel ;


        public int uid = 0;
        public string uip = "";

        public string Wxfocus_url = "";
        public string Wxfocus_author = "";
        B2b_crm userinfo;

        public decimal childreduce = 0;//儿童减免费用
        protected void Page_Load(object sender, EventArgs e)
        {
            lineid = Request["lineid"].ConvertTo<int>(0);
            uid = Request["uid"].ConvertTo<int>(0);
            //获取IP地址
            uip = CommonFunc.GetRealIP();
            nowdate = DateTime.Now;
            

            if (lineid != 0) {
                var prodata = new B2bComProData();
                var pro = prodata.GetProById(lineid.ToString());

                if (pro != null)
                {
                    //作废超时未支付订单，完成回滚操作
                    int rs = new B2bComProData().CancelOvertimeOrder(pro);

                    childreduce = pro.Childreduce;
                    
                    pro_name = pro.Pro_name;
                    com_id = pro.Com_id;
                    comid = pro.Com_id;
                            pro_state =  pro.Pro_state;
                            server_type  =  pro.Server_type;
                            pro_type  =  pro.Pro_type;
                            source_type  =  pro.Source_type;
                            pro_Remark  =  pro.Pro_Remark;
                            pro_start  =  pro.Pro_start;
                            pro_end  =  pro.Pro_end;
                            face_price  =  pro.Face_price;
                            advise_price  =  pro.Advise_price;
                            agent1_price  =  pro.Agent1_price;
                            agent2_price  =  pro.Agent2_price;
                            agent3_price  =  pro.Agent3_price;
                            agentsettle_price  =  pro.Agentsettle_price;
                            thatDay_can  =  pro.ThatDay_can;
                            thatday_can_day  =  pro.Thatday_can_day;
                            service_Contain  =  pro.Service_Contain;
                            service_NotContain  =  pro.Service_NotContain;
                            precautions  =  pro.Precautions;
                            tuan_pro  =  pro.Tuan_pro;
                            zhixiao  =  pro.Zhixiao;
                            agentsale  =  pro.Agentsale;
                            createtime  =  pro.Createtime;
                            sms  =  pro.Sms;
                            createuserid  =  pro.Createuserid;
                            imgurl  =  pro.Imgurl;
                            pro_number =  pro.Pro_number;
                            pro_explain = pro.Pro_explain ;
                            tuipiao = pro.Tuipiao ;
                            tuipiao_guoqi = pro.Tuipiao_guoqi ;
                            tuipiao_endday  =  pro.Tuipiao_endday;
                            imgaddress = FileSerivce.GetImgUrl(pro.Imgurl);
                            projectid = pro.Projectid ;
                            travelproductid = pro.Travelproductid;
                            travelstarting = pro.Travelstarting;
                            travelending = pro.Travelending;
                            ispanicbuy = pro.Ispanicbuy;
                            panic_begintime = pro.Panic_begintime;
                            panicbuy_endtime = pro.Panicbuy_endtime;
                            limitbuytotalnum = pro.Limitbuytotalnum;

                            if (ispanicbuy == 1)
                            {
                                TimeSpan tss = panic_begintime - nowdate;
                                var day = tss.Days * 24 * 3600; ;      //这是相差的天数
                                var h = tss.Hours * 3600;              //这是相差的小时数，
                                var m = tss.Minutes * 60;
                                var s = tss.Seconds;
                                shijiacha = day + h + m + s;
                            }
                }



               

            }

            if (com_id != 0)
            {
                B2b_company company = B2bCompanyData.GetCompany(com_id);
                if (company != null)
                {
                    companyname = company.Com_name;
                    tel = company.B2bcompanyinfo.Tel;
                }
                //获取渠道门市电话，当用户为门市会员则调取门市电话 tel
            }




            if (comid != 0)
            {

                var commodel = B2bCompanyData.GetCompany(comid);

                if (commodel != null) {
                    if (commodel.B2bcompanyinfo != null) {
                        Wxfocus_url = commodel.B2bcompanyinfo.Wxfocus_url;
                        Wxfocus_author = commodel.B2bcompanyinfo.Wxfocus_author; ;
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


            
            //微信转发访问归属渠道
            if (uid != 0)//必须记录转发用户信息才能继续统计
            {
                
                //判断有转发人的渠道
                var crmdata = new B2bCrmData();
                var pro = crmdata.Readuser(uid, com_id);//读取转发人用户信息
                string zhuanfa_phone = "";
                if (pro != null)
                {
                    zhuanfa_phone = pro.Phone;
                }

                if (zhuanfa_phone != "")
                { //转发人手机存在
                    MemberChannelData channeldata = new MemberChannelData();
                    var channeinfo = channeldata.GetPhoneComIdChannelDetail(zhuanfa_phone, com_id);//查询渠道
                    if (channeinfo != null)
                    {
                        //转发人渠道记录COOKI
                        HttpCookie cookie = new HttpCookie("ZF_ChanneId");     //实例化HttpCookie类并添加值
                        cookie.Value = channeinfo.Id.ToString();
                        cookie.Expires = DateTime.Now.AddDays(120);
                        Response.Cookies.Add(cookie);
                    }
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

                //读取渠道商户名称

                var channelcompany = new MemberChannelcompanyData().GetChannelCompanyByCrmId (userinfo.Id);
                if (channelcompany != null) {
                   tel = channelcompany.Companyphone;
                }


                dateuser.WeixinConPass(userinfo.Weixin, comid);//只要包含SESSION登陆成功，清空微信密码
            }


        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.Common.Business;
using ETS2.WeiXin.Service.WeiXinService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using FileUpload.FileUpload.Data;
using System.IO;
using ETS2.WeiXin.Service.WeiXinService.Model;
using System.Xml;
using Newtonsoft.Json;
using ETS.JsonFactory;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;
using Com.Tenpay.TenpayAppV3;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.VAS.Service.VASService.Data;
using System.Text;
using ETS2.PM.Service.PMService.Modle.Enum;


namespace ETS2.WebApp.wxpay
{
    public partial class micromart_orderpay : System.Web.UI.Page
    {
        #region 辨别商家参数
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        public int comid = 0;
        #endregion
        public string comName = "";
        public string title = "";
        public string comlogo = "";
        public string key = "";
        public int proclass = 0;
        public Decimal price = 0;

        public string Wxfocus_url = "";
        public string Wxfocus_author = "";

        public B2b_crm userinfo = null;

        public string openid = "";//微信号

        public int uid = 0;
        public string uip = "";

        public int buyuid = 0; //购买用户ID
        public int tocomid = 0;//来访商户COMID
        public string biaoti = "在线预订";



        public string id = "";
        public int cart_id = 0;
        public int num = 1;
        public string cart_num = "";//购物车 订购数量
        public string proid = "";

        public string summary = "";
        public string sumaryend = "";
        public string nowdate = "";//现在日期
        public DateTime nowtoday;

        public DateTime panic_begintime;
        public DateTime panicbuy_endtime;
        public int ispanicbuy = 0;//抢购判断


        public string ordertype = "1";//订单类型 默认为1订单；2充值
        public string costprice = "";//成本价
        public string lirun_price = "";//利润

        public string remark = "";

        public int pro_num = 0;
        public int pro_max = 0;
        public int pro_min = 0;
        public string pro_explain = "";
        public string phone = "";

        public Decimal Imprest = 0;
        public Decimal Integral = 0;
         public Decimal Express = 0;//快递
         public Decimal pro_price = 0;//合计
         public decimal pay_price = 0;//单价
        
        public string weixinpass = "";//微信一次性密码

        public int shijiacha = 0;

        public string provalidatemethod;//判断 1按产品有效期，2指定有效期
        public int appointdate;//1=一星期，，2=1个月，3=3个月，4=6个月，5=一年
        public int iscanuseonsameday;//1当天可用，0当天不可用


        public DateTime pro_end;
        public DateTime pro_start;
        public string pro_youxiaoqi = "";


        public int projectid = 0;//项目id
        public string pro_name = "";


        public int iscanbook = 1;//产品是否可以预订

        public decimal childreduce = 0;//儿童减免费用
        public int pro_servertype = 1;//产品服务类型1.票务10.旅游大巴

        public string pickuppoint = "";//上车地点
        public string dropoffpoint = "";//下车地点

        public string imgurl = "";
        public string weixinname = "";
        public string logoimg = "image/shop.png!60x60.jpg";
        public string Scenic_intro = "";

        public string appId = "";//公众号appID
        public string appsecret = "";
        public string addrSign = "";//签名
        public string timeStamp = "";//时间戳
        public string nonceStr = "";//随机字符串
        public string access_tokenstring = "";

        public string selfrefreshurl = "";//自刷新url
        public string code = "";//微信端返回的code

        public bool bo = true;
        public int cart = 0;//0是非购物车
        public string userid = "";

        public string address="";

        public bool issetfinancepaytype = false;//是否设置了微信支付参数
        public int order_state = 0;
        public string orderstatus = "";//订单状态
        public string subtime = "";

        public int Ispanicbuy = 0;
        public int Server_type = 0;
        public int paystate = 0;

        public void Page_Load(object sender, EventArgs e)
        {

            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            bo = detectmobilebrowser.HttpUserAgent(u);


            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "Request.Url:" + Request.Url.ToString());

            id = Request["id"].ConvertTo<string>("");
            string temp_id = Request["id"].ConvertTo<string>("");


            if (Request.Cookies["temp_userid"] != null)
            {
                userid = Request.Cookies["temp_userid"].Value;
            }
            else
            {
                userid = Domain_def.HuoQu_Temp_UserId();
                //Response.Cookies("userid").val();

                HttpCookie cookie = new HttpCookie("temp_userid");     //实例化HttpCookie类并添加值
                cookie.Value = userid;
                cookie.Expires = DateTime.Now.AddDays(365);
                Response.Cookies.Add(cookie);
            }


            //取消超时订单
            B2b_com_pro pro_cannelorder = new B2b_com_pro();
            pro_cannelorder.Server_type = 0;
            int rs_cannelorder = new B2bComProData().CancelOvertimeOrder(pro_cannelorder);



            num = Request["num"].ConvertTo<int>(1);

            string aRequestUrl = Request.ServerVariables["SERVER_NAME"].ToLower();

            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "aRequestUrl:" + aRequestUrl);
            if (Domain_def.Domain_yanzheng(aRequestUrl))//如果符合shop101.etown.cn的格式
            {
                //先通过正则表达式获取COMid
                comid = Int32.Parse(Domain_def.Domain_Huoqu(aRequestUrl).ToString());
            }

            if (comid == 0)//如果非标准格式，查询 是否有绑定的域名
            {
                var domaincomid = B2bCompanyData.GetComId(RequestUrl);
                if (domaincomid != null)
                {
                    comid = domaincomid.Com_id;
                }
            }




            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "comid:" + comid);
            #region 判断是否含有微信端传递过来的code值，不含有自刷新
            code = Request.QueryString["code"].ConvertTo<string>("");
            if (code == "")
            {
                selfrefreshurl = WeiXinJsonData.GetFollowOpenLinkUrlAboutPay(comid, "http://shop" + comid + ".etown.cn/wxpay/micromart_pay_" + num + "_" + id + ".aspx");
                //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "selfrefreshurl:" + selfrefreshurl);
                //Response.Redirect(refreshurl);
            }
            #endregion


            //buyuid = Request["buyuid"].ConvertTo<int>(0);
            //tocomid = Request["tocomid"].ConvertTo<int>(0);
            //uid = Request["uid"].ConvertTo<int>(0);
            //获取IP地址
            uip = CommonFunc.GetRealIP();


            nowdate = DateTime.Now.ToString("yyyy-MM-dd");
            nowtoday = DateTime.Now;

            if (temp_id != "")
            {
                B2bOrderData orderdata=new B2bOrderData();
                var pro = orderdata.GetOrderById( int.Parse(temp_id));
                if (pro != null)
                {

                    orderstatus = EnumUtils.GetName((OrderStatus)pro.Order_state);
                    order_state = pro.Order_state;
                    subtime = pro.U_subdate.ToString("yyyy/MM/dd hh:mm:ss");
                    paystate = pro.Pay_state;

                    #region 微信端 共享收货地址接口 参数获取

                    //根据产品判断商家是否含有自己的微信支付:a.含有的话支付到商家；b.没有的话支付到平台的微信公众号账户中
                    B2b_finance_paytype model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(pro.Comid);
                    //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "b");
                    if (model != null)
                    {
                        //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "c");
                        //商家微信支付的所有参数都存在
                        if (model.Wx_appid != "" && model.Wx_appkey != "" && model.Wx_partnerid != "" && model.Wx_paysignkey != "")
                        {
                            appId = model.Wx_appid;
                            appsecret = model.Wx_appkey;
                            //appkey = model.Wx_paysignkey;
                            //mchid = model.Wx_partnerid;
                            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "d");
                            issetfinancepaytype = true;
                            if (code != "")
                            {

                                string url =
                                    string.Format(
                                        "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",
                                        appId, appsecret, code);
                                string returnStr = HttpUtil.Send("", url);
                                //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "returnStr:" + returnStr);

                                var obj = JsonConvert.DeserializeObject<ModelOpenID>(returnStr);
                                if (obj.openid == null)
                                {
                                    //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "微信转发页面打开的,returnStr:" + returnStr);

                                    selfrefreshurl = WeiXinJsonData.GetFollowOpenLinkUrlAboutPay(comid, "http://shop" + comid + ".etown.cn/wxpay/micromart_pay_" + num + "_" + id + ".aspx");
                                    Response.Redirect(selfrefreshurl);
                                }

                                if (obj != null)
                                {
                                    timeStamp = TenpayUtil.getTimestamp();
                                    nonceStr = TenpayUtil.getNoncestr();
                                    access_tokenstring = obj.access_token;
                                    openid = obj.openid;

                                    HttpCookie newCookie = new HttpCookie("openid");
                                    //往Cookie里面添加值，均为键/值对。Cookie可以根据关键字寻找到相应的值
                                    newCookie.Values.Add("openid", openid);
                                    newCookie.Expires = DateTime.Now.AddDays(365);
                                    //Cookie的设置页面要用Response
                                    Response.AppendCookie(newCookie);

                                    //签名字段:appId、url（当前网页url）、timestamp、noncestr、accessToken
                                    var paySignReqHandler = new RequestHandler(Context);
                                    paySignReqHandler.setParameter("appid", appId);
                                    paySignReqHandler.setParameter("timestamp", timeStamp);
                                    paySignReqHandler.setParameter("noncestr", nonceStr);
                                    paySignReqHandler.setParameter("url", Request.Url.ToString());
                                    paySignReqHandler.setParameter("accesstoken", obj.access_token);
                                    //addrSign = paySignReqHandler.CreateAddrSign();
                                    //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", appId + ";" + timeStamp + ";" + nonceStr + ";" + Request.Url.ToString() + ";" + obj.access_token + ";" + addrSign);
                                    //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "Request.Url:" + Request.Url.ToString());
                                    //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "addrSign:" + addrSign);
                                }

                            }
                        }
                    }

                    #endregion



                    address = pro.U_name + " " + pro.U_phone + "<br>" + pro.Province + " " + pro.City + " " + pro.Address;



                    if (pro.Shopcartid == 0)
                    {
                        B2b_com_pro proinfo = new B2bComProData().GetProById(pro.Pro_id.ToString(),pro.Speciid,pro.channelcoachid);
                        if (proinfo != null)
                        {
                            pro_name = proinfo.Pro_name;
                            imgurl = FileSerivce.GetImgUrl(proinfo.Imgurl);
                            Ispanicbuy = proinfo.Ispanicbuy;
                            Server_type=proinfo.Server_type;

                        }
                        else
                        {
                            pro_name = "对订单进行支付";
                        }
                        pay_price = Math.Round(pro.Pay_price, 2);
                        pro_price = Math.Round(pro.Pay_price * pro.U_num, 2);
                        price = Math.Round(pro.Pay_price * pro.U_num - pro.Integral1 - pro.Imprest1 + pro.Express, 2);
                        num = pro.U_num;
                        Express = Math.Round(pro.Express, 2);
                        
                    }
                    else
                    { //如果是购物车订单，必须一起支付
                        cart_id = pro.Shopcartid;
                        cart = 1;
                        price = Math.Round(orderdata.GetCartOrderMoneyById(pro.Id), 2);
                        pro_name = orderdata.GetCartOrderProById(pro.Id);
                        Express = Math.Round(orderdata.GetCartOrderExpressMoneyById(pro.Id), 2);
                        num = 1;
                        var shopcart = orderdata.shopcartorder(pro.Shopcartid);
                        if (shopcart != null) {
                            for (int i = 0; i < shopcart.Count; i++) {

                                pro_price += shopcart[i].Pay_price * shopcart[i].U_num;
                                proid += shopcart[i].Pro_id + ",";
                            }
                            
                        }

                        pro_price = Math.Round(pro_price, 2);
                    }



                }


                

                if (price == 0)
                {
                    price = 0;
                }
                else
                {
                    CommonFunc.OperTwoDecimal(price.ToString());
                    //price = price.IndexOf(".") != -1 ? price.Substring(0, price.IndexOf(".")) : price;                    
                }

                nowdate = DateTime.Now.ToString("yyyy-MM-dd");
                
                if (summary.Length > 30)
                {
                    summary = summary.Substring(0, 30);
                }
                if (summary.Length > 150)
                {
                    summary = summary.Substring(0, 150) + "...";
                }
                var commodel = B2bCompanyData.GetCompany(comid);
                if (commodel != null)
                {
                    if (commodel.B2bcompanyinfo != null)
                    {
                        Wxfocus_url = commodel.B2bcompanyinfo.Wxfocus_url;
                        Wxfocus_author = commodel.B2bcompanyinfo.Wxfocus_author; ;
                    }
                }


                var saleset = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
                if (saleset != null)
                {
                    phone = saleset.Service_Phone;
                }

                //查询项目电话,如果有项目电话调取项目电话
                var projectdata = new B2b_com_projectData();
                var projectmodel = projectdata.GetProject(projectid, comid);
                if (projectmodel != null)
                {
                    if (projectmodel.Mobile != "")
                    {
                        phone = projectmodel.Mobile;
                    }
                }


                //从cookie中得到微信号
                if (Request.Cookies["openid"] != null)
                {
                    openid = Request.Cookies["openid"].Value;
                }
                B2bCrmData b2b_crm = new B2bCrmData();
                if (openid != "")
                {
                    B2b_crm b2bmodle = b2b_crm.b2b_crmH5(openid, comid);
                    if (b2bmodle != null)
                    {
                        Imprest = b2bmodle.Imprest;
                        Integral = b2bmodle.Integral;
                    }
                }
            }


            if (Domain_def.Domain_yanzheng(RequestUrl))//如果符合shop101.etown.cn的格式，则从多微信商户基本信息表中获取comid
            {
                //先通过正则表达式获取COMid
                comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl).ToString());
                if (comid == 0)
                {
                    comid = new WeiXinBasicData().GetWeiXinBasicByDomain(RequestUrl).Comid;
                }
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

                        comlogo = FileSerivce.GetImgUrl(pro.Smalllogo.ConvertTo<int>(0));
                    }

                    ////获取微信平台端code
                    //string weixincode = Request["code"].ConvertTo<string>("");
                    ////获取微信号和一次性密码
                    //openid = Request["openid"].ConvertTo<string>("");
                    //string weixinpass = Request["weixinpass"].ConvertTo<string>("");

                    ////获得会员信息
                    //GetCrmInfo(weixincode, openid, weixinpass);



                }
            }


            //获取BANNER，及logo
            if (comid != 0)
            {


                //根据公司id得到 直销设置
                B2b_company_saleset saleset = B2bCompanySaleSetData.GetDirectSellByComid(comid.ToString());
                if (saleset != null)
                {
                    logoimg = FileSerivce.GetImgUrl(saleset.Smalllogo.ConvertTo<int>(0));
                }

            }



            //微信转发访问归属渠道
            if (uid != 0)//必须记录转发用户信息才能继续统计
            {

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
                    }
                }

            }



        }
        /// <summary>
        /// 获取会员信息分两种方式:a.根据客户端保存的cookie值获取 b.根据传递过来的参数获取
        /// </summary>
        /// <param name="weixincode"></param>
        /// <param name="openid"></param>
        /// <param name="weixinpass"></param>
        private void GetCrmInfo(string weixincode, string openid, string weixinpass)
        {
            if (Request.Cookies["AccountId"] != null)
            {
                string accountmd5 = "";
                int AccountId = int.Parse(Request.Cookies["AccountId"].Value);
                if (Request.Cookies["AccountKey"] != null)
                {
                    accountmd5 = Request.Cookies["AccountKey"].Value;
                }

                var data1 = CrmMemberJsonData.WeixinCookieLogin(AccountId.ToString(), accountmd5, comid, out userinfo);
                if (data1 == "OK")
                {
                    //从cookie中得到微信号
                    if (Request.Cookies["openid"] != null)
                    {
                        openid = Request.Cookies["openid"].Value;
                    }
                    if (userinfo != null)
                    {
                        Readuser(userinfo.Idcard, comid);//读取用户信息
                    }
                }
                else
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
                    //根据传递过来的参数获取会员信息
                    GetCrmByParam(weixincode, openid, weixinpass);
                }
            }
            else
            {
                //根据传递过来的参数获取会员信息
                GetCrmByParam(weixincode, openid, weixinpass);
            }
        }

        private void GetCrmByParam(string weixincode, string openid, string weixinpass)
        {
            if (weixincode != "")//商家已经进行过微信认证
            {
                GetOpenId(weixincode, comid);
            }
            else//商家没有进行过微信认证
            {

                VerifyOneOffPass(openid, weixinpass);
            }
        }
        private void VerifyOneOffPass(string openid, string weixinpass)
        {
            if (openid != "" && weixinpass != "")
            {
                B2bCrmData dateuser = new B2bCrmData();
                string data = CrmMemberJsonData.WeixinLogin(openid, weixinpass, comid, out userinfo);

                if (data == "OK")//正确的一次性密码
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
            new B2bCrmData().WeixinConPass(openid, comid);//清空微信密码
        }



        private void GetOpenId(string codee, int comid)
        {

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
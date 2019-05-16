using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using ETS.JsonFactory;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Common.Business;
using System.Web.SessionState;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Member.Service.MemberService.Model.Enum;
using Newtonsoft.Json;


namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// CrmMemberHandler 的摘要说明
    /// by:lixinghai
    /// </summary>
    public class CrmMemberHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string oper = context.Request["oper"].ConvertTo<string>("");

            if (oper != "")
            {
                if (oper == "editcrmposition")
                {
                    string usern = context.Request["usern"].ConvertTo<string>("");
                    string usere=context.Request["usere"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string openid = context.Request["openid"].ConvertTo<string>("");
                    string data = CrmMemberJsonData.Editcrmposition(usern,usere,comid,openid);
                    context.Response.Write(data);

                }
                if (oper == "GetMemberChanelCompanyByUserid")
                {
                    int yy = context.Request["yy"].ConvertTo<int>(0);
                    string data = CrmMemberJsonData.GetMemberChanelCompanyByUserid(yy);
                    context.Response.Write(data);
                }
       
                if (oper == "GetChannelByMasterId")
                {
                    int masterid = context.Request["masterid"].ConvertTo<int>(0);
                    var data = CrmMemberJsonData.GetChannelByMasterId(masterid);
                    context.Response.Write(data);
                }
                if (oper == "getb2bcrmlevels")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    var data = CrmMemberJsonData.getb2bcrmlevels(comid );
                    context.Response.Write(data);
                }
                if (oper == "editb2bcrmlevels")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string levelids = context.Request["levelids"].ConvertTo<string>("");
                    string crmlevels = context.Request["crmlevels"].ConvertTo<string>("");
                    string levelnames = context.Request["levelnames"].ConvertTo<string>("");
                    string dengjifens = context.Request["dengjifens"].ConvertTo<string>("");
                    string tequans = context.Request["tequans"].ConvertTo<string>("");
                    string isavailables = context.Request["isavailables"].ConvertTo<string>("");

                    var data = CrmMemberJsonData.editb2bcrmlevels(comid, levelids.Trim(), crmlevels.Trim(), levelnames.Trim(), dengjifens.Trim(), tequans.Trim(), isavailables.Trim());
                    context.Response.Write(data);
                }
                //修改密码
                if (oper == "updatepass")
                {
                    int com_id = context.Request["comid"].ConvertTo<int>(0);
                    int id = context.Request["id"].ConvertTo<int>(0);
                    string pass = context.Request["password1"].ConvertTo<string>("");
                    var data = CrmMemberJsonData.UpMemberpass(com_id, id, pass);
                    context.Response.Write(data);
                }
                //修改手机
                if (oper == "updatephone")
                {
                    int com_id = context.Request["comid"].ConvertTo<int>(0);
                    int id = context.Request["id"].ConvertTo<int>(0);
                    string phone = context.Request["phone"].ConvertTo<string>("");
                    var data = CrmMemberJsonData.UpMemberphone(com_id, id, phone);
                    context.Response.Write(data);
                }
                //修改邮箱
                if (oper == "updatemail")
                {
                    int com_id = context.Request["comid"].ConvertTo<int>(0);
                    int id = context.Request["id"].ConvertTo<int>(0);
                    string mail = context.Request["mail"].ConvertTo<string>("");
                    var data = CrmMemberJsonData.UpMembermail(com_id, id, mail);
                    context.Response.Write(data);
                }
                //修改卡号
                if (oper == "upcard")
                {

                    int com_id = context.Request["comid"].ConvertTo<int>(0);
                    var Cardcode = context.Request["Cardcode"].ConvertTo<decimal>(0);
                    string Phone = context.Request["Phone"].ConvertTo<string>("");
                    var data = CrmMemberJsonData.UpMembercard(com_id, Phone, Cardcode);
                    context.Response.Write(data);
                }
                if (oper == "getCard")
                {
                    string Cardcode = context.Request["Card"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    // int comid = 101;

                    var data = CrmMemberJsonData.GetCard(Cardcode, comid);
                    context.Response.Write(data);
                }
                //客户消息模板提示
                if (oper == "kehumessage")
                {
                    string weixin = context.Request["openid"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    // int comid = 101;

                    var data = CrmMemberJsonData.KehuMessage(comid,weixin);
                    context.Response.Write(data);
                }

                


                //绑定渠道，或更换渠道
                if (oper == "bangdingchannelid")
                {
                    int com_id = context.Request["comid"].ConvertTo<int>(0);
                    string openid = context.Request["openid"].ConvertTo<string>("");
                    int masterid = context.Request["masterid"].ConvertTo<int>(0);


                    var data = CrmMemberJsonData.UpCrmChannelid(com_id, masterid, openid);
                    context.Response.Write(data);
                }


                //渠道列表
                if (oper == "peopleList")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var openid = context.Request["openid"].ConvertTo<string>("");
                    var usern = context.Request["usern"].ConvertTo<string>("");
                    var usere = context.Request["usere"].ConvertTo<string>("");
                    var isheadofficekf = context.Request["isheadofficekf"].ConvertTo<int>(0);
                    var isonlycoachlist = context.Request["isonlycoachlist"].ConvertTo<int>(0);//是否仅显示教练
                    var isviewjiaolian = context.Request["isviewjiaolian"].ConvertTo<string>("0,1");
                    

                    string data = AccountInfoJsonData.ViewChanneluserpagelist(comid, channelcompanyid, pageindex, pagesize, key, openid,usern,usere,isheadofficekf,isonlycoachlist,isviewjiaolian);

                    context.Response.Write(data);
                } 
                //渠道列表
                if (oper == "channelqqList")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var openid = context.Request["openid"].ConvertTo<string>("");
                    var usern = context.Request["usern"].ConvertTo<string>("");
                    var usere = context.Request["usere"].ConvertTo<string>("");

                    string data = AccountInfoJsonData.ViewQQpagelist(comid, channelcompanyid, pageindex, pagesize, key, openid, usern, usere);

                    context.Response.Write(data);
                }

                if (oper == "getChannelCard")
                {
                    string Cardcode = context.Request["Card"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    //int comid = 101;

                    var data = CrmMemberJsonData.GetChannelCard(Cardcode, comid);
                    context.Response.Write(data);

                }


                if (oper == "getEmail")
                {
                    string Email = context.Request["Email"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    var data = CrmMemberJsonData.GetEmail(Email, comid);
                    context.Response.Write(data);

                }


                if (oper == "getPhone")
                {
                    string Phone = context.Request["Phone"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    var data = CrmMemberJsonData.GetPhone(Phone, comid);
                    context.Response.Write(data);

                }

                if (oper == "BtnPhone")//填写(修改手机)
                {
                    string Phone = context.Request["Phone"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int AccountId = context.Request["AccountId"].ConvertTo<int>();
                    var Btndata = CrmMemberJsonData.UpMemberphone(comid, AccountId, Phone);
                    context.Response.Write(Btndata);
                }

                if (oper == "BtnName")
                {
                    string Name = context.Request["Name"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int AccountId = context.Request["AccountId"].ConvertTo<int>();
                    var Btndata = CrmMemberJsonData.UpMembername(comid, AccountId, Name);
                    context.Response.Write(Btndata);
                }

                if (oper == "Btnopenid")
                {
                    string Phone = context.Request["Phone"].ConvertTo<string>("");
                    string openid = context.Request["openid"].ConvertTo<string>("");
                    string Password1 = context.Request["Password1"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    B2b_crm userinfo = new B2b_crm();
                    var wdata = CrmMemberJsonData.Weixinopenid(Phone, openid, Password1, comid, out userinfo);
                    if (wdata == "{\"type\":100,\"msg\":\"OK\"}")
                    {
                        context.Session["AccountId"] = userinfo.Id;
                        context.Session["AccountName"] = userinfo.Name;
                        context.Session["AccountCard"] = userinfo.Idcard;

                        HttpCookie cookie = new HttpCookie("AccountId", userinfo.Id.ToString());     //实例化HttpCookie类并添加值
                        cookie.Expires = DateTime.Now.AddDays(120);
                        context.Response.Cookies.Add(cookie);
                        cookie = new HttpCookie("AccountName", userinfo.Name.ToString());     //实例化HttpCookie类并添加值
                        cookie.Expires = DateTime.Now.AddDays(120);
                        context.Response.Cookies.Add(cookie);
                        var returnmd5 = EncryptionHelper.ToMD5(userinfo.Idcard.ToString() + userinfo.Id.ToString(), "UTF-8");
                        cookie = new HttpCookie("AccountKey", returnmd5);     //实例化HttpCookie类并添加值
                        cookie.Expires = DateTime.Now.AddDays(120);
                        context.Response.Cookies.Add(cookie);

                    }
                    context.Response.Write(wdata);
                }

                //微信完善个人信心，绑定微信号
                if (oper == "weiBtnopenid")
                {



                    string Phone = context.Request["Phone"].ConvertTo<string>("");
                    string openid = context.Request["openid"].ConvertTo<string>("");
                    var code = context.Request["code"].ConvertTo<decimal>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    var Password1 = "";

                    B2b_crm userinfo = new B2b_crm();

                    var list = Phone_code.code_info(decimal.Parse(Phone), comid);
                    if (list.Code == code)
                    {
                        var crmdata = new B2bCrmData();
                        var crmlist = crmdata.GetSjKeHu(Phone, comid);
                        Password1 = crmlist.Password1;
                    }


                    var wdata = CrmMemberJsonData.Weixinopenid(Phone, openid, Password1, comid, out userinfo);
                    if (wdata == "{\"type\":100,\"msg\":\"OK\"}")
                    {
                        context.Session["AccountId"] = userinfo.Id;
                        context.Session["AccountName"] = userinfo.Name;
                        context.Session["AccountCard"] = userinfo.Idcard;

                        HttpCookie cookie = new HttpCookie("AccountId", userinfo.Id.ToString());     //实例化HttpCookie类并添加值
                        cookie.Expires = DateTime.Now.AddDays(120);
                        context.Response.Cookies.Add(cookie);
                        cookie = new HttpCookie("AccountName", userinfo.Name.ToString());     //实例化HttpCookie类并添加值
                        cookie.Expires = DateTime.Now.AddDays(120);
                        context.Response.Cookies.Add(cookie);
                        var returnmd5 = EncryptionHelper.ToMD5(userinfo.Idcard.ToString() + userinfo.Id.ToString(), "UTF-8");
                        cookie = new HttpCookie("AccountKey", returnmd5);     //实例化HttpCookie类并添加值
                        cookie.Expires = DateTime.Now.AddDays(120);
                        context.Response.Cookies.Add(cookie);

                    }
                    context.Response.Write(wdata);
                }


                if (oper == "Login")
                {
                    string Account = context.Request["Account"].ConvertTo<string>("");
                    string Pwd = context.Request["Pwd"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    B2b_crm userinfo = new B2b_crm();
                    var data = CrmMemberJsonData.Login(Account, Pwd, comid, out userinfo);

                    if (data == "OK")
                    {
                        context.Session["AccountId"] = userinfo.Id;
                        context.Session["AccountName"] = userinfo.Name;
                        context.Session["AccountCard"] = userinfo.Idcard;
                    }
                    context.Response.Write(data);

                }
                if (oper == "Login2")
                {
                    string Account = context.Request["Account"].ConvertTo<string>("");
                    string Pwd = context.Request["Pwd"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string getcode = context.Request["getcode"];
                    string initcode = context.Session["SomeValidateCode"].ToString();
                    if (getcode != initcode)
                    {
                        context.Response.Write("验证码错误");
                    }
                    else
                    {
                        B2b_crm userinfo = new B2b_crm();
                        var data = CrmMemberJsonData.Login(Account, Pwd, comid, out userinfo);

                        if (data == "OK")
                        {
                            context.Session["AccountId"] = userinfo.Id;
                            context.Session["AccountName"] = userinfo.Name;
                            context.Session["AccountCard"] = userinfo.Idcard;
                        }
                        context.Response.Write(data);
                    }
                }

                if (oper == "getkey")
                {
                    string key = context.Request["key"].ConvertTo<string>("");
                    var data = CrmMemberJsonData.Getkey(key);
                    context.Response.Write(data);
                }

                if (oper == "readuser")
                {
                    int AccountId = 0;
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    if (context.Session["AccountId"] != null)
                    {
                        AccountId = Int32.Parse(context.Session["AccountId"].ToString());
                    }

                    var data = CrmMemberJsonData.Readuser(AccountId, comid);
                    context.Response.Write(data);

                }

                if (oper == "Actlist")
                {
                    int AccountId = 0;
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);//会员是否属于门市
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    string pno = context.Request["pno"].ConvertTo<string>("");
                    int userinfo = 0;

                    int cardid = 0;
                    //判断如果有传入 账户参数则不查询 session值
                    if (pno != "")
                    {
                        var datauser = CardJsonDate.SearchCard(pno, comid, out userinfo);
                        AccountId = userinfo;
                    }
                    else
                    {

                        ////判断如果有传入 账户参数则不查询 session值
                        if (context.Session["AccountId"] != null)
                        {
                            AccountId = Int32.Parse(context.Session["AccountId"].ToString());
                        }
                        else
                        {
                            if (context.Request.Cookies["AccountId"] != null)
                            {
                                AccountId = Int32.Parse(context.Request.Cookies["AccountId"].Value.ToString());
                            }
                        }

                    }
                    var crmdata = new B2bCrmData();
                    var pro = crmdata.Readuser(AccountId, comid);
                    if (pro != null)
                    {
                        cardid = pro.Cardid;
                    }

                    string data = PromotionJsonDate.AccountActPageList(cardid, comid, channelcompanyid, pageindex, pagesize);
                    context.Response.Write(data);

                }

                if (oper == "VerActlist")//验证查询此会员卡优惠活动
                {
                    int AccountId = 0;
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);//会员是否属于门市
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    string pno = context.Request["pno"].ConvertTo<string>("");
                    int userinfo = 0;

                    int cardid = 0;
                    string data = "";
                    int userid = 0;//登陆商户ID
                    int channelcomid = 0;//登陆商户账户的所属，总社还是门市

                    //先查询登陆用户ID,
                    B2b_company_manageuser user = UserHelper.CurrentUser();
                    if (user != null) {
                        userid = user.Id;
                    }
                    //再通过ID 获得渠道信息
                    Member_Channel_company channelcom = new MemberChannelcompanyData().GetChannelCompanyByUserId(userid);
                    if (channelcom != null)
                    {
                        channelcomid = channelcom.Id;
                    }


                    //判断如果有传入 没传入则 报错
                    if (pno != "")
                    {
                        var datauser = CardJsonDate.SearchCard(pno, comid, out userinfo);
                        AccountId = userinfo;
                        var crmdata = new B2bCrmData();
                        var pro = crmdata.Readuser(AccountId, comid);
                        if (pro != null)
                        {
                            cardid = pro.Cardid;
                        }
                        data = PromotionJsonDate.VerAccountActPageList(cardid, comid, channelcompanyid, pageindex, pagesize, channelcomid);
                    }
                    else
                    {
                        data = "{\"type\":1,\"msg\":\"参数传递错误\"}";
                    }
                    context.Response.Write(data);
                }


                if (oper == "UnActlist")
                {
                    int AccountId = 0;
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);//会员是否属于门市
                    int cardid = 0;

                    ////判断如果有传入 账户参数则不查询 session值
                    if (context.Session["AccountId"] != null)
                    {
                        AccountId = Int32.Parse(context.Session["AccountId"].ToString());
                    }
                    else
                    {
                        if (context.Request.Cookies["AccountId"] != null)
                        {
                            AccountId = Int32.Parse(context.Request.Cookies["AccountId"].Value.ToString());
                        }
                    }


                    var crmdata = new B2bCrmData();
                    var pro = crmdata.Readuser(AccountId, comid);
                    if (pro != null)
                    {
                        cardid = pro.Cardid;
                    }

                    string data = PromotionJsonDate.AccountUnActPageList(cardid, comid, channelcompanyid);
                    context.Response.Write(data);

                }

                #region 根据用户及活动查询验证日志
                if (oper == "Loadingactlog")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0); ;
                    var actid = context.Request["aid"].ConvertTo<int>(0);
                    int cardid = 0;
                    int AccountId = 0;

                    //判断如果有传入 账户参数则不查询 session值
                    if (context.Session["AccountId"] != null)
                    {
                        AccountId = Int32.Parse(context.Session["AccountId"].ToString());
                    }

                    var crmdata = new B2bCrmData();
                    var pro = crmdata.Readuser(AccountId, comid);
                    if (pro != null)
                    {
                        cardid = pro.Cardid;
                    }

                    string data = BusinessCustomersJsonData.Logcardact(actid, cardid, comid);

                    context.Response.Write(data);
                }
                #endregion

                if (oper == "ClaimActlist")
                {
                    int AccountId = 0;
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int aid = context.Request["aid"].ConvertTo<int>(0);
                    int cardid = 0;

                    //判断如果有传入 账户参数则不查询 session值
                    if (context.Session["AccountId"] != null)
                    {
                        AccountId = Int32.Parse(context.Session["AccountId"].ToString());
                    }

                    var crmdata = new B2bCrmData();
                    var pro = crmdata.Readuser(AccountId, comid);
                    if (pro != null)
                    {
                        cardid = pro.Cardid;
                    }

                    string data = PromotionJsonDate.AccountClaimActPageList(aid, cardid, AccountId, comid);
                    context.Response.Write(data);

                }


                if (oper == "regcard")
                {

                    string Phone = context.Request["Phone"].ConvertTo<string>("");
                    string Cardcode = context.Request["Cardcode"].ConvertTo<string>("");
                    string Email = context.Request["Email"].ConvertTo<string>("");
                    string Password1 = context.Request["Password1"].ConvertTo<string>("");
                    string Name = context.Request["Name"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string ChannelCard = context.Request["ChannelCard"].ConvertTo<string>("");
                    string Sex = context.Request["Sex"].ConvertTo<string>("");


                    //int comid = 101;


                    var data = CrmMemberJsonData.RegCard(Cardcode, Email, Password1, Name, Phone, comid, ChannelCard, Sex);

                    if (data != "0")
                    {
                        if (data == "err")
                        {
                            data = "{\"type\":1,\"msg\":\"此手机已经注册\"}";
                        }else if(data=="err1"){

                            data = "{\"type\":1,\"msg\":\"此卡已经使用\"}";
                        }
                        else if (data == "err2")
                        {

                            data = "{\"type\":1,\"msg\":\"没有此卡号\"}";
                        }

                        else
                        {
                            
                                context.Session["UserCard"] = Cardcode;

                                //返回读取用户信息
                                B2bCrmData crmdate = new B2bCrmData();
                                B2b_crm userinfo1 = crmdate.Readuser(Int32.Parse(data), comid);
                                if (userinfo1 != null)
                                {
                                    context.Session["UserCard"] = userinfo1.Idcard;

                                    //发送开卡短信
                                    SendSmsHelper.GetMember_sms(Phone, Name, userinfo1.Idcard.ToString(), Password1, 0, "开卡短信", comid);
                                }

                        }

                    }
                    context.Response.Write(data);

                }
                #region  用户注册
                if (oper == "Useregcard")
                {
                    string Phone = context.Request["Phone"].ConvertTo<string>("");
                    string Email = context.Request["Email"].ConvertTo<string>("");
                    string Password1 = context.Request["Password1"].ConvertTo<string>("");
                    string Name = context.Request["Name"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string Sex = context.Request["Sex"].ConvertTo<string>("");
                    string CardId = "";
                    var data = CrmMemberJsonData.UsereRegCard(CardId, Email, Password1, Name, Phone, comid, Sex);
                    context.Response.Write(data);
                }
                #endregion

                if (oper == "webregcard")
                {

                    string Phone = context.Request["Phone"].ConvertTo<string>("");
                    string Cardcode = context.Request["Cardcode"].ConvertTo<string>("");
                    string Email = context.Request["Email"].ConvertTo<string>("");
                    string Password1 = context.Request["Password1"].ConvertTo<string>("");
                    string Name = context.Request["Name"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string sex = context.Request["sex"].ConvertTo<string>("");
                    string ChanneCard = "";
                    //int comid = 101;


                    var data = CrmMemberJsonData.RegCard(Cardcode, Email, Password1, Name, Phone, comid, ChanneCard, sex);

                    if (data != "0")
                    {
                        if (data == "1")
                        {
                            data = "{\"type\":1,\"msg\":\"卡号还未录入，请先录入\"}";
                        }
                        else
                        {


                            //返回读取用户信息
                            B2bCrmData crmdate = new B2bCrmData();
                            B2b_crm userinfo1 = crmdate.Readuser(Int32.Parse(data), comid);
                            if (userinfo1 != null)
                            {
                                context.Session["UserCard"] = userinfo1.Idcard;

                                //发送开卡短信
                                SendSmsHelper.GetMember_sms(Phone, Name, userinfo1.Idcard.ToString(), Password1, 0, "会员开卡", comid);
                            }

                        }

                    }
                    context.Response.Write(data);

                }

                //提交预订
                if (oper == "Reservation_insert")
                {
                    int id = context.Request["id"].ConvertTo<int>();
                    int comid = context.Request["comid"].ConvertTo<int>();
                    int number = context.Request["number"].ConvertTo<int>();
                    var name = context.Request["name"].ConvertTo<string>();
                    var phone = context.Request["phone"].ConvertTo<decimal>();
                    var datetime = context.Request["datetime"].ConvertTo<DateTime>();

                    var data = CrmMemberJsonData.insert_res(id, comid, number, name, phone, datetime);
                    context.Response.Write(data);
                }
                //查询预订
                if (oper == "ResLoadingList")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);

                    int userid = context.Request["userid"].ConvertTo<int>(0);

                    string data = CrmMemberJsonData.ResLoadingList(comid, pageindex, pagesize,userid);

                    context.Response.Write(data);
                }
                if (oper == "ResSearchList")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");


                    string data = CrmMemberJsonData.ResSearchList(comid, pageindex, pagesize, key);

                    context.Response.Write(data);
                }
                //确认预订
                if (oper == "upRes")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var userid = context.Request["userid"].ConvertTo<int>(0);
                    var beiz = context.Request["beiz"].ConvertTo<string>("");

                    string data = CrmMemberJsonData.upRes(id, comid, userid, beiz);

                    context.Response.Write(data);
                }

                if (oper == "phoneLogin")
                {
                    string phone = context.Request["phone"].ConvertTo<string>("");
                    string phonepass = context.Request["phonepass"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    var data = "";

                    if (phone == "" || phonepass == "")
                    {
                        data = "手机或密码不能为空";
                        return;
                    }

                    var selectcode = Phone_code.code_info(decimal.Parse(phone), comid);
                    if (selectcode != null)
                    {

                        if (selectcode.Code == decimal.Parse(phonepass))
                        {
                            B2b_crm userinfo = new B2b_crm();
                            data = CrmMemberJsonData.phoneLogin(phone, comid, out userinfo);

                            if (data == "OK")
                            {
                                context.Session["AccountId"] = userinfo.Id;
                                context.Session["AccountName"] = userinfo.Name;
                                context.Session["AccountCard"] = userinfo.Idcard;
                            }
                        }
                        else
                        {
                            data = "动态密码错误";

                        }

                    }
                    else
                    {
                        data = "请查看手机号码是否输入正确";
                    }


                    context.Response.Write(data);
                }

                if (oper == "Webcode")
                {
                    //获取随机数
                    Random ra = new Random();
                    var code = ra.Next(1000, 9999);

                    var comid = context.Request["comid"].ConvertTo<int>(0);

                    var phone = context.Request["Phone"].ConvertTo<decimal>(0);

                    var num = 0;

                    var data = "";

                    var selectnum = Phone_code.code_info(phone, comid);

                    if (selectnum != null)
                    {
                        num = selectnum.Codenum + 1;

                        int numtime = DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 60 * 60 - selectnum.Codetime.Second - selectnum.Codetime.Minute * 60 - selectnum.Codetime.Hour * 60 * 60;
                        if (numtime < 60)//重复点击
                        {
                            data = "{\"type\":1,\"msg\":\"Notime\"}";
                            return;
                        }

                        if (selectnum.Codenum > 3)//防止大量重发
                        {
                            data = "{\"type\":100,\"msg\":\"No\"}";
                            return;
                        }


                        //修改随机数状态为1，可以发送
                        data = Phone_code.upcode(phone, code, comid, num, 1);

                        B2b_crm userinfo = new B2b_crm();
                        var card = "";

                        SendSmsHelper.GetMember_sms(phone.ToString(), "", card, selectnum.Code.ToString(), 0, "随机码", comid);

                        context.Response.Write(data);

                    }
                    else
                    {
                        num = 1;
                        data = Phone_code.insertcode(phone, code, comid, num);
                        B2b_crm userinfo = new B2b_crm();
                        var card = "";
                        var list = Phone_code.code_info(phone, comid);
                        if (list != null)
                        {
                            SendSmsHelper.GetMember_sms(phone.ToString(), "", card, code.ToString(), 0, "随机码", comid);

                        }
                        context.Response.Write(data);
                    }
                }

                if (oper == "UpCode")
                {
                    //获取随机数
                    Random ra = new Random();
                    var code = ra.Next(1000, 9999);

                    var comid = context.Request["comid"].ConvertTo<int>(0);

                    var phone = context.Request["Phone"].ConvertTo<decimal>(0);

                    var id = context.Request["id"].ConvertTo<int>(0);

                    var num = 0;

                    var data = "";

                    var selectnum = Phone_code.code_info(phone, comid);

                    if (selectnum != null)
                    {
                        num = selectnum.Codenum + 1;

                        int numtime = DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 60 * 60 - selectnum.Codetime.Second - selectnum.Codetime.Minute * 60 - selectnum.Codetime.Hour * 60 * 60;
                        if (numtime < 60)//重复点击
                        {
                            data = "{\"type\":1,\"msg\":\"Notime\"}";
                            return;
                        }

                        if (selectnum.Codenum > 3)//防止大量重发
                        {
                            data = "{\"type\":100,\"msg\":\"No\"}";
                            return;
                        }


                        //修改随机数状态为1，可以发送
                        data = Phone_code.upcode(phone, code, comid, num, 1);

                        B2b_crm userinfo = new B2b_crm();
                        var card = "";

                        var crmdata = new B2bCrmData();
                        var crmlist = crmdata.Readuser(id, comid);
                        card = crmlist.Idcard.ToString();

                        SendSmsHelper.GetMember_sms(phone.ToString(), "", card, selectnum.Code.ToString(), 0, "随机码", comid);

                        context.Response.Write(data);

                    }
                    else
                    {
                        num = 1;
                        data = Phone_code.insertcode(phone, code, comid, num);
                        B2b_crm userinfo = new B2b_crm();
                        var card = "";
                        var list = Phone_code.code_info(phone, comid);
                        if (list != null)
                        {
                            var crmdata = new B2bCrmData();
                            var crmlist = crmdata.Readuser(id, comid);
                            card = crmlist.Idcard.ToString();

                            SendSmsHelper.GetMember_sms(phone.ToString(), "", card, code.ToString(), 0, "随机码", comid);

                        }
                        context.Response.Write(data);
                    }


                }

                if (oper == "weiupmember")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var Cardcode = context.Request["Cardcode"].ConvertTo<decimal>(0);
                    var Name = context.Request["Name"].ConvertTo<string>("");
                    var Sex = context.Request["Sex"].ConvertTo<string>("");
                    var Phone = context.Request["Phone"].ConvertTo<string>("");
                    var Birthday = context.Request["inBirthday"].ConvertTo<DateTime>(DateTime.Parse("1990-01-01"));
                    var code = context.Request["code"].ConvertTo<decimal>(0);

                    var data = BusinessCustomersJsonData.weiUpMember(comid, Cardcode, Name, Phone, Sex, Birthday, code);

                    context.Response.Write(data);

                }

                if (oper == "codedelete")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);

                    var phone = context.Request["Phone"].ConvertTo<decimal>(0);

                    var id = context.Request["id"].ConvertTo<int>(0);
                    int num = 3;
                    var data = "";
                    var list = Phone_code.code_info(phone, comid);
                    if (list != null)
                    {
                        var code = list.Code;
                        //修改随机数状态为0，关闭
                        Phone_code.upcode(phone, code, comid, num, 0);
                        data = "{\"type\":100,\"msg\":\"OK\"}";
                    }
                    else
                    {
                        data = "{\"type\":100,\"msg\":\"NO\"}";
                    }
                    context.Response.Write(data);
                }

                if (oper == "getCode")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var code = context.Request["code"].ConvertTo<decimal>();
                    var phone = context.Request["Phone"].ConvertTo<decimal>();

                    var data = "";
                    var list = Phone_code.code_info(phone, comid);
                    if (list != null)
                    {
                        data = "{\"type\":100,\"msg\":\"OK\"}";
                    }
                    else
                    {
                        data = "{\"type\":100,\"msg\":\"NO\"}";
                    }

                    context.Response.Write(data);

                }

                if (oper == "weixinregcard")
                {

                    string Phone = context.Request["Phone"].ConvertTo<string>("");
                    string Cardcode = context.Request["Cardcode"].ConvertTo<string>("");
                    string openid = context.Request["openid"].ConvertTo<string>("");
                    string Name = context.Request["Name"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int VLogin = context.Request["VLogin"].ConvertTo<int>(0);
                    string Password1 = context.Request["Password1"].ConvertTo<string>("");
                    B2b_crm userinfo;
                    var data = "";

                    //int comid = 101;

                    if (VLogin == 0)
                    {

                        if (openid != "")//微信开卡
                        {

                            data = CrmMemberJsonData.WeixinRegCard(Cardcode, openid, Name, Phone, comid);
                            if (data != "0")
                            {
                                if (data == "1")
                                {
                                    data = "{\"type\":1,\"msg\":\"卡号还未录入，请先录入\"}";
                                }
                                else if (data == "err1")
                                {
                                    data = "{\"type\":1,\"msg\":\"此卡已经使用\"}";
                                }
                                else if (data == "err2")
                                {
                                    data = "{\"type\":1,\"msg\":\"没有此卡号\"}";
                                }
                                else if (data == "err3")
                                {
                                    data = "{\"type\":1,\"msg\":\"此手机已经注册使用了\"}";
                                }
                                else
                                {




                                    //返回读取用户信息并绑定session
                                    B2bCrmData crmdate = new B2bCrmData();
                                    B2b_crm userinfo1 = crmdate.Readuser(Int32.Parse(data), comid);
                                    if (userinfo1 != null)
                                    {
                                        context.Session["AccountId"] = userinfo1.Id;
                                        context.Session["AccountName"] = userinfo1.Name;
                                        context.Session["AccountCard"] = userinfo1.Idcard;

                                        //context.Response.Cookies.Add(new HttpCookie("AccountId", userinfo1.Id.ToString()));
                                        //context.Response.Cookies.Add(new HttpCookie("AccountName", userinfo1.Name.ToString()));
                                        //context.Response.Cookies.Add(new HttpCookie("AccountKey", returnmd5));

                                        HttpCookie cookie = new HttpCookie("AccountId", userinfo1.Id.ToString());     //实例化HttpCookie类并添加值
                                        cookie.Expires = DateTime.Now.AddDays(120);
                                        context.Response.Cookies.Add(cookie);
                                        cookie = new HttpCookie("AccountName", userinfo1.Name.ToString());     //实例化HttpCookie类并添加值
                                        cookie.Expires = DateTime.Now.AddDays(120);
                                        context.Response.Cookies.Add(cookie);
                                        var returnmd5 = EncryptionHelper.ToMD5(userinfo1.Idcard.ToString() + userinfo1.Id.ToString(), "UTF-8");
                                        cookie = new HttpCookie("AccountKey", returnmd5);     //实例化HttpCookie类并添加值
                                        cookie.Expires = DateTime.Now.AddDays(120);
                                        context.Response.Cookies.Add(cookie);


                                        //发送开卡短信
                                        SendSmsHelper.GetMember_sms(Phone, Name, userinfo1.Idcard.ToString(), userinfo1.Password1, 0, "会员开卡", comid);


                                    }

                                    context.Session["UserCard"] = Cardcode;
                                }
                            }
                        }
                        else
                        { //注册账户
                            data = CrmMemberJsonData.RegAccount(Phone, Name, Password1, comid);

                            if (data == "err1")
                            {
                                data = "{\"type\":1,\"msg\":\"手机号错误\"}";
                            }
                            else if (data == "err3")
                            {
                                data = "{\"type\":1,\"msg\":\"此手机已经注册使用了\"}";
                            }
                            else if (data != "0")
                            {


                                //返回读取用户信息并绑定session
                                B2bCrmData crmdate = new B2bCrmData();
                                B2b_crm userinfo1 = crmdate.Readuser(Int32.Parse(data), comid);
                                if (userinfo1 != null)
                                {
                                    context.Session["AccountId"] = userinfo1.Id;
                                    context.Session["AccountName"] = userinfo1.Name;
                                    context.Session["AccountCard"] = userinfo1.Idcard;
                                    //context.Response.Cookies.Add(new HttpCookie("AccountId", userinfo1.Id.ToString()));
                                    //context.Response.Cookies.Add(new HttpCookie("AccountName", userinfo1.Name.ToString()));
                                    //var returnmd5 = EncryptionHelper.ToMD5(userinfo1.Idcard.ToString() + userinfo1.Id.ToString(), "UTF-8");
                                    //context.Response.Cookies.Add(new HttpCookie("AccountKey", returnmd5));
                                    HttpCookie cookie = new HttpCookie("AccountId", userinfo1.Id.ToString());     //实例化HttpCookie类并添加值
                                    cookie.Expires = DateTime.Now.AddDays(120);
                                    context.Response.Cookies.Add(cookie);
                                    cookie = new HttpCookie("AccountName", userinfo1.Name.ToString());     //实例化HttpCookie类并添加值
                                    cookie.Expires = DateTime.Now.AddDays(120);
                                    context.Response.Cookies.Add(cookie);
                                    var returnmd5 = EncryptionHelper.ToMD5(userinfo1.Idcard.ToString() + userinfo1.Id.ToString(), "UTF-8");
                                    cookie = new HttpCookie("AccountKey", returnmd5);     //实例化HttpCookie类并添加值
                                    cookie.Expires = DateTime.Now.AddDays(120);
                                    context.Response.Cookies.Add(cookie);

                                    //发送开卡短信
                                    SendSmsHelper.GetMember_sms(Phone, Name, userinfo1.Idcard.ToString(), userinfo1.Password1, 0, "会员开卡", comid);


                                }
                            }


                        }
                        context.Response.Write(data);
                    }
                    else
                    {//微信或手机端登陆
                        if (Password1 == "" || Phone == "")
                        {
                            context.Response.Write("{\"type\":1,\"msg\":\"请输入手机及登陆密码\"}");
                            return;
                        }

                        data = CrmMemberJsonData.WeixinPassLogin(Phone, openid, Password1, comid, out userinfo);
                        if (data == "OK")
                        {
                            context.Session["AccountId"] = userinfo.Id;
                            context.Session["AccountName"] = userinfo.Name;
                            context.Session["AccountCard"] = userinfo.Idcard;
                            //context.Response.Cookies.Add(new HttpCookie("AccountId", userinfo.Id.ToString()));
                            //context.Response.Cookies.Add(new HttpCookie("AccountName", userinfo.Name.ToString()));
                            //var returnmd5 = EncryptionHelper.ToMD5(userinfo.Idcard.ToString() + userinfo.Id.ToString(), "UTF-8");
                            //context.Response.Cookies.Add(new HttpCookie("AccountKey", returnmd5));

                            HttpCookie cookie = new HttpCookie("AccountId", userinfo.Id.ToString());     //实例化HttpCookie类并添加值
                            cookie.Expires = DateTime.Now.AddDays(120);
                            context.Response.Cookies.Add(cookie);
                            cookie = new HttpCookie("AccountName", userinfo.Name.ToString());     //实例化HttpCookie类并添加值
                            cookie.Expires = DateTime.Now.AddDays(120);
                            context.Response.Cookies.Add(cookie);
                            var returnmd5 = EncryptionHelper.ToMD5(userinfo.Idcard.ToString() + userinfo.Id.ToString(), "UTF-8");
                            cookie = new HttpCookie("AccountKey", returnmd5);     //实例化HttpCookie类并添加值
                            cookie.Expires = DateTime.Now.AddDays(120);
                            context.Response.Cookies.Add(cookie);

                        }


                        context.Response.Write(data);

                    }
                }
                if (oper == "GetCrmStatistics")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var userid = context.Request["userid"].ConvertTo<int>(0);
                    var data = BusinessCustomersJsonData.GetCrmStatistics(comid, userid);

                    context.Response.Write(data);
                }
                if (oper == "GetCompanyInfo")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);

                    var data = BusinessCustomersJsonData.GetCompanyInfo(comid);

                    context.Response.Write(data);
                }
                if (oper == "GetMenshisByComid")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    var data = CrmMemberJsonData.GetMenshisByComid(comid);
                    context.Response.Write(data);
                }
                if (oper == "GetCompanyUser")
                {
                    int userid = context.Request["userid"].ConvertTo<int>(0);

                    var data = CrmMemberJsonData.GetCompanyUser(userid);
                    context.Response.Write(data);
                }
                if (oper == "GetWxMemberCountry")
                {
                    var data = CrmMemberJsonData.GetWxMemberCountry();
                    context.Response.Write(data);
                }
                if (oper == "GetWxMemberProvince")
                {
                    var country = context.Request["country"].ConvertTo<string>("");
                    var data = CrmMemberJsonData.GetWxMemberProvince(country);
                    context.Response.Write(data);
                }
                if (oper == "GetWxMemberCity")
                {
                    var province = context.Request["province"].ConvertTo<string>("");
                    var data = CrmMemberJsonData.GetWxMemberCity(province);
                    context.Response.Write(data);
                }
                if (oper == "GetIndustryById")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);

                    string data = CrmMemberJsonData.GetIndustryById(id);
                    context.Response.Write(data);

                }
                if (oper == "EditComIndustry")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    string name = context.Request["name"].ConvertTo<string>("");
                    string remark = context.Request["remark"].ConvertTo<string>("");


                    string data = CrmMemberJsonData.EditComIndustry(id, name, remark);
                    context.Response.Write(data);

                }
                if (oper == "delindustry")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    string data = CrmMemberJsonData.Delindustry(id);
                    context.Response.Write(data);

                }
                if (oper == "deltagtype")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    string data = CrmMemberJsonData.Deltagtype(id);
                    context.Response.Write(data);
                }
                if (oper == "GetTagTypeById")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    string data = CrmMemberJsonData.GetTagTypeById(id);
                    context.Response.Write(data);
                }
                if (oper == "EditComTagType")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    string name = context.Request["name"].ConvertTo<string>("");
                    string remark = context.Request["remark"].ConvertTo<string>("");
                    int industryid = context.Request["industryid"].ConvertTo<int>(0);

                    string data = CrmMemberJsonData.EditComTagType(id, name, remark, industryid);
                    context.Response.Write(data);
                }
                if (oper == "GetIndustryList")
                {
                    int industryid = context.Request["industryid"].ConvertTo<int>(0);

                    string data = CrmMemberJsonData.GetIndustryList(industryid);
                    context.Response.Write(data);
                }

                if (oper == "GetCompanyDetail")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = CrmMemberJsonData.GetCompanyDetail(comid);
                    context.Response.Write(data);
                }
                if (oper == "GetTagTypeListByIndustryid")
                {
                    int industryid = context.Request["industryid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = CrmMemberJsonData.GetTagListByIndustryid(industryid, comid);
                    context.Response.Write(data);
                }
                if (oper == "GetTagListByTypeid")
                {
                    int typeid = context.Request["typeid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string issystemadd = context.Request["issystemadd"].ConvertTo<string>("0,1");

                    string data = CrmMemberJsonData.GetTagListByTypeid(typeid, comid, issystemadd);
                    context.Response.Write(data);
                }
                if (oper == "GetTagById")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);

                    string data = CrmMemberJsonData.GetTagById(id);
                    context.Response.Write(data);
                }
                if (oper == "EditTag")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    int tagtypeid = context.Request["tagtypeid"].ConvertTo<int>(0);
                    string name = context.Request["name"].ConvertTo<string>("");

                    int issystemadd = context.Request["issystemadd"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = CrmMemberJsonData.EditTag(id, name, tagtypeid, issystemadd, comid);
                    context.Response.Write(data);
                }
                if (oper == "GetCrmInterest")
                {
                    int crmid = context.Request["crmid"].ConvertTo<int>(0);

                    string data = CrmMemberJsonData.GetCrmInterest(crmid);
                    context.Response.Write(data);
                }
                if (oper == "EditCrmInterest")
                {
                    int crmid = context.Request["crmid"].ConvertTo<int>(0);
                    string checkedstr = context.Request["checkedstr"].ConvertTo<string>("");

                    string data = CrmMemberJsonData.EditCrmInterest(crmid, checkedstr);
                    context.Response.Write(data);
                }
                if (oper == "ChangeHangye")
                {

                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int hangye = context.Request["hangye"].ConvertTo<int>(0);

                    string data = CrmMemberJsonData.ChangeComType(comid, hangye);
                    context.Response.Write(data);
                }
                if (oper == "Deltag")
                {

                    int tagid = context.Request["tagid"].ConvertTo<int>(0);


                    string data = CrmMemberJsonData.Deltag(tagid);
                    context.Response.Write(data);
                }
                if (oper == "GetCrmGroupList")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(10);


                    string data = CrmMemberJsonData.GetCrmGroupList(comid, pageindex, pagesize);
                    context.Response.Write(data);
                }
                if (oper == "GetB2bGroupById")
                {
                    int groupid = context.Request["groupid"].ConvertTo<int>(0);
                    string data = CrmMemberJsonData.GetB2bGroupById(groupid);
                    context.Response.Write(data);
                }
                if (oper == "EditB2bGroup")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int userid = context.Request["userid"].ConvertTo<int>(0);
                    string name = context.Request["name"].ConvertTo<string>("");
                    string remark = "";
                    DateTime createtime = DateTime.Now;

                    string data = CrmMemberJsonData.EditB2bGroup(id, name, comid, userid, remark, createtime);
                    context.Response.Write(data);
                }
                if (oper == "Delb2bgroup")
                {
                    int groupid = context.Request["groupid"].ConvertTo<int>(0);
                    string data = CrmMemberJsonData.Delb2bgroup(groupid);
                    context.Response.Write(data);
                }
                if (oper == "GetCompanyB2bgroup")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = CrmMemberJsonData.GetCompanyB2bgroup(comid);
                    context.Response.Write(data);
                }
                if (oper == "Changefenzu")
                {
                    int crmid = context.Request["crmid"].ConvertTo<int>(0);
                    int groupid = context.Request["groupid"].ConvertTo<int>(0);
                    string data = CrmMemberJsonData.Changefenzu(crmid, groupid);
                    context.Response.Write(data);
                }
                if (oper == "Getqunfaphone")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string qunfanum = context.Request["qunfanum"].ConvertTo<string>("-1");
                    string data = CrmMemberJsonData.Getqunfaphone(comid,qunfanum);
                    context.Response.Write(data);
                }

                if (oper == "SendSmsLogin")
                {
                    string mobile = context.Request["mobile"].ConvertTo<string>("");
                    string getcode = context.Request["imgcode"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string initcode = context.Session["SomeValidateCode"].ToString();
                    if (getcode != initcode)
                    {
                        context.Response.Write("验证码错误");
                    }
                    else
                    {
                        string data = CrmMemberJsonData.SendSmsLogin(comid, mobile);
                        context.Response.Write(data);
                    }
                }
                if (oper == "SmsphoneLogin")
                {

                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    var mobile = context.Request["mobile"].ConvertTo<string>("");
                    var smscode = context.Request["smscode"].ConvertTo<string>("");
                    var source = "用户短信登录"; 

                    int result = 1;//返回处理结果：1出错；100成功
                    var data = AgentJosnData.Judgevalidsms(mobile, smscode, source, out result);
                    //手机动态密码登录
                    if (result == 100 )
                    {
                        //分销手机动态密码登录用到，其他请求不用传入值
                        B2b_crm userinfo = new B2b_crm();
                        data = CrmMemberJsonData.phoneLogin(mobile, comid, out userinfo);

                        if (data == "OK")
                        {
                            context.Session["AccountId"] = userinfo.Id;
                            context.Session["AccountName"] = userinfo.Name;
                            context.Session["AccountCard"] = userinfo.Idcard;

                            HttpCookie cookie = new HttpCookie("AccountId");     //实例化HttpCookie类并添加值
                            cookie.Value = userinfo.Id.ToString();
                            cookie.Expires = DateTime.Now.AddDays(120);
                            context.Response.Cookies.Add(cookie);
                            var returnmd5 = EncryptionHelper.ToMD5(userinfo.Idcard.ToString() + userinfo.Id.ToString(), "UTF-8");
                            cookie = new HttpCookie("AccountKey");     //实例化HttpCookie类并添加值
                            cookie.Value = returnmd5;
                            cookie.Expires = DateTime.Now.AddDays(120);
                            context.Response.Cookies.Add(cookie);


                            data = "{\"type\":100,\"msg\":\"登录成功!\"}";
                        }
                        else
                        {

                            //如果没有查询到此账户，直接为用户创建 一个新账户
                            B2bCrmData crmdata = new B2bCrmData();
                            //没有的话创建新账户
                            B2b_crm crm = new B2b_crm()
                            {
                                Id = 0,
                                Com_id = comid,
                                Name = "",
                                Sex = "0",
                                Phone = mobile,
                                Email = "",
                                Weixin = "",
                                Laiyuan = "",
                                Regidate = DateTime.Now,
                                Age = 0
                            };

                            string cardcode = MemberCardData.CreateECard(1, comid);//创建卡号并插入活动,1:网站；2:微信

                            crm.Idcard = cardcode.ConvertTo<decimal>(0);
                            int u_id = crmdata.InsertOrUpdate(crm);

                            context.Session["AccountId"] = u_id;
                            context.Session["AccountName"] = "";
                            context.Session["AccountCard"] = cardcode;

                            HttpCookie cookie = new HttpCookie("AccountId");     //实例化HttpCookie类并添加值
                            cookie.Value = u_id.ToString();
                            cookie.Expires = DateTime.Now.AddDays(120);
                            context.Response.Cookies.Add(cookie);
                            var returnmd5 = EncryptionHelper.ToMD5(cardcode + u_id.ToString(), "UTF-8");
                            cookie = new HttpCookie("AccountKey");     //实例化HttpCookie类并添加值
                            cookie.Value = returnmd5;
                            cookie.Expires = DateTime.Now.AddDays(120);
                            context.Response.Cookies.Add(cookie);



                            data = "{\"type\":100,\"msg\":\"登录成功!\"}";

                            //data = "{\"type\":1,\"msg\":\"该账户状态异常!\"}";
                        }

                    }
                    else {
                        data = "{\"type\":1,\"msg\":\"短信验证码错误，请重新发送!\"}";
                    }
                    context.Response.Write(data);
                }




            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
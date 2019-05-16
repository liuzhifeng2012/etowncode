using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using ETS.JsonFactory;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// BusinessCustomersHandler 的摘要说明
    /// </summary>
    public class BusinessCustomersHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string oper = context.Request["oper"].ConvertTo<string>("");
            if (oper != "")
            {
                if (oper == "readdengjifen")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);

                    string data = BusinessCustomersJsonData.Readdengjifen(id, comid);

                    context.Response.Write(data);
                }
                if (oper == "sjkehupagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);

                    var userid = context.Request["userid"].ConvertTo<int>(0);



                    string data = BusinessCustomersJsonData.SJKeHuPageList(comid, pageindex, pagesize, userid);


                    context.Response.Write(data);
                    //context.Response.Write("Hello World");
                }

                if (oper == "Fuwupagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var user = context.Request["user"].ConvertTo<int>();

                    string data = BusinessCustomersJsonData.fuwuPageList(comid, pageindex, pagesize, user);


                    context.Response.Write(data);
                }

                #region 活动加载明细列表
                if (oper == "LoadingList")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);

                    var userid = context.Request["userid"].ConvertTo<int>(0);

                    string data = BusinessCustomersJsonData.LoadingList(comid, pageindex, pagesize, userid);

                    context.Response.Write(data);
                }
                #endregion

                if (oper == "searchpagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");

                    var userid = context.Request["userid"].ConvertTo<int>(0);

                    string data = BusinessCustomersJsonData.SearchPageList(userid, comid, pageindex, pagesize, key);

                    context.Response.Write(data);
                    //context.Response.Write("Hello World");
                }
                if (oper == "searchpagelist2")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");

                    var userid = context.Request["userid"].ConvertTo<int>(0);
                    string isactivate = context.Request["isactivate"].ConvertTo<string>("1");//激活状态：默认激活1

                    string iswxfocus = context.Request["iswxfocus"].ConvertTo<string>("0,1");//微信是否关注:默认全部
                    string ishasweixin = context.Request["ishasweixin"].ConvertTo<string>("0,1");//会员是否含有微信:默认全部


                    string ishasphone = context.Request["ishasphone"].ConvertTo<string>("0,1");//会员是否含有手机:默认全部
                    string data = BusinessCustomersJsonData.SearchPageList(userid, comid, pageindex, pagesize, key, isactivate,iswxfocus,ishasweixin,ishasphone);


                    context.Response.Write(data);
                    //context.Response.Write("Hello World");
                }
                #region  活动使用日志
                if (oper == "SearchActivityList")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var ServerName = context.Request["ServerName"].ConvertTo<string>("");

                    var userid = context.Request["userid"].ConvertTo<int>(0);


                    string data = BusinessCustomersJsonData.SearchActivityList(comid, pageindex, pagesize, key, ServerName, userid);

                    context.Response.Write(data);
                    //context.Response.Write("Hello World");
                }
                #endregion

                if (oper == "readintegral")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);

                    string data = BusinessCustomersJsonData.ReadIntegral(id, comid);

                    context.Response.Write(data);
                    //context.Response.Write("Hello World");
                }

                if (oper == "readimprest")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);

                    string data = BusinessCustomersJsonData.ReadImprest(id, comid);

                    context.Response.Write(data);
                    //context.Response.Write("Hello World");
                }




                if (oper == "readuser")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);


                    string data = BusinessCustomersJsonData.Readuser(id, comid);

                    context.Response.Write(data);

                }

                //初始会员密码
                if (oper == "initialuser")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);


                    string data = BusinessCustomersJsonData.initialuser(id, comid);

                    context.Response.Write(data);

                }

                #region  crm活动日志详细页面
                if (oper == "logdetails")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);


                    string data = BusinessCustomersJsonData.Logdetails(id, comid);

                    context.Response.Write(data);

                }
                #endregion

                if (oper == "upmember")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var Name = context.Request["Name"].ConvertTo<string>("");
                    var Sex = context.Request["Sex"].ConvertTo<string>("");
                    var Phone = context.Request["Phone"].ConvertTo<string>("");
                    var Email = context.Request["Email"].ConvertTo<string>("");
                    var Age = context.Request["Age"].ConvertTo<int>(0);

                    string data = BusinessCustomersJsonData.UpMember(id, comid, Name, Phone, Sex, Email, Age);

                    context.Response.Write(data);

                }



                if (oper == "writemoney")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var acttype = context.Request["acttype"].ConvertTo<string>("");
                    var money = context.Request["money"].ConvertTo<string>("");
                    var ordername = context.Request["ordername"].ConvertTo<string>("");
                    var remark = context.Request["ordername"].ConvertTo<string>("");

                    string data = BusinessCustomersJsonData.WriteMoney(id, comid, acttype, money, 0, ordername,remark);

                    context.Response.Write(data);

                }
                if (oper == "smssend")//发送短信
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var Smsphone = context.Request["Smsphone"].ConvertTo<string>("");
                    var Smstext = context.Request["Smstext"].ConvertTo<string>("");

                    string data = BusinessCustomersJsonData.SmsSend(comid, Smsphone, Smstext);

                    context.Response.Write(data);

                }

                if (oper == "contentsend")//合作商提交发送短信
                {
                    var comid = 0;
                    var mobile = context.Request["mobile"].ConvertTo<string>("");
                    var name = context.Request["name"].ConvertTo<string>("");
                    var company = context.Request["company"].ConvertTo<string>("");
                    var address = context.Request["address"].ConvertTo<string>("");

                    string data = BusinessCustomersJsonData.Contentsend(comid, mobile, name, company, address);

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
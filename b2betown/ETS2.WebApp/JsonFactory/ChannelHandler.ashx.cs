using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using ETS2.Member.Service.MemberService.Data;
using ETS.JsonFactory;
using ETS2.Member.Service.MemberService.Model;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// ChannelHandler 的摘要说明
    /// </summary>
    public class ChannelHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string oper = context.Request["oper"].ConvertTo<string>("");
            if (oper != "")
            {

                if (oper == "getunitlist")
                {
                    int unittype = context.Request["unittype"].ConvertTo<int>(0);//默认显示内部渠道
                    string data = ChannelJsonData.GetUnitList(unittype);
                    context.Response.Write(data);
                }
                if (oper == "getunitlistneww")
                {
                    int unittype = context.Request["unittype"].ConvertTo<int>(0);//默认显示内部渠道,加公司限制
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int actid = context.Request["actid"].ConvertTo<int>(0);
                    int userid = context.Request["userid"].ConvertTo<int>(0);

                    string data = ChannelJsonData.GetUnitList(comid, unittype, userid, actid);
                    context.Response.Write(data);
                }
                if (oper == "getunitlist2")//只是用于员工管理页面
                {
                    string ischosen = context.Request["ischosen"].ConvertTo<string>("0");//渠道来源中“所属门市”是否选中
                    int userid = context.Request["userid"].ConvertTo<int>(0);
                    string unittype = context.Request["unittype"].ConvertTo<string>("0");//默认显示内部渠道,加公司限制
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = ChannelJsonData.GetUnitList(comid, unittype, ischosen, userid);
                    context.Response.Write(data);
                }
                if (oper == "GetChannelCompanyList")
                {
                    string channeltype = context.Request["channeltype"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>();
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(10);

                    string data = ChannelJsonData.GetChannelCompanyList(comid, channeltype, pageindex, pagesize);
                    context.Response.Write(data);
                }
                if (oper == "GetChannelCompany")
                {
                    string channelcompanyid = context.Request["channelcompanyid"].ConvertTo<string>("");
                    string data = ChannelJsonData.GetChannelCompany(channelcompanyid);
                    context.Response.Write(data);
                }
                if (oper == "getchanneldetail")
                {
                    int channelid = context.Request["channelid"].ConvertTo<int>(0);

                    string date = ChannelJsonData.GetChannelDetail(channelid);
                    context.Response.Write(date);
                }
                if (oper == "getchanneldetailnew")
                {
                    int channelid = context.Request["channelid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string date = ChannelJsonData.GetChannelDetail(channelid, comid);
                    context.Response.Write(date);
                }
                if (oper == "getchannelcompanyname")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);

                    string date = ChannelJsonData.Getchannelcompanyname(channelcompanyid, comid);
                    context.Response.Write(date);
                }
                if (oper == "editchannel")
                {
                    var id = context.Request["id"];
                    var comid = context.Request["comid"];
                    var Issuetype = context.Request["Issuetype"];
                    var companyid = context.Request["companyid"];
                    var name = context.Request["name"];
                    var mobile = context.Request["mobile"];
                    var cardcode = context.Request["cardcode"].ConvertTo<decimal>(0);
                    var chadress = context.Request["chadress"];
                    var chobjects = context.Request["chobjects"];
                    var rebateopen = context.Request["rebateopen"];
                    var rebateconsume = context.Request["rebateconsume"];
                    var rebateleval = context.Request["rebateleval"];

                    var opencardnum = context.Request["opencardnum"];
                    var firstdealnum = context.Request["firstdealnum"];
                    var summoney = context.Request["summoney"];

                    int whetherdefaultchannel = context.Request["whetherdefaultchannel"].ConvertTo<int>(0);
                    int runstate = context.Request["runstate"].ConvertTo<int>(1);

                    Member_Channel channel = new Member_Channel()
                    {
                        Id = id.ConvertTo<int>(0),
                        Com_id = comid.ConvertTo<int>(0),
                        Issuetype = Issuetype.ConvertTo<int>(1),
                        Companyid = companyid.ConvertTo<int>(0),
                        Name = name,
                        Mobile = mobile,
                        Cardcode = cardcode,
                        Chaddress = chadress,
                        ChObjects = chobjects,
                        RebateOpen = rebateopen.ConvertTo<decimal>(0),
                        RebateConsume = rebateconsume.ConvertTo<decimal>(0),
                        RebateConsume2 = 0,
                        RebateLevel = rebateleval.ConvertTo<int>(0),
                        Opencardnum = opencardnum.ConvertTo<int>(0),
                        Firstdealnum = firstdealnum.ConvertTo<int>(0),
                        Summoney = summoney.ConvertTo<decimal>(0),
                        Whetherdefaultchannel = whetherdefaultchannel,
                        Runstate = runstate

                    };

                    string data = ChannelJsonData.EditChannel(channel);
                    context.Response.Write(data);
                }
                if (oper == "pagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var issuetype = context.Request["issuetype"].ConvertTo<string>("all");

                    var userid = context.Request["userid"].ConvertTo<int>(0);

                    string data = ChannelJsonData.PageList(comid, pageindex, pagesize, issuetype, userid);

                    context.Response.Write(data);
                }
                if (oper == "channellistbycomid")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);


                    string data = ChannelJsonData.Channellistbycomid(comid, pageindex, pagesize);

                    context.Response.Write(data);
                }

                //渠道查询
                if (oper == "searchpagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var select = context.Request["select"].ConvertTo<int>(0);

                    var userid = context.Request["userid"].ConvertTo<int>(0);
                    string data = ChannelJsonData.SeachList(comid, pageindex, pagesize, key, select, userid);

                    context.Response.Write(data);
                }
                if (oper == "editchannelcompany")
                {
                    var channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var Issuetype = context.Request["Issuetype"].ConvertTo<int>(1);
                    var companyname = context.Request["companyname"].ConvertTo<string>("");

                    string companyaddress = context.Request["companyaddress"].ConvertTo<string>("");
                    string companyphone = context.Request["companyphone"].ConvertTo<string>("");
                    decimal companyCoordinate = context.Request["companycoordinate"].ConvertTo<decimal>(0);
                    string companyLocate = context.Request["companyLocate"].ConvertTo<string>("");

                    int companyimg = context.Request["companyimg"].ConvertTo<int>(0);
                    string companyintro = context.Request["companyintro"].ConvertTo<string>("");
                    string companyproject = context.Request["companyproject"].ConvertTo<string>("");

                    int companystate = context.Request["companystate"].ConvertTo<int>(1);
                    int whetherdepartment = context.Request["whetherdepartment"].ConvertTo<int>(0);

                    string bookurl=context.Request["bookurl"].ConvertTo<string>("");
                    string city = context.Request["city"].ConvertTo<string>("");
                    string province = context.Request["Province"].ConvertTo<string>("");

                    int outshop = context.Request["outshop"].ConvertTo<int>(0);

                    Member_Channel_company company = new Member_Channel_company()
                    {
                        Id = channelcompanyid,
                        Companyname = companyname,
                        Com_id = comid,
                        Issuetype = Issuetype,
                        Companyaddress = companyaddress,
                        Companyphone = companyphone,
                        CompanyCoordinate = companyCoordinate,
                        CompanyLocate = companyLocate,
                        Companyimg = companyimg,
                        Companyintro = companyintro,
                        Companyproject = companyproject,
                        Companystate = companystate,
                        Whetherdepartment = whetherdepartment,
                        Bookurl=bookurl,
                        City = city,
                        Province = province,
                        Outshop = outshop
                    };
                    string data = ChannelJsonData.EditChannelCompany(company);
                    context.Response.Write(data);
                }
                if (oper == "GetChannelByCompanyid")
                {
                    var companyid = context.Request["companyid"];
                    string data = ChannelJsonData.GetChannelByCompanyid(companyid);
                    context.Response.Write(data);
                }

                //修改渠道和推荐人
                if (oper == "upchannl")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var channlcard = context.Request["channlcard"].ConvertTo<decimal>();
                    var oldcard = context.Request["oldcard"].ConvertTo<decimal>();
                    var uptype = context.Request["Uptype"].ConvertTo<int>();

                    string data = ChannelJsonData.Upchannl(id, comid, channlcard, oldcard, uptype);

                    context.Response.Write(data);
                }

                //修改门市店长推荐
                if (oper == "upchannelcompanproject")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["channelcompanyid"].ConvertTo<int>(0);
                    var companyproject = context.Request["companyproject"].ConvertTo<string>("");

                    string data = ChannelJsonData.Upchannelcompanproject(id, companyproject);

                    context.Response.Write(data);
                }

                //渠道统计
                if (oper == "Channelstatistics")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    //var issuetype = context.Request["issuetype"].ConvertTo<string>("all") == "all" ? "all" : context.Request["issuetype"];
                    string channelcompanytype = context.Request["channelcompanytype"].ConvertTo<string>("out");

                    string data = ChannelJsonData.Channelstatistics(comid, pageindex, pagesize, channelcompanytype);

                    context.Response.Write(data);
                }
                //渠道统计
                if (oper == "Channelstatistics2")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);

                    string channelcompanytype = context.Request["channelcompanytype"].ConvertTo<string>("out");
                    string companystate = context.Request["companystate"].ConvertTo<string>("1");

                    string data = ChannelJsonData.Channelstatistics2(comid, pageindex, pagesize, channelcompanytype, companystate);

                    context.Response.Write(data);
                }

                if (oper == "Channelstatistics3")
                {
                    int channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);

                    string channelcompanytype = context.Request["channelcompanytype"].ConvertTo<string>("out");
                    string companystate = context.Request["companystate"].ConvertTo<string>("1");

                    string data = ChannelJsonData.Channelstatistics2(comid, pageindex, pagesize, channelcompanytype, companystate, channelcompanyid);

                    context.Response.Write(data);
                }
                //渠道统计_验卡
                if (oper == "ChannelYk")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var issuetype = context.Request["issuetype"].ConvertTo<string>("all") == "all" ? "all" : context.Request["issuetype"];
                    string data = ChannelJsonData.ChannelYk(comid, pageindex, pagesize, issuetype);

                    context.Response.Write(data);
                }
                if (oper == "GetAllInnerChannels")//得到所有的内部渠道
                {

                    string data = ChannelJsonData.GetAllInnerChannels();
                    context.Response.Write(data);
                }

                if (oper == "GetAllInnerChannelCompanys")//得到所有的内部渠道门市
                {

                    string data = ChannelJsonData.GetUnitList(0);
                    context.Response.Write(data);
                }
                if (oper == "GetUnitListNew")//得到内部/外部渠道门市
                {
                    int sourceid = context.Request["sourceid"].ConvertTo<int>(0);

                    string data = ChannelJsonData.GetUnitList(sourceid);
                    context.Response.Write(data);
                }

                if (oper == "GetUnitListselected")//得到渠道及被选中效果
                {
                    int actid = context.Request["actid"].ConvertTo<int>(0);

                    string data = ChannelJsonData.GetUnitListselected(actid);
                    context.Response.Write(data);
                }
                if (oper == "SearchChannelByChannelUnit")//得到渠道公司下的渠道列表
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);


                    string data = ChannelJsonData.SearchChannelByChannelUnit(comid, pageindex, pagesize, channelcompanyid);

                    context.Response.Write(data);
                }
                if (oper == "Channelcompanypagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");

                    int channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);

                    string channelcompanytype = context.Request["channelcompanytype"].ConvertTo<string>("0,1,3,4");
                    var accountid = 0;
                    if (context.Request.Cookies["AccountId"] != null)
                    {
                        accountid = context.Request.Cookies["AccountId"].Value.ConvertTo<int>(0);
                    }
                    var openid = "";
                    var n1 = "";//用户的精度
                    var e1 = "";//用户的维度
                    var b2bdate = new B2bCrmData();

                    if (accountid != 0) {
                        var b2bmodel = b2bdate.Readuser(accountid, int.Parse(comid));
                        if (b2bmodel != null) {
                            openid = b2bmodel.Weixin;
                        }
                        var crmlocation = b2bdate.GetB2bCrmDistanceByid(openid);
                        if (crmlocation != "") {
                            var locatarr = crmlocation.Split(',');
                            if (locatarr.Count() >= 2) {
                                n1 = locatarr[1];
                                e1 = locatarr[0];
                            }
                        }



                    }



                    string data = ChannelJsonData.Channelcompanypagelist(comid, pageindex, pagesize, key, channelcompanyid, channelcompanytype, openid);

                    context.Response.Write(data);
                }

                if (oper == "ChannelcompanyOrderlocation")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");

                    int channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);

                    string channelcompanytype = context.Request["channelcompanytype"].ConvertTo<string>("0,1,3,4");
                    var accountid = 0;
                    if (context.Request.Cookies["AccountId"] != null)
                    {
                        accountid = context.Request.Cookies["AccountId"].Value.ConvertTo<int>(0);
                    }
                    var openid = "";
                    var n1 = "";//用户的精度
                    var e1 = "";//用户的维度
                    var b2bdate = new B2bCrmData();

                    if (accountid != 0)
                    {
                        var b2bmodel = b2bdate.Readuser(accountid, int.Parse(comid));
                        if (b2bmodel != null)
                        {
                            openid = b2bmodel.Weixin;
                        }
                        var crmlocation = b2bdate.GetB2bCrmDistanceByid(openid);
                        if (crmlocation != "")
                        {
                            var locatarr = crmlocation.Split(',');
                            if (locatarr.Count() >= 2)
                            {
                                n1 = locatarr[1];
                                e1 = locatarr[0];
                            }
                        }

                    }

                    string data = ChannelJsonData.ChannelcompanyOrderlocation(comid, pageindex, pagesize, key, channelcompanyid, channelcompanytype, openid, n1, e1);

                    context.Response.Write(data);
                }


                if (oper == "adjustchannelcompanystatus")
                {
                    int companyid = context.Request["companyid"].ConvertTo<int>(0);
                    int status = context.Request["status"].ConvertTo<int>(0);
                    string data = ChannelJsonData.Adjustchannelcompanystatus(companyid, status);

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
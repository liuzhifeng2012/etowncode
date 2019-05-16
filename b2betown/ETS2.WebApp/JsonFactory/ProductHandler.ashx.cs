using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using ETS.JsonFactory;
using ETS2.PM.Service.PMService.Modle;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Common.Business;
using System.Web.SessionState;

namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// ProductHandler 的摘要说明
    /// </summary>
    public class ProductHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string oper = context.Request["oper"].ConvertTo<string>("");

            if (oper != "")
            {
                if (oper == "downstockpro")
                {
                    int stockagentcompanyid = context.Request["agentcompanyid"].ConvertTo<int>(0);
                    int proid = context.Request["proid"].ConvertTo<int>(0);

                    List<int> prolist = new List<int>();
                    prolist.Add(proid);

                    string data = ProductJsonData.DownStockPro(prolist,stockagentcompanyid);
                    context.Response.Write(data);
                }
                //上架产品
                if (oper == "stockPro")
                {
                    string proidstr = context.Request["proidstr"].ConvertTo<string>("");
                    string pronamestr = context.Request["pronamestr"].ConvertTo<string>("");
                    int isstock = context.Request["isstock"].ConvertTo<int>(0);
                    int groupbuytype = context.Request["groupbuytype"].ConvertTo<int>(0);
                    int operuserid = context.Request["operuserid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int stockagentcompanyid = context.Request["stockagentcompanyid"].ConvertTo<int>(0);
                    string stockagentcompanyname = context.Request["stockagentcompanyname"].ConvertTo<string>("");

                    string data = ProductJsonData.StockPro(proidstr,pronamestr,isstock,groupbuytype,operuserid,comid,stockagentcompanyid,stockagentcompanyname);
                    context.Response.Write(data);
                }
                if (oper == "getNotStockProPagelist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var agentid = context.Request["agentid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);

                    string data = ProductJsonData.GetNotStockProPagelist(comid, agentid, pageindex, pagesize);
                    context.Response.Write(data);
                }
                if (oper == "groupbuystocklogpagelist")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var groupbuytype = context.Request["groupbuytype"].ConvertTo<int>(0);
                    var stockstate = context.Request["stockstate"].ConvertTo<string>("0,1");
                    var comid = context.Request["comid"].ConvertTo<int>(0);


                    string data = ProductJsonData.GroupbuyStockLogPagelist(pageindex, pagesize, key, groupbuytype, stockstate, comid);

                    context.Response.Write(data);
                }
                if (oper == "editprogroup")
                {
                    int progroupid = context.Request["progroupid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string groupname = context.Request["groupname"].ConvertTo<string>("");
                    int runstate = context.Request["runstate"].ConvertTo<int>(1);

                    string data = ProductJsonData.Editprogroup(progroupid, comid, groupname, runstate);
                    context.Response.Write(data);
                }
                if (oper == "GetProgrouplistByComid")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string runstate = context.Request["runstate"].ConvertTo<string>("0,1");
                    string data = ProductJsonData.GetProgrouplistByComid(comid, runstate);
                    context.Response.Write(data);
                }
                if (oper == "GetProgroupPagelistByComid")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    string data = ProductJsonData.GetProgroupPagelistByComid(comid, pageindex, pagesize);
                    context.Response.Write(data);
                }
                if (oper == "CompanyPoslist")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = ProductJsonData.CompanyPoslist(comid);
                    context.Response.Write(data);
                }
                //服务归还超时统计
                if (oper == "serverTimeoutPagelist")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    string startime = context.Request["startime"].ConvertTo<string>("");
                    string endtime = context.Request["endtime"].ConvertTo<string>("");
                    string key = context.Request["key"].ConvertTo<string>("");

                    string data = ProductJsonData.GetserverTimeoutPagelist(comid, pageindex, pagesize, startime, endtime, key);
                    context.Response.Write(data);
                }

                //索道票打印统计
                if (oper == "serverSuodaoPagelist")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string startime = context.Request["startime"].ConvertTo<string>(DateTime.Now.ToString("yyyy-MM-dd"));
                    string endtime = context.Request["endtime"].ConvertTo<string>(DateTime.Now.ToString("yyyy-MM-dd"));
                    string key = context.Request["key"].ConvertTo<string>("");

                    string data = ProductJsonData.serverSuodaoPagelist(comid, startime, endtime, key);
                    context.Response.Write(data);
                }
                //闸机日志
                if (oper == "zhajilogPagelist")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int id = context.Request["id"].ConvertTo<int>(0);


                    string data = ProductJsonData.zhajilogPagelist(comid, id);
                    context.Response.Write(data);
                }
                

                //服务发卡统计
                if (oper == "serverfakaPagelist")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    string startime = context.Request["startime"].ConvertTo<string>("");
                    string endtime = context.Request["endtime"].ConvertTo<string>("");
                    string key = context.Request["key"].ConvertTo<string>("");
                    int serverstate = context.Request["serverstate"].ConvertTo<int>(-1);
                    int serverid = context.Request["serverid"].ConvertTo<int>(0);

                    string data = ProductJsonData.GetserverfakaPagelist(comid, pageindex, pagesize, startime, endtime, key, serverstate, serverid);
                    context.Response.Write(data);
                }

                //服务发卡统计
                if (oper == "xiaojiancountPagelist")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(20);
                    string startime = context.Request["startime"].ConvertTo<string>(DateTime.Now.ToString("yyyy-MM-dd"));
                    string endtime = context.Request["endtime"].ConvertTo<string>(DateTime.Now.ToString("yyyy-MM-dd"));

                    endtime = endtime + " 23:59:59";

                    string data = ProductJsonData.xiaojiancountPagelist(comid, pageindex, pagesize, startime, endtime);
                    context.Response.Write(data);
                }

                //退押金
                if (oper == "tuiyajin")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int id = context.Request["id"].ConvertTo<int>(0);

                    string data = OrderJsonData.BackServerDeposit(id, comid);
                    context.Response.Write(data);
                }
                if (oper == "getorderstatelist")
                {
                    string data = ProductJsonData.Getorderstatelist();
                    context.Response.Write(data);
                }
                if (oper == "Upworktimeblackoutdate")
                {
                    string initdatestr = context.Request["initdatestr"].ConvertTo<string>("");
                    string datestr = context.Request["datestr"].ConvertTo<string>("");
                    string starttime = context.Request["starttime"].ConvertTo<string>("");
                    string endtime = context.Request["endtime"].ConvertTo<string>("");
                    int userid = context.Request["userid"].ConvertTo<int>();
                    int proworktimeid = context.Request["proworktimeid"].ConvertTo<int>();
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = ProductJsonData.Upworktimeblackoutdate(proworktimeid, initdatestr, datestr, starttime, endtime, userid, comid);
                    context.Response.Write(data);
                }
                if (oper == "DelblackoutdateByWorktimeId")
                {
                    string daydate = context.Request["daydate"].ConvertTo<string>("");
                    int proworktimeid = context.Request["proworktimeid"].ConvertTo<int>(0);
                    string data = ProductJsonData.DelblackoutdateByWorktimeId(daydate, proworktimeid);

                    context.Response.Write(data);
                }
                if (oper == "GetblackoutdateByWorktimeId")
                {
                    string daydate = context.Request["daydate"].ConvertTo<string>("");
                    int proworktimeid = context.Request["proworktimeid"].ConvertTo<int>(0);
                    string data = ProductJsonData.GetblackoutdateByWorktimeId(daydate, proworktimeid);

                    context.Response.Write(data);
                }
                if (oper == "getservertypelist")
                {
                    string data = ProductJsonData.Getservertypelist();
                    context.Response.Write(data);
                }
                //删除服务卡 和 卡面印刷编号的关联
                if (oper == "delRelationserver_printid_chipid")
                {
                    int relationid = context.Request["relationid"].ConvertTo<int>(0);

                    string data = ProductJsonData.delRelationserver_printid_chipid(relationid);
                    context.Response.Write(data);
                }

                //把服务卡 芯片id 和 卡面印刷编号对应
                if (oper == "Relationserver_printid_chipid")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string cardchipid = context.Request["cardchipid"].ConvertTo<string>("");
                    int cardprintid = context.Request["cardprintid"].ConvertTo<int>(0);
                    string data = ProductJsonData.Relationserver_printid_chipid(comid, cardchipid, cardprintid);
                    context.Response.Write(data);
                }
                //得到服务卡列表
                if (oper == "Relationserver_printid_chipidList")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    decimal cardchipid = context.Request["cardchipid"].ConvertTo<decimal>(0);
                    int beginprintid = context.Request["beginprintid"].ConvertTo<int>(0);
                    int endprintid = context.Request["endprintid"].ConvertTo<int>(0);
                    string data = ProductJsonData.Relationserver_printid_chipidList(comid, cardchipid, beginprintid, endprintid);
                    context.Response.Write(data);
                }
                //得到商户下保险产品（产品id；产品名称）
                if (oper == "getbaoxianlist")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = ProductJsonData.Getbaoxianlist(comid);
                    context.Response.Write(data);
                }
                if (oper == "getguigepricearr")
                {
                    int proid = context.Request["proid"].ConvertTo<int>(0);
                    string data = ProductJsonData.Getguigepricearr(proid);
                    context.Response.Write(data);
                }
                if (oper == "channelcompanylist")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string Issuetype = context.Request["Issuetype"].ConvertTo<string>();
                    string companystate = context.Request["companystate"].ConvertTo<string>("");
                    string key = context.Request["key"].ConvertTo<string>("");

                    string data = ProductJsonData.Getchannelcompanylist(comid, Issuetype, companystate, key);
                    context.Response.Write(data);
                }
                if (oper == "getjoinpolicy")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = ProductJsonData.Getjoinpolicy(comid);
                    context.Response.Write(data);
                }
                if (oper == "editjoinpolicy")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var joinpolicy = context.Request["joinpolicy"].ConvertTo<string>("");

                    string data = ProductJsonData.Editjoinpolicy(comid, joinpolicy);
                    context.Response.Write(data);
                }
                if (oper == "WhetherSaled")
                {
                    int lineid = context.Request["lineid"].ConvertTo<int>(0);
                    string daydate = context.Request["daydate"].ConvertTo<string>("");

                    string data = ProductJsonData.WhetherSaled(lineid, daydate);
                    context.Response.Write(data);
                }
                if (oper == "deliverytmppagelist")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(20);

                    string data = ProductJsonData.Deliverytmppagelist(comid, pageindex, pagesize);
                    context.Response.Write(data);
                }
                if (oper == "getprochildimglist")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int proid = context.Request["proid"].ConvertTo<int>(0);
                    string data = ProductJsonData.Getprochildimglist(comid, proid);
                    context.Response.Write(data);
                }
                if (oper == "deltmp")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int tmpid = context.Request["tmpid"].ConvertTo<int>(0);
                    string data = ProductJsonData.deltmp(comid, tmpid);
                    context.Response.Write(data);
                }
                if (oper == "getdeliverytmplist")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = ProductJsonData.Getdeliverytmplist(comid);
                    context.Response.Write(data);
                }
                if (oper == "getdeliverytmp")
                {
                    int tmpid = context.Request["tmpid"].ConvertTo<int>(0);
                    string data = ProductJsonData.Getdeliverytmp(tmpid);
                    context.Response.Write(data);
                }
                if (oper == "editdeliverytmp")
                {
                    int tmpid = context.Request["tmpid"].ConvertTo<int>(0);
                    string tmpname = context.Request["tmpname"].ConvertTo<string>("");
                    string deliverytypes = context.Request["deliverytypes"].ConvertTo<string>("");
                    string join_deliverytype = context.Request["join_deliverytype"].ConvertTo<string>("");
                    int ComputedPriceMethod = context.Request["ComputedPriceMethod"].ConvertTo<int>(1);


                    string join_provinces = context.Request["join_provinces"].ConvertTo<string>("");
                    string join_areas = context.Request["join_areas"].ConvertTo<string>("");
                    string join_startstandards = context.Request["join_startstandards"].ConvertTo<string>("");
                    string join_startfees = context.Request["join_startfees"].ConvertTo<string>("");
                    string join_addstandards = context.Request["join_addstandards"].ConvertTo<string>("");
                    string join_addfees = context.Request["join_addfees"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int operor = context.Request["operor"].ConvertTo<int>(0);

                    string data = ProductJsonData.Editdeliverytmp(ComputedPriceMethod, join_provinces, deliverytypes, tmpid, tmpname, join_deliverytype, join_areas, join_startstandards, join_startfees, join_addstandards, join_addfees, comid, operor);
                    context.Response.Write(data);
                }

                if (oper == "uplimitbuytotalnum")
                {
                    int proid = context.Request["proid"].ConvertTo<int>(0);
                    int upnum = context.Request["upnum"].ConvertTo<int>(0);

                    string data = ProductJsonData.Uplimitbuytotalnum(proid, upnum);
                    context.Response.Write(data);
                }
                if (oper == "Geteticket_usesetlist")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = ProductJsonData.Geteticket_usesetlist(comid);
                    context.Response.Write(data);
                }
                if (oper == "Upcomblackoutdate")
                {
                    string initdatestr = context.Request["initdatestr"].ConvertTo<string>("");
                    string datestr = context.Request["datestr"].ConvertTo<string>("");
                    string datetype = context.Request["datetype"].ConvertTo<string>("");
                    int userid = context.Request["userid"].ConvertTo<int>();
                    int comid = context.Request["comid"].ConvertTo<int>();

                    string ordinaryday_etickettypes = context.Request["ordinaryday_etickettypes"].ConvertTo<string>("");
                    string weekday_etickettypes = context.Request["weekday_etickettypes"].ConvertTo<string>("");
                    string holiday_etickettypes = context.Request["holiday_etickettypes"].ConvertTo<string>("");


                    string data = ProductJsonData.Upcomblackoutdate(ordinaryday_etickettypes, weekday_etickettypes, holiday_etickettypes, comid, initdatestr, datestr, datetype, userid);
                    context.Response.Write(data);
                }
                if (oper == "Delblackoutdate")
                {
                    string daydate = context.Request["daydate"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = ProductJsonData.Delblackoutdate(daydate, comid);

                    context.Response.Write(data);

                }


                //得到公司特定日期某天的设定
                if (oper == "Getblackoutdate")
                {
                    string daydate = context.Request["daydate"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = ProductJsonData.Getblackoutdate(daydate, comid);

                    context.Response.Write(data);

                }
                //得到公司预约列表
                if (oper == "getbespeaklist")
                {
                    string bespeakdate = context.Request["bespeakdate"].ConvertTo<string>("");
                    string comid = context.Request["comid"].ConvertTo<string>("");
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(20);
                    string key = context.Request["key"].ConvertTo<string>("");
                    string bespeaktype = context.Request["bespeaktype"].ConvertTo<string>("0,1");
                    string bespeakstate = context.Request["bespeakstate"].ConvertTo<string>("0");

                    string data = ProductJsonData.Getbespeaklist(pageindex, pagesize, bespeakdate, comid, key, bespeaktype, bespeakstate);

                    context.Response.Write(data);

                }
                if (oper == "operbespeakstate")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    int bespeakstate = context.Request["bespeakstate"].ConvertTo<int>(0);
                    string data = ProductJsonData.Operbespeakstate(id, bespeakstate);

                    context.Response.Write(data);
                }
                // 得到预约日期列表(暂时设为一个月)
                if (oper == "Getbespeakdatelist")
                {
                    string data = ProductJsonData.Getbespeakdatelist();
                    context.Response.Write(data);
                }
                if (oper == "subautobespeak")
                {
                    var bespeakname = context.Request["bespeakname"].ConvertTo<string>("");
                    var phone = context.Request["phone"].ConvertTo<string>("");
                    var idcard = context.Request["idcard"].ConvertTo<string>("");
                    var bespeakdate = context.Request["bespeakdate"].ConvertTo<DateTime>();
                    var bespeaknum = context.Request["bespeaknum"].ConvertTo<int>(0);
                    var remark = context.Request["remark"].ConvertTo<string>("");
                    var orderid = context.Request["orderid"].ConvertTo<int>(0);
                    var pno = context.Request["pno"].ConvertTo<string>("");

                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var proid = context.Request["proid"].ConvertTo<int>(0);
                    var proname = context.Request["proname"].ConvertTo<string>("");

                    B2b_order_bespeak m = new B2b_order_bespeak
                    {
                        Id = 0,
                        Bespeakname = bespeakname,
                        Phone = phone,
                        Idcard = idcard,
                        Bespeakdate = bespeakdate,
                        Bespeaknum = bespeaknum,
                        Remark = remark,
                        Orderid = orderid,
                        Comid = comid,
                        Pno = pno,
                        Proid = proid,
                        Proname = proname,
                        beaspeakstate = 0,
                        beaspeaktype = 1,
                        subtime = DateTime.Now,
                        remark = remark
                    };

                    string data = ProductJsonData.Subautobespeak(m);
                    context.Response.Write(data);

                }
                if (oper == "GetMinValidByProjectid")
                {
                    var projectid = context.Request["projectid"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = ProductJsonData.GetMinValidByProjectid(projectid, comid);
                    context.Response.Write(data);
                }

                if (oper == "GetMinValidByProid")
                {
                    var proid = context.Request["proid"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = ProductJsonData.GetMinValidByProid(proid, comid);
                    context.Response.Write(data);
                }
                if (oper == "getProById")
                {
                    var proid = context.Request["proid"];
                    string data = ProductJsonData.GetProById(proid);
                    context.Response.Write(data);
                }
                if (oper == "getprobypno")
                {
                    var pno = context.Request["pno"].ConvertTo<string>("");
                    string getcode = context.Request["getcode"];
                    string logindata = "";
                    try
                    {
                        if (context.Session["SomeValidateCode"] == null)
                        {
                            logindata = "{\"type\":1,\"msg\":\"【用户验证码错误】\"}";
                        }
                        else
                        {
                            string initcode = context.Session["SomeValidateCode"].ToString();
                            if (getcode != initcode)
                            {
                                logindata = "{\"type\":1,\"msg\":\"【用户验证码错误】\"}";
                            }
                            else
                            {
                                logindata = ProductJsonData.Getprobypno(pno);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        logindata = "{\"type\":1,\"msg\":\"【用户验证码错误】\"}";
                    }
                    context.Response.Write(logindata);
                }
                if (oper == "GetLineById")
                {
                    var lineid = context.Request["lineid"].ConvertTo<int>(0);
                    string data = ProductJsonData.GetLineById(lineid);
                    context.Response.Write(data);
                }

                if (oper == "getLinetripById")
                {
                    var lineid = context.Request["lineid"].ConvertTo<int>(0);
                    string data = ProductJsonData.GetTripById(lineid);
                    context.Response.Write(data);
                }


                if (oper == "getagentProById")
                {
                    var proid = context.Request["proid"];
                    var agentid = context.Request["agentid"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);

                    int agent_level = 0;//分销级别

                    if (agentid != 0)
                    {
                        Agent_company agentinfo = AgentCompanyData.GetAgentWarrant(agentid, comid);
                        if (agentinfo != null)
                        {
                            agent_level = agentinfo.Warrant_level;
                        }

                    }
                    string data = ProductJsonData.GetAgentProById(proid, agent_level);
                    context.Response.Write(data);
                }

                if (oper == "ClientProById")
                {
                    string RequestUrl = context.Request.ServerVariables["SERVER_NAME"].ToLower();
                    int comid = 0;
                    //根据域名读取商户ID,如果没有绑定域名直接跳转后台
                    B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestUrl);
                    if (companyinfo != null)
                    {
                        comid = companyinfo.Com_id;
                    }
                    else
                    {
                        //判定是否为自助域名规则安 shop1.etown.cn
                        if (Domain_def.Domain_yanzheng(RequestUrl))
                        {
                            comid = Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl));
                        }
                    }
                    var proid = context.Request["proid"];
                    string data = ProductJsonData.ClientGetProById(proid, comid);

                    context.Response.Write(data);
                }

                if (oper == "editprobasic")
                {
                    var issetidcard = context.Request["issetidcard"].ConvertTo<int>(0);
                    var progroupid = context.Request["progroupid"].ConvertTo<int>(0);
                    var SpecifyPosid = context.Request["SpecifyPosid"].ConvertTo<string>("");
                    var pnonumperticket = context.Request["pnonumperticket"].ConvertTo<int>(1);
                    var firststationtime = context.Request["firststationtime"].ConvertTo<string>("");
                    var pro_yanzheng_method = context.Request["pro_yanzheng_method"].ConvertTo<int>(0);
                    var selbindbx = context.Request["selbindbx"].ConvertTo<int>(0);
                    var userid = context.Request["userid"];
                    var comid = context.Request["comid"];

                    var pro_id = context.Request["pro_id"].ConvertTo<int>(0);
                    var merchant_code = context.Request["merchant_code"].ConvertTo<string>("");


                    var pro_name = context.Request["pro_name"];
                    var server_type = context.Request["server_type"];
                    var pro_type = context.Request["pro_type"];
                    var source_type = context.Request["source_type"];
                    var pro_Remark = context.Request["pro_Remark"];

                    var proclass = context.Request["proclass"].ConvertTo<int>(0);

                    var pro_start = context.Request["pro_start"];
                    var pro_end = context.Request["pro_end"];
                    var face_price = context.Request["face_price"];
                    var advise_price = context.Request["advise_price"];
                    var agent1_price = context.Request["agent1_price"];
                    var agent2_price = context.Request["agent2_price"];
                    var agent3_price = context.Request["agent3_price"];
                    var agentsettle_price = context.Request["agentsettle_price"];
                    var tuan_pro = context.Request["tuan_pro"];
                    var zhixiao = context.Request["zhixiao"];
                    var agentsale = context.Request["agentsale"];
                    var ThatDay_can = context.Request["ThatDay_can"];
                    var Thatday_can_day = context.Request["Thatday_can_day"];
                    var service_Contain = context.Request["service_Contain"].ConvertTo<string>("");
                    var service_NotContain = context.Request["service_NotContain"].ConvertTo<string>("");
                    var Precautions = context.Request["Precautions"].ConvertTo<string>("");
                    var Sms = context.Request["Sms"].ConvertTo<string>("");


                    var Imgurl = context.Request["imgurl"].ConvertTo<int>(0);
                    var pro_Number = context.Request["pro_Number"].ConvertTo<int>(0);
                    var pro_Explain = context.Request["pro_Explain"].ConvertTo<string>("");

                    var pro_Integral = context.Request["pro_Integral"].ConvertTo<decimal>(0);

                    var tuipiao = context.Request["tuipiao"].ConvertTo<int>(1);
                    var tuipiao_guoqi = context.Request["tuipiao_guoqi"].ConvertTo<int>(0);
                    var tuipiao_endday = context.Request["tuipiao_endday"].ConvertTo<int>(0);



                    var prostate = context.Request["prostate"].ConvertTo<int>(0);


                    int serviceid = context.Request["serviceid"].ConvertTo<int>(0);
                    string Service_proid = context.Request["service_proid"].ConvertTo<string>("");
                    int realnametype = context.Request["realnametype"].ConvertTo<int>(0);

                    int projectid = context.Request["projectid"].ConvertTo<int>(0);

                    int travelproductid = context.Request["travelproductid"].ConvertTo<int>(0);
                    int traveltype = context.Request["traveltype"].ConvertTo<int>(0);
                    string travelstarting = context.Request["travelstarting"].ConvertTo<string>("");
                    string travelending = context.Request["travelending"].ConvertTo<string>("");

                    int ispanicbuy = context.Request["ispanicbuy"].ConvertTo<int>(0);
                    DateTime panic_begintime = context.Request["panic_begintime"].ConvertTo<DateTime>(DateTime.Now);
                    DateTime panicbuy_endtime = context.Request["panicbuy_endtime"].ConvertTo<DateTime>(DateTime.Now);
                    int panicbuy_limitbuytotalnum = context.Request["panicbuy_limitbuytotalnum"].ConvertTo<int>(10000);
                    int linepro_booktype = context.Request["linepro_booktype"].ConvertTo<int>(0);

                    string ProValidateMethod = context.Request["ProValidateMethod"].ConvertTo<string>("1");
                    int appointdata = context.Request["appointdata"].ConvertTo<int>(0);
                    int iscanuseonsameday = context.Request["iscanuseonsameday"].ConvertTo<int>(1);
                    int viewmethod = context.Request["viewmethod"].ConvertTo<int>(1);

                    decimal childreduce = context.Request["childreduce"].ConvertTo<decimal>(0);

                    string pickuppoint = context.Request["pickuppoint"].ConvertTo<string>("").Replace(",", "，");
                    string dropoffpoint = context.Request["dropoffpoint"].ConvertTo<string>("").Replace(",", "，");

                    string pro_note = context.Request["pro_note"].ConvertTo<string>("");

                    int QuitTicketMechanism = context.Request["QuitTicketMechanism"].ConvertTo<int>(0);

                    int isneedbespeak = context.Request["isneedbespeak"].ConvertTo<int>(0);
                    int daybespeaknum = context.Request["daybespeaknum"].ConvertTo<int>(0);
                    string bespeaksucmsg = context.Request["bespeaksucmsg"].ConvertTo<string>("");
                    string bespeakfailmsg = context.Request["bespeakfailmsg"].ConvertTo<string>("");
                    string customservicephone = context.Request["customservicephone"].ConvertTo<string>("");
                    int isblackoutdate = context.Request["isblackoutdate"].ConvertTo<int>(0);
                    int etickettype = context.Request["etickettype"].ConvertTo<int>(0);


                    int ishasdeliveryfee = context.Request["ishasdeliveryfee"].ConvertTo<int>(0);
                    int deliverytmp = context.Request["deliverytmp"].ConvertTo<int>(0);

                    var MultiImgUpIds = context.Request["MultiImgUpIds"].ConvertTo<string>("");
                    var pro_weight = context.Request["pro_weight"].ConvertTo<decimal>(0);
                    //预订产品新增3个属性:产品关联人姓名 ;关联人手机（给预订发通知）;预订前是否支付
                    var bookpro_bindname = context.Request["bookpro_bindname"].ConvertTo<string>("");
                    var bookpro_bindphone = context.Request["bookpro_bindphone"].ConvertTo<string>("");
                    var bookpro_bindcompany = context.Request["bookpro_bindcompany"].ConvertTo<string>("");
                    var bookpro_ispay = context.Request["bookpro_ispay"].ConvertTo<int>(0);

                    //多规格信息
                    var manyspeci = context.Request["manyspeci"].ConvertTo<int>(0);
                    var guigestr = context.Request["guigestr"].ConvertTo<string>("");

                    var isrebate = context.Request["isrebate"].ConvertTo<int>(0);
                    var unsure = context.Request["unsure"].ConvertTo<int>(0);
                    var unyuyueyanzheng = context.Request["unyuyueyanzheng"].ConvertTo<int>(0);


                    var Wrentserver = context.Request["Wrentserver"].ConvertTo<int>(0);
                    var WDeposit = context.Request["WDeposit"].ConvertTo<int>(0);
                    var Depositprice = context.Request["Depositprice"].ConvertTo<decimal>(0);
                    var Rentserverid = context.Request["Rentserverid"].ConvertTo<string>("");

                    var worktimehour = context.Request["worktimehour"].ConvertTo<int>(0);
                    var worktimeid = context.Request["worktimeid"].ConvertTo<int>(0);

                    var isSetVisitDate = context.Request["isSetVisitDate"].ConvertTo<int>(0);
                    var bandingzhajiid = context.Request["bandingzhajiid"].ConvertTo<string>("");

                    var zhaji_usenum = context.Request["zhaji_usenum"].ConvertTo<int>(999);

                    //如果不需要押金则押金金额为0
                    if (WDeposit == 0)
                    {
                        Depositprice = 0;
                    }

                    B2b_com_pro product = new B2b_com_pro
                    {
                        issetidcard = issetidcard,
                        progroupid = progroupid,
                        isSetVisitDate = isSetVisitDate,
                        SpecifyPosid = SpecifyPosid,
                        pnonumperticket = pnonumperticket,
                        firststationtime = firststationtime,
                        pro_yanzheng_method = pro_yanzheng_method,
                        selbindbx = selbindbx,
                        isrebate = isrebate,
                        Manyspeci = manyspeci,
                        guigestr = guigestr,
                        bookpro_bindname = bookpro_bindname,
                        bookpro_bindphone = bookpro_bindphone,
                        bookpro_bindcompany = bookpro_bindcompany,
                        bookpro_ispay = bookpro_ispay,
                        pro_weight = pro_weight,
                        MultiImgUpIds = MultiImgUpIds,
                        ishasdeliveryfee = ishasdeliveryfee,
                        deliverytmp = deliverytmp,
                        isneedbespeak = isneedbespeak,
                        daybespeaknum = daybespeaknum,
                        bespeaksucmsg = bespeaksucmsg,
                        bespeakfailmsg = bespeakfailmsg,
                        customservicephone = customservicephone,
                        isblackoutdate = isblackoutdate,
                        etickettype = etickettype,
                        QuitTicketMechanism = QuitTicketMechanism,//退票机制
                        pro_note = pro_note,//票务备注
                        pickuppoint = pickuppoint,
                        dropoffpoint = dropoffpoint,
                        Childreduce = childreduce,
                        ProValidateMethod = ProValidateMethod,
                        Appointdata = appointdata,
                        Iscanuseonsameday = iscanuseonsameday,
                        Viewmethod = viewmethod,
                        Linepro_booktype = linepro_booktype,
                        Ispanicbuy = ispanicbuy,
                        Panic_begintime = panic_begintime,
                        Panicbuy_endtime = panicbuy_endtime,
                        Limitbuytotalnum = panicbuy_limitbuytotalnum,

                        Projectid = projectid,//项目id
                        Serviceid = serviceid,//服务商id
                        Service_proid = Service_proid,//服务商产品编号
                        Realnametype = realnametype,//服务商产品实名制类型
                        Id = pro_id,
                        merchant_code=merchant_code,
                        Pro_name = pro_name,
                        Server_type = server_type.ConvertTo<int>(0),
                        Pro_type = pro_type.ConvertTo<int>(0),
                        Source_type = source_type.ConvertTo<int>(1),
                        Pro_Remark = pro_Remark,
                        Pro_start = pro_start.ConvertTo<DateTime>(DateTime.Now),
                        Pro_end = pro_end.ConvertTo<DateTime>(DateTime.Now),
                        Face_price = face_price.ConvertTo<decimal>(0),
                        Advise_price = advise_price.ConvertTo<decimal>(0),
                        Agentsettle_price = agentsettle_price.ConvertTo<decimal>(0),
                        Tuan_pro = tuan_pro.ConvertTo<int>(0),
                        Zhixiao = zhixiao.ConvertTo<int>(0),
                        Agentsale = agentsale.ConvertTo<int>(0),
                        ThatDay_can = ThatDay_can.ConvertTo<int>(0),
                        Thatday_can_day = Thatday_can_day.ConvertTo<int>(0),
                        Service_Contain = service_Contain,
                        Service_NotContain = service_NotContain,
                        Precautions = Precautions,
                        Com_id = comid.ConvertTo<int>(0),
                        Createuserid = userid.ConvertTo<int>(0),
                        Imgurl = Imgurl,
                        Pro_number = pro_Number,
                        Pro_explain = pro_Explain,
                        Pro_Integral = pro_Integral,
                        Pro_state = prostate,
                        Sms = Sms,
                        Agent1_price = agent1_price.ConvertTo<decimal>(0),
                        Agent2_price = agent2_price.ConvertTo<decimal>(0),
                        Agent3_price = agent3_price.ConvertTo<decimal>(0),
                        Tuipiao = tuipiao,
                        Tuipiao_guoqi = tuipiao_guoqi,
                        Tuipiao_endday = tuipiao_endday,
                        Proclass = proclass,

                        Travelproductid = travelproductid,
                        Traveltype = traveltype,
                        Travelstarting = travelstarting,
                        Travelending = travelending,
                        unyuyueyanzheng = unyuyueyanzheng,
                        unsure = unsure,
                        Wrentserver = Wrentserver,
                        WDeposit = WDeposit,
                        Depositprice = Depositprice,
                        Rentserverid = Rentserverid,
                        worktimehour = worktimehour,
                        worktimeid = worktimeid,
                        bandingzhajiid = bandingzhajiid,
                        zhaji_usenum = zhaji_usenum,
                    };
                    string data = ProductJsonData.InsertOrUpdatePro(product);

                    context.Response.Write(data);
                }
                if (oper == "modifyproExt")
                {
                    var proid = context.Request["proid"];
                    var comid = context.Request["comid"];
                    var userid = context.Request["userid"];

                    var service_Contain = context.Request["service_Contain"];
                    var service_NotContain = context.Request["service_NotContain"];
                    var Precautions = context.Request["Precautions"];
                    var Sms = context.Request["Sms"];

                    string data = ProductJsonData.ModifyProExt(proid, service_Contain, service_NotContain, Precautions, Sms);

                    context.Response.Write(data);
                }
                if (oper == "pagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");
                    int projectid = context.Request["projectid"].ConvertTo<int>(0);
                    int pro_state = context.Request["pro_state"].ConvertTo<int>(0);
                    int userid = context.Request["userid"].ConvertTo<int>(0);
                    int servertype = context.Request["servertype"].ConvertTo<int>(0);

                    string data = ProductJsonData.ProPageList(comid, pageindex, pagesize, pro_state, projectid, key, "", userid, servertype);

                    context.Response.Write(data);
                }

                if (oper == "wlpagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");
                    
                    int pro_state = context.Request["pro_state"].ConvertTo<int>(0);
                    int userid = context.Request["userid"].ConvertTo<int>(0);

                    string data = ProductJsonData.WLProPageList(comid, pageindex, pagesize, pro_state, key);

                    context.Response.Write(data);
                }
                if (oper == "wluppro")
                {
                    var comid = context.Request["comid"];
                    

                    string data = ProductJsonData.WLProup(comid);

                    context.Response.Write(data);
                }
                if (oper == "pagelistname")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");
                    int projectid = context.Request["projectid"].ConvertTo<int>(0);
                    int pro_state = context.Request["pro_state"].ConvertTo<int>(0);

                    string data = ProductJsonData.ProPageListName(comid, pageindex, pagesize, pro_state, projectid, key);

                    context.Response.Write(data);
                }
                if (oper == "sortlist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = ProductJsonData.sortlist(comid);

                    context.Response.Write(data);
                }

                if (oper == "Menusort")
                {
                    string menuids = context.Request["menuids"].ConvertTo<string>("");

                    string data = ProductJsonData.MenuSort(menuids);
                    context.Response.Write(data);
                }

                if (oper == "projectMenusort")
                {
                    string menuids = context.Request["menuids"].ConvertTo<string>("");

                    string data = ProductJsonData.ProjectMenusort(menuids);
                    context.Response.Write(data);
                }

                if (oper == "statepagelist")
                {
                    var state = 1;
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    string data = ProductJsonData.statepagelist(comid, pageindex, pagesize, state);

                    context.Response.Write(data);
                }

                if (oper == "searchpnolist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var pro_id = context.Request["pro_id"].ConvertTo<int>(10);
                    var statetype = context.Request["statetype"].ConvertTo<int>(10);
                    string data = ProductJsonData.SearchPnoPageList(comid, pageindex, pagesize, pro_id, statetype);

                    context.Response.Write(data);
                }

                if (oper == "Compagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    string data = ProductJsonData.ComPageList(pageindex, pagesize, 1);

                    context.Response.Write(data);
                }
                if (oper == "Comlist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var comstate = context.Request["comstate"].ConvertTo<int>(0);

                    string key = context.Request["key"].ConvertTo<string>("");
                    string data = ProductJsonData.ComList(pageindex, pagesize, comstate, key);

                    context.Response.Write(data);
                }

                if (oper == "Companysortlist")
                {

                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var comstate = context.Request["comstate"].ConvertTo<int>(0);

                    string data = ProductJsonData.Companysortlist(pageindex, pagesize, comstate);

                    context.Response.Write(data);
                }
                if (oper == "ComSort")
                {
                    string comids = context.Request["comids"].ConvertTo<string>("");

                    string data = ProductJsonData.ComSort(comids);
                    context.Response.Write(data);
                }
                if (oper == "UpComPlatform_state")
                {

                    var id = context.Request["id"].ConvertTo<int>(0);
                    var state = context.Request["state"].ConvertTo<string>("");
                    string data = ProductJsonData.UpCom(id, state);
                    context.Response.Write(data);
                }
                if (oper == "UpComstate")
                {
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var state = context.Request["state"].ConvertTo<string>("");
                    string data = ProductJsonData.UpComstate(id, state);
                    context.Response.Write(data);
                }
                if (oper == "ComSelectpagelist")
                {
                    //var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var proclass = context.Request["proclass"].ConvertTo<int>(0);
                    string data = ProductJsonData.ComSelectpagelist(1, pageindex, pagesize, key, proclass);

                    context.Response.Write(data);
                }
                if (oper == "ProjectSelectpagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var proclass = context.Request["proclass"].ConvertTo<int>(0);
                    int projectid = context.Request["projectid"].ConvertTo<int>(0);
                    int price = context.Request["price"].ConvertTo<int>(0);

                    var accountid = 0;
                    if (context.Request.Cookies["AccountId"] != null)
                    {
                        accountid = context.Request.Cookies["AccountId"].Value.ConvertTo<int>(0);
                    }
                    var openid = "";
                    var b2bdate = new B2bCrmData();
                    if (accountid != 0)
                    {
                        var b2bmodel = b2bdate.Readuser(accountid, int.Parse(comid));
                        if (b2bmodel != null)
                        {
                            openid = b2bmodel.Weixin;
                        }
                    }




                    string data = ProductJsonData.ProjectSelectpagelist(1, pageindex, pagesize, key, proclass, comid, projectid, openid, price);

                    context.Response.Write(data);
                }

                if (oper == "Selectpagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");
                    int projectid = context.Request["projectid"].ConvertTo<int>(0);
                    int proclass = context.Request["proclass"].ConvertTo<int>(0);
                    int isoutpro = context.Request["isoutpro"].ConvertTo<int>(0);
                    string openid = context.Request["openid"].ConvertTo<string>("");
                    string viewmethod = context.Request["viewmethod"].ConvertTo<string>("");
                    int MasterId = context.Request["MasterId"].ConvertTo<int>(0);
                    int menuid = context.Request["menuid"].ConvertTo<int>(0);
                    int channelid = context.Request["channelid"].ConvertTo<int>(0);

                    var pno = context.Request["pno"].ConvertTo<string>("");//在线预约使用
                    int Servertype = context.Request["Servertype"].ConvertTo<int>(0);
                    //针对顾问页面 去掉产品分类选择，而选择了项目。
                    if (isoutpro == 0)
                    {
                        proclass = 0;
                    }

                    string data = ProductJsonData.Selectpagelist(comid, pageindex, pagesize, key, projectid, proclass, isoutpro, openid, viewmethod, MasterId, menuid, channelid, pno, Servertype);

                    context.Response.Write(data);
                }
                if (oper == "SelectMenupagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int projectid = context.Request["projectid"].ConvertTo<int>(0);
                    int menuid = context.Request["menuid"].ConvertTo<int>(0);
                    int allview = context.Request["allview"].ConvertTo<int>(0);
                    string key = context.Request["key"].ConvertTo<string>("");

                    string data = ProductJsonData.SelectMenupagelist(comid, pageindex, pagesize, menuid, projectid, allview, key);

                    context.Response.Write(data);
                }

                if (oper == "SelectMenuHotelpagelist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int projectid = context.Request["projectid"].ConvertTo<int>(0);
                    int menuid = context.Request["menuid"].ConvertTo<int>(0);
                    int allview = context.Request["allview"].ConvertTo<int>(0);
                    string key = context.Request["key"].ConvertTo<string>("");

                    string data = ProductJsonData.SelectMenuHotelpagelist(comid, pageindex, pagesize, projectid);

                    context.Response.Write(data);
                }


                if (oper == "XiangguanPropagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int proid = context.Request["proid"].ConvertTo<int>(0);

                    string data = ProductJsonData.XiangguanPropagelist(comid, pageindex, pagesize, proid);

                    context.Response.Write(data);
                }
                if (oper == "TopPropagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);


                    string data = ProductJsonData.TopPropagelist(comid, pageindex, pagesize);

                    context.Response.Write(data);
                }


                if (oper == "modifyprostate")
                {
                    var proid = context.Request["proid"].ConvertTo<int>(0);
                    var prostate = context.Request["prostate"].ConvertTo<int>(0);

                    string data = ProductJsonData.ModifyProState(proid, prostate);
                    context.Response.Write(data);
                }
                if (oper == "lurueticket")
                {
                    var proid = context.Request["proid"].ConvertTo<int>(0);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var leixing = context.Request["leixing"].ConvertTo<string>("");

                    string data = ProductJsonData.LuruEticket(proid, key, comid, leixing);
                    context.Response.Write(data);
                }

                if (oper == "pagelistbystate")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var prostate = context.Request["prostate"].ConvertTo<int>(1);
                    var projectid = context.Request["projectid"].ConvertTo<int>(0);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var viewmethod = context.Request["viewmethod"].ConvertTo<string>("");


                    string data = ProductJsonData.ProPageList(comid, pageindex, pagesize, prostate, projectid, key, viewmethod);

                    context.Response.Write(data);
                }
                if (oper == "Webpagelistbystate")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var prostate = context.Request["prostate"].ConvertTo<int>(1);
                    var projectid = context.Request["projectid"].ConvertTo<int>(0);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var viewmethod = context.Request["viewmethod"].ConvertTo<string>("");


                    string data = ProductJsonData.WebProPageList(comid, pageindex, pagesize, prostate, projectid, key, viewmethod);

                    context.Response.Write(data);
                }
                if (oper == "AdjustFee")
                {
                    var id = context.Request["id"];
                    var fee = context.Request["fee"].ConvertTo<decimal>(0);

                    string data = ProductJsonData.AdjustFee(id, fee);
                    context.Response.Write(data);
                }

                if (oper == "AdjustServiceFee")
                {
                    var id = context.Request["id"];
                    var ServiceFee = context.Request["ServiceFee"].ConvertTo<decimal>(0);

                    string data = ProductJsonData.AdjustServiceFee(id, ServiceFee);
                    context.Response.Write(data);
                }
                if (oper == "editroomtype")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    string name = context.Request["name"].ConvertTo<string>("");
                    string bedtype = context.Request["bedtype"].ConvertTo<string>("");
                    string bedwidth = context.Request["bedwidth"].ConvertTo<string>("");
                    bool whetherextrabed = context.Request["whetherextrabed"].ConvertTo<bool>(false);
                    decimal extrabedprice = context.Request["extrabedprice"].ConvertTo<decimal>(0);
                    int ReserveType = context.Request["ReserveType"].ConvertTo<int>(1);//默认前台现付
                    string wifi = context.Request["wifi"].ConvertTo<string>("");
                    int Breakfast = context.Request["Breakfast"].ConvertTo<int>(1);//默认无早
                    int server_type = context.Request["server_type"].ConvertTo<int>(1);//默认电子凭证
                    int pro_type = context.Request["pro_type"].ConvertTo<int>(1);//默认电子票
                    int source_type = context.Request["source_type"].ConvertTo<int>(1);//默认系统自动生成
                    string Sms = context.Request["Sms"].ConvertTo<string>("");
                    string Builtuparea = context.Request["Builtuparea"].ConvertTo<string>("");
                    string floor = context.Request["floor"].ConvertTo<string>("");
                    int largestguestnum = context.Request["largestguestnum"].ConvertTo<int>(1);//默认1人
                    bool whethernonsmoking = context.Request["whethernonsmoking"].ConvertTo<bool>(false);//默认不可以安排
                    string amenities = context.Request["amenities"].ConvertTo<string>("");
                    string Mediatechnology = context.Request["Mediatechnology"].ConvertTo<string>("");
                    string Foodanddrink = context.Request["Foodanddrink"].ConvertTo<string>("");
                    string ShowerRoom = context.Request["ShowerRoom"].ConvertTo<string>("");
                    int imgurl = context.Request["imgurl"].ConvertTo<int>(0);
                    bool whetheravailabel = context.Request["whetheravailabel"].ConvertTo<bool>(false);//默认下线
                    string roomtyperemark = context.Request["roomtyperemark"].ConvertTo<string>("");
                    int userid = context.Request["createruserid"].ConvertTo<int>();
                    int comid = context.Request["comid"].ConvertTo<int>();
                    string RecerceSMSName = context.Request["RecerceSMSName"].ConvertTo<string>("");
                    string RecerceSMSPhone = context.Request["RecerceSMSPhone"].ConvertTo<string>("");


                    B2b_com_roomtype roomtype = new B2b_com_roomtype
                    {
                        Id = id,
                        Name = name,
                        Bedtype = bedtype,
                        Wifi = wifi,
                        ReserveType = ReserveType,
                        Builtuparea = Builtuparea,
                        Floor = floor,
                        Bedwidth = bedwidth,
                        Whetherextrabed = whetherextrabed,
                        Extrabedprice = extrabedprice,
                        Largestguestnum = largestguestnum,
                        Whethernonsmoking = whethernonsmoking,
                        Amenities = amenities,
                        Mediatechnology = Mediatechnology,

                        Foodanddrink = Foodanddrink,
                        ShowerRoom = ShowerRoom,
                        Breakfast = Breakfast,
                        Sms = Sms,
                        Sortid = 10000,
                        Server_type = server_type,
                        Pro_type = pro_type,
                        Source_type = source_type,
                        Createuserid = userid,
                        Createtime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        Whetheravailabel = whetheravailabel,
                        Roomtyperemark = roomtyperemark,
                        Comid = comid,

                        Roomtypeimg = imgurl,
                        RecerceSMSName = RecerceSMSName,
                        RecerceSMSPhone = RecerceSMSPhone


                    };
                    DateTime prostart = context.Request["prostart"].ConvertTo<DateTime>();
                    DateTime proend = context.Request["proend"].ConvertTo<DateTime>();
                    string roomtypepara = context.Request["roomtypepara"].ConvertTo<string>("");


                    string data = ProductJsonData.InsertOrUpdatePro(roomtype, prostart, proend, roomtypepara);

                    context.Response.Write(data);
                }
                if (oper == "Hotelpagelist")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(10);

                    string startdate = context.Request["startdate"].ConvertTo<string>("");
                    string enddate = context.Request["enddate"].ConvertTo<string>("");

                    int proid = context.Request["proid"].ConvertTo<int>(0);
                    int projectid = context.Request["projectid"].ConvertTo<int>(0);
                    int Agentlevel = context.Request["Agentlevel"].ConvertTo<int>(0);

                    string data = ProductJsonData.GetRoomTypePagelist(comid, pageindex, pagesize, startdate, enddate, proid, projectid, Agentlevel);

                    context.Response.Write(data);
                }
                if (oper == "GetRoomType")
                {
                    int roomtypeid = context.Request["roomtypeid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = ProductJsonData.GetRoomType(roomtypeid, comid);
                    context.Response.Write(data);
                }
                if (oper == "GetHouseType")
                {
                    int proid = context.Request["proid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = ProductJsonData.GetHouseType(proid, comid);
                    context.Response.Write(data);
                }
                if (oper == "GetHouseTypeDayList")
                {
                    int proid = context.Request["proid"].ConvertTo<int>(0);
                    string startdate = context.Request["startdate"].ConvertTo<string>();
                    string enddate = context.Request["enddate"].ConvertTo<string>();

                    string data = ProductJsonData.GetHouseTypeDayList(proid, startdate, enddate);
                    context.Response.Write(data);
                }

                if (oper == "roomtypepagelist")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(0);

                    string data = ProductJsonData.GetRoomTypePagelist(comid, pageindex, pagesize);
                    context.Response.Write(data);
                }
                if (oper == "GetRoomTypeDayList")
                {
                    int roomtypeid = context.Request["roomtypeid"].ConvertTo<int>(0);

                    string data = ProductJsonData.GetRoomTypeDayList(roomtypeid);
                    context.Response.Write(data);
                }
                if (oper == "GetLineDayGroupDate")
                {
                    DateTime daydate = context.Request["daydate"].ConvertTo<DateTime>();
                    int lineid = context.Request["lineid"].ConvertTo<int>(0);

                    string data = ProductJsonData.GetLineDayGroupDate(daydate, lineid);
                    context.Response.Write(data);
                }
                if (oper == "Uplinegroupdate")
                {
                    string initdatestr = context.Request["initdatestr"].ConvertTo<string>("");
                    string datestr = context.Request["datestr"].ConvertTo<string>("");
                    string dayprice = context.Request["dayprice"].ConvertTo<string>("");
                    string emptynum = context.Request["emptynum"].ConvertTo<string>("");
                    int lineid = context.Request["lineid"].ConvertTo<int>();

                    string agent1_back = context.Request["agent1_back"].ConvertTo<string>("");
                    string agent2_back = context.Request["agent2_back"].ConvertTo<string>("");
                    string agent3_back = context.Request["agent3_back"].ConvertTo<string>("");

                    string data = ProductJsonData.Uplinegroupdate(lineid, initdatestr, datestr, dayprice, emptynum, agent1_back, agent2_back, agent3_back);
                    context.Response.Write(data);
                }
                if (oper == "DelLineGroupDate")
                {
                    string daydate = context.Request["daydate"].ConvertTo<string>("");
                    int lineid = context.Request["lineid"].ConvertTo<int>();

                    string data = ProductJsonData.DelLineGroupDate(lineid, daydate);
                    context.Response.Write(data);
                }
                if (oper == "AdjustHasInnerChannel")
                {
                    int companyid = context.Request["companyid"].ConvertTo<int>(0);
                    string hasinnerchannel = context.Request["hasinnerchannel"].ConvertTo<string>("false");

                    string data = ProductJsonData.AdjustHasInnerChannel(companyid, hasinnerchannel);
                    context.Response.Write(data);
                }
                //产品分类列表
                if (oper == "proclasslist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0); ;
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var industryid = context.Request["industryid"].ConvertTo<int>(0);
                    string data = ProductJsonData.Proclasslist(pageindex, pagesize, 0, industryid);

                    context.Response.Write(data);
                }
                //产品获取单个分类
                if (oper == "proclassbyid")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0); ;
                    var classid = context.Request["classid"].ConvertTo<int>(0);
                    string data = ProductJsonData.Proclassbyid(classid);
                    context.Response.Write(data);
                }
                //删除单个分类了
                if (oper == "proclassdel")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0); ;
                    var classid = context.Request["classid"].ConvertTo<int>(0);
                    string data = ProductJsonData.Proclassdel(classid);
                    context.Response.Write(data);
                }
                //产品管理
                if (oper == "proclassmanage")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var classid = context.Request["classid"].ConvertTo<int>(0);
                    var Classname = context.Request["Classname"].ConvertTo<string>("");
                    var industryid = context.Request["industryid"].ConvertTo<int>(0);


                    string data = ProductJsonData.Proclassmanage(classid, Classname, industryid);
                    context.Response.Write(data);
                }
                if (oper == "editproject")
                {
                    int projectid = context.Request["projectid"].ConvertTo<int>(0);
                    string projectname = context.Request["projectname"].ConvertTo<string>("");
                    int img = context.Request["img"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string province = context.Request["province"].ConvertTo<string>("");
                    string city = context.Request["city"].ConvertTo<string>("");
                    int com_type = context.Request["com_type"].ConvertTo<int>(0);
                    string briefintroduce = context.Request["briefintroduce"].ConvertTo<string>("");
                    string address = context.Request["address"].ConvertTo<string>("");
                    string mobile = context.Request["mobile"].ConvertTo<string>("");
                    string coordinate = context.Request["coordinate"].ConvertTo<string>("");
                    string serviceintroduce = context.Request["serviceintroduce"].ConvertTo<string>("");
                    string onlinestate = context.Request["onlinestate"].ConvertTo<string>("0");
                    int userid = context.Request["userid"].ConvertTo<int>(0);


                    int hotelset = context.Request["hotelset"].ConvertTo<int>(0);
                    decimal grade = context.Request["grade"].ConvertTo<decimal>(0);
                    string star = context.Request["star"].ConvertTo<string>("");
                    int parking = context.Request["parking"].ConvertTo<int>(0);
                    string cu = context.Request["cu"].ConvertTo<string>("");




                    DateTime createtime = DateTime.Now;

                    B2b_com_project model = new B2b_com_project
                    {
                        Id = projectid,
                        Projectname = projectname,
                        Projectimg = img,
                        Province = province,
                        City = city,
                        Industryid = com_type,
                        Briefintroduce = briefintroduce,
                        Address = address,
                        Mobile = mobile,
                        Coordinate = coordinate,
                        Serviceintroduce = serviceintroduce,
                        Onlinestate = onlinestate,
                        Comid = comid,
                        Createuserid = userid,
                        Createtime = createtime,
                        hotelset = hotelset,
                        grade = grade,
                        star = star,
                        parking = parking,
                        cu = cu
                    };

                    string data = ProductJsonData.EditProject(model);
                    context.Response.Write(data);
                }
                if (oper == "getproject")
                {
                    int projectid = context.Request["id"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = ProductJsonData.GetProject(projectid, comid);
                    context.Response.Write(data);
                }
                if (oper == "projectpagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var projectstate = context.Request["projectstate"].ConvertTo<string>("0,1");
                    string key = context.Request["key"].ConvertTo<string>("");
                    int projectid = context.Request["projectid"].ConvertTo<int>(0);
                    int Servertype = context.Request["Servertype"].ConvertTo<int>(0);
                    int runpro = context.Request["runpro"].ConvertTo<int>(0);

                    string data = ProductJsonData.Projectpagelist(comid, pageindex, pagesize, projectstate, key, runpro, projectid, Servertype);

                    context.Response.Write(data);
                }

                if (oper == "webprojectpagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var projectstate = context.Request["projectstate"].ConvertTo<string>("0,1");
                    string key = context.Request["key"].ConvertTo<string>("");
                    int projectid = context.Request["projectid"].ConvertTo<int>(0);
                    int Servertype = context.Request["Servertype"].ConvertTo<int>(0);
                    int runpro = context.Request["runpro"].ConvertTo<int>(0);

                    string data = ProductJsonData.WebProjectpagelist(comid, pageindex, pagesize, projectstate, key, runpro, projectid, Servertype);

                    context.Response.Write(data);
                }

                if (oper == "projectpageuserlist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var projectstate = context.Request["projectstate"].ConvertTo<string>("0,1");
                    string key = context.Request["key"].ConvertTo<string>("");

                    string data = ProductJsonData.Projectpageuserlist(comid, pageindex, pagesize, projectstate, key);

                    context.Response.Write(data);
                }


                if (oper == "projectlist")
                {
                    var comid = context.Request["comid"];

                    var projectstate = context.Request["projectstate"].ConvertTo<string>("0,1");
                    var prosort = context.Request["prosort"].ConvertTo<int>(0);

                    string data = ProductJsonData.Projectlist(comid, projectstate, prosort);

                    context.Response.Write(data);
                }
                if (oper == "GetLineTripById")
                {
                    int productid = context.Request["productid"].ConvertTo<int>(0);
                    int tripid = context.Request["tripid"].ConvertTo<int>(0);

                    string data = ProductJsonData.GetLineTripById(tripid, productid);
                    //string data = "{\"Id\":\"0\",\"ActivityArea\":\"null\",\"Description\":\"null\",\"Traffic\":\"null\",\"ScenicActivity\":\"null\",\"Hotel\":\"null\",\"Dining\":\"null\",\"ProductId\":\"0\",\"Creator\":\"0\",\"CreateDate\":\"2014-02-14\"}";
                    context.Response.Write(data);
                }
                if (oper == "editlinetrip")
                {
                    string saveInfor = context.Request["saveInfor"].ConvertTo<string>("");
                    int productId = context.Request["productId"].ConvertTo<int>(0);
                    int creator = context.Request["providerId"].ConvertTo<int>(0);


                    string data = ProductJsonData.Edittrip(saveInfor, productId, creator);

                    context.Response.Write(data);
                }
                if (oper == "Neweditlinetrip")
                {
                    int tripid = context.Request["tripid"].ConvertTo<int>(0);
                    int ProductId = context.Request["ProductId"].ConvertTo<int>(0);
                    string ActivityArea = context.Request["ActivityArea"].ConvertTo<string>("");
                    string Traffic = context.Request["Traffic"].ConvertTo<string>("");
                    string ScenicActivity = context.Request["ScenicActivity"].ConvertTo<string>("");
                    string textarea = context.Request["textarea"].ConvertTo<string>("");
                    string Hotel = context.Request["Hotel"].ConvertTo<string>("");
                    string diningInfor = context.Request["diningInfor"].ConvertTo<string>("");
                    int creator = context.Request["providerId"].ConvertTo<int>(0);
                    B2b_com_protrip trip = new B2b_com_protrip
                    {
                        Id = tripid,
                        Productid = ProductId,
                        ActivityArea = ActivityArea,
                        Traffic = Traffic,
                        ScenicActivity = ScenicActivity,
                        Description = textarea,
                        Hotel = Hotel,
                        Dining = diningInfor,
                        CreateDate = DateTime.Now,
                        Creator = creator
                    };

                    string data = ProductJsonData.Edittrip(trip);

                    context.Response.Write(data);
                }
                if (oper == "deletelinetrip")
                {
                    int tripid = context.Request["tripid"].ConvertTo<int>(0);
                    int ProductId = context.Request["ProductId"].ConvertTo<int>(0);


                    string data = ProductJsonData.DeleteLineTrip(tripid, ProductId);

                    context.Response.Write(data);
                }
                if (oper == "Selectguigelist")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int proid = context.Request["proid"].ConvertTo<int>(0);


                    string data = ProductJsonData.Selectguigelist(comid, proid);

                    context.Response.Write(data);
                }
                if (oper == "SelectServerlist")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int proid = context.Request["proid"].ConvertTo<int>(0);
                    string pno = context.Request["pno"].ConvertTo<string>("");
                    if (pno != "")
                    {
                        pno = EncryptionHelper.EticketPnoDES(pno, 1);//解密
                    }
                    string data = ProductJsonData.SelectServerlist(comid, proid, 0, pno);

                    context.Response.Write(data);
                }



                if (oper == "edithousetype")
                {
                    int proid = context.Request["proid"].ConvertTo<int>(0);
                    //string name = context.Request["name"].ConvertTo<string>("");
                    string bedtype = context.Request["bedtype"].ConvertTo<string>("");
                    string bedwidth = context.Request["bedwidth"].ConvertTo<string>("");
                    bool whetherextrabed = context.Request["whetherextrabed"].ConvertTo<bool>(false);
                    decimal extrabedprice = context.Request["extrabedprice"].ConvertTo<decimal>(0);
                    int ReserveType = context.Request["ReserveType"].ConvertTo<int>(1);//默认前台现付
                    string wifi = context.Request["wifi"].ConvertTo<string>("");
                    int Breakfast = context.Request["Breakfast"].ConvertTo<int>(1);//默认无早
                    //int server_type = context.Request["server_type"].ConvertTo<int>(1);//默认电子凭证
                    //int pro_type = context.Request["pro_type"].ConvertTo<int>(1);//默认电子票
                    //int source_type = context.Request["source_type"].ConvertTo<int>(1);//默认系统自动生成
                    //string Sms = context.Request["Sms"].ConvertTo<string>("");
                    string Builtuparea = context.Request["Builtuparea"].ConvertTo<string>("");
                    string floor = context.Request["floor"].ConvertTo<string>("");
                    int largestguestnum = context.Request["largestguestnum"].ConvertTo<int>(1);//默认1人
                    bool whethernonsmoking = context.Request["whethernonsmoking"].ConvertTo<bool>(false);//默认不可以安排
                    string amenities = context.Request["amenities"].ConvertTo<string>("");
                    string Mediatechnology = context.Request["Mediatechnology"].ConvertTo<string>("");
                    string Foodanddrink = context.Request["Foodanddrink"].ConvertTo<string>("");
                    string ShowerRoom = context.Request["ShowerRoom"].ConvertTo<string>("");
                    //int imgurl = context.Request["imgurl"].ConvertTo<int>(0);
                    //bool whetheravailabel = context.Request["whetheravailabel"].ConvertTo<bool>(false);//默认下线
                    string roomtyperemark = context.Request["roomtyperemark"].ConvertTo<string>("");
                    //int userid = context.Request["createruserid"].ConvertTo<int>();
                    int comid = context.Request["comid"].ConvertTo<int>();
                    string RecerceSMSName = context.Request["RecerceSMSName"].ConvertTo<string>("");
                    string RecerceSMSPhone = context.Request["RecerceSMSPhone"].ConvertTo<string>("");


                    B2b_com_housetype housetype = new B2b_com_housetype
                    {

                        Proid = proid,
                        Bedtype = bedtype,
                        Wifi = wifi,
                        ReserveType = ReserveType,
                        Builtuparea = Builtuparea,
                        Floor = floor,
                        Bedwidth = bedwidth,
                        Whetherextrabed = whetherextrabed,
                        Extrabedprice = extrabedprice,
                        Largestguestnum = largestguestnum,
                        Whethernonsmoking = whethernonsmoking,
                        Amenities = amenities,
                        Mediatechnology = Mediatechnology,

                        Foodanddrink = Foodanddrink,
                        ShowerRoom = ShowerRoom,
                        Breakfast = Breakfast,

                        Roomtyperemark = roomtyperemark,
                        Comid = comid,

                        RecerceSMSName = RecerceSMSName,
                        RecerceSMSPhone = RecerceSMSPhone


                    };

                    string data = ProductJsonData.InsertOrUpdateHouseType(housetype);

                    context.Response.Write(data);
                }

                if (oper == "delbusfeeticket")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);


                    string data = ProductJsonData.delbusfeeticket(id, comid);

                    context.Response.Write(data);
                }
                if (oper == "busfeeticketpagelist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);


                    string data = ProductJsonData.busfeeticketpagelist(comid, pageindex, pagesize);

                    context.Response.Write(data);

                }

                if (oper == "upbusfeeticket")
                {
                    var businfo = new Bus_Feeticket();
                    businfo.Id = context.Request["id"].ConvertTo<int>(0);
                    businfo.Title = context.Request["Title"].ConvertTo<string>("");
                    businfo.Feeday = context.Request["Feeday"].ConvertTo<int>(0);
                    businfo.Comid = context.Request["Comid"].ConvertTo<int>(0);
                    businfo.Startime = context.Request["Startime"].ConvertTo<DateTime>(DateTime.Now);//暂时不用，按票的有效期
                    businfo.Endtime = context.Request["Endtime"].ConvertTo<DateTime>(DateTime.Now);//暂时不用，按票的有效期
                    businfo.Iuse = context.Request["Iuse"].ConvertTo<int>(0);

                    string data = ProductJsonData.upbusfeeticket(businfo);

                    context.Response.Write(data);

                }

                if (oper == "getbusfeeticketbyid")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);
                    string data = ProductJsonData.GetBus_FeeticketById(id, comid);
                    context.Response.Write(data);
                }
                if (oper == "busbindingpro")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var busid = context.Request["busid"].ConvertTo<int>(0);//
                    var proid = context.Request["proid"].ConvertTo<int>(0); //绑定产品产品
                    var type = context.Request["type"].ConvertTo<int>(0);//类型 1=使用产品，0=使用码
                    var subtype = context.Request["subtype"].ConvertTo<int>(1);//操作方式 1=绑定，0取消绑定

                    var limitweek = context.Request["limitweek"].ConvertTo<int>(0);
                    var limitweekdaynum = context.Request["limitweekdaynum"].ConvertTo<int>(0);
                    var limitweekendnum = context.Request["limitweekendnum"].ConvertTo<int>(0);

                    string data = ProductJsonData.Busbindingpro(busid, comid, proid, type, subtype, limitweek, limitweekdaynum, limitweekendnum);

                    context.Response.Write(data);
                }
                if (oper == "busfeeticketbindingpropagelist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);

                    var busid = context.Request["busid"].ConvertTo<int>(10);
                    var bindingprotype = context.Request["bindingprotype"].ConvertTo<int>(0);



                    string data = ProductJsonData.busfeeticketbindingpropagelist(pageindex, pagesize, busid, bindingprotype, comid);
                    context.Response.Write(data);
                }

                if (oper == "upRentserver")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);//
                    var servername = context.Request["servername"].ConvertTo<string>(""); //服务名称
                    var renttype = context.Request["renttype"].ConvertTo<string>(""); //服务类别
                    var WR = context.Request["WR"].ConvertTo<int>(1);//是否归还 0=不需要归还 1= 需要归还
                    var posid = context.Request["posid"].ConvertTo<int>(0);//POS
                    var num = context.Request["num"].ConvertTo<int>(1);//数量
                    var saleprice = context.Request["saleprice"].ConvertTo<decimal>(0);//销售价
                    var serverDepositprice = context.Request["serverDepositprice"].ConvertTo<decimal>(0);//押金
                    var mustselect = context.Request["mustselect"].ConvertTo<int>(0);//是否为必选产品
                    var servertype = context.Request["servertype"].ConvertTo<int>(0);//默认为验证服务，1=退款流程
                    var printticket = context.Request["printticket"].ConvertTo<int>(0);//是否打印索道票
                    var Fserver = context.Request["Fserver"].ConvertTo<int>(0);//父服务
                    var Sonserver = context.Request["Sonserver"].ConvertTo<int>(0);//选择为子服务

                    if (Sonserver == 0)
                    {
                        Fserver = 0;//如果未选择子服务，则服务绑定ID为0
                    }


                    string data = ProductJsonData.upRentserver(id, comid, servername, WR, num, posid, saleprice, serverDepositprice, renttype, mustselect, servertype, printticket, Fserver);

                    context.Response.Write(data);
                }

                if (oper == "delRentserver")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);//

                    string data = ProductJsonData.delRentserver(id, comid);

                    context.Response.Write(data);
                }


                if (oper == "Rentserverbyid")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);


                    string data = ProductJsonData.Rentserverbyid(id, comid);

                    context.Response.Write(data);
                }
                if (oper == "Rentserverpagelist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var proid = context.Request["proid"].ConvertTo<int>(0);

                    string data = ProductJsonData.Rentserverpagelist(pageindex, pagesize, comid, proid);

                    context.Response.Write(data);
                }
                if (oper == "uppro_worktime")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var title = context.Request["title"].ConvertTo<string>("");
                    var defaultendtime = context.Request["defaultendtime"].ConvertTo<string>("");
                    var defaultstartime = context.Request["defaultstartime"].ConvertTo<string>("");

                    string data = ProductJsonData.uppro_worktime(id, comid, title, defaultendtime, defaultstartime);

                    context.Response.Write(data);
                }
                if (oper == "pro_worktimepagelist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var proid = context.Request["proid"].ConvertTo<int>(0);

                    string data = ProductJsonData.pro_worktimepagelist(pageindex, pagesize, comid, proid);

                    context.Response.Write(data);
                }
                if (oper == "pro_worktimebyid")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);


                    string data = ProductJsonData.pro_worktimebyid(id, comid);

                    context.Response.Write(data);
                }
                if (oper == "delpro_worktime")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);//

                    string data = ProductJsonData.delpro_worktime(id, comid);

                    context.Response.Write(data);
                }
                //获取产品日历结算价
                if (oper == "procostrilipagelist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0); ;
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var proid = context.Request["proid"].ConvertTo<int>(0);

                    string data = ProductJsonData.procostrilipagelist(pageindex, pagesize, comid, proid);

                    context.Response.Write(data);
                }
                //修改产品日历结算价
                if (oper == "upprocostrili")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0); ;
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var costprice = context.Request["costprice"].ConvertTo<decimal>(0);
                    var stardate = context.Request["stardate"].ConvertTo<string>("");
                    var enddate = context.Request["enddate"].ConvertTo<string>("");
                    var proid = context.Request["proid"].ConvertTo<int>(0);
                    var admin = "";

                    string data = ProductJsonData.upprocostrili(comid, id, proid, costprice, stardate, enddate, admin);
                    context.Response.Write(data);
                }
                //获取单个产品日历结算价
                if (oper == "procostrilibyid")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0); ;
                    var id = context.Request["id"].ConvertTo<int>(0);
                    string data = ProductJsonData.procostrilibyid(comid, id);
                    context.Response.Write(data);
                }
                //删除产品日历结算价
                if (oper == "delcostrili")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0); ;
                    var id = context.Request["id"].ConvertTo<int>(0);
                    string data = ProductJsonData.delcostrili(comid, id);
                    context.Response.Write(data);
                }

                //获取项目结算金额
                if (oper == "projectfinancepagelist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0); ;
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var projectid = context.Request["projectid"].ConvertTo<int>(0);
                    var startime = context.Request["startime"].ConvertTo<string>("");
                    var endtime = context.Request["endtime"].ConvertTo<string>("");

                    string data = ProductJsonData.projectfinancepagelist(pageindex, pagesize, comid, projectid, startime, endtime);

                    context.Response.Write(data);
                }
                //添加项目结算
                if (oper == "upprojectfinance")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0); ;
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var Money = context.Request["Money"].ConvertTo<decimal>(0);
                    var Remarks = context.Request["Remarks"].ConvertTo<string>("");
                    var Projectid = context.Request["Projectid"].ConvertTo<int>(0);
                    var admin = "";

                    var accountid = 0;
                    if (context.Request.Cookies["AccountId"] != null)
                    {
                        accountid = context.Request.Cookies["AccountId"].Value.ConvertTo<int>(0);
                    }
                    var openid = "";
                    var b2bdate = new B2bCrmData();
                    if (accountid != 0)
                    {
                        var b2bmodel = b2bdate.Readuser(accountid, comid);
                        if (b2bmodel != null)
                        {
                            admin = b2bmodel.Name;
                        }
                    }



                    string data = ProductJsonData.upprojectfinance(comid, id, Projectid, Money, Remarks, admin);
                    context.Response.Write(data);
                }
                //获取结算统计值
                if (oper == "projectfinancesum")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0); ;
                    var projectid = context.Request["projectid"].ConvertTo<int>(0);
                    var startime = context.Request["startime"].ConvertTo<string>("");
                    var endtime = context.Request["endtime"].ConvertTo<string>("");
                    string data = ProductJsonData.projectfinancesum(comid, projectid, startime, endtime);
                    context.Response.Write(data);
                }

                //获取结算统计值
                if (oper == "DelPackageid")
                {
                    var id = context.Request["id"].ConvertTo<int>(0); ;
                    var comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = ProductJsonData.DelProPackagbyid(id, comid);
                    context.Response.Write(data);
                }

                //获取结算统计值
                if (oper == "ProPackagelist")
                {
                    var pid = context.Request["pid"].ConvertTo<int>(0); ;
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);

                    string data = ProductJsonData.GetProPackagPagelistByid(pid, pageindex, pagesize);
                    context.Response.Write(data);
                }
                if (oper == "GetProPackagbyid")
                {
                    var id = context.Request["id"].ConvertTo<int>(0); ;
                    var comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = ProductJsonData.GetProPackagbyid(id);
                    context.Response.Write(data);
                }

                if (oper == "ProPackageup")
                {
                    var id = context.Request["id"].ConvertTo<int>(0); ;
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var sid = context.Request["sid"].ConvertTo<int>(0);
                    var fid = context.Request["fid"].ConvertTo<int>(0);
                    var snum = context.Request["snum"].ConvertTo<int>(1);

                    B2b_com_pro_Package product = null;
                    product.Id = id;
                    product.Fid = fid;
                    product.Sid = sid;
                    product.Snum = snum;


                    string data = ProductJsonData.ProPackageInsertOrUpdate(product);
                    context.Response.Write(data);
                }
                if (oper == "CompanyProbandignzhaji")
                {
                    var proid = context.Request["proid"].ConvertTo<int>(0); ;
                    var comid = context.Request["comid"].ConvertTo<int>(0);

                    string data = ProductJsonData.CompanyProbandignzhaji(comid,proid);
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
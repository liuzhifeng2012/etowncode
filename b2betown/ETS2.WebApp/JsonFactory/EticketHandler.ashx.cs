using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using System.Web.SessionState;
using ETS.JsonFactory;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Permision.Service.PermisionService.Model;
using ETS2.Permision.Service.PermisionService.Data;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle.Enum;

namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// EticketHandler 的摘要说明
    /// </summary>
    public class EticketHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string oper = context.Request["oper"].ConvertTo<string>("");
            if (oper != "")
            { 
                if (oper == "ValidateEticket")
                {
                    var pno = context.Request["pno"].ToString();

                    if (pno != null && pno != "")
                    {
                        pno = EncryptionHelper.EticketPnoDES(pno, 1);//对码进行解密
                    }


                    var comid = context.Request["comid"];


                    string data = "";//获取电子票详细信息


                    int isget = 0;//判断获取详情是否成功
                    data = EticketJsonData.GetEticketDetail(pno, comid, out  isget);

                    if (isget == 1)//如果获取成功，附session值，防止确认使用操作后重复刷新页面造成电子票重复打印的操作
                    {
                        context.Session[pno] = "1";
                    }
                    else
                    {
                        context.Session[pno] = "0";
                    }

                    context.Response.Write(data);
                }
                if (oper == "getagentsearchlist")
                {
                    var pno = context.Request["key"].ConvertTo<string>("");
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var agentid = context.Request["agentid"].ConvertTo<int>(0);

                    var data = EticketJsonData.GetEticketSearch(pno, comid, agentid);
                    context.Response.Write(data);
                }

                if (oper == "getagentbacklist")
                {
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var agentid = context.Request["agentid"].ConvertTo<int>(0);

                    var data = EticketJsonData.GetBackEticketlist(comid, agentid, pageindex, pagesize);
                    context.Response.Write(data);
                }

                //退票
                if (oper == "backagentticket")
                {
                    var pno = context.Request["pno"].ConvertTo<string>("");
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var agentid = context.Request["agentid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);

                    var data = EticketJsonData.BackAgentEticket_interface(id, pno);
                    context.Response.Write(data);
                }



                if (oper == "GetPnoDetail")//得到电子票详细信息（包括厂家信息，产品信息，电子票信息）
                {
                    var pno = context.Request["pno"];

                    string data = EticketJsonData.GetPnoDetail(pno);

                    context.Response.Write(data);
                }
                if (oper == "getedetail")//得到此次后台验证票的详细情况
                {
                    var pno = context.Request["pno"];
                    if (pno != null && pno != "")
                    {
                        pno = EncryptionHelper.EticketPnoDES(pno, 1);//对码进行解密
                    }
                    var comid = context.Request["comid"];
                    var validateticketlogid = context.Request["validateticketlogid"];

                    string data = "";//获取电子票详细信息


                    data = EticketJsonData.GetEticketDetailNotValidate(pno, comid, validateticketlogid);

                    context.Response.Write(data);
                }
                if (oper == "econfirm")
                {
                    var pno = context.Request["pno"];
                    if (pno != null && pno != "")
                    {
                        pno = EncryptionHelper.EticketPnoDES(pno, 1);//对码进行解密
                    }


                    var usenum = context.Request["usenum"];
                    var comid = context.Request["comid"];

                    if (context.Session[pno].ToString() == "1")//防止点击后退键重复使用
                    {
                        //获取PC验证登陆账户
                        B2b_company_manageuser user = UserHelper.CurrentUser();
                        var username = user.Accounts;

                        //得到登录用户角色 验证电子票的类型
                        int validateservertype = 0;

                        Sys_Group mgroup = new Sys_GroupData().GetGroupByUserId(user.Id);
                        if (mgroup != null)
                        {
                            validateservertype = mgroup.validateservertype;
                        }
                        if (validateservertype > 0)
                        {
                            //根据电子码 得到产品的服务类型，如果和用户角色所能验证的服务类型不符，则不可验证电子票
                            int pnoservertype = new B2bComProData().GetServertypeByPno(pno);
                            if (pnoservertype == 0)
                            {
                                context.Response.Write("{\"type\":1,\"msg\":\"根据电子码得到产品服务类型失败\"}");
                                return;
                            }
                            else
                            {
                                if (pnoservertype != validateservertype)
                                {
                                    context.Response.Write("{\"type\":1,\"msg\":\"登录用户角色只可以验证" + EnumUtils.GetName((ProductServer_Type)validateservertype) + "\"}");
                                    return;
                                }
                            }
                        }

                        string data = EticketJsonData.EConfirm(pno, usenum, comid, 999999999, "", username);//返回数据
                        context.Session[pno] = "0";
                        context.Response.Write(data);
                    }
                    else
                    {

                        context.Response.Redirect("/ui/pmui/eticket/eticketindex.aspx");

                    }



                }


                if (oper == "zizhueconfirm")
                {
                    var pno = context.Request["pno"];
                    if (pno != null && pno != "")
                    {
                        pno = EncryptionHelper.EticketPnoDES(pno, 1);//对码进行解密
                    }

                    var usenum = context.Request["usenum"];
                    var comid = context.Request["comid"];

                    string data = EticketJsonData.EConfirm(pno, usenum, comid, 999999999, "", "自助验证");//返回数据
                    context.Response.Write(data);
                }


                if (oper == "pagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var eclass = context.Request["eclass"].ConvertTo<int>(0);
                    var proid = context.Request["proid"].ConvertTo<string>("0");
                    var jsid = context.Request["jsid"].ConvertTo<string>("0");
                    var posid = context.Request["posid"].ConvertTo<int>(0);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var startime = context.Request["startime"].ConvertTo<string>("");
                    var endtime = context.Request["endtime"].ConvertTo<string>("");
                    var projectid = context.Request["projectid"].ConvertTo<int>(0);
                    var saleagentid = context.Request["agentid"].ConvertTo<int>(0);


                    string data = EticketJsonData.EPageList(comid, pageindex, pagesize, eclass, int.Parse(proid), int.Parse(jsid), posid, key, startime, endtime, 0, projectid, saleagentid);

                    context.Response.Write(data);
                }

                if (oper == "interfaceuselogpagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");
                    var startime = context.Request["startime"].ConvertTo<string>("");
                    var endtime = context.Request["endtime"].ConvertTo<string>("");



                    string data = EticketJsonData.InterfaceUsePageList(comid, pageindex, pagesize, key, startime, endtime);

                    context.Response.Write(data);
                }

                if (oper == "dayjslist")
                {
                    var comid = context.Request["comid"];


                    string data = EticketJsonData.DayJSList(comid);

                    context.Response.Write(data);
                }
                if (oper == "dayjsresult")
                {
                    var comid = context.Request["comid"];
                    var jsid = context.Request["jsid"];


                    string data = EticketJsonData.DayJSResult(comid, jsid);

                    context.Response.Write(data);
                }
                if (oper == "GetPnoConsumeLogList")
                {
                    string pno = context.Request["pno"];

                    string data = EticketJsonData.GetPnoConsumeLogList(pno);
                    context.Response.Write(data);
                }

                if (oper == "agentEticketlog")
                {
                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string key = context.Request["key"].ConvertTo<string>("");
                    string startime = context.Request["startime"].ConvertTo<string>("");
                    string endtime = context.Request["endtime"].ConvertTo<string>("");

                    var data = EticketJsonData.AgentEticketlog(comid, agentid, Pageindex, Pagesize, key, startime, endtime);
                    context.Response.Write(data);
                }
                //安订单，已验票列表
                if (oper == "VagentEticketlog")
                {
                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    int orderid = context.Request["orderid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    var data = EticketJsonData.VAgentEticketlog(comid, agentid, orderid, Pageindex, Pagesize);
                    context.Response.Write(data);
                }

                //安全码
                if (oper == "eticketsafety")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    DateTime startime = context.Request["startime"].ConvertTo<DateTime>(DateTime.Now);

                    var data = EticketJsonData.GetComDayRandomlist(comid, startime);
                    context.Response.Write(data);
                }
                //生成安全码
                if (oper == "createeticketsafety")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    DateTime startime = context.Request["startime"].ConvertTo<DateTime>(DateTime.Now);

                    var data = EticketJsonData.CreateComDayRandom(comid, startime);
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
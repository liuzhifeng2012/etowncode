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
using ETS2.PM.Service.PMService.Data;
using ETS2.CRM.Service.CRMService.Modle.Enum;


namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// AgentHandler 的摘要说明
    /// </summary>
    public class AgentHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string oper = context.Request["oper"].ConvertTo<string>("");

            if (oper != "")
            {
                if (oper=="getmeituanagentcompanylist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);

                    var data = AgentJosnData.GetMeituanAgentCompanyList(comid);
                    context.Response.Write(data);
                }
                if (oper == "editagentoutinterface")
                {
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    string agent_updateurl = context.Request["agent_updateurl"].ConvertTo<string>("");
                    string txtagentip = context.Request["txtagentip"].ConvertTo<string>("");
                    string inter_sendmethod = context.Request["inter_sendmethod"].ConvertTo<string>("post");

                    var data = AgentJosnData.Editagentoutinterface(agentid, agent_updateurl.Trim(), txtagentip.Trim(), inter_sendmethod.Trim());
                    context.Response.Write(data);
                }
                if (oper == "getclientlistbyagentid")
                {
                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    string Key = context.Request["Key"].ConvertTo<string>("");
                    int Agentid = context.Request["Agentid"].ConvertTo<int>(0);

                    var data = AgentJosnData.Getclientlistbyagentid(Pageindex, Pagesize, Key, Agentid);
                    context.Response.Write(data);
                }
                if (oper == "GetTb_agent_relationList")
                {
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);

                    var data = AgentJosnData.GetTb_agent_relationList(agentid);

                    context.Response.Write(data);
                }
                if (oper == "GetTb_agent_relation")
                {
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    int serialnum = context.Request["serialnum"].ConvertTo<int>(0);
                    var data = AgentJosnData.GetTb_agent_relation(agentid, serialnum);

                    context.Response.Write(data);
                }
                if (oper == "EditpartTb_agent_relation")
                {
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    int serialnum = context.Request["serialnum"].ConvertTo<int>(0);
                    string tb_id = context.Request["tb_id"].ConvertTo<string>("");

                    string tb_seller_wangwang = context.Request["tb_seller_wangwang"].ConvertTo<string>("");
                    string tb_shop_name = context.Request["tb_shop_name"].ConvertTo<string>("");
                    string tb_shop_url = context.Request["tb_shop_url"].ConvertTo<string>("");
                    int tb_shop_state = context.Request["tb_shop_state"].ConvertTo<int>(0);

                    Taobao_agent_relation r = new Taobao_agent_relation
                    {
                        serialnum = serialnum,
                        tb_id = tb_id,
                        tb_seller_wangwang = tb_seller_wangwang,
                      
                        tb_shop_name = tb_shop_name,
                        tb_shop_state = tb_shop_state,
                        tb_shop_url = tb_shop_url,
                        agentid = agentid
                    };

                    var data = AgentJosnData.EditpartTb_agent_relation(r);

                    context.Response.Write(data);
                }
                if (oper == "EditTb_agent_relation")
                {
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    int serialnum = context.Request["serialnum"].ConvertTo<int>(0);
                    string tb_id = context.Request["tb_id"].ConvertTo<string>("");
                    string tb_seller_wangwangid = context.Request["tb_seller_wangwangid"].ConvertTo<string>("");
                  
                    string tb_seller_wangwang = context.Request["tb_seller_wangwang"].ConvertTo<string>("");
                    string tb_shop_name = context.Request["tb_shop_name"].ConvertTo<string>("");
                    string tb_shop_url = context.Request["tb_shop_url"].ConvertTo<string>("");
                    int tb_shop_state = context.Request["tb_shop_state"].ConvertTo<int>(0);

                    Taobao_agent_relation r = new Taobao_agent_relation
                    {
                        serialnum = serialnum,
                        tb_id = tb_id,
                        tb_seller_wangwangid = tb_seller_wangwangid,
                        tb_seller_wangwang = tb_seller_wangwang,
                        tb_shop_name = tb_shop_name,
                        tb_shop_state = tb_shop_state,
                        tb_shop_url = tb_shop_url,
                        agentid = agentid
                    };

                   
                    var data = AgentJosnData.EditTb_agent_relation(r);

                    context.Response.Write(data);
                }
                //更改分销商户手机
                if (oper == "AgentUpPhone")
                {
                    var agentid = context.Request["agentid"].ConvertTo<int>(0);
                    var newphone = context.Request["newphone"].ConvertTo<string>("");
                    var data = AgentJosnData.AgentUpPhone(agentid, newphone);

                    context.Response.Write(data);
                }
                //更改分销商员工手机
                if (oper == "AgentUserUpPhone")
                {
                    var agentuserid = context.Request["agentuserid"].ConvertTo<int>(0);
                    var newphone = context.Request["newphone"].ConvertTo<string>("");
                    var data = AgentJosnData.AgentUserUpPhone(agentuserid, newphone);

                    context.Response.Write(data);
                }

                //判断验证码是否正确
                if (oper == "judgevalidsms")
                {
                    var mobile = context.Request["mobile"].ConvertTo<string>("");
                    var smscode = context.Request["smscode"].ConvertTo<string>("");
                    var source = context.Request["source"].ConvertTo<string>("");

                    int result = 1;//返回处理结果：1出错；100成功
                    var data = AgentJosnData.Judgevalidsms(mobile, smscode, source, out result);
                    //手机动态密码登录
                    if (result == 100 && source == "手机动态密码")
                    {
                        //分销手机动态密码登录用到，其他请求不用传入值
                        string Email = context.Request["Account"].ConvertTo<string>("");

                        int agentid = new Agent_companyData().GetAgentIdByAccount(Email);

                        if (agentid > 0)
                        {
                            context.Session["Agentid"] = agentid;
                            context.Session["Account"] = Email;

                             
                            HttpCookie cookie = new HttpCookie("Agentid");     //实例化HttpCookie类并添加值
                            cookie.Value = agentid.ToString();
                            cookie.Expires = DateTime.Now.AddDays(1);
                            context.Response.Cookies.Add(cookie);

                            cookie = new HttpCookie("Account");     //实例化HttpCookie类并添加值
                            cookie.Value = Email;
                            cookie.Expires = DateTime.Now.AddDays(1);
                            context.Response.Cookies.Add(cookie);

                            var returnmd5 = EncryptionHelper.ToMD5(Email + "lixh1210" + agentid.ToString(), "UTF-8");
                            cookie = new HttpCookie("AgentKey");     //实例化HttpCookie类并添加值
                            cookie.Value = returnmd5;
                            cookie.Expires = DateTime.Now.AddDays(1);
                            context.Response.Cookies.Add(cookie);

                        }
                        else
                        {
                            data = "{\"type\":1,\"msg\":\"该账户状态异常!\"}";
                        }

                    }
                    context.Response.Write(data);
                }
                //判断验证码是否正确
                if (oper == "judgevalidsms_regi")
                {
                    var mobile = context.Request["mobile"].ConvertTo<string>("");
                    var smscode = context.Request["smscode"].ConvertTo<string>("");
                    var source = context.Request["source"].ConvertTo<string>("");
                    var phonestate = context.Request["phonestate"].ConvertTo<int>(0);
                    var agentid = context.Request["agentid"].ConvertTo<int>(0);

                    int result = 1;//返回处理结果：1出错；100成功
                    var data = AgentJosnData.Judgevalidsms(mobile, smscode, source, out result);
                    //手机动态密码登录
                    if (result == 100)
                    {
                        if (phonestate == 0)
                        {//新注册

                            //读取分销信息用于注册
                            var agentinfo = AgentCompanyData.GetAgentByid(agentid);

                            //读取分销开户账户信息
                            var openaccount = AgentCompanyData.GetAgentAccount(agentid);
                            var agentaccount = AgentCompanyData.GetAgentAccountByUid(openaccount, agentid);

                            if (agentaccount == null)
                            {

                                data = "{\"type\":1,\"msg\":\"绑定错误，分销商开户账户信息错误!\"}";
                                context.Response.Write(data);
                                return;
                            }


                            if (agentinfo != null)
                            {
                                B2b_company b2b_company = new B2b_company()
                                {
                                    ID = 0,
                                    Com_name = agentinfo.Company,
                                    Com_type = 0,
                                    Scenic_name = agentinfo.Company,
                                    Com_state = 2,//默认暂停 
                                    Imprest = 0,//默认预付款0
                                    Agentid = agentid,
                                    Bindingagent = agentid
                                };

                                B2b_company_info B2b_Company_Info = new B2b_company_info()
                                {
                                    Id = 0,
                                    Com_id = 0,
                                    Com_city = agentinfo.com_city,
                                    Com_class = 0,
                                    Com_add = agentinfo.Address,
                                    Com_code = "",
                                    Com_sitecode = "",
                                    Com_license = "",
                                    Sale_Agreement = "",
                                    Agent_Agreement = "",
                                    Scenic_address = agentinfo.Address,
                                    Scenic_intro = "",
                                    Scenic_Takebus = "",
                                    Scenic_Drivingcar = "",
                                    Contact = agentinfo.Contentname,
                                    Tel = agentinfo.Mobile,
                                    Phone = agentinfo.Mobile,
                                    Qq = "",
                                    Email = "",
                                    Defaultprint = "",
                                    //新加的
                                    Serviceinfo = "",
                                    Coordinate = "",
                                    Coordinatesize = 0,
                                    Domainname = "",
                                    Province = ""

                                };
                                B2b_company_manageuser manageuser = new B2b_company_manageuser
                                {
                                    Id = 0,
                                    Accounts = agentid + "_" + agentinfo.Mobile,
                                    Atype = 0,
                                    Com_id = 0,
                                    Createuserid = 0,
                                    Employeename = agentinfo.Contentname,
                                    Employeestate = (int)EmployeeStatus.Open,//开通,
                                    Job = "",
                                    Passwords = agentaccount.Pwd,
                                    Tel = agentinfo.Mobile,
                                    //新加的
                                    Channelcompanyid = 0,
                                    Channelsource = 0
                                };

                                try
                                {
                                    data = RegisterUserJsonData.InsertOrUpdateRegister(b2b_company, B2b_Company_Info, manageuser);
                                }
                                catch (Exception ex)
                                {
                                    data = "";
                                }
                                context.Response.Write(data);
                                return;
                            }
                            else
                            {
                                data = "{\"type\":1,\"msg\":\"绑定错误，分销商信息错误!\"}";
                                context.Response.Write(data);
                                return;
                            }


                        }
                        else
                        {//绑定

                            var comid = B2bCompanyData.GetAllComMsgbyphone(mobile);
                            if (comid > 0)
                            {
                                var data1 = AgentCompanyData.BindingAgent(comid, agentid);

                                if (data1 > 0)
                                {
                                    data = "{\"type\":100,\"msg\":\"绑定成功!\"}";
                                    context.Response.Write(data);
                                    return;
                                }
                                else
                                {
                                    data = "{\"type\":1,\"msg\":\"绑定错误,请刷新重新操作!\"}";
                                    context.Response.Write(data);
                                    return;
                                }
                            }
                            else
                            {
                                data = "{\"type\":1,\"msg\":\"绑定错误，未查询到该商户账户!\"}";
                                context.Response.Write(data);
                                return;
                            }

                        }
                    }
                    else
                    {
                        data = "{\"type\":1,\"msg\":\"验证出错，请重新发送验证!\"}";
                        context.Response.Write(data);
                        return;

                    }
                    context.Response.Write(data);
                }
                /// <summary>
                ///发送验证码 comid 可为0，account可为""（密码找回时需要有值）
                /// </summary>
                /// <param name="mobile"></param>
                /// <param name="comid"></param>
                /// <param name="source"></param>
                /// <param name="account"></param>
                /// <returns></returns>
                if (oper == "callvalidsms")
                {
                    var mobile = context.Request["mobile"].ConvertTo<string>("");
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var source = context.Request["source"].ConvertTo<string>("");
                    var account = context.Request["account"].ConvertTo<string>("");

                    string getcode = context.Request["imgcode"];

                    string ret = "";
                    if (getcode == "")
                    {
                        ret = "{\"type\":1,\"msg\":\"【图形验证码错误】\"}";
                        context.Response.Write(ret);
                        return;
                    }
                    else
                    {
                        if (context.Session["SomeValidateCode"] == null)
                        {
                            ret = "{\"type\":1,\"msg\":\"【图形验证码错误】\"}";
                            context.Response.Write(ret);
                        }
                        else
                        {
                            string initcode = context.Session["SomeValidateCode"].ToString();
                            if (getcode != initcode)
                            {
                                ret = "{\"type\":1,\"msg\":\"【图形验证码错误】\"}";
                                context.Response.Write(ret);
                            }
                            else
                            {

                                var data = AgentJosnData.Callvalidsms(mobile, comid, source, account);
                                context.Response.Write(data);
                            }
                        }
                    }

                }
                //人工充值预付款列表
                if (oper == "personrechargepagelist")
                {
                    int pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string key = context.Request["key"].ConvertTo<string>("");
                    string payment_type = context.Request["payment_type"].ConvertTo<string>("");

                    var data = AgentJosnData.Personrechargepagelist(pageindex, pagesize, agentid, comid, key, payment_type);
                    context.Response.Write(data);
                }
                //查询邮箱
                if (oper == "getEmail")
                {
                    string Email = context.Request["Email"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    var data = AgentJosnData.GetEmail(Email, comid);
                    context.Response.Write(data);
                }
                if (oper == "getPhone")
                {
                    string Phone = context.Request["Phone"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    var data = AgentJosnData.GetPhone(Phone);
                    context.Response.Write(data);
                }

                if (oper == "Agentsearch")
                {
                    string Phone = context.Request["Phone"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    var data = AgentJosnData.Agentsearch(Phone);
                    context.Response.Write(data);
                }

                //注册
                if (oper == "Agentregi")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    Agent_company regiinfo = new Agent_company();

                    regiinfo.Account = context.Request["Email"].ConvertTo<string>("");
                    regiinfo.Pwd = context.Request["Password1"].ConvertTo<string>("");
                    regiinfo.Contentname = context.Request["Name"].ConvertTo<string>("");
                    regiinfo.Tel = context.Request["Tel"].ConvertTo<string>("");
                    regiinfo.Mobile = context.Request["Phone"].ConvertTo<string>("");
                    regiinfo.Company = context.Request["Company"].ConvertTo<string>("");
                    regiinfo.Address = context.Request["Address"].ConvertTo<string>("");
                    regiinfo.Comid = context.Request["comid"].ConvertTo<int>(0);
                    regiinfo.Agentsort = context.Request["agentsort"].ConvertTo<int>(0);
                    regiinfo.Projectid = context.Request["Projectid"].ConvertTo<int>(0);
                    regiinfo.agentsourcesort = context.Request["agentsourcesort"].ConvertTo<int>(0);

                    regiinfo.com_province = context.Request["com_province"].ConvertTo<string>("");
                    regiinfo.com_city = context.Request["com_city"].ConvertTo<string>("");
                    regiinfo.Sms = context.Request["sms"].ConvertTo<int>(0);
                    regiinfo.Warrant_lp = context.Request["Warrant_lp"].ConvertTo<int>(0);


                    //如果注册分销shi
                    if (comid == 0)
                    {
                        string RequestUrl = context.Request.ServerVariables["SERVER_NAME"].ToLower();
                        comid = GeneralFunc.GetComid(RequestUrl);
                        if (comid != 0) {
                            regiinfo.Comid = comid;
                        }

                    }

                    var data = AgentJosnData.RegiAccount(regiinfo);
                    context.Response.Write(data);
                }



                if (oper == "getagentinfo")
                {
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    if (context.Session["Agentid"] != null)
                    {
                        if (agentid != Int32.Parse(context.Session["Agentid"].ToString()))
                        {
                            var data1 = "{\"type\":1,\"msg\":\"登陆信息丢失，请重新登录\"}";
                            context.Response.Write(data1);
                            return;
                        }
                    }
                    else
                    {
                        var data2 = "{\"type\":1,\"msg\":\"登陆信息丢失，请重新登录\"}";
                        context.Response.Write(data2);
                        return;
                    }


                    var data = AgentJosnData.GetAgentByid(agentid);
                    context.Response.Write(data);
                }

                if (oper == "companygetagentinfo")
                {
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);


                    var data = AgentJosnData.GetAgentByid(agentid);
                    context.Response.Write(data);
                }

                if (oper == "AgentUp")
                {
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);

                    Agent_company agentinfo = AgentCompanyData.GetAgentByid(agentid);
                    agentinfo.Id = agentid;
                    agentinfo.Contentname = context.Request["Name"].ConvertTo<string>("");
                    agentinfo.Tel = context.Request["Tel"].ConvertTo<string>("");
                    agentinfo.Mobile = context.Request["Phone"].ConvertTo<string>("");
                    agentinfo.Address = context.Request["Address"].ConvertTo<string>("");
                    agentinfo.Agent_domain = context.Request["Agentdomain"].ConvertTo<string>("");


                    int loginagentid = 0;

                    if (context.Session["Agentid"] != null)
                    {
                        loginagentid = int.Parse(context.Session["Agentid"].ToString());
                    }
                    else
                    {
                        var data2 = "{\"type\":1,\"msg\":\"登陆信息丢失，请重新登录\"}";
                        context.Response.Write(data2);
                        return;
                    }
                    if (loginagentid != agentid)
                    {
                        var data2 = "{\"type\":1,\"msg\":\"权限错误，请重新登录\"}";
                        context.Response.Write(data2);
                        return;
                    }



                    var data = AgentJosnData.RegiUpCompany(agentinfo);
                    context.Response.Write(data);
                }
                //管理员修改分销信息
                if (oper == "AdminAgentUp")
                {
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);

                    Agent_company agentinfo = AgentCompanyData.GetAgentByid(agentid);
                    agentinfo.Id = agentid;
                    agentinfo.Contentname = context.Request["Name"].ConvertTo<string>("");
                    agentinfo.Company = context.Request["Company"].ConvertTo<string>("");
                    agentinfo.Run_state = context.Request["Run_state"].ConvertTo<int>(0);
                    agentinfo.Tel = context.Request["Tel"].ConvertTo<string>("");
                    agentinfo.Mobile = context.Request["Phone"].ConvertTo<string>("");
                    agentinfo.Address = context.Request["Address"].ConvertTo<string>("");

                    agentinfo.com_province = context.Request["com_province"].ConvertTo<string>("");
                    agentinfo.com_city = context.Request["com_city"].ConvertTo<string>("");
                    agentinfo.Agent_domain = context.Request["agentdomain"].ConvertTo<string>("");

                    //开通淘宝码商
                    agentinfo.istaobao = context.Request["istaobao"].ConvertTo<int>(0);
                    agentinfo.tb_syncurl = context.Request["tb_syncurl"].ConvertTo<string>("");
                    agentinfo.tb_isret_consumeresult = context.Request["tb_isret_consumeresult"].ConvertTo<int>(0);

                    agentinfo.maxremindmoney = context.Request["maxremindmoney"].ConvertTo<int>(0);

                    //美团对接信息
                    agentinfo.ismeituan = context.Request["ismeituan"].ConvertTo<int>(0);

                    //如果是驴妈妈的，把类型改成3，美团改成0
                    if (context.Request["ismeituan"].ConvertTo<int>(0) == 3)
                    {
                        agentinfo.Agent_type = 3;
                        agentinfo.ismeituan = 0;
                    }
                    //如果是普通分销，重新赋值，直接利用是否是美团的，判断分销渠道
                    if (context.Request["ismeituan"].ConvertTo<int>(0) == 0)
                    {
                        agentinfo.Agent_type = 1;
                        agentinfo.ismeituan = 0;
                    }

                    if (context.Request["ismeituan"].ConvertTo<int>(0) == 1)
                    {
                        agentinfo.Agent_type = 1;
                    }


                    agentinfo.Lvmama_uid = context.Request["Lvmama_uid"].ConvertTo<string>("");
                    agentinfo.Lvmama_password = context.Request["Lvmama_password"].ConvertTo<string>("");
                    agentinfo.Lvmama_Apikey = context.Request["Lvmama_Apikey"].ConvertTo<string>("");


                    agentinfo.mt_partnerId = context.Request["mt_partnerId"].ConvertTo<string>("");
                    agentinfo.mt_client = context.Request["mt_client"].ConvertTo<string>("");
                    agentinfo.mt_secret = context.Request["mt_secret"].ConvertTo<string>("");
                    agentinfo.mt_mark = context.Request["mt_mark"].ConvertTo<string>("");

                    var data = AgentJosnData.AdminRegiUpCompany(agentinfo);
                    context.Response.Write(data);
                }

                //账户修改自己信息
                if (oper == "AgentAccountUp")
                {
                    string Account = context.Session["Account"].ToString();
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    int Accounttype = context.Request["Accounttype"].ConvertTo<int>(2);
                    decimal Amount = context.Request["Amount"].ConvertTo<decimal>(0);

                    Agent_regiinfo agentinfo = new Agent_regiinfo();
                    agentinfo.Agentid = agentid;
                    agentinfo.Contentname = context.Request["Accountname"].ConvertTo<string>("");
                    agentinfo.Pwd = context.Request["Pwd"].ConvertTo<string>("");
                    agentinfo.Mobile = context.Request["AccountMobile"].ConvertTo<string>("");
                    agentinfo.Account = Account;
                    agentinfo.Accounttype = 3;

                    var data = AgentJosnData.RegiUpAccount(agentinfo);
                    context.Response.Write(data);
                }

                //分销开户账户管理员工
                if (oper == "ManageAccountUp")
                {

                    int Accounttype = context.Request["Accounttype"].ConvertTo<int>(0);
                    Agent_regiinfo agentinfo = new Agent_regiinfo();
                    agentinfo.Id = context.Request["Id"].ConvertTo<int>(0);
                    agentinfo.Agentid = context.Request["agentid"].ConvertTo<int>(0);
                    agentinfo.Contentname = context.Request["Accountname"].ConvertTo<string>("");
                    agentinfo.Pwd = context.Request["Pwd"].ConvertTo<string>("");
                    agentinfo.Mobile = context.Request["AccountMobile"].ConvertTo<string>("");
                    agentinfo.Amount = context.Request["Amount"].ConvertTo<decimal>(0);
                    agentinfo.Account = context.Request["Account"].ConvertTo<string>("");


                    int loginagentid = 0;

                    if (context.Session["Agentid"] != null)
                    {
                        loginagentid = int.Parse(context.Session["Agentid"].ToString());
                    }
                    else
                    {
                        var data2 = "{\"type\":1,\"msg\":\"登陆信息丢失，请重新登录\"}";
                        context.Response.Write(data2);
                        return;
                    }
                    if (loginagentid != agentinfo.Agentid) {
                        var data2 = "{\"type\":1,\"msg\":\"权限错误，请重新登录\"}";
                        context.Response.Write(data2);
                        return;
                    }

                    var data = AgentJosnData.ManageUpAccount(agentinfo);
                    context.Response.Write(data);
                }

                if (oper == "Accountlist")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    int Agentid = context.Request["Agentid"].ConvertTo<int>(0);
                    int accountid = context.Request["accountid"].ConvertTo<int>(0);//默认为0

                    var data = AgentJosnData.Accountlist(Pageindex, Pagesize, Agentid,accountid);
                    context.Response.Write(data);
                }
                //获取分销账户信息
                if (oper == "getagentaccountinfo")
                {
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);

                    if (context.Session["Agentid"] != null)
                    {
                        if (agentid != Int32.Parse(context.Session["Agentid"].ToString()))
                        {
                            var data1 = "{\"type\":1,\"msg\":\"登陆信息丢失，请重新登录\"}";
                            context.Response.Write(data1);
                            return;
                        }
                    }
                    else
                    {
                        var data2 = "{\"type\":1,\"msg\":\"登陆信息丢失，请重新登录\"}";
                        context.Response.Write(data2);
                        return;
                    }


                    string Account = context.Session["Account"].ToString();

                    var data = AgentJosnData.GetAgentAccountByUid(Account, agentid);
                    context.Response.Write(data);
                }

                //获取分销账户信息
                if (oper == "getmanageaccountinfo")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    string Account = context.Request["Account"].ConvertTo<string>("");

                    int loginagentid = context.Request["agentid"].ConvertTo<int>(0);

                    if (context.Session["Agentid"] != null)
                    {
                        loginagentid = int.Parse(context.Session["Agentid"].ToString()) ;
                    }
                    else
                    {
                        var data2 = "{\"type\":1,\"msg\":\"登陆信息丢失，请重新登录\"}";
                        context.Response.Write(data2);
                        return;
                    }

                    var data = AgentJosnData.GetManageAgentAccountByUid(id, loginagentid);
                  

                    context.Response.Write(data);
                }

                //授权列表，分销商看到的
                if (oper == "warrantlist")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    string Key = context.Request["Key"].ConvertTo<string>("");
                    int Agentid = context.Request["Agentid"].ConvertTo<int>(0);

                    var data = AgentJosnData.Warrantpagelist(Pageindex, Pagesize, Key, Agentid);
                    context.Response.Write(data);
                }
                //更多供应商列表，分销商看到的
                if (oper == "unwarrantlist")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    string Key = context.Request["Key"].ConvertTo<string>("");
                    int Agentid = context.Request["Agentid"].ConvertTo<int>(0);

                    var data = AgentJosnData.UnWarrantpagelist(Pageindex, Pagesize, Key, Agentid);
                    context.Response.Write(data);
                }


                //绑定分销的商户 授权列表，分销商看到的
                if (oper == "bindingwarrantlist")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    string Key = context.Request["Key"].ConvertTo<string>("");
                    int Agentid = 0;

                    B2b_company company = UserHelper.CurrentCompany;
                    var comdata = B2bCompanyData.GetCompany(company.ID);
                    if (comdata != null)
                    {
                        Agentid = comdata.Bindingagent;
                    }

                    var data = AgentJosnData.Warrantpagelist(Pageindex, Pagesize, Key, Agentid);
                    context.Response.Write(data);
                }




                //微站分销列表，分销商看到的
                if (oper == "agentcomlist")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    string Key = context.Request["Key"].ConvertTo<string>("");
                    int Agentid = context.Request["Agentid"].ConvertTo<int>(0);

                    var data = AgentJosnData.AgentComlist(Pageindex, Pagesize, Key, Agentid);
                    context.Response.Write(data);
                }


                //授权商家产品的项目列表
                if (oper == "warrantprojectlist")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    string Key = context.Request["Key"].ConvertTo<string>("");
                    int Agentid = context.Request["Agentid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    var data = AgentJosnData.WarrantProjectlist(Pageindex, Pagesize, Key, Agentid, comid);
                    context.Response.Write(data);
                }


                //授权商家产品列表
                if (oper == "warrantprolist")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    string Key = context.Request["Key"].ConvertTo<string>("");
                    int Agentid = context.Request["Agentid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int projectid = context.Request["projectid"].ConvertTo<int>(0);
                    string viewmethod = context.Request["viewmethod"].ConvertTo<string>("");

                    var data = AgentJosnData.WarrantProlist(Pageindex, Pagesize, Key, Agentid, comid, projectid, viewmethod);
                    context.Response.Write(data);
                }
                if (oper == "allwarrantprolist")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    string Key = context.Request["Key"].ConvertTo<string>("");
                    int Agentid = context.Request["Agentid"].ConvertTo<int>(0);
                    int servertype = context.Request["servertype"].ConvertTo<int>(0);
                  
                    string viewmethod = context.Request["viewmethod"].ConvertTo<string>("");

                    var data = AgentJosnData.AllWarrantProlist(Pageindex, Pagesize, Key, Agentid,  viewmethod,servertype);
                    context.Response.Write(data);
                }


                //未授权商家产品列表
                if (oper == "unwarrantprolist")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    string Key = context.Request["Key"].ConvertTo<string>("");
                    int Agentid = context.Request["Agentid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int projectid = context.Request["projectid"].ConvertTo<int>(0);
                    string viewmethod = context.Request["viewmethod"].ConvertTo<string>("");

                    var data = AgentJosnData.UnWarrantProlist(Pageindex, Pagesize, Key, Agentid, comid, projectid, viewmethod);
                    context.Response.Write(data);
                }

                //绑定分销商授权商家产品列表
                if (oper == "bindingwarrantprolist")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    string Key = context.Request["Key"].ConvertTo<string>("");
                    int Agentid = 0;
                    int bindingcomid = context.Request["comid"].ConvertTo<int>(0);

                    B2b_company company = UserHelper.CurrentCompany;
                    var comdata = B2bCompanyData.GetCompany(company.ID);
                    if (comdata != null)
                    {
                        Agentid = comdata.Bindingagent;
                    }

                    var data = AgentJosnData.BindingWarrantProlist(Pageindex, Pagesize, Key, Agentid, bindingcomid, company.ID);
                    context.Response.Write(data);
                }
                //绑定分销商授权商家产品列表
                if (oper == "imprestpro")
                {


                    string proid = context.Request["proid"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);


                    var data = AgentJosnData.ImprestPro(proid, comid);
                    context.Response.Write(data);
                }


                //授权列表，服务商看到的
                if (oper == "agentpagelist")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    string Key = context.Request["Key"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string com_province = context.Request["com_province"].ConvertTo<string>("");
                    string com_city = context.Request["com_city"].ConvertTo<string>("");
                    int agentsourcesort = context.Request["agentsourcesort"].ConvertTo<int>(-1);
                    string warrantstate = context.Request["warrantstate"].ConvertTo<string>("0,1");
                    int projectid = context.Request["projectid"].ConvertTo<int>(0);

                    string startime = context.Request["startime"].ConvertTo<string>("");
                    string endtime = context.Request["endtime"].ConvertTo<string>("");



                    var data = AgentJosnData.Agentpagelist(Pageindex, Pagesize, Key, comid, com_province, com_city, warrantstate, agentsourcesort, projectid,startime,endtime);
                    context.Response.Write(data);
                }

                //分销商出票情况
                if (oper == "agentoutticketlist")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    string Key = context.Request["Key"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int projectid = context.Request["projectid"].ConvertTo<int>(0);
                    string warrantstate = context.Request["warrantstate"].ConvertTo<string>("0,1");

                    string startime = context.Request["startime"].ConvertTo<string>(DateTime.Now.ToString("yyyy-MM-dd"));
                    string endtime = context.Request["endtime"].ConvertTo<string>(DateTime.Now.ToString("yyyy-MM-dd"));


                    var data = AgentJosnData.Agentoutticketlist(Pageindex, Pagesize, Key, comid, warrantstate, projectid, startime, endtime);
                    context.Response.Write(data);
                }

                //特别授权列表，服务商看到的
                if (oper == "agentsetwarrantpagelist")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    int proid = context.Request["proid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    var data = AgentJosnData.AgentSetWarrantpagelist(Pageindex, Pagesize, proid, comid);
                    context.Response.Write(data);
                }
                //特别授权
                if (oper == "setwarrant")
                {

                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    int type = context.Request["type"].ConvertTo<int>(1);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int proid = context.Request["proid"].ConvertTo<int>(0);


                    var data = AgentJosnData.SetWarrant(agentid, type, proid, comid);
                    context.Response.Write(data);
                }

                //授权列表，服务商看到的
                if (oper == "etownagentwarrant")
                {

                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    var data = AgentJosnData.EtownAgentWarrant(comid);
                    context.Response.Write(data);
                }
                //所有分销商列表
                if (oper == "manageagentpagelist")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    string Key = context.Request["Key"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string com_province = context.Request["com_province"].ConvertTo<string>("");
                    string com_city = context.Request["com_city"].ConvertTo<string>("");
                    int agentsourcesort = context.Request["agentsourcesort"].ConvertTo<int>(-1);

                    var data = AgentJosnData.ManageAgentpagelist(Pageindex, Pagesize, Key, com_province, com_city, agentsourcesort);
                    context.Response.Write(data);
                }
                //调整分销短信设置
                if (oper == "ChangeAgentMsgset")
                {
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    int agentmsgset = context.Request["agent_messagesetting"].ConvertTo<int>(0);

                    var data = AgentJosnData.ChangeAgentMsgset(agentid, agentmsgset);
                    context.Response.Write(data);
                }

                if (oper == "unagentlist")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    string Key = context.Request["Key"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    var data = AgentJosnData.Unagentlist(Pageindex, Pagesize, Key, comid);
                    context.Response.Write(data);
                }

                if (oper == "agentFinacelist")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int recharge = context.Request["recharge"].ConvertTo<int>(0);

                    var data = AgentJosnData.AgentFinacelist(Pageindex, Pagesize, agentid, comid, recharge);
                    context.Response.Write(data);
                }
                if (oper == "projectlist")
                {
                    int agentid = 0;
                    if (context.Session["Agentid"] != null)
                    {
                        agentid = Int32.Parse(context.Session["Agentid"].ToString());
                    }

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    string key = context.Request["key"].ConvertTo<string>("");

                    var data = AgentJosnData.Projectlist(Pageindex, Pagesize, agentid, key);
                    context.Response.Write(data);
                }
                //商户 查看授权此项目的账户列表
                if (oper == "projectagentpagelist")
                {

                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int projectid = context.Request["projectid"].ConvertTo<int>(0);

                    var data = AgentJosnData.ProjectAgentlist(Pageindex, Pagesize, comid, projectid);
                    context.Response.Write(data);
                }

                //授权账户查看 指定项目的产品列表
                if (oper == "prolistbyprojectid")
                {
                    int agentid = 0;
                    if (context.Session["Agentid"] != null)
                    {
                        agentid = Int32.Parse(context.Session["Agentid"].ToString());
                    }
                    int Pageindex = context.Request["Pageindex"].ConvertTo<int>(1);
                    int Pagesize = context.Request["Pagesize"].ConvertTo<int>(10);
                    int projectid = context.Request["id"].ConvertTo<int>(0);

                    DateTime startime = context.Request["startime"].ConvertTo<DateTime>(DateTime.Now);
                    DateTime endtime = context.Request["endtime"].ConvertTo<DateTime>(DateTime.Now);

                    if (projectid == 0)
                    {
                        var data1 = "{\"type\":1,\"msg\":\"参数传递错误\"}";
                        context.Response.Write(data1);
                        return;
                    }
                    var data = AgentJosnData.ProlistbyProjectid(Pageindex, Pagesize, agentid, projectid, startime, endtime);
                    context.Response.Write(data);
                }

                //查看 验票日志
                if (oper == "pnolistbyproid")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var proid = context.Request["id"].ConvertTo<int>(0);
                    var startime = context.Request["startime"].ConvertTo<string>("");
                    var endtime = context.Request["endtime"].ConvertTo<string>("");

                    int agentid = 0;
                    if (context.Session["Agentid"] != null)
                    {
                        agentid = Int32.Parse(context.Session["Agentid"].ToString());
                    }

                    var prodata = new B2bComProData();
                    comid = prodata.GetComidByProid(proid).ToString();

                    if (agentid == 0 || proid == 0 || comid == "0")
                    {
                        var data1 = "{\"type\":1,\"msg\":\"参数传递错误\"}";
                        context.Response.Write(data1);
                        return;
                    }





                    string data = EticketJsonData.EPageList(comid, pageindex, pagesize, -1, proid, 0, 0, "", startime, endtime, agentid);

                    context.Response.Write(data);
                }


                if (oper == "AgentHotelOrderlist")
                {
                    var comid = 0;
                    var begindate = context.Request["begindate"].ConvertTo<string>("");
                    var enddate = context.Request["enddate"].ConvertTo<string>("");
                    var productid = context.Request["productid"].ConvertTo<int>(0);
                    var orderstate = context.Request["orderstate"].ConvertTo<string>("");

                    int agentid = 0;
                    if (context.Session["Agentid"] != null)
                    {
                        agentid = Int32.Parse(context.Session["Agentid"].ToString());
                    }

                    var prodata = new B2bComProData();
                    comid = prodata.GetComidByProid(productid);

                    if (agentid == 0 || productid == 0 || comid == 0)
                    {
                        var data1 = "{\"type\":1,\"msg\":\"参数传递错误\"}";
                        context.Response.Write(data1);
                        return;
                    }



                    string data = FinanceJsonData.HotelOrderlist(comid, begindate, enddate, productid, orderstate);
                    context.Response.Write(data);
                }

                //预付款后台操作
                if (oper == "writeagentmoney")
                {

                    string acttype = context.Request["acttype"].ConvertTo<string>("");
                    decimal money = context.Request["money"].ConvertTo<decimal>(0);
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string ordername = context.Request["ordername"].ConvertTo<string>("");
                    int Rebatetype = context.Request["Rebatetype"].ConvertTo<int>(0);
                    int userid = context.Request["userid"].ConvertTo<int>(0);

                    var data = AgentJosnData.WriteAgentMoney(acttype, money, agentid, comid, ordername, Rebatetype, userid);
                    context.Response.Write(data);
                }

                //修改分销账户类型
                if (oper == "ChangeAgentsort")
                {

                    int Agentsort = context.Request["Agentsort"].ConvertTo<int>(0);
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);

                    var data = AgentJosnData.ChangeAgentsort(agentid, Agentsort);
                    context.Response.Write(data);
                }
                //修改分销类型
                if (oper == "ChangeAgentsourcesort")
                {

                    int Agentsourcesort = context.Request["Agentsourcesort"].ConvertTo<int>(0);
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);

                    var data = AgentJosnData.ChangeAgentsourcesort(agentid, Agentsourcesort);
                    context.Response.Write(data);
                }

                //查询未开通的商户
                if (oper == "searchunopencom")
                {

                    string key = context.Request["key"].ConvertTo<string>("");
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);

                    var data = AgentJosnData.SearchUnopenCom(agentid, key);
                    context.Response.Write(data);
                }

                //开通商户
                if (oper == "AgentComOpen")
                {

                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    string Account = context.Request["Account"].ConvertTo<string>("");
                    int youxiaoqi = context.Request["youxiaoqi"].ConvertTo<int>(1);
                    string AdjustHasInnerChannel = context.Request["AdjustHasInnerChannel"].ConvertTo<string>("false");
                    var data = AgentJosnData.AgentComOpen(agentid, comid, Account, youxiaoqi, AdjustHasInnerChannel);


                    context.Response.Write(data);
                }

                //预付款后台操作
                if (oper == "setagentcredit")
                {

                    string acttype = context.Request["acttype"].ConvertTo<string>("");
                    decimal money = context.Request["money"].ConvertTo<decimal>(0);
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string ordername = context.Request["ordername"].ConvertTo<string>("");
                    int Rebatetype = context.Request["Rebatetype"].ConvertTo<int>(0);

                    var data = AgentJosnData.Setagentcredit(money, agentid, comid, ordername);
                    context.Response.Write(data);
                }


                //根据ID获取分销信息
                if (oper == "getAgentId")
                {
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    Agent_company userinfo = new Agent_company();
                    var data = AgentJosnData.GetAgentWarrant(agentid, comid);
                    context.Response.Write(data);

                }

                //授权，修改授权
                if (oper == "modifyAgentExt")
                {
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    int warrant_type = context.Request["warrant_type"].ConvertTo<int>(0);
                    int warrant_state = context.Request["warrant_state"].ConvertTo<int>(0);
                    int warrant_level = context.Request["warrant_level"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string com_province = context.Request["com_province"].ConvertTo<string>("");
                    string com_city = context.Request["com_city"].ConvertTo<string>("");

                    Agent_company userinfo = new Agent_company
                    {
                        Id = agentid,
                        Warrant_state = warrant_state,
                        Warrant_type = warrant_type,
                        Warrant_level = warrant_level,
                        Warrant_lp = 0,
                        Comid = comid,
                        com_province = com_province,
                        com_city = com_city

                    };
                    var data = AgentJosnData.ModifyAgentInfo(userinfo);
                    context.Response.Write(data);

                }

                //授权，修改授权
                if (oper == "autowarrant")
                {
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    int warrant_type = 1;
                    int warrant_state = 1;
                    int warrant_level = context.Request["warrant_level"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string com_province = context.Request["com_province"].ConvertTo<string>("");
                    string com_city = context.Request["com_city"].ConvertTo<string>("");

                    var cominfo = B2bCompanyData.GetAllComMsg(comid);
                    if (cominfo == null)
                    {
                        var data1 = "{\"type\":1,\"msg\":\"商户传递错误\"}";
                        context.Response.Write(data1);
                        return;
                    }
                    if (cominfo.Lp == 0)
                    {
                        var data1 = "{\"type\":1,\"msg\":\"商户设定不能自动授权，请联系商家进行授权\"}";
                        context.Response.Write(data1);
                        return;
                    }

                    warrant_level = cominfo.Lp_agentlevel;

                    Agent_company userinfo = new Agent_company
                    {
                        Id = agentid,
                        Warrant_state = warrant_state,
                        Warrant_type = warrant_type,
                        Warrant_level = warrant_level,
                        Warrant_lp = 1,// 1为平台分销，是由分销发起自动授权，而由商户发起的都是自有分销
                        Comid = comid,
                        com_province = com_province,
                        com_city = com_city
                    };
                    var data = AgentJosnData.ModifyAgentInfo(userinfo);
                    context.Response.Write(data);

                }

                if (oper == "phoneLogin")
                {
                    string Email = context.Request["Account"].ConvertTo<string>("");
                    string Pwd = context.Request["Pwd"].ConvertTo<string>("");
                    string isfreelanding = context.Request["isfreelanding"].ConvertTo<string>("");
                    string openid = context.Request["openid"].ConvertTo<string>("");

                    Agent_company userinfo = new Agent_company();
                    var data = AgentJosnData.Login(Email, Pwd, out userinfo);
                    if (data == "OK")
                    {
                        if (userinfo != null)
                        {
                            context.Session["Agentid"] = userinfo.Id;
                            context.Session["Account"] = Email;


                            HttpCookie cookie = new HttpCookie("Agentid");     //实例化HttpCookie类并添加值
                            cookie.Value = userinfo.Id.ToString();
                            cookie.Expires = DateTime.Now.AddDays(1);
                            context.Response.Cookies.Add(cookie);

                            cookie = new HttpCookie("Account");     //实例化HttpCookie类并添加值
                            cookie.Value = Email;
                            cookie.Expires = DateTime.Now.AddDays(1);
                            context.Response.Cookies.Add(cookie);

                            var returnmd5 = EncryptionHelper.ToMD5(Email + "lixh1210" + userinfo.Id.ToString(), "UTF-8");
                            cookie = new HttpCookie("AgentKey");     //实例化HttpCookie类并添加值
                            cookie.Value = returnmd5;
                            cookie.Expires = DateTime.Now.AddDays(1);
                            context.Response.Cookies.Add(cookie);

                            //微信端退出后，在点击微信免登陆 进行免登陆绑定
                            HttpCookie cookie1 = context.Request.Cookies["openid"];
                            if (cookie1 != null)
                            {
                                openid = cookie1.Value;
                            }

                            //把分销免登陆账户 进入 会员表
                            if (isfreelanding == "微信免登录" && openid != "")
                            {
                                new B2bCrmData().UpFreelandingAccount(openid, Email);
                            }
                            else
                            {
                                new B2bCrmData().UpFreelandingAccount(openid, "");
                            }
                        }
                    }
                    context.Response.Write(data);

                }

                if (oper == "Login")
                {
                    string Email = context.Request["Account"].ConvertTo<string>("");
                    string Pwd = context.Request["Pwd"].ConvertTo<string>("");
                    string getcode = context.Request["getcode"].ConvertTo<string>("");

                    string logindata = "";

                    try
                    {

                        if (context.Session["SomeValidateCode"] == null)
                        {
                            logindata = "用户验证码错误.";
                            context.Response.Write(logindata);
                            return;
                        }
                        else
                        {
                            string initcode = context.Session["SomeValidateCode"].ToString();
                            if (getcode != initcode)
                            {
                                logindata = "用户验证码错误";
                                context.Response.Write(logindata);
                                return;
                            }
                        }

                        Agent_company userinfo = new Agent_company();
                        var data = AgentJosnData.Login(Email, Pwd, out userinfo);
                        if (data == "OK")
                        {
                            if (userinfo != null)
                            {
                                context.Session["Agentid"] = userinfo.Id;
                                context.Session["Account"] = Email;


                                HttpCookie cookie = new HttpCookie("Agentid");     //实例化HttpCookie类并添加值
                                cookie.Value = userinfo.Id.ToString();
                                cookie.Expires = DateTime.Now.AddDays(1);
                                context.Response.Cookies.Add(cookie);

                                cookie = new HttpCookie("Account");     //实例化HttpCookie类并添加值
                                cookie.Value = Email;
                                cookie.Expires = DateTime.Now.AddDays(1);
                                context.Response.Cookies.Add(cookie);

                                var returnmd5 = EncryptionHelper.ToMD5(Email + "lixh1210" + userinfo.Id.ToString(), "UTF-8");
                                cookie = new HttpCookie("AgentKey");     //实例化HttpCookie类并添加值
                                cookie.Value = returnmd5;
                                cookie.Expires = DateTime.Now.AddDays(1);
                                context.Response.Cookies.Add(cookie);

                            }
                        }
                        context.Response.Write(data);
                    }
                    catch (Exception e)
                    {
                        logindata = "登录超时，请重新登录";
                        context.Response.Write(logindata);
                        return;
                    }

                }

                if (oper == "bindingagent")
                {
                    string Email = context.Request["Account"].ConvertTo<string>("");
                    string Pwd = context.Request["Pwd"].ConvertTo<string>("");
                    Agent_company userinfo = new Agent_company();
                    var data = AgentJosnData.BindingAgent(Email, Pwd, out userinfo);
                    if (data == "OK")
                    {
                        if (userinfo != null)
                        {
                            context.Session["Agentid"] = userinfo.Id;
                            context.Session["Account"] = Email;
                        }
                    }
                    context.Response.Write(data);

                }

                if (oper == "bindingcom")
                {
                    string Account = context.Request["Account"].ConvertTo<string>("");
                    string Pwd = context.Request["Pwd"].ConvertTo<string>("");
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);
                    Agent_company userinfo = new Agent_company();
                    var data = AgentJosnData.BindingCom(Account, Pwd, agentid);

                    context.Response.Write(data);

                }

                if (oper == "Agentbindingproject")
                {
                    string bindingemail = context.Request["bindingemail"].ConvertTo<string>("");
                    var projectid = context.Request["projectid"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);

                    Agent_company userinfo = new Agent_company();
                    var data = AgentJosnData.AgentBindingProject(bindingemail, projectid, comid);
                    context.Response.Write(data);

                }



                if (oper == "unbindingagent")
                {

                    var data = AgentJosnData.UnBindingAgent();
                    context.Response.Write(data);

                }

                if (oper == "loginagent")
                {
                    int comid = 0;
                    int agentid = 0;
                    if (UserHelper.ValidateLogin())
                    {
                        comid = UserHelper.CurrentCompany.ID;
                    }

                    B2b_company company = UserHelper.CurrentCompany;
                    var comdata = B2bCompanyData.GetCompany(company.ID);
                    if (comdata != null)
                    {
                        agentid = comdata.Bindingagent;
                        if (agentid != 0)
                        {
                            Agent_company userinfo = new Agent_company();
                            var data = AgentCompanyData.GetAgentByid(agentid);
                            var Account = AgentCompanyData.GetAgentAccount(agentid);
                            if (data != null)
                            {
                                context.Session["Agentid"] = data.Id;
                                context.Session["Account"] = Account;
                                context.Response.Write("OK");
                                return;
                            }
                        }

                    }
                    context.Response.Write("ERR");
                }


                if (oper == "printeticket")
                {
                    int pagesize = context.Request["pagesize"].ConvertTo<int>(0);
                    int id = context.Request["id"].ConvertTo<int>(0);
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int agentid = context.Request["agentid"].ConvertTo<int>(0);


                    int login_agentid = 0;
                    if (context.Session["Agentid"] != null)
                    {
                        login_agentid = Int32.Parse(context.Session["Agentid"].ToString());
                    }

                    if (agentid == 0 || agentid != login_agentid)
                    {
                        var data1 = "{\"type\":1,\"msg\":\"登陆信息错误，请重新登陆\"}";
                        context.Response.Write(data1);
                        return;
                    }
                    if (id == 0)
                    {
                        var data1 = "{\"type\":1,\"msg\":\"订单信息信息错误，请重新操作\"}";
                        context.Response.Write(data1);
                        return;
                    }
                    if (pagesize == 0)
                    {
                        var data1 = "{\"type\":1,\"msg\":\"打印数量错误，请重新操作\"}";
                        context.Response.Write(data1);
                        return;
                    }


                    var data = AgentJosnData.PrinteTicket(id, comid, agentid, pagesize);

                    context.Response.Write(data);

                }
                if (oper == "agenttravelbusordercountbyday")
                {
                    DateTime startdate = context.Request["startdate"].ConvertTo<DateTime>(DateTime.Now);
                    DateTime enddate = context.Request["enddate"].ConvertTo<DateTime>(DateTime.Now);
                    int servertype = context.Request["servertype"].ConvertTo<int>(10);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    int agentid = 0;
                    if (context.Session["Agentid"] != null)
                    {
                        agentid = Int32.Parse(context.Session["Agentid"].ToString());
                    }

                    if (agentid == 0)
                    {
                        var data1 = "{\"type\":1,\"msg\":\"登陆信息错误，请重新登陆\"}";
                        context.Response.Write(data1);
                        return;
                    }

                    string data = OrderJsonData.agenttravelbusordercountbyday(startdate, enddate, servertype, comid, agentid);
                    context.Response.Write(data);
                }
                if (oper == "agentGetb2bcomprobytraveldate")
                {
                    DateTime daydate = context.Request["daydate"].ConvertTo<DateTime>(DateTime.Now);
                    int servertype = context.Request["servertype"].ConvertTo<int>(10);
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    int agentid = 0;
                    if (context.Session["Agentid"] != null)
                    {
                        agentid = Int32.Parse(context.Session["Agentid"].ToString());
                    }

                    if (agentid == 0)
                    {
                        var data1 = "{\"type\":1,\"msg\":\"登陆信息错误，请重新登陆\"}";
                        context.Response.Write(data1);
                        return;
                    }
                    string data = OrderJsonData.agentGetb2bcomprobytraveldate(daydate, servertype, comid, agentid);
                    context.Response.Write(data);
                }
                if (oper == "agenttravelbustravelerlistBypaystate")
                {
                    //根据订单状态查询 旅游大巴订单子表 --乘客表
                    string gooutdate = context.Request["gooutdate"].ConvertTo<string>("");
                    int proid = context.Request["proid"].ConvertTo<int>(0);
                    int paystate = context.Request["paystate"].ConvertTo<int>(0);
                    string orderstate = context.Request["orderstate"].ConvertTo<string>("");
                    int agentid = 0;
                    if (context.Session["Agentid"] != null)
                    {
                        agentid = Int32.Parse(context.Session["Agentid"].ToString());
                    }

                    if (agentid == 0)
                    {
                        var data1 = "{\"type\":1,\"msg\":\"登陆信息错误，请重新登陆\"}";
                        context.Response.Write(data1);
                        return;
                    }

                    string date = OrderJsonData.agenttravelbustravelerlistBypaystate(gooutdate, proid, paystate, agentid, orderstate);
                    context.Response.Write(date);

                }

                if (oper == "agentmessagepagelist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);


                    var data = AgentJosnData.AgentMessageList(pageindex, pagesize, comid);
                    context.Response.Write(data);

                }

                if (oper == "agentmessageinfo")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);

                    var data = AgentJosnData.AgentMessageInfo(id, comid);
                    context.Response.Write(data);

                }

                if (oper == "agentmessageup")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var id = context.Request["id"].ConvertTo<int>(0);
                    var Title = context.Request["Title"].ConvertTo<string>("");
                    var Message = context.Request["Message"].ConvertTo<string>("");
                    var State = context.Request["State"].ConvertTo<int>(1);
                    var Sendsms = context.Request["Sendsms"].ConvertTo<int>(0);
                    var Smstext = context.Request["Smstext"].ConvertTo<string>("");

                    B2b_com_agent_message messageinfo = new B2b_com_agent_message();
                    messageinfo.Id = id;
                    messageinfo.Title = Title;
                    messageinfo.Message = Message;
                    messageinfo.State = State;
                    messageinfo.Comid = comid;
                    messageinfo.Sendsms = Sendsms;
                    messageinfo.Smstext = Smstext;

                    var data = AgentJosnData.AgentMessageUp(messageinfo);
                    context.Response.Write(data);

                }

                if (oper == "messagelist")
                {
                    int agentid = 0;
                    if (context.Session["Agentid"] != null)
                    {
                        agentid = Int32.Parse(context.Session["Agentid"].ToString());
                    }

                    if (agentid == 0)
                    {
                        var data1 = "{\"type\":1,\"msg\":\"登陆信息错误，请重新登陆\"}";
                        context.Response.Write(data1);
                        return;
                    }
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(1);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);


                    var data = AgentJosnData.AgentViewMessageList(pageindex, pagesize, comid, agentid);
                    context.Response.Write(data);

                }
                if (oper == "messagecookie")
                {
                    int agentid = 0;
                    if (context.Session["Agentid"] != null)
                    {
                        agentid = Int32.Parse(context.Session["Agentid"].ToString());
                    }

                    if (agentid == 0)
                    {
                        var data1 = "{\"type\":1,\"msg\":\"登陆信息错误，请重新登陆\"}";
                        context.Response.Write(data1);
                        return;
                    }

                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    if (comid == 0)
                    {
                        var data1 = "{\"type\":1,\"msg\":\"参数传递错误\"}";
                        context.Response.Write(data1);
                        return;
                    }

                    HttpCookie cookie = new HttpCookie("agentmessage" + comid);     //实例化HttpCookie类并添加值
                    cookie.Value = DateTime.Now.ToString();
                    context.Response.Cookies.Add(cookie);

                    var data = "{\"type\":100,\"msg\":\"设置成功\"}";
                    context.Response.Write(data);

                }


                if (oper == "getagentprovincelist")
                {
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var data = AgentJosnData.Getagentprovincelist(comid);
                    context.Response.Write(data);
                }
                if (oper == "managegetagentprovincelist")
                {

                    var data = AgentJosnData.Getagentprovincelist();
                    context.Response.Write(data);
                }
                if (oper == "GetAgentSortlist")
                {

                    var data = AgentJosnData.GetAgentSortlist();
                    context.Response.Write(data);
                }
                if (oper == "GetAgentSourceSortlist")
                {

                    var data = AgentJosnData.GetAgentSourceSortlist();
                    context.Response.Write(data);
                }
                if (oper == "GetAgentFinanceCount")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string StartDate = context.Request["StartDate"].ConvertTo<string>("");
                    string EndDate = context.Request["EndDate"].ConvertTo<string>("");

                    var data = AgentJosnData.GetAgentFinanceCount(comid, StartDate, EndDate);
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
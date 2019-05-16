using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.JsonFactory;
using System.Web.SessionState;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle.Enum;


namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// Company 的摘要说明
    /// </summary>
    public class Company : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string oper = context.Request["oper"].ConvertTo<string>("");
            if (oper != "")
            {
                if (oper == "verifyimgcode")
                {
                    string getcode = context.Request["imgcode"];

                    string ret = "";
                    if (context.Session["SomeValidateCode"] == null)
                    {
                        ret = "{\"type\":1,\"msg\":\"【图形验证码错误】\"}";
                    }
                    else
                    {
                        string initcode = context.Session["SomeValidateCode"].ToString();
                        if (getcode != initcode)
                        {
                            ret = "{\"type\":1,\"msg\":\"【图形验证码错误】\"}";
                        }
                        else
                        {
                            ret = "{\"type\":100,\"msg\":\"\"}";
                        }

                    }

                    context.Response.Write(ret);

                }
                if (oper == "login")
                {
                    string username = context.Request["username"];
                    string pwd = context.Request["pwd"];
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
                                logindata = RegisterUserJsonData.Login(username, pwd);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        logindata = "{\"type\":1,\"msg\":\"登录超时，请重新登录\"}";
                    }
                    context.Response.Write(logindata);
                }

                if (oper == "agnetlogincom")
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


                    string logindata = "";
                    try
                    {
                        if (agentid == 0)
                        {
                            logindata = "{\"type\":1,\"msg\":\"登陆出错，请重新登录！\"}";
                            context.Response.Write(logindata);
                            return;
                        }

                        var com = B2bCompanyData.GetAllComMsgbyAgentid(agentid);
                        if (com != null)
                        {
                            var com_user = B2bCompanyManagerUserData.GetFirstIDUser(com.ID);
                            if (com_user != 0)
                            {
                                logindata = RegisterUserJsonData.AgentLoginCom(com_user, com.ID);
                                context.Session["Agent_Com_Login"] = com_user;
                            }
                            else
                            {
                                logindata = "{\"type\":1,\"msg\":\"登录超时，请重新登录！\"}";
                            }
                            //logindata = RegisterUserJsonData.Login(username, pwd);
                        }
                        else
                        {
                            logindata = "{\"type\":1,\"msg\":\"登录超时，请重新登录！\"}";
                        }
                    }
                    catch (Exception e)
                    {
                        logindata = "{\"type\":1,\"msg\":\"登录超时，请重新登录！\"}";
                    }
                    context.Response.Write(logindata);
                }

                if (oper == "findpass")
                {

                    string Account = context.Request["Account"];
                    string phone = context.Request["phone"];
                    string getcode = context.Request["getcode"];
                    string findway = context.Request["findway"];
                    string accounttype = context.Request["accounttype"];
                    int comid = context.Request["comid"].ConvertTo<int>(0);

                    string logindata = "";
                    try
                    {
                        if (context.Session["SomeValidateCode"] == null)
                        {
                            logindata = "{\"type\":1,\"msg\":\"【验证码错误】\"}";
                        }
                        else
                        {
                            string initcode = context.Session["SomeValidateCode"].ToString();
                            if (getcode != initcode)
                            {
                                logindata = "{\"type\":1,\"msg\":\"【验证码错误】\"}";
                            }
                            else
                            {
                                if (accounttype == "1")
                                {//商家重置密码
                                    logindata = RegisterUserJsonData.FindPass(Account, phone, findway);
                                }
                                else if (accounttype == "2")//分销重置密码
                                {
                                    if (comid == 0)
                                    {
                                        logindata = "{\"type\":1,\"msg\":\"参数错误，请刷新后重新操作\"}";
                                    }
                                    else
                                    {
                                        logindata = AgentJosnData.FindPass(Account, phone, findway, comid);
                                    }
                                }
                                else
                                {
                                    logindata = "{\"type\":1,\"msg\":\"参数错误，请刷新后重新操作\"}";
                                }





                            }
                        }
                    }
                    catch (Exception e)
                    {
                        logindata = "{\"type\":1,\"msg\":\"超时，请重新操作\"}";
                    }
                    context.Response.Write(logindata);
                }

                if (oper == "edit")
                {

                    int id = context.Request["id"].ConvertTo<int>(0);//商家id
                    //商家员工登录信息
                    int staffid = context.Request["staffid"].ConvertTo<int>(0);//商家员工id
                    string Account = context.Request["Account"];
                    string passwords = context.Request["passwords"];
                    int atype = context.Request["atype"].ConvertTo<int>(0);
                    //商家基本信息          
                    int com_type = context.Request["com_type"].ConvertTo<int>(0);
                    string com_name = context.Request["com_name"];
                    string Scenic_name = context.Request["Scenic_name"].ConvertTo<string>("");
                    int Com_state = context.Request["com_state"].ConvertTo<int>(0);
                    decimal imprest = context.Request["imprest"].ConvertTo<decimal>(0);
                    //商家扩展信息
                    int com_extid = context.Request["com_extid"].ConvertTo<int>(0);
                    string com_city = context.Request["city"].ConvertTo<string>("");
                    string com_add = context.Request["com_add"].ConvertTo<string>("");
                    int com_class = context.Request["com_class"].ConvertTo<int>(0);
                    string com_code = context.Request["com_code"].ConvertTo<string>("");
                    string com_sitecode = context.Request["com_sitecode"].ConvertTo<string>("");
                    string com_license = context.Request["com_license"].ConvertTo<string>("");
                    string sale_Agreement = context.Request["sale_Agreement"].ConvertTo<string>("");
                    string agent_Agreement = context.Request["agent_Agreement"].ConvertTo<string>("");
                    string Scenic_address = context.Request["Scenic_address"].ConvertTo<string>("");
                    string Scenic_intro = context.Request["Scenic_intro"].ConvertTo<string>("");
                    string Scenic_Takebus = context.Request["Scenic_Takebus"].ConvertTo<string>("");
                    string Scenic_Drivingcar = context.Request["Scenic_Drivingcar"].ConvertTo<string>("");
                    string Contact = context.Request["Contact"].ConvertTo<string>("");
                    string tel = context.Request["tel"].ConvertTo<string>("");
                    string phone = context.Request["phone"].ConvertTo<string>("");
                    string qq = context.Request["qq"].ConvertTo<string>("");
                    string email = context.Request["email"].ConvertTo<string>("");
                    string Defaultprint = context.Request["Defaultprint"].ConvertTo<string>("");
                    string Domainname = context.Request["Domainname"].ConvertTo<string>("");

                    string province = context.Request["province"].ConvertTo<string>("");

                    int agentid = 0;
                    string RequestUrl = context.Request.ServerVariables["SERVER_NAME"].ToLower();
                    agentid = AgentCompanyData.DomainGetAgentid(RequestUrl);
                    int bindingagentid = context.Request["bindingagentid"].ConvertTo<int>(0);

                    if (phone == "")
                    {
                        if (tel.Length == 11)
                        {
                            phone = tel;
                        }
                    }


                    B2b_company b2b_company = new B2b_company()
                    {
                        ID = id,
                        Com_name = com_name,
                        Com_type = com_type,
                        Scenic_name = Scenic_name,
                        Com_state = 2,//默认暂停 
                        Imprest = 0,//默认预付款0
                        Agentid = agentid,
                        Bindingagent = bindingagentid
                    };

                    B2b_company_info B2b_Company_Info = new B2b_company_info()
                    {
                        Id = com_extid,
                        Com_id = id,
                        Com_city = com_city,
                        Com_class = com_class,
                        Com_add = com_add,
                        Com_code = com_code,
                        Com_sitecode = com_sitecode,
                        Com_license = com_license,
                        Sale_Agreement = sale_Agreement,
                        Agent_Agreement = agent_Agreement,
                        Scenic_address = Scenic_address,
                        Scenic_intro = Scenic_intro,
                        Scenic_Takebus = Scenic_Takebus,
                        Scenic_Drivingcar = Scenic_Drivingcar,
                        Contact = Contact,
                        Tel = tel,
                        Phone = phone,
                        Qq = qq,
                        Email = email,
                        Defaultprint = Defaultprint,
                        //新加的
                        Serviceinfo = "",
                        Coordinate = "",
                        Coordinatesize = 0,
                        Domainname = Domainname,
                        Province = province

                    };
                    B2b_company_manageuser manageuser = new B2b_company_manageuser
                    {
                        Id = staffid,
                        Accounts = Account,
                        Atype = atype,
                        Com_id = id,
                        Createuserid = UserHelper.ValidateLogin() == true ? UserHelper.CurrentUserId() : 0,
                        Employeename = Account,
                        Employeestate = (int)EmployeeStatus.Open,//开通,
                        Job = "",
                        Passwords = passwords,
                        Tel = tel,
                        //新加的
                        Channelcompanyid = 0,
                        Channelsource = 0
                    };
                    string data = "";//注册信息返回结果
                    try
                    {
                        data = RegisterUserJsonData.InsertOrUpdateRegister(b2b_company, B2b_Company_Info, manageuser);
                    }
                    catch (Exception ex)
                    {
                        data = "";
                    }
                    context.Response.Write(data);
                }
                if (oper == "GetAccountInfo")
                {
                    //北青总社获得用户登录信息
                    if (context.Session["AccountId"] != null)
                    {
                        int AccountId = Int32.Parse(context.Session["AccountId"].ToString());
                        string AccountName = context.Session["AccountName"].ToString();
                        string AccountCard = context.Session["AccountCard"].ToString();
                        context.Response.Write("{\"type\":100,\"msg\":{\"AccountId\":" + AccountId + ",\"AccountName\":" + AccountName + ",\"AccountCard\":" + AccountCard + "}");

                    }
                    else
                    {
                        context.Response.Write("{\"type\":100,\"msg\":\"\"}");
                    }
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
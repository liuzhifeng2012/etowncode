using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.JsonFactory;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Permision.Service.PermisionService.Model;
using ETS2.Common.Business;
using ETS2.Permision.Service.PermisionService.Data;
using ETS2.Member.Service.MemberService.Data;

namespace ETS2.WebApp.JsonFactory
{
    /// <summary>
    /// AccountInfo 的摘要说明
    /// </summary>
    public class AccountInfo : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "text/plain";

            string oper = context.Request["oper"].ConvertTo<string>("");
            if (oper != "")
            {
                if (oper == "Getqunarbycomid")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = AccountInfoJsonData.Getqunarbycomid(comid);
                    context.Response.Write(data);
                }
                if (oper == "Editqunarbycomid")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int isqunar = context.Request["isqunar"].ConvertTo<int>(0);
                    string qunar_username = context.Request["qunar_username"].ConvertTo<string>("");
                    string qunar_pass = context.Request["qunar_pass"].ConvertTo<string>("");

                    string data = AccountInfoJsonData.Editqunarbycomid(comid, isqunar, qunar_username, qunar_pass);
                    context.Response.Write(data);
                }
                if (oper == "editall")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);//当前商家id      
                    int userid = context.Request["userid"].ConvertTo<int>(0);//当前用户id     
                    //商家基本信息          
                    int com_type = context.Request["com_type"].ConvertTo<int>(0);
                    string com_name = context.Request["com_name"];
                    string Scenic_name = context.Request["Scenic_name"].ConvertTo<string>("");
                    int Com_state = context.Request["com_state"].ConvertTo<int>(0);
                    decimal imprest = context.Request["imprest"].ConvertTo<decimal>(0);
                    //商家扩展信息
                    int com_extid = context.Request["comextid"].ConvertTo<int>(0);

                    string com_province = context.Request["com_province"].ConvertTo<string>("");
                    string com_city = context.Request["com_city"].ConvertTo<string>("");
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

                    string wl_PartnerId = context.Request["wl_PartnerId"].ConvertTo<string>("");
                    string wl_userkey = context.Request["wl_userkey"].ConvertTo<string>("");


                    string serviceinfo = context.Request["serviceinfo"].ConvertTo<string>("");
                    string coordinate = context.Request["coordinate"].ConvertTo<string>("");
                    int coordinatesize = context.Request["coordinatesize"].ConvertTo<int>(13);
                    if (coordinatesize == 0)
                    {//地图显示大小不能等于0，按标准13大小显示
                        coordinatesize = 13;
                    }

                    string domainname = context.Request["domainname"].ConvertTo<string>("");
                    string admindomain = context.Request["admindomain"].ConvertTo<string>("");

                    string merchantintro = context.Request["merchantintro"].ConvertTo<string>("");

                    string weixinimg = context.Request["weixinimg"].ConvertTo<string>("");
                    string weixinname = context.Request["weixinname"].ConvertTo<string>("");
                    bool hasinnerchannel = context.Request["hasinnerchannel"].ConvertTo<bool>(true);



                    B2b_company b2b_company = new B2b_company()
                    {
                        ID = comid,
                        Com_name = com_name,
                        Com_type = com_type,
                        Scenic_name = Scenic_name,
                        Com_state = 1,//默认供应商    
                        Imprest = 0//预付款默认为0
                    };
                  

                    B2b_company_info B2b_Company_Info = new B2b_company_info()
                    {
                        Id = com_extid,
                        Com_id = comid,
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
                        Serviceinfo = serviceinfo,
                        Coordinate = coordinate,
                        Coordinatesize = coordinatesize,
                        Domainname = domainname,
                        Admindomain = admindomain,
                        Merchant_intro = merchantintro,
                        Weixinimg = weixinimg,
                        Weixinname = weixinname,
                        HasInnerChannel = hasinnerchannel,
                        Province = com_province,
                        wl_PartnerId = wl_PartnerId,
                        wl_userkey = wl_userkey
                    };

                    string data = "";//注册信息返回结果
                    try
                    {
                        data = AccountInfoJsonData.InsertOrUpdateB2bCompany(b2b_company, B2b_Company_Info);
                    }
                    catch (Exception ex)
                    {
                        data = "";
                    }
                    context.Response.Write(data);
                }
                if (oper == "getcurcompany")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = "";//获取公司基本信息和扩展信息
                    try
                    {
                        data = AccountInfoJsonData.GetAllComMsg(comid);

                    }
                    catch
                    {
                        data = "";
                    }
                    context.Response.Write(data);
                }
                if (oper == "editComName")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);//当前商家id      
                    //商家基本信息          
                    string com_name = context.Request["com_name"];
                    string data = "";//注册信息返回结果
                    try
                    {
                        data = AccountInfoJsonData.UpdateB2bCompanyName(comid, com_name);
                    }
                    catch (Exception ex)
                    {
                        data = "";
                    }
                    context.Response.Write(data);
                }

                if (oper == "editsearchset")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);//当前商家id      
                    //商家基本信息          
                    int setsearch = context.Request["setsearch"].ConvertTo<int>(0);
                    string data = "";//注册信息返回结果
                    try
                    {
                        data = AccountInfoJsonData.UpdateB2bCompanySearchset(comid, setsearch);
                    }
                    catch (Exception ex)
                    {
                        data = "";
                    }
                    context.Response.Write(data);
                }


                if (oper == "getcurcompanyguanzhu")
                {
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    string data = "";//获取公司基本信息和扩展信息
                    try
                    {
                        data = AccountInfoJsonData.GetAllComGuanzhuMsg(comid);

                    }
                    catch
                    {
                        data = "";
                    }
                    context.Response.Write(data);
                }
                if (oper == "editcomzizhi")
                {
                    int comextid = context.Request["comextid"].ConvertTo<int>(0);
                    string comcode = context.Request["com_code"].ConvertTo<string>("");
                    string comsitecode = context.Request["weixinname"].ConvertTo<string>("");
                    string comlicence = context.Request["com_license"].ConvertTo<string>("0");

                    string domainname = context.Request["domainname"].ConvertTo<string>("");
                    string scenic_intro = context.Request["scenic_intro"].ConvertTo<string>("");

                    string data = "";//获取修改商家资质信息
                    try
                    {
                        data = AccountInfoJsonData.ModifyComZizhi(comextid, comcode, comsitecode, comlicence, scenic_intro, domainname);
                    }
                    catch
                    {
                        data = "";
                    }
                    context.Response.Write(data);

                }
                if (oper == "changestaffpwd")
                {
                    var staffid = context.Request["staffid"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    
                    var password = context.Request["password"].ConvertTo<string>("");

                    string job = context.Request["job"].ConvertTo<string>("");
                    string Selfbrief = context.Request["Selfbrief"].ConvertTo<string>("");

                    string Workdaystime = context.Request["Workdaystime"].ConvertTo<string>("");
                    string Workendtime = context.Request["Workendtime"].ConvertTo<string>("");
                    string Fixphone = context.Request["Fixphone"].ConvertTo<string>("");
                    string Email = context.Request["Email"].ConvertTo<string>("");
                    string Homepage = context.Request["Homepage"].ConvertTo<string>("");
                    string Weibopage = context.Request["Weibopage"].ConvertTo<string>("");
                    string QQ = context.Request["QQ"].ConvertTo<string>("");
                    string Weixin = context.Request["Weixin"].ConvertTo<string>("");
                    string workdays = context.Request["workdays"].ConvertTo<string>("");

                    int projectid = context.Request["projectid"].ConvertTo<int>(0);
                    string proid = context.Request["proid"].ConvertTo<string>("");
                    int bindingproid = context.Request["bindingproid"].ConvertTo<int>(0);
                    B2b_company_manageuser manageruser = B2bCompanyManagerUserData.GetUser(staffid);
                    if(manageruser != null){
                        manageruser.Job = job;
                        manageruser.Passwords = password;
                        manageruser.Selfbrief = Selfbrief;
                        manageruser.Workdaystime = Workdaystime;
                        manageruser.Workendtime = Workendtime;
                        manageruser.Fixphone = Fixphone;
                        manageruser.Email = Email;
                        manageruser.Homepage = Homepage;
                        manageruser.QQ = QQ;
                        manageruser.Weixin = Weixin;
                        manageruser.Weibopage = Weibopage;
                        manageruser.bindingproid = bindingproid;
                    }


                    try
                    {
                        //暂时不对菜单进行跟踪，默认添加成功，发生错误概率较低
                        //通过员工id获取渠道id
                        var channelid = new MemberChannelData().GetChannelidbymanageuserid(staffid, comid);

                        //获取是否已经此渠道是否已经添加过菜单，对原有菜单修改
                        var id = new B2bCompanyMenuData().getConsultantidbychannelid(channelid);

                        //添加渠道菜单
                        B2b_company_menu menumodel = new B2b_company_menu()
                        {
                            Id = id,
                            Com_id = comid,
                            Imgurl = 0,
                            Linkurl = "",
                            Linktype = projectid,
                            Name = "我的推荐",
                            Fonticon = "",
                            Outdata = 0,
                            Prolist = proid,
                            Channelid = channelid,
                        };
                        var data1 = DirectSellJsonData.ConsultantInsertOrUpdate(menumodel);
                        //context.Response.Write(data1);

                    }
                    catch { }


                    var data = AccountInfoJsonData.Changestaffpwd(staffid, password, manageruser);
                    context.Response.Write(data);
                }
                
                if (oper == "editlp")
                {

                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var lp_agentlevel = context.Request["lp_agentlevel"].ConvertTo<int>(0);
                    var lp = context.Request["lp"].ConvertTo<int>(0);


                    var data = AccountInfoJsonData.EditLp(comid, lp, lp_agentlevel);
                    context.Response.Write(data);
                }



                if (oper == "editstaff")
                {
                    var staffid = context.Request["staffid"].ConvertTo<int>(0);
                    var comid = context.Request["comid"].ConvertTo<int>(0);
                    var userid = context.Request["userid"].ConvertTo<int>(0);
                    var account = context.Request["account"].ConvertTo<string>("");
                    var password = context.Request["password"].ConvertTo<string>("");
                    var employeename = context.Request["employeename"].ConvertTo<string>("");
                    var tel = context.Request["tel"].ConvertTo<string>("");
                    var viewtel = context.Request["viewtel"].ConvertTo<int>(1);
                    var oldtel = context.Request["oldtel"].ConvertTo<string>("");

                    var groupids = context.Request["groupids"].ConvertTo<string>("");

                    var channelcompanyid = context.Request["channelcompanyid"].ConvertTo<int>(0);
                    var channelsource = context.Request["channelsource"].ConvertTo<int>(0);

                    var peoplelistview = context.Request["peoplelistview"].ConvertTo<int>(0);

                    var employeestate = context.Request["employeestate"].ConvertTo<int>(1);
                  
                    string  job = context.Request["job"].ConvertTo<string>("");
                    int headimg = context.Request["headimg"].ConvertTo<int>(0);
                    int Workingyears = context.Request["Workingyears"].ConvertTo<int>(0);
                    string Selfbrief = context.Request["Selfbrief"].ConvertTo<string>("");
                    string workdays = context.Request["workdays"].ConvertTo<string>("");
                    string Workdaystime = context.Request["Workdaystime"].ConvertTo<string>("");
                    string Workendtime = context.Request["Workendtime"].ConvertTo<string>("");
                    string Fixphone = context.Request["Fixphone"].ConvertTo<string>("");
                    string Email = context.Request["Email"].ConvertTo<string>("");
                    string Homepage = context.Request["Homepage"].ConvertTo<string>("");
                    string Weibopage = context.Request["Weibopage"].ConvertTo<string>("");
                    
                    string QQ = context.Request["QQ"].ConvertTo<string>("");
                    string Weixin = context.Request["Weixin"].ConvertTo<string>("");
                    //以下数据暂时没什么用
                    var atype = context.Request["atype"].ConvertTo<int>(0);
                    int worktimestar = context.Request["worktimestar"].ConvertTo<int>(9);
                    int worktimeend = context.Request["worktimeend"].ConvertTo<int>(17);
                    int workendtimestar = context.Request["workendtimestar"].ConvertTo<int>(9);
                    int workendtimeend = context.Request["workendtimeend"].ConvertTo<int>(17);
                    int bindingproid = context.Request["bindingproid"].ConvertTo<int>(0);



                    //编辑员工信息
                    B2b_company_manageuser manageuser = new B2b_company_manageuser()
                    {
                        Id = staffid,
                        Accounts = account,
                        Atype = atype,
                        Com_id = comid,
                        Createuserid = userid,
                        Employeename = employeename,
                        Employeestate = employeestate,
                        Job = job,
                        Passwords = password,
                        Tel = tel,
                        Viewtel = viewtel,
                        OldTel = oldtel,
                        Channelcompanyid = channelcompanyid,
                        Channelsource = channelsource,
                         Headimg=headimg,
                         Workingyears=Workingyears,
                         Selfbrief=Selfbrief,
                         Workdays=workdays,
                         Workdaystime=Workdaystime,
                         Workendtime=Workendtime,
                         Fixphone=Fixphone,
                         Email=Email,
                         Homepage=Homepage,
                         QQ=QQ,
                         Weixin=Weixin,
                         Weibopage=Weibopage,
                         Peoplelistview = peoplelistview,
                         worktimestar= worktimestar,
                         worktimeend=worktimeend,
                         workendtimestar = workendtimestar,
                         workendtimeend= workendtimeend,
                        bindingproid = bindingproid,
                    };
                    //var data1 = AccountInfoJsonData.EditStaff(manageuser);

                    //编辑人员小组映射表
                    B2b_company_manageuser user = UserHelper.CurrentUser();
                    int createmasterid = user.Id;
                    string createmastername = user.Employeename;
                    DateTime createdate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    string data = PermissionJsonData.EditUserAndMasterGroup(manageuser, staffid.ToString(), employeename, groupids, createmasterid, createmastername, createdate);

                    context.Response.Write(data);



                }
                if (oper == "editcomshouquan")
                {
                    int comextid = context.Request["comextid"].ConvertTo<int>(0);
                    string sale_Agreement = context.Request["sale_Agreement"].ConvertTo<string>("");
                    string agent_Agreement = context.Request["agent_Agreement"].ConvertTo<string>("");

                    string data = "";//获取修改商家授权信息
                    try
                    {
                        data = AccountInfoJsonData.ModifyComShouquan(comextid, sale_Agreement, agent_Agreement);

                    }
                    catch
                    {
                        data = "";
                    }
                    context.Response.Write(data);
                }
                if (oper == "editbangprint")
                {
                    int comextid = context.Request["comextid"].ConvertTo<int>(0);
                    string Defaultprint = context.Request["Defaultprint"].ConvertTo<string>("");

                    string data = "";//获取修改绑定打印机信息
                    try
                    {
                        data = AccountInfoJsonData.ModifyBangPrint(comextid, Defaultprint);

                    }
                    catch
                    {
                        data = "";
                    }
                    context.Response.Write(data);
                }
                if (oper == "editbangpos")
                {
                    int posid = context.Request["posid"].ConvertTo<int>(0);
                    string poscompany = context.Request["poscompany"].ConvertTo<string>("");
                    string remark = context.Request["Remark"].ConvertTo<string>("");
                    int comid = context.Request["comid"].ConvertTo<int>(0);
                    int userid = context.Request["userid"].ConvertTo<int>(0);
                    int pos_id = context.Request["pos_id"].ConvertTo<int>(0);
                    int projectid = context.Request["projectid"].ConvertTo<int>(0);
                    string md5key = context.Request["md5key"].ConvertTo<string>("");
                    string data = "";//获取修改绑定打印机信息
                    try
                    {
                        data = AccountInfoJsonData.ModifyBangPos(posid, poscompany, comid, userid, remark, pos_id, md5key, projectid);

                    }
                    catch
                    {
                        data = "";
                    }
                    context.Response.Write(data);
                }




                if (oper == "allpos")
                {

                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    var key = context.Request["key"].ConvertTo<string>("");
                    string data = AccountInfoJsonData.posList(pageindex, pagesize,key);

                    context.Response.Write(data);
                }
                if (oper == "manageuserpagelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    string data = AccountInfoJsonData.Manageuserpagelist(comid, pageindex, pagesize);

                    context.Response.Write(data);
                }
                if (oper == "posinfo")
                {
                    int pos_id = int.Parse(context.Request["pos_id"]);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    string data = AccountInfoJsonData.posinfo(pos_id);

                    context.Response.Write(data);
                }
                if (oper == "enterNote")
                {
                    string key = context.Request["key"].ConvertTo<string>("");
                    var content = context.Request["content"].ConvertTo<string>("");
                    string title = context.Request["title"].ConvertTo<string>("");
                    bool radio = context.Request["radio"].ConvertTo<bool>();
                    int note_id = context.Request["note_id"].ConvertTo<int>(0);

                    string data = AccountInfoJsonData.Insertnote(key, content, title, radio, note_id);
                    context.Response.Write(data);
                }

                if (oper == "notelist")
                {
                    var comid = context.Request["comid"];
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    string data = AccountInfoJsonData.notList(comid, pageindex, pagesize);

                    context.Response.Write(data);
                }

                if (oper == "noteinfo")
                {
                    int note_id = int.Parse(context.Request["note_id"]);
                    var pageindex = context.Request["pageindex"].ConvertTo<int>(0);
                    var pagesize = context.Request["pagesize"].ConvertTo<int>(10);
                    string data = AccountInfoJsonData.noteinfo(note_id);

                    context.Response.Write(data);
                }

                if (oper == "delnote")
                {
                    int id = context.Request["id"].ConvertTo<int>(0);
                    string key = context.Request["key"].ConvertTo<string>("");

                    string data = AccountInfoJsonData.Delnote(id, key);

                    context.Response.Write(data);
                }
                if (oper == "ViewChildCompany")
                {
                    int childcomid = context.Request["childcomid"].ConvertTo<int>(0);
                    int curuserid = context.Request["curuserid"].ConvertTo<int>(0);

                    string data = AccountInfoJsonData.ViewChildCompany(childcomid, curuserid);

                    context.Response.Write(data);

                }
                if (oper == "GetAllCompany")
                {
                    string data = AccountInfoJsonData.GetAllCompany();

                    context.Response.Write(data);
                }
                if (oper == "adjustemploerstate")
                {
                    int masterid = context.Request["masterid"].ConvertTo<int>();
                    int employerstate = context.Request["employerstate"].ConvertTo<int>(1);
                    string data = AccountInfoJsonData.Adjustemploerstate(masterid, employerstate);

                    context.Response.Write(data);
                }
                if (oper == "editwxauthorfocus")
                {
                    int comid = context.Request["comid"].ConvertTo<int>();
                    string author = context.Request["author"].ConvertTo<string>("");
                    string url = context.Request["url"].ConvertTo<string>("");

                    string data = AccountInfoJsonData.Editwxauthorfocus(comid,author,url);
                    context.Response.Write(data);
                }
                if (oper == "editcompanyistran_customer_service")
                {
                    int comid = context.Request["comid"].ConvertTo<int>();
                    int istransfer_customer_service = context.Request["istransfer_customer_service"].ConvertTo<int>(0);
                

                    string data = AccountInfoJsonData.Editwxauthorfocus(comid, istransfer_customer_service);
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
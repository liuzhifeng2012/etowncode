using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using System.Collections;
using ETS2.Permision.Service.PermisionService.Data;
using ETS2.Member.Service.MemberService.Data;
using ETS.Framework;
using ETS2.Common.Business;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;

namespace ETS.JsonFactory
{
    /// <summary>
    /// 商家员工操作
    /// </summary>
    public class UserHandleJsonData
    {
        public static string ChangePwd(int userid, string oldpwd, string pwd1)
        {
            try
            {
                string message = "";
                int changepwd = B2bCompanyManagerUserData.ChangePwd(userid, oldpwd, pwd1, out message);
                if (changepwd == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = message });
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = message });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });

            }
        }

        public static string GetManageUserInfo(int userid)
        {
            try
            {
                B2b_company_manageuser manageruser = B2bCompanyManagerUserData.GetUser(userid);
                if (manageruser == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "null" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = manageruser });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }

        public static string GetAccountInfo(int userid)
        {
            try
            {
                B2b_company_manageuser manageruser = B2bCompanyManagerUserData.GetUser(userid);
                if (manageruser == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "null" });
                }
                else
                {
                    var list = new List<B2b_company_manageuser>();
                    list.Add(manageruser);

                    IEnumerable result = "";
                    if (list != null)

                        result = from model in list
                                 select new
                                 {
                                     Accounts = model.Accounts,
                                     PassWord = model.Passwords,
                                     MasterId = model.Id,
                                     MasterName = model.Employeename,
                                     CompanyName = B2bCompanyData.GetCompanyByUid(model.Id).Com_name,
                                     Tel = model.Tel,
                                     Viewtel = model.Viewtel,
                                     GroupNames = new Sys_MasterGroupData().GetGroupNameStrByMasterId(model.Id),
                                     GroupIds = new Sys_MasterGroupData().GetGroupIdStrByMasterId(model.Id),
                                     ChannelCompanyId = model.Channelcompanyid,
                                     ChannelCompanyName = model.Channelcompanyid == 0 ? "全部渠道" : new MemberChannelcompanyData().GetCompanyById(model.Channelcompanyid.ToString().ConvertTo<int>(0)).Companyname,
                                     Channelsource = model.Channelsource,
                                     CreateUserId = model.Createuserid,
                                     EmployeState = model.Employeestate,
                                     
                                     Job=model.Job,
                                     Selfbrief=model.Selfbrief,
                                     Headimg=model.Headimg,
                                     Headimgurl=FileSerivce.GetImgUrl(model.Headimg),
                                     Workingyears=model.Workingyears,
                                     Workdays=model.Workdays,
                                     Workdaystime=model.Workdaystime,
                                     Workendtime=model.Workendtime,
                                    
                                     //WorkAddress=new B2bCompanyData().GetWordAddressByChannelCompany(model.Channelcompanyid,model.Com_id),

                                     Fixphone =model.Fixphone,
                                     Email=model.Email,
                                     Homepage=model.Homepage,
                                     Weibopage=model.Weibopage,
                                     QQ=model.QQ,
                                     Weixin=model.Weixin,
                                     Selfhomepage_qrcordurl = model.Selfhomepage_qrcordurl,
                                     Peoplelistview = model.Peoplelistview,
                                     worktimestar=model.worktimestar,
                                     worktimeend=model.worktimeend,
                                     workendtimestar=model.workendtimestar,
                                     workendtimeend=model.workendtimeend,
                                     bindingproid = model.bindingproid,

                                     
                                 };

                //读取产品，渠道自定义产品
                    var prodata = new B2bComProData();
                    var menudata = new B2bCompanyMenuData();
                    List<B2b_com_pro> Prolist = null;
                    int projcetid=0;
                    var channelid =new MemberChannelData().GetChannelidbymanageuserid(userid,manageruser.Com_id);
                    if (channelid != 0) {
                        int totalcount1 = 0;
                        Prolist = prodata.Selectpagelist_diaoyong(manageruser.Com_id.ToString(), 1, 50, "", out totalcount1, 0, 0, 0, 0, channelid);//读出每个栏目的产品，每页12个
                        projcetid = menudata.selectprojceidbychannelid(manageruser.Com_id, channelid);
                        
                    }


                    return JsonConvert.SerializeObject(new { type = 100, msg = result, Prolist = Prolist,projcetid=projcetid });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }

        public static string GetManageUserByAccount(string account)
        {
            try
            {
                B2b_company_manageuser manageruser = B2bCompanyManagerUserData.GetManageUserByAccount(account);
                if (manageruser == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "null" });
                }
                else
                {
                    var list = new List<B2b_company_manageuser>();
                    list.Add(manageruser);

                    IEnumerable result = "";
                    if (list != null)

                        result = from model in list
                                 select new
                                 {
                                     Accounts = model.Accounts,
                                     PassWord = model.Passwords,
                                     MasterId = model.Id,
                                     MasterName = model.Employeename,
                                     CompanyName = B2bCompanyData.GetCompanyByUid(model.Id).Com_name,
                                     Tel = model.Tel,
                                     GroupNames = new Sys_MasterGroupData().GetGroupNameStrByMasterId(model.Id),
                                     GroupIds = new Sys_MasterGroupData().GetGroupIdStrByMasterId(model.Id),
                                     ChannelCompanyId = model.Channelcompanyid,
                                     ChannelCompanyName = model.Channelcompanyid == 0 ? "全部渠道" : new MemberChannelcompanyData().GetCompanyById(model.Channelcompanyid.ToString().ConvertTo<int>(0)).Companyname,
                                     Channelsource = model.Channelsource
                                 };


                    return JsonConvert.SerializeObject(new { type = 100, msg = result });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }

        public static string GetB2bCompanyByCompanyName(string companyname)
        {
            B2b_company company = B2bCompanyManagerUserData.GetB2bCompanyByCompanyName(companyname);
            if (company == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = company });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = company });
            }
        }

        public static string GetB2bCompanyInfoByDomainname(string Domainname)
        {
            B2b_company_info companyinfo = new B2bCompanyInfoData().GetB2bCompanyInfoByDomainname(Domainname);
            if (companyinfo == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = companyinfo });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = companyinfo });
            }
        }

        public static string GetWorktimeInfo(int userid,DateTime date)
        {
            try
            {
                B2b_company_manageuser manageruser = B2bCompanyManagerUserData.GetUser(userid);
                if (manageruser == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
                else
                {
                    string listarr = "";
                    var dateweek = 0;
                    var weekend = 0;//是否为周末
                    var dt = date.DayOfWeek.ToString();
                    switch (dt)
                    {
                        case "Monday":
                            dateweek = 2;
                            break;
                        case "Tuesday":
                            dateweek = 3;
                            break;
                        case "Wednesday":
                            dateweek = 4;
                            break;
                        case "Thursday":
                            dateweek = 5;
                            break;
                        case "Friday":
                            dateweek = 6;
                            break;
                        case "Saturday":
                            dateweek = 7;
                            weekend = 1;
                            break;
                        case "Sunday":
                            dateweek = 1;
                            weekend = 1;
                            break;
                    }

                    if (manageruser.Workdays.Contains(dateweek.ToString()))//有当前日期
                    {
                        if (weekend == 1)//如果是周末
                        {
                            var starh = manageruser.workendtimestar;
                            var endh = manageruser.workendtimeend;

                            if (starh <= endh)//不允许隔天
                            {
                                for (int i = starh; i < endh; i++) {
                                    listarr += i + ",";
                                }
                            }
                        }
                        else {
                            var starh = manageruser.worktimestar;
                            var endh = manageruser.worktimeend;

                            if (starh <= endh)
                            {
                                for (int i = starh; i < endh; i++)
                                {
                                    listarr += i + ",";
                                }
                            }
                        }
                    }
                    else {//如果没当前日期，则为空不让选择时间
                        return JsonConvert.SerializeObject(new { type = 100, msg = "" });
                    }

                    if (listarr != "") { 
                        //对工作时间再加上黑名单判定，如果已经使用的时间列为黑名单
                        int outcount=0;
                        var workdata =new B2bCompanyManagerUserData().Worktimepagelist(manageruser.Com_id, userid, date, "", out outcount);
                        if (outcount > 0) { //如果有黑名单数据

                            for (int i = 0; i < outcount; i++) {
                                var cannelstr = workdata[i].Hournum + ",";
                                listarr = listarr.Replace(cannelstr, "");//把查出来的时间过滤掉
                            }
                        }
                    }

                    return JsonConvert.SerializeObject(new { type = 100, msg = listarr });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }

    }
}

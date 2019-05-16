using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using Newtonsoft.Json;
using ETS2.Common.Business;
using ETS2.Member.Service.MemberService.Model;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Member.Service.MemberService.Model.Enum;
using ETS2.Member.Service.MemberService.Data.InternalData;
using ETS2.WeiXin.Service.WeiXinService.Model;
using System.Web;
using ETS2.WeiXin.Service.WeiXinService.Data;
using System.Collections;
using ETS2.PM.Service.PMService.Data;
using ETS.Framework;
using ETS2.VAS.Service.VASService.Data;
using ETS2.CRM.Service.CRMService.Modle.Enum;
using System.Data;
using ETS2.PM.Service.PMService.Modle;



namespace ETS.JsonFactory
{    /// <summary>
    /// 分销操作
    /// </summary>
    public class AgentJosnData
    {

        #region 判断邮箱
        public static string GetEmail(string email, int comid)
        {
            var pro = "";
            try
            {
                //bool isNum = Domain_def.RegexValidate("^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$", email.Trim());
                if (email.Length > 2)
                {
                    pro = AgentCompanyData.GetEmail(email).ToString();

                    if (pro != "0")
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = "此账户已经使用" });
                    }

                }
                else
                {
                    pro = ("账户错误");
                    return JsonConvert.SerializeObject(new { type = 100, msg = pro });
                }


                return JsonConvert.SerializeObject(new { type = 100, msg = "OK" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 判断手机
        public static string GetPhone(string phone)
        {
            var pro = "";
            try
            {
                bool isNum = Domain_def.RegexValidate("^\\d{11}$", phone);//验证是11位数字
                if (isNum)
                {
                    pro = AgentCompanyData.GetPhone(phone).ToString();

                    if (pro != "0")
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = "此手机已经使用" });
                    }
                }
                else
                {
                    pro = ("手机地址错误");
                    return JsonConvert.SerializeObject(new { type = 100, msg = pro });
                }


                return JsonConvert.SerializeObject(new { type = 100, msg = "OK" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 查询分销商
        public static string Agentsearch(string phone)
        {
            var pro = "";
            try
            {
                bool isNum = Domain_def.RegexValidate("^\\d{11}$", phone);//验证是11位数字
                if (isNum)
                {
                    pro = AgentCompanyData.Agentsearch(phone).ToString();

                    if (pro != "0")
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = "OK", id = pro });
                    }
                    else
                    {
                        pro = ("没有此分销商");
                        return JsonConvert.SerializeObject(new { type = 1, msg = pro });
                    }
                }
                else
                {
                    pro = ("手机号错误");
                    return JsonConvert.SerializeObject(new { type = 1, msg = pro });
                }


            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 注册账户
        public static string RegiAccount(Agent_company regiinfo)
        {
            try
            {
                int count = new AgentCompanyData().GetAgentUserCountByMobile(regiinfo.Mobile);
                if (count > 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "该手机号码已经注册!" });

                }

                var pro = AgentCompanyData.RegiAccount(regiinfo);
                if (pro == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "注册账户错误" });
                }
                else
                {

                    string cominfo = "";
                    if (regiinfo.Comid != 0)
                    {
                        var comdata = new B2bCompanyData();
                        cominfo = comdata.GetCompanyNameById(regiinfo.Comid);
                    }


                    //注册成功 发送短信
                    if (regiinfo.Sms == 1)
                    {
                        //发送固定短信

                        string smsstr = cominfo + "为您创建分销账户:" + regiinfo.Account + " 默认密码:123456 请登录后立即更改，登录地址 http://shop.etown.cn/agent";
                        string msg = "";
                        int sendback = SendSmsHelper.SendSms(regiinfo.Mobile, smsstr, regiinfo.Comid, out msg);
                    }
                }

                //新增分销商直接授权为1级，出票扣款，再次修改数据//不跟踪数据，失败了手工再添加
                Agent_company userinfo = new Agent_company
                {
                    Id = pro,
                    Warrant_state = 1,//-1为未授权状态，0关闭，1开通
                    Warrant_type = 1,
                    Warrant_level = 3,
                    Comid = regiinfo.Comid,
                    Projectid = regiinfo.Projectid,
                    Warrant_lp = regiinfo.Warrant_lp
                };




                if (regiinfo.Agentsort == 0)
                {//分销则进入授权流程
                    //查询是否授权过，只能授权一次
                    if (regiinfo.Comid != 0 && userinfo.Id != 0)
                    {
                        var searchsatate = AgentCompanyData.SearchAgentwarrant(userinfo);
                        if (searchsatate == 0)
                        {//授权
                            var shouquan = AgentCompanyData.AddAgentInfo(userinfo);

                        }
                    }
                }

                if (regiinfo.Agentsort == 2)
                {//项目账户则做项目授权
                    if (regiinfo.Comid != 0 && userinfo.Id != 0)
                    {
                        var shouquan = AgentCompanyData.AddAgentProject(userinfo);
                    }
                }



                return JsonConvert.SerializeObject(new { type = 100, msg = "OK", id = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 绑定分销账户
        public static string AgentBindingProject(string email, int projectid, int comid)
        {
            var agentdata = new AgentCompanyData();

            try
            {
                if (UserHelper.ValidateLogin())
                {
                    comid = UserHelper.CurrentCompany.ID;
                }

                //先判断分销信息，返回ok则正确
                var pro = AgentCompanyData.AgentSearchAccount(email);

                if (pro != null)
                {
                    var agentinfo = agentdata.GetAgentCompany(pro.Agentid);

                    if (agentinfo != null)
                    {
                        if (agentinfo.Agentsort == 2)
                        {//项目账户则做项目授权
                            if (comid != 0 && agentinfo.Id != 0)
                            {
                                agentinfo.Comid = comid;
                                agentinfo.Projectid = projectid;

                                var shouquan = AgentCompanyData.AddAgentProject(agentinfo);
                                return JsonConvert.SerializeObject(new { type = 100, msg = "OK" });
                            }
                        }
                    }
                }

                return JsonConvert.SerializeObject(new { type = 1, msg = "绑定错误,请重新操作" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 修改分销商公司信息
        public static string RegiUpCompany(Agent_company regiinfo)
        {
            try
            {
                var pro = AgentCompanyData.RegiUpCompany(regiinfo);
                if (pro == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "账户错误" });
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = "OK" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 管理员修改分销商公司信息
        public static string AdminRegiUpCompany(Agent_company regiinfo)
        {
            try
            {

                if (regiinfo.ismeituan == 1)
                {
                    //美团配置信息其他分销需要没有配置过
                    bool isexist = new Agent_companyData().IsExistMeituanConfig(regiinfo);
                    if (isexist)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "美团信息其他分销配置过" });
                    }

                    //美团分销 只可以 得到一家商户授权(美团查询余额接口是根据分销id查询的，如果授权多个商户的话，无法确认返回哪个授权的商户)
                    List<Agent_warrant> warrantinfolist = new AgentCompanyData().GetAgentWarrantList(regiinfo.Id, "1");
                    if (warrantinfolist.Count > 1)
                    {
                        string shouquancomid = "";
                        foreach (Agent_warrant iiinfo in warrantinfolist)
                        {
                            shouquancomid += iiinfo.Comid.ToString() + ",";
                        }
                        shouquancomid = shouquancomid.Substring(0, shouquancomid.Length - 1);

                        return JsonConvert.SerializeObject(new { type = 1, msg = "美团分销 只可以 得到一家商户授权" });
                    }
                }


                var pro = AgentCompanyData.AdminRegiUpCompany(regiinfo);
                if (pro == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "账户错误" });
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = "OK" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 修改分销商登陆信息
        public static string RegiUpAccount(Agent_regiinfo regiinfo)
        {
            try
            {

                var pro = AgentCompanyData.GetAgentAccountByUid(regiinfo.Account, regiinfo.Agentid);
                if (pro == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "账户错误" });
                }
                //得到账户id
                regiinfo.Id = pro.Id;

                var uppro = AgentCompanyData.RegiUpAccount(regiinfo);

                if (uppro == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "账户错误" });
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = "OK" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 管理员修改分销商登陆信息
        public static string ManageUpAccount(Agent_regiinfo regiinfo)
        {
            try
            {

                var uppro = AgentCompanyData.ManageUpAccount(regiinfo);


                if (uppro == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "账户错误" });
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = "OK" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 获取分销登陆账户信息
        public static string GetAgentAccountByUid(string account, int agentid)
        {
            try
            {
                var pro = AgentCompanyData.GetAgentAccountByUid(account, agentid);
                if (pro == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "账户错误" });
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 获取分销登陆账户信息
        public static string GetManageAgentAccountByUid(int id, int loginagentid)
        {
            try
            {
                var pro = AgentCompanyData.GetManageAgentAccountByUid(id);

                if (pro.Agentid != loginagentid)
                {

                    return JsonConvert.SerializeObject(new { type = 1, msg = "权限错误，请重新登陆后再次尝试操作！" });
                }

                if (pro == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "账户错误" });
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 获取分销账户信息
        public static string GetAgentByid(int agentid)
        {
            try
            {
                Agent_company pro = AgentCompanyData.GetAgentByid(agentid);
                if (pro == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "没有查询到账户" });
                }

                if (pro.Agent_type == 3) {
                    pro.ismeituan = 3;
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 授权列表
        public static string Warrantpagelist(int Pageindex, int Pagesize, string Key, int Agentid)
        {
            int totalcount = 0;
            DateTime ViewEndtime = DateTime.Now.AddMonths(-1);


            try
            {
                List<Agent_warrant> list = new List<Agent_warrant>();

                //if (Pageindex == 1)
                //{
                string comstate = "1";//公司状态

                //调整供应商列表位置，把106放在最前方
                string containcomids = "106";//包含的商家id列表
                List<Agent_warrant> aheadlist = new AgentCompanyData().GetWarrantlist(Key, Agentid, comstate, containcomids);

                string offcomids = "106";//去除的商家id列表
                List<Agent_warrant> behindlist = new AgentCompanyData().Warrantpagelist(Pageindex, Pagesize, Key, Agentid, out totalcount, comstate, offcomids);

                if (aheadlist.Count > 0 && behindlist.Count > 0)
                {
                    foreach (Agent_warrant mw in aheadlist)
                    {
                        list.Add(mw);
                    }
                    foreach (Agent_warrant mw in behindlist)
                    {
                        list.Add(mw);
                    }
                }
                else if (aheadlist.Count > 0 && behindlist.Count == 0)
                {
                    list = aheadlist;
                }
                else if (aheadlist.Count == 0 && behindlist.Count > 0)
                {
                    list = behindlist;
                }

                totalcount = totalcount + aheadlist.Count;
                //}
                //else 
                //{
                //    string comstate = "1";//公司状态
                //    string offcomids = "106";//去除的商家id列表
                //    List<Agent_warrant> behindlist = new AgentCompanyData().Warrantpagelist(Pageindex, Pagesize, Key, Agentid, out totalcount, comstate, offcomids);

                //    list = behindlist;
                //}


                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Agentid = pro.Agentid,
                                 Comid = pro.Comid,
                                 Comname = B2bCompanyData.GetCompany(pro.Comid).Com_name,
                                 Cominfo = new B2bCompanyInfoData().GetCompanyInfo(pro.Comid),
                                 Countpro = new B2bCompanyInfoData().GetCompanyProCount(pro.Comid),
                                 Warrant_state = pro.Warrant_state == 1 ? "开通" : "关闭",
                                 Warrant_type = pro.Warrant_type == 1 ? "出票扣款" : "验证扣款",
                                 Warrant_level = pro.Warrant_level,
                                 Imprest = pro.Imprest,
                                 Credit = pro.Credit,
                                 MessageNew = new B2b_comagentmessageData().AgentMessageNew(pro.Agentid, pro.Comid, ViewEndtime),
                                 ComPhoneLogo = FileSerivce.GetImgUrl(B2bCompanyData.GetComPhoneLogo(pro.Comid).ConvertTo<int>(0)),
                                 ComWebLogo = FileSerivce.GetImgUrl(B2bCompanyData.GetComWebLogo(pro.Comid).ConvertTo<int>(0)),
                                 Topimage = FileSerivce.GetImgUrl(new B2bComProData().GetTopProImageById(pro.Comid)),
                                 ComState = B2bCompanyData.GetCompany(pro.Comid).Com_state
                             };
                    return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 100, totalCount = 0, msg = result });
                }

            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        public static int CompareDinosByLength(string x, string y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater. 
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the 
                    // lengths of the two strings.
                    //
                    int retval = x.Length.CompareTo(y.Length);

                    if (retval != 0)
                    {
                        // If the strings are not of equal length,
                        // the longer string is greater.
                        //
                        return retval;
                    }
                    else
                    {
                        // If the strings are of equal length,
                        // sort them with ordinary string comparison.
                        //
                        return x.CompareTo(y);
                    }
                }
            }
        }


        #region 更多供应商列表
        public static string UnWarrantpagelist(int Pageindex, int Pagesize, string Key, int Agentid)
        {
            int totalcount = 0;
            DateTime ViewEndtime = DateTime.Now.AddMonths(-1);

            try
            {
                List<B2b_company> list = new List<B2b_company>();

                //if (Pageindex == 1)
                //{ 
                //调整供应商列表位置，把106放在最前方
                string containcomids = "106";//包含商家id列表
                List<B2b_company> aheadlist = new AgentCompanyData().UnWarrantlist(Key, Agentid, containcomids);

                string offcomids = "106";//不包含商家id列表
                List<B2b_company> behindlist = new AgentCompanyData().UnWarrantpagelist(Pageindex, Pagesize, Key, Agentid, out totalcount, offcomids);


                if (aheadlist.Count > 0 && behindlist.Count > 0)
                {
                    foreach (B2b_company mw in aheadlist)
                    {
                        list.Add(mw);
                    }
                    foreach (B2b_company mw in behindlist)
                    {
                        list.Add(mw);
                    }
                }
                else if (aheadlist.Count > 0 && behindlist.Count == 0)
                {
                    list = aheadlist;
                }
                else if (aheadlist.Count == 0 && behindlist.Count > 0)
                {
                    list = behindlist;
                }
                totalcount = totalcount + aheadlist.Count;
                //}
                //else
                //{
                //    string offcomids = "106";//不包含商家id列表
                //    List<B2b_company> behindlist = new AgentCompanyData().UnWarrantpagelist(Pageindex, Pagesize, Key, Agentid, out totalcount, offcomids);

                //    list = behindlist;
                //}

                //if (list != null)
                //{ //先有小到大，这样 一般 106都会排在前面

                //    list.Sort(delegate(B2b_company left, B2b_company right)
                //    {
                //        return left.ID - right.ID;
                //    });
                //} 

                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.ID,
                                 Com_name = pro.Com_name,
                                 Companyinfo = B2bCompanyData.GetB2bcompanyinfo(pro.ID),
                                 Countpro = new B2bCompanyInfoData().GetCompanyProCount(pro.ID),
                                 Countchengjiao = new B2bCompanyInfoData().GetCompanyChengjiaoCount(pro.ID),
                                 Countweekchengjiao = new B2bCompanyInfoData().GetCompanyWeekChengjiaoCount(pro.ID),
                             };
                }
                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion



        #region  //微站分销列表，分销商看到的
        public static string AgentComlist(int Pageindex, int Pagesize, string Key, int Agentid)
        {
            int totalcount = 0;
            try
            {
                AgentCompanyData agentcom = new AgentCompanyData();
                B2bCompanyInfoData comdata = new B2bCompanyInfoData();

                var list = agentcom.AgentComlist(Pageindex, Pagesize, Key, Agentid, out totalcount);
                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.ID,
                                 Agentid = pro.Agentid,
                                 Comid = pro.ID,
                                 Comname = pro.Com_name,
                                 Com_state = pro.Com_state == 1 ? "开通" : "关闭",
                                 Agentopenstate = pro.Agentopenstate,
                                 OpenDate = pro.OpenDate,
                                 EndData = pro.EndDate,
                                 Accountinfo = B2bCompanyManagerUserData.GetFirstAccountUser(pro.ID),
                                 HasInnerChannel = comdata.GetCompanyInfo(pro.ID).HasInnerChannel == true ? "连锁" : "商户",

                             };
                }
                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 分销账户列表
        public static string Accountlist(int Pageindex, int Pagesize, int Agentid, int accountid = 0)
        {
            int totalcount = 0;
            try
            {
                AgentCompanyData agentcom = new AgentCompanyData();

                var list = agentcom.Accountlist(Pageindex, Pagesize, Agentid, out totalcount, accountid);

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = list });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 授权产品列表
        public static string WarrantProjectlist(int Pageindex, int Pagesize, string Key, int Agentid, int comid)
        {
            int totalcount = 0;
            try
            {
                var prodata = new B2bComProData();
                int Warrant_type = 0;
                Agent_companyData agentcom = new Agent_companyData();



                int Agentlevel = 3;
                var agentmodel = AgentCompanyData.GetAgentWarrant(Agentid, comid);
                if (agentmodel != null)
                {
                    Agentlevel = agentmodel.Warrant_level;
                    Warrant_type = agentmodel.Warrant_type;
                }


                var list = agentcom.WarrantProjectlist(Pageindex, Pagesize, Key, Agentid, Warrant_type, comid, out totalcount, Agentlevel);

                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 pro.Id,
                                 pro.Projectname,
                                 Projectimg = GetProjectImg(pro.Projectimg, pro.Id),
                                 pro.Province,
                                 pro.City,
                                 pro.Industryid,
                                 pro.Briefintroduce,
                                 pro.Address,
                                 pro.Mobile,
                                 pro.Coordinate,
                                 pro.Serviceintroduce,
                                 pro.Onlinestate,
                                 pro.Comid,
                                 pro.Createtime,
                                 pro.Createuserid
                             };
                }



                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        private static string GetProjectImg(int projectimg, int projectid)
        {
            if (projectimg == 0 || projectimg == 3962)
            {
                //得到项目下第一个在线产品图片做为项目图片
                int firstimg = new B2bComProData().GetFirstProImgInProjectId(projectid);
                return FileSerivce.GetImgUrl(firstimg);
            }
            else
            {
                return FileSerivce.GetImgUrl(projectimg);
            }

        }
        #endregion

        #region 授权产品列表
        public static string WarrantProlist(int Pageindex, int Pagesize, string Key, int Agentid, int comid, int projectid = 0, string viewmethod = "")
        {
            int totalcount = 0;
            try
            {
                var prodata = new B2bComProData();
                int Warrant_type = 0;
                Agent_companyData agentcom = new Agent_companyData();



                int Agentlevel = 3;
                var agentmodel = AgentCompanyData.GetAgentWarrant(Agentid, comid);
                if (agentmodel != null)
                {
                    Agentlevel = agentmodel.Warrant_level;
                    Warrant_type = agentmodel.Warrant_type;
                }


                var list = agentcom.WarrantProlist(Pageindex, Pagesize, Key, Agentid, Warrant_type, comid, out totalcount, Agentlevel, projectid, viewmethod);
                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Pro_name = pro.Pro_name,
                                 Face_price = pro.Face_price,
                                 Advise_price = pro.Advise_price,
                                 Agentsettle_price = pro.Agentsettle_price,
                                 Pro_end = pro.Pro_end,
                                 Pro_start = pro.Pro_start,

                                 Source_type = pro.Source_type == 1 ? "自动生成" : pro.Source_type == 2 ? "倒码" : pro.Source_type == 4 ? "分销导入" : "外部接口",
                                 Pro_state = pro.Pro_state == 0 ? "下线" : "上线",
                                 Service_Contain = pro.Service_Contain.Replace((char)13, (char)0).Replace((char)10, (char)0),
                                 Service_NotContain = pro.Service_NotContain.Replace((char)13, (char)0).Replace((char)10, (char)0),
                                 Precautions = pro.Precautions.Replace((char)13, (char)0).Replace((char)10, (char)0),
                                 Pro_Remark = pro.Pro_Remark.Replace((char)13, (char)0).Replace((char)10, (char)0),
                                 Pro_explain = pro.Pro_explain.Replace((char)13, (char)0).Replace((char)10, (char)0),
                                 Agent_price = GetAgentPrice(Agentlevel, pro.Id, pro.Server_type, pro.Agent1_price, pro.Agent2_price, pro.Agent3_price, pro.Advise_price),
                                 Count_Num = prodata.ProSEPageCount(pro.Id),
                                 Use_Num = prodata.ProSEPageCount_Use(pro.Id),
                                 UnUse_Num = prodata.ProSEPageCount_UNUse(pro.Id),
                                 Invalid_Num = prodata.ProSEPageCount_Con(pro.Id),
                                 Projectname = new B2b_com_projectData().GetProjectNameByid(pro.Projectid),
                                 Pro_youxiaoqi = pro.Pro_youxiaoqi,
                                 ProValidateMethod = pro.ProValidateMethod,
                                 Appointdata = pro.Appointdata,
                                 Iscanuseonsameday = pro.Iscanuseonsameday,
                                 binding_Warrant_type = AgentCompanyData.GetAgentWarranttypebyproid(pro.Id, pro.Com_id),
                                 ProImg = FileSerivce.GetImgUrl(pro.Imgurl),
                                 IsViewStockNum = GetIsViewStockNum(pro.Id, pro.Ispanicbuy, pro.Server_type),
                                 StockNum = GetStockNum(pro.Id, pro.Ispanicbuy, pro.Server_type),
                                 Ispanicbuy = pro.Ispanicbuy,
                                 Server_type = pro.Server_type,
                                 Bindingid = pro.Bindingid,
                                 Manyspeci = pro.Manyspeci,
                                 BindingSource_type = pro.Source_type == 4 ? prodata.GetSourcetypebyproid(pro.Bindingid).ToString() : "",
                                 GuigeList = pro.Manyspeci == 0 ? null : new B2b_com_pro_SpeciData().AgentGetgglist(pro.Id, Agentlevel)
                             };
                }
                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
        #region 授权产品列表
        public static string AllWarrantProlist(int Pageindex, int Pagesize, string Key, int Agentid, string viewmethod = "", int servertype = 0)
        {
            int totalcount = 0;
            try
            {

                var list = new Agent_companyData().AllWarrantProlist(Pageindex, Pagesize, Key, Agentid, out totalcount, viewmethod, servertype);
                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Pro_name = pro.Pro_name,
                                 Face_price = pro.Face_price,
                                 Advise_price = pro.Advise_price,
                                 Agentsettle_price = pro.Agentsettle_price,
                                 Pro_end = pro.Pro_end,
                                 Pro_start = pro.Pro_start,

                                 Source_type = pro.Source_type == 1 ? "自动生成" : pro.Source_type == 2 ? "倒码" : pro.Source_type == 4 ? "分销导入" : "外部接口",
                                 Pro_state = pro.Pro_state == 0 ? "下线" : "上线",
                                 Service_Contain = pro.Service_Contain.Replace((char)13, (char)0).Replace((char)10, (char)0),
                                 Service_NotContain = pro.Service_NotContain.Replace((char)13, (char)0).Replace((char)10, (char)0),
                                 Precautions = pro.Precautions.Replace((char)13, (char)0).Replace((char)10, (char)0),
                                 Pro_Remark = pro.Pro_Remark.Replace((char)13, (char)0).Replace((char)10, (char)0),
                                 Pro_explain = pro.Pro_explain.Replace((char)13, (char)0).Replace((char)10, (char)0),
                                 //Agent_price = GetAgentPrice(Agentlevel, pro.Id, pro.Server_type, pro.Agent1_price, pro.Agent2_price, pro.Agent3_price),
                                 Agent_price = GetAgentPrice(Agentid, pro.Com_id, pro.Id, pro.Server_type, pro.Agent1_price, pro.Agent2_price, pro.Agent3_price, pro.Advise_price),
                                 Count_Num = new B2bComProData().ProSEPageCount(pro.Id),
                                 Use_Num = new B2bComProData().ProSEPageCount_Use(pro.Id),
                                 UnUse_Num = new B2bComProData().ProSEPageCount_UNUse(pro.Id),
                                 Invalid_Num = new B2bComProData().ProSEPageCount_Con(pro.Id),
                                 Projectname = new B2b_com_projectData().GetProjectNameByid(pro.Projectid),
                                 Pro_youxiaoqi = pro.Pro_youxiaoqi,
                                 ProValidateMethod = pro.ProValidateMethod,
                                 Appointdata = pro.Appointdata,
                                 Iscanuseonsameday = pro.Iscanuseonsameday,
                                 binding_Warrant_type = AgentCompanyData.GetAgentWarranttypebyproid(pro.Id, pro.Com_id),
                                 ProImg = FileSerivce.GetImgUrl(pro.Imgurl),
                                 IsViewStockNum = GetIsViewStockNum(pro.Id, pro.Ispanicbuy, pro.Server_type),
                                 StockNum = GetStockNum(pro.Id, pro.Ispanicbuy, pro.Server_type),
                                 Ispanicbuy = pro.Ispanicbuy,
                                 Server_type = pro.Server_type,
                                 Bindingid = pro.Bindingid,
                                 BindingSource_type = pro.Source_type == 4 ? new B2bComProData().GetSourcetypebyproid(pro.Bindingid).ToString() : "",
                                 Warrant_type = new AgentCompanyData().GetWarrant_type(Agentid, pro.Com_id),
                                 Comname = new B2bCompanyData().GetCompanyNameById(pro.Com_id),
                                 Manyspeci = pro.Manyspeci,
                                 GuigeList = pro.Manyspeci == 0 ? null : new B2b_com_pro_SpeciData().AgentGetgglist(pro.Id, AgentCompanyData.GetAgentWarrantAgentlevel(Agentid, pro.Com_id))
                             };
                }
                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        #endregion

        #region 授权产品列表
        public static string UnWarrantProlist(int Pageindex, int Pagesize, string Key, int Agentid, int comid, int projectid = 0, string viewmethod = "")
        {
            int totalcount = 0;
            try
            {
                var prodata = new B2bComProData();
                int Warrant_type = 0;
                int lp = 0;
                int Agentlevel = 3;
                Agent_companyData agentcom = new Agent_companyData();


                var cominfo = B2bCompanyData.GetAllComMsg(comid);
                if (cominfo != null)
                {
                    lp = cominfo.Lp;
                    Agentlevel = cominfo.Lp_agentlevel;
                }


                var list = agentcom.UnWarrantProlist(Pageindex, Pagesize, Key, Agentid, Warrant_type, comid, out totalcount, Agentlevel, projectid, viewmethod);
                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Pro_name = pro.Pro_name.Length > 30 ? pro.Pro_name.Substring(0, 30) : pro.Pro_name,
                                 Face_price = pro.Face_price,
                                 Advise_price = pro.Advise_price,
                                 Agentsettle_price = pro.Agentsettle_price,
                                 Pro_end = pro.Pro_end,
                                 Pro_start = pro.Pro_start,
                                 Source_type = pro.Source_type == 1 ? "自动生成" : "倒码",
                                 Pro_state = pro.Pro_state == 0 ? "下线" : "上线",
                                 Service_Contain = pro.Service_Contain.Replace((char)13, (char)0).Replace((char)10, (char)0),
                                 Service_NotContain = pro.Service_NotContain.Replace((char)13, (char)0).Replace((char)10, (char)0),
                                 Precautions = pro.Precautions.Replace((char)13, (char)0).Replace((char)10, (char)0),
                                 Pro_Remark = pro.Pro_Remark.Replace((char)13, (char)0).Replace((char)10, (char)0),
                                 Pro_explain = pro.Pro_explain.Replace((char)13, (char)0).Replace((char)10, (char)0),
                                 Agent_price = GetAgentPrice(Agentlevel, pro.Id, pro.Server_type, pro.Agent1_price, pro.Agent2_price, pro.Agent3_price),
                                 Count_Num = prodata.ProSEPageCount(pro.Id),
                                 Use_Num = prodata.ProSEPageCount_Use(pro.Id),
                                 UnUse_Num = prodata.ProSEPageCount_UNUse(pro.Id),
                                 Invalid_Num = prodata.ProSEPageCount_Con(pro.Id),
                                 Projectname = new B2b_com_projectData().GetProjectNameByid(pro.Projectid),
                                 Pro_youxiaoqi = pro.Pro_youxiaoqi,
                                 ProValidateMethod = pro.ProValidateMethod,
                                 Appointdata = pro.Appointdata,
                                 Iscanuseonsameday = pro.Iscanuseonsameday,
                                 binding_Warrant_type = AgentCompanyData.GetAgentWarranttypebyproid(pro.Id, pro.Com_id),
                                 ProImg = FileSerivce.GetImgUrl(pro.Imgurl),
                                 IsViewStockNum = GetIsViewStockNum(pro.Id, pro.Ispanicbuy, pro.Server_type),
                                 StockNum = GetStockNum(pro.Id, pro.Ispanicbuy, pro.Server_type),
                                 Ispanicbuy = pro.Ispanicbuy,
                                 Server_type = pro.Server_type,
                             };
                }
                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
        public static decimal GetAgentPrice(int Agentid, int comid, int proid, int servertype, decimal Agent1_price, decimal Agent2_price, decimal Agent3_price, decimal Advise_price = 0)
        {
            Agent_warrant warrant = new Agent_companyData().GetAgentWarrant(Agentid, comid);
            if (warrant != null)
            {
                int Agentlevel = warrant.Warrant_level;
                //票务；实物；大巴 查询分销价格
                if (servertype == 1 || servertype == 11 || servertype == 10)
                {
                    if (Agentlevel == 1)
                    {
                        return Agent1_price;
                    }
                    else if (Agentlevel == 2)
                    {
                        return Agent2_price;
                    }
                    else if (Agentlevel == 3)
                    {
                        return Agent3_price;
                    }
                    else
                    {
                        return 0;
                    }

                }
                //当地游；跟团游;酒店  查询最小价格
                else if (servertype == 2 || servertype == 8)
                {
                    decimal differ = 0;//差价
                    if (Agentlevel == 1)
                    {
                        differ = Agent1_price;
                    }
                    else if (Agentlevel == 2)
                    {
                        differ = Agent2_price;
                    }
                    else if (Agentlevel == 3)
                    {
                        differ = Agent3_price;
                    }
                    else
                    {
                        return 0;
                    }

                    decimal minprice = new B2b_com_LineGroupDateData().GetMinValidePrice(proid);
                    decimal agentprice = minprice - differ;
                    if (agentprice <= 0)
                    {
                        agentprice = 0;
                    }
                    return agentprice;
                }
                else if (servertype == 9)
                {
                    decimal differ = 0;//差价
                    if (Agentlevel == 1)
                    {
                        differ = Agent1_price;
                    }
                    else if (Agentlevel == 2)
                    {
                        differ = Agent2_price;
                    }
                    else if (Agentlevel == 3)
                    {
                        differ = Agent3_price;
                    }
                    else
                    {
                        return 0;
                    }

                    differ = Advise_price - differ;

                    if (differ > 0)
                    {

                        return differ;
                    }
                    else
                    {
                        return 0;
                    }

                }
                //保险产品
                else if (servertype == 14)
                {
                    //查询第一个可用的分销价格
                    decimal agentprice = new B2b_com_pro_SpeciData().Gettop1availableprice(Agentlevel, proid);
                    return agentprice;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        private static decimal GetAgentPrice(int Agentlevel, int proid, int servertype, decimal Agent1_price, decimal Agent2_price, decimal Agent3_price, decimal Advise_price = 0)
        {
            //票务；实物；大巴 查询分销价格
            if (servertype == 1 || servertype == 11 || servertype == 10)
            {
                if (Agentlevel == 1)
                {
                    return Agent1_price;
                }
                else if (Agentlevel == 2)
                {
                    return Agent2_price;
                }
                else if (Agentlevel == 3)
                {
                    return Agent3_price;
                }
                else
                {
                    return 0;
                }

            }
            //当地游；跟团游;酒店  查询最小价格
            else if (servertype == 2 || servertype == 8)
            {
                decimal differ = 0;//差价
                if (Agentlevel == 1)
                {
                    differ = Agent1_price;
                }
                else if (Agentlevel == 2)
                {
                    differ = Agent2_price;
                }
                else if (Agentlevel == 3)
                {
                    differ = Agent3_price;
                }
                else
                {
                    return 0;
                }
                decimal minprice = new B2b_com_LineGroupDateData().GetMinValidePrice(proid);
                decimal agentprice = minprice - differ;
                if (agentprice <= 0)
                {
                    agentprice = 0;
                }
                return agentprice;
            }
            else if (servertype == 9)
            {
                decimal differ = 0;//差价
                if (Agentlevel == 1)
                {
                    differ = Agent1_price;
                }
                else if (Agentlevel == 2)
                {
                    differ = Agent2_price;
                }
                else if (Agentlevel == 3)
                {
                    differ = Agent3_price;
                }
                else
                {
                    return 0;
                }

                differ = Advise_price - differ;

                if (differ > 0)
                {

                    return differ;
                }
                else
                {
                    return 0;
                }

            }
            else
            {
                return 0;
            }
        }

        private static int GetIsViewStockNum(int proid, int ispanicbuy, int servertype)
        {
            if (servertype == 1 || servertype == 11)//票务产品 或者 实物产品
            {
                if (ispanicbuy == 1 || ispanicbuy == 2)//抢购或者限购
                {

                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            } throw new NotImplementedException();
        }
        #region
        public static int GetStockNum(int proid, int ispanicbuy, int servertype)
        {
            if (servertype == 1 || servertype == 11)//票务产品 或者 实物产品
            {
                if (ispanicbuy == 1 || ispanicbuy == 2)//抢购或者限购
                {
                    int cansellnum = 0;//可销售数量
                    int hassellnum = 0;//已销售数量

                    //读取产品，如果产品为绑定产品则 提起原产品的 限购数量
                    var prodata = new B2bComProData();
                    var proinfo = prodata.GetProById(proid.ToString());
                    if (proinfo == null)
                    {
                        return 0;
                    }
                    if (proinfo.Bindingid != 0)
                    {
                        proid = proinfo.Bindingid;
                    }

                    new B2bComProData().Getsalenum(proid, out cansellnum, out hassellnum);
                    return cansellnum;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        #endregion


        #region 授权产品列表
        public static string BindingWarrantProlist(int Pageindex, int Pagesize, string Key, int Agentid, int bindingcomid, int comid)
        {
            int totalcount = 0;
            try
            {
                var prodata = new B2bComProData();
                int Warrant_type = 0;
                Agent_companyData agentcom = new Agent_companyData();



                int Agentlevel = 3;
                var agentmodel = AgentCompanyData.GetAgentWarrant(Agentid, bindingcomid);
                if (agentmodel != null)
                {
                    Agentlevel = agentmodel.Warrant_level;
                    Warrant_type = agentmodel.Warrant_type;
                }


                var list = agentcom.BindingWarrantProlist(Pageindex, Pagesize, Key, Agentid, Warrant_type, bindingcomid, comid, out totalcount, Agentlevel);
                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Pro_name = pro.Pro_name.Length > 18 ? pro.Pro_name.Substring(0, 18) : pro.Pro_name,
                                 Face_price = pro.Face_price,
                                 Advise_price = pro.Advise_price,
                                 Agentsettle_price = pro.Agentsettle_price,

                                 Pro_end = pro.Pro_end,
                                 Pro_start = pro.Pro_start,
                                 Source_type = pro.Source_type == 1 ? "自动生成" : "倒码",
                                 Pro_state = pro.Pro_state == 0 ? "下线" : "上线",
                                 Service_Contain = pro.Service_Contain,
                                 Service_NotContain = pro.Service_NotContain,
                                 Precautions = pro.Precautions,
                                 Pro_Remark = pro.Pro_Remark,
                                 Pro_explain = pro.Pro_explain,
                                 Agent_price = GetAgentPrice(Agentid, bindingcomid, pro.Id, pro.Server_type, pro.Agent1_price, pro.Agent2_price, pro.Agent3_price, pro.Advise_price),
                                 Count_Num = prodata.ProSEPageCount(pro.Id),
                                 Use_Num = prodata.ProSEPageCount_Use(pro.Id),
                                 UnUse_Num = prodata.ProSEPageCount_UNUse(pro.Id),
                                 Invalid_Num = prodata.ProSEPageCount_Con(pro.Id),
                                 Projectname = new B2b_com_projectData().GetProjectNameByid(pro.Projectid),
                                 Pro_youxiaoqi = pro.Pro_youxiaoqi,
                                 ProValidateMethod = pro.ProValidateMethod,
                                 Appointdata = pro.Appointdata,
                                 Iscanuseonsameday = pro.Iscanuseonsameday,

                             };
                }
                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion



        #region 倒入产品bindingcomid 产品提供方ID
        public static string ImprestPro(string proid, int bindingcomid)
        {
            int totalcount = 0;
            int Agentid = 0;
            int comid = 0;
            try
            {
                var prodata = new B2bComProData();
                int Warrant_type = 0;
                Agent_companyData agentcom = new Agent_companyData();
                B2b_company company = UserHelper.CurrentCompany;

                comid = company.ID;

                //对商户判断不能把自己的产品
                if (comid == bindingcomid)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "导入失败，自己的产品不能再次导入" });
                }

                var comdata = B2bCompanyData.GetCompany(company.ID);
                if (comdata != null)
                {
                    Agentid = comdata.Bindingagent;
                }


                int Agentlevel = 3;
                var agentmodel = AgentCompanyData.GetAgentWarrant(Agentid, bindingcomid);
                if (agentmodel != null)
                {
                    Agentlevel = agentmodel.Warrant_level;
                    Warrant_type = agentmodel.Warrant_type;
                }
                if (agentmodel == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "导入失败，您未授权" });
                }

                if (proid == "")
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "导入失败，没有选择产品" });
                }
                //如果结尾是，则去掉
                proid = proid.Remove(proid.LastIndexOf(","), 1);


                var proid_arr = proid.Split(',');
                for (int i = 0; i < proid_arr.Count(); i++)
                {

                    //插入产品
                    var impstate = Agent_companyData.ImprestPro(comid, int.Parse(proid_arr[i]), Agentlevel);

                    //对插入产品和插入的项目进行关联
                    if (impstate != 0)
                    {

                        //查询原来产品信息
                        var new_proid = Agent_companyData.SearchImprestNewProid(comid, int.Parse(proid_arr[i]));
                        if (new_proid != 0)
                        {
                            var new_projectid = Agent_companyData.SearchImprestNewProjectid(comid, int.Parse(proid_arr[i]));

                            if (new_projectid != 0)
                            {
                                //新产品，新项目ID
                                var upimpstate = Agent_companyData.UpImprestPro(comid, new_proid, new_projectid);
                            }
                        }

                        //检查产品是否为多规格产品，多规格进行多规格导入
                        if (new_proid != 0)
                        {
                            var b2bprodata = new B2bComProData();
                            var b2bproinfo = b2bprodata.GetProById(new_proid.ToString());
                            if (b2bproinfo != null)
                            {
                                if (b2bproinfo.Manyspeci == 1)
                                { //如果是多规格产品，下面导入多规格产品

                                    //导入规格
                                    var impguige = Agent_companyData.ImprestProGuige(comid, new_proid, int.Parse(proid_arr[i]), Agentlevel);

                                }
                            }


                        }



                    }

                }

                return JsonConvert.SerializeObject(new { type = 100, msg = "导入完成" });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 分销商列表
        public static string Agentpagelist(int Pageindex, int Pagesize, string Key, int comid, string com_province = "", string com_city = "", string warrantstate = "0,1", int agentsourcesort = 0, int projectid = 0, string startime = "", string endtime = "")
        {
            int totalcount = 0;
            try
            {
                string endtime_temp = "";
                if (endtime != "")
                {
                    endtime_temp = DateTime.Parse(endtime).AddDays(1).ToString("yyyy-MM-dd");
                }


                IEnumerable result1 = "";

                AgentCompanyData agentcom = new AgentCompanyData();
                IEnumerable result = "";
                var list = agentcom.Agentpagelist(Pageindex, Pagesize, Key, comid, out totalcount, com_province, com_city, warrantstate, agentsourcesort, "Imprest");

                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Agentid = pro.Agentid,
                                 Agentname = AgentCompanyData.GetAgentWarrant(pro.Agentid, comid).Company,
                                 Name = AgentCompanyData.GetAgentWarrant(pro.Agentid, comid).Name,
                                 Hand_Imprest = AgentCompanyData.Hand_Imprest(pro.Agentid, comid, startime, endtime_temp),//后台充入预付款

                                 HandOut_Imprest = AgentCompanyData.HandOut_Imprest(pro.Agentid, comid, startime, endtime_temp),//分销后台扣预付款
                                 Hand_Rebate = AgentCompanyData.Hand_Rebate(pro.Agentid, comid, startime, endtime_temp),//后台充入返点
                                 Line_Imprest = AgentCompanyData.Line_Imprest(pro.Agentid, comid, startime, endtime_temp),//网上支付预付款

                                 Sale_price = AgentCompanyData.Sale_price(pro.Agentid, comid, projectid, startime, endtime_temp),//销售额
                                 Maoli_price = AgentCompanyData.Maoli_price(pro.Agentid, comid, projectid, startime, endtime_temp),//毛利
                                 Xiaofei_price = Math.Abs(AgentCompanyData.Xiaofei_price(pro.Agentid, comid, projectid, startime, endtime_temp)),// + Math.Abs(AgentCompanyData.AgentDaoru_Xiaofei_price(pro.Agentid, comid, projectid, startime, endtime)),//已消费
                                 Chendian_price = AgentCompanyData.Chendian_price(pro.Agentid, comid, projectid, startime, endtime_temp),// + AgentCompanyData.AgentDaoru_Chendian_price(pro.Agentid, comid, projectid, startime, endtime),//沉淀
                                 Daoma_Sale_price = AgentCompanyData.Daoma_Sale_price(pro.Agentid, comid, projectid, startime, endtime_temp),//销售额
                                 Daoma_Maoli_price = AgentCompanyData.Daoma_Maoli_price(pro.Agentid, comid, projectid, startime, endtime_temp),//毛利
                                 // Daoma_Xiaofei_price = AgentCompanyData.Daoma_Xiaofei_price(pro.Agentid, comid),//已消费

                                 Comid = pro.Comid,
                                 Warrant_state = pro.Warrant_state == 1 ? "开通" : pro.Warrant_state == 0 ? "关闭" : "未授权",
                                 Warrant_type = pro.Warrant_type == 1 ? "出票扣款" : "验证扣款",
                                 Warrant_level = pro.Warrant_level,
                                 Imprest = pro.Imprest,//余额
                                 Credit = pro.Credit,
                                 com_province = AgentCompanyData.GetAgentComProvince(pro.Agentid),
                                 com_city = AgentCompanyData.GetAgentComCity(pro.Agentid),
                                 Warrant_lp = pro.Warrant_lp
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result, msg1 = result1 });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 分销商出票及验票情况
        public static string Agentoutticketlist(int Pageindex, int Pagesize, string Key, int comid, string warrantstate = "0,1", int projectid = 0, string startime = "", string endtime = "")
        {
            int totalcount = 0;
            try
            {
                string endtime_temp = "";
                if (endtime != "")
                {
                    endtime_temp = DateTime.Parse(endtime).AddDays(1).ToString("yyyy-MM-dd");
                }


                IEnumerable result1 = "";

                AgentCompanyData agentcom = new AgentCompanyData();
                IEnumerable result = "";
                var list = agentcom.Agentpagelist(Pageindex, Pagesize, Key, comid, out totalcount, "", "", warrantstate, 0, "Imprest");

                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Agentid = pro.Agentid,
                                 startime=startime,
                                 endtime=endtime,
                                 AgentWarrant = AgentCompanyData.GetAgentWarrant(pro.Agentid, comid),//授权公司信息
                                 Outticketnum = AgentCompanyData.Outticketnum(pro.Agentid, comid, projectid, startime, endtime_temp),//销售情况
                                 Yanzhengticketnum = AgentCompanyData.Yanzhengticketnum(pro.Agentid, comid, projectid, startime, endtime_temp),//验票情况
                                 Comid = pro.Comid,
                             
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result, msg1 = result1 });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 分销商列表
        public static string AgentSetWarrantpagelist(int Pageindex, int Pagesize, int proid, int comid)
        {
            int totalcount = 0;
            try
            {

                AgentCompanyData agentcom = new AgentCompanyData();
                IEnumerable result = "";
                var list = agentcom.Agentpagelist(Pageindex, Pagesize, "", comid, out totalcount, "", "", "1", 0);

                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Agentid = pro.Agentid,
                                 Agentname = AgentCompanyData.GetAgentWarrant(pro.Agentid, comid).Company,
                                 Comid = pro.Comid,
                                 Warrant_state = pro.Warrant_state == 1 ? "开通" : "关闭",
                                 Warrant_type = pro.Warrant_type == 1 ? "出票扣款" : "验证扣款",
                                 Warrant_level = pro.Warrant_level,
                                 Imprest = pro.Imprest,//余额
                                 Credit = pro.Credit,
                                 SetWarrant = agentcom.SearchSetWarrant(pro.Agentid, proid, comid)
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 特别授权
        public static string SetWarrant(int agentid, int type, int proid, int comid)
        {
            int totalcount = 0;
            try
            {

                if (agentid == 0 || proid == 0 || comid == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "参数传递错误" });
                }

                AgentCompanyData agentcom = new AgentCompanyData();

                var list = agentcom.SetWarrant(agentid, type, proid, comid);

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = list });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 对易城分销是否授权
        public static string EtownAgentWarrant(int comid)
        {
            int totalcount = 0;
            try
            {
                string result = "";

                AgentCompanyData agentcom = new AgentCompanyData();
                // 易城商户分销=9
                var etownWarrant = agentcom.GetAgent_Warrant(9, comid);
                if (etownWarrant == null)
                {

                }
                else
                {
                    if (etownWarrant.Warrant_state == 1)
                    {
                        result = "OK";
                    }

                }



                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 所有分销商列表
        public static string ManageAgentpagelist(int Pageindex, int Pagesize, string Key, string com_province, string com_city, int agentsourcesort = 0)
        {
            int totalcount = 0;
            try
            {
                AgentCompanyData agentcom = new AgentCompanyData();

                var list = agentcom.ManageAgentpagelist(Pageindex, Pagesize, Key, out totalcount, com_province, com_city, agentsourcesort);
                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Company = pro.Company,
                                 Mobile = pro.Mobile,
                                 Run_state = pro.Run_state,
                                 Tel = pro.Tel,
                                 Name = pro.Contentname,
                                 Account = AgentCompanyData.GetAgentAccount(pro.Id),//获取分销商开户账户
                                 Agentsort = pro.Agentsort,
                                 Agentsortvalue = GetAgentsortvalueBykey(pro.Agentsort),
                                 Agent_domain = pro.Agent_domain,
                                 Weixin_img = pro.Weixin_img,
                                 Agent_messagesetting = pro.Agent_messagesetting,
                                 pro.agentsourcesort,
                                 Agentsourcesortvalue = GetAgentsourcesortvalueBykey(pro.agentsourcesort),
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        private static string GetAgentsourcesortvalueBykey(int agentsourcesortkey)
        {
            return EnumUtils.GetName((Agentsourcesort)agentsourcesortkey);
        }

        private static string GetAgentsortvalueBykey(int agentsortkey)
        {
            return EnumUtils.GetName((Agentsort)agentsortkey);
        }
        #endregion

        #region 查询未授权分销商列表
        public static string Unagentlist(int Pageindex, int Pagesize, string Key, int comid)
        {
            int totalcount = 0;
            try
            {
                AgentCompanyData agentcom = new AgentCompanyData();

                var list = agentcom.Unagentlist(Pageindex, Pagesize, Key, comid, out totalcount);
                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Company = pro.Company,
                                 //Name = pro.Contentname,
                                 //Tel = pro.Tel,
                                 //Mobile = pro.Mobile
                                 Com_province = pro.Com_province,
                                 Com_city = pro.Com_city,
                                 Agent_sourcesort = pro.Agent_sourcesort,
                                 Sale = AgentCompanyData.Agent_AllSale(pro.Id),

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 分销商列表
        public static string AgentFinacelist(int Pageindex, int Pagesize, int agentid, int comid, int recharge = 0)
        {
            int totalcount = 0;
            try
            {
                AgentCompanyData agentcom = new AgentCompanyData();
                B2bOrderData orderdata = new B2bOrderData();
                B2bPayData paydata = new B2bPayData();

                var list = agentcom.AgentFinacelist(Pageindex, Pagesize, agentid, comid, out totalcount, recharge);



                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Agentid = pro.Agentid,
                                 Warrantid = pro.Warrantid,
                                 Com_id = pro.Com_id,
                                 Order_id = pro.Order_id,
                                 PayNo = paydata.GetPayByoId(pro.Order_id),
                                 Servicesname = pro.Servicesname,
                                 Money = pro.Money,
                                 Over_money = pro.Over_money,
                                 Payment_type = pro.Payment_type,
                                 Payment = pro.Payment,
                                 Remarks = pro.Remarks,
                                 Subdate = pro.Subdate,
                                 pno = orderdata.GetPnoByOrderId(pro.Order_id),
                                 Rebatetype = pro.Rebatetype
                             };





                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 指定分销项目列表
        public static string Projectlist(int Pageindex, int Pagesize, int agentid, string key = "")
        {
            int totalcount = 0;
            try
            {
                DateTime today_star = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                DateTime today = DateTime.Now;



                DateTime ymonth_star = DateTime.Parse(today.AddMonths(-1).ToString("yyyy-MM") + "-1");
                DateTime ymonth_ned = DateTime.Parse(ymonth_star.AddMonths(1).ToString("yyyy-MM-dd"));

                DateTime today_month_star = DateTime.Parse(today.ToString("yyyy-MM") + "-1");
                DateTime today_month_end = DateTime.Now;

                DateTime yday_star = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                DateTime yday_end = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));



                //AgentCompanyData agentcom = new AgentCompanyData();
                B2b_com_projectData projectdata = new B2b_com_projectData();
                var prodata = new B2bComProData();

                var list = projectdata.Projectlist(Pageindex, Pagesize, agentid, out totalcount, key);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {

                                 Id = pro.Id,
                                 Projectname = pro.Projectname,
                                 All_Use_pnum = prodata.ProYanzhengCountbyProjectid(pro.Comid, pro.Id, today, today, 1) + prodata.ProHotelYanzhengCountbyProjectid(pro.Comid, pro.Id, today, today, 1),
                                 Today_Use_pnum = prodata.ProYanzhengCountbyProjectid(pro.Comid, pro.Id, today_star, today, 0) + prodata.ProHotelYanzhengCountbyProjectid(pro.Comid, pro.Id, today_star, today, 0),
                                 Yday_Use_pnum = prodata.ProYanzhengCountbyProjectid(pro.Comid, pro.Id, yday_star, yday_end, 0) + prodata.ProHotelYanzhengCountbyProjectid(pro.Comid, pro.Id, yday_star, yday_end, 0),
                                 ToM_Use_pnum = prodata.ProYanzhengCountbyProjectid(pro.Comid, pro.Id, today_month_star, today_month_end, 0) + prodata.ProHotelYanzhengCountbyProjectid(pro.Comid, pro.Id, today_month_star, today_month_end, 0),
                                 YoM_Use_pnum = prodata.ProYanzhengCountbyProjectid(pro.Comid, pro.Id, ymonth_star, ymonth_ned, 0) + prodata.ProHotelYanzhengCountbyProjectid(pro.Comid, pro.Id, ymonth_star, ymonth_ned, 0),


                             };



                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
        #region 分销商列表
        public static string ProjectAgentlist(int Pageindex, int Pagesize, int comid, int projectid)
        {
            int totalcount = 0;
            try
            {
                //AgentCompanyData agentcom = new AgentCompanyData();
                B2b_com_projectData projectdata = new B2b_com_projectData();


                var list = projectdata.ProjectAgentlist(Pageindex, Pagesize, comid, projectid, out totalcount);
                IEnumerable result = "";
                if (list != null)
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Company = pro.Company,
                                 Mobile = pro.Mobile,
                                 Contentname = pro.Contentname,
                                 Account = AgentCompanyData.GetAgentAccount(pro.Id),//获取分销商开户账户

                             };


                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 项目账户根据项目id查看商列表
        public static string ProlistbyProjectid(int Pageindex, int Pagesize, int agentid, int projectid, DateTime startime, DateTime endtime)
        {
            int totalcount = 0;
            try
            {
                var prodata = new B2bComProData();
                B2b_com_projectData projectdata = new B2b_com_projectData();

                var list = projectdata.ProlistbyProjectid(Pageindex, Pagesize, agentid, projectid, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {

                                 Id = pro.Id,
                                 Pro_name = pro.Pro_name,
                                 Pro_end = pro.Pro_end,
                                 Pro_start = pro.Pro_start,
                                 Pro_state = pro.Pro_state == 0 ? "下线" : "上线",

                                 Use_pnum = prodata.ProYanzhengCount(pro.Id, startime, endtime),
                                 Agentsettle_price = pro.Agentsettle_price,
                                 Advise_price = pro.Advise_price,
                                 Projectid = pro.Projectid,
                                 Servertype = pro.Server_type,
                                 Bindingid = pro.Bindingid,
                                 startime = startime,
                                 endtime = endtime,

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 预付款管理
        public static string WriteAgentMoney(string acttype, decimal money, int agentid, int comid, string ordername = "", int Rebatetype = 0, int userid = 0)
        {
            int totalcount = 0;
            int warrantid = 0;
            int Payment = 0;
            string Payment_type = "";
            string Servicesname = "";

            if (acttype == "add_imprest")
            {
                Payment = 0;
                Servicesname = "分销预付款";
                Payment_type = "分销充值";
            }
            else if (acttype == "reduce_imprest")
            {
                Payment = 1;
                Servicesname = "分销预付款";
                Payment_type = "分销扣款";
                money = 0 - money;

            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "录入错误" });
            }

            Servicesname += "[" + ordername + "]";


            decimal overmoney = 0;
            try
            {
                var agentinfo = AgentCompanyData.GetAgentWarrant(agentid, comid);
                if (agentinfo != null)
                {
                    warrantid = agentinfo.Warrantid;
                    overmoney = agentinfo.Imprest + money;
                }

                //分销商财务扣款
                Agent_Financial Financialinfo = new Agent_Financial
                {
                    Id = 0,
                    Com_id = comid,
                    Agentid = agentid,
                    Warrantid = warrantid,
                    Order_id = 0,
                    Servicesname = Servicesname,
                    SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                    Money = money,
                    Payment = Payment,            //收支(0=收款,1=支出)
                    Payment_type = Payment_type,       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点,分销扣款）
                    Rebatetype = Rebatetype,
                    Money_come = "商家",         //资金来源（网上支付,银行收款，分销账户等）
                    Over_money = overmoney,
                    userid = userid
                };
                var fin = AgentCompanyData.AgentFinancial(Financialinfo);

                //分销商预付款充值 则修改分销提醒通知短信状态
                if (acttype == "add_imprest")
                {
                    //设置分销充值提醒短信通知为 处理成功
                    int r = new Agent_RechargeRemindSmsData().UpremindSmsState(agentid, comid, 1);
                }

                return JsonConvert.SerializeObject(new { type = 100, msg = fin });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 设定信用额度
        public static string Setagentcredit(decimal money, int agentid, int comid, string ordername = "")
        {
            int warrantid = 0;
            try
            {
                var agentinfo = AgentCompanyData.GetAgentWarrant(agentid, comid);
                if (agentinfo != null)
                {
                    warrantid = agentinfo.Warrantid;
                }

                var fin = AgentCompanyData.UpdateCredit(warrantid, money);

                //设置分销充值提醒短信通知为 处理成功
                int r = new Agent_RechargeRemindSmsData().UpremindSmsState(agentid, comid, 1);

                return JsonConvert.SerializeObject(new { type = 100, msg = fin });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 修改分销账户类型
        public static string ChangeAgentsort(int agentid, int Agentsort)
        {
            try
            {
                var agentinfo = AgentCompanyData.ChangeAgentsort(agentid, Agentsort);


                return JsonConvert.SerializeObject(new { type = 100, msg = agentinfo });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
        #region 修改分销账户类型
        public static string ChangeAgentsourcesort(int agentid, int Agentsourcesort)
        {
            try
            {
                var agentinfo = AgentCompanyData.ChangeAgentsourcesort(agentid, Agentsourcesort);


                return JsonConvert.SerializeObject(new { type = 100, msg = agentinfo });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 查询未开通的商户
        public static string SearchUnopenCom(int agentid, string key)
        {
            try
            {
                var cominfo = B2bCompanyData.SearchUnopenCom(key);
                if (cominfo != null)
                {
                    var result = new
                    {
                        Company = cominfo.Com_name,
                        Tel = cominfo.B2bcompanyinfo.Tel,
                        Contentname = cominfo.B2bcompanyinfo.Contact,
                        Account = B2bCompanyManagerUserData.GetFirstAccountUser(cominfo.ID),
                        id = cominfo.ID,
                    };

                    return JsonConvert.SerializeObject(new { type = 100, msg = result });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "没有此商户" });
                }
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 登陆
        public static string Login(string email, string pwd, out Agent_company userinfo)
        {
            var pro = "";
            try
            {

                pro = AgentCompanyData.Login(email, pwd, out userinfo);
                if (pro != "OK")
                {
                    userinfo = null;
                    return pro;
                }
                return "OK";
            }
            catch (Exception ex)
            {
                userinfo = null;
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 绑定分销账户
        public static string BindingAgent(string email, string pwd, out Agent_company userinfo)
        {
            var pro = "";
            int comid = 0;
            try
            {
                if (UserHelper.ValidateLogin())
                {
                    comid = UserHelper.CurrentCompany.ID;
                }

                //先判断分销信息，返回ok则正确
                pro = AgentCompanyData.Login(email, pwd, out userinfo);
                if (pro != "OK")
                {
                    userinfo = null;
                    return pro;
                }
                if (userinfo != null)
                {
                    var bangdinginfo = AgentCompanyData.BindingAgent(comid, userinfo.Id);
                    if (bangdinginfo > 0)
                    {
                        return "OK";
                    }
                    else
                    {
                        return "绑定错误";
                    }
                }

                return "绑定错误,请重新操作";

            }
            catch (Exception ex)
            {
                userinfo = null;
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 绑定商户账户 ,未做完
        public static string BindingCom(string email, string pwd, int agentid)
        {
            var msg = "";
            try
            {
                B2b_company_manageuser userinfo = B2bCompanyManagerUserData.VerifyUser(email, pwd, out msg);

                //先判断分销信息，返回ok则正确
                if (userinfo != null)
                {
                    var bangdinginfo = AgentCompanyData.BindingAgent(userinfo.Com_id, agentid);
                    if (bangdinginfo > 0)
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = "OK" });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "绑定错误" });
                    }
                }

                return JsonConvert.SerializeObject(new { type = 1, msg = "绑定错误，请重新操作" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 解除绑定分销账户
        public static string UnBindingAgent()
        {
            int comid = 0;
            try
            {
                if (UserHelper.ValidateLogin())
                {
                    comid = UserHelper.CurrentCompany.ID;
                }

                var bangdinginfo = AgentCompanyData.UnBindingAgent(comid);
                if (bangdinginfo > 0)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = "OK" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "绑定错误" });
                }

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 开通商户
        public static string AgentComOpen(int agentid, int comid, string Account, int youxiaoqi, string AdjustHasInnerChannel)
        {

            if (agentid == 0 || comid == 0 || Account == "")
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "参数传递错误，请重新操作" });
            }

            //通过扥股账户获得Id
            B2bCompanyManagerUserData Userdata = new B2bCompanyManagerUserData();
            var masterid = Userdata.FromAccountGetId(Account);
            if (masterid == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "商户登陆账户错误" });
            }

            var CompanyData = new B2bCompanyData();
            var cominfo = B2bCompanyData.GetCompany(comid);
            if (cominfo != null)
            {
                if (cominfo.Agentopenstate != 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "此商户已经开户" });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "无此商户" });
            }

            //读取授权ID，
            int Warrantid = 0;
            decimal overmoney = 0;
            decimal Credit = 0;

            //读取分销商信息，现在都有易城106 账户管理
            Agent_company agentinfo = AgentCompanyData.GetAgentWarrant(agentid, 106);
            if (agentinfo == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "分销商户未开通授权" });
            }
            else
            {
                Warrantid = agentinfo.Warrantid;
                overmoney = agentinfo.Imprest;
                Credit = agentinfo.Credit;
            }
            //对于授权金额+预付款低于0的则直接返回不能开通
            if (overmoney + Credit <= 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "您的预付款金额不足！" });
            }

            try
            {
                string msg = "";
                //开通，赋值权限
                bool isset = PermissionJsonData.SetMasterGroup(masterid.ToString(), "商户负责人", out msg);

                if (isset)
                {
                    //对商户设定是 普通还是连锁
                    int result1 = new B2bCompanyInfoData().AdjustHasInnerChannel(comid, AdjustHasInnerChannel);


                    //对商户赋值为分销开户
                    var opendate = DateTime.Now;
                    var enddate = opendate.AddYears(1).AddDays(1);

                    var agentopencom = CompanyData.Agent_Open_Comid(comid, agentid, opendate, enddate);

                    //对商户开通
                    var prodata = new B2bCompanyInfoData();
                    var result = prodata.UpComstate(comid, "已暂停");//对已未开通（已暂停）的账户进行开通操作。


                    int money = 100;//扣款默认金额
                    if (AdjustHasInnerChannel == "ture")
                    {
                        money = 500;
                    }

                    //账户金额 先要扣去本次交易金额
                    overmoney = overmoney - money;

                    //分销商财务扣款
                    Agent_Financial Financialinfo = new Agent_Financial
                    {
                        Id = 0,
                        Com_id = 106, //微站都使用106商户
                        Agentid = agentid,
                        Warrantid = Warrantid,
                        Order_id = 0,
                        Servicesname = "开通商户扣款",
                        SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff"),       //财务流水号
                        Money = 0 - money,        //每个商户扣款100元
                        Payment = 1,            //收支(0=收款,1=支出)
                        Payment_type = "分销扣款",       //类型（充值，商家提现，直销收款，消费（验票），商家退款，直销退票，手续费，商家返点,分销扣款）
                        Money_come = "分销账户",         //资金来源（网上支付,银行收款，分销账户等）
                        Over_money = overmoney
                    };
                    var fin = AgentCompanyData.AgentFinancial(Financialinfo);

                    //发送通知短信
                    string msg1 = "";
                    //获取开通的账户
                    var comaccount = B2bCompanyManagerUserData.GetFirstAccountUser(comid);
                    var commanageusredata = B2bCompanyManagerUserData.GetManageUserByAccount(comaccount);
                    if (commanageusredata != null)
                    {

                        string smsstr = "已经为您开通供应商账户。账户名:" + comaccount + " 密码:" + commanageusredata.Passwords + " 请登录后立即更改，登录地址 " + agentinfo.Agent_domain;
                        if (agentinfo.Agent_domain == "www.maikexing.com" || agentinfo.Agent_domain == "agent.maikexing.com" || agentinfo.Agent_domain == "admin.maikexing.com")
                        {
                            smsstr = "已经为您开通麦客行海淘购物平台供应商账户。账户名:" + comaccount + " 密码:" + commanageusredata.Passwords + " 请登录后立即更改，登录地址 " + agentinfo.Agent_domain;
                        }


                        int sendback = SendSmsHelper.SendSms(cominfo.B2bcompanyinfo.Phone, smsstr, comid, out msg1);
                    }




                    return JsonConvert.SerializeObject(new { type = 100, msg = "开通成功" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = msg });
                }

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 修改授权，或新授权
        public static string ModifyAgentInfo(Agent_company regiinfo)
        {
            try
            {
                //如果商户或分销ID为0，则为失败授权
                if (regiinfo.Comid == 0 || regiinfo.Id == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "失败，参数错误，请刷新后重新提交" });
                }
                if (regiinfo.Warrant_state == 1)
                {
                    /*****
                     * 判断美团分销是否授权过其他商户，保证美团分销 和 商家授权 一对一（如果在多个商家下授权，美团账号余额查询接口无法确认查询哪个商家下的余额）
                     * 1.判断是否是美团分销
                     * 2.是美团分销的话，判断是否授权过其他商户
                     * ***/
                    bool ismeituan = new AgentCompanyData().IsMeituanAgent(regiinfo.Id);
                    if (ismeituan)
                    {
                        //得到分销授权信息
                        List<Agent_warrant> warrantinfolist = new AgentCompanyData().GetAgentWarrantList(regiinfo.Id, "1");
                        if (warrantinfolist.Count > 1)
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "美团分销 只可以 授权一家商户" });
                        }
                        if (warrantinfolist.Count == 1)
                        {
                            int shouquancomid = 0;
                            foreach (Agent_warrant winfo in warrantinfolist)
                            {
                                if (winfo != null)
                                {
                                    shouquancomid = winfo.Comid;
                                }
                            }
                            if (shouquancomid != regiinfo.Comid)
                            {
                                return JsonConvert.SerializeObject(new { type = 1, msg = "美团分销 只可以 授权一家商户" });
                            }
                        }
                    }

                }


                //查询是否授权过，只能授权一次
                var searchsatate = AgentCompanyData.SearchAgentwarrant(regiinfo);

                if (searchsatate == 0)
                {
                    //对已有分销，商户不能授权

                    var pro = AgentCompanyData.AddAgentInfo(regiinfo);

                    if (pro == 0)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "操作失败" });
                    }
                    return JsonConvert.SerializeObject(new { type = 100, msg = "OK" });


                }
                else
                {
                    //对已有授权，可以进行修改
                    var pro = AgentCompanyData.ModifyAgentInfo(regiinfo);

                    if (pro == 0)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "操作失败" });
                    }
                    return JsonConvert.SerializeObject(new { type = 100, msg = "OK" });
                }


            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 读取分销账户信息
        public static string GetAgentWarrant(int agentid, int comid)
        {

            try
            {
                if (agentid == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "信息错误" });
                }
                else
                {
                    //先联合读取授权及分销信息
                    var list = AgentCompanyData.GetAgentWarrant(agentid, comid);

                    if (list == null)
                    {
                        //如果没有授权，直接读取分销本身注册信息
                        list = AgentCompanyData.GetAgentByid(agentid);
                    }
                    Agent_company result = new Agent_company
                    {
                        Imprest = list.Imprest,
                        Credit = list.Credit,
                        Company = list.Company,
                        Id = list.Id,
                        Tel = list.Tel,
                        Mobile = list.Mobile,
                        Address = list.Address,
                        Contentname = list.Contentname,
                        Warrant_type = list.Warrant_type,
                        Warrant_state = list.Warrant_state,
                        Warrant_level = list.Warrant_level,
                        Run_state = list.Run_state,
                        Agentsort = list.Agentsort,
                        Agent_domain = list.Agent_domain,
                        Weixin_img = list.Weixin_img,
                        Account = AgentCompanyData.GetAgentAccount(list.Id),//获取分销商开户账户
                        Rebatemoney = AgentCompanyData.GetAgentRebatemoney(list.Id, comid)//返点录入金额
                    };


                    return JsonConvert.SerializeObject(new { type = 100, msg = result });
                }
            }
            catch (Exception ex)
            {
                // return ex.Message;
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 忘记密码
        public static string FindPass(string account, string phone, string findway, int comid)
        {
            string message;
            string content = "您账户密码重置成功,新密码：$pass$，请登陆后更改密码！";
            int userid = 0;
            if (findway == "")
            {
                findway = "sms";
            }

            //判断登录账户是否存在
            var model2 = AgentCompanyData.AgentSearchAccount(account);
            if (model2 == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "账户与手机匹配错误！" });
            }
            if (phone.Trim() == "")
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "手机信息错误！" });
            }
            if (phone.Trim() != model2.Mobile.Trim())
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "账户与手机匹配错误！" });
            }

            if (findway == "sms")
            {
                //短信重置密码
                Random ra = new Random();
                string newPass = ra.Next(26844521, 98946546).ToString();

                model2.Pwd = newPass;//重置密码

                var uppass = AgentCompanyData.ManageUpAccount(model2);
                if (uppass == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "重置密码错误！" });
                }

                content = content.Replace("$pass$", newPass);
                var backContent = SendSmsHelper.SendSms(phone, content, comid, out message);
                if (backContent < 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "重置短信发送失败，请重新重置密码！" });
                }

            }

            if (findway == "email")
            {
                //邮件重置密码连接
                //尚未做


            }



            return JsonConvert.SerializeObject(new { type = 100, msg = "密码重置成功" });
        }
        #endregion


        public static string ChangeAgentMsgset(int agentid, int agentmsgset)
        {
            int r = new Agent_companyData().ChangeAgentMsgset(agentid, agentmsgset);
            if (r > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = r });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = r });
            }
        }


        #region 分销商通知
        public static string AgentMessageList(int Pageindex, int Pagesize, int comid)
        {
            int totalcount = 0;
            try
            {
                B2b_comagentmessageData agentcom = new B2b_comagentmessageData();

                var list = agentcom.AgentMessageList(Pageindex, Pagesize, comid, out totalcount);

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = list });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion


        #region 分销商通知
        public static string AgentViewMessageList(int Pageindex, int Pagesize, int comid, int agentid)
        {
            int totalcount = 0;
            try
            {
                B2b_comagentmessageData agentcom = new B2b_comagentmessageData();

                var list = agentcom.AgentViewMessageList(Pageindex, Pagesize, comid, agentid, out totalcount);

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = list });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 修改分销通知
        public static string AgentMessageInfo(int id, int comid)
        {
            try
            {
                B2b_comagentmessageData agentcom = new B2b_comagentmessageData();
                var agentinfo = agentcom.AgentMessageInfo(id, comid);

                return JsonConvert.SerializeObject(new { type = 100, msg = agentinfo });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 修改分销通知
        public static string AgentMessageUp(B2b_com_agent_message messageinfo)
        {
            try
            {
                //B2b_comagentmessageData agentcom = new B2b_comagentmessageData();
                var agentinfo = B2b_comagentmessageData.AgentMessageUp(messageinfo);

                if (agentinfo > 0)
                {
                    if (messageinfo.Sendsms == 1)
                    {
                        if (messageinfo.Smstext != "")
                        {

                            //读取所有授权分销商列表
                            B2b_com_projectData projectdata = new B2b_com_projectData();
                            var phonestr = projectdata.GetAgentPhonelist(messageinfo.Comid);
                            string msg = "";
                            if (phonestr != "")
                            {
                                //要用群发短信接口，未做完
                                int sendback = SendSmsHelper.SendSms(phonestr, messageinfo.Smstext, messageinfo.Comid, out msg);
                            }
                        }
                    }
                }



                return JsonConvert.SerializeObject(new { type = 100, msg = agentinfo });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 打印票务
        public static string PrinteTicket(int orderid, int comid, int agentid, int size)
        {

            //首先查询订单
            B2bOrderData orderdata = new B2bOrderData();
            var orderinfo = orderdata.GetOrderById(orderid);
            if (orderinfo == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "订单错误！" });
            }


            if (orderinfo.Comid != comid)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "订单商户错误！" });
            }

            if (orderinfo.Agentid != agentid)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "订单分销账户错误！" });
            }

            //订单订购数量一定大于打印数量
            if (orderinfo.U_num < size)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "打印数量不符！" });
            }

            B2bEticketData eticketdata = new B2bEticketData();
            var eticketinfo = eticketdata.PrintTicketbyOrderid(orderid, size);


            if (eticketinfo != null)
            {
                if (eticketinfo.Count != size)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "打印数量与未打印数量不符,或未打印电子票已经使用！" });
                }

                IEnumerable result = "";
                if (eticketinfo != null)

                    for (int i = 0; i < eticketinfo.Count; i++)
                    {
                        var up = eticketdata.PrintStateUp(eticketinfo[i].Id);//修改已打印状态
                    }
                result = from pro in eticketinfo
                         select new
                         {
                             Id = pro.Id,
                             PnoMd5 = EncryptionHelper.EticketPnoDES(pro.Pno, 0),
                             Pno = pro.Pno,
                             Pagecode = pro.Pagecode.ToString(),
                             Printstate = pro.Printstate,
                         };
                return JsonConvert.SerializeObject(new { type = 100, msg = result });

            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "未查询到电子票！" });
            }
        }
        #endregion

        /// <summary>
        /// 获得公司下 分销商所在的地区
        /// </summary>
        /// <param name="comid"></param>
        /// <returns></returns>
        public static string Getagentprovincelist(int comid = 0)
        {
            List<string> r = new Agent_companyData().Getagentprovincelist(comid);
            if (r == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = r });
            }
            else
            {
                if (r.Count > 0)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = r });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = "" });
                }
            }
        }
        /// <summary>
        /// 获得分销分类列表
        /// </summary>
        /// <param name="reqsource"></param>
        /// <returns></returns>
        public static string GetAgentSortlist()
        {

            Dictionary<string, string> diclist = EnumUtils.GetValueName(typeof(Agentsort));
            DataTable dt = new DataTable();
            dt.Columns.Add("key", typeof(int));
            dt.Columns.Add("value", typeof(string));

            foreach (KeyValuePair<string, string> item in diclist)
            {
                DataRow dr = dt.NewRow();
                dr["key"] = item.Key;
                dr["value"] = item.Value;
                dt.Rows.Add(dr);
            }
            return JsonConvert.SerializeObject(new { type = 100, msg = dt });

        }
        /// <summary>
        /// 查询分销商统计信息
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public static string GetAgentFinanceCount(int comid, string StartDate, string EndDate)
        {
            if (comid == 0 || StartDate == "" || EndDate == "")
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "传递参数错误" });
            }

            DateTime startdate = StartDate.ConvertTo<DateTime>();
            DateTime enddate = EndDate.ConvertTo<DateTime>();

            List<DateTime> list = new List<DateTime>();
            for (int i = 0; i < (enddate - startdate).Days + 1; i++)
            {
                list.Add(startdate.AddDays(i));
            }
            if (list.Count > 0)
            {
                IEnumerable result = "";
                if (list != null)
                {
                    result = from m in list
                             select new
                             {
                                 daydate = m.ToString("yyyy-MM-dd"),
                                 Line_Imprest = AgentCompanyData.Line_Imprest(comid, m),//网上支付预付款
                                 Hand_Imprest = AgentCompanyData.Hand_Imprest(comid, m),//后台充入预付款
                                 Hand_Rebate = AgentCompanyData.Hand_Rebate(comid, m),//后台充入返点

                                 //Imprest = AgentCompanyData.OverMoney(comid, m),//总余额(当天最后一笔交易后剩余预付款)

                                 Sale_price = AgentCompanyData.Sale_price(comid, m),//销售额
                                 Maoli_price = AgentCompanyData.Maoli_price(comid, m),//毛利
                                 Daoma_Sale_price = AgentCompanyData.Daoma_Sale_price(comid, m),//倒码销售额
                                 Daoma_Maoli_price = AgentCompanyData.Daoma_Maoli_price(comid, m),//倒码毛利
                                 Xiaofei_price = Math.Abs(AgentCompanyData.Xiaofei_price(comid, m)),//消费(出票扣款 当天出的票已经验证的款项)

                                 Chendian_price = AgentCompanyData.Chendian_price(comid, m),//沉淀(出票扣款 当天出的票没有验证的款项)
                             };
                    return JsonConvert.SerializeObject(new { type = 100, msg = result });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }


        public static string GetAgentSourceSortlist()
        {
            Dictionary<string, string> diclist = EnumUtils.GetValueName(typeof(Agentsourcesort));
            DataTable dt = new DataTable();
            dt.Columns.Add("key", typeof(int));
            dt.Columns.Add("value", typeof(string));

            foreach (KeyValuePair<string, string> item in diclist)
            {
                DataRow dr = dt.NewRow();
                dr["key"] = item.Key;
                dr["value"] = item.Value;
                dt.Rows.Add(dr);
            }
            return JsonConvert.SerializeObject(new { type = 100, msg = dt });

        }



        public static string Personrechargepagelist(int pageindex, int pagesize, int agentid, int comid, string key, string payment_type)
        {
            int totalcount = 0;
            try
            {
                var list = new AgentCompanyData().Agent_Financialpagelist(pageindex, pagesize, key, agentid, comid, payment_type, out totalcount);
                IEnumerable result = "";
                if (list != null)
                {
                    result = from pro in list
                             select new
                             {
                                 pro.Id,
                                 pro.Agentid,
                                 pro.Warrantid,
                                 pro.Com_id,
                                 pro.Order_id,
                                 pro.Servicesname,
                                 pro.Money,
                                 pro.SerialNumber,
                                 pro.Over_money,
                                 pro.Money_come,
                                 pro.Payment_type,
                                 pro.Payment,
                                 pro.Remarks,
                                 pro.Subdate,
                                 pro.Rebatetype,
                                 AgentCompanyName = new AgentCompanyData().GetAgentCompanyName(pro.Agentid),
                                 pro.userid,
                                 UserName = new B2bCompanyManagerUserData().GetCompanyUserName(pro.userid)
                             };
                }
                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        /// <summary>
        ///发送验证码 comid 可为0，account可为""（密码找回时需要有值）
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="comid"></param>
        /// <param name="source"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static string Callvalidsms(string mobile, int comid, string source, string account)
        {
            //判断手机号在分销员工中是否存在
            if (mobile == "")
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "传递手机号码格式不正确" });
            }

            string smsstr = "";//验证码发送内容
            if (source == "通用验证码")
            {
                smsstr = "你的验证码为@randomcode,请妥善保存。有效期30分钟";
            }
            else if (source == "分销商商户管理验证码")
            {
                int count = new AgentCompanyData().GetAgentCountByMobile(mobile);
                if (count > 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "该手机号码已经使用!" });

                }
                smsstr = "你的分销商商户管理验证码为@randomcode,请妥善保存。有效期30分钟";
            }
            else if (source == "员工管理验证码")
            {
                int count = new AgentCompanyData().GetAgentUserCountByMobile(mobile);
                if (count > 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "该手机号码已经使用!" });

                }
                smsstr = "你的员工管理验证码为@randomcode,请妥善保存。有效期30分钟";
            }
            else if (source == "分销注册验证码")
            {
                int count = new AgentCompanyData().GetAgentUserCountByMobile(mobile);
                if (count > 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "该手机号码已经注册!" });

                }
                smsstr = "你的分销注册验证码为@randomcode,请妥善保存。有效期30分钟";
            }
            else if (source == "分销密码找回验证码")
            {
                int count = new AgentCompanyData().GetAgentUserCountByMobile(mobile, account);
                if (count == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "该手机号码与账户信息不符!" });

                }
                smsstr = "你的分销密码找回验证码为@randomcode,请妥善保存。有效期30分钟";

            }
            else if (source == "手机动态密码")
            {
                int count = new AgentCompanyData().GetAgentUserCountByMobile(mobile, account);
                if (count == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "该手机号码与账户信息不符!" });

                }
                smsstr = "你的手机动态密码为@randomcode,请妥善保存。有效期30分钟";

            }
            else if (source == "分销密码找回")
            {
                smsstr = "你的分销找回密码为@randomcode,请妥善保存。有效期30分钟";
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "验证码来源没设定" });
            }



            //创建随机码
            Random ra = new Random();
            decimal code = ra.Next(100000, 999999);


            //30分钟内发送短信的次数，超过3次，提醒“同一个手机号半小时内最多接收3次短信”
            int sendnumin30 = new ValidCode_SendLogData().GetSendNumIn30ByMobile(mobile, source);
            if (sendnumin30 >= 3)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "同一个手机号半小时内最多接收3次短信!" });
            }
            //得到30分钟内最新的发送记录
            ValidCode_SendLog sendrecord = new ValidCode_SendLogData().GetLasterLogByMobile(mobile, source);


            if (sendrecord != null)//重发验证码，设置验证码为第一次发送的验证码
            {
                //30分钟内最新的发送记录的验证码 第一次发送时间如果超过30分钟，则验证码过期需要生成新的验证码
                DateTime firstsendtime = new ValidCode_SendLogData().GetFirstsendtimeByCode(mobile, source, decimal.Parse(sendrecord.randomcode));
                if (DateTime.Now < firstsendtime.AddMinutes(30))
                {
                    code = decimal.Parse(sendrecord.randomcode);
                }
                else
                {
                    sendrecord.send_serialnum = 0;
                }
            }
            else //第一次发送验证码，设置验证码发送次数为0次
            {
                sendrecord = new ValidCode_SendLog
                {
                    send_serialnum = 0,
                };
            }

            #region  如果是分销密码找回，需要修改分销用户的密码
            if (source == "分销密码找回")
            {
                if (account != "")
                {
                    int upagentuserpass = new AgentCompanyData().UpAgentUserPass(mobile, code, account);
                    if (upagentuserpass == 0)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "修改分销用户密码出错!" });

                    }

                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "分销登录账户不可为空!" });

                }
            }
            #endregion


            //发送随机码
            smsstr = smsstr.Replace("@randomcode", code.ToString());
            string msg = "";
            int sendback = SendSmsHelper.SendSms(mobile, smsstr, comid, out msg);

            //把发送记录录入数据库
            ValidCode_SendLog log = new ValidCode_SendLog
            {
                id = 0,
                mobile = mobile,
                randomcode = code.ToString(),
                Content = smsstr,
                send_serialnum = sendrecord.send_serialnum + 1,
                sendtime = DateTime.Now,
                returnmsg = msg,
                source = source,
                sendip = CommonFunc.GetRealIP()
            };
            int data = new ValidCode_SendLogData().InsertLog(log);

            if (sendback > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "发送成功" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = msg });
            }

        }

        public static string Judgevalidsms(string mobile, string smscode, string source, out int result)
        {
            //得到最新的发送记录
            ValidCode_SendLog sendrecord = new ValidCode_SendLogData().GetLasterLogByMobile(mobile, source);
            if (sendrecord == null)
            {
                result = 1;
                if (source == "手机动态密码")
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "手机动态密码已过期" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "验证码已过期" });
                }
            }
            else
            {
                if (smscode.Trim() == sendrecord.randomcode)
                {
                    result = 100;
                    return JsonConvert.SerializeObject(new { type = 100, msg = "" });
                }
                else
                {
                    result = 1;
                    if (source == "手机动态密码")
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "手机动态密码不相符" });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "验证码不相符" });
                    }
                }
            }
        }

        public static string AgentUpPhone(int agentid, string newphone)
        {
            int r = new Agent_companyData().AgentUpPhone(agentid, newphone);
            if (r > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "修改出错" });
            }

        }

        public static string AgentUserUpPhone(int agentuserid, string newphone)
        {
            int r = new Agent_companyData().AgentUserUpPhone(agentuserid, newphone);
            if (r > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "修改出错" });
            }
        }

        public static string GetTb_agent_relation(int agentid, int serialnum)
        {
            Taobao_agent_relation r = new Taobao_agent_relationData().GetTb_agent_relation(agentid, serialnum);
            return JsonConvert.SerializeObject(new { type = 100, msg = r });
        }

        public static string EditpartTb_agent_relation(Taobao_agent_relation r)
        {
            //通过淘宝旺旺号 判断当前淘宝店铺是否已经申请过 
            if (r.tb_seller_wangwang != "")
            {
                bool isbind = new Taobao_agent_relationData().IsAskByTbWangwang(r.tb_seller_wangwang);
                if (isbind)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "当前淘宝店铺已经申请过，无需再申请" });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "淘宝旺旺号不可为空" });
            }


            int result = new Taobao_agent_relationData().EditpartTb_agent_relation(r);
            if (result > 0)
            {
                int agentid = r.agentid;
                Agent_company agentcompanyy = new AgentCompanyData().GetAgentCompany(agentid);
                if (agentcompanyy == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "根据分销id得到分销信息出错" });
                }

                //如果是提交审核的话，给易城码商工作人员发送短信 
                if (r.serialnum == 0)
                {
                    string smsstr = "分销商：" + agentcompanyy.Company + " 提交了开通淘宝的请求，请及时登录平台总账户处理。";
                    string msg = "";
                    int sendback = SendSmsHelper.SendSms("13671288690", smsstr, 0, out msg);
                }
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = result });
            }

        }

        public static string GetTb_agent_relationList(int agentid)
        {
            IList<Taobao_agent_relation> list = new Taobao_agent_relationData().GetTb_agent_relationList(agentid);
            string rr = JsonConvert.SerializeObject(new { type = 100, msg = list });
            return rr;
        }

        public static string EditTb_agent_relation(Taobao_agent_relation r)
        {

            if (r.tb_seller_wangwangid != "")
            {
                //当前合作卖家是否 已经绑定了分销
                bool isb = new Taobao_agent_relationData().IsbAgentBytbwangwangid(r.tb_seller_wangwangid);
                if (isb)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "当前合作卖家已经绑定了分销" });
                }
            }
            //添加操作判断 合作卖家是否申请过
            if (r.serialnum == 0)
            {
                bool isbind = new Taobao_agent_relationData().IsAskByTbWangwang(r.tb_seller_wangwang);
                if (isbind)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "当前合作卖家已经申请过，无需再申请" });
                }
            }

            int result = new Taobao_agent_relationData().EditTb_agent_relation(r);

            if (result > 0)
            {
                int agentid = r.agentid;

                //把分销商的开通淘宝状态 变为开通
                int rd = new Agent_companyData().UpAgentTaobaoState(1, agentid);

                Agent_company agentcompanyy = new AgentCompanyData().GetAgentCompany(agentid);
                if (agentcompanyy == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "根据分销id得到分销信息出错" });
                }

                //给分销商工作人员发送短信 
                string smsstr = "恭喜你，" + r.tb_seller_wangwang + " 的淘宝店铺已成功绑定易城淘宝码商，请进入店铺产品管理中选择码商:易城。";
                string msg = "";
                int sendback = SendSmsHelper.SendSms(agentcompanyy.Mobile, smsstr, 0, out msg);

                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = result });
            }




        }

        public static string Getclientlistbyagentid(int Pageindex, int Pagesize, string Key, int Agentid)
        {
            int totalcount = 0;
            IList<B2b_order> list = new B2bOrderData().Getclientlistbyagentid(Pageindex, Pagesize, Key, Agentid, out totalcount);
            if (list.Count > 0)
            {
                string rr = JsonConvert.SerializeObject(new { type = 100, msg = list, totalcount = totalcount });
                return rr;
            }
            else
            {
                string rr = JsonConvert.SerializeObject(new { type = 1, msg = "客户信息为空", totalcount = 0 });
                return rr;
            }

        }

        public static string Editagentoutinterface(int agentid, string agent_updateurl, string txtagentip, string inter_sendmethod = "post")
        {
            int r = new AgentCompanyData().Editagentoutinterface(agentid, agent_updateurl, txtagentip, inter_sendmethod);
            if (r > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "编辑成功" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "编辑失败" });
            }
        }

        public static string GetMeituanAgentCompanyList(int comid)
        {

            IList<Agent_company> list = new AgentCompanyData().GetMeituanAgentCompanyList(comid);

            string rr = JsonConvert.SerializeObject(new { type = 100, msg = list });
            return rr;

        }
    }
}

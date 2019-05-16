using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Data.InternalData;
using Newtonsoft.Json;
using ETS2.CRM.Service.CRMService.Data;
using System.Data;
using System.Collections;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle.Enum;
using ETS2.Common.Business;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Member.Service.MemberService.Model;
using ETS2.PM.Service.PMService.Data;

namespace ETS.JsonFactory
{
    public class AccountInfoJsonData
    {
        #region 编辑商家信息

        /// <summary>
        /// 编辑商家信息 by:xiaoliu
        /// </summary>
        /// <param name="b2b_company"></param>
        /// <param name="B2b_Company_Info"></param>
        /// <param name="manageuser"></param>
        /// <returns></returns>
        public static string InsertOrUpdateB2bCompany(B2b_company b2b_company, B2b_company_info B2b_Company_Info)
        {
            try
            {
                using (var sql = new SqlHelper())
                {
                    var internalb2bcompany = new InternalB2bCompany(sql);
                    var companyid = internalb2bcompany.InsertOrUpdate(b2b_company);

                    var internalb2bcompanyinfo = new InternalB2bCompanyInfo(sql);

                    var companyinfoid = internalb2bcompanyinfo.InsertOrUpdate(B2b_Company_Info);

                    return JsonConvert.SerializeObject(new { type = 100, msg = companyid });
                }
            }
            catch
            {
                new SqlHelper().Dispose();
                throw;
            }


        }
        #endregion

        #region 修改公司title
        public static string UpdateB2bCompanyName(int comid, string comname)
        {
            try
            {
                var result = B2bCompanyInfoData.UpdateB2bCompanyName(comid, comname);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }

        }
        #endregion

        #region 修改公司title
        public static string UpdateB2bCompanySearchset(int comid, int setsearch)
        {
            try
            {
                var result = B2bCompanyInfoData.UpdateB2bCompanySearchset(comid, setsearch);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }

        }
        #endregion

        #region 得到公司基本信息和扩展信息
        public static string GetAllComMsg(int comid)
        {
            try
            {
                B2b_company com = B2bCompanyData.GetAllComMsg(comid);
                return JsonConvert.SerializeObject(new { type = 100, msg = com });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }
        #endregion
        #region 得到公司基本信息关注值
        public static string GetAllComGuanzhuMsg(int comid)
        {
            try
            {
                B2b_company com = B2bCompanyData.GetAllComMsg(comid);
                if (com != null) {
                    return JsonConvert.SerializeObject(new { type = 100, msg = com.B2bcompanyinfo.Wxfocus_url });
                }
                return JsonConvert.SerializeObject(new { type = 1, msg = "失败" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }
        #endregion
        #region 修改公司资质信息
        public static string ModifyComZizhi(int comextid, string comcode, string comsitecode, string comlicence, string scenic_intro = "", string domainname = "")
        {
            try
            {
                var result = B2bCompanyInfoData.ModifyZizhi(comextid, comcode, comsitecode, comlicence, scenic_intro, domainname);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }

        }
        #endregion
        #region 修改公司授权和协议信息
        public static string ModifyComShouquan(int comextid, string sale_Agreement, string agent_Agreement)
        {
            try
            {
                var result = B2bCompanyInfoData.ModifyComShouquan(comextid, sale_Agreement, agent_Agreement);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }
        #endregion
        #region 修改绑定打印机信息
        public static string ModifyBangPrint(int comextid, string Defaultprint)
        {
            try
            {
                var result = B2bCompanyInfoData.ModifyBangPrint(comextid, Defaultprint);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }
        #endregion
        #region 根据公司id获取pos绑定列表
        public static string posList(string comid, int pageindex, int pagesize)
        {
            var totalcount = 0;
            try
            {

                var prodata = new B2bCompanyInfoData();
                var list = prodata.PosList(comid, pageindex, pagesize, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 pro.Id,
                                 pro.Com_id,
                                 pro.Posid,
                                 pro.Remark,
                                 pro.Poscompany,
                                 pro.BindingTime
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
        public static string posList(int pageindex, int pagesize,string key="")
        {
            var totalcount = 0;
            try
            {
                var projectdata = new B2b_com_projectData();
                var prodata = new B2bCompanyInfoData();
                var list = prodata.PosList(pageindex, pagesize, out totalcount,key);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Com_id = pro.Com_id,
                                 Posid = pro.Posid,
                                 Remark = pro.Remark,
                                 Poscompany = pro.Poscompany,
                                 BindingTime = pro.BindingTime,
                                 Projectid = pro.Projectid,
                                 Projectname = projectdata.GetProjectname(pro.Projectid),
                                 md5key = pro.md5key
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #region 根据公司id获取pos详细信息
        public static string posinfo(int pos_id)
        {
            var totalcount = 0;
            try
            {
                var projectdata = new B2b_com_projectData();
                var prodata = new B2bCompanyInfoData();
                var list = prodata.PosInfo(pos_id);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 pro.Id,
                                 pro.Com_id,
                                 pro.Posid,
                                 pro.Remark,
                                 pro.Poscompany,
                                 pro.BindingTime,
                                 Projectid = pro.Projectid,
                                 Projectname = projectdata.GetProjectname(pro.Projectid),
                                 md5key = pro.md5key
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


        #region 修改新绑定POS信息
        public static string ModifyBangPos(int posid, string poscompany, int com_id, int userid, string Remark, int pos_id, string md5key, int projectid = 0)
        {
            try
            {
                var result = B2bCompanyInfoData.ModifyBangPos(posid, poscompany, com_id, userid, Remark, pos_id, md5key, projectid);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }
        #endregion

        #region 编辑员工信息
        public static string EditStaff(B2b_company_manageuser manageruser)
        {
            try
            {

                //if (manageruser.Id!=0)
                //{
                //    B2b_company_manageuser manageruser2 = B2bCompanyManagerUserData.GetUser(manageruser.Id);
                //    if (manageruser.Passwords != manageruser2.Passwords)
                //    {
                //        return JsonConvert.SerializeObject(new { type = 1, msg = "原密码与新密码不符，请刷新页面后重新编辑" });
                //    }
                //}

                var id = B2bCompanyManagerUserData.InsertOrUpdate(manageruser);

                return JsonConvert.SerializeObject(new { type = 100, msg = id });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
        #region 获取特定公司的员工列表
        public static string Manageuserpagelist(string comid, int pageindex, int pagesize)
        {
            var totalcount = 0;
            try
            {

                var prodata = new B2bCompanyManagerUserData();
                var list = prodata.Manageuserpagelist(comid, pageindex, pagesize, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 id = pro.Id,
                                 name = pro.Employeename,
                                 power = EnumUtils.GetName((ManageUserPower)pro.Atype)
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

        #region 获取公司的员工(渠道)列表,展示到前台
        public static string ViewChanneluserpagelist(int comid, int channelcompanyid, int pageindex, int pagesize, string key = "", string openid = "", string usern = "", string usere = "", int isheadofficekf = 0, int isonlycoachlist=0,string isviewjiaolian="0,1")
        {
            var totalcount = 0;
            try
            {
                B2bCrmData crmdata = new B2bCrmData();
                var prodata = new B2bCompanyManagerUserData();

                if (openid != "")
                {
                    //根据微信号得到用户所在顾问的渠道单位信息
                    //Member_Channel_company m_channelcompany = new MemberChannelcompanyData().GetGuWenChannelCompanyByCrmWeixin(openid, comid);
                    //if (m_channelcompany != null)
                    //{
                    //    channelcompanyid = m_channelcompany.Id;
                    //}

                }
                //得到用户坐标
                if (usere == "" || usern == "" || usere == "0" || usern == "0")
                {
                    if (openid != "")
                    {
                        B2b_crm_location location = new B2bCrmData().GetB2bcrmlocationByopenid(openid);
                        if (location != null)
                        {
                            usern = location.Latitude;
                            usere = location.Longitude;
                        }
                    }
                }


                var list = prodata.ViewChanneluserpagelist(comid, channelcompanyid, pageindex, pagesize, out totalcount, key, openid, usern, usere, isheadofficekf, isonlycoachlist,isviewjiaolian);
                IEnumerable result = "";
                if (list != null)

                    //result = from pro in list
                    //         orderby new Model_guwen().IsCanZixun descending,new Model_guwen().Distance
                    //        select new Model_guwen
                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Name = pro.Employeename,
                                 Comid = pro.Com_id,
                                 CompanyName = new B2bCompanyData().GetCompanyNameById(pro.Com_id),
                                 Atype = pro.Atype,
                                 Createuserid = pro.Createuserid,
                                 Employeestate = pro.Employeestate,
                                 Job = pro.Job,
                                 Tel = pro.Tel,
                                 channelcompanyid = pro.Channelcompanyid,
                                 ChannelCompany = new MemberChannelcompanyData().GetCompanyNameById(pro.Channelcompanyid.ToString().ConvertTo<int>(0)),
                                 Channelcompanyid = pro.Channelcompanyid,
                                 Channelsource = pro.Channelsource,
                                 Selfbrief = pro.Selfbrief,
                                 Headimg = pro.Headimg,
                                 Headimgurl = FileSerivce.GetImgUrl(pro.Headimg),
                                 Workingyears = pro.Workingyears,
                                 Workdays = pro.Workdays,
                                 Workdaystime = pro.Workdaystime,
                                 Workendtime = pro.Workendtime,
                                 Fixphone = pro.Fixphone,
                                 Email = pro.Email,
                                 Homepage = pro.Homepage,
                                 Weibopage = pro.Weibopage,
                                 QQ = pro.QQ,
                                 Weixin = pro.Weixin,
                                 Selfhomepage_qrcordurl = pro.Selfhomepage_qrcordurl,
                                 bindingproid =pro.bindingproid,
                                 //根据用户微信号判断 其顾问是不是可以咨询 
                                 IsCanZixun = crmdata.IsCanZixun(pro.Weixin, pro.Com_id),
                                 //IsCanZixun = pro.IsCanZixun,
                                 //距离用户距离
                                 //Distance = crmdata.PeopleCoordinates(openid, Int32.Parse(pro.Channelcompanyid.ToString()), pro.Com_id, usern, usere),
                                 Distance = pro.Distance,
                                 channelid =new MemberChannelData().GetChannelid(pro.Com_id, pro.Tel)
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


        #region 获取公司的员工qq
        public static string ViewQQpagelist(int comid, int channelcompanyid, int pageindex, int pagesize, string key = "", string openid = "", string usern = "", string usere = "")
        {
            var totalcount = 0;
            try
            {
                B2bCrmData crmdata = new B2bCrmData();
                var prodata = new B2bCompanyManagerUserData();


                var list = prodata.ViewQQpagelist(comid, channelcompanyid, pageindex, pagesize, out totalcount, key, openid, usern, usere);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 Id = pro.Id,
                                 Name = pro.Employeename,
                                 QQ = pro.QQ,
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


        #region 编辑短信
        public static string Insertnote(string key, string content, string title, bool radio, int note_id)
        {
            try
            {
                var result = B2bCompanyInfoData.Insertnote(key, content, title, radio, note_id); ;
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }

        public static string notList(string comid, int pageindex, int pagesize)
        {
            var totalcount = 0;
            try
            {

                var prodata = new B2bCompanyInfoData();
                var list = prodata.notList(comid, pageindex, pagesize, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 pro.Id,
                                 pro.Sms_key,
                                 pro.Title,
                                 pro.Remark,
                                 pro.Openstate,
                                 pro.Subdate,
                                 pro.Ip
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }



        public static string noteinfo(int note_id)
        {
            var totalcount = 0;
            try
            {

                var prodata = new B2bCompanyInfoData();
                var list = prodata.noteInfo(note_id);
                IEnumerable result = "";
                if (list != null)

                    result = from pro in list
                             select new
                             {
                                 pro.Id,
                                 pro.Sms_key,
                                 pro.Title,
                                 pro.Remark,
                                 pro.Openstate,
                                 pro.Subdate,
                                 pro.Ip
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

        #region  删除短信
        public static string Delnote(int id, string key)
        {
            try
            {
                var result = B2bCompanyInfoData.Delnote(id, key);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }
        #endregion

        public static string ViewChildCompany(int childcomid, int curuserid)
        {
            //当前用户为“平台总账户”才可登录子商户
            if (curuserid == 1035)
            {
                //得到子商户的直销网站经理的账户信息
                B2b_company_manageuser childcompanyuser = new B2bCompanyManagerUserData().Getchildcompanyuser(childcomid);
                if (childcompanyuser != null)
                {
                    ////注销当前账户
                    //UserHelper.DirictLogout();

                    //登录子商户的直销网站经理账户
                    UserHelper.SetCookie(childcompanyuser.Id);


                    return JsonConvert.SerializeObject(new { type = 100, msg = "" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }

            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "只有平台总账户才具有此功能" });
            }
        }

        public static string GetAllCompany()
        {
            try
            {

                int totalcount = 0;
                List<B2b_company> list = new B2bCompanyData().GetAllCompanys(out totalcount);

                return JsonConvert.SerializeObject(new { type = 100, msg = list, totalcount = totalcount });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string Adjustemploerstate(int masterid, int employerstate)
        {
            try
            {

                int result = new B2bCompanyManagerUserData().Adjustemploerstate(masterid, employerstate);

                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string Changestaffpwd(int staffid, string password, B2b_company_manageuser manageruser=null)
        {
            try
            {
                int result = 0;

                if (manageruser != null)
                {
                    result = B2bCompanyManagerUserData.InsertOrUpdate(manageruser);
                }
                else {
                    result = ExcelSqlHelper.ExecuteNonQuery("update b2b_company_manageuser set passwords ='" + password + "' where id=" + staffid);
                }



                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string EditLp(int comid, int lp, int lp_agentlevel)
        {
            try
            {
                int result = ExcelSqlHelper.ExecuteNonQuery("update b2b_company set lp =" + lp + ",lp_agentlevel=" + lp_agentlevel + " where id=" + comid);

                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string Editwxauthorfocus(int comid, string author, string url)
        {
            try
            {
                int result = ExcelSqlHelper.ExecuteNonQuery("update  b2b_company_info set wxfocus_author='" + author + "' ,wxfocus_url='" + url + "' where com_id=" + comid);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }

        public static string Editwxauthorfocus(int comid, int istransfer_customer_service)
        {
            try
            {
                int result = ExcelSqlHelper.ExecuteNonQuery("update  b2b_company_info set Istransfer_customer_service='" + istransfer_customer_service + "'  where com_id=" + comid);
                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }

        public static string Editqunarbycomid(int comid, int isqunar, string qunar_username, string qunar_pass)
        {
            int r = new B2bCompanyData().Editqunarbycomid(comid,isqunar,qunar_username,qunar_pass);
            if (r > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = r });
            }
            else 
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }

        public static string Getqunarbycomid(int comid)
        {
            B2b_company company_qunar = new B2bCompanyData().Getqunarbycomid(comid);
            if(company_qunar!=null)
            {
                return JsonConvert.SerializeObject(new { type=100,msg=company_qunar});
            }
            else
            {
                return JsonConvert.SerializeObject(new { type=1,msg="查询信息失败"});
            }
        }
    }
    public class Model_guwen
    {
        public Model_guwen() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Comid { get; set; }
        public string CompanyName { get; set; }
        public int Atype { get; set; }
        public int Createuserid { get; set; }
        public int Employeestate { get; set; }
        public string Job { get; set; }
        public string Tel { get; set; }
        public int? Channelcompanyid { get; set; }
        public string ChannelCompany { get; set; }
        public int? Channelsource { get; set; }
        public string Selfbrief { get; set; }
        public int Headimg { get; set; }
        public string Headimgurl { get; set; }
        public int Workingyears { get; set; }
        public string Workdays { get; set; }
        public string Workdaystime { get; set; }
        public string Fixphone { get; set; }
        public string Email { get; set; }
        public string Homepage { get; set; }
        public string Weibopage { get; set; }
        public string QQ { get; set; }
        public string Weixin { get; set; }
        public string Selfhomepage_qrcordurl { get; set; }
        public int IsCanZixun { get; set; }
        public double Distance { get; set; }
        public string Workendtime { get; set; }

    }
}

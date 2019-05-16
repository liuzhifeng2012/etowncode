using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Collections;
using ETS2.Permision.Service.PermisionService.Data;
using ETS2.Permision.Service.PermisionService.Model;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Member.Service.MemberService.Model;
using System.Web.Script.Serialization;
using ETS.Data.SqlHelper;
using ETS.Framework;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.VAS.Service.VASService.Data;

namespace ETS.JsonFactory
{
    public class PermissionJsonData
    {
        #region 获取权限列表
        public static string PermissionPageList(int pageindex, int pagesize)
        {
            var totalcount = 0;
            try
            {
                var list = new Sys_ActionData().PermissionPageList(pageindex, pagesize, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from model in list
                             select new
                             {
                                 Id = model.Actionid,
                                 ActionName = model.Actionname,
                                 ActionColumnId = model.Actioncolumnid,
                                 ActionColumnName = model.Actioncolumnname,
                                 ActionUrl = model.Actionurl,
                                 ViewMode = model.Viewmode,
                                 SortId = model.Sortid
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion 获取权限列表
        #region 获取管理组列表
        public static string GroupPageList(int pageindex, int pagesize)
        {
            var totalcount = 0;
            try
            {
                var list = new Sys_GroupData().GroupPageList(pageindex, pagesize, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from model in list
                             select new
                             {
                                 Groupid = model.Groupid,
                                 Groupname = model.Groupname,
                                 Groupinfo = model.Groupinfo,

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

        #region 根据groupid获得管理组信息
        public static string GetGroupById(int groupid)
        {
            try
            {

                var pro = new Sys_GroupData().GetGroupById(groupid);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
        #region 编辑管理组表
        public static string EditGroup(Sys_Group sysgroup)
        {
            try
            {

                var pro = new Sys_GroupData().EditGroup(sysgroup);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
        #region  获得联系人列表
        public static string Masterpagelist(int pageindex, int pagesize, string groupid, int childcomid)
        {
            var totalcount = 0;
            try
            {
                var list = new B2bCompanyManagerUserData().Masterpagelist(pageindex, pagesize, groupid, childcomid, out totalcount);
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
                                 GroupName = new Sys_MasterGroupData().GetGroupNameStrByMasterId(model.Id),
                                 ChannelCompanyName = model.Channelcompanyid == null || model.Channelcompanyid == 0 ? "--" : new MemberChannelcompanyData().GetCompanyById(int.Parse(model.Channelcompanyid.ToString())) == null ? "--" : new MemberChannelcompanyData().GetCompanyById(int.Parse(model.Channelcompanyid.ToString())).Companyname
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
        public static string Masterpagelist(string employstate, int pageindex, int pagesize, string groupid, int childcomid, string key = "")
        {
            var totalcount = 0;
            try
            {
                var list = new B2bCompanyManagerUserData().Masterpagelist(employstate, pageindex, pagesize, groupid, childcomid, out totalcount, key);
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
                                 GroupName = new Sys_MasterGroupData().GetGroupNameStrByMasterId(model.Id),
                                 ChannelCompanyName = model.Channelcompanyid == null || model.Channelcompanyid == 0 ? "--" : new MemberChannelcompanyData().GetCompanyById(int.Parse(model.Channelcompanyid.ToString())) == null ? "--" : new MemberChannelcompanyData().GetCompanyById(int.Parse(model.Channelcompanyid.ToString())).Companyname,
                                 Employeestate = model.Employeestate
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        #region 根据actionid获得权限信息
        public static string GetActionById(int groupid)
        {
            try
            {

                var pro = new Sys_ActionData().GetActionById(groupid);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
        #region 编辑权限
        public static string EditAction(Sys_Action sysaction)
        {
            try
            {

                var pro = new Sys_ActionData().EditAction(sysaction);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
        #region 编辑人员映射表
        public static string EditMasterGroup(string masterid, string mastername, string grouparr, int createmasterid, string createmastername, DateTime createdate)
        {

            try
            {

                var pro = new Sys_MasterGroupData().EditMasterGroup(masterid, mastername, grouparr, createmasterid, createmastername, createdate);
                if (pro > 0)
                {
                    //重新分组，设定人员的所属渠道公司为公司(即channelcompanyid=0)
                    int r = new B2bCompanyManagerUserData().UpChannelcompanyid(masterid,0);

                    return JsonConvert.SerializeObject(new { type = 100, msg = pro });
                }
                else
                {
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
        #region 管理组权限分配
        public static string DistributeAction(int groupid, string selednodeid, int createuserid, string createusername, DateTime createdate, string selednodepId)
        {
            SqlHelper sqlHelper = new SqlHelper();
            sqlHelper.BeginTrancation();

            try
            {

                //删除管理组权限关系表
                string sql1 = "delete Sys_ActionGroup where groupid=" + groupid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql1);
                cmd.ExecuteNonQuery();

                string[] init_pidarr = selednodepId.Split(',');
                string[] init_idarr = selednodeid.Split(',');

                //删除管理组权限右侧子导航关系表
                for (int i = 0; i < init_idarr.Length; i++)
                {
                    if (int.Parse(init_idarr[i]) >= 100000)
                    {
                        string sql2 = "delete sys_groupactionsubnav where groupid=" + groupid + " and actionid=" + init_pidarr[i];
                        cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                        cmd.ExecuteNonQuery();
                    }
                }
                //添加管理组权限关系表
                for (int i = 0; i < init_idarr.Length; i++)
                {
                    if (int.Parse(init_idarr[i]) < 100000)
                    {
                        string sql3 = @"INSERT INTO [dbo].[Sys_ActionGroup]
					   (actionid
					   ,groupid
					   ,masterid
					   ,mastername
					   ,createdate
					   )
				       values(@actionid,@GroupId,@CreateMasterId,@CreateMasterName,@CreateDate)";
                        cmd = sqlHelper.PrepareTextSqlCommand(sql3);
                        cmd.AddParam("@actionid", init_idarr[i]);
                        cmd.AddParam("@GroupId", groupid);
                        cmd.AddParam("@CreateMasterId", createuserid);
                        cmd.AddParam("@CreateMasterName", createusername);
                        cmd.AddParam("@CreateDate", createdate);

                        cmd.ExecuteNonQuery();
                    }
                }


                //添加管理组权限右侧子导航关系表
                for (int i = 0; i < init_idarr.Length; i++)
                {
                    if (int.Parse(init_idarr[i]) >= 100000)
                    {
                        string sql4 = @"INSERT INTO  [sys_groupactionsubnav]
                                           ([groupid]
                                           ,[actionid]
                                           ,[subnavid])
                                     VALUES
                                           (@groupid 
                                           ,@actionid 
                                           ,@subnavid)";
                        cmd = sqlHelper.PrepareTextSqlCommand(sql4);
                        cmd.AddParam("@groupid", groupid);
                        cmd.AddParam("@actionid", init_pidarr[i]);
                        cmd.AddParam("@subnavid", init_idarr[i]);
                        cmd.ExecuteNonQuery();
                    }
                }

                sqlHelper.Commit();
                sqlHelper.Dispose();
                return JsonConvert.SerializeObject(new { type = 100, msg = "" });
            }
            catch (Exception ex)
            {
                sqlHelper.Rollback();
                sqlHelper.Dispose();
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }
        #endregion

        #region 得到所在公司的人员信息

        public static string MasterpagelistByComId(int pageindex, int pagesize, string comid, int userid)
        {
            var totalcount = 0;
            try
            {
                var list = new B2bCompanyManagerUserData().Manageuserpagelist(comid, pageindex, pagesize, out totalcount, userid);
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
                                 GroupName = new Sys_MasterGroupData().GetGroupNameStrByMasterId(model.Id)
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


        public static string ChangeIsDefaultKf(int userid)
        {
            try
            {
                var manageuserdata = new B2bCompanyManagerUserData();
                var managuserinfo = manageuserdata.GetCompanyUser(userid);

                if (managuserinfo == null)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "员工信息错误" });
                }

                int Isdefaultkf = 0;
                if (managuserinfo.Isdefaultkf == 0)
                {
                    Isdefaultkf = 1;
                }
                else
                {
                    Isdefaultkf = 0;
                }


                var list = B2bCompanyManagerUserData.ChangeIsDefaultKf(userid, Isdefaultkf);

                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string MasterpagelistByComId(string employstate, int pageindex, int pagesize, string comid, int userid, string key = "")
        {
            var totalcount = 0;
          
            var memberdata = new MemberChannelData();
            MemberChannelcompanyData channel = new MemberChannelcompanyData();
            try
            {


                var list = new B2bCompanyManagerUserData().Manageuserpagelist(employstate, comid, pageindex, pagesize, out totalcount, userid, key);
                IEnumerable result = "";
                if (list != null)

                    result = from model in list
                             select new
                             {
                                 model.Id,
                                 Accounts = model.Accounts,
                                 PassWord = model.Passwords,
                                 MasterId = model.Id,
                                 MasterName = model.Employeename,
                                 CompanyName = channel.GetCompanyById(Int32.Parse(model.Channelcompanyid.ToString())) != null ? channel.GetCompanyById(Int32.Parse(model.Channelcompanyid.ToString())).Companyname : "",
                                 Tel = model.Tel,
                                 GroupName = new Sys_MasterGroupData().GetGroupNameStrByMasterId(model.Id),
                                 Employstate = model.Employeestate,
                                 weixinstate = memberdata.GetChannelListByComidState(int.Parse(comid), model.Id),
                                 Peoplelistview = model.Peoplelistview,
                                 Workdays = model.Workdays,
                                 Isdefaultkf = model.Isdefaultkf,

                                 Channelid=new MemberChannelData().GetChannelid(model.Com_id,model.Tel),//渠道id
                                 rebatenum=new Member_channel_rebatelogData().Getrebatenum(model.Com_id,model.Tel), //返佣次数 
                                 rebateapplytotal = new Member_channel_rebateApplylogData().Getrebateapplytotal(model.Com_id, model.Tel), //返佣申请总额
                                 rebatehastixian = new Member_channel_rebateApplylogData().Getrebatehastixian(model.Com_id, model.Tel),   //返佣申请已提现金额
                                 rebatenottixian = new Member_channel_rebateApplylogData().Getrebatenottixian(model.Com_id, model.Tel),   //返佣申请尚未提现金额
                                 restrebate = new MemberChannelData().Getrestrebate(model.Com_id, model.Tel),        //剩余可申请返佣金额
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string EditUserAndMasterGroup(B2b_company_manageuser manageuser, string masterid, string mastername, string grouparr, int createmasterid, string createmastername, DateTime createdate)
        {
            try
            {
                //员工渠道管理，关联说明。
                //以员工位主表，渠道为从表，添加，修改时都先判断员工表，是否有此账户或手机，有则错误返回。
                //当添加或修改员工时，再判断渠道表，如果渠道没有则添加，如果有，在添加操作时，不操作，在修改的时候同时修改。


                if (manageuser.Id == 0)//添加操作
                {
                    //判断添加员工时 员工表中账户是否存在，避免账户重复问题
                    bool ishasaccount = new B2bCompanyManagerUserData().Ishasaccount(manageuser.Accounts);
                    if (ishasaccount)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "账户已存在，请更换登录账户名" });
                    }
                    //判断添加员工时 员工表中手机是否存在，避免手机重复
                    bool ishasphone1 = new B2bCompanyManagerUserData().Ishasphone(manageuser.Tel, manageuser.Com_id);

                    if (ishasphone1 == false)
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "已经有此手机账户了,请修改此手机账户信息" });
                    }
                }
                else //编辑操作
                {
                    if (manageuser.Tel != manageuser.OldTel)//更换手机添加
                    {
                        //新手机判断是否有重复的
                        bool ishasphone1 = new B2bCompanyManagerUserData().Ishasphone(manageuser.Tel, manageuser.Com_id);
                        if (ishasphone1 == false)
                        {
                            return JsonConvert.SerializeObject(new { type = 1, msg = "已经有此手机账户了,修改失败" });
                        }
                    }
                }


                var data = B2bCompanyManagerUserData.InsertOrUpdate(manageuser);
                if (data > 0)
                {
                    var dataa = new Sys_MasterGroupData().EditMasterGroup(data.ToString(), mastername, grouparr, createmasterid, createmastername, createdate);

                    var channelid = 0;

                    //判断是否含有当前渠道信息
                    if (manageuser.Id == 0)//添加员工操作
                    {
                        int cid = 0;
                        var channnel = new MemberChannelData().Ishasphone(manageuser.Tel, manageuser.Com_id, out cid);
                        if (channnel)
                        {
                            //添加渠道信息
                            channelid = new MemberChannelData().EditSimplyChannel(0, manageuser.Com_id, manageuser.Employeename, manageuser.Channelcompanyid.ToString(), manageuser.Tel, manageuser.Channelsource.ToString().ConvertTo<int>(0),manageuser.Employeestate);

                        }
                        else
                        {
                            //编辑渠道信息
                            channelid = new MemberChannelData().EditSimplyChannel(cid, manageuser.Com_id, manageuser.Employeename, manageuser.Channelcompanyid.ToString(), manageuser.Tel, manageuser.Channelsource.ToString().ConvertTo<int>(0),manageuser.Employeestate);
                        }

                    }
                    else
                    {//修改员工信息时

                        //首先判断手机是否更换
                        if (manageuser.Tel == manageuser.OldTel)//没有更换就直接查找渠道，无此手机添加，由此手机更新
                        {
                            int cid = 0;
                            var channnel = new MemberChannelData().Ishasphone(manageuser.Tel, manageuser.Com_id, out cid);//先判断
                            if (channnel)
                            {
                                //添加渠道信息
                                channelid = new MemberChannelData().EditSimplyChannel(0, manageuser.Com_id, manageuser.Employeename, manageuser.Channelcompanyid.ToString(), manageuser.Tel, manageuser.Channelsource.ToString().ConvertTo<int>(0), manageuser.Employeestate);
                            }
                            else
                            {
                                //编辑渠道信息
                                channelid = new MemberChannelData().EditSimplyChannel(cid, manageuser.Com_id, manageuser.Employeename, manageuser.Channelcompanyid.ToString(), manageuser.Tel, manageuser.Channelsource.ToString().ConvertTo<int>(0), manageuser.Employeestate);
                            }
                        }
                        else
                        { //更换新手机，先查询新手机是否使用，如果未使用
                            int cid = 0;
                            var channnel = new MemberChannelData().Ishasphone(manageuser.Tel, manageuser.Com_id, out cid);//先判断
                            if (channnel)
                            {
                                var channnel2 = new MemberChannelData().Ishasphone(manageuser.OldTel, manageuser.Com_id, out cid);//获取老手机渠道信息，
                                if (channnel2)
                                {
                                    //未查询到老手机，直接添加新手机添加渠道信息
                                    channelid = new MemberChannelData().EditSimplyChannel(0, manageuser.Com_id, manageuser.Employeename, manageuser.Channelcompanyid.ToString(), manageuser.Tel, manageuser.Channelsource.ToString().ConvertTo<int>(0), manageuser.Employeestate);
                                }
                                else
                                {
                                    //对老的手机渠道信息更新编辑渠道信息
                                    channelid = new MemberChannelData().EditSimplyChannel(cid, manageuser.Com_id, manageuser.Employeename, manageuser.Channelcompanyid.ToString(), manageuser.Tel, manageuser.Channelsource.ToString().ConvertTo<int>(0), manageuser.Employeestate);
                                }
                            }
                            else
                            { //如果已使用，则不操作渠道，等于员工和新手机渠道绑定

                            }
                        }

                    }
                    if (channelid > 0)
                    {
                        Member_Channel m_channel = new MemberChannelData().GetChannelDetail(channelid);
                        if (m_channel != null)
                        {
                            //判断公司微信是否是 已认证服务号
                            bool isrz_fuwuno = new MemberChannelData().IsRz_fuwuno(manageuser.Com_id);
                            if (isrz_fuwuno)
                            {
                                //判断渠道是否已经生成过参数二维码
                                bool iscreate_paramqrcode = new WxSubscribeSourceData().Ishascreate_paramqrcode(channelid);
                                if (iscreate_paramqrcode == false)
                                {
                                    JavaScriptSerializer ser = new JavaScriptSerializer();
                                    ResultClass foo = ser.Deserialize<ResultClass>(WeiXinJsonData.EditWxQrcode(0, manageuser.Com_id, "系统生成" + m_channel.Name + m_channel.Mobile, 0, m_channel.Companyid, channelid, 1));

                                }
                            }
                        }
                    }

                    if (dataa > 0)
                    {
                        return JsonConvert.SerializeObject(new { type = 100, msg = dataa });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { type = 1, msg = "编辑账户信息时出现意外错误" });
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "编辑账户信息时出现意外错误." });
                }


            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string GetAllGroups()
        {
            var totalcount = 0;
            try
            {
                var list = new Sys_GroupData().GetAllGroups(out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from model in list
                             select new
                             {
                                 Groupid = model.Groupid,
                                 Groupname = model.Groupname,
                                 Groupinfo = model.Groupinfo,

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        internal static string GetCompanyIdByUserId(string userid)
        {
            string companyid = "0";

            //判断用户是否是管理员，如是管理员，不对用户渠道公司限制
            Sys_Group sysgroup = new Sys_GroupData().GetGroupByUserId(int.Parse(userid));
            if (sysgroup.Groupid != 1)//不是管理员
            {
                //根据userid得到用户信息，如果用户没有渠道公司的分配，则显示全部门市
                B2b_company_manageuser muser = B2bCompanyManagerUserData.GetUser(int.Parse(userid));

                companyid = muser.Channelcompanyid.ToString();

            }
            return companyid;
        }

        public static string GetGroupByUserId(int userid)
        {
            try
            {
                var pro = new Sys_GroupData().GetGroupByUserId(userid);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string DelActionById(int actionid)
        {
            try
            {
                var dataa = new Sys_ActionData().DelActionById(actionid);


                if (dataa > 0)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = dataa });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = dataa });
                }

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string DelGroupById(int groupid)
        {
            try
            {
                var dataa = new Sys_GroupData().DelGroupById(groupid);


                if (dataa > 0)
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = dataa });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = dataa });
                }

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        //设定员工所在分组
        public static bool SetMasterGroup(string masterid, string groupname, out string msg)
        {

            B2b_company_manageuser user = B2bCompanyManagerUserData.GetUser(int.Parse(masterid));
            if (user == null)
            {
                msg = "账户信息获取为空";
                return false;
            }
            else
            {
                string mastername = user.Employeename;

                Sys_Group group = new Sys_GroupData().GetGroupByName(groupname.Trim());
                if (group == null)
                {
                    msg = "管理组获取为空";
                    return false;
                }
                else
                {
                    string grouparr = group.Groupid.ToString();
                    int createmasterid = 0;
                    string createmastername = "开户后系统自动创建";
                    DateTime createdate = DateTime.Now;

                    int count = new Sys_MasterGroupData().EditMasterGroup(masterid, mastername, grouparr, createmasterid, createmastername, createdate);

                    if (count > 0)
                    {
                        msg = "";
                        return true;
                    }
                    else
                    {
                        msg = "设定账户权限失败";
                        return false;
                    }
                }
            }
        }
        public class ResultClass
        {
            public string type;
            public string msg;
        }
        /// <summary>
        /// //如果是门市 ，则显示门市经理 权限内的管理组列表
        ///如果是合作单位 ，则显示合作单位负责人  权限内的管理组列表
        ///如果是公司 ，根据登录账户角色判断其可以显示的管理组列表 
        /// </summary>
        /// <param name="channelsource"></param>
        /// <returns></returns>
        public static string GetGroupBychannelsource(string channelsource, int userid = 0)
        {
            try
            {
                var pro = new Sys_GroupData().GetGroupBychannelsource(channelsource, userid);

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string Getallactioncolumns()
        {
            try
            {
                var pro = new Sys_ActionColumnData().Getallactioncolumns();

                return JsonConvert.SerializeObject(new { type = 100, msg = pro });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }


        public static string Getsyssubnav(int subnavid)
        {
            try
            {
                var m = new Sys_subnavData().Getsyssubnav(subnavid);

                return JsonConvert.SerializeObject(new { type = 100, msg = m });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string Permissionlist(int columnid)
        {
            try
            {
                var m = new Sys_ActionData().Permissionlist(columnid);

                return JsonConvert.SerializeObject(new { type = 100, msg = m });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }

        public static string Editsys_subnav(int id, int actionid, int columnid, string subnavurl, string subnavname)
        {
            //插入操作需要判断是否有重复的子导航(名字 或者 相对路径 相同)
            if (id == 0)
            {
                int num = new Sys_subnavData().GetsubnavNum(subnavname, subnavurl);
                if (num > 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "子导航在库中已经含有" });
                }
            }
            int r = new Sys_subnavData().Editsys_subnav(id, actionid, columnid, subnavurl, subnavname);
            if (r == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = r });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "" });
            }
        }

        public static string Getsys_subnavpagelist(int pageindex, int pagesize, int columnid, int actionid)
        {
            int totalcount = 0;
            IList<Sys_subnav> list = new Sys_subnavData().Getsys_subnavpagelist(pageindex, pagesize, columnid, actionid, out totalcount);
            if (list != null)
            {
                if (list.Count == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
                else
                {
                    IEnumerable r = "";
                    r = from m in list
                        select new
                        {
                            m.id,
                            m.subnav_name,
                            m.subnav_url,
                            m.actioncolumnid,
                            actioncolumnname = Getactioncolumnname(m.actioncolumnid),
                            m.actionid,
                            actionname = GetActionname(m.actionid),
                            m.viewcode,
                            m.sortid,
                            //含有此导航的管理组的 连接字符串
                            groupids = new Sys_groupactionsubnavData().GetGroupsByactionsubnavid(m.actionid, m.id)
                        };
                    return JsonConvert.SerializeObject(new { type = 100, msg = r, totalcount = totalcount });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }

        private static string Getactioncolumnname(int actioncolumnid)
        {
            Sys_ActionColumn m = new Sys_ActionColumnData().GetActionColumn(actioncolumnid);
            if (m == null)
            {
                return "";
            }
            else
            {
                return m.Actioncolumnname;
            }
        }

        private static string GetActionname(int actionid)
        {
            Sys_Action m = new Sys_ActionData().GetActionById(actionid);
            if (m == null)
            {
                return "";
            }
            else
            {
                return m.Actionname;
            }
        }

        public static string Upsubnavviewcode(int subnavid, int viewcode, int actionid, string groupids)
        {
            int r = new Sys_subnavData().Upsubnavviewcode(subnavid, viewcode, actionid, groupids);
            if (r > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }

        public static string Getsys_subnavlistbyvirtualurl(string virtualurl, int viewcode, int groupid, string parastr)
        {
            IList<Sys_subnav> list = new Sys_subnavData().Getsys_subnavlistbyvirtualurl(virtualurl, viewcode, groupid, parastr);
            if (list.Count == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
        }

        public static string Delsubnav(int subnavid)
        {
            SqlHelper sqlHelper = new SqlHelper();
            sqlHelper.BeginTrancation();
            try
            {
                string sql1 = "update  [sys_subnav] set viewcode=0, actioncolumnid=0,actionid=0 where id=" + subnavid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql1);
                cmd.ExecuteNonQuery();

                string sql2 = "delete sys_groupactionsubnav where subnavid=" + subnavid;
                cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                cmd.ExecuteNonQuery();

                sqlHelper.Commit();
                sqlHelper.Dispose();
                return JsonConvert.SerializeObject(new { type = 100, msg = "" });
            }
            catch
            {
                sqlHelper.Dispose();
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }

        public static string Getallsys_subnavpagelist(int pageindex, int pagesize, int seled_columnid, int seled_actionid)
        {
            int totalcount = 0;
            IList<Sys_subnav> list = new Sys_subnavData().Getallsys_subnavpagelist(pageindex, pagesize, out totalcount);
            if (list != null)
            {
                if (list.Count == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "" });
                }
                else
                {
                    IEnumerable r = "";
                    r = from m in list
                        select new
                        {
                            m.id,
                            m.subnav_name,
                            m.subnav_url,
                            m.actioncolumnid,
                            actioncolumnname = Getactioncolumnname(m.actioncolumnid),
                            m.actionid,
                            actionname = GetActionname(m.actionid),
                            m.viewcode,
                            //子导航在当前权限下面
                            isunderaction = GetIsunderaction(m.actioncolumnid, m.actionid, seled_columnid, seled_actionid),
                            m.sortid,
                            //子导航所在的管理组 的连接字符串
                            groupids = new Sys_groupactionsubnavData().GetGroupsByactionsubnavid(m.actionid, m.id)
                        };
                    return JsonConvert.SerializeObject(new { type = 100, msg = r, totalcount = totalcount });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }

        /// <summary>
        /// 如果右侧子导航在当前权限下 
        /// </summary>
        /// <param name="viewcode"></param>
        /// <param name="actioncolumnid"></param>
        /// <param name="actionid"></param>
        /// <param name="seled_columnid"></param>
        /// <param name="seled_actionid"></param>
        /// <returns></returns>
        private static int GetIsunderaction(int actioncolumnid, int actionid, int seled_columnid, int seled_actionid)
        {
            if (actioncolumnid == seled_columnid & actionid == seled_actionid)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static string Upsubnavdatabase(int subnavid, int oldviewcode, int oldcolumnid, int oldactionid, string oldgroupids, int newviewcode, int newcolumnid, int newactionid)
        {
            int r = new Sys_subnavData().Upsubnavdatabase(subnavid, oldviewcode, oldcolumnid, oldactionid, oldgroupids, newviewcode, newcolumnid, newactionid);
            if (r == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "" });
            }
        }

        public static string Getsys_subnav(string vurl, string parastr)
        {
            Sys_subnav r = new Sys_subnavData().Getsys_subnav(vurl, parastr);
            if (r == null)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = r });
            }
        }

        public static string Channelrebatelist(int pageindex, int pagesize, int channelid, string payment)
        {
            int totalcount = 0;
            IList<Member_channel_rebatelog> list = new Member_channel_rebatelogData().Channelrebatelist(pageindex, pagesize, channelid, payment, out totalcount);
            if (list.Count > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = list, totalcount = totalcount });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }



        public static string GetchanelrebateApplyaccount(int channelid)
        {
            Member_channel_rebateApplyaccount m = new Member_channel_rebateApplyaccountData().GetchanelrebateApplyaccount(channelid);
            if (m != null)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = m });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }

        public static string Upchannelrebateaccount(int channelid, string truename, string account, string newphone, int comid)
        {
            int r = new Member_channel_rebateApplyaccountData().Upchannelrebateaccount(channelid, truename, account, newphone, comid);
            if (r > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = r });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }

        public static string Channelapplyrebate(int channelid, string applytype, string applydetail, decimal applymoney, int comid)
        {
            Member_Channel m = new MemberChannelData().GetChannelDetail(channelid);
            if (m != null)
            {
                decimal rebatemoney = m.Rebatemoney;
                if (rebatemoney < applymoney)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "提现金额不可大于账户余额" });
                }

                Member_channel_rebateApplylog applylog = new Member_channel_rebateApplylog
                {
                    id = 0,
                    applytime = DateTime.Now,
                    applytype = applytype,
                    applydetail = applydetail,
                    applymoney = applymoney,
                    channelid = channelid,
                    operstatus = 0,
                    comid = comid
                };
                int r = new Member_channel_rebateApplylogData().Insrebateapplylog(applylog);
                if (r > 0)
                {
                    ////账户提现，返佣表和 渠道表变动

                    //获得渠道人的返佣余额
                    decimal channelrebatemoney = new Member_channel_rebatelogData().Getrebatemoney(channelid);
                    //返佣记录
                    Member_channel_rebatelog rebatelog = new Member_channel_rebatelog
                    {
                        id = 0,
                        channelid = channelid,
                        orderid = 0,
                        payment = 2,
                        payment_type = "返佣提现",
                        proid = "0",
                        proname = applytype + "-" + applydetail,
                        subdatetime = DateTime.Now,
                        ordermoney = 0,
                        rebatemoney = -applymoney,
                        over_money = decimal.Round(channelrebatemoney - applymoney, 2),
                        comid = comid
                    };
                    //增加返佣记录 同时增加渠道人的返佣金额
                    new Member_channel_rebatelogData().Editrebatelog(rebatelog);
                    new Member_channel_rebatelogData().Editchannelrebate(rebatelog.channelid, rebatelog.over_money);


                    return JsonConvert.SerializeObject(new { type = 100, msg = "成功" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "意外错误" });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "意外错误" });
            }
        }

        public static string Channelrebateapplylist(int pageindex, int pagesize, int channelid, string operstatus)
        {
            int totalcount = 0;
            IList<Member_channel_rebateApplylog> list = new Member_channel_rebateApplylogData().Channelrebateapplylist(pageindex, pagesize, channelid, operstatus, out totalcount);
            if (list.Count > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = list, totalcount = totalcount });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }

        public static string Channelrebateapplyalllist(int pageindex, int pagesize, int comid, string operstatus)
        {
            int totalcount = 0;
            IList<Member_channel_rebateApplylog> list = new Member_channel_rebateApplylogData().Channelrebateapplyalllist(pageindex, pagesize, comid, operstatus, out totalcount);
            if (list.Count > 0)
            {
                var result = from b in list
                             select new
                             {
                                 b.id,
                                 b.applytime,
                                 b.applytype,
                                 b.applydetail,
                                 b.applymoney,
                                 b.opertor,
                                 b.opertime,
                                 b.operstatus,
                                 b.operremark,
                                 b.zhuanzhangsucimg,
                                 zhuanzhangsucimgurl = FileSerivce.GetImgUrl(b.zhuanzhangsucimg),
                                 b.comid,
                                 b.channelid,
                                 channelinfo = new Member_channel_rebateApplyaccountData().GetchanelrebateApplyaccount(b.channelid),
                             };

                return JsonConvert.SerializeObject(new { type = 100, msg = result, totalcount = totalcount });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }

        public static string Confirmcompletedakuan(int id, int operstatus, int opertor, string operremark, int zhuanzhangsucimg)
        {
            int r = new Member_channel_rebateApplylogData().Confirmcompletedakuan(id, operstatus, opertor, operremark, zhuanzhangsucimg);
            if (r > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = "成功" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }
    }
}

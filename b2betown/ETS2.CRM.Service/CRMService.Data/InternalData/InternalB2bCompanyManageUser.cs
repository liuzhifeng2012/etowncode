using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;
using System.Data;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    public class InternalB2bCompanyManageUser
    {
        private SqlHelper sqlHelper;
        public InternalB2bCompanyManageUser(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        #region 添加或者编辑 商家员工信息
        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateB2bCompanyUser";
        public int InsertOrUpdate(B2b_company_manageuser model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Com_id", model.Com_id);
            cmd.AddParam("@Accounts", model.Accounts);
            cmd.AddParam("@Passwords", model.Passwords);
            cmd.AddParam("@Atype", model.Atype);
            cmd.AddParam("@Employeename", model.Employeename);
            cmd.AddParam("@Tel", model.Tel);
            cmd.AddParam("@Viewtel", model.Viewtel);
            cmd.AddParam("@Employeestate", model.Employeestate);
            cmd.AddParam("@Createuserid", model.Createuserid);
            cmd.AddParam("@channelcompanyid", model.Channelcompanyid);
            cmd.AddParam("@channelsource", model.Channelsource);

            cmd.AddParam("@Headimg", model.Headimg);
            cmd.AddParam("@Workingyears", model.Workingyears);
            cmd.AddParam("@Selfbrief", model.Selfbrief);
            cmd.AddParam("@Workdays", model.Workdays);
            cmd.AddParam("@Workdaystime", model.Workdaystime);
            cmd.AddParam("@Workendtime", model.Workendtime);
            cmd.AddParam("@Fixphone", model.Fixphone);
            cmd.AddParam("@Email", model.Email);
            cmd.AddParam("@Homepage", model.Homepage);
            cmd.AddParam("@Weibopage", model.Weibopage);
            cmd.AddParam("@QQ", model.QQ);
            cmd.AddParam("@Weixin", model.Weixin);
            cmd.AddParam("@Job", model.Job);
            cmd.AddParam("@Peoplelistview", model.Peoplelistview);

            cmd.AddParam("@worktimestar", model.worktimestar);
            cmd.AddParam("@worktimeend", model.worktimeend);
            cmd.AddParam("@workendtimestar", model.workendtimestar);
            cmd.AddParam("@workendtimeend", model.workendtimeend);

            cmd.AddParam("@bindingproid", model.bindingproid);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion


        #region 根据标识列获得 商家员工信息
        /// <summary>
        /// 根据标识列获得 商家员工信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public B2b_company_manageuser GetUser(int userId)
        {
            const string sqlTxt = @"SELECT [id]
      ,[com_id]
      ,[Accounts]
      ,[passwords]
      ,[atype]
      ,[employeename]
      ,[tel]
      ,viewtel
      ,[employeestate]
      ,[createuserid]
      ,[channelcompanyid]
      ,[channelsource]
      ,[job]
      ,[selfbrief]
      ,[headimg]
      ,[workingyears]
      ,[workdays]
      ,[workdaystime]
      ,[workendtime]
      ,[fixphone]
      ,[email]
      ,[homepage]
      ,[weibopage]
      ,[QQ]
      ,[weixin]
      ,[selfhomepage_qrcordurl]
      ,[Peoplelistview]
,[worktimestar]
,[worktimeend]
,[workendtimestar]
,[workendtimeend]
,bindingproid
  FROM [EtownDB].[dbo].[b2b_company_manageuser]  where [id] = @id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@id", userId);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_company_manageuser u = null;

                while (reader.Read())
                {
                    u = new B2b_company_manageuser
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Accounts = reader.GetValue<string>("Accounts"),
                        Passwords = reader.GetValue<string>("passwords"),
                        Atype = reader.GetValue<int>("atype"),
                        Employeename = reader.GetValue<string>("employeename"),
                        Job = reader.GetValue<string>("job"),
                        Tel = reader.GetValue<string>("tel"),
                        Employeestate = reader.GetValue<int>("employeestate"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        Channelcompanyid = reader.GetValue<int?>("channelcompanyid") == null ? 0 : reader.GetValue<int?>("channelcompanyid"),
                        Channelsource = reader.GetValue<int?>("channelsource") == null ? 0 : reader.GetValue<int?>("channelsource"),

                        Selfbrief = reader.GetValue<string>("Selfbrief"),
                        Headimg = reader.GetValue<int>("Headimg"),
                        Workingyears = reader.GetValue<int>("Workingyears"),
                        Workdays = reader.GetValue<string>("Workdays"),
                        Workdaystime = reader.GetValue<string>("Workdaystime"),
                        Workendtime = reader.GetValue<string>("Workendtime"),
                        Fixphone = reader.GetValue<string>("Fixphone"),
                        Email = reader.GetValue<string>("Email"),
                        Homepage = reader.GetValue<string>("Homepage"),
                        Weibopage = reader.GetValue<string>("Weibopage"),
                        QQ = reader.GetValue<string>("QQ"),
                        Weixin = reader.GetValue<string>("Weixin"),
                        Selfhomepage_qrcordurl = reader.GetValue<string>("Selfhomepage_qrcordurl"),
                        Peoplelistview = reader.GetValue<int>("Peoplelistview"),
                        Viewtel = reader.GetValue<int>("Viewtel"),
                        worktimestar = reader.GetValue<int>("worktimestar"),
                        worktimeend = reader.GetValue<int>("worktimeend"),
                        workendtimestar = reader.GetValue<int>("workendtimestar"),
                        workendtimeend = reader.GetValue<int>("workendtimeend"),
                        bindingproid = reader.GetValue<int>("bindingproid"),

                    };

                }
                return u;
            }
        }
        #endregion
        #region 验证用户是否存在
        private static readonly string SQL_VerifyUser = @"select  a.id,a.com_id,a.Accounts,a.passwords,a.atype,a.employeestate from  dbo.b2b_company_manageuser a 
                                       left outer join  dbo.b2b_company b on a.com_id=b.ID   
                                       where a.Accounts=@Accounts and a.passwords=@passwords";

        private static readonly string SQL_VerifyUserName = @"SELECT * FROM dbo.b2b_company_manageuser a left outer JOIN dbo.b2b_company b ON a.com_id = b.ID
                                                    WHERE a.Accounts = @Accounts";
        public B2b_company_manageuser VerfyUser(string username, string password, out string message)
        {
            var command = sqlHelper.PrepareTextSqlCommand(SQL_VerifyUserName);
            command.AddParam("@Accounts", username);
            using (var er = command.ExecuteReader())
            {
                if (er.HasRows)
                {
                    er.Close();

                    var cmd = sqlHelper.PrepareTextSqlCommand(SQL_VerifyUser);
                    cmd.AddParam("@Accounts", username);
                    cmd.AddParam("@passwords", password);

                    using (var reader = cmd.ExecuteReader())
                    {
                        B2b_company_manageuser u = null;
                        if (reader.Read())
                        {
                            message = "";
                            u = new B2b_company_manageuser
                            {
                                Id = reader.GetValue<int>("id"),
                                Com_id = reader.GetValue<int>("com_id"),
                                Accounts = reader.GetValue<string>("Accounts"),
                                Passwords = reader.GetValue<string>("passwords"),
                                Atype = reader.GetValue<int>("atype"),
                                Employeestate = reader.GetValue<int>("employeestate")
                            };

                            if (u.Employeestate == 0)//账户已注销
                            {
                                message = "账户已注销";

                                return null;
                            }
                            else
                            {
                                return u;
                            }

                        }
                        message = "用户密码错误";

                        return null;
                    }
                }
                else
                {
                    message = "用户名不存在";

                    return null;
                }
            }
        }
        #endregion

        #region 获得商户第一个账户也就是 开户账户
        private static readonly string SQL_GetFirstAccountUser = @"SELECT top 1 * FROM dbo.b2b_company_manageuser a 
                                                    WHERE a.com_id = @comid order by id ";
        public string GetFirstAccountUser(int comid)
        {
            var command = sqlHelper.PrepareTextSqlCommand(SQL_GetFirstAccountUser);
            command.AddParam("@comid", comid);
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<string>("Accounts");
                }
            }
            return "";
        }
        #endregion


        #region 获得商户 开户账户 的ID
        private static readonly string SQL_GetFirstIDUser = @"SELECT top 1 * FROM dbo.b2b_company_manageuser a 
                                                    WHERE a.com_id = @comid order by id ";
        public int GetFirstIDUser(int comid)
        {
            var command = sqlHelper.PrepareTextSqlCommand(SQL_GetFirstIDUser);
            command.AddParam("@comid", comid);
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("id");
                }
            }
            return 0;
        }
        #endregion


        #region 判断手机是否存在
        private static readonly string SQL_SearchPhone = @"SELECT * FROM dbo.b2b_company_manageuser a 
                                                    WHERE a.tel = @moblie and a.com_id=@comid";
        public bool Ishasphone(string moblie, int comid)
        {
            var command = sqlHelper.PrepareTextSqlCommand(SQL_SearchPhone);
            command.AddParam("@moblie", moblie);
            command.AddParam("@comid", comid);
            using (var er = command.ExecuteReader())
            {
                if (er.HasRows)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        #endregion


        #region 查看原密码是否存在
        internal int CheckOldPwd(int userid, string oldpwd)
        {
            const string sqlTxt = "select count(1)  from dbo.b2b_company_manageuser a where a.passwords=@pwd and a.id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@id", userid);
            cmd.AddParam("@pwd", oldpwd);

            return (int)cmd.ExecuteScalar();
        }
        #endregion
        #region 修改老密码
        internal int ChangePwd(int userid, string pwd1)
        {
            const string sqlTxt = "update dbo.b2b_company_manageuser set passwords=@pwd where  id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@id", userid);
            cmd.AddParam("@pwd", pwd1);

            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region 修改默认客服
        internal int ChangeIsDefaultKf(int userid, int IsDefaultKf)
        {
            const string sqlTxt = "update dbo.b2b_company_manageuser set IsDefaultKf=@IsDefaultKf where  id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@id", userid);
            cmd.AddParam("@IsDefaultKf", IsDefaultKf);

            return cmd.ExecuteNonQuery();
        }
        #endregion


        internal List<B2b_company_manageuser> Manageuserpagelist(string comid, int pageindex, int pagesize, out int totalcount, int userid = 0)
        {
            //var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            //var tblName = "b2b_company_manageuser";
            //var strGetFields = "*";
            //var sortKey = "id";
            //var sortMode = "1";
            //var condition = "com_id=" + comid;
            //cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var Condition = "employeestate=1";
            if (userid > 0)
            {
                B2b_company_manageuser userr = B2bCompanyManagerUserData.GetUser(userid);
                if (userr != null)
                {
                    if (userr.Channelcompanyid == 0)//总公司账户，根据comid得到crm
                    {
                        Condition += "and com_id=" + comid;
                    }
                    else //总公司下面渠道，渠道表+卡号表+会员表连接查询得到渠道公司(门市)下的会员信息
                    {
                        Condition += "and channelcompanyid=" + userr.Channelcompanyid + " and com_id=" + comid;
                    }
                }
            }
            cmd.PagingCommand1("b2b_company_manageuser", "*", "id desc", "", pagesize, pageindex, "", Condition);

            List<B2b_company_manageuser> list = new List<B2b_company_manageuser>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company_manageuser
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Accounts = reader.GetValue<string>("Accounts"),
                        Atype = reader.GetValue<int>("Atype"),
                        Createuserid = reader.GetValue<int>("Createuserid"),
                        Employeename = reader.GetValue<string>("Employeename"),
                        Employeestate = reader.GetValue<int>("Employeestate"),
                        Job = reader.GetValue<string>("Job"),
                        Passwords = reader.GetValue<string>("Passwords"),
                        Tel = reader.GetValue<string>("Tel"),

                    });

                }
            }
            //totalcount = int.Parse(cmd.Parameters[7].Value.ToString());
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }

        internal List<B2b_company_manageuser> Manageuserpagelist(string employstate, string comid, int pageindex, int pagesize, out int totalcount, int userid = 0, string key = "")
        {

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var Condition = "employeestate in (" + employstate + ")";
            if (userid > 0)
            {
                B2b_company_manageuser userr = B2bCompanyManagerUserData.GetUser(userid);
                if (userr != null)
                {
                    if (userr.Channelcompanyid == 0)//总公司账户，根据comid得到crm
                    {
                        Condition += " and com_id=" + comid;
                    }
                    else //总公司下面渠道，渠道表+卡号表+会员表连接查询得到渠道公司(门市)下的会员信息
                    {
                        Condition += " and channelcompanyid=" + userr.Channelcompanyid + " and com_id=" + comid;
                    }
                }
            }

            if (key != "")
            {
                Condition += " and (accounts='" + key + "' or employeename='" + key + "' or tel='" + key + "' or Channelcompanyid in (SELECT [id] FROM [Member_Channel_company] where Companyname like '%" + key + "%' ))";
            }


            cmd.PagingCommand1("b2b_company_manageuser", "*", "id desc", "", pagesize, pageindex, "", Condition);

            List<B2b_company_manageuser> list = new List<B2b_company_manageuser>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company_manageuser
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Accounts = reader.GetValue<string>("Accounts"),
                        Atype = reader.GetValue<int>("Atype"),
                        Createuserid = reader.GetValue<int>("Createuserid"),
                        Employeename = reader.GetValue<string>("Employeename"),
                        Employeestate = reader.GetValue<int>("Employeestate"),
                        Job = reader.GetValue<string>("Job"),
                        Passwords = reader.GetValue<string>("Passwords"),
                        Tel = reader.GetValue<string>("Tel"),
                        Channelcompanyid = reader.GetValue<int>("Channelcompanyid"),
                        Peoplelistview = reader.GetValue<int>("peoplelistview"),
                        Workdays = reader.GetValue<string>("workdays"),
                        Isdefaultkf = reader.GetValue<int>("Isdefaultkf"),
                        bindingproid = reader.GetValue<int>("bindingproid"),
                    });

                }
            }
            //totalcount = int.Parse(cmd.Parameters[7].Value.ToString());
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }


        internal List<B2b_company_manageuser> ViewChanneluserpagelist(int comid, int channelcompanyid, int pageindex, int pagesize, out int totalcount, string key = "", string openid = "", string usern = "", string usere = "", int isheadofficekf = 0, int isonlycoachlist=0,string isviewjiaolian="0,1")
        {

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var Condition = "a.com_id= " + comid + " and  b.com_id= " + comid + " and   c.comid= " + comid + " and a.Employeestate=1 and a.peoplelistview=1";

            if (channelcompanyid != 0)
            {
                Condition += " and a.Channelcompanyid= " + channelcompanyid;
            }


            if (key != "")
            {
                Condition += " and (a.accounts='" + key + "' or a.employeename='" + key + "' or a.tel='" + key + "' or a.Channelcompanyid in (SELECT [id] FROM [Member_Channel_company] where a.Companyname like '%" + key + "%' ))";
            }

            if (isheadofficekf == 1)//显示内部部门顾问
            {
                Condition += " and channelcompanyid=0 or channelcompanyid in (select id from Member_Channel_company where com_id=" + comid + " and whetherdepartment=1)";
            }


            Condition += " and a.tel!='' and b.phone!='' and b.weixin!=''";

          
            //是否显示教练
            if (isviewjiaolian != "0,1")
            {
                if (isviewjiaolian == "0")
                {
                    Condition += " and a.id not in (select masterid from Sys_MasterGroup where groupid=1026)";
                }
                else
                {
                    //仅显示教练
                    if (isonlycoachlist == 1)
                    {
                        Condition += " and a.id in (select masterid from Sys_MasterGroup where groupid=1026)";
                    }
                }
            }
            else 
            {
                //仅显示教练
                if (isonlycoachlist == 1)
                {
                    Condition += " and a.id in (select masterid from Sys_MasterGroup where groupid=1026)";
                }
            }

            string tb = " b2b_company_manageuser as a " +
            " left join  b2b_crm  as  b on b.phone=a.tel " +
            " left join b2b_crm_location as c on b.weixin=c.weixin ";
            string columnstr = " dbo.fnGetDistance('" + usern + "','" + usere + "',c.Latitude,c.Longitude)*1000 as t,a.*,b.weixin as bweixin";
            //string orderstr= "charindex(cast(datepart(dw,getdate()) as varchar(1)),a.workdays) desc,dbo.fnGetDistance('" + usern + "','" + usere + "',c.Latitude,c.Longitude)";
            string orderstr = "dbo.fnGetDistance('" + usern + "','" + usere + "',c.Latitude,c.Longitude),a.id";


            cmd.PagingCommand1(tb, columnstr, orderstr, "", pagesize, pageindex, "", Condition);


            List<B2b_company_manageuser> list = new List<B2b_company_manageuser>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company_manageuser
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Accounts = reader.GetValue<string>("Accounts"),
                        Passwords = reader.GetValue<string>("passwords"),
                        Atype = reader.GetValue<int>("atype"),
                        Employeename = reader.GetValue<string>("employeename"),
                        Job = reader.GetValue<string>("job"),
                        Tel = reader.GetValue<string>("tel"),
                        Employeestate = reader.GetValue<int>("employeestate"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        Channelcompanyid = reader.GetValue<int?>("channelcompanyid") == null ? 0 : reader.GetValue<int?>("channelcompanyid"),
                        Channelsource = reader.GetValue<int?>("channelsource") == null ? 0 : reader.GetValue<int?>("channelsource"),

                        Selfbrief = reader.GetValue<string>("Selfbrief"),
                        Headimg = reader.GetValue<int>("Headimg"),
                        Workingyears = reader.GetValue<int>("Workingyears"),
                        Workdays = reader.GetValue<string>("Workdays"),
                        Workdaystime = reader.GetValue<string>("Workdaystime"),
                        Workendtime = reader.GetValue<string>("Workendtime"),
                        Fixphone = reader.GetValue<string>("Fixphone"),
                        Email = reader.GetValue<string>("Email"),
                        Homepage = reader.GetValue<string>("Homepage"),
                        Weibopage = reader.GetValue<string>("Weibopage"),
                        QQ = reader.GetValue<string>("QQ"),
                        Weixin = reader.GetValue<string>("bweixin"),
                        Selfhomepage_qrcordurl = reader.GetValue<string>("Selfhomepage_qrcordurl"),
                        Distance = reader.GetValue<double>("t"),
                        // IsCanZixun = reader.GetValue<int>("iscanzixun"),
                        bindingproid = reader.GetValue<int>("bindingproid"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }


        internal List<B2b_company_manageuser> ViewQQpagelist(int comid, int channelcompanyid, int pageindex, int pagesize, out int totalcount, string key = "", string openid = "", string usern = "", string usere = "")
        {

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var Condition = "a.com_id= " + comid + " and a.peoplelistview=1";

            if (channelcompanyid != 0)
            {
                Condition += " and a.Channelcompanyid= " + channelcompanyid;
            }


            if (key != "")
            {
                Condition += " and (a.accounts='" + key + "' or a.employeename='" + key + "' or a.tel='" + key + "' or a.Channelcompanyid in (SELECT [id] FROM [Member_Channel_company] where a.Companyname like '%" + key + "%' ))";
            }

            Condition += " and a.QQ!='' and a.com_id=" + comid;

            string tb = " b2b_company_manageuser as a ";
            string columnstr = "*";
            string orderstr = "charindex(cast(datepart(dw,getdate()) as varchar(1)),a.workdays) desc ";


            cmd.PagingCommand1(tb, columnstr, orderstr, "", pagesize, pageindex, "", Condition);


            List<B2b_company_manageuser> list = new List<B2b_company_manageuser>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company_manageuser
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Accounts = reader.GetValue<string>("Accounts"),
                        Passwords = reader.GetValue<string>("passwords"),
                        Atype = reader.GetValue<int>("atype"),
                        Employeename = reader.GetValue<string>("employeename"),
                        Job = reader.GetValue<string>("job"),
                        Tel = reader.GetValue<string>("tel"),
                        Employeestate = reader.GetValue<int>("employeestate"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        Channelcompanyid = reader.GetValue<int?>("channelcompanyid") == null ? 0 : reader.GetValue<int?>("channelcompanyid"),
                        Channelsource = reader.GetValue<int?>("channelsource") == null ? 0 : reader.GetValue<int?>("channelsource"),

                        Selfbrief = reader.GetValue<string>("Selfbrief"),
                        Headimg = reader.GetValue<int>("Headimg"),
                        Workingyears = reader.GetValue<int>("Workingyears"),
                        Workdays = reader.GetValue<string>("Workdays"),
                        Workdaystime = reader.GetValue<string>("Workdaystime"),
                        Workendtime = reader.GetValue<string>("Workendtime"),
                        Fixphone = reader.GetValue<string>("Fixphone"),
                        Email = reader.GetValue<string>("Email"),
                        Homepage = reader.GetValue<string>("Homepage"),
                        Weibopage = reader.GetValue<string>("Weibopage"),
                        QQ = reader.GetValue<string>("QQ"),
                        Selfhomepage_qrcordurl = reader.GetValue<string>("Selfhomepage_qrcordurl"),
                        bindingproid = reader.GetValue<int>("bindingproid"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }


        #region 显示全部或者特定管理组中的联系人信息；
        internal List<B2b_company_manageuser> Manageuserpagelist(int pageindex, int pagesize, string groupid, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_company_manageuser";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = "";
            if (groupid != "0")
            {
                condition = "id in (select masterid from Sys_MasterGroup where groupid=" + groupid + ")";
            }
            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_company_manageuser> list = new List<B2b_company_manageuser>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company_manageuser
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Accounts = reader.GetValue<string>("Accounts"),
                        Atype = reader.GetValue<int>("Atype"),
                        Createuserid = reader.GetValue<int>("Createuserid"),
                        Employeename = reader.GetValue<string>("Employeename"),
                        Employeestate = reader.GetValue<int>("Employeestate"),
                        Job = reader.GetValue<string>("Job"),
                        Passwords = reader.GetValue<string>("Passwords"),
                        Tel = reader.GetValue<string>("Tel"),
                        Channelcompanyid = reader.GetValue<int>("channelcompanyid"),
                        Channelsource = reader.GetValue<int>("channelsource")

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        #endregion
        #region 显示全部或者特定管理组中的联系人信息；
        internal List<B2b_company_manageuser> Manageuserpagelist(int pageindex, int pagesize, string groupid, int childcomid, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            //var tblName = "b2b_company_manageuser";
            //var strGetFields = "*";
            //var sortKey = "id";
            //var sortMode = "1";
            var condition = "";
            if (groupid != "0")
            {
                condition += "  id in (select masterid from Sys_MasterGroup where groupid=" + groupid + ")";
            }
            if (childcomid != 0)
            {
                condition += " id in (select id from b2b_company_manageuser where com_id=" + childcomid + ")";
            }
            //cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);

            cmd.PagingCommand1("b2b_company_manageuser", "*", "id desc", "", pagesize, pageindex, "", condition);


            List<B2b_company_manageuser> list = new List<B2b_company_manageuser>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company_manageuser
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Accounts = reader.GetValue<string>("Accounts"),
                        Atype = reader.GetValue<int>("Atype"),
                        Createuserid = reader.GetValue<int>("Createuserid"),
                        Employeename = reader.GetValue<string>("Employeename"),
                        Employeestate = reader.GetValue<int>("Employeestate"),
                        Job = reader.GetValue<string>("Job"),
                        Passwords = reader.GetValue<string>("Passwords"),
                        Tel = reader.GetValue<string>("Tel"),
                        Channelcompanyid = reader.GetValue<int>("channelcompanyid"),
                        Channelsource = reader.GetValue<int>("channelsource")

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }
        #endregion
        internal List<B2b_company_manageuser> Manageuserpagelist(string employstate, int pageindex, int pagesize, string groupid, int childcomid, out int totalcount, string key = "")
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            var condition = "employeestate in (" + employstate + ")";


            if (groupid != "0")
            {
                condition += " and id in (select masterid from Sys_MasterGroup where groupid=" + groupid + ")";
            }
            if (childcomid != 0)
            {
                condition += " and  id in (select id from b2b_company_manageuser where com_id=" + childcomid + ")";
            }

            if (key != "")
            {
                condition += " and (accounts='" + key + "' or  employeename='" + key + "' or  tel='" + key + "' or com_id in (select id from b2b_company where com_name like '%" + key + "%') ) ";
            }


            //cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);
            cmd.PagingCommand1("b2b_company_manageuser", "*", "id desc", "", pagesize, pageindex, "", condition);


            List<B2b_company_manageuser> list = new List<B2b_company_manageuser>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company_manageuser
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Accounts = reader.GetValue<string>("Accounts"),
                        Atype = reader.GetValue<int>("Atype"),
                        Createuserid = reader.GetValue<int>("Createuserid"),
                        Employeename = reader.GetValue<string>("Employeename"),
                        Employeestate = reader.GetValue<int>("Employeestate"),
                        Job = reader.GetValue<string>("Job"),
                        Passwords = reader.GetValue<string>("Passwords"),
                        Tel = reader.GetValue<string>("Tel"),
                        Channelcompanyid = reader.GetValue<int>("channelcompanyid"),
                        Channelsource = reader.GetValue<int>("channelsource")

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }

        internal B2b_company_manageuser GetManageUserByAccount(string account)
        {
            const string sqlTxt = @"SELECT [id]
      ,[com_id]
      ,[Accounts]
      ,[passwords]
      ,[atype]
      ,[employeename]
      ,[job]
      ,[tel]
      ,[employeestate]
      ,[createuserid]
      ,channelcompanyid
      ,channelsource
  FROM [EtownDB].[dbo].[b2b_company_manageuser]  where [Accounts] = @account";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@account", account);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_company_manageuser u = null;

                while (reader.Read())
                {
                    u = new B2b_company_manageuser
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Accounts = reader.GetValue<string>("Accounts"),
                        Passwords = reader.GetValue<string>("passwords"),
                        Atype = reader.GetValue<int>("atype"),
                        Employeename = reader.GetValue<string>("employeename"),
                        Job = reader.GetValue<string>("job"),
                        Tel = reader.GetValue<string>("tel"),
                        Employeestate = reader.GetValue<int>("employeestate"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        Channelcompanyid = reader.GetValue<int?>("channelcompanyid") == null ? 0 : reader.GetValue<int?>("channelcompanyid"),
                        Channelsource = reader.GetValue<int?>("channelsource") == null ? 0 : reader.GetValue<int?>("channelsource"),

                    };

                }
                return u;
            }
        }

        internal B2b_company GetB2bCompanyByCompanyName(string companyname)
        {
            const string sqlTxt = @"SELECT  [ID]
                  ,[com_name]
                  ,[Scenic_name]
                  ,[com_type]
                  ,[com_state]
                  ,[imprest]
              FROM  [dbo].[b2b_company] a where a.com_name=@com_name";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@com_name", companyname);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_company u = null;

                while (reader.Read())
                {
                    u = new B2b_company
                    {
                        ID = reader.GetValue<int>("id"),
                        Com_name = reader.GetValue<string>("com_name"),
                        Com_state = reader.GetValue<int>("com_state"),
                        Com_type = reader.GetValue<int>("com_type"),
                        Scenic_name = reader.GetValue<string>("Scenic_name"),
                        Imprest = reader.GetValue<decimal>("imprest")
                    };

                }
                return u;
            }
        }

        internal B2b_company_manageuser Getchildcompanyuser(int childcomid)
        {
            const string sqlTxt = @"SELECT top 1 [id]
      ,[com_id]
      ,[Accounts]
      ,[passwords]
      ,[atype]
      ,[employeename]
      ,[job]
      ,[tel]
      ,[employeestate]
      ,[createuserid]
      ,[channelcompanyid]
      ,[channelsource]
      FROM [EtownDB].[dbo].[b2b_company_manageuser] where com_id=@comid ";

            //  FROM [EtownDB].[dbo].[b2b_company_manageuser] where com_id=@comid and id in (select top 1 masterid from Sys_MasterGroup where order by  sortid )";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", childcomid);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_company_manageuser u = null;

                while (reader.Read())
                {
                    u = new B2b_company_manageuser
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Accounts = reader.GetValue<string>("Accounts"),
                        Passwords = reader.GetValue<string>("passwords"),
                        Atype = reader.GetValue<int>("atype"),
                        Employeename = reader.GetValue<string>("employeename"),
                        Job = reader.GetValue<string>("job"),
                        Tel = reader.GetValue<string>("tel"),
                        Employeestate = reader.GetValue<int>("employeestate"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        Channelcompanyid = reader.GetValue<int?>("channelcompanyid") == null ? 0 : reader.GetValue<int?>("channelcompanyid"),
                        Channelsource = reader.GetValue<int?>("channelsource") == null ? 0 : reader.GetValue<int?>("channelsource"),

                    };

                }
                return u;
            }
        }

        internal int Adjustemploerstate(int masterid, int employerstate)
        {
            string sql = "update b2b_company_manageuser set employeestate=" + employerstate + " where id=" + masterid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int AdjustChannelCompanyEmploerstate(int companyid, int employstate)
        {
            string sql = "update b2b_company_manageuser set employeestate=" + employstate + " where channelcompanyid=" + companyid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal bool IsParentCompanyUser(int userid)
        {
            string sql = "select count(1) from b2b_company_manageuser where id=" + userid + " and channelsource=0 and channelcompanyid=0";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            if (int.Parse(o.ToString()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal int FromAccountGetId(string account)
        {
            string sql = "select id from b2b_company_manageuser where  accounts='" + account + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("id");
                }
                else
                {
                    return 0;
                }
            }
        }


        internal bool Ishasaccount(string account)
        {
            string sql = "select count(1) from b2b_company_manageuser where  accounts='" + account + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            if (int.Parse(o.ToString()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal B2b_company_manageuser GetMenshiManagerByMenshiId(int menshiid)
        {
            const string sqlTxt = @"SELECT top 1 [id]
      ,[com_id]
      ,[Accounts]
      ,[passwords]
      ,[atype]
      ,[employeename]
      ,[job]
      ,[tel]
      ,[employeestate]
      ,[createuserid]
      ,[channelcompanyid]
      ,[channelsource]
      FROM [EtownDB].[dbo].[b2b_company_manageuser] where   channelcompanyid=@channelcompanyid and employeestate=1 and  id in (select masterid from Sys_MasterGroup where groupid=5) order by id   ";


            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@channelcompanyid", menshiid);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_company_manageuser u = null;

                if (reader.Read())
                {
                    u = new B2b_company_manageuser
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Accounts = reader.GetValue<string>("Accounts"),
                        Passwords = reader.GetValue<string>("passwords"),
                        Atype = reader.GetValue<int>("atype"),
                        Employeename = reader.GetValue<string>("employeename"),
                        Job = reader.GetValue<string>("job"),
                        Tel = reader.GetValue<string>("tel"),
                        Employeestate = reader.GetValue<int>("employeestate"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        Channelcompanyid = reader.GetValue<int?>("channelcompanyid") == null ? 0 : reader.GetValue<int?>("channelcompanyid"),
                        Channelsource = reader.GetValue<int?>("channelsource") == null ? 0 : reader.GetValue<int?>("channelsource"),

                    };

                }
                return u;
            }
        }

        internal B2b_company_manageuser GetCompanyUser(int userid)
        {
            const string sqlTxt = @"SELECT   [id]
      ,[com_id]
      ,[Accounts]
      ,[passwords]
      ,[atype]
      ,[employeename]
      ,[job]
      ,[tel]
      ,[employeestate]
      ,[createuserid]
      ,[channelcompanyid]
      ,[channelsource]
      ,[Isdefaultkf]
      FROM [EtownDB].[dbo].[b2b_company_manageuser] where  id=@id  ";


            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@id", userid);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_company_manageuser u = null;

                if (reader.Read())
                {
                    u = new B2b_company_manageuser
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Accounts = reader.GetValue<string>("Accounts"),
                        Passwords = reader.GetValue<string>("passwords"),
                        Atype = reader.GetValue<int>("atype"),
                        Employeename = reader.GetValue<string>("employeename"),
                        Job = reader.GetValue<string>("job"),
                        Tel = reader.GetValue<string>("tel"),
                        Employeestate = reader.GetValue<int>("employeestate"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        Channelcompanyid = reader.GetValue<int?>("channelcompanyid") == null ? 0 : reader.GetValue<int?>("channelcompanyid"),
                        Channelsource = reader.GetValue<int?>("channelsource") == null ? 0 : reader.GetValue<int?>("channelsource"),
                        Isdefaultkf = reader.GetValue<int>("Isdefaultkf"),


                    };

                }
                return u;
            }
        }


        //通过登录账户id获取公司名称
        internal string GetCompanynamebyUserid(int userid)
        {
            const string sqlTxt = @"select * from Member_Channel_company where id in( SELECT  channelcompanyid FROM [b2b_company_manageuser] where  id=@id ) ";


            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@id", userid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<string>("companyname");
                }
                return "";
            }
        }

        internal B2b_company_manageuser GetCompanyUserByPhone(string phone, int comid)
        {
            string sql = " select * from b2b_company_manageuser where tel='" + phone + "' and com_id=" + comid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                B2b_company_manageuser u = null;

                if (reader.Read())
                {
                    u = new B2b_company_manageuser
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Accounts = reader.GetValue<string>("Accounts"),
                        Passwords = reader.GetValue<string>("passwords"),
                        Atype = reader.GetValue<int>("atype"),
                        Employeename = reader.GetValue<string>("employeename"),
                        Job = reader.GetValue<string>("job"),
                        Tel = reader.GetValue<string>("tel"),
                        Employeestate = reader.GetValue<int>("employeestate"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        Channelcompanyid = reader.GetValue<int?>("channelcompanyid") == null ? 0 : reader.GetValue<int?>("channelcompanyid"),
                        Channelsource = reader.GetValue<int?>("channelsource") == null ? 0 : reader.GetValue<int?>("channelsource"),
                        Workdays = reader.GetValue<string>("Workdays"),
                        Headimg = reader.GetValue<int>("Headimg"),
                    };

                }
                return u;
            }
        }

        internal B2b_company_manageuser GetGuwenByVipweixin(string openid, int comid)
        {
            if (openid == "")
            {
                return null;
            }
            string sql = "select   * from B2b_company_manageuser where com_id =" + comid + " and  tel =(select mobile from member_channel where com_id=" + comid + " and id =(select issuecard from member_card where com_id=" + comid + " and  cardcode=(select idcard from b2b_crm where weixin='" + openid + "' and weixin!='')))";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                B2b_company_manageuser u = null;

                if (reader.Read())
                {
                    u = new B2b_company_manageuser
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Accounts = reader.GetValue<string>("Accounts"),
                        Passwords = reader.GetValue<string>("passwords"),
                        Atype = reader.GetValue<int>("atype"),
                        Employeename = reader.GetValue<string>("employeename"),
                        Job = reader.GetValue<string>("job"),
                        Tel = reader.GetValue<string>("tel"),
                        Employeestate = reader.GetValue<int>("employeestate"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        Channelcompanyid = reader.GetValue<int?>("channelcompanyid") == null ? 0 : reader.GetValue<int?>("channelcompanyid"),
                        Channelsource = reader.GetValue<int?>("channelsource") == null ? 0 : reader.GetValue<int?>("channelsource"),
                        Workdays = reader.GetValue<string>("Workdays"),
                    };

                }
                return u;
            }
        }

        internal string GetCompanyUserName(int userid)
        {
            string sql = " select  Accounts from b2b_company_manageuser where id=" + userid;
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                return o.ToString();
            }
            catch
            {
                return "";
            }
        }

        internal B2b_company_manageuser GetOpenAccount(int comid)
        {

            string sql = "select top 1   * from B2b_company_manageuser where com_id =" + comid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                B2b_company_manageuser u = null;

                if (reader.Read())
                {
                    u = new B2b_company_manageuser
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Accounts = reader.GetValue<string>("Accounts"),
                        Passwords = reader.GetValue<string>("passwords"),
                        Atype = reader.GetValue<int>("atype"),
                        Employeename = reader.GetValue<string>("employeename"),
                        Job = reader.GetValue<string>("job"),
                        Tel = reader.GetValue<string>("tel"),
                        Employeestate = reader.GetValue<int>("employeestate"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        Channelcompanyid = reader.GetValue<int?>("channelcompanyid") == null ? 0 : reader.GetValue<int?>("channelcompanyid"),
                        Channelsource = reader.GetValue<int?>("channelsource") == null ? 0 : reader.GetValue<int?>("channelsource"),
                        Workdays = reader.GetValue<string>("Workdays"),
                    };

                }
                return u;
            }
        }

        internal B2b_company_manageuser GetCompanyUserByOpenid(string openid)
        {
            string sql = "select    * from B2b_company_manageuser where tel in (select phone from b2b_crm where weixin='" + openid + "')";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                B2b_company_manageuser u = null;

                if (reader.Read())
                {
                    u = new B2b_company_manageuser
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Accounts = reader.GetValue<string>("Accounts"),
                        Passwords = reader.GetValue<string>("passwords"),
                        Atype = reader.GetValue<int>("atype"),
                        Employeename = reader.GetValue<string>("employeename"),
                        Job = reader.GetValue<string>("job"),
                        Tel = reader.GetValue<string>("tel"),
                        Employeestate = reader.GetValue<int>("employeestate"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        Channelcompanyid = reader.GetValue<int?>("channelcompanyid") == null ? 0 : reader.GetValue<int?>("channelcompanyid"),
                        Channelsource = reader.GetValue<int?>("channelsource") == null ? 0 : reader.GetValue<int?>("channelsource"),
                        Workdays = reader.GetValue<string>("Workdays"),
                    };

                }
                return u;
            }
        }


        #region 查询指定 日期 教练
        internal List<B2b_company_manageuser_useworktime> Worktimepagelist(int comid, int MasterId, DateTime date, string hourstr, out int totalcount)
        {
            int i = 0;

            var condition = "comid = " + comid + " and  masterid=" + MasterId + " and useDate='" + date + "'";
            if (hourstr != "")
            {
                condition += " and hournum in (" + hourstr + ")";
            }
            string sql = "select  * from B2b_company_manageuser_useworktime where " + condition;

            List<B2b_company_manageuser_useworktime> list = new List<B2b_company_manageuser_useworktime>();
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_company_manageuser_useworktime
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        MasterId = reader.GetValue<int>("MasterId"),
                        useDate = reader.GetValue<DateTime>("useDate"),
                        Hournum = reader.GetValue<int>("Hournum"),
                        oid = reader.GetValue<int>("oid"),
                        text = reader.GetValue<string>("text"),
                    });
                    i++;
                }
            }
            totalcount = i;
            return list;
        }
        #endregion

        #region 添加已使用 工时间
        internal int UseworktimeInsertOrUpdate(B2b_company_manageuser_useworktime model)
        {
            const string sqlTxt = "insert B2b_company_manageuser_useworktime (comid,masterid,usedate,hournum,oid,text) values(@comid,@MasterId,@useDate,@Hournum,@oid,@text)";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", model.comid);
            cmd.AddParam("@MasterId", model.MasterId);
            cmd.AddParam("@oid", model.oid);
            cmd.AddParam("@useDate", model.useDate);
            cmd.AddParam("@Hournum", model.Hournum);
            cmd.AddParam("@text", model.text);

            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region 删除已使用 工时间
        internal int UseworktimeDel(int id)
        {
            const string sqlTxt = "delete B2b_company_manageuser_useworktime  where id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@id", id);

            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region 删除已使用 工时间
        internal int UseworktimeDelOid(int oid)
        {
            const string sqlTxt = "delete B2b_company_manageuser_useworktime  where oid=@oid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@oid", oid);

            return cmd.ExecuteNonQuery();
        }
        #endregion

        internal int UpChannelcompanyid(string masterid, int channelcompanyid)
        {
            string sql = "update b2b_company_manageuser set channelcompanyid='" + channelcompanyid + "' where id=" + masterid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }
    }
}

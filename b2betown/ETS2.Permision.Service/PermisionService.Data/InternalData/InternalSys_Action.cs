using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.Permision.Service.PermisionService.Model;

namespace ETS2.Permision.Service.PermisionService.Data.InternalData
{
    public class InternalSys_Action
    {
        private SqlHelper sqlHelper;
        public InternalSys_Action(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal List<Sys_Action> GetAllAction(out int totalcount)
        {
            string sql = @"select [actionid]
                  ,[actionname]
                  ,[actioncolumnid]
                  ,[actionurl]
                  ,[viewmode]
                  ,[sortid]
              from [etowndb].[dbo].[sys_action]";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<Sys_Action> list = new List<Sys_Action>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Sys_Action
                    {
                        Actioncolumnid = reader.GetValue<int>("Actioncolumnid"),
                        Actionid = reader.GetValue<int>("Actionid"),
                        Actionname = reader.GetValue<string>("Actionname"),
                        Actionurl = reader.GetValue<string>("Actionurl"),
                        Sortid = reader.GetValue<int>("Sortid"),
                        Viewmode = reader.GetValue<bool>("Viewmode"),

                    });

                }
            }
            totalcount = list.Count;

            return list;

        }


        private static readonly string SQLInsertOrUpdate2 = "usp_ActionInit";
        internal int ActionInit()
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate2);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;


        }

        internal List<Sys_Action> PermissionPageList(int pageindex, int pagesize, out int totalcount)
        {
            //var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            //var tblName = "Sys_Action  left join Sys_ActionColumn  on Sys_Action.actioncolumnid=Sys_ActionColumn.actioncolumnid";
            //var strGetFields = "Sys_Action.*,Sys_ActionColumn.actioncolumnname";
            //var sortKey = "Sys_Action.actioncolumnid";
            //var sortMode = "0";

            //var condition = "";


            //cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);
            var Table = "Sys_Action  left join Sys_ActionColumn  on Sys_Action.actioncolumnid=Sys_ActionColumn.actioncolumnid";
            var Column = "Sys_Action.*,Sys_ActionColumn.actioncolumnname";
            var OrderColumn = "Sys_Action.actioncolumnid";
            var GroupColumn = "";
            var PageSize = pagesize;
            var CurrentPage = pageindex;
            var Group = "";
            var Condition = "";

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");



            cmd.PagingCommand1(Table, Column, OrderColumn, GroupColumn, PageSize, CurrentPage, Group, Condition);

            List<Sys_Action> list = new List<Sys_Action>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Sys_Action
                    {
                        Actioncolumnid = reader.GetValue<int>("Actioncolumnid"),
                        Actionid = reader.GetValue<int>("Actionid"),
                        Actionname = reader.GetValue<string>("Actionname"),
                        Actionurl = reader.GetValue<string>("Actionurl"),
                        Sortid = reader.GetValue<int>("Sortid"),
                        Viewmode = reader.GetValue<bool>("Viewmode"),
                        Actioncolumnname = reader.GetValue<string>("actioncolumnname"),

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;

        }

        internal Sys_Action GetActionById(int actionid)
        {
            const string sqltxt = @"select a.*,b.actioncolumnname from Sys_Action a left join Sys_ActionColumn b on a.actioncolumnid=b.actioncolumnid  where a.actionid=@actionid";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@actionid", actionid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Sys_Action
                    {
                        Actionid = reader.GetValue<int>("Actionid"),
                        Actionname = reader.GetValue<string>("Actionname"),
                        Actioncolumnid = reader.GetValue<int>("Actioncolumnid"),
                        Actioncolumnname = reader.GetValue<string>("Actioncolumnname"),
                        Actionurl = reader.GetValue<string>("Actionurl"),
                        Viewmode = reader.GetValue<bool>("Viewmode"),
                        Sortid = reader.GetValue<int>("Sortid"),
                    };
                }
                return null;
            }
        }
        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateSysAction";
        internal int EditAction(Sys_Action model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);

            cmd.AddParam("@Actionid", model.Actionid);
            cmd.AddParam("@Actionname", model.Actionname);
            cmd.AddParam("@Actionurl", model.Actionurl);
            cmd.AddParam("@Actioncolumnid", model.Actioncolumnid);
            cmd.AddParam("@ViewMode", model.Viewmode);


            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }

        internal List<Sys_Action> GetActionsByColumnId(int columnid, out int totalcount)
        {
            string sql = @"select [actionid]
                  ,[actionname]
                  ,[actioncolumnid]
                  ,[actionurl]
                  ,[viewmode]
                  ,[sortid]
              from [dbo].[sys_action] where   actioncolumnid=@actioncolumnid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@actioncolumnid", columnid);


            List<Sys_Action> list = new List<Sys_Action>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Sys_Action
                    {
                        Actioncolumnid = reader.GetValue<int>("Actioncolumnid"),
                        Actionid = reader.GetValue<int>("Actionid"),
                        Actionname = reader.GetValue<string>("Actionname"),
                        Actionurl = reader.GetValue<string>("Actionurl"),
                        Sortid = reader.GetValue<int>("Sortid"),
                        Viewmode = reader.GetValue<bool>("Viewmode"),

                    });

                }
            }
            totalcount = list.Count;

            return list;
        }
        internal List<Sys_Action> GetActionsByColumnId(int userid, int columnid, out int totalcount)
        {
            string sql = @"select [actionid]
                  ,[actionname]
                  ,[actioncolumnid]
                  ,[actionurl]
                  ,[viewmode]
                  ,[sortid]
              from [etowndb].[dbo].[sys_action] where   actioncolumnid=@actioncolumnid and actionid in (select actionid from Sys_ActionGroup where groupid in (select groupid from Sys_MasterGroup where masterid=@masterid)) order by sortid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@actioncolumnid", columnid);
            cmd.AddParam("@masterid", userid);

            List<Sys_Action> list = new List<Sys_Action>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Sys_Action
                    {
                        Actioncolumnid = reader.GetValue<int>("Actioncolumnid"),
                        Actionid = reader.GetValue<int>("Actionid"),
                        Actionname = reader.GetValue<string>("Actionname"),
                        Actionurl = reader.GetValue<string>("Actionurl"),
                        Sortid = reader.GetValue<int>("Sortid"),
                        Viewmode = reader.GetValue<bool>("Viewmode"),

                    });

                }
            }
            totalcount = list.Count;

            return list;
        }

        internal IList<Sys_Action> GetActionsByGroupdId(string groupid, out int totalcount)
        {
            string sql = @"select [actionid]
                  ,[actionname]
                  ,[actioncolumnid]
                  ,[actionurl]
                  ,[viewmode]
                  ,[sortid]
              from [etowndb].[dbo].[sys_action] where actionid in (select actionid from Sys_ActionGroup where groupid =@groupid )";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@groupid", groupid);

            List<Sys_Action> list = new List<Sys_Action>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Sys_Action
                    {
                        Actioncolumnid = reader.GetValue<int>("Actioncolumnid"),
                        Actionid = reader.GetValue<int>("Actionid"),
                        Actionname = reader.GetValue<string>("Actionname"),
                        Actionurl = reader.GetValue<string>("Actionurl"),
                        Sortid = reader.GetValue<int>("Sortid"),
                        Viewmode = reader.GetValue<bool>("Viewmode"),

                    });

                }
            }
            totalcount = list.Count;

            return list;
        }

        internal List<Sys_Action> GetAllRoleFuncByUser(int userid, out int totalcount)
        {
            string sql = @"select a.*,b.actioncolumnname from Sys_Action a left join Sys_ActionColumn b on a.actioncolumnid=b.actioncolumnid

 where a.actionid in (select c.actionid from Sys_ActionGroup c  where c.groupid in (select d.groupid from Sys_MasterGroup d where d.masterid=@userid))";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@userid", userid);

            List<Sys_Action> list = new List<Sys_Action>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Sys_Action
                    {
                        Actioncolumnid = reader.GetValue<int>("Actioncolumnid"),
                        Actionid = reader.GetValue<int>("Actionid"),
                        Actionname = reader.GetValue<string>("Actionname"),
                        Actionurl = reader.GetValue<string>("Actionurl"),
                        Sortid = reader.GetValue<int>("Sortid"),
                        Viewmode = reader.GetValue<bool>("Viewmode"),
                        Actioncolumnname = reader.GetValue<string>("actioncolumnname")
                    });

                }
            }
            totalcount = list.Count;

            return list;
        }
        private static readonly string SQLInsertOrUpdate1 = "usp_DelActionById";
        internal int DelActionById(int actionid)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate1);
            cmd.AddParam("@ActionId", actionid);


            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }

        internal bool Isactionurl(string Requestfile)
        {
            var sql = "select count(1) from sys_action where actionurl='" + Requestfile + "'";

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                object o = cmd.ExecuteScalar();
                if (null == o)
                {
                    return false;
                }
                else
                {
                    if (o.ToString() == "")
                    {
                        return false;
                    }
                    else
                    {
                        if (int.Parse(o.ToString()) > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        internal bool Iscanvisit(string Requestfile, int userid)
        {
            string sql = "select count(1) from Sys_ActionGroup where actionid in (select actionid from sys_action where actionurl='" + Requestfile + "') and groupid in (select groupid from sys_mastergroup where masterid= " + userid + ")";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                object o = cmd.ExecuteScalar();
                if (null == o)
                {
                    return false;
                }
                else
                {
                    if (o.ToString() == "")
                    {
                        return false;
                    }
                    else
                    {
                        if (int.Parse(o.ToString()) > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }

        }

        internal List<Sys_Action> Permissionlist(int columnid = 0)
        {
            string sql = "SELECT   [actionid]   ,[actionname]  ,[actioncolumnid]  ,[actionurl]  ,[viewmode]   ,[sortid]   FROM [Sys_Action] ";
            if (columnid > 0)
            {
                sql += " where actioncolumnid=" + columnid;
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                List<Sys_Action> r = new List<Sys_Action>();
                while (reader.Read())
                {
                    r.Add(new Sys_Action
                    {
                        Actioncolumnid = reader.GetValue<int>("Actioncolumnid"),
                        Actionid = reader.GetValue<int>("Actionid"),
                        Actionname = reader.GetValue<string>("Actionname"),
                        Actionurl = reader.GetValue<string>("Actionurl"),
                        Sortid = reader.GetValue<int>("Sortid"),
                        Viewmode = reader.GetValue<bool>("Viewmode"),

                    });
                }
                return r;
            }
        }

        internal int GetActionNumByColumn(int columnid)
        {
            string sql = "select count(1) from Sys_Action where Actioncolumnid=" + columnid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            return int.Parse(o.ToString());
        }
    }
}

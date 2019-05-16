using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.Permision.Service.PermisionService.Model;

namespace ETS2.Permision.Service.PermisionService.Data.InternalData
{
    public class InternalSys_ActionColumn
    {
        private SqlHelper sqlHelper;
        public InternalSys_ActionColumn(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal IList<Sys_ActionColumn> GetColumns(out int totalcount)
        {
            string sql = @"SELECT [actioncolumnid]
      ,[actioncolumnname]
      ,[viewmode]
  FROM [dbo].[Sys_ActionColumn]";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<Sys_ActionColumn> list = new List<Sys_ActionColumn>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Sys_ActionColumn
                    {
                        Actioncolumnid = reader.GetValue<int>("Actioncolumnid"),
                        Actioncolumnname = reader.GetValue<string>("Actioncolumnname")


                    });

                }
            }
            totalcount = list.Count;

            return list;
        }

        internal List<Sys_ActionColumn> GetActionColumnByUser(int userid, out int totalcount)
        {
            string sql = @"select a.actioncolumnid,a.actioncolumnname  from Sys_ActionColumn a where actioncolumnid in (select actioncolumnid from Sys_Action where   actionid in (select actionid from Sys_ActionGroup where groupid in (select groupid from Sys_MasterGroup where masterid=@masterid))) order by a.sortid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@masterid", userid);

            List<Sys_ActionColumn> list = new List<Sys_ActionColumn>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Sys_ActionColumn
                    {
                        Actioncolumnid = reader.GetValue<int>("Actioncolumnid"),
                        Actioncolumnname = reader.GetValue<string>("Actioncolumnname")


                    });

                }
            }
            totalcount = list.Count;

            return list;
        }

        internal List<Sys_ActionColumn> Getallactioncolumns()
        {
            string sql = @"select * from Sys_ActionColumn";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);


            List<Sys_ActionColumn> list = new List<Sys_ActionColumn>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Sys_ActionColumn
                    {
                        Actioncolumnid = reader.GetValue<int>("Actioncolumnid"),
                        Actioncolumnname = reader.GetValue<string>("Actioncolumnname")
                    });

                }
            }
            return list;
        }

        internal Sys_ActionColumn GetActionColumn(int columnid)
        {
            string sql = @"SELECT [actioncolumnid]
                              ,[actioncolumnname]
                              ,[viewmode]
                          FROM [dbo].[Sys_ActionColumn] where actioncolumnid=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", columnid);
            using (var reader = cmd.ExecuteReader())
            {
                Sys_ActionColumn m = null;
                if (reader.Read())
                {
                    m = new Sys_ActionColumn
                    {
                        Actioncolumnid = reader.GetValue<int>("Actioncolumnid"),
                        Actioncolumnname = reader.GetValue<string>("Actioncolumnname")
                    };
                }
                return m;
            }

        }
    }
}

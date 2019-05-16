using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.Permision.Service.PermisionService.Model;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.Permision.Service.PermisionService.Data.InternalData
{
    public class InternalSys_MasterGroup
    {

        private SqlHelper sqlHelper;
        public InternalSys_MasterGroup(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal List<Sys_Group> GetGroupByMasterId(int masterid)
        {
            const string sqltxt = @"SELECT [groupid]
      ,[groupname]
      ,[groupinfo]
      ,[masterid]
      ,[mastername]
      ,[createdate]
  FROM [EtownDB].[dbo].[Sys_Group] where groupid in (select groupid from Sys_MasterGroup where masterid= @masterid ) ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@masterid", masterid);

            List<Sys_Group> list = new List<Sys_Group>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Sys_Group
                    {
                        Groupid = reader.GetValue<int>("Groupid"),
                        Groupname = reader.GetValue<string>("Groupname"),
                        Groupinfo = reader.GetValue<string>("Groupinfo"),
                        Masterid = reader.GetValue<int>("Masterid"),
                        Mastername = reader.GetValue<string>("Mastername"),
                        Createdate = reader.GetValue<DateTime>("Createdate"),

                    });

                }
                return list;
            }
        }
        private static readonly string SQLInsertOrUpdate2 = "usp_EditMasterGroup";
        internal int EditMasterGroup(string masterid, string mastername, string grouparr, int createmasterid, string createmastername, DateTime createdate)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate2);
            cmd.AddParam("@MasterId", masterid);
            cmd.AddParam("@MasterName", mastername);
            cmd.AddParam("@GroupArr", grouparr);
            cmd.AddParam("@CreateMasterId", createmasterid);
            cmd.AddParam("@CreateMasterName", createmastername);
            cmd.AddParam("@CreateDate", createdate);


            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;

        }

     
    }
}

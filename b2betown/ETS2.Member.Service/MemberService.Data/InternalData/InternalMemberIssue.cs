using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.Member.Service.MemberService.Model;

namespace ETS2.Member.Service.MemberService.Data.InternalData
{
    public class InternalMemberIssue
    {
        private SqlHelper sqlHelper;
        public InternalMemberIssue(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateMemberIssue";

        internal int InsertOrUpdate(Model.Member_Issue model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);

            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Com_id", model.Com_id);
            cmd.AddParam("@Title", model.Title);
            cmd.AddParam("@Num", model.Num);
            cmd.AddParam("@Openyes", model.Openyes);
            cmd.AddParam("@Openaddress", model.Openaddress);
            cmd.AddParam("@Crid", model.Crid);
            cmd.AddParam("@Chid", model.Chid);


            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }

        internal Member_Issue GetIssueDetailById(int issueid)
        {
            const string sqltext = @"SELECT [id]
      ,[Com_id]
      ,[Crid]
      ,[Chid]
      ,[title]
      ,[num]
      ,[openyes]
      ,[openaddress]
  FROM [EtownDB].[dbo].[Member_Issue] where id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltext);
            cmd.AddParam("@id", issueid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_Issue()
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Crid = reader.GetValue<int>("Crid"),
                        Chid = reader.GetValue<int>("Chid"),
                        Title = reader.GetValue<string>("title"),
                        Num = reader.GetValue<int>("num"),
                        Openyes = reader.GetValue<bool>("openyes"),
                        Openaddress = reader.GetValue<int>("openaddress"),

                    };
                }
            }
            return null;
        }

        internal List<Member_Issue> PageList(string comid, int pageindex, int pagesize, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "com_id=" + comid ;
            cmd.PagingCommand1("Member_Issue", "*", "id desc", "", pagesize, pageindex, "", condition);

 
            List<Member_Issue> list = new List<Member_Issue>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Issue
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Crid = reader.GetValue<int>("Crid"),
                        Chid = reader.GetValue<int>("Chid"),
                        Title = reader.GetValue<string>("Title"),
                        Num = reader.GetValue<int>("Num"),
                        Openaddress = reader.GetValue<int>("Openaddress"),
                        Openyes = reader.GetValue<bool>("Openyes"),




                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }
    }
}

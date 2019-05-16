using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.Member.Service.MemberService.Model;

namespace ETS2.Member.Service.MemberService.Data.InternalData
{
    public class InternalMemberIssueActivity
    {
        private SqlHelper sqlHelper;
        public InternalMemberIssueActivity(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal int DelIssueIdByIssueId(int issueid)
        {
            var sqlTxt = @"delete Member_Issue_Activity where isid=@issueid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@issueid", issueid);


            object obj = cmd.ExecuteScalar();

            return obj != null ? int.Parse(obj.ToString()) : 0;
        }
        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateMemberIssueAct";
        internal int InsertOrUpdate(Model.Member_Issue_Activity model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);

            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@ISid", model.ISid);
            cmd.AddParam("@Acid", model.Acid);



            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }

        internal List<Member_Issue_Activity> GetIssuePromot(int issueid)
        {
            string sqltext = @"SELECT [id]
      ,[ISid]
      ,[Acid]
  FROM [EtownDB].[dbo].[Member_Issue_Activity] where isid=@isid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltext);
            cmd.AddParam("@isid", issueid);

            List<Member_Issue_Activity> list = new List<Member_Issue_Activity>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Member_Issue_Activity()
                    {
                        Id = reader.GetValue<int>("id"),
                        ISid = reader.GetValue<int>("ISid"),
                        Acid = reader.GetValue<int>("Acid")
                    });
                }
                return list;
            }

        }

        internal string GetIssueActStr(int isid)
        {
            string sqltext = @"select  b.title from Member_Issue_Activity a  left join Member_Activity b on a.Acid=b.id
  
  where a.ISid=@isid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltext);
            cmd.AddParam("@isid", isid);

            var result = "";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {

                    var actname = reader.GetValue<string>("title");

                    result += actname + ",";
                }
                return result.Length > 0 ? result.Substring(0, result.Length - 1) : "";
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.Member.Service.MemberService.Model;

namespace ETS2.Member.Service.MemberService.Data.InternalData
{
    public class InternalMemberCardActivity
    {
        private SqlHelper sqlHelper;
        public InternalMemberCardActivity(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateMemberCardActivity";
        internal int EditMemberCardActivity(Member_Card_Activity model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@CardID", model.CardID);
            cmd.AddParam("@ACTID", model.ACTID);
            cmd.AddParam("@Actnum", model.Actnum);
            cmd.AddParam("@USEstate", model.USEstate);
            cmd.AddParam("@USEsubdate", model.USEsubdate);


            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }

        internal Member_Card_Activity GetCardActInfo(int cardid, int actid)
        {
            const string sqltxt = @"SELECT [id]
      ,[CardID]
      ,[ACTID]
      ,[Actnum]
      ,[USEstate]
      ,[USEsubdate]
  FROM [EtownDB].[dbo].[Member_Card_Activity] where cardid=@cardid and actid=@actid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@cardid", cardid);
            cmd.AddParam("@actid", actid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Member_Card_Activity
                    {
                        Id = reader.GetValue<int>("id"),
                        ACTID = reader.GetValue<int>("CardID"),
                        CardID = reader.GetValue<int>("ACTID"),
                        Actnum = reader.GetValue<int>("Actnum"),
                        USEstate = reader.GetValue<int>("USEstate"),
                        USEsubdate = reader.GetValue<DateTime>("USEsubdate"),


                    };
                }
                return null;
            }
        }
    }
}

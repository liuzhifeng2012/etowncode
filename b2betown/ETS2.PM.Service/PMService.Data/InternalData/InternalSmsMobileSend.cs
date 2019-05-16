using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalSmsMobileSend
    {

        private SqlHelper sqlHelper;
        public InternalSmsMobileSend(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        #region 发送电子票电子票信息
        private static readonly string SQLInsertOrUpdate = "usp_Insertsmsmoblesend";//调用存储过程，
        internal int InsertOrUpdate(B2b_smsmobilesend model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);

            cmd.AddParam("@mobile", model.Mobile);
            cmd.AddParam("@content", model.Content);
            cmd.AddParam("@flag", model.Flag);
            cmd.AddParam("@text", model.Text);
            cmd.AddParam("@delaysendtime", model.Delaysendtime);
            cmd.AddParam("@oid", model.Oid);
            cmd.AddParam("@realsendtime", model.Realsendtime);
            cmd.AddParam("@smsid", model.Smsid);
            cmd.AddParam("@sendeticketid", model.Sendeticketid);
            cmd.AddParam("@pno", model.Pno);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion

        #region 发送电子票电子票信息
        internal B2b_smsmobilesend Searchsmslog(int oid)
        {

            const string sqltxt = @"SELECT *
  FROM [dbo].[smsmobilesend] where [oid]=@oid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@oid", oid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_smsmobilesend
                    {
                        Id = reader.GetValue<int>("id"),
                        Content = reader.GetValue<string>("Content"),
                        Mobile = reader.GetValue<string>("Mobile"),
                        Flag = reader.GetValue<int>("Flag"),
                        Smsid = reader.GetValue<int>("Smsid"),
                        Pno = reader.GetValue<string>("Pno")
                    };
                }
                return null;
            }
        }
        #endregion


        internal List<B2b_smsmobilesend> GetTop5SendFail()
        {
            string sql = "select top 10 * from SmsMobileSend where " +
                            " oid in (select id from b2b_order where order_state in (4,22) and send_state!=2) " +
                            " and oid>0 " +
                            " and pno is not null " +
                            " and oid in (select oid from SmsMobileSend group by oid having count(1)<3)";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<B2b_smsmobilesend> list = new List<B2b_smsmobilesend>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_smsmobilesend()
                    {
                        Id = reader.GetValue<int>("id"),
                        Content = reader.GetValue<string>("Content"),
                        Mobile = reader.GetValue<string>("Mobile"),
                        Flag = reader.GetValue<int>("Flag"),
                        Smsid = reader.GetValue<int>("Smsid"),
                        Oid = reader.GetValue<int>("oid"),
                        Sendeticketid = reader.GetValue<int>("sendEticketid"),
                         Pno=reader.GetValue<string>("pno"),
                        Text = reader.GetValue<string>("text"),
                        Realsendtime = reader.GetValue<DateTime>("RealSendTime"),
                        Delaysendtime = reader.GetValue<string>("DelaySendTime")
                    });
                }
                return list;
            }
        }
    }
}

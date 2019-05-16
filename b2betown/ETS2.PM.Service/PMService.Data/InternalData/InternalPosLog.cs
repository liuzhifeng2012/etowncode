using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalPosLog
    {
        public SqlHelper sqlHelper;
        public InternalPosLog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        #region 编辑pos日志信息
        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdatePosLog";
        internal int InsertOrUpdate(Pos_log model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);

            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Str", model.Str);
            cmd.AddParam("@SubDate", model.Subdate);
            cmd.AddParam("@Uip", model.Uip);
            cmd.AddParam("@ReturnStr", model.ReturnStr);
            cmd.AddParam("@ReturnSubDate", model.ReturnSubdate);


            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion

        internal Pos_log GetPosLogById(int poslogid)
        {
            const string sqltxt = @"SELECT [id]
      ,[str]
      ,[subdate]
      ,[uip]
      ,[returnstr]
      ,[returndate]
  FROM [EtownDB].[dbo].[pos_log]  where [Id]=@poslogid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@poslogid", poslogid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Pos_log
                    {
                        Id = reader.GetValue<int>("id"),
                        Str = reader.GetValue<string>("Str"),
                        Subdate = reader.GetValue<DateTime>("Subdate"),
                        Uip = reader.GetValue<string>("Uip"),
                        ReturnStr = reader.GetValue<string>("returnstr"),
                        ReturnSubdate = reader.GetValue<DateTime>("returndate")
                    };
                }
                return null;
            }
        }

        internal List<Pos_log> GetPosLogList(int pageindex, int pagesize, string starttime, string key, out int totalcount)
        {
            string condition = "1=1 ";
            if (key != "")
            {
                condition += " and str like '%" + key + "%' ";
            }
            if (starttime != "")
            {
                condition += " and convert(varchar(10),subdate,120)='" + starttime + "'";
            }
            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            cmd.PagingCommand1("pos_log", "*", "id desc", "", pagesize, pageindex, "", condition);

            List<Pos_log> list = new List<Pos_log>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Pos_log
                    {
                        Id = reader.GetValue<int>("id"),
                        Str = reader.GetValue<string>("Str"),
                        Subdate = reader.GetValue<DateTime>("Subdate"),
                        Uip = reader.GetValue<string>("Uip"),
                        ReturnStr = reader.GetValue<string>("returnstr"),
                        ReturnSubdate = reader.GetValue<DateTime>("returndate")
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }
    }
}

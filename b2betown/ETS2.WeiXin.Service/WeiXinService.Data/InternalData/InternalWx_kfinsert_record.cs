using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class InternalWx_kfinsert_record
    {
        public SqlHelper sqlHelper;
        public InternalWx_kfinsert_record(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int Insertkfinsert_record(Wx_kfinsert_record kfinsert_record)
        {
            string sql = @"INSERT INTO [EtownDB].[dbo].[wx_kfinsert_record]
           ([openid]
           ,[kf_id]
           ,[kf_account]
           ,[comid]
           ,[lastinserttime])
     VALUES
           (@openid 
           ,@kf_id 
           ,@kf_account 
           ,@comid 
           ,@lastinserttime )";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@openid", kfinsert_record.Openid);
            cmd.AddParam("@kf_id", kfinsert_record.Kf_id);
            cmd.AddParam("@kf_account", kfinsert_record.Kf_account);
            cmd.AddParam("@comid", kfinsert_record.Comid);
            cmd.AddParam("@lastinserttime", kfinsert_record.Lastinserttime);

            return cmd.ExecuteNonQuery();
        }

        internal Wx_kfinsert_record GetLastRecord(string openid, int comid)
        {
            string sql = @"SELECT top 1  [id]
      ,[openid]
      ,[kf_id]
      ,[kf_account]
      ,[comid]
      ,[lastinserttime]
  FROM [EtownDB].[dbo].[wx_kfinsert_record] where openid=@openid and comid=@comid order by lastinserttime desc";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@openid", openid);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                Wx_kfinsert_record m = null;
                if (reader.Read())
                {
                    m = new Wx_kfinsert_record
                    {
                        Id = reader.GetValue<int>("id"),
                        Openid = reader.GetValue<string>("openid"),
                        Kf_id = reader.GetValue<int>("kf_id"),
                        Kf_account = reader.GetValue<string>("kf_account"),
                        Comid = reader.GetValue<int>("comid"),
                        Lastinserttime = reader.GetValue<DateTime>("lastinserttime")
                    };
                }
                return m;
            }
        }
    }
}

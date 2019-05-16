using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalApi_yg_Syncordernotice
    {
        public SqlHelper sqlHelper;
        public InternalApi_yg_Syncordernotice(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int EditNotice(Api_yg_Syncordernotice m)
        {
            if (m.id > 0)
            {
                string sql = @"UPDATE  [api_yg_Syncordernotice]
                               SET [req_seq] = @req_seq 
                                  ,[platform_req_seq] = @platform_req_seq 
                                  ,[order_num] = @order_num 
                                  ,[num] = @num 
                                  ,[use_time] = @use_time 
                                  ,[rcontent] = @rcontent 
                                  ,[orderid] = @orderid 
                             WHERE id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", m.id);
                cmd.AddParam("@req_seq", m.req_seq);
                cmd.AddParam("@platform_req_seq", m.platform_req_seq);
                cmd.AddParam("@order_num", m.order_num);
                cmd.AddParam("@num", m.num);
                cmd.AddParam("@use_time", m.use_time);
                cmd.AddParam("@rcontent", m.rcontent);
                cmd.AddParam("@orderid", m.orderid);

                cmd.ExecuteNonQuery();
                return m.id;
            }
            else
            {
                string sql = @"INSERT  [api_yg_Syncordernotice]
                                   ([req_seq]
                                   ,[platform_req_seq]
                                   ,[order_num]
                                   ,[num]
                                   ,[use_time]
                                   ,[rcontent]
                                   ,[orderid])
                             VALUES
                                   (@req_seq 
                                   ,@platform_req_seq 
                                   ,@order_num 
                                   ,@num 
                                   ,@use_time 
                                   ,@rcontent 
                                   ,@orderid);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@req_seq", m.req_seq);
                cmd.AddParam("@platform_req_seq", m.platform_req_seq);
                cmd.AddParam("@order_num", m.order_num);
                cmd.AddParam("@num", m.num);
                cmd.AddParam("@use_time", m.use_time);
                cmd.AddParam("@rcontent", m.rcontent);
                cmd.AddParam("@orderid", m.orderid);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
        }

        internal Api_yg_Syncordernotice GetSucNotice(string platform_req_seq)
        {
            string sql = "select * from api_yg_Syncordernotice where platform_req_seq='" + platform_req_seq + "' and rcontent='success'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                Api_yg_Syncordernotice m = null;
                if (reader.Read())
                {
                    m = new Api_yg_Syncordernotice
                    {
                        id = reader.GetValue<int>("id"),
                        req_seq = reader.GetValue<string>("req_seq"),
                        platform_req_seq = reader.GetValue<string>("platform_req_seq"),
                        order_num = reader.GetValue<string>("order_num"),
                        num = reader.GetValue<int>("num"),
                        use_time = reader.GetValue<DateTime>("use_time"),
                        rcontent = reader.GetValue<string>("rcontent"),
                        orderid = reader.GetValue<int>("orderid"),
                    };
                }
                return m;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalapi_mjld_AsyncBacknotice
    {
        public SqlHelper sqlHelper;
        public Internalapi_mjld_AsyncBacknotice(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal int EditBacknotice(Api_mjld_AsyncBacknotice m)
        {
            if (m.id > 0)
            {
                string sql = @"UPDATE [api_mjld_AsyncBacknotice]
                                   SET [type] = @type 
                                      ,[mjldorderid] = @mjldorderid 
                                      ,[backCount] = @backCount 
                                      ,[backStatus] = @backStatus 
                                      ,[postTime] = @postTime 
                                      ,[rcontent] = @rcontent 
                                      ,[orderid] = @orderid 
                                 WHERE id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", m.id);
                cmd.AddParam("@type", m.type);
                cmd.AddParam("@mjldorderid", m.mjldorderid);
                cmd.AddParam("@backCount", m.backCount);
                cmd.AddParam("@backStatus", m.backStatus);
                cmd.AddParam("@postTime", m.postTime);
                cmd.AddParam("@rcontent", m.rcontent);
                cmd.AddParam("@orderid", m.orderid);
                cmd.ExecuteNonQuery();

                return m.id;
            }
            else
            {
                string sql = @"INSERT  [api_mjld_AsyncBacknotice]
                                   ([type]
                                   ,[mjldorderid]
                                   ,[backCount]
                                   ,[backStatus]
                                   ,[postTime]
                                   ,[rcontent]
                                   ,[orderid])
                             VALUES
                                   (@type 
                                   ,@mjldorderid 
                                   ,@backCount 
                                   ,@backStatus 
                                   ,@postTime 
                                   ,@rcontent
                                   ,@orderid);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@type", m.type);
                cmd.AddParam("@mjldorderid", m.mjldorderid);
                cmd.AddParam("@backCount", m.backCount);
                cmd.AddParam("@backStatus", m.backStatus);
                cmd.AddParam("@postTime", m.postTime);
                cmd.AddParam("@rcontent", m.rcontent);
                cmd.AddParam("@orderid", m.orderid);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
        }

        internal Api_mjld_AsyncBacknotice GetSucApi_mjld_AsyncBacknotice(string mjldorderid)
        {
            string sql = "select * from api_mjld_AsyncBacknotice where mjldorderid='" + mjldorderid + "' and rcontent='1'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                Api_mjld_AsyncBacknotice m = null;
                if (reader.Read())
                {
                    m = new Api_mjld_AsyncBacknotice
                    {
                        id = reader.GetValue<int>("id"),
                        type = reader.GetValue<string>("type"),
                        mjldorderid = reader.GetValue<string>("mjldorderid"),
                        backCount = reader.GetValue<int>("backCount"),
                        backStatus = reader.GetValue<int>("backStatus"),
                        postTime = reader.GetValue<string>("postTime"),
                        rcontent = reader.GetValue<string>("rcontent"),
                        orderid = reader.GetValue<int>("orderid"),

                    };
                }
                return m;
            }
        }
    }
}

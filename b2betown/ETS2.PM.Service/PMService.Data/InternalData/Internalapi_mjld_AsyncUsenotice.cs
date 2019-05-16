using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalapi_mjld_AsyncUsenotice
    {
        public SqlHelper sqlHelper;
        public Internalapi_mjld_AsyncUsenotice(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }


        internal Api_mjld_AsyncUsenotice GetSucUseNoticeByExchangeId(string exchangeId)
        {
            string sql = "select * from api_mjld_AsyncUsenotice where exchangeId='" + exchangeId + "' and rcontent='1'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                Api_mjld_AsyncUsenotice m = null;
                if (reader.Read())
                {
                    m = new Api_mjld_AsyncUsenotice
                    {
                        id = reader.GetValue<int>("id"),
                        type = reader.GetValue<string>("type"),
                        mjldOrderId = reader.GetValue<string>("mjldOrderId"),
                        credence = reader.GetValue<string>("credence"),
                        useCount = reader.GetValue<int>("useCount"),
                        lastCount = reader.GetValue<int>("lastCount"),
                        useTime = reader.GetValue<string>("useTime"),
                        exchangeId = reader.GetValue<string>("exchangeId"),
                        ScenicId = reader.GetValue<string>("ScenicId"),
                        postTime = reader.GetValue<string>("postTime"),
                        rcontent = reader.GetValue<string>("rcontent"),
                        orderId = reader.GetValue<int>("orderId"),
                    };
                }
                return m;
            }
        }

        internal int EditUsenotice(Api_mjld_AsyncUsenotice m)
        {
            if (m.id > 0)
            {
                string sql = @"UPDATE [api_mjld_AsyncUsenotice]
                               SET [type] = @type 
                                  ,[mjldOrderId] = @mjldOrderId 
                                  ,[credence] = @credence 
                                  ,[useCount] = @useCount 
                                  ,[lastCount] = @lastCount 
                                  ,[useTime] = @useTime 
                                  ,[exchangeId] = @exchangeId 
                                  ,[ScenicId] = @ScenicId 
                                  ,[postTime] = @postTime 
                                  ,[rcontent] = @rcontent 
                                  ,[orderId] = @orderId 
                             WHERE id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", m.id);
                cmd.AddParam("@type", m.type);
                cmd.AddParam("@mjldOrderId", m.mjldOrderId);
                cmd.AddParam("@credence", m.credence);
                cmd.AddParam("@useCount", m.useCount);
                cmd.AddParam("@lastCount", m.lastCount);
                cmd.AddParam("@useTime", m.useTime);
                cmd.AddParam("@exchangeId", m.exchangeId);
                cmd.AddParam("@ScenicId", m.ScenicId);
                cmd.AddParam("@postTime", m.postTime);
                cmd.AddParam("@rcontent", m.rcontent);
                cmd.AddParam("@orderId", m.orderId);

                cmd.ExecuteNonQuery();
                return m.id;
            }
            else
            {
                string sql = @"INSERT  [api_mjld_AsyncUsenotice]
                                       ([type]
                                       ,[mjldOrderId]
                                       ,[credence]
                                       ,[useCount]
                                       ,[lastCount]
                                       ,[useTime]
                                       ,[exchangeId]
                                       ,[ScenicId]
                                       ,[postTime]
                                       ,[rcontent]
                                       ,[orderId])
                                 VALUES
                                       (@type 
                                       ,@mjldOrderId 
                                       ,@credence 
                                       ,@useCount 
                                       ,@lastCount 
                                       ,@useTime 
                                       ,@exchangeId 
                                       ,@ScenicId 
                                       ,@postTime 
                                       ,@rcontent 
                                       ,@orderId);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@type", m.type);
                cmd.AddParam("@mjldOrderId", m.mjldOrderId);
                cmd.AddParam("@credence", m.credence);
                cmd.AddParam("@useCount", m.useCount);
                cmd.AddParam("@lastCount", m.lastCount);
                cmd.AddParam("@useTime", m.useTime);
                cmd.AddParam("@exchangeId", m.exchangeId);
                cmd.AddParam("@ScenicId", m.ScenicId);
                cmd.AddParam("@postTime", m.postTime);
                cmd.AddParam("@rcontent", m.rcontent);
                cmd.AddParam("@orderId", m.orderId);


                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
        }
    }
}

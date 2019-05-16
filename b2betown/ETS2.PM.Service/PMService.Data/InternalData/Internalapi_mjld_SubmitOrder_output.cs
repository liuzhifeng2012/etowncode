using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalapi_mjld_SubmitOrder_output
    {
        public SqlHelper sqlHelper;
        public Internalapi_mjld_SubmitOrder_output(SqlHelper sqlHelper) 
        {
            this.sqlHelper = sqlHelper;
        }

        internal int EditApi_Mjld_SubmitOrder_output(Api_Mjld_SubmitOrder_output m)
        {
            if (m.id > 0)
            {
                return 0;
            }
            else 
            {
                string sql = @"INSERT  [api_mjld_SubmitOrder_output]
                                   ([timeStamp]
                                   ,[mjldOrderId]
                                   ,[endTime]
                                   ,[credence]
                                   ,[inCount]
                                   ,[status]
                                   ,[orderId])
                             VALUES
                                   (@timeStamp 
                                   ,@mjldOrderId 
                                   ,@endTime 
                                   ,@credence
                                   ,@inCount 
                                   ,@status 
                                   ,@orderId);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@timeStamp",m.timeStamp);
                cmd.AddParam("@mjldOrderId", m.mjldOrderId);
                cmd.AddParam("@endTime", m.endTime);
                cmd.AddParam("@credence", m.credence);
                cmd.AddParam("@inCount", m.inCount);
                cmd.AddParam("@status", m.status);
                cmd.AddParam("@orderId", m.orderId);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
        }

        internal Api_Mjld_SubmitOrder_output GetApi_Mjld_SubmitOrder_output(int orderid)
        {
            string sql = "select * from api_mjld_SubmitOrder_output where orderid="+orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using(var reader=cmd.ExecuteReader())
            {
                Api_Mjld_SubmitOrder_output m = null;
                if(reader.Read())
                {
                    m = new Api_Mjld_SubmitOrder_output 
                    {
                     id=reader.GetValue<int>("id"),
                     timeStamp = reader.GetValue<string>("timeStamp"),
                     mjldOrderId = reader.GetValue<string>("mjldOrderId"),
                     endTime = reader.GetValue<string>("endTime"),
                     credence = reader.GetValue<string>("credence"),
                     inCount = reader.GetValue<string>("inCount"),
                     status = reader.GetValue<int>("status"),
                     orderId = reader.GetValue<int>("orderId"),

                    };
                }
                return m;
            }
        }

        internal string GetMjldinsureNo(int orderid)
        {
            string sql = "select mjldOrderId from api_mjld_SubmitOrder_output where orderid="+orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using(var reader=cmd.ExecuteReader())
            { 
                if(reader.Read())
                {
                    return reader.GetValue<string>("mjldOrderId");
                }
                return "";
            }
        }
    }
}

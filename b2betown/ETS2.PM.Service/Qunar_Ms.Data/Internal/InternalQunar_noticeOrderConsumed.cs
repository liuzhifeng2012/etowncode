using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.Qunar_Ms.Data.Internal
{
    public class InternalQunar_noticeOrderConsumed
    {
        public SqlHelper sqlHelper;
        public InternalQunar_noticeOrderConsumed(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int InsRequest(string partnerorderId, string orderQuantity, string useQuantity, string consumeInfo)
        {
            try
            {
                string sql = @"INSERT INTO  [qunar_noticeOrderConsumed]
           ([partnerorderId]
           ,[orderQuantity]
           ,[useQuantity]
           ,[consumeInfo]
           ,[r_message]
           ,[r_response])
     VALUES
           (@partnerorderId 
           ,@orderQuantity 
           ,@useQuantity 
           ,@consumeInfo 
           ,@r_message 
           ,@r_response);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@partnerorderId", partnerorderId);
                cmd.AddParam("@orderQuantity", orderQuantity);
                cmd.AddParam("@useQuantity", useQuantity);
                cmd.AddParam("@consumeInfo", consumeInfo);
                cmd.AddParam("@r_message", "");
                cmd.AddParam("@r_response", "");
                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
            catch
            {
                return 0;
            }
        }

        internal int InsResponse(string message, string data, int qunar_requestid)
        {
            try
            {
                string sql = "update qunar_noticeOrderConsumed set r_message='" + message + "',r_response='" + data + "' where id=" + qunar_requestid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                return cmd.ExecuteNonQuery();
            }
            catch
            {
                return 0;
            }
        }
    }
}

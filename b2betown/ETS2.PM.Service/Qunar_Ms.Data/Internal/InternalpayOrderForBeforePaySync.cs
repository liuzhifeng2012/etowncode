using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.Qunar_Ms.Data.Internal
{
    public class InternalpayOrderForBeforePaySync
    {
        public SqlHelper sqlHelper;
        public InternalpayOrderForBeforePaySync(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal bool Ishasrequest(string partnerOrderId)
        {
            string sql = "select count(1) from qunar_payOrderForBeforePaySync where partnerOrderId='" + partnerOrderId + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            if (int.Parse(o.ToString()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal int InsQunar_payOrderForBeforePaySync_request(string partnerOrderId, string orderStatus, string orderPrice, string paymentSerialno, string eticketNo)
        {
            try
            {
                string sql = @"INSERT INTO  [qunar_payOrderForBeforePaySync]
           ([partnerOrderId]
           ,[orderStatus]
           ,[orderPrice]
           ,[paymentSerialno]
           ,[eticketNo]
           ,[r_partnerorderId]
           ,[r_orderStatus]
           ,[r_eticketNo]
           ,[r_response])
     VALUES
           (@partnerOrderId 
           ,@orderStatus 
           ,@orderPrice 
           ,@paymentSerialno 
           ,@eticketNo 
           ,@r_partnerorderId 
           ,@r_orderStatus 
           ,@r_eticketNo 
           ,@r_response)";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@partnerOrderId", partnerOrderId);
                cmd.AddParam("@orderStatus", orderStatus);
                cmd.AddParam("@orderPrice", orderPrice);
                cmd.AddParam("@paymentSerialno", paymentSerialno);
                cmd.AddParam("@eticketNo", eticketNo);
                cmd.AddParam("@r_partnerorderId", "");
                cmd.AddParam("@r_orderStatus", "");
                cmd.AddParam("@r_eticketNo", "");
                cmd.AddParam("@r_response", "");
                return cmd.ExecuteNonQuery();
            }
            catch
            {
                return 0;
            }
        }

        internal int InsQunar_payOrderForBeforePaySync_response(string partnerOrderId, string orderStatus, string eticketNo, string responseParam)
        {
            try
            {
                string sql = @"UPDATE  [qunar_payOrderForBeforePaySync]
                                   SET  [r_partnerorderId] = @r_partnerorderId 
                                      ,[r_orderStatus] = @r_orderStatus 
                                      ,[r_eticketNo] = @r_eticketNo 
                                      ,[r_response] = @r_response 
                                 WHERE [partnerOrderId] = @partnerOrderId ";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@r_partnerorderId", partnerOrderId);
                cmd.AddParam("@r_orderStatus", orderStatus);
                cmd.AddParam("@r_eticketNo", eticketNo);
                cmd.AddParam("@r_response", responseParam);
                cmd.AddParam("@partnerOrderId", partnerOrderId);
                return cmd.ExecuteNonQuery();
            }
            catch
            {
                return 0;
            }
        }
    }
}

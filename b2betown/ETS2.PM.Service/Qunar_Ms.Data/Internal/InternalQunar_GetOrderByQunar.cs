using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.Qunar_Ms.Data.Internal
{
    public class InternalQunar_GetOrderByQunar
    {
        public SqlHelper sqlHelper;
        public InternalQunar_GetOrderByQunar(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int InsQunar_GetOrderByQunar_request(string partnerOrderId)
        {
            try
            {
                string sql = @"INSERT INTO  [qunar_GetOrderByQunar]
           ([partnerOrderId]
           ,[r_partnerOrderId]
           ,[r_orderStatus]
           ,[r_orderQuantity]
           ,[r_eticketNo]
           ,[r_eticketSended]
           ,[r_useQuantity]
           ,[r_consumeInfo]
           ,[r_response])
     VALUES
           (@partnerOrderId 
           ,@r_partnerOrderId 
           ,@r_orderStatus
           ,@r_orderQuantity 
           ,@r_eticketNo 
           ,@r_eticketSended 
           ,@r_useQuantity 
           ,@r_consumeInfo 
           ,@r_response )";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@partnerOrderId", partnerOrderId);
                cmd.AddParam("@r_partnerOrderId", 0);
                cmd.AddParam("@r_orderStatus", "");
                cmd.AddParam("@r_orderQuantity", "");
                cmd.AddParam("@r_eticketNo", "");
                cmd.AddParam("@r_eticketSended", "");
                cmd.AddParam("@r_useQuantity", "");
                cmd.AddParam("@r_consumeInfo", "");
                cmd.AddParam("@r_response", "");
                return cmd.ExecuteNonQuery();
            }
            catch 
            {
                return 0;
            }
        }

        internal int InsQunar_GetOrderByQunar_response(string partnerOrderId, string orderstatus, string orderQuantity, string eticketNo, string eticketSended, int total_consumenum, string consumeInfo, string responseParam)
        {
            try
            {
                string sql = @"UPDATE  [qunar_GetOrderByQunar]
                           SET  [r_partnerOrderId] = @r_partnerOrderId 
                              ,[r_orderStatus] = @r_orderStatus 
                              ,[r_orderQuantity] = @r_orderQuantity 
                              ,[r_eticketNo] = @r_eticketNo 
                              ,[r_eticketSended] = @r_eticketSended 
                              ,[r_useQuantity] = @r_useQuantity 
                              ,[r_consumeInfo] = @r_consumeInfo 
                              ,[r_response] = @r_response 
                         WHERE [partnerOrderId] = @partnerOrderId ";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@partnerOrderId", partnerOrderId);
                cmd.AddParam("@r_partnerOrderId", partnerOrderId);
                cmd.AddParam("@r_orderStatus", orderstatus);
                cmd.AddParam("@r_orderQuantity", orderQuantity);
                cmd.AddParam("@r_eticketNo", eticketNo);
                cmd.AddParam("@r_eticketSended", eticketSended);
                cmd.AddParam("@r_useQuantity", total_consumenum);
                cmd.AddParam("@r_consumeInfo", consumeInfo);
                cmd.AddParam("@r_response", responseParam);
                return cmd.ExecuteNonQuery();
            }
            catch 
            {
                return 0;
            }
        }
    }
}

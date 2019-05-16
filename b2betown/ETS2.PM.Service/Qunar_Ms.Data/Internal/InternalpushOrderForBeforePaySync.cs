using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.Qunar_Ms.Data.Internal
{
    public class InternalpushOrderForBeforePaySync
    {
        public SqlHelper sqlHelper;
        public InternalpushOrderForBeforePaySync(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int InsRequest_cancel(string partnerorderId, string orderStatus, string cancelreason, string c_mobile)
        {
            try
            {
                string sql = @"INSERT INTO [qunar_pushOrderForBeforePaySync]
           ([partnerOrderId]
           ,[orderStatus]
           ,[cancelReason]
           ,[c_name]
           ,[c_namePinyin]
           ,[c_mobile]
           ,[c_email]
           ,[c_address]
           ,[c_zipCode]
           ,[orderRemark]
           ,[r_partnerOrderId]
           ,[r_orderStatus]
           ,[r_response])
     VALUES
           (@partnerOrderId 
           ,@orderStatus 
           ,@cancelReason 
           ,@c_name 
           ,@c_namePinyin 
           ,@c_mobile 
           ,@c_email 
           ,@c_address 
           ,@c_zipCode 
           ,@orderRemark 
           ,@r_partnerOrderId
           ,@r_orderStatus 
           ,@r_response);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@partnerOrderId", partnerorderId);
                cmd.AddParam("@orderStatus", orderStatus);
                cmd.AddParam("@cancelReason", cancelreason);
                cmd.AddParam("@c_name", "");
                cmd.AddParam("@c_namePinyin", "");
                cmd.AddParam("@c_mobile", c_mobile);
                cmd.AddParam("@c_email", "");
                cmd.AddParam("@c_address", "");
                cmd.AddParam("@c_zipCode", "");
                cmd.AddParam("@orderRemark", "");
                cmd.AddParam("@r_partnerOrderId", "");
                cmd.AddParam("@r_orderStatus", "");
                cmd.AddParam("@r_response", "");
                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
            catch 
            {
                return 0;
            }
        }

        internal int InsRequest_contactperson(string partnerorderId, string c_name, string c_namePinyin, string c_mobile, string c_email, string c_address, string c_zipCode, string orderRemark)
        {
            try
            {
                string sql = @"INSERT INTO [qunar_pushOrderForBeforePaySync]
           ([partnerOrderId]
           ,[orderStatus]
           ,[cancelReason]
           ,[c_name]
           ,[c_namePinyin]
           ,[c_mobile]
           ,[c_email]
           ,[c_address]
           ,[c_zipCode]
           ,[orderRemark]
           ,[r_partnerOrderId]
           ,[r_orderStatus]
           ,[r_response])
     VALUES
           (@partnerOrderId 
           ,@orderStatus 
           ,@cancelReason 
           ,@c_name 
           ,@c_namePinyin 
           ,@c_mobile 
           ,@c_email 
           ,@c_address 
           ,@c_zipCode 
           ,@orderRemark 
           ,@r_partnerOrderId
           ,@r_orderStatus 
           ,@r_response);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@partnerOrderId", partnerorderId);
                cmd.AddParam("@orderStatus", "");
                cmd.AddParam("@cancelReason", "");
                cmd.AddParam("@c_name", c_name);
                cmd.AddParam("@c_namePinyin", c_namePinyin);
                cmd.AddParam("@c_mobile", c_mobile);
                cmd.AddParam("@c_email", c_email);
                cmd.AddParam("@c_address", c_address);
                cmd.AddParam("@c_zipCode", c_zipCode);
                cmd.AddParam("@orderRemark", orderRemark);
                cmd.AddParam("@r_partnerOrderId", "");
                cmd.AddParam("@r_orderStatus", "");
                cmd.AddParam("@r_response", "");
                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
            catch 
            {
                return 0;
            }
        }

        internal int InsResponse(string partnerorderId, string orderstates, string responseparam, int rid)
        {
            try
            {
                string sql = "update qunar_pushOrderForBeforePaySync set r_partnerOrderId=@r_partnerOrderId,r_orderStatus=@r_orderStatus,r_response=@r_response where id=@rid";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@r_partnerOrderId", partnerorderId);
                cmd.AddParam("@r_orderStatus", orderstates);
                cmd.AddParam("@r_response", responseparam);
                cmd.AddParam("@rid", rid);
                return cmd.ExecuteNonQuery();
            }
            catch 
            {
                return 0;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.Qunar_Ms.Data.Internal
{
    public class InternalnoticeOrderRefundedByQunar
    {
        public SqlHelper sqlHelper;
        public InternalnoticeOrderRefundedByQunar(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal bool Ishasrequest(string partnerorderId)
        {
            string sql = "select count(1) from  qunar_noticeOrderRefundedByQunar where partnerorderId='" + partnerorderId + "'";
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

        internal int Insrequest(string partnerorderId, string refundSeq, string orderQuantity, string refundQuantity, string orderPrice, string refundReason, string refundExplain, string refundJudgeMark, string orderRefundCharge)
        {
            try
            {
                string sql = @"INSERT INTO  [qunar_noticeOrderRefundedByQunar]
           ([partnerorderId]
           ,[refundSeq]
           ,[orderQuantity]
           ,[refundQuantity]
           ,[orderPrice]
           ,[refundReason]
           ,[refundExplain]
           ,[refundJudgeMark]
           ,[orderRefundCharge]
           ,[r_message]
           ,[r_response])
     VALUES
           (@partnerorderId 
           ,@refundSeq 
           ,@orderQuantity 
           ,@refundQuantity 
           ,@orderPrice 
           ,@refundReason 
           ,@refundExplain 
           ,@refundJudgeMark 
           ,@orderRefundCharge 
           ,@r_message 
           ,@r_response)";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@partnerorderId", partnerorderId);
                cmd.AddParam("@refundSeq", refundSeq);
                cmd.AddParam("@orderQuantity", orderQuantity);
                cmd.AddParam("@refundQuantity", refundQuantity);
                cmd.AddParam("@orderPrice", orderPrice);
                cmd.AddParam("@refundReason", refundReason);
                cmd.AddParam("@refundExplain", refundExplain);
                cmd.AddParam("@refundJudgeMark", refundJudgeMark);
                cmd.AddParam("@orderRefundCharge", orderRefundCharge);
                cmd.AddParam("@r_message", "");
                cmd.AddParam("@r_response", "");
                return cmd.ExecuteNonQuery();
            }
            catch 
            {
                return 0;
            }
        }

        internal int Insresponse(string partnerorderId, string message, string responseParam)
        {
            try
            {
                string sql = "update qunar_noticeOrderRefundedByQunar set r_message=@r_message,r_response=@r_response where partnerorderId=@partnerorderId";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@partnerorderId", partnerorderId);
                cmd.AddParam("@r_message", message);
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

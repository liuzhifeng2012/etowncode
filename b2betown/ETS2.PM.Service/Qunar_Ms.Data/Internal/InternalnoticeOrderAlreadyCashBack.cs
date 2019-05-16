using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.Qunar_Ms.Data.Internal
{
    public class InternalnoticeOrderAlreadyCashBack
    {
        public SqlHelper sqlHelper;
        public InternalnoticeOrderAlreadyCashBack(SqlHelper sqlHelper) 
        {
            this.sqlHelper = sqlHelper;
        }

        internal bool Ishasrequest(string partnerorderId)
        {
            string sql = "select count(1) from qunar_noticeOrderAlreadyCashBack where partnerorderId="+partnerorderId;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object c = cmd.ExecuteScalar();
            if (int.Parse(c.ToString()) > 0)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        internal int Insrequest(string partnerorderId, string orderCashBackMoney)
        {
            string sql = @"INSERT INTO  [qunar_noticeOrderAlreadyCashBack]
                                   ([partnerorderId]
                                   ,[orderCashBackMoney]
                                   ,[r_message]
                                   ,[r_response])
                             VALUES
                                   (@partnerorderId 
                                   ,@orderCashBackMoney 
                                   ,@r_message 
                                   ,@r_response)";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@partnerorderId", partnerorderId);
            cmd.AddParam("@orderCashBackMoney", orderCashBackMoney);
            cmd.AddParam("@r_message", "");
            cmd.AddParam("@r_response", "");
            return cmd.ExecuteNonQuery();
        }

        internal int InsResponse(string partnerorderId, string message, string responseparam)
        {
            string sql = "update qunar_noticeOrderAlreadyCashBack set r_message='" + message + "',r_response='" + responseparam + "' where partnerorderId="+partnerorderId;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }
    }
}

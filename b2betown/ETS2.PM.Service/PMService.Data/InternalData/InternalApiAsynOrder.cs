using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalApiAsynOrder
    {
        private SqlHelper sqlHelper;
        public InternalApiAsynOrder(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }


        internal ApiAsynOrder GetAsynOrder(string platform_req_seq)
        {
            string sql = @"SELECT   [id]
      ,[req_seq]
      ,[platform_req_seq]
      ,[order_num]
      ,[num]
      ,[use_time]
      ,serviceid
      ,issecondsend
      ,issuc
      ,logid
  FROM [EtownDB].[dbo].[api_syncorder] where platform_req_seq=@platform_req_seq  ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@platform_req_seq", platform_req_seq);

            using (var reader = cmd.ExecuteReader())
            {
                ApiAsynOrder u = null;
                if (reader.Read())
                {
                    u = new ApiAsynOrder
                    {
                        Id = reader.GetValue<int>("id"),
                        Req_seq = reader.GetValue<string>("req_seq"),
                        Platform_req_seq = reader.GetValue<string>("platform_req_seq"),
                        Order_num = reader.GetValue<string>("order_num"),
                        Num = reader.GetValue<int>("num"),
                        Use_time = reader.GetValue<DateTime>("use_time"),
                        Serviceid = reader.GetValue<int>("serviceid"),
                        Issecondsend = reader.GetValue<int>("issecondsend"),
                        Issuc = reader.GetValue<int>("issuc"),
                        Logid = reader.GetValue<int>("logid")
                    };
                }
                return u;
            }
        }

        internal int EditApiAsynOrder(int id, string req_seq, string platform_req_seq, string order_num, string num, string use_time, int serviceid, int issecondsend, int issuc, int logid)
        {
            string sql = @"INSERT INTO [EtownDB].[dbo].[api_syncorder]
           ([req_seq]
           ,[platform_req_seq]
           ,[order_num]
           ,[num]
           ,[use_time]
           ,serviceid
           ,issecondsend,issuc,logid)
     VALUES
           (@req_seq
           ,@platform_req_seq
           ,@order_num
           ,@num
           ,@use_time
           ,@serviceid
           ,@issecondsend,@issuc,@logid);select @@IDENTITY;";
            if (id > 0)
            {
                sql = "UPDATE [EtownDB].[dbo].[api_syncorder]" +
       " SET [req_seq] = @req_seq" +
          " ,[platform_req_seq] = @platform_req_seq" +
         "  ,[order_num] = @order_num" +
        "   ,[num] = @num " +
       "    ,[use_time] = @use_time" +
      "     ,[serviceid] = @serviceid,issecondsend=@issecondsend ,issuc=@issuc,logid=@logid " +
      "WHERE id=@id";
            }


            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            if (id > 0)
            {
                cmd.AddParam("@id", id);
            }
            cmd.AddParam("@req_seq", req_seq);
            cmd.AddParam("@platform_req_seq", platform_req_seq);
            cmd.AddParam("@order_num", order_num);
            cmd.AddParam("@num", num);
            cmd.AddParam("@use_time", use_time);
            cmd.AddParam("@serviceid", serviceid);
            cmd.AddParam("@issecondsend", issecondsend);
            cmd.AddParam("@issuc", issuc);
            cmd.AddParam("@logid", logid);
            if (id > 0)
            {
                cmd.ExecuteScalar();
                return id;
            }
            else
            {
                object o = cmd.ExecuteScalar();
                return o == null ? 0 : int.Parse(o.ToString());
            }
        }

        internal int GetTotalAsyncCount(string order_num, int serviceid)
        {
            string sql = "select sum(num) from api_syncorder where order_num='" + order_num + "' and serviceid=" + serviceid + " and issecondsend=0";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                var o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            catch (Exception e)
            {
                sqlHelper.Dispose();
                return 0;
            }
        }



        internal int GetCorrectAsyncOrder(string platform_req_seq)
        {
            string sql = "select count(1) from api_syncorder where platform_req_seq='" + platform_req_seq + "' and issuc=1";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                var o = cmd.ExecuteScalar();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}

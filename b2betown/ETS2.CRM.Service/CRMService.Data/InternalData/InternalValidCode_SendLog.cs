using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    public class InternalValidCode_SendLog
    {
        public SqlHelper sqlHelper;
        public InternalValidCode_SendLog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal ValidCode_SendLog GetLasterLogByMobile(string mobile, string source)
        {
            string sql = "SELECT top 1 [id] ,[mobile]   ,[randomcode]  ,[Content] ,[sendtime] ,[send_serialnum] ,[returnmsg] ,[source] ,[sendip] FROM [ValidCode_SendLog] where mobile='" + mobile + "' and source='" + source + "' and CONVERT(varchar(19), sendtime,120)>'" + DateTime.Now.AddMinutes(-30).ToString("yyyy-MM-dd HH:mm:ss") + "' order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            ValidCode_SendLog log = null;
            using (var read = cmd.ExecuteReader())
            {
                if (read.Read())
                {
                    log = new ValidCode_SendLog
                    {
                        id = read.GetValue<int>("id"),
                        mobile = read.GetValue<string>("mobile"),
                        randomcode = read.GetValue<string>("randomcode"),
                        Content = read.GetValue<string>("content"),
                        sendtime = read.GetValue<DateTime>("sendtime"),
                        send_serialnum = read.GetValue<int>("send_serialnum"),
                        returnmsg = read.GetValue<string>("returnmsg"),
                        source = read.GetValue<string>("source"),
                        sendip = read.GetValue<string>("sendip")

                    };
                }
            }
            return log;
        }

        internal int InsertLog(ValidCode_SendLog log)
        {
            string sql = @"INSERT INTO  [ValidCode_SendLog]
           ([mobile]
           ,[randomcode]
           ,[Content]
           ,[sendtime]
           ,[send_serialnum]
           ,[returnmsg]
           ,[source]
           ,[sendip])
     VALUES
           (@mobile
           ,@radomcode 
           ,@Content 
           ,@sendtime 
           ,@send_serialnum 
           ,@returnmsg 
           ,@source 
           ,@sendip)";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@mobile", log.mobile);
            cmd.AddParam("@radomcode", log.randomcode);
            cmd.AddParam("@Content", log.Content);
            cmd.AddParam("@sendtime", log.sendtime);
            cmd.AddParam("@send_serialnum", log.send_serialnum);
            cmd.AddParam("@returnmsg", log.returnmsg);
            cmd.AddParam("@source", log.source);
            cmd.AddParam("@sendip", log.sendip);
            return cmd.ExecuteNonQuery();
        }

        internal int GetSendNumIn30ByMobile(string mobile, string source)
        {
            string sql = "select count(1) from ValidCode_SendLog where mobile='" + mobile + "' and source='" + source + "' and CONVERT(varchar(19), sendtime,120)>'" + DateTime.Now.AddMinutes(-30).ToString("yyyy-MM-dd HH:mm:ss") + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            return int.Parse(o.ToString());
        }

        internal DateTime GetFirstsendtimeByCode(string mobile, string source, decimal code)
        {
            string sql = "select top 1 sendtime from  ValidCode_SendLog where mobile='" + mobile + "' and source='" + source + "' and randomcode='" + code + "' order by id ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                DateTime sendtime = DateTime.Now.AddMinutes(-60);
                if (reader.Read())
                {
                    sendtime = reader.GetValue<DateTime>("sendtime");
                }
                return sendtime;
            }
        }
    }
}

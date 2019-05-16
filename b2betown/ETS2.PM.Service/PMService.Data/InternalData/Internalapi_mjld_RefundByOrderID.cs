using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalapi_mjld_RefundByOrderID
    {
        public SqlHelper sqlHelper;
        public Internalapi_mjld_RefundByOrderID(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal int Editmjldrefundlog(Api_mjld_RefundByOrderID m)
        {
            if (m.id > 0)
            {
                return 0;
            }
            else
            {
                string sql = @"INSERT  [api_mjld_RefundByOrderID]
                                   ([timeStamp]
                                   ,[user]
                                   ,[password]
                                   ,[RefundPart]
                                   ,[mjldorderId]
                                   ,[rtimeStamp]
                                   ,[status]
                                   ,[scredenceno]
                                   ,[fcredenceno]
                                   ,[backCount]
                                   ,[orderid])
                             VALUES
                                   (@timeStamp 
                                   ,@user 
                                   ,@password 
                                   ,@RefundPart 
                                   ,@mjldorderId 
                                   ,@rtimeStamp 
                                   ,@status 
                                   ,@scredenceno 
                                   ,@fcredenceno 
                                   ,@backCount 
                                   ,@orderid);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@timeStamp", m.timeStamp);
                cmd.AddParam("@user", m.user);
                cmd.AddParam("@password", m.password);
                cmd.AddParam("@RefundPart", m.RefundPart);
                cmd.AddParam("@mjldorderId", m.mjldorderId);
                cmd.AddParam("@rtimeStamp", m.rtimeStamp);
                cmd.AddParam("@status", m.status);
                cmd.AddParam("@scredenceno", m.scredenceno);
                cmd.AddParam("@fcredenceno", m.fcredenceno);
                cmd.AddParam("@backCount", m.backCount);
                cmd.AddParam("@orderid", m.orderid);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
        }
    }
}

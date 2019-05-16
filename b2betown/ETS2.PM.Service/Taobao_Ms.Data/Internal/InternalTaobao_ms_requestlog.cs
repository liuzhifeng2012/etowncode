using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Taobao_Ms.Model;

namespace ETS2.PM.Service.Taobao_Ms.Data.Internal
{
    public class InternalTaobao_ms_requestlog
    {
        public SqlHelper sqlHelper;
        public InternalTaobao_ms_requestlog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int EditLog(Taobao_ms_requestlog log)
        {
            if (log.id == 0)
            {
                string sql = @"INSERT INTO  [taobao_ms_requestlog]
           ([noticemethod]
           ,[parastr]
           ,[subtime]
           ,[sendip]
           ,[httpmethod]
           ,[isrightsign])
     VALUES
           (@noticemethod
           ,@parastr
           ,@subtime
           ,@sendip
           ,@httpmethod
           ,@isrightsign);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@noticemethod", log.noticemethod);
                cmd.AddParam("@parastr", log.parastr);
                cmd.AddParam("@subtime", log.subtime);
                cmd.AddParam("@sendip", log.sendip);
                cmd.AddParam("@httpmethod", log.httpmethod);
                cmd.AddParam("@isrightsign", log.isrightsign);

                try
                {
                    object o = cmd.ExecuteScalar();
                    return int.Parse(o.ToString());
                }
                catch
                {
                    sqlHelper.Dispose();
                    return 0;
                }
            }
            else
            {
                string sql = @"UPDATE [taobao_ms_requestlog]
                               SET [noticemethod] = @noticemethod 
                                  ,[parastr] = @parastr 
                                  ,[subtime] = @subtime 
                                  ,[sendip] = @sendip 
                                  ,[httpmethod] = @httpmethod
                                  ,[isrightsign] = @isrightsign
                             WHERE  id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", log.id);
                cmd.AddParam("@noticemethod", log.noticemethod);
                cmd.AddParam("@parastr", log.parastr);
                cmd.AddParam("@subtime", log.subtime);
                cmd.AddParam("@sendip", log.sendip);
                cmd.AddParam("@httpmethod", log.httpmethod);
                cmd.AddParam("@isrightsign", log.isrightsign);
                cmd.ExecuteNonQuery();
                return log.id;
            }
        }
    }
}

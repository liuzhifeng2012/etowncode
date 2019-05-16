using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalApiLog
    {
        private SqlHelper sqlHelper;
        public InternalApiLog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int EditLog(int id, int serviceid, string initxml, DateTime subdate, string returnstr, DateTime returndate, string errmsg)
        {
            string sql = @"INSERT INTO [EtownDB].[dbo].[api_log]
           ([serviceid]
           ,[str]
           ,[subdate]
           ,[returnstr]
           ,[returndate]
           ,[errmsg]
           )
            VALUES
           (@serviceid,@str,@subdate,@returnstr,@returndate,@errmsg);select @@IDENTITY;";
            if (id > 0)
            {
                sql = @"UPDATE [EtownDB].[dbo].[api_log]
                           SET [serviceid] = @serviceid
                              ,[str] = @str
                              ,[subdate] = @subdate
                              ,[returnstr] = @returnstr
                              ,[returndate] = @returndate
                              ,[errmsg] = @errmsg
                            
                         WHERE  id=@id";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            if (id > 0)
            {
                cmd.AddParam("@id", id);

            }
            cmd.AddParam("@serviceid", serviceid);
            cmd.AddParam("@str", initxml);
            cmd.AddParam("@subdate", subdate);
            cmd.AddParam("@returnstr", returnstr);
            cmd.AddParam("@returndate", returndate);
            cmd.AddParam("@errmsg", errmsg);


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

        internal int EditLog(ApiLog m)
        { 
            if (m.Id > 0)
            {
                string sql = @"update  [api_log]
                                   set serviceid=@serviceid 
                                   ,str=@str 
                                   ,subdate=@subdate
                                   ,returnstr=@returnstr 
                                   ,returndate=@returndate
                                   ,errmsg=@errmsg
                                   ,request_type=@request_type
                               where id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@serviceid", m.Serviceid);
                cmd.AddParam("@str", m.Str);
                cmd.AddParam("@subdate", m.Subdate);
                cmd.AddParam("@returnstr", m.ReturnStr);
                cmd.AddParam("@returndate", m.ReturnSubdate);
                cmd.AddParam("@errmsg", m.Errmsg);
                cmd.AddParam("@request_type", m.request_type);
                cmd.AddParam("@id", m.Id);

                cmd.ExecuteNonQuery();
                return m.Id; 
            }
            else
            {
                string sql = @"INSERT INTO  [api_log]
                                   ([serviceid]
                                   ,[str]
                                   ,[subdate]
                                   ,[returnstr]
                                   ,[returndate]
                                   ,[errmsg]
                                   ,[request_type])
                             VALUES
                                   (@serviceid 
                                   ,@str 
                                   ,@subdate
                                   ,@returnstr 
                                   ,@returndate 
                                   ,@errmsg 
                                   ,@request_type);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@serviceid", m.Serviceid);
                cmd.AddParam("@str", m.Str);
                cmd.AddParam("@subdate", m.Subdate);
                cmd.AddParam("@returnstr", m.ReturnStr);
                cmd.AddParam("@returndate", m.ReturnSubdate);
                cmd.AddParam("@errmsg", m.Errmsg);
                cmd.AddParam("@request_type", m.request_type);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
        }
    }
}

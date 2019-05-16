using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.WL.Model;

namespace ETS2.PM.Service.WL.Data.Internal
{
    public class InternalWL_reqlog
    {
        public SqlHelper sqlHelper;
        public InternalWL_reqlog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }


        internal int EditReqlog(WL_reqlog m)
        {
            if (m.id == 0)
            {
                string sql = @"INSERT INTO  [WL_reqlog]
                           ([reqstr]
                           ,[subtime]
                           ,[respstr]
                           ,[resptime]
                           ,[code]
                           ,[describe]
                           ,[req_type]
                           ,[sendip]
                           ,orderid
                           ,mtorderid
                           ,ordernum
                           ,issecond_req,stockagentcompanyid)
                     VALUES
                           (@reqstr 
                           ,@subtime 
                           ,@respstr 
                           ,@resptime 
                           ,@code
                           ,@describe 
                           ,@req_type 
                           ,@sendip
                           ,@orderid
                           ,@mtorderid
                           ,@ordernum
                           ,@issecond_req,@stockagentcompanyid);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@reqstr", m.reqstr);
                cmd.AddParam("@subtime", m.subtime);
                cmd.AddParam("@respstr", m.respstr);
                cmd.AddParam("@resptime", m.resptime);
                cmd.AddParam("@code", m.code);
                cmd.AddParam("@describe", m.describe);
                cmd.AddParam("@req_type", m.req_type);
                cmd.AddParam("@sendip", m.sendip);

                if (m.mtorderid == null)
                {
                    m.mtorderid = "";
                }
                cmd.AddParam("@mtorderid", m.mtorderid);
                if (m.orderid == null)
                {
                    m.orderid = "";
                }
                cmd.AddParam("@orderid", m.orderid);
                if (m.ordernum == null)
                {
                    m.ordernum = "";
                }
                cmd.AddParam("@ordernum", m.ordernum);
                if (m.issecond_req == null)
                {
                    m.issecond_req = 0;
                }
                cmd.AddParam("@issecond_req", m.issecond_req);
                if (m.stockagentcompanyid == null)
                {
                    m.stockagentcompanyid = 0;
                }
                cmd.AddParam("@stockagentcompanyid", m.stockagentcompanyid);

                object o = cmd.ExecuteScalar();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            else
            {
                string sql = @"UPDATE  [WL_reqlog]
                               SET [reqstr] = @reqstr 
                                  ,[subtime] = @subtime 
                                  ,[respstr] = @respstr 
                                  ,[resptime] = @resptime 
                                  ,[code] = @code 
                                  ,[describe] = @describe 
                                  ,[req_type] = @req_type 
                                  ,[sendip] = @sendip 
                                  ,mtorderid=@mtorderid
                                  ,orderid=@orderid
                                  ,ordernum=@ordernum
                                  ,issecond_req=@issecond_req
                                  ,stockagentcompanyid=@stockagentcompanyid
                             WHERE id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", m.id);
                cmd.AddParam("@reqstr", m.reqstr);
                cmd.AddParam("@subtime", m.subtime);

                if (m.respstr == null)
                {
                    m.respstr = "";
                }
                cmd.AddParam("@respstr", m.respstr);
                cmd.AddParam("@resptime", m.resptime);
                cmd.AddParam("@code", m.code);
                cmd.AddParam("@describe", m.describe);
                cmd.AddParam("@req_type", m.req_type);
                cmd.AddParam("@sendip", m.sendip);
                if (m.mtorderid == null)
                {
                    m.mtorderid = "";
                }
                cmd.AddParam("@mtorderid", m.mtorderid);
                if (m.orderid == null)
                {
                    m.orderid = "";
                }
                cmd.AddParam("@orderid", m.orderid);
                if (m.ordernum == null)
                {
                    m.ordernum = "";
                }
                cmd.AddParam("@ordernum", m.ordernum);
                if (m.issecond_req == null)
                {
                    m.issecond_req = 0;
                }
                cmd.AddParam("@issecond_req", m.issecond_req);
                if (m.stockagentcompanyid == null)
                {
                    m.stockagentcompanyid = 0;
                }
                cmd.AddParam("@stockagentcompanyid", m.stockagentcompanyid);

                cmd.ExecuteNonQuery();
                return m.id;
            }
        }





    }
}

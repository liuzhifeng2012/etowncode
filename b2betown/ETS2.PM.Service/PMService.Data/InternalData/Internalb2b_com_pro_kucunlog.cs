using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalb2b_com_pro_kucunlog
    {
        public SqlHelper sqlHelper;
        public Internalb2b_com_pro_kucunlog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int Editkucunlog(B2b_com_pro_kucunlog m)
        {
            if (m.id > 0)
            {
                string sql = @"UPDATE  [b2b_com_pro_kucunlog]
                               SET [orderid] = @orderid 
                                  ,[proid] = @proid 
                                  ,[servertype] = @servertype 
                                  ,[daydate] = @daydate 
                                  ,[proSpeciId] = @proSpeciId 
                                  ,[surplusnum] = @surplusnum 
                                  ,[opertype] = @opertype 
                                  ,[operor] = @operor 
                                  ,[opertime] = @opertime 
                                  ,oper=@oper
                             WHERE id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", m.id);
                cmd.AddParam("@orderid", m.orderid);
                cmd.AddParam("@proid", m.proid);
                cmd.AddParam("@servertype", m.servertype);
                cmd.AddParam("@daydate", m.daydate);
                cmd.AddParam("@proSpeciId", m.proSpeciId);
                cmd.AddParam("@surplusnum", m.surplusnum);
                cmd.AddParam("@opertype", m.opertype);
                cmd.AddParam("@operor", m.operor);
                cmd.AddParam("@opertime", m.opertime);
                cmd.AddParam("@oper", m.oper);
                cmd.ExecuteNonQuery();
                return m.id;
            }
            else
            {
                string sql = @"INSERT [b2b_com_pro_kucunlog]
                                   ([orderid]
                                   ,[proid]
                                   ,[servertype]
                                   ,[daydate]
                                   ,[proSpeciId]
                                   ,[surplusnum]
                                   ,[opertype]
                                   ,[operor]
                                   ,[opertime]
                                   ,oper)
                             VALUES
                                   (@orderid 
                                   ,@proid 
                                   ,@servertype 
                                   ,@daydate 
                                   ,@proSpeciId 
                                   ,@surplusnum 
                                   ,@opertype 
                                   ,@operor 
                                   ,@opertime
                                   ,@oper);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@orderid", m.orderid);
                cmd.AddParam("@proid", m.proid);
                cmd.AddParam("@servertype", m.servertype);
                cmd.AddParam("@daydate", m.daydate);
                cmd.AddParam("@proSpeciId", m.proSpeciId);
                cmd.AddParam("@surplusnum", m.surplusnum);
                cmd.AddParam("@opertype", m.opertype);
                cmd.AddParam("@operor", m.operor);
                cmd.AddParam("@opertime", m.opertime);
                cmd.AddParam("@oper", m.oper);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
        }
    }
}

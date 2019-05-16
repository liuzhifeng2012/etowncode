using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalb2b_order_busRemindSmsLog
    {
        public SqlHelper sqlHelper;

        public Internalb2b_order_busRemindSmsLog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal B2b_order_busRemindSmsLog GetB2b_order_busRemindSmsSucLog(int proid, string gooutdate)
        {
            string sql = "select * from b2b_order_busRemindSmsLog where proid=" + proid + " and traveldate='" + gooutdate + "' and issuc=1";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                B2b_order_busRemindSmsLog m = null;
                if (reader.Read())
                {
                    m = new B2b_order_busRemindSmsLog
                    {
                        id = reader.GetValue<int>("id"),
                        smscontent = reader.GetValue<string>("smscontent"),
                        sendtophones = reader.GetValue<string>("sendtophones"),
                        sendtime = reader.GetValue<DateTime>("sendtime"),
                        opertorid = reader.GetValue<int>("opertorid"),
                        opertorname = reader.GetValue<string>("opertorname"),
                        proid = reader.GetValue<int>("proid"),
                        traveldate = reader.GetValue<DateTime>("traveldate"),
                        issuc = reader.GetValue<int>("issuc")
                    };
                }
                return m;
            }
        }

        internal int EditRemindSmsLog(B2b_order_busRemindSmsLog m)
        {
            if (m.id == 0)
            {
                string sql = @"INSERT  [b2b_order_busRemindSmsLog]
                                   ([smscontent]
                                   ,[sendtophones]
                                   ,[sendtime]
                                   ,[opertorid]
                                   ,[opertorname]
                                   ,[proid]
                                   ,[traveldate]
                                   ,issuc)
                             VALUES
                                   (@smscontent 
                                   ,@sendtophones 
                                   ,@sendtime 
                                   ,@opertorid 
                                   ,@opertorname 
                                   ,@proid 
                                   ,@traveldate
                                   ,@issuc);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@smscontent", m.smscontent);
                cmd.AddParam("@sendtophones", m.sendtophones);
                cmd.AddParam("@sendtime", m.sendtime);
                cmd.AddParam("@opertorid", m.opertorid);
                cmd.AddParam("@opertorname", m.opertorname);
                cmd.AddParam("@proid", m.proid);
                cmd.AddParam("@traveldate", m.traveldate);
                cmd.AddParam("@issuc", m.issuc);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
            else
            {
                string sql = @"UPDATE  [b2b_order_busRemindSmsLog]
                               SET [smscontent] = @smscontent 
                                  ,[sendtophones] = @sendtophones 
                                  ,[sendtime] = @sendtime 
                                  ,[opertorid] = @opertorid 
                                  ,[opertorname] = @opertorname 
                                  ,[proid] = @proid 
                                  ,[traveldate] = @traveldate 
                                  ,issuc=@issuc
                             WHERE id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@smscontent", m.smscontent);
                cmd.AddParam("@sendtophones", m.sendtophones);
                cmd.AddParam("@sendtime", m.sendtime);
                cmd.AddParam("@opertorid", m.opertorid);
                cmd.AddParam("@opertorname", m.opertorname);
                cmd.AddParam("@proid", m.proid);
                cmd.AddParam("@traveldate", m.traveldate);
                cmd.AddParam("@id", m.id);
                cmd.AddParam("@issuc", m.issuc);

                cmd.ExecuteNonQuery();
                return m.id;
            }
        }

        internal int UpRemindSmsLogState(string gooutdate, int proid, int issuc)
        {
            string sql = "update B2b_order_busRemindSmsLog set issuc=0 where traveldate='" + gooutdate + "' and proid=" + proid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }
    }
}

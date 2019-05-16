using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalB2b_order_busNamelistSendLog
    {
        public SqlHelper sqlHelper;
        public InternalB2b_order_busNamelistSendLog(SqlHelper sqlHelper) 
        {
            this.sqlHelper = sqlHelper;
        }

        internal int EditLog(B2b_order_busNamelistSendLog m)
        {
            if (m.id == 0)
            {
                string sql = @"INSERT  [B2b_order_busNamelistSendLog]
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
                string sql = @"UPDATE  [B2b_order_busNamelistSendLog]
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

        internal B2b_order_busNamelistSendLog GetB2b_order_busNamelistSendSucLog(int proid, string gooutdate)
        {
            string sql = "select * from B2b_order_busNamelistSendLog where proid=" + proid + " and traveldate='" + gooutdate + "' and issuc=1";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                B2b_order_busNamelistSendLog m = null;
                if (reader.Read())
                {
                    m = new B2b_order_busNamelistSendLog
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
    }
}

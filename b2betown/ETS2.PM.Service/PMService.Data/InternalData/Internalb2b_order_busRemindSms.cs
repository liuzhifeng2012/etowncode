using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalb2b_order_busRemindSms
    {
        public SqlHelper sqlHelper;
        public Internalb2b_order_busRemindSms(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal B2b_order_busRemindSms GetB2b_order_busRemindSms(int proid, string gooutdate)
        {
            string sql = "select top 1 * from b2b_order_busRemindSms where proid=" + proid + " and traveldate='" + gooutdate + "' order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                B2b_order_busRemindSms m = null;
                if (reader.Read())
                {
                    m = new B2b_order_busRemindSms
                    {
                        id = reader.GetValue<int>("id"),
                        licenceplate = reader.GetValue<string>("licenceplate"),
                        telphone = reader.GetValue<string>("telphone"),
                        proid = reader.GetValue<int>("proid"),
                        instime = reader.GetValue<DateTime>("instime"),
                        traveldate = reader.GetValue<DateTime>("traveldate")
                    };
                }
                return m;
            }
        }

        internal int EditRemindSms(B2b_order_busRemindSms m)
        {
            if (m.id == 0)
            {
                string sql = @"INSERT  [b2b_order_busRemindSms]
                               ([licenceplate]
                               ,[telphone]
                               ,[instime]
                               ,[proid]
                               ,[traveldate])
                         VALUES
                               (@licenceplate 
                               ,@telphone 
                               ,@instime 
                               ,@proid 
                               ,@traveldate);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@licenceplate", m.licenceplate);
                cmd.AddParam("@telphone", m.telphone);
                cmd.AddParam("@instime", m.instime);
                cmd.AddParam("@proid", m.proid);
                cmd.AddParam("@traveldate", m.traveldate);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
            else
            {
                string sql = @"UPDATE  [b2b_order_busRemindSms]
                           SET [licenceplate] = @licenceplate 
                              ,[telphone] = @telphone 
                              ,[instime] = @instime 
                              ,[proid] = @proid 
                              ,[traveldate] = @traveldate 
                         WHERE id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@licenceplate", m.licenceplate);
                cmd.AddParam("@telphone", m.telphone);
                cmd.AddParam("@instime", m.instime);
                cmd.AddParam("@proid", m.proid);
                cmd.AddParam("@traveldate", m.traveldate);
                cmd.AddParam("@id", m.id);
                cmd.ExecuteNonQuery();

                return m.id;
            }

        }
    }
}

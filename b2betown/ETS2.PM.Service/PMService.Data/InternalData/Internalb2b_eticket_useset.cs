using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalb2b_eticket_useset
    {
        public SqlHelper sqlHelper;
        public Internalb2b_eticket_useset(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }



        internal int Delcomusesetbycomid(int comid)
        {
            string sql = "delete b2b_eticket_useset where comid=" + comid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int Inscomusesetbycomid(B2b_eticket_useset m)
        {
            string sql = "insert into b2b_eticket_useset (datetype,etickettype,comid,operid,opertime) values(" + m.datetype + "," + m.etickettype + "," + m.comid + "," + m.operid + ",'" + m.opertime + "')";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal List<B2b_eticket_useset> Geteticket_usesetlist(int comid,string nowdatetype="0,1,2")
        {
            string sql = "select * from b2b_eticket_useset where comid=" + comid+" and datetype in ("+nowdatetype+")";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                List<B2b_eticket_useset> list = new List<B2b_eticket_useset>();
                while (reader.Read())
                {
                    list.Add(new B2b_eticket_useset
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        datetype = reader.GetValue<int>("datetype"),
                        etickettype = reader.GetValue<int>("etickettype"),
                        operid = reader.GetValue<int>("operid"),
                        opertime = reader.GetValue<DateTime>("opertime"),

                    });
                }
                return list;
            }
        }
    }
}

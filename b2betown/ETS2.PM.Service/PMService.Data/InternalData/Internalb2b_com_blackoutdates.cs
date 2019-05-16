using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalb2b_com_blackoutdates
    {
        public SqlHelper sqlHelper;
        public Internalb2b_com_blackoutdates(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        /// <summary>
        /// state (0全部；1有效日期及今天以后的日期包括今天；2.失效日期)
        /// </summary>
        /// <param name="comid"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        internal List<B2b_com_blackoutdates> Getblackoutdatebycomid(int comid, string state)
        {
            string sql = "select * from b2b_com_blackoutdates where comid=" + comid;
            if (state != "0")
            {
                if (state == "1")
                {
                    sql += "  and convert(varchar(10), blackoutdate,120)>='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                }
                if (state == "2")
                {
                    sql += "  and convert(varchar(10), blackoutdate,120)<'" + DateTime.Now.ToString("yyyy-MM-dd") + "'";

                }
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                List<B2b_com_blackoutdates> list = new List<B2b_com_blackoutdates>();
                while (reader.Read())
                {
                    list.Add(new B2b_com_blackoutdates
                    {
                        id = reader.GetValue<int>("id"),
                        blackoutdate = reader.GetValue<DateTime>("blackoutdate"),
                        datetype = reader.GetValue<int>("datetype"),
                        comid = reader.GetValue<int>("comid"),
                        operid = reader.GetValue<int>("operid"),
                        opertime = reader.GetValue<DateTime>("opertime")
                    });
                }
                return list;
            }
        }

        internal B2b_com_blackoutdates Getblackoutdate(string daydate, int comid)
        {
            try
            {
                string sql = "select * from b2b_com_blackoutdates where comid=" + comid + " and convert(varchar(10), blackoutdate,120)='" + DateTime.Parse(daydate).ToString("yyyy-MM-dd") + "'";

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    B2b_com_blackoutdates m = null;
                    if (reader.Read())
                    {
                        m = new B2b_com_blackoutdates
                        {
                            id = reader.GetValue<int>("id"),
                            blackoutdate = reader.GetValue<DateTime>("blackoutdate"),
                            datetype = reader.GetValue<int>("datetype"),
                            comid = reader.GetValue<int>("comid"),
                            operid = reader.GetValue<int>("operid"),
                            opertime = reader.GetValue<DateTime>("opertime")
                        };
                    }
                    return m;
                }
            }
            catch
            {
                sqlHelper.Dispose();
                return null;
            }
        }


        internal int Delblackoutdate(string daydate, int comid)
        {
            string sql = "delete b2b_com_blackoutdates where comid=" + comid + "   and convert(varchar(10), blackoutdate,120)='" + DateTime.Parse(daydate).ToString("yyyy-MM-dd") + "'";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int InsB2b_com_blackoutdates(B2b_com_blackoutdates model)
        {
            string sql = "INSERT INTO b2b_com_blackoutdates ( [blackoutdate],[datetype],[comid],[operid],[opertime]) VALUES('"+model.blackoutdate+"',"+model.datetype+" ,"+model.comid+" ,"+model.operid+",'"+model.opertime+"')";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }
    }
}

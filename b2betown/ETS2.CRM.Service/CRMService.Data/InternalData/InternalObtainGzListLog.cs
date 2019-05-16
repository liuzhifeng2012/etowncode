using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    public class InternalObtainGzListLog
    {
        private SqlHelper sqlHelper;
        public InternalObtainGzListLog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int GetMaxObtainNo(int comid)
        {
            string sql = "select max(obtainno) from ObtainGzListLog where comid=" + comid;
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            catch (Exception e)
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal string GetNextOpenId(int comid, int obtainno)
        {
            string sql = "select  nextopenid from ObtainGzListLog where comid=" + comid + " and obtainno=" + obtainno;
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o.ToString();
            }
            catch (Exception e)
            {
                sqlHelper.Dispose();
                return "";
            }
        }

        internal int InsLog(int id, string total, string count, string openid, string next_openid, DateTime obtaintime, string errcode, string errmsg, int comid, int dealuserid, int MaxOtainNo)
        {
            string sql = @"INSERT INTO [EtownDB].[dbo].[ObtainGzListLog]
           ([total]
           ,[count]
           ,[openid]
           ,[nextopenid]
           ,[obtainTime]
           ,[errcode]
           ,[errmsg]
           ,[comid]
           ,[dealerid]
           ,[obtainNo])
     VALUES
           (@total
           ,@count
           ,@openid
           ,@nextopenid
           ,@obtainTime
           ,@errcode
           ,@errmsg
           ,@comid
           ,@dealerid
           ,@obtainNo)";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@total", total);
            cmd.AddParam("@count", count);
            cmd.AddParam("@openid", openid);
            cmd.AddParam("@nextopenid", next_openid);
            cmd.AddParam("@obtainTime", obtaintime);
            cmd.AddParam("@errcode", errcode);
            cmd.AddParam("@errmsg", errmsg);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@dealerid", dealuserid);
            cmd.AddParam("@obtainNo", MaxOtainNo);
            return cmd.ExecuteNonQuery();
        }

        internal List<ObtainGzListLog> GetObtaingzlistlog(int comid, int MaxOtainNo)
        {
            string sql = @"select id, [total]
           ,[count]
           ,[openid]
           ,[nextopenid]
           ,[obtainTime]
           ,[errcode]
           ,[errmsg]
           ,[comid]
           ,[dealerid]
           ,[obtainNo] from ObtainGzListLog where comid=@comid  and obtainno=@obtainno";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@obtainno", MaxOtainNo);

            try
            {
                List<ObtainGzListLog> list = new List<ObtainGzListLog>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ObtainGzListLog
                        {
                            Id = reader.GetValue<int>("id"),
                            Total = reader.GetValue<int>("total"),
                            Count = reader.GetValue<int>("count"),
                            Openid = reader.GetValue<string>("openid"),
                            Nextopenid = reader.GetValue<string>("nextopenid"),
                            Obtaintime = reader.GetValue<DateTime>("obtainTime"),
                            Errcode = reader.GetValue<string>("errcode"),
                            Errmsg = reader.GetValue<string>("errmsg"),
                            Comid = reader.GetValue<int>("comid"),
                            Dealerid = reader.GetValue<int>("dealerid"),
                            Obtainno = reader.GetValue<int>("obtainNo")
                        });

                    }
                }

                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}

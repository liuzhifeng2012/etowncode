using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    public class InternalB2bcrmlevels
    {
        public SqlHelper sqlHelper;
        public InternalB2bcrmlevels(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal List<B2bcrmlevels> Getb2bcrmlevelsbycomid(int comid,out int totalcount)
        {
            string sql = @"SELECT [id]
      ,[crmlevel]
      ,[levelname]
      ,[dengjifen_begin]
      ,[dengjifen_end]
      ,[tequan]
      ,[com_id]
      ,[isavailable]
  FROM [EtownDB].[dbo].[B2bcrmlevels]  where com_id=@comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                List<B2bcrmlevels> list = new List<B2bcrmlevels>();
                while (reader.Read())
                {
                    list.Add(new B2bcrmlevels()
                    {
                        id = reader.GetValue<int>("id"),
                        crmlevel = reader.GetValue<string>("crmlevel"),
                        levelname = reader.GetValue<string>("levelname"),
                        dengjifen_begin = reader.GetValue<decimal>("dengjifen_begin"),
                        dengjifen_end = reader.GetValue<decimal>("dengjifen_end"),
                        tequan = reader.GetValue<string>("tequan"),
                        com_id = reader.GetValue<int>("com_id"),
                        isavailable = reader.GetValue<int>("isavailable")
                    });
                }
                totalcount = list.Count;
                return list;
            }
        }

        internal int EditB2bCrmLevel(B2bcrmlevels m)
        {
            if (m.id == 0)
            {
                string sql = @"INSERT INTO [EtownDB].[dbo].[B2bcrmlevels]
           ([crmlevel]
           ,[levelname]
           ,[dengjifen_begin]
           ,[dengjifen_end]
           ,[tequan]
           ,[com_id]
           ,[isavailable])
     VALUES
           (@crmlevel
           ,@levelname
           ,@dengjifen_begin
           ,@dengjifen_end
           ,@tequan
           ,@com_id
           ,@isavailable);select @@identity;";

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@crmlevel", m.crmlevel);
                cmd.AddParam("@levelname", m.levelname);
                cmd.AddParam("@dengjifen_begin", m.dengjifen_begin);
                cmd.AddParam("@dengjifen_end", m.dengjifen_end);
                cmd.AddParam("@tequan", m.tequan);
                cmd.AddParam("@com_id", m.com_id);
                cmd.AddParam("@isavailable", m.isavailable);

                object o = cmd.ExecuteScalar();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            else 
            {
                string sql = @"UPDATE [EtownDB].[dbo].[B2bcrmlevels]
                           SET [crmlevel] = @crmlevel
                              ,[levelname] = @levelname
                              ,[dengjifen_begin] = @dengjifen_begin
                              ,[dengjifen_end] = @dengjifen_end
                              ,[tequan] = @tequan
                              ,[com_id] = @com_id
                              ,[isavailable] = @isavailable
                         WHERE id=@id";

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id",m.id);
                cmd.AddParam("@crmlevel", m.crmlevel);
                cmd.AddParam("@levelname", m.levelname);
                cmd.AddParam("@dengjifen_begin", m.dengjifen_begin);
                cmd.AddParam("@dengjifen_end", m.dengjifen_end);
                cmd.AddParam("@tequan", m.tequan);
                cmd.AddParam("@com_id", m.com_id);
                cmd.AddParam("@isavailable", m.isavailable);

                cmd.ExecuteNonQuery();
                return m.id;
            }
        }

        internal B2bcrmlevels Getb2bcrmlevel(int comid, string crmlevel)
        {
            string sql = @"SELECT [id]
      ,[crmlevel]
      ,[levelname]
      ,[dengjifen_begin]
      ,[dengjifen_end]
      ,[tequan]
      ,[com_id]
      ,[isavailable]
  FROM [EtownDB].[dbo].[B2bcrmlevels]  where com_id=@comid and crmlevel=@crmlevel";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@crmlevel",crmlevel);

            using (var reader = cmd.ExecuteReader())
            {
                 B2bcrmlevels m= null;
                if (reader.Read())
                {
                    m=new B2bcrmlevels()
                    {
                        id = reader.GetValue<int>("id"),
                        crmlevel = reader.GetValue<string>("crmlevel"),
                        levelname = reader.GetValue<string>("levelname"),
                        dengjifen_begin = reader.GetValue<decimal>("dengjifen_begin"),
                        dengjifen_end = reader.GetValue<decimal>("dengjifen_end"),
                        tequan = reader.GetValue<string>("tequan"),
                        com_id = reader.GetValue<int>("com_id"),
                        isavailable = reader.GetValue<int>("isavailable")
                    };
                }
               
                return m;
            }
        }

        internal int Getcrmlevelscount(int comid)
        {
            string sql = "select count(1) from b2bcrmlevels where com_id="+comid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            return o == null ? 0 : int.Parse(o.ToString());
        }

        internal B2bcrmlevels Getb2bcrmlevelbyweixin(string weixin, int comid)
        {
            string sql = @"SELECT [id]
      ,[crmlevel]
      ,[levelname]
      ,[dengjifen_begin]
      ,[dengjifen_end]
      ,[tequan]
      ,[com_id]
      ,[isavailable]
  FROM [EtownDB].[dbo].[B2bcrmlevels]  where com_id=@comid and crmlevel =(select crmlevel from b2b_crm where weixin=@weixin)";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@weixin", weixin);

            using (var reader = cmd.ExecuteReader())
            {
                B2bcrmlevels m = null;
                if (reader.Read())
                {
                    m = new B2bcrmlevels()
                    {
                        id = reader.GetValue<int>("id"),
                        crmlevel = reader.GetValue<string>("crmlevel"),
                        levelname = reader.GetValue<string>("levelname"),
                        dengjifen_begin = reader.GetValue<decimal>("dengjifen_begin"),
                        dengjifen_end = reader.GetValue<decimal>("dengjifen_end"),
                        tequan = reader.GetValue<string>("tequan"),
                        com_id = reader.GetValue<int>("com_id"),
                        isavailable = reader.GetValue<int>("isavailable")
                    };
                }

                return m;
            }
        }

        internal B2bcrmlevels Getb2bcrmlevelbyweixin(int comid, decimal djf_begin)
        {
            string sql = @"SELECT [id]
      ,[crmlevel]
      ,[levelname]
      ,[dengjifen_begin]
      ,[dengjifen_end]
      ,[tequan]
      ,[com_id]
      ,[isavailable]
  FROM [EtownDB].[dbo].[B2bcrmlevels]  where com_id=@comid and  dengjifen_begin=@djf_begin";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@djf_begin", djf_begin);

            using (var reader = cmd.ExecuteReader())
            {
                B2bcrmlevels m = null;
                if (reader.Read())
                {
                    m = new B2bcrmlevels()
                    {
                        id = reader.GetValue<int>("id"),
                        crmlevel = reader.GetValue<string>("crmlevel"),
                        levelname = reader.GetValue<string>("levelname"),
                        dengjifen_begin = reader.GetValue<decimal>("dengjifen_begin"),
                        dengjifen_end = reader.GetValue<decimal>("dengjifen_end"),
                        tequan = reader.GetValue<string>("tequan"),
                        com_id = reader.GetValue<int>("com_id"),
                        isavailable = reader.GetValue<int>("isavailable")
                    };
                }

                return m;
            }
        }
    }
}

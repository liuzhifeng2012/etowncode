using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{

    public class InternalObtainGzListSplit
    {
        private SqlHelper sqlHelper;
        public InternalObtainGzListSplit(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int MaxSplitNo(int comid, int OtainNo)
        {
            string sql = "SELECT MAX(splitno)  FROM [EtownDB].[dbo].[ObtainGzList_Split] where comid=" + comid + " and obtainno=" + OtainNo;
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return int.Parse(o.ToString());
            }
            catch (Exception ex)
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal int InsertsObtainGzListSplit(Modle.ObtainGzListSplit model)
        {
            string sql = @"INSERT INTO [EtownDB].[dbo].[ObtainGzList_Split]
           ([total]
           ,[splitcount]
           ,[splitopenid]
           ,[splittime]
           ,[dealerid]
           ,[comid]
           ,[obtainno]
           ,[splitno]
           ,[issuc])
     VALUES
           (@total 
           ,@splitcount 
           ,@splitopenid 
           ,@splittime 
           ,@dealerid 
           ,@comid 
           ,@obtainno 
           ,@splitno 
           ,@issuc) ";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@total", model.Total);
                cmd.AddParam("@splitcount", model.Splitcount);
                cmd.AddParam("@splitopenid", model.Splitopenid);
                cmd.AddParam("@splittime", model.Splittime);
                cmd.AddParam("@dealerid", model.Dealerid);
                cmd.AddParam("@comid", model.Comid);
                cmd.AddParam("@obtainno", model.Obtainno);
                cmd.AddParam("@splitno", model.Splitno);
                cmd.AddParam("@issuc", model.Issuc);

                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        internal ObtainGzListSplit GetObtainGzListSplit(int id)
        {
            string sql = @"SELECT [id]
      ,[total]
      ,[splitcount]
      ,[splitopenid]
      ,[splittime]
      ,[dealerid]
      ,[comid]
      ,[obtainno]
      ,[splitno]
      ,[issuc]
  FROM [EtownDB].[dbo].[ObtainGzList_Split] where id=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);

            using (var reader = cmd.ExecuteReader())
            {
                ObtainGzListSplit u = null;
                if (reader.Read())
                {
                    u = new ObtainGzListSplit
                    {
                        Id = reader.GetValue<int>("id"),
                        Total = reader.GetValue<int>("total"),
                        Splitcount = reader.GetValue<int>("splitcount"),
                        Splitopenid = reader.GetValue<string>("splitopenid"),
                        Splittime = reader.GetValue<DateTime>("splittime"),
                        Dealerid = reader.GetValue<int>("dealerid"),
                        Comid = reader.GetValue<int>("comid"),
                        Obtainno = reader.GetValue<int>("obtainno"),
                        Splitno = reader.GetValue<int>("splitno"),
                        Issuc = reader.GetValue<int>("issuc"),
                    };
                }
                return u;
            }
        }

        internal ObtainGzListSplit LastSplitNoByComid(int comid)
        {
            string sql = @"SELECT top 1  [id]
      ,[total]
      ,[splitcount]
      ,[splitopenid]
      ,[splittime]
      ,[dealerid]
      ,[comid]
      ,[obtainno]
      ,[splitno]
      ,[issuc]
  FROM [EtownDB].[dbo].[ObtainGzList_Split] where comid=@comid  order by id desc";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                ObtainGzListSplit u = null;
                if (reader.Read())
                {
                    u = new ObtainGzListSplit
                    {
                        Id = reader.GetValue<int>("id"),
                        Total = reader.GetValue<int>("total"),
                        Splitcount = reader.GetValue<int>("splitcount"),
                        Splitopenid = reader.GetValue<string>("splitopenid"),
                        Splittime = reader.GetValue<DateTime>("splittime"),
                        Dealerid = reader.GetValue<int>("dealerid"),
                        Comid = reader.GetValue<int>("comid"),
                        Obtainno = reader.GetValue<int>("obtainno"),
                        Splitno = reader.GetValue<int>("splitno"),
                        Issuc = reader.GetValue<int>("issuc"),
                    };
                }
                return u;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalB2b_com_protrip
    {
        private SqlHelper sqlHelper;
        public InternalB2b_com_protrip(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal List<B2b_com_protrip> Gettriplistbylineid(int productid)
        {
            string sql = @"SELECT   [id]
      ,[ActivityArea]
      ,[Traffic]
      ,[ScenicActivity]
      ,[Description]
      ,[Hotel]
      ,[Dining]
      ,[productid]
      ,creator
      ,createdate
  FROM [EtownDB].[dbo].[b2b_com_protrip]  where productid=@productid";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@productid", productid);

                List<B2b_com_protrip> list = new List<B2b_com_protrip>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new B2b_com_protrip()
                        {
                            Id = reader.GetValue<int>("id"),
                            ActivityArea = reader.GetValue<string>("ActivityArea"),
                            Traffic = reader.GetValue<string>("Traffic"),
                            ScenicActivity = reader.GetValue<string>("ScenicActivity"),
                            Description = reader.GetValue<string>("Description"),
                            Hotel = reader.GetValue<string>("hotel"),
                            Dining = reader.GetValue<string>("Dining"),
                            Productid = reader.GetValue<int>("productid"),
                            Creator = reader.GetValue<int>("creator"),
                            CreateDate = reader.GetValue<DateTime>("createdate")
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

        internal B2b_com_protrip GetLineTripById(int tripid, int productid)
        {
            string sql = @"SELECT   [id]
      ,[ActivityArea]
      ,[Traffic]
      ,[ScenicActivity]
      ,[Description]
      ,[Hotel]
      ,[Dining]
      ,[productid]
      ,creator
      ,createdate
  FROM [EtownDB].[dbo].[b2b_com_protrip]  where productid=@productid and id=@tripid";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@productid", productid);
                cmd.AddParam("@tripid", tripid);

                B2b_com_protrip u = null;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        u = new B2b_com_protrip()
                        {
                            Id = reader.GetValue<int>("id"),
                            ActivityArea = reader.GetValue<string>("ActivityArea"),
                            Traffic = reader.GetValue<string>("Traffic"),
                            ScenicActivity = reader.GetValue<string>("ScenicActivity"),
                            Description = reader.GetValue<string>("Description"),
                            Hotel = reader.GetValue<string>("hotel"),
                            Dining = reader.GetValue<string>("Dining"),
                            Productid = reader.GetValue<int>("productid"),
                            Creator = reader.GetValue<int>("creator"),
                            CreateDate = reader.GetValue<DateTime>("createdate")
                        };
                    }
                }
                return u;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        internal int Edittrip(B2b_com_protrip tourJourney)
        {
            if (tourJourney.Id == 0)//新插入
            {
                string sql = @"INSERT INTO [EtownDB].[dbo].[b2b_com_protrip]
                                       ([ActivityArea]
                                       ,[Traffic]
                                       ,[ScenicActivity]
                                       ,[Description]
                                       ,[Hotel]
                                       ,[Dining]
                                       ,[productid]
                                       ,[Creator]
                                       ,[CreateDate])
                                 VALUES
                                       (@ActivityArea
                                       ,@Traffic
                                       ,@ScenicActivity
                                       ,@Description
                                       ,@Hotel
                                       ,@Dining
                                       ,@productid
                                       ,@Creator
                                       ,@CreateDate);select @@identity;";

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@ActivityArea", tourJourney.ActivityArea);
                cmd.AddParam("@Traffic", tourJourney.Traffic);
                cmd.AddParam("@ScenicActivity", tourJourney.ScenicActivity);
                cmd.AddParam("@Description", tourJourney.Description);
                cmd.AddParam("@Hotel", tourJourney.Hotel);
                cmd.AddParam("@Dining", tourJourney.Dining);
                cmd.AddParam("@productid", tourJourney.Productid);
                cmd.AddParam("@Creator", tourJourney.Creator);
                cmd.AddParam("@CreateDate", tourJourney.CreateDate);

                object o = cmd.ExecuteScalar();
                return o == null ? 0 : int.Parse(o.ToString());


            }
            else//编辑
            {
                string sql = @"UPDATE [EtownDB].[dbo].[b2b_com_protrip]
                                   SET [ActivityArea] = @ActivityArea
                                      ,[Traffic] = @Traffic
                                      ,[ScenicActivity] = @ScenicActivity
                                      ,[Description] = @Description
                                      ,[Hotel] = @Hotel
                                      ,[Dining] = @Dining
                                      ,[productid] = @productid
                                      ,[Creator] = @Creator
                                      ,[CreateDate] = @CreateDate
                                 WHERE id=@Id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@Id", tourJourney.Id);
                cmd.AddParam("@ActivityArea", tourJourney.ActivityArea);
                cmd.AddParam("@Traffic", tourJourney.Traffic);
                cmd.AddParam("@ScenicActivity", tourJourney.ScenicActivity);
                cmd.AddParam("@Description", tourJourney.Description);
                cmd.AddParam("@Hotel", tourJourney.Hotel);
                cmd.AddParam("@Dining", tourJourney.Dining);
                cmd.AddParam("@productid", tourJourney.Productid);
                cmd.AddParam("@Creator", tourJourney.Creator);
                cmd.AddParam("@CreateDate", tourJourney.CreateDate);

                cmd.ExecuteNonQuery();
                return tourJourney.Id;
            }
        }

        internal int DeleteLineTrip(int tripid, int ProductId)
        {
            string sql = "delete b2b_com_protrip where id=" + tripid + " and productid=" + ProductId;
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}

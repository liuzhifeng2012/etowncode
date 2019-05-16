using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalB2b_com_roomtypeday
    {
        private SqlHelper sqlHelper;
        public InternalB2b_com_roomtypeday(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int DelRoomTypeDayByRoomTypeId(int roomtypeid)
        {
            string sql = "delete b2b_com_roomtypeday where roomtypeid=@roomtypeid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@roomtypeid", roomtypeid);



            object obj = cmd.ExecuteScalar();

            return obj != null ? int.Parse(obj.ToString()) : 0;
        }

        internal int InsertOrUpdate(B2b_com_roomtypeday model)
        {
            if (model.Id == 0)
            {
                string sql = @"INSERT INTO [EtownDB].[dbo].[b2b_com_roomtypeday]([dayprice],[dayavailablenum],[ReserveType],[daydate],[roomtypeid])
                       VALUES(@dayprice ,@dayavailablenum ,@ReserveType ,@daydate ,@roomtypeid);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@dayprice", model.Dayprice);
                cmd.AddParam("@dayavailablenum", model.Dayavailablenum);
                cmd.AddParam("@ReserveType", model.ReserveType);
                cmd.AddParam("@daydate", model.Daydate);
                cmd.AddParam("@roomtypeid", model.Roomtypeid);
                object obj = cmd.ExecuteScalar();

                return obj != null ? int.Parse(obj.ToString()) : 0;
            }
            else
            {
                string sql = @" UPDATE [EtownDB].[dbo].[b2b_com_roomtypeday]   SET [dayprice] =  @dayprice  ,[dayavailablenum] =  @dayavailablenum  ,[ReserveType] = @ReserveType  ,[daydate] = @daydate ,[roomtypeid] =@roomtypeid WHERE id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", model.Id);
                cmd.AddParam("@dayprice", model.Dayprice);
                cmd.AddParam("@dayavailablenum", model.Dayavailablenum);
                cmd.AddParam("@ReserveType", model.ReserveType);
                cmd.AddParam("@daydate", model.Daydate);
                cmd.AddParam("@roomtypeid", model.Roomtypeid);

                int upnum = cmd.ExecuteNonQuery();
                return model.Id;

            }


        }
        internal List<B2b_com_roomtypeday> GetRoomTypeDayList(int roomtypeid, out string pro_start, out string pro_end, out int totalcount)
        {
            string sql = @"SELECT [id]
      ,[dayprice]
      ,[dayavailablenum]
      ,[ReserveType]
      ,[daydate]
      ,[roomtypeid]
  FROM [EtownDB].[dbo].[b2b_com_roomtypeday] where roomtypeid=@roomtypeid ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@roomtypeid", roomtypeid);

            List<B2b_com_roomtypeday> list = new List<B2b_com_roomtypeday>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_com_roomtypeday
                    {
                        Id = reader.GetValue<int>("id"),
                        Dayprice = reader.GetValue<decimal>("dayprice"),
                        Dayavailablenum = reader.GetValue<int>("dayavailablenum"),
                        ReserveType = reader.GetValue<int>("ReserveType"),
                        Daydate = reader.GetValue<DateTime>("daydate"),
                        Roomtypeid = reader.GetValue<int>("roomtypeid")
                    });
                }
                if (list.Count > 0)
                {
                    pro_start = list[0].Daydate.ToString("yyyy-MM-dd");
                    pro_end = list[list.Count-1].Daydate.ToString("yyyy-MM-dd");
                }
                else
                {
                    pro_start = DateTime.Now.ToString("yyyy-MM-dd");
                    pro_end = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                }
                totalcount = list.Count;
                return list;
            }


        }
        internal B2b_com_roomtypeday GetRoomTypeDay(int roomtypeid)
        {
            string sql = @"SELECT [id]
      ,[dayprice]
      ,[dayavailablenum]
      ,[ReserveType]
      ,[daydate]
      ,[roomtypeid]
  FROM [EtownDB].[dbo].[b2b_com_roomtypeday] where roomtypeid=@roomtypeid ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@roomtypeid", roomtypeid);

            var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new B2b_com_roomtypeday
                {
                        Id = reader.GetValue<int>("id"),
                        Dayprice = reader.GetValue<decimal>("dayprice"),
                        Dayavailablenum = reader.GetValue<int>("dayavailablenum"),
                        ReserveType = reader.GetValue<int>("ReserveType"),
                        Daydate = reader.GetValue<DateTime>("daydate"),
                        Roomtypeid = reader.GetValue<int>("roomtypeid")

                };
            }
            return null;

        }


    }
}

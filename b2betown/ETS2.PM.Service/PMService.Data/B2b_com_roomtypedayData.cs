using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2b_com_roomtypedayData
    {
        public int DelRoomTypeDayByRoomTypeId(int roomtypeid)
        {
            using (var sql = new SqlHelper())
            {
                int result = new InternalB2b_com_roomtypeday(sql).DelRoomTypeDayByRoomTypeId(roomtypeid);
                return result;
            }
        }

        public int InsertOrUpdate(B2b_com_roomtypeday model)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2b_com_roomtypeday(sql);
                    int result = internalData.InsertOrUpdate(model);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        public List<B2b_com_roomtypeday> GetRoomTypeDayList(int roomtypeid,out string pro_start,out string pro_end, out int totalcount)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    List<B2b_com_roomtypeday> model = new InternalB2b_com_roomtypeday(sql).GetRoomTypeDayList(roomtypeid,out pro_start,out pro_end, out totalcount);
                    return model;
                }
                catch
                {
                    throw;
                }
            }
        }
        public B2b_com_roomtypeday GetRoomTypeDay(int roomtypeid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    B2b_com_roomtypeday model = new InternalB2b_com_roomtypeday(sql).GetRoomTypeDay(roomtypeid);
                    return model;
                }
                catch
                {
                    throw;
                }
            }
        }
       
    }
}

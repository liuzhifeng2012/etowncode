using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2b_com_roomtypeData
    {
        public int InsertOrUpdate( B2b_com_roomtype model)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2b_com_roomtype(sql);
                    int result = internalData.InsertOrUpdate(model);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public B2b_com_roomtype GetRoomType(int roomtypeid, int comid)
        {
             using(var sql=new SqlHelper())
             {
                 try
                 {
                     B2b_com_roomtype model = new InternalB2b_com_roomtype(sql).GetRoomType(roomtypeid,comid);
                     return model;
                 }
                 catch 
                 {
                     throw;
                 }
             }
        }

        public List<B2b_com_roomtype> GetRoomTypePageList(int pageindex, int pagesize, int comid, out int totalcount)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    List<B2b_com_roomtype> model = new InternalB2b_com_roomtype(sql).GetRoomTypePageList(pageindex,pagesize, comid,out totalcount);
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

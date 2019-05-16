using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public  class B2b_com_housetypeData
    {
        public int InsertOrUpdate(B2b_com_housetype model)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2b_com_housetype(sql);
                    int result = internalData.InsertOrUpdate(model);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public B2b_com_housetype GetHouseType(int proid, int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    B2b_com_housetype model = new InternalB2b_com_housetype(sql).GetB2b_com_housetype(proid, comid);
                    return model;
                }
                catch
                {
                    throw;
                }
            }
        }






        public decimal GetHousetypeNowdayprice(int proid,int bangdingproid)
        {
            //如果是绑定产品，查询绑定的日历
            if (bangdingproid != 0) {
                proid = bangdingproid;
            }



            using (var helper=new SqlHelper())
            {
                decimal nowprice = new InternalB2b_com_housetype(helper).GetHousetypeNowdayprice(proid);
                return nowprice;
            }
        }


        public decimal GetHousetypeNowdaypricebyprojectid(int projectid)
        {
        

            using (var helper = new SqlHelper())
            {
                decimal nowprice = new InternalB2b_com_housetype(helper).GetHousetypeNowdaypricebyprojectid(projectid);
                return nowprice;
            }
        }
    }
}

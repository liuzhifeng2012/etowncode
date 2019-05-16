using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class Travelbusorder_operlogData
    {
        public int Edittravelbusorder_operlog(int operlogid, int proid, string proname, string gooutdate, string operremark, int bustotal, string busids, string travelbus_model, string seatnum, string licenceplate, string drivername, string driverphone, int userid, int comid, string issavebus)
        {
            using(var helper=new SqlHelper())
            {
                int r = new Internaltravelbusorder_operlog(helper).Edittravelbusorder_operlog(operlogid, proid, proname, gooutdate, operremark, bustotal, busids, travelbus_model, seatnum, licenceplate, drivername, driverphone, userid, comid, issavebus);
                return r;
            }
        }

        public int Ishasplanbus(int proid, DateTime daydate)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internaltravelbusorder_operlog(helper).Ishasplanbus(  proid,daydate);
                return r;
            }
        }
    }
}

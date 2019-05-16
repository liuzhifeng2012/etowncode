using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data
{
    public class ApiLogData
    {
        public int EditLog(int id, int serviceid, string initxml, DateTime subdate, string returnstr, DateTime returndate, string errmsg   )
        {
            using (var sql = new SqlHelper())
            {
                return new InternalApiLog(sql).EditLog(id, serviceid, initxml, subdate, returnstr, returndate, errmsg);
            }
        }



        public int EditLog(ApiLog m)
        {
            using (var sql = new SqlHelper())
            {
                return new InternalApiLog(sql).EditLog(m);
            }
        }
    }
}

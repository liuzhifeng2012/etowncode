using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class Api_mjld_AsyncBacknoticeData
    {
        public int EditBacknotice(Api_mjld_AsyncBacknotice  notice)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internalapi_mjld_AsyncBacknotice(helper).EditBacknotice( notice);
                return r;
            }
        }

        public Api_mjld_AsyncBacknotice GetSucApi_mjld_AsyncBacknotice(string mjldorderid)
        {
            using (var helper = new SqlHelper())
            {
                Api_mjld_AsyncBacknotice r = new Internalapi_mjld_AsyncBacknotice(helper).GetSucApi_mjld_AsyncBacknotice(mjldorderid);
                return r;
            }
        }
    }
}

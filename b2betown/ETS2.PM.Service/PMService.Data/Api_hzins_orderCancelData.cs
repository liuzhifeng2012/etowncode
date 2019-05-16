using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class Api_hzins_orderCancelData
    {
        public int EditApi_hzins_orderCancel(Api_hzins_orderCancel mApi_hzins_orderCancel)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internalapi_hzins_orderCancel(helper).EditApi_hzins_orderCancel(mApi_hzins_orderCancel);
                return r;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class Api_mjld_RefundByOrderIDData
    {
        public int Editmjldrefundlog(Api_mjld_RefundByOrderID mrefund)
        {
             using(var helper=new SqlHelper())
             {
                 int r = new Internalapi_mjld_RefundByOrderID(helper).Editmjldrefundlog(mrefund);
                 return r;
             }
        }
    }
}

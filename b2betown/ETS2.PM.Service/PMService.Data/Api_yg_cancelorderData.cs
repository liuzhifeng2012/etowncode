using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class Api_yg_cancelorderData
    {
        public int EditApi_yg_cancelorder(Api_yg_cancelorder m)
        {
             using(var helper=new SqlHelper())
             {
                 int r = new Internalapi_yg_cancelorder(helper).EditApi_yg_cancelorder(m);
                 return r;
             }
        }
    }
}

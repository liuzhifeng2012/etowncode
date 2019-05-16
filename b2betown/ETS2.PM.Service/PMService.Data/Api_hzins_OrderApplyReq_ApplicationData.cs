using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class Api_hzins_OrderApplyReq_ApplicationData
    {
        public int EditOrderApplyReq_Application(Api_hzins_OrderApplyReq_Application m)
        {
            using(var helper=new SqlHelper())
            {
                int r = new Internalapi_hzins_OrderApplyReq_Application(helper).EditOrderApplyReq_Application(m);
                return r;
            }
        }

        public Api_hzins_OrderApplyReq_Application GetOrderApplyReq_Application(int orderid)
        {
             using(var helper=new SqlHelper())
             {
                 Api_hzins_OrderApplyReq_Application r = new Internalapi_hzins_OrderApplyReq_Application(helper).GetOrderApplyReq_Application(orderid);
                 return r;
             }
        }

      
    }
}

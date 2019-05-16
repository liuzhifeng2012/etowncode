using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class Api_hzins_OrderApplyReq_insurantInfoData
    {
        public int EditOrderApplyReq_insurantInfo(Api_hzins_OrderApplyReq_insurantInfo m)
        {
            using(var helper=new SqlHelper())
            {
                int r = new Internalapi_hzins_OrderApplyReq_insurantInfo(helper).EditOrderApplyReq_insurantInfo(m);
                return r;
            }
        }

        public List<Api_hzins_OrderApplyReq_insurantInfo> GetOrderApplyReq_insurantInfo(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                List<Api_hzins_OrderApplyReq_insurantInfo> r = new Internalapi_hzins_OrderApplyReq_insurantInfo(helper).GetOrderApplyReq_insurantInfo(orderid);
                return r;
            }
        }
    }
}

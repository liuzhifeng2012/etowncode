using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class Api_hzins_OrderApplyReq_applicantInfoData
    {
        public int EditOrderApplyReq_applicantInfo(Api_hzins_OrderApplyReq_applicantInfo m)
        {
             using(var helper=new SqlHelper())
             {
                 int r = new Internalapi_hzins_OrderApplyReq_applicantInfo(helper).EditOrderApplyReq_applicantInfo(m);
                 return r;
             }
        }

        public Api_hzins_OrderApplyReq_applicantInfo GetOrderApplyReq_applicantInfo(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                Api_hzins_OrderApplyReq_applicantInfo r = new Internalapi_hzins_OrderApplyReq_applicantInfo(helper).GetOrderApplyReq_applicantInfo(orderid);
                return r;
            }
        }
    }
}

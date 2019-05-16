using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class Api_hzins_OrderApplyResp_OrderInfoData
    {
        public int EditOrderApplyResp_OrderInfo(Api_hzins_OrderApplyResp_OrderInfo m)
        {
            using(var helper=new SqlHelper())
            {
                int r = new Internalapi_hzins_OrderApplyResp_OrderInfo(helper).EditOrderApplyResp_OrderInfo(m);
                return r;
            }
        }
    }
}

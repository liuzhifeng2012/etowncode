using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class Api_hzins_OrderApplyResp_OrderExtData
    {
        public int EditOrderApplyResp_OrderExt(Api_hzins_OrderApplyResp_OrderExt m1)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internalapi_hzins_OrderApplyResp_OrderExt(helper).EditOrderApplyResp_OrderExt(m1);
                return r;
            }
        }
        public int GetorderidbyinsureNum(string insureNum)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internalapi_hzins_OrderApplyResp_OrderExt(helper).GetorderidbyinsureNum(insureNum);
                return r;
            }
        }

        public string GetinsureNumbyorderid(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                int bindorderid = new B2bOrderData().GetBindOrderIdByOrderid(orderid);
                if (bindorderid > 0)
                {
                    orderid = bindorderid;
                }

                string r = new Internalapi_hzins_OrderApplyResp_OrderExt(helper).GetinsureNumbyorderid(orderid);
                return r;
            }
        }

        public List<Api_hzins_OrderApplyResp_OrderExt> GetinsureNumsbyorderids(string orderidstr)
        {
            using (var helper = new SqlHelper())
            {
                List<Api_hzins_OrderApplyResp_OrderExt> r = new Internalapi_hzins_OrderApplyResp_OrderExt(helper).GetinsureNumsbyorderids(orderidstr);
                return r;
            }
        }
    }
}

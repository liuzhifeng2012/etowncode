using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;
using ETS.Data.SqlHelper;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
    public class WxOperationTypeData
    {
        public WxOperationType GetOprationType(int typeid)
        {
            using (var helper = new SqlHelper())
            {
                var id = new InternalWxOperationType(helper).GetOprationType(typeid);
                return id;
            }
        }
    }
}

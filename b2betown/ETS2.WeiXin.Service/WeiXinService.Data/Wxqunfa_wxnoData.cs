using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
    public class Wxqunfa_wxnoData
    {
        public int InsWxno(int qunfalogid, string weixin)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalWxqunfa_wxno(helper).InsWxno(qunfalogid, weixin);
                return result;
            }
        }
    }
}

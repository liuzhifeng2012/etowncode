using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.VAS.Service.VASService.Modle;
using ETS.Data.SqlHelper;
using ETS2.VAS.Service.VASService.Data.InternalData;

namespace ETS2.VAS.Service.VASService.Data
{
    public class MemberIntegralData
    {

        #region 插入支付记录
        public int InsertOrUpdate(Member_Integral intinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberIntegral(sql);
                    int result = internalData.InsertOrUpdate(intinfo);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 查询预付款日志
        public  List<Member_Integral> ReadIntegral(int id, int comid, out int totalcount)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalMemberIntegral(sql);
                    var result = internalData.ReadIntegral(id, comid, out  totalcount);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion
    }
}

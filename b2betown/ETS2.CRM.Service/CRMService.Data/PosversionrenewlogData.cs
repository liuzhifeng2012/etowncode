using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Data.InternalData;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class PosversionrenewlogData
    {
        /// <summary>
        /// 添加或者编辑公司基本信息 By:Xiaoliu
        /// </summary>
        /// <param name="model">商家 实体</param>
        /// <returns>标识列</returns>
        public int InsertOrUpdate(Posversionrenewlog model)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalPosversionrenewlog(sql);
                    int result = internalData.InsertOrUpdate(model);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2b_com_pro_pricelogData
    {
        /// <summary>
        /// 记录产品价格变动日志
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public int EditPriceLog(int proid, B2b_com_pro product)
        {
            using (var helper = new SqlHelper())
            {

                var result = new InternalB2b_com_pro_pricelog(helper).EditPriceLog(proid, product);

                return result;
            }
        }
    }
}

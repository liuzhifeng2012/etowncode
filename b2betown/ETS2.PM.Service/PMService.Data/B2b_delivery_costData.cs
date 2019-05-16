using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data.InternalData;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2b_delivery_costData
    {
        public IList<B2b_delivery_cost> GetB2b_delivery_costlist(int tmpid,out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_delivery_cost> r = new InternalB2b_delivery_cost(helper).GetB2b_delivery_costlist(tmpid,out totalcount);
                return r;
            }
        }
        /// <summary>
        /// 得到运费
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="city"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public decimal Getdeliverycost(int proid, string city, int num, out string errmsg, int deliverytype = 2)
        {
            using (var helper = new SqlHelper())
            {
                decimal r = new InternalB2b_delivery_cost(helper).Getdeliverycost(proid, city, num, out errmsg, deliverytype);
                return r;
            }
        }
        /// <summary>
        ///  购物车运费计算:正确返回价格连接字符串，错误返回 “-1，错误原因”
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="city"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public decimal Getdeliverycost_ShopCart(string proidstr, string citystr, string numstr,out string feedetail)
        {
            using (var helper = new SqlHelper())
            {
                decimal r = new InternalB2b_delivery_cost(helper).Getdeliverycost_ShopCart( proidstr,   citystr,   numstr,out feedetail);
                return r;
            }
        }
    }
}

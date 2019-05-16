using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Qunar_Ms.Data.Internal;

namespace ETS2.PM.Service.Qunar_Ms.Data
{
    public class Qunar_GetOrderByQunarData
    {
        /// <summary>
        /// 录入请求数据，返回受影响行数
        /// </summary>
        /// <param name="partnerOrderId"></param>
        /// <returns></returns>
        public int InsQunar_GetOrderByQunar_request(string partnerOrderId)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalQunar_GetOrderByQunar(helper).InsQunar_GetOrderByQunar_request(partnerOrderId);
                return r;
            }
        }
        /// <summary>
        /// 录入返回数据，返回受影响行数
        /// </summary>
        /// <param name="partnerOrderId"></param>
        /// <param name="orderstatus"></param>
        /// <param name="orderQuantity"></param>
        /// <param name="eticketNo"></param>
        /// <param name="eticketSended"></param>
        /// <param name="total_consumenum"></param>
        /// <param name="consumeInfo"></param>
        /// <param name="responseParam"></param>
        /// <returns></returns>
        public int InsQunar_GetOrderByQunar_response(string partnerOrderId, string orderstatus, string orderQuantity, string eticketNo, string eticketSended, int total_consumenum, string consumeInfo, string responseParam)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalQunar_GetOrderByQunar(helper).InsQunar_GetOrderByQunar_response(partnerOrderId, orderstatus, orderQuantity, eticketNo, eticketSended, total_consumenum, consumeInfo, responseParam);
                return r;
            }
        }


      
    }
}

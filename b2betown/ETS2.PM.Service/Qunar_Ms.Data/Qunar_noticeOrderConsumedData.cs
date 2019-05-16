using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Qunar_Ms.Data.Internal;

namespace ETS2.PM.Service.Qunar_Ms.Data
{
    public class Qunar_noticeOrderConsumedData
    {
        /// <summary>
        /// 录入请求数据，返回id
        /// </summary>
        /// <param name="partnerorderId"></param>
        /// <param name="orderQuantity"></param>
        /// <param name="useQuantity"></param>
        /// <param name="consumeInfo"></param>
        /// <returns></returns>
        public int InsRequest(string partnerorderId, string orderQuantity, string useQuantity, string consumeInfo)
        {
             using(var helper=new SqlHelper())
             {
                 int id = new InternalQunar_noticeOrderConsumed(helper).InsRequest(partnerorderId,orderQuantity,useQuantity,consumeInfo);
                 return id;
             }
        }
        /// <summary>
        /// 录入返回数据，返回受影响行数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <param name="qunar_requestid"></param>
        /// <returns></returns>
        public int InsResponse(string message, string data, int qunar_requestid)
        {
            using (var helper = new SqlHelper())
            {
                int id = new InternalQunar_noticeOrderConsumed(helper).InsResponse(message, data, qunar_requestid);
                return id;
            }
        }
    }
}

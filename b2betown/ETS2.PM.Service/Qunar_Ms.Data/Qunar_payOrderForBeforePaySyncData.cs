using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Qunar_Ms.Data.Internal;

namespace ETS2.PM.Service.Qunar_Ms.Data
{
    public class Qunar_payOrderForBeforePaySyncData
    {
        public bool Ishasrequest(string partnerOrderId)
        {
             using(var helper=new SqlHelper())
             {
                 bool r = new InternalpayOrderForBeforePaySync(helper).Ishasrequest(partnerOrderId);
                 return r;
             }
        }
        /// <summary>
        /// 录入请求数据 返回影响行数
        /// </summary>
        /// <param name="partnerOrderId"></param>
        /// <param name="orderStatus"></param>
        /// <param name="orderPrice"></param>
        /// <param name="paymentSerialno"></param>
        /// <param name="eticketNo"></param>
        /// <returns></returns>
        public int InsQunar_payOrderForBeforePaySync_request(string partnerOrderId, string orderStatus, string orderPrice, string paymentSerialno, string eticketNo)
        {
            using(var helper=new SqlHelper())
            {
                int r = new InternalpayOrderForBeforePaySync(helper).InsQunar_payOrderForBeforePaySync_request(partnerOrderId,orderStatus,orderPrice,paymentSerialno,eticketNo);
                return r;
            }
        }
        /// <summary>
        /// 录入返回数据 返回影响行数
        /// </summary>
        /// <param name="partnerOrderId"></param>
        /// <param name="orderStatus"></param>
        /// <param name="eticketNo"></param>
        /// <param name="responseParam"></param>
        /// <returns></returns>
        public int InsQunar_payOrderForBeforePaySync_response(string partnerOrderId, string orderStatus, string eticketNo, string responseParam)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalpayOrderForBeforePaySync(helper).InsQunar_payOrderForBeforePaySync_response(partnerOrderId, orderStatus,   eticketNo,responseParam);
                return r;
            }
             
        }
    }
}

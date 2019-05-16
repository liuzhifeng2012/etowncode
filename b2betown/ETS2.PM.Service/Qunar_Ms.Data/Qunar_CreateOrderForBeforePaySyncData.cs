using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.Qunar_Ms.QMRequestDataSchema;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Qunar_Ms.Data.Internal;

namespace ETS2.PM.Service.Qunar_Ms.Data
{
    public class Qunar_CreateOrderForBeforePaySyncData
    {
        /// <summary>
        /// 支付前下单通知信息记录
        /// </summary>
        /// <param name="createOrderForBeforePaySyncRequestBody"></param>

        /// <param name="parterorderid"></param>
        /// <param name="orderStatus"></param>
        /// <param name="eticketNo"></param>
        /// <returns></returns>
        public int InsQunar_CreateOrderForBeforePaySync(CreateOrderForBeforePaySyncRequestBody createOrderForBeforePaySyncRequestBody)
        {
            using (var helper = new SqlHelper())
            {
                int id = new InternalQunar_CreateOrderForBeforePaySync(helper).InsQunar_CreateOrderForBeforePaySync(createOrderForBeforePaySyncRequestBody);
                return id;
            }
        }
        /// <summary>
        /// 支付前提单 向去哪发送请求
        /// </summary>
        /// <param name="qunar_orderId"></param>
        /// <param name="parterorderid"></param>
        /// <param name="orderStatus"></param>
        /// <param name="eticketNo"></param>
        public int  InsQunar_CreateOrderForBeforePaySync_ret(string qunar_orderId, int parterorderid, string orderStatus, string eticketNo, string qunar_ret)
        {
            using (var helper = new SqlHelper())
            {
                return new InternalQunar_CreateOrderForBeforePaySync(helper).InsQunar_CreateOrderForBeforePaySync_ret(qunar_orderId, parterorderid, orderStatus, eticketNo, qunar_ret);

            }
        }

        public bool Ishasrequest(string qunar_orderId)
        {
            using (var helper = new SqlHelper())
            {
                bool r = new InternalQunar_CreateOrderForBeforePaySync(helper).Ishasrequest(qunar_orderId);
                return r;
            }
        }



        public string GetQunarOrderId(string partnerOrderId)
        {
            using (var helper = new SqlHelper())
            {
                string r = new InternalQunar_CreateOrderForBeforePaySync(helper).GetQunarOrderId(partnerOrderId);
                return r;
            }
        }
    }
}

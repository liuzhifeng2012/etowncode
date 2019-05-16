using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Qunar_Ms.Data.Internal;

namespace ETS2.PM.Service.Qunar_Ms.Data
{
    public class Qunar_noticeOrderAlreadyCashBackData
    {
        public bool Ishasrequest(string partnerorderId)
        {
             using(var helper=new SqlHelper())
             {
                bool r=new InternalnoticeOrderAlreadyCashBack(helper).Ishasrequest(partnerorderId);
                return r;
             }
        }
        /// <summary>
        /// 录入请求信息，返回受影响行数
        /// </summary>
        /// <param name="partnerorderId"></param>
        /// <param name="orderCashBackMoney"></param>
        /// <returns></returns>
        public int Insrequest(string partnerorderId, string orderCashBackMoney)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalnoticeOrderAlreadyCashBack(helper).Insrequest(partnerorderId,orderCashBackMoney);
                return r;
            }
        }

        public int InsResponse(string partnerorderId, string message, string responseparam)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalnoticeOrderAlreadyCashBack(helper).InsResponse(partnerorderId, message,responseparam);
                return r;
            }
        }
    }
}

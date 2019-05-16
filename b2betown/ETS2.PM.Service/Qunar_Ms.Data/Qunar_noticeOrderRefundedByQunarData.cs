using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Qunar_Ms.Data.Internal;

namespace ETS2.PM.Service.Qunar_Ms.Data
{
    public class Qunar_noticeOrderRefundedByQunarData
    {
        public bool Ishasrequest(string partnerorderId)
        {
            using (var helper = new SqlHelper())
            {
                bool r = new InternalnoticeOrderRefundedByQunar(helper).Ishasrequest(partnerorderId);
                return r;
            }
        }

        public int Insrequest(string partnerorderId, string refundSeq, string orderQuantity, string refundQuantity, string orderPrice, string refundReason, string refundExplain, string refundJudgeMark, string orderRefundCharge)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalnoticeOrderRefundedByQunar(helper).Insrequest(partnerorderId,refundSeq,orderQuantity,refundQuantity,orderPrice,refundReason,refundExplain,refundJudgeMark,orderRefundCharge);
                return r;
            }
        }

        public int Insresponse(string partnerorderId,string message, string responseParam)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalnoticeOrderRefundedByQunar(helper).Insresponse(partnerorderId,message,responseParam);
                return r;
            }
        }
    }
}

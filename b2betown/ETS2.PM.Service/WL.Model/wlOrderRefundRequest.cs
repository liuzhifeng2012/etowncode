using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.WL.Model
{
    public class wlOrderRefundRequest
    {
        public string partnerId { get; set; }

        public wlOrderRefundRequestBody body { get; set; }
    }

    public class wlOrderRefundRequestBody
    {
        public wlOrderRefundRequestBody() { }
        public string wlOrderId { get; set; }
        public string wlDealId { get; set; }
        public string partnerRefundId { get; set; }
        public string partnerOrderId { get; set; }
        public string partnerDealId { get; set; }
        public string[] voucherList { get; set; }

        public int refundQuantity { get; set; }

        public double unitPrice { get; set; }
        public double refundPrice { get; set; }
        public double refundFee { get; set; }

        public string refundTime { get; set; }

        


    }
}

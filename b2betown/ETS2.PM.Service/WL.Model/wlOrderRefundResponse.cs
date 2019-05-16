using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.WL.Model
{
    public class wlOrderRefundResponse
    {
        public int code { get; set; }
        public string describe { get; set; }
        public int partnerId { get; set; }

        public wlOrderRefundResponseBody body { get; set; }
    }

    public class wlOrderRefundResponseBody
    {
        public wlOrderRefundResponseBody() { }
        public string wlOrderId { get; set; }
        public string refundId { get; set; }
        public string partnerOrderId { get; set; }
        public string requestTime { get; set; }
        public string responeTime { get; set; }


    }
}

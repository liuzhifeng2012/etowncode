using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.WL.Model
{
    public class wlOrderPayResponse
    {
        public int code { get; set; }
        public string describe { get; set; }
        public string partnerId { get; set; }

        public wlOrderPayResponseBody body { get; set; }
    }

    public class wlOrderPayResponseBody
    {
        public wlOrderPayResponseBody() { }
        public string wlOrderId { get; set; }
        public string partnerOrderId { get; set; }
        public int voucherType { get; set; }
        public string[] vouchers { get; set; }
        public string[] voucherPics { get; set; }
        
    }
}

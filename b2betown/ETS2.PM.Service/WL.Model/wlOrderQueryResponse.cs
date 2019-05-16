using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.WL.Model
{
    public class wlOrderQueryResponse
    {
        public string partnerId { get; set; }
        public int code { get; set; }
        public string describe { get; set; }

        public wlOrderQueryResponseBody body { get; set; }
    }

    public class wlOrderQueryResponseBody
    {
        public wlOrderQueryResponseBody() { }
        public string partnerOrderId { get; set; }
        public int orderStatus { get; set; }
        public int orderQuantity { get; set; }
        public int usedQuantity { get; set; }
        public int refundedQuantity { get; set; }
        public int voucherType { get; set; }
        public List<wlVoucher> voucherList { get; set; }

    }

    public class wlVoucher
    {
        public wlVoucher() { }
        public string voucher { get; set; }
        public int quantity { get; set; }
        public int status { get; set; }
        public string voucherPics { get; set; }
        public string voucherInvalidTime { get; set; }

    }
}

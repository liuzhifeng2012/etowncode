using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    //已经弃用
    public class VoucherRequest
    {
        public VoucherRequest() { }
        public string partnerId { get; set; }
        public VoucherRequestBody body { get; set; }
    }
    public class VoucherRequestBody
    {
        public VoucherRequestBody() { }
        public string bookOrderId { get; set; }
        public string partnerOrderId { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    //已经弃用
    public class VoucherResponse
    {
        public VoucherResponse() { }
        public int code { get; set; }
        public string describe { get; set; }
        public int partnerId { get; set; }
        public VoucherResponseBody body { get; set; }
    }
    public class VoucherResponseBody 
    {
        public VoucherResponseBody() { }
        public int voucherType { get; set; }
        public string voucher { get; set; }
    }
}

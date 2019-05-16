using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.Meituan.Model;

namespace ETS2.PM.Service.Meituan.Data
{
    public class OrderConsumeNotice
    {
        public OrderConsumeNotice() { }

        public int partnerId { get; set; }
        public OrderConsumeNoticeBody body { get; set; }
    }
    public class OrderConsumeNoticeBody
    {
        public OrderConsumeNoticeBody() { }

        public Int64 orderId { get; set; }
        public string partnerOrderId { get; set; }
        public int quantity { get; set; }
        public int usedQuantity { get; set; }
        public int refundedQuantity { get; set; }
        public List<Voucher> voucherList { get; set; }
    }
}

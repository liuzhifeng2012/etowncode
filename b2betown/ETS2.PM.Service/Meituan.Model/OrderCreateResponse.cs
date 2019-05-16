using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    public class OrderCreateResponse
    {
        public OrderCreateResponse() { }
        public int code { get; set; }
        public string describe { get; set; }
        public int partnerId { get; set; }
        public OrderCreateResponseBody body { get; set; }

    }
    public class OrderCreateResponseBody
    {
        public OrderCreateResponseBody() { }
        public string partnerOrderId { get; set; } 
    }
}

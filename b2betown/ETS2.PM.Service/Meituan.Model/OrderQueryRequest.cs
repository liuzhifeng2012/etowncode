using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    public class OrderQueryRequest
    {
        public OrderQueryRequest(){}
        public string partnerId{get;set;}
        public OrderQueryRequestBody body{get;set;}
    }
    public class OrderQueryRequestBody 
    {
        public OrderQueryRequestBody() { }
        public Int64 orderId { get; set; }
        public string partnerOrderId { get; set; } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    public class OrderPayRequest
    {
        public OrderPayRequest() { }
        public string partnerId { get; set; }
        public OrderPayRequestBody body { get; set; }
    }
    public class OrderPayRequestBody
    {
      public OrderPayRequestBody(){}
      //required	旅游订单号	unique
      public Int64 orderId { get; set; }
      //optional	合作方订单ID	unique
      public string partnerOrderId { get; set; }
     
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    public class OrderCancelResponse
    {
        public OrderCancelResponse() { }
        public int code { get; set; }
        public string describe { get; set; }
        public int partnerId { get; set; }
        public OrderCancelResponseBody body { get; set; }

    }
    public class OrderCancelResponseBody
    {
        public OrderCancelResponseBody() { }
        //required	美团订单ID	unique
        public Int64 orderId { get; set; }
        //required	美团退款流水ID	支持部分退款时，商家必须存储和返回美团refundid"
        public string refundId { get; set; }
        //required	合作方订单ID	unique
        public string partnerOrderId { get; set; }
        //required	商家接收到退款申请的时间	格式 yyyy-MM-dd HH:mm:ss
        public string requestTime { get; set; }
        //required	商家处理退款申请的时间	格式 yyyy-MM-dd HH:mm:ss
        public string responseTime { get; set; } 

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    public class OrderCancelRequest
    {
        public OrderCancelRequest() { }
        public string partnerId { get; set; }
        public OrderCancelRequestBody body { get; set; }
    }
    public class OrderCancelRequestBody
    {
        public OrderCancelRequestBody() { }
        //required	美团订单ID	unique
        public Int64 orderId { get; set; }
        //required	美团退款流水ID	unique,退款流水号标记唯一退款申请,幂等
        public string refundId { get; set; }
        //required	合作方订单ID	unique
        public string partnerOrderId { get; set; }
        //required	合作方产品ID	unique
        public string partnerDealId { get; set; }
        //required	要退的票对应的凭证码列表	如果是一码一验_不绑定身份，美团会带上退款票所对应的验证码数组，如果是一码多验，美团会带上整体的验证码
        public string[] voucherList { get; set; }
        //required	退款票数	请一定要注意退款票数
        public int refundQuantity { get; set; }
        //required	美团售卖单价	用户在美团下单的单价
        public double unitPrice { get; set; }
        //required	美团需退还用户总金额	美团根据退款规则计算出的需退还用户的金额
        public double refundPrice { get; set; }
        //required	本次退款合作方收取的手续费	美团根据退款规则计算出的合作方收取的手续费
        public double refundFee { get; set; }
        //required	用户第一次发起退款申请的时间	用户第一次申请退款时间,阶梯退款时以这个时间计算为准，时间格式yyyy-MM-dd hh:mm:ss
        public string refundTime { get; set; }
    }
}

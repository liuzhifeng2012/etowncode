using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    public class OrderQueryResponse
    {
        public  OrderQueryResponse() { }
        public int code { get; set; }
        public string describe { get; set; }
        public int partnerId { get; set; }
        public OrderQueryResponseBody body { get; set; }
    }
    public class OrderQueryResponseBody
    {
        public OrderQueryResponseBody() { }
        //required	我方预约订单id
        public Int64 orderId { get; set; }
        //required	合作方订单ID	unique
        public string partnerOrderId { get; set; }
        /*******************************
         * required	订单状态	取值请参考订单状态映射表
         *  2	创建订单成功
            3	创建订单失败
            4	支付成功
            5	支付中 不能明确支付状态时使用该状态
            6	支付失败
         * ******************************/
        public int orderStatus { get; set; }
        //required	订单总票数
        public int orderQuantity { get; set; }
        //required	订单已经使用的总票数
        public int usedQuantity { get; set; }
        //required	订单已经退款的总票数	
        public int refundedQuantity { get; set; }
        /**************************
         * required	凭证类型	见映射表<凭证码类型>
         * 1	商家一码一验码_不绑定身份（一码一验：一个券一个码）
           2	商家一码多验码（一码多验，一个订单一个码，码只能验一次或码可以验多次）
         * *************************/
        public int voucherType { get; set; }
        //optional 消费凭证信息	如果有凭证信息，则必需传入所有的凭证
        public List<Voucher> voucherList { get; set; }
    }
    public class Voucher 
    {
        //required	凭证码
        public string voucher { get; set; }
        //optional	二维码凭证
        public string voucherPics { get; set; }
        //required	凭证被消费或者被退款或者凭证最初生效时间	格式:YYYY-MM-DD HH:MM:SS 与status配合使用，status是退款则是退款时间，status是消费则是消费时间，status是未使用则是凭证生效时间
        public string voucherInvalidTime { get; set; }
        //required	与status配合使用，此状态下的凭证代表的数量
        public int quantity { get; set; }
        /************************
         * required	凭证状态	见映射表<凭证码状态>
         *  0	未使用
            1	已使用
            2	已退款
            3	已废弃 对应的门票还未消费，但是此凭证码作废了
         * ************************/
        public int status { get; set; }
    }
}

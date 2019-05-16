using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    /// <summary>
    /// 美团已退款消息(基本不会出现这种情况，防范美团不经退款审核退款而造成用户票已经退但是仍然可以用的情况)
    /// 美团对如下三种退款不会走商家退款审核：美团客服和商家核实可以退款的客服操作退款、商家审核退款申请超过三个工作日美团自动退款、商家支付结果未知美团多次通过订单查询也没查询到支付成功时退款。
    /// 对于上述三种退款，美团会通过该接口告知商家系统商，也会通过邮件形式告知商家人员。请系统商及时撤销支付凭证避免用户既得到了退款又能入园，因此建议商家对接该接口。
    /// 支付结果商家明确返回“支付失败”的这种情况，不会通过该接口告知商家。
    /// 三种退款通知type对应在“已退款消息类型”映射表中。
    /// 1	用户未消费美团客服操作退款
    /// 2	支付状态未知，未查询到支付成功，美团自动退款
    /// 3	商家退款审核超时，美团自动退款
    /// </summary> 
    public class MtpOrderRefundedMessageRequest
    {
        public int partnerId { get; set; }
        public MtpOrderRefundedMessageRequestBody body { get; set; }
    }
    public class MtpOrderRefundedMessageRequestBody 
    {
        //required	美团订单id
        public Int64 orderId { get; set; }
        //optional	商家订单id
        public string partnerOrderId { get; set; }
        //optional	退款流水号
        public string refundSerialNo { get; set; }
        //optional	凭证码
        public string[] voucherList { get; set; }
        /***************************
         * optional	凭证类型	详情见<凭证码类型>映射表
         * 1	商家一码一验码_不绑定身份（一码一验：一个券一个码）
           2	商家一码多验码（一码多验，一个订单一个码，码只能验一次或码可以验多次）
         * *************************/
        public int voucherType { get; set; }
        //required	单张门票退款金额	-1表示全额退款
        public int refundPrice { get; set; }
        /**************************
         * required	已退款消息类型	详情见<已退款消息类型>映射表
         *  1	用户未消费美团客服操作退款
            2	支付状态未知，未查询到支付成功，美团自动退款
            3	商家退款审核超时，美团自动退款
         * ************************/
        public int refundMessageType { get; set; }
        //optional	商家客服处理人
        public string operator1	{get;set;}
        //required	退款原因
        public string reason { get; set; }
        //required	退款时间
        public string refundTime { get; set; }
        //required	退款份数
        public int count { get; set; }
    }
}

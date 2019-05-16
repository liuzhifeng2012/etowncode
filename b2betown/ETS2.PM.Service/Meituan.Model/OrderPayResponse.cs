using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    public class OrderPayResponse
    {
        public OrderPayResponse() { }
        public int code { get; set; }
        public string describe { get; set; }
        public int partnerId { get; set; }
        public OrderPayResponseBody body { get; set; }
        
    }
    public class OrderPayResponseBody 
    {
        public OrderPayResponseBody() { }
        //required	旅游订单号 unique
        public Int64 orderId { get; set; }
        //required	合作方订单ID	unique
        public string partnerOrderId { get; set; }
        //required	电子票类型	凭证码类型
        public int voucherType { get; set; }
        //optional	是否异步回传电子票	false:否，true:是
        public bool asyReturnVoucher { get; set; }
        //optional	电子票数组	每个凭证码的最大长度为 varchar(20)
        public string[] vouchers { get; set; }
        //optional	图片电子票数组	传图片链接地址, 有多个时,顺序必须与vouchers的顺序一致
        public string[] voucherPics { get; set; }
    }
}

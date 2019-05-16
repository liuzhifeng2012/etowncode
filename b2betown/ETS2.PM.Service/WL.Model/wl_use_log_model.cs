using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.WL.Model
{
    public class wl_use_log_model
    {
        public int id { get; set; }
        public int comid { get; set; }
        public int orderid { get; set; }
        public int partnerdealid { get; set; }
        public string partnerId { get; set; }
        public DateTime usetime { get; set; }

        public string name { get; set; }
        public string mobile { get; set; }
        public string wldealid { get; set; }
        public double buyprice { get; set; }
        public double unitprice { get; set; }
        public double totalprice { get; set; }
        public int quantity { get; set; }
        public int usedQuantity { get; set; }
        public int refundedQuantity { get; set; }

        public string partnerOrderId { get; set; }
        public string traveldate { get; set; }
        public string wlorderid { get; set; }
        public int code { get; set; }
        public string describe { get; set; }
        public DateTime subtime { get; set; }
        public int pay_code { get; set; }
        public string pay_describe { get; set; }
        public int voucherType { get; set; }
        public string vouchers { get; set; }
        public string voucherPics { get; set; }

        public int status { get; set; }
        public string partnerRefundId { get; set; }
        public string refundId { get; set; }
        public string requestTime { get; set; }
        public string responeTime { get; set; }
    }
}

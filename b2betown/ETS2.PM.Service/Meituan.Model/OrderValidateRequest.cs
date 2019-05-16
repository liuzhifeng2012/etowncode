using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    #region 下单前预约 Request 已经弃用
    public class OrderValidateRequest
    {
        public int partnerId { get; set; }
        public OrderValidateRequestBody body { get; set; }
    }
    public class OrderValidateRequestBody
    {
        public string bookOrderId { get; set; }
        public string partnerDealId { get; set; }
        public decimal sellPrice { get; set; }
        public int quantity { get; set; }
        public string visitDate { get; set; }
        public VisitorInfo firstVisitor { get; set; }
        public List<VisitorInfo> otherVisitor { get; set; }
    }
    public class VisitorInfo
    {
        public string name { get; set; }
        public string pinyin { get; set; }
        public string mobile { get; set; }
        public string address { get; set; }
        public string postCode { get; set; }
        public string email { get; set; }
        public string credentials { get; set; }
        public int credentialsType { get; set; }

    }
    #endregion
   
}

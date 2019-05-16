using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    public class DealRequest
    {
        public DealRequest() { }
        public string partnerId { get; set; }
        public DealRequestBody body { get; set; }
    }
    public class DealRequestBody
    {
        public DealRequestBody() { }
        public string method { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public List<string> partnerDealIds { get; set; } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    public class PriceRequest
    {
        public PriceRequest() { }
        public string partnerId { get; set; }
        public PriceRequestBody body { get; set; }
    }
    public class PriceRequestBody 
    {
        public PriceRequestBody() { }
        public string partnerDealId { get; set; } 
        public string startTime { get; set; }
        public string endTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    public class PriceResponse
    {
        public PriceResponse() { }
        public int code { get; set; }
        public string describe { get; set; }
        public int partnerId { get; set; }  
        public List<PriceResponseBody> body { get; set; }
    }
    public class PriceResponseBody
    {
        public PriceResponseBody() { }
        public string partnerDealId { get; set; }
        public string date { get; set; } 
        public decimal marketPrice { get; set; }
        public decimal mtPrice { get; set; }
        public decimal settlementPrice { get; set; }
        public int stock { get; set; } 
    }
}

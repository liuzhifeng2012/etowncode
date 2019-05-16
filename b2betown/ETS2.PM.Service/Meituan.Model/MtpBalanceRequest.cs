using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    public class MtpBalanceRequest
    {
        public int partnerId { get; set; }
        public MtpBalanceRequestBody body { get; set; }
    }
    public class MtpBalanceRequestBody 
    {
        public string[] partnerDealIds { get; set; }
    }
}

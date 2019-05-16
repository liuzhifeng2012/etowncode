using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.LMM.Model
{
    public class asynnoticecallback
    {
        public string uid { get; set; }
        public string password { get; set; }
        public string timestamp { get; set; }
        public string visitTime { get; set; }

        public string supplierGoodsId { get; set; }
        public string settlePrice { get; set; }

        public string num { get; set; }
        public string serialNo { get; set; }

        public string sign { get; set; }

        public string status { get; set; }
        public string msg { get; set; }

        public string authCode { get; set; }
        public string codeURL { get; set; }
        public string orderId { get; set; }
    }
}

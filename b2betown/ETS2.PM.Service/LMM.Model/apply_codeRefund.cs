using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.LMM.Model
{
    public class apply_codeRefund
    {
        public int id { get; set; }
        public string status { get; set; }
        public string msg { get; set; }

        public string serialNo { get; set; }

        public string authCode { get; set; }
        public string codeURL { get; set; }
        public string orderId { get; set; }
        public int agentcomid { get; set; }
        public string pno { get; set; }
        public string uid { get; set; }

    }
}

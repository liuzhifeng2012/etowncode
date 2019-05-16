using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class Api_mjld_RefundByOrderID
    {
        public Api_mjld_RefundByOrderID() { }
        public int id { get; set; }
        public string timeStamp { get; set; }
        public string user { get; set; }
        public string password { get; set; }
        public string RefundPart { get; set; }
        public string mjldorderId { get; set; }
        public string rtimeStamp { get; set; }
        public int status { get; set; }
        public string scredenceno { get; set; }
        public string fcredenceno { get; set; }
        public int backCount { get; set; }
        public int orderid { get; set; }

    }
}

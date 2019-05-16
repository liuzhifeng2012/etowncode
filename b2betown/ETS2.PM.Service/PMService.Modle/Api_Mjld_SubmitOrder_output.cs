using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class Api_Mjld_SubmitOrder_output
    {
        public Api_Mjld_SubmitOrder_output() { }
        public int id { get; set; }
        public string timeStamp { get; set; }
        public string mjldOrderId { get; set; }
        public string endTime { get; set; }
        public string credence { get; set; }
        public string inCount { get; set; }
        public int status { get; set; }
        public int orderId { get; set; } 
    }
}

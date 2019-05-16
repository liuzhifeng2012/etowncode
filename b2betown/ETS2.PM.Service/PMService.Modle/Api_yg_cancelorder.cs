using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class Api_yg_cancelorder
    {
        public Api_yg_cancelorder() { }
        public int id { get; set; }
        public string organization { get; set; }
        public string password { get; set; }
        public string req_seq { get; set; }
        public string ygorder_num { get; set; }
        public int num { get; set; }
        public string rResultid { get; set; } 
        public string rResultComment { get; set; }
        public string rygorder_num { get; set; }
        public int rnum { get; set; }
        public int orderId { get; set; }
        public DateTime opertime { get; set; } 
    }
}

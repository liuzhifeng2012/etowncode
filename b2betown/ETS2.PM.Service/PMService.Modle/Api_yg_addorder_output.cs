using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class Api_yg_addorder_output
    {
        public Api_yg_addorder_output() { }
        public int id { get; set; }
        public string req_seq { get; set; }
        public string resultid { get; set; }
        public string resultcomment { get; set; }
        public string yg_ordernum { get; set; }
        public string code { get; set; }
        public int orderId { get; set; } 
    }
}

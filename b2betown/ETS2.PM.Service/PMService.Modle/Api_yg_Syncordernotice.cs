using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class Api_yg_Syncordernotice
    {
        public Api_yg_Syncordernotice() { }
        public int id { get; set; }
        public string req_seq { get; set; }
        public string platform_req_seq { get; set; }
        public string order_num { get; set; }
        public int num { get; set; }
        public DateTime use_time { get; set; }
        public string rcontent { get; set; }
        public int orderid { get; set; }
    }
}

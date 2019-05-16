using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class Api_mjld_AsyncBacknotice
    {
        public Api_mjld_AsyncBacknotice() { }
        public int id { get; set; }
        public string type { get; set; }
        public string mjldorderid { get; set; }
        public int backCount { get; set; }
        public int backStatus { get; set; }
        public string postTime { get; set; }
        public string rcontent { get; set; }
        public int orderid { get; set; }
    }
}

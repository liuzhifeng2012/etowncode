using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class Api_mjld_AsyncUsenotice
    {
        public Api_mjld_AsyncUsenotice() { }

        public int id { get; set; }
        public string type { get; set; }
        public string mjldOrderId { get; set; }
        public string credence { get; set; }
        public int useCount { get; set; }
        public int lastCount { get; set; }
        public string useTime { get; set; }
        public string exchangeId { get; set; }
        public string ScenicId { get; set; }
        public string postTime { get; set; }
        public string rcontent { get; set; }
        public int orderId { get; set; }
    }
}

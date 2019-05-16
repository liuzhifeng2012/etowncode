using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    /// <summary>
    /// 客人名单发送给送车员
    /// </summary>
    public class B2b_order_busNamelistSendLog
    {
        public B2b_order_busNamelistSendLog() { }
        public int id { get; set; }
        public string smscontent { get; set; }
        public string sendtophones { get; set; }
        public DateTime sendtime { get; set; }
        public int opertorid { get; set; }
        public string opertorname { get; set; }
        public int proid { get; set; }
        public DateTime traveldate { get; set; }
        public int issuc { get; set; }
    }
}

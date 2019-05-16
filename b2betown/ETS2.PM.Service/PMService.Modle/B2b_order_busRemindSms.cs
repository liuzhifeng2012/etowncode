using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class B2b_order_busRemindSms
    {
        public B2b_order_busRemindSms() { }
        public int id { get; set; }
        public string licenceplate { get; set; }
        public string telphone { get; set; }
        public DateTime instime { get; set; }
        public int proid { get; set; }
        public DateTime traveldate { get; set; }
    }
}

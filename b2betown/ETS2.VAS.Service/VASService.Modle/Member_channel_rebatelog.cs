using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.VAS.Service.VASService.Modle
{
    public class Member_channel_rebatelog
    { 
        public Member_channel_rebatelog() { }
        public int id { get; set; }
        public DateTime subdatetime { get; set; }
        public int orderid { get; set; }
        public string proid { get; set; }
        public string proname { get; set; }
        public decimal ordermoney { get; set; }
        public decimal rebatemoney { get; set; }
        public decimal over_money { get; set; }
        public int channelid { get; set; }
        public int payment { get; set; }
        public string payment_type { get; set; }
        public int comid { get; set; }
    
    }
}

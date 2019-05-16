using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.VAS.Service.VASService.Modle
{
    public class Member_channel_rebateApplyaccount
    {
        public Member_channel_rebateApplyaccount() { }
        public int id { get; set; }
        public int channelid { get; set; }
        public string truename { get; set; }
        public string alipayaccount { get; set; }
        public string alipayphone { get; set; }
        public int accountstatus { get; set; }
        public int comid { get; set; }

       
    }
}

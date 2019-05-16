using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.VAS.Service.VASService.Modle
{
    public class Member_channel_rebateApplylog
    {
        public Member_channel_rebateApplylog() { }
        public int id { get; set; }
        public DateTime applytime { get; set; }

        public string applytype { get; set; }
        public string applydetail { get; set; }
        public decimal applymoney { get; set; }
        public int channelid { get; set; }
        public int operstatus { get; set; }
        public int comid { get; set; }

        public int opertor { get; set; }
        public DateTime opertime { get; set; }

        public string operremark { get; set; }
        public int zhuanzhangsucimg { get; set; }
     
    }
}

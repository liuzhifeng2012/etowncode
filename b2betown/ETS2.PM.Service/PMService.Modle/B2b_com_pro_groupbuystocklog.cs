using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class B2b_com_pro_groupbuystocklog
    {
        public B2b_com_pro_groupbuystocklog() { }
        public int id { get; set; }
        public int proid { get; set; }
        public string proname { get; set; }
        public int isstock { get; set; }
        public int groupbuytype { get; set; }
        public DateTime stocktime { get; set; }
        public int operuserid { get; set; }
        public int comid { get; set; }
        public int stockagentcompanyid { get; set; }
        public string stockagentcompanyname { get; set; }

        public int groupbuystatus { get; set; }
        public string groupbuystatusdesc { get; set; }
    }
}

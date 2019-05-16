using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class B2b_com_pro_pricelog
    {
        public B2b_com_pro_pricelog() { }

        public int id { get; set; }
        public int comid { get; set; }
        public int proid { get; set; }
        public decimal face_price { get; set; }
        public decimal advise_price { get; set; }
        public decimal agentsettle_price { get; set; }
        public decimal Agent1_price { get; set; }
        public decimal Agent2_price { get; set; }
        public decimal Agent3_price { get; set; }
        public DateTime  opertime { get; set; }
        public int operor { get; set; }
    }
}

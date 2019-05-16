using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class B2b_order_bespeak
    {
        public B2b_order_bespeak() { }

        public int Id { get; set; }
        public string Bespeakname { get; set; }
        public string Phone { get; set; }
        public string Idcard { get; set; }
        public DateTime Bespeakdate { get; set; }
        public int Bespeaknum { get; set; }
        public string Remark { get; set; }
        public int Orderid { get; set; }
        public string Pno { get; set; }
        public int Comid { get; set; }
        public int Proid { get; set; }
        public string Proname { get; set; }
        public int beaspeaktype { get; set; }
        public int beaspeakstate { get; set; }
        public string remark { get; set; }
        public DateTime subtime { get; set; }

    }
}

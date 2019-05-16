using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class B2b_com_pro_kucunlog
    {
        public B2b_com_pro_kucunlog() { }
        public int id { get; set; }
        public int orderid { get; set; }
        public int proid { get; set; }
        public int servertype { get; set; }
        public DateTime daydate { get; set; }
        public int proSpeciId { get; set; }
      
        public int surplusnum { get; set; }
        public string opertype { get; set; }
        public string operor { get; set; }
        public string oper { get; set; }
        public DateTime opertime { get; set; }
    }
}

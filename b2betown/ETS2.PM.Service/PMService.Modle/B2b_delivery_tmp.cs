using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class B2b_delivery_tmp
    {
        public B2b_delivery_tmp() { }

        public int id { get; set; }
        public string tmpname { get; set; }
        public string extypes { get; set; }
        public int Operor { get; set; }
        public DateTime opertime { get; set; }
        public int comid { get; set; }
        public int ComputedPriceMethod { get; set; }

      
    }
}

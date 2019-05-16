using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class B2b_delivery_cost
    {
        public B2b_delivery_cost() { }

        public int id { get; set; }
        public int tmpid { get; set; }
        public int Extype { get; set; }
        public int Deftype { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public int First_num { get; set; }
        public decimal First_price { get; set; }
        public int Con_num { get; set; }
        public decimal Con_price { get; set; }
        public int Comid { get; set; }
    }
}

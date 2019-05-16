using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    [Serializable()]
    public class Bus_feeticket_Pro
    {
        public int Id { get; set; }
        public int Proid { get; set; }
        public int Busid { get; set; }
        public string proname { get; set; }
         public int limitweek { get; set; }
         public int limitweekdaynum { get; set; }
         public int limitweekendnum { get; set; }
        


    }
}

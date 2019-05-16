using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    [Serializable()]
    public class Bus_Feeticket_pno
    {
        public int Id { get; set; }
        public int Busid { get; set; }
        public int Proid { get; set; }
        public String Pno { get; set; }
        public int Num { get; set; }
        public string proname { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    [Serializable()]
    public class Bus_Feeticket
    {

         public int Id { get; set; }
         public int Comid { get; set; }
         public String Title { get; set; }
         public String Pno { get; set; }
         public int Num { get; set; }
         public int Feeday { get; set; }
         public DateTime Startime { get; set; }
         public DateTime Endtime { get; set; }
         public int Iuse { get; set; }

    }
}

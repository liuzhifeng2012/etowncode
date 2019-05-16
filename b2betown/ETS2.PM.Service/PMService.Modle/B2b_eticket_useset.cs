using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
     public class B2b_eticket_useset
    {
         public B2b_eticket_useset() { }
         public int id { get; set; }
         public int datetype { get; set; }
         public int etickettype { get; set; }
         public int comid { get; set; }
         public int operid { get; set; }
         public DateTime opertime { get; set; }
             
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    [Serializable()]
    public class b2b_eticket_Deposit
    {
        public int id { get; set; }
        public int eticketid { get; set; }
        public int sid { get; set; }
        public decimal saleprice { get; set; }
        public decimal Depositprice { get; set; }
        public int Depositorder { get; set; }
        public DateTime subtime { get; set; }
        public int Backdepositstate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
        [Serializable()]
    public class B2b_Rentserver_User_info
    {
        public int id { get; set; }
        public int comid { get; set; }
        public int Userid { get; set; }
        public int Rentserverid { get; set; }
        public decimal Depositprice { get; set; }
        public int Verstate { get; set; }
        public int num { get; set; }
        public DateTime Vertime { get; set; }
        public DateTime Rettime { get; set; }
        public string servername { get; set; }
        public string Remarks { get; set; }
    }
}

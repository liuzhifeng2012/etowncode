using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class Travelbusorder_sendbus
    {
        public Travelbusorder_sendbus() { }
        public int id { get;set;}
        public int travelbusorder_operlogid { get; set; }
        public string drivername { get; set; }
        public string driverphone { get; set; }
        public string licenceplate { get; set; }
        public string travelbus_model { get; set; }
        public int seatnum { get; set; }
    }
}

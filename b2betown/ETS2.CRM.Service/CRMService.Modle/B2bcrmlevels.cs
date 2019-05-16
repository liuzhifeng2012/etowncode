using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    public class B2bcrmlevels
    {
        public B2bcrmlevels() { }

        public int id { get; set; }
        public string crmlevel { get; set; }
        public string levelname { get; set; }
        public decimal dengjifen_begin { get; set; }
        public decimal dengjifen_end { get; set; }
        public string tequan { get; set; }
        public int com_id { get; set; }
        public int isavailable { get; set; }

    }
}

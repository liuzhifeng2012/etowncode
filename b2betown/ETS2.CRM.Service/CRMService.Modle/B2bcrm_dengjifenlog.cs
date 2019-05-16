using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    public class B2bcrm_dengjifenlog
    {
        public int id { get; set; }
        public int crmid { get; set; }
        public decimal dengjifen { get; set; }
        public int ptype { get; set; }
        public string opertor { get; set; }
        public DateTime opertime { get; set; }
        public int orderid { get; set; }
        public string ordername { get; set; }
        public string remark { get; set; }
    }
}

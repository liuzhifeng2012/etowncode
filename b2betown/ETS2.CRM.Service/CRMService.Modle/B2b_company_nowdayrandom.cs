using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    public class B2b_company_nowdayrandom
    {
        public int id { get; set; }
        public int comid { get; set; }
        public string randomstr { get; set; }
        public DateTime nowdate { get; set; }
        public DateTime createtime { get; set; }
        public string createposid { get; set; }
        public string remark { get; set; }
    }
}

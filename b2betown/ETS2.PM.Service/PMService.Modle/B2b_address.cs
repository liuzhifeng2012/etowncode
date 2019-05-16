using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class B2b_address
    {
        public B2b_address() { }
        public int Id { get; set; }
        public string U_name { get; set; }
        public string U_phone { get; set; }
        public int Agentid { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Code { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.LMM.Model
{

        public class Lvmama_reqlog
        {
            public Lvmama_reqlog() { }
            public int id { get; set; }
            public string reqstr { get; set; }
            public string subtime { get; set; }
            public string respstr { get; set; }
            public string resptime { get; set; }
            public string code { get; set; }
            public string describe { get; set; }
            public string usecode { get; set; }
            public string usedescribe { get; set; }
            public string req_type { get; set; }
            public string sendip { get; set; }

            public string mtorderid { get; set; }
            public string ordernum { get; set; }
            public int issecond_req { get; set; }

            public int stockagentcompanyid { get; set; }
        }
}

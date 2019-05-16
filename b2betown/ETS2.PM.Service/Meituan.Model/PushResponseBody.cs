using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    public class PushResponseBody
    {
        public PushResponseBody() { }
        public int partnerId { get; set; }
        public string code { get; set; }
        public string describe { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    public class MtpApiResponse
    {
        public MtpApiResponse() { }
        public int code { get; set; }
        public string describe { get; set; }
        public int partnerId { get; set; } 
    }
}

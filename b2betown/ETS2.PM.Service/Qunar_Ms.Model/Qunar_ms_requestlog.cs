using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Qunar_Ms.Model
{
    public class Qunar_ms_requestlog
    {
        public int id { get; set; }
        public string method { get; set; }
        public string requestParam { get; set; }
        public string base64data { get; set; }
        public string securityType { get; set; }
        public string signed { get; set; }
        public string frombase64data { get; set; }
        public string bodyType { get; set; }
        public string createUser { get; set; }
        public string supplierIdentity { get; set; }
        public DateTime createTime { get; set; }
        public string qunar_orderId { get; set; }
        public string msg { get; set; }
    }
}

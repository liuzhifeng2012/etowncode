using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.WL.Model
{
    public class wlOrderCreateResponse
    {
        public int code { get; set; }
        public string describe { get; set; }
        public string partnerId { get; set; }
        public int id { get; set; }


        public wlOrderCreateResponseBody body { get; set; }
    }

    public class wlOrderCreateResponseBody
    {
        public wlOrderCreateResponseBody() { }
        public string partnerOrderId { get; set; }
        public string wlOrderId { get; set; }
    }
}

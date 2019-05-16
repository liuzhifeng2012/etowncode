using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.WL.Model
{
    public class wlOrderQueryRequest
    {
        public string partnerId { get; set; }

        public wlOrderQueryRequestBody body { get; set; }
    }

    public class wlOrderQueryRequestBody
    {
        public wlOrderQueryRequestBody() { }
        public string partnerOrderId { get; set; }
        

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.WL.Model
{

    public class wlhexiao
    {
        public int partnerId { get; set; }
        public body body { get; set; }
    }

    public class body
    {
        public body() { }
        public string wlOrderId { get; set; }
        public int usedQuantity { get; set; }
        public int quantity { get; set; }
        public int refundedQuantity { get; set; }

    }
}

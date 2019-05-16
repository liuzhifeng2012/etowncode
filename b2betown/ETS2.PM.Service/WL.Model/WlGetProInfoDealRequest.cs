using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.WL.Model
{
    public class WlGetProInfoDealRequest
    {
        public string PartnerId { get; set; }
        public WlDealRequestBody body { get; set; }
    }

    public class WlDealRequestBody
    {
        public WlDealRequestBody() { }

        public string method { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        
    }
}

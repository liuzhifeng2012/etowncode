using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.WL.Model
{
    public class wlOrderCloseNoticeRequest
    {
        public int partnerId { get; set; }

        public List<wlOrderCloseNoticeRequestBody> body { get; set; }
    }

    public class wlOrderCloseNoticeRequestBody
    {
        public wlOrderCloseNoticeRequestBody() { }
        public string wlOrderId { get; set; }
     

    }
}

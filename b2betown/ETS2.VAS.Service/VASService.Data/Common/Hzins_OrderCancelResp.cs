using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.VAS.Service.VASService.Data.Common
{
    public class Hzins_OrderCancelResp
    {
        public int respCode { get; set; }
        public string respMsg { get; set; }
        public OrderCancelResp_data data { get; set; }
    }
    public class OrderCancelResp_data
    {
        public string transNo { get; set; }
        public int partnerId { get; set; }
        public string insureNo { get; set; }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    /// <summary>
    /// 投保订单信息
    /// </summary>
    public class Api_hzins_OrderApplyReq_Application
    {
        public Api_hzins_OrderApplyReq_Application() { }
        public int id { get; set; }
        public string applicationDate { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public decimal singlePrice { get; set; }
        public int orderid { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class B2b_Rentserver_RefundLog
    {
        public B2b_Rentserver_RefundLog() { }
        public int id { get; set; }
        public int orderid { get; set; }

        public int proid { get; set; }
        public string proname { get; set; }
        public int comid { get; set; }
        public string comname { get; set; }

        public int rentserverid { get; set; }
        public string rentservername { get; set; }
        public decimal ordertotalfee { get; set; }
        public decimal refundfee { get; set; }
        public DateTime subtime { get; set; }
        public string pay_com { get; set; }
        public int  refundstate { get; set; }
        public string refundremark { get; set; }

        public int b2b_eticket_Depositid { get; set; }

    }
}

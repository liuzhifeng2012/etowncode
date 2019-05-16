using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Tenpay.WxpayApi.sysprogram.model
{
    public class B2b_pay_wxrefundlog
    {
        public B2b_pay_wxrefundlog() { }
        public int id { get; set; }
        public string out_refund_no { get; set; }
        public string out_trade_no { get; set; }
        public string transaction_id { get; set; }
        public int total_fee { get; set; }
        public int refund_fee { get; set; }
        public string send_xml { get; set; }
        public DateTime send_time { get; set; }
        public string return_code { get; set; }
        public string return_msg { get; set; }
        public string err_code { get; set; }
        public string err_code_des { get; set; }
        public string refund_id { get; set; }
        public string return_xml { get; set; }
        public DateTime return_time { get; set; }
    }
}

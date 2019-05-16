using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Alipiay.app_code2.SysProgram.model
{
    public class B2b_pay_alipayrefundlog
    {
        public B2b_pay_alipayrefundlog() { }

        public int id { get; set; }
        public int orderid { get; set; }
        public string service { get; set; }
        public string partner { get; set; }
        public string notify_url { get; set; }
        public string seller_email { get; set; }

        public string seller_user_id { get; set; }
        public DateTime refund_date { get; set; }
        public string batch_no { get; set; }
        public int batch_num { get; set; }
        public string detail_data { get; set; }
        public DateTime notify_time { get; set; }
        public string notify_type { get; set; }
        public string notify_id { get; set; }

        public int success_num { get; set; }
        public string result_details { get; set; } 
        public string error_code { get; set; }
        public string error_desc { get; set; }

        public decimal refund_fee { get; set; }

        public int rentserver_refundlogid { get; set; }
    }
}

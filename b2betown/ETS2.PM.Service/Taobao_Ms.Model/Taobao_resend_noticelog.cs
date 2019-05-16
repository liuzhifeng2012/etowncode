using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Taobao_Ms.Model
{
    public class Taobao_resend_noticelog
    {
        public Taobao_resend_noticelog() { }

        public int id { get; set; }
        public string timestamp { get; set; }
        public string sign { get; set; }
        public string order_id { get; set; }
        public string mobile { get; set; }
        public int num { get; set; }
         public int left_num { get; set; }
        public string method { get; set; }
        public string taobao_sid { get; set; }
        public string seller_nick { get; set; }
        public string item_title { get; set; }
         public string num_iid { get; set; }
        public string outer_iid { get; set; }
        public string sub_outer_iid { get; set; }
        public string sku_properties { get; set; }
        public int send_type { get; set; }
        public int consume_type { get; set; }
        public string sms_template { get; set; }
        public DateTime valid_start { get; set; }
        public DateTime valid_ends { get; set; }
        public string token { get; set; }

        public DateTime subtime { get; set; }
        public string responsecode { get; set; }
        public DateTime responsetime { get; set; }
        public int  self_order_id { get; set; }
        public int agentid { get; set; }
        public string errmsg { get; set; }
        public int type { get; set; }
        public string encrypt_mobile { get; set; }
        public string md5_mobile { get; set; }
    }
}

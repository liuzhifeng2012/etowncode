using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Taobao_Ms.Model
{
    public class Taobao_reverse_retlog
    {
        public Taobao_reverse_retlog() { }
        public int id { get; set; }
        public string order_id { get; set; }
        public string reverse_code { get; set; }
        public int reverse_num { get; set; }
        public string consume_secial_num { get; set; }
        public string verify_codes { get; set; }
        public string qr_images { get; set; }
        public string token { get; set; }
        public string codemerchant_id { get; set; }
        public string posid { get; set; }

       
        public string ret_code { get; set; } 
        public string item_title { get; set; }
        public int left_num { get; set; }
        
        public DateTime ret_time { get; set; }
    }
}

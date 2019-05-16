using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Taobao_Ms.Model
{
    public class Taobao_order_modify_noticelog
    {
        public Taobao_order_modify_noticelog() { }

        public int id { get; set; }
        public string timestamp { get; set; }
        public string sign { get; set; }
        public string order_id { get; set; }
        
        public string method { get; set; }
        public string taobao_sid { get; set; }
        public string seller_nick { get; set; }

        public string sub_method { get; set; }
        public string data { get; set; }
       
        public DateTime subtime { get; set; }
        public string responsecode { get; set; }
        public DateTime responsetime { get; set; }
        public int  self_order_id { get; set; }
        public int agentid { get; set; }
        public string errmsg { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    /// <summary>
    /// 大巴给客人发送提醒日志
    /// </summary>
    public class B2b_order_busRemindSmsLog
    {
        public B2b_order_busRemindSmsLog() { }
        public int id { get; set; }
        public string smscontent { get; set; }
        public string sendtophones { get; set; }
        public DateTime sendtime { get; set; }
        public int opertorid { get; set; }
        public string opertorname { get; set; }
        public int proid { get; set; }
        public DateTime traveldate { get; set; }
        public int issuc { get; set; }
    }
   
}

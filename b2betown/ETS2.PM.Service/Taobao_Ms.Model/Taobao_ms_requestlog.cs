using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Taobao_Ms.Model
{
    public class Taobao_ms_requestlog
    {
        public Taobao_ms_requestlog() { }
        public int id { get; set; }
        public string noticemethod { get; set; }//通知类型
        public string parastr { get; set; }
        public DateTime subtime { get; set; }
        public string sendip { get; set; }
        public string httpmethod { get; set; }
        public int isrightsign { get; set; }
    }
}

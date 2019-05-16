using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    public class ValidCode_SendLog
    {
        public int id { get; set; }
        public string mobile { get; set; }
        public string randomcode { get; set; }
        public string Content { get; set; }
        public DateTime sendtime { get; set; }
        public int send_serialnum { get; set; }//发送序列号
        public string returnmsg { get; set; }
        public string source { get; set; }
        public string sendip { get; set; }
    }
}

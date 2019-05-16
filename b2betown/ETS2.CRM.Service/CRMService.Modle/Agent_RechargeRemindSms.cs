using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    public class Agent_RechargeRemindSms
    {
        public Agent_RechargeRemindSms() { }
        public int id { get; set; }
        public int agentid { get; set; }
        public int comid { get; set; }
        public string smscontent { get; set; }
        public string tishi_phone { get; set; }
        public DateTime tishi_time { get; set; }
        public string tishi_edu { get; set; }
        public int isdeal { get; set; }
    }
}

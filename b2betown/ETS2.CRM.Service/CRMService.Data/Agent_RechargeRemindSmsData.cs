using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Data.InternalData;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class Agent_RechargeRemindSmsData
    {
        public Agent_RechargeRemindSms GetAgent_RechargeRemindSms(int agentid, int comid, int edu, int isdeal)
        {
            using (var helper = new SqlHelper())
            {
                Agent_RechargeRemindSms r = new Internalagent_RechargeRemindSms(helper).GetAgent_RechargeRemindSms(agentid, comid, edu, isdeal);
                return r;
            }
        }

        public int InsRemindsms(Agent_RechargeRemindSms m)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internalagent_RechargeRemindSms(helper).InsRemindsms(m);
                return r;
            }
        }

        public int UpremindSmsState(int agentid, int comid, int isdeal)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internalagent_RechargeRemindSms(helper).UpremindSmsState(agentid, comid, isdeal);
                return r;
            }
        }

        public int GetAgent_NowdayRechargeRemindSmsNum(int agentid, int comid, int edu)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internalagent_RechargeRemindSms(helper).GetAgent_NowdayRechargeRemindSmsNum(agentid, comid, edu);
                return r;
            }
        }
    }
}

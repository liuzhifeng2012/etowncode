using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    public class Internalagent_RechargeRemindSms
    {
        public SqlHelper sqlHelper;
        public Internalagent_RechargeRemindSms(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal Agent_RechargeRemindSms GetAgent_RechargeRemindSms(int agentid, int comid, int edu, int isdeal)
        {
            string sql = "select * from agent_RechargeRemindSms where agentid=" + agentid + " and comid=" + comid + " and tishi_edu=" + edu + " and isdeal=" + isdeal;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                Agent_RechargeRemindSms m = null;
                if (reader.Read())
                {
                    m = new Agent_RechargeRemindSms
                    {
                        id = reader.GetValue<int>("id"),
                        agentid = reader.GetValue<int>("agentid"),
                        comid = reader.GetValue<int>("comid"),
                        smscontent = reader.GetValue<string>("smscontent"),
                        tishi_phone = reader.GetValue<string>("tishi_phone"),
                        tishi_time = reader.GetValue<DateTime>("tishi_time"),
                        tishi_edu = reader.GetValue<string>("tishi_edu"),
                        isdeal = reader.GetValue<int>("isdeal")
                    };
                }
                return m;
            }
        }

        internal int InsRemindsms(Agent_RechargeRemindSms m)
        {
            string sql = @"INSERT  [agent_RechargeRemindSms]
           ([agentid]
           ,[comid]
           ,[smscontent]
           ,[tishi_phone]
           ,[tishi_time]
           ,[tishi_edu]
           ,[isdeal])
     VALUES
           (@agentid 
           ,@comid 
           ,@smscontent 
           ,@tishi_phone 
           ,@tishi_time 
           ,@tishi_edu 
           ,@isdeal);select @@identity;";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@agentid", m.agentid);
            cmd.AddParam("@comid", m.comid);
            cmd.AddParam("@smscontent", m.smscontent);
            cmd.AddParam("@tishi_phone", m.tishi_phone);
            cmd.AddParam("@tishi_time", m.tishi_time);
            cmd.AddParam("@tishi_edu", m.tishi_edu);
            cmd.AddParam("@isdeal", m.isdeal);

            object o = cmd.ExecuteScalar();
            return int.Parse(o.ToString());
        }

        internal int UpremindSmsState(int agentid, int comid, int isdeal)
        {
            string sql = "update agent_RechargeRemindSms set isdeal=" + isdeal + " where agentid=" + agentid + " and comid=" + comid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int GetAgent_NowdayRechargeRemindSmsNum(int agentid, int comid, int edu)
        {
            string sql = "select count(1) as remindnum from agent_RechargeRemindSms where agentid=" + agentid + " and comid=" + comid + " and tishi_edu=" + edu + " and convert(varchar(10),tishi_time,120)='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int remindnum = 0;
                if (reader.Read())
                {
                    remindnum = reader.GetValue<int>("remindnum");
                }
                return remindnum;
            }
        }
    }
}

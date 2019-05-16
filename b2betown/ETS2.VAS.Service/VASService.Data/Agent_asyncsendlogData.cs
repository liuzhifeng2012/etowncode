using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.VAS.Service.VASService.Modle;
using ETS.Data.SqlHelper;
using ETS2.VAS.Service.VASService.Data.InternalData;

namespace ETS2.VAS.Service.VASService.Data
{
    public class Agent_asyncsendlogData
    {
        public int EditLog(Agent_asyncsendlog log)
        {
            using (var helper = new SqlHelper())
            {

                int result = new InternalAgent_asyncsendlog(helper).EditLog(log);
                return result;

            }
        }

        public Agent_asyncsendlog GetTop1SendFail()
        {
            using (var helper = new SqlHelper())
            {

                Agent_asyncsendlog result = new InternalAgent_asyncsendlog(helper).GetTop1SendFail();
                return result;

            }
        }

        public List<Agent_asyncsendlog> GetTop10SendFail()
        {
            using (var helper = new SqlHelper())
            {

                List<Agent_asyncsendlog> result = new InternalAgent_asyncsendlog(helper).GetTop10SendFail();
                return result;

            }
        }

        public int Getnoticelognum(string key)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalAgent_asyncsendlog(helper).Getnoticelognum(key);
                return result;
            }
        }

        public List<Agent_asyncsendlog> GetSendNoticeErrList(string pno)
        {
            using (var helper = new SqlHelper())
            {
                List<Agent_asyncsendlog> result = new InternalAgent_asyncsendlog(helper).GetSendNoticeErrList(pno);
                return result;
            }
        }

        public List<Agent_asyncsendlog> Getyzlogs(int agentcomid, string yzdate, string pno, int comid,int pageindex,int pagesize,out int totalcount, int id = 0)
        {
            using (var helper = new SqlHelper())
            { 
                List<Agent_asyncsendlog> result = new InternalAgent_asyncsendlog(helper).Getyzlogs(agentcomid, yzdate, pno, comid,pageindex,pagesize,out totalcount, id);
                return result;
            }
        }

        public int IsneedreissueById(int id)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalAgent_asyncsendlog(helper).IsneedreissueById(id);
                return result;
            }
        }

        public List<Agent_asyncsendlog> getNoticesByYzlogid(int b2b_etcket_logid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                List<Agent_asyncsendlog> result = new InternalAgent_asyncsendlog(helper).getNoticesByYzlogid(b2b_etcket_logid,out totalcount);
                return result;
            }
        }

        public Agent_asyncsendlog getNoticeLog(int noticelogid)
        {
            using (var helper = new SqlHelper())
            {
                Agent_asyncsendlog result = new InternalAgent_asyncsendlog(helper).getNoticeLog(noticelogid);
                return result;
            }
        }
    }
}

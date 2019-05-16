using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Data.InternalData;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class B2b_invitecodesendlogData
    {
   
        public int Inslog(B2b_invitecodesendlog log)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalB2b_invitecodesendlog(helper).Inslog(log);
                return result;
            }
        }

        public List<B2b_invitecodesendlog> Getinvitecodesendlog(int comid, int userid, int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                List<B2b_invitecodesendlog> result = new InternalB2b_invitecodesendlog(helper).Getinvitecodesendlog(comid,  userid,  pageindex,  pagesize, out  totalcount);
                return result;
            }
        }

        public B2b_invitecodesendlog GetNoteRecord(string invitecode, int comid)
        {
            using (var helper = new SqlHelper())
            {
                B2b_invitecodesendlog result = new InternalB2b_invitecodesendlog(helper).GetNoteRecord(invitecode, comid);
                return result;
            }
        }
    }
}

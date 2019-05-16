using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Data.InternalData;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class B2b_crm_interesttagData
    {

        public IList<B2b_crm_interesttag> GetCrmInterest(out int total, int crmid)
        {
            using (var sql = new SqlHelper())
            {
                IList<B2b_crm_interesttag> result = new InternalB2b_crm_interesttag(sql).GetCrmInterest(out total, crmid);
                return result;

            }
        }

        public int EditCrmInterest(int crmid, string checkedstr)
        {
            using(var sql=new SqlHelper())
            {
                int result = new InternalB2b_crm_interesttag(sql).EditCrmInterest(crmid,checkedstr);
                return result;
            }
        }
    }
}

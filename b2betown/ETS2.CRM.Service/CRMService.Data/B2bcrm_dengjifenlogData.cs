using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Data.InternalData;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class B2bcrm_dengjifenlogData
    {
        public int InsertOrUpdate(B2bcrm_dengjifenlog djflog)
        {
             using(var helper=new SqlHelper())
             {
                 int m = new InternalB2bcrm_dengjifenlog(helper).InsertOrderUpdate(djflog);
                 return m;
             }
        }

        public List<B2bcrm_dengjifenlog> Getdengjifenlist(int id, int comid, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                List<B2bcrm_dengjifenlog> m = new InternalB2bcrm_dengjifenlog(helper).Getdengjifenlist(id, comid, out totalcount);
                return m;
            }
        }
    }
}

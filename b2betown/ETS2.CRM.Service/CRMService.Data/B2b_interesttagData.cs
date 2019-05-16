using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Data.InternalData;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class B2b_interesttagData
    {

        public IList<B2b_interesttag> GetTagListByTypeid(out int total, int typeid,int comid,string issystemadd)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalB2b_interesttag(helper).GetTagListByTypeid(out total, typeid,comid,issystemadd);

                return crmid;
            }
        }

        public B2b_interesttag GetTagById(int id)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalB2b_interesttag(helper).GetTagById(id);

                return crmid;
            }
        }

        public int EditTag(int id, string name, int tagtypeid,int issystemadd=0,int comid=0)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalB2b_interesttag(helper).EditTag(id, name, tagtypeid,issystemadd,comid);
                return result;
            }
        }

        public int Deltag(int tagid)
        {
            using (var helper = new SqlHelper())
            {
                int result = new InternalB2b_interesttag(helper).Deltag(tagid);
                return result;
            }
        }
    }
}

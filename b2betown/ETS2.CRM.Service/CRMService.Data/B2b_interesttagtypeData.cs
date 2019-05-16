using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Data.InternalData;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class B2b_interesttagtypeData
    {
        public IList<B2b_interesttagtype> GetTagTypeList(int industryid, out int secondtotal)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalB2b_interesttagtype(helper).GetTagTypeList(industryid, out secondtotal);

                return crmid;
            }
        }

        public B2b_interesttagtype GetTagTypeById(int id)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalB2b_interesttagtype(helper).GetTagTypeById(id);

                return crmid;
            }
        }

        public int EditComTagType(int id, string name, string remark,int industryid)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalB2b_interesttagtype(helper).EditComTagType(id, name, remark, industryid);

                return crmid;
            }
        }

        public int Deltagtype(int id)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalB2b_interesttagtype(helper).Deltagtype(id);

                return crmid;
            }
        }
    }
}

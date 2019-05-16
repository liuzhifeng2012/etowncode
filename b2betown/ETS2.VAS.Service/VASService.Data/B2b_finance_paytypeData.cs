using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.VAS.Service.VASService.Modle;
using ETS.Data.SqlHelper;
using ETS2.VAS.Service.VASService.Data.InternalData;

namespace ETS2.VAS.Service.VASService.Data
{
    public class B2b_finance_paytypeData
    {
        public B2b_finance_paytype GetFinancePayTypeByComid(int comid)
        {
            using (var helper=new SqlHelper())
            {
                B2b_finance_paytype m = new InternalB2b_finance_paytype(helper).GetFinancePayTypeByComid(comid);
                return m;
            }
        }
    }
}

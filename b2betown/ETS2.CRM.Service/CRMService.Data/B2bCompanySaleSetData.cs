using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data.InternalData;
using ETS.Data.SqlHelper;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class B2bCompanySaleSetData
    {
        public int InsertOrUpdate(B2b_company_saleset saleset)
        {
            using (var helper = new SqlHelper())
            {

                var id = new InternalB2bCompanySaleSet(helper).InsertOrUpdate(saleset);

                return id;
            }
        }

        public int Updatesms(B2b_company_saleset saleset)
        {
            using (var helper = new SqlHelper())
            {

                var id = new InternalB2bCompanySaleSet(helper).Updatesms(saleset);

                return id;
            }
        }

        public static B2b_company_saleset GetDirectSellByComid(string comid)
        {
            using (var helper = new SqlHelper())
            {
                try
                {
                    var salesetinfo = new InternalB2bCompanySaleSet(helper).GetDirectSellByComid(comid);

                    return salesetinfo;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}

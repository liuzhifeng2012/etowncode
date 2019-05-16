using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data.InternalData;
using ETS.Data.SqlHelper;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class B2bCompanyPosData
    {
        public B2b_company_pos GetPosByPosId(string pos_id)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bCompanyPos(sql);
                    B2b_company_pos result = internalData.GetPosByPosId(pos_id);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public List<B2b_company_pos> GetPosByComId(int comid, out int secondtotalcount)
        {
            using (var sql = new SqlHelper())
            {
              
                    var internalData = new InternalB2bCompanyPos(sql);
                   List<B2b_company_pos> result = internalData.GetPosByComId(comid,out secondtotalcount);

                    return result;
              
            }
        }
    }
}

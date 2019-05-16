using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Data.InternalData;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class PosversionmodifylogData
    {
        public Posversionmodifylog GetLatestVersion(string pos_id)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalPosversionmodifylog(sql);
                    Posversionmodifylog result = internalData.GetLasterPosversionmodifylogByPosid(pos_id);

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public IList<Posversionmodifylog> GetPosVersionPageList(int pageindex, int pagesize, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalPosversionmodifylog(helper).GetPosVersionPageList(pageindex, pagesize, out totalcount);

                return list;
            }
        }
    }
}

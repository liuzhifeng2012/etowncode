using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.Meituan.Model;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Meituan.Data.Internal;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.PM.Service.Meituan.Data
{
    public class DealResponseData
    {
                                                                                                                          
        public List<DealResponseBody> GetDealResponseBody(out int totalcount,Agent_company agentinfo,string method, List<string> productIdList,  int currentPage=0, int  pageSize=0)
        {
            using(var helper=new SqlHelper())
            {
                List<DealResponseBody> list = new InternalDealResponse(helper).GetDealResponseBody(out totalcount, agentinfo, method,  productIdList,  currentPage, pageSize);
                return list;
            }
        }
    }
}

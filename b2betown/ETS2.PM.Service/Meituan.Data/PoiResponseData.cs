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
    public class PoiResponseData
    {
        public List<PoiResponseBody> GetPoiResponseBody(out int totalcount,string method,List<string> poiIdList, Agent_company agentinfo,int pageindex=0,int pagesize=0)
        {
             using(var helper=new SqlHelper())
             {
                 List<PoiResponseBody> list = new InternalPoiResponse(helper).GetPoiResponseBody(out totalcount,method, poiIdList,agentinfo,pageindex,pagesize);
                 return list;
             }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Data.InternalData;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class ObtainGzListSplitData
    {
        public int MaxSplitNo(int comid, int OtainNo)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalObtainGzListSplit(helper).MaxSplitNo(comid, OtainNo);

                return crmid;
            }
        }

        public int InsertsObtainGzListSplit(ObtainGzListSplit splitmodel)
        {
            using(var helper=new SqlHelper())
            {
                int result = new InternalObtainGzListSplit(helper).InsertsObtainGzListSplit(splitmodel);

                return result ;
            }
        }

        public ObtainGzListSplit GetObtainGzListSplit(int id)
        {
            using (var helper = new SqlHelper())
            {
                ObtainGzListSplit result = new InternalObtainGzListSplit(helper).GetObtainGzListSplit(id);

                return result;
            }
        }

        public ObtainGzListSplit LastSplitNoByComid(int comid)
        {
            using (var helper = new SqlHelper())
            {

                var model = new InternalObtainGzListSplit(helper).LastSplitNoByComid(comid);

                return model;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Data.InternalData;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class ObtainGzListLogData
    {
        public int GetMaxObtainNo(int comid)
        {
            using (var helper = new SqlHelper())
            {

                var crmid = new InternalObtainGzListLog(helper).GetMaxObtainNo(comid);

                return crmid;
            }
        }

        public int InsObtainLog(int id, string total, string count, string openid, string next_openid, DateTime obtaintime, string errcode, string errmsg, int comid, int dealuserid, int MaxOtainNo)
        {
            using (var helper = new SqlHelper())
            {

                int logid = new InternalObtainGzListLog(helper).InsLog(id,total,count,openid,next_openid,obtaintime,errcode,errmsg,comid,dealuserid,MaxOtainNo);

                return logid;
            }
        }

        public string GetNextOpenId(int comid, int obtainno)
        {
            using (var helper = new SqlHelper())
            {

                string nextopenid = new InternalObtainGzListLog(helper).GetNextOpenId(comid,obtainno);

                return nextopenid;
            }
        }

        public IList<Modle.ObtainGzListLog> GetObtaingzlistlog(int comid, int MaxOtainNo)
        {
            using (var helper = new SqlHelper())
            {

                List<Modle.ObtainGzListLog> list = new InternalObtainGzListLog(helper).GetObtaingzlistlog(comid, MaxOtainNo);

                return list;
            }
        }
    }
}

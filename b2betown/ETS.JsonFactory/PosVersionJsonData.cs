using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Newtonsoft.Json;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle.Enum;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS.JsonFactory
{
    public class PosVersionJsonData
    {
        #region 得到pos机最近更新版本列表
        public static string GetPosVersionPageList(int pageindex, int pagesize)
        {
            var totalcount = 0;
            try
            {

                var edata = new PosversionmodifylogData();
                var list = edata.GetPosVersionPageList(pageindex, pagesize, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from e in list
                             select new
                             {
                                 id = e.Id,
                                 posid=e.Posid,
                                 versionno = e.VersionNo,
                                 updatetype = EnumUtils.GetName((PosVersionType)e.Updatetype),
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion
    }
}

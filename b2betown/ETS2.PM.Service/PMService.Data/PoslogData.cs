using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data
{
    public class PoslogData
    {
        #region pos发送参数日志
        public int InsertOrUpdate(Modle.Pos_log poslog)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalPosLog(sql);
                    int result = internalData.InsertOrUpdate(poslog);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        public Modle.Pos_log GetPosLogById(int poslogid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalPosLog(helper).GetPosLogById(poslogid);

                return pro;
            }
        }

        public List<Pos_log> GetPosLogList(int pageindex, int pagesize, string starttime, string key,out int totalcount)
        {
             using(var helper=new SqlHelper())
             {
                 List<Pos_log> list = new InternalPosLog(helper).GetPosLogList(pageindex,pagesize,starttime,key,out totalcount);
                 return list;
             }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
    public class Wxqunfa_logData
    {
        public int EditLog(Wxqunfa_log log)
        {
            using (var helper = new SqlHelper())
            {
                int id = new InternalWxqunfa_log(helper).EditLog(log);
                return id;

            }
        }

        public int GetSendNum(int comid, int channelcompanyid, string yearmonth)
        {
            using (var helper = new SqlHelper())
            {
                int id = new InternalWxqunfa_log(helper).GetSendNum(comid,channelcompanyid,yearmonth);
                return id;

            }
        }

        public List<Wxqunfa_log> GetQunfalist(int comid, int userid, int pageindex, int pagesize, out int totalcount)
        {
             using (var helper=new SqlHelper())
             {
                 List<Wxqunfa_log> list = new InternalWxqunfa_log(helper).GetQunfalist(comid, userid,pageindex,pagesize,out totalcount);
                 return list;
             }
        }
    }
}

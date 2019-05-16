using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
    public class Wxqunfa_news_addrecordData
    {
        public List<Wxqunfa_news_addrecord> Wxqunfa_news_addrecordpagelist(int userid, int comid, int pageindex, int pagesize, string key, out int totalcount)
        {
            using (var helper = new SqlHelper())
            {
                List<Wxqunfa_news_addrecord> list = new Internalwxqunfa_news_addrecord(helper).Wxqunfa_news_addrecordpagelist(userid, comid, pageindex, pagesize, key, out totalcount);
                return list;
            }
        }

        public Wxqunfa_news_addrecord Wxqunfa_news_addrecord(int userid, int comid, int tuwen_recordid)
        {
            using (var helper = new SqlHelper())
            {
                Wxqunfa_news_addrecord r = new Internalwxqunfa_news_addrecord(helper).Wxqunfa_news_addrecord(userid, comid, tuwen_recordid);
                return r;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
    public class Wxqunfa_newsData
    {
        public List<Wxqunfa_news> GetNewsListByRecordid(int recordid)
        {
            using (var helper = new SqlHelper())
            {

                List<Wxqunfa_news> list = new Internalwxqunfa_news(helper).GetNewsListByRecordid(recordid);
                return list;
            }
        }



        public List<Wxqunfa_news> GetTop1NewsListByRecordid(int recordid)
        {
            using (var helper = new SqlHelper())
            {

                List<Wxqunfa_news> list = new Internalwxqunfa_news(helper).GetTop1NewsListByRecordid(recordid);
                return list;
            }
        }
    }
}

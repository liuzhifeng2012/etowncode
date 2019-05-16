using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.Taobao_Ms.Model;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Taobao_Ms.Data.Internal;

namespace ETS2.PM.Service.Taobao_Ms.Data
{
    public class Taobao_modified_noticelogData
    {
        public int Editnoticelog(Taobao_modified_noticelog noticelog)
        {
            using (var helper = new SqlHelper())
            {
                var r = new InternalTaobao_modified_noticelog(helper).Editnoticelog(noticelog);
                return r;
            }
        }

        public int GetNoticeNum(string token)
        {
            using (var helper = new SqlHelper())
            {
                var r = new InternalTaobao_modified_noticelog(helper).GetNoticeNum(token);
                return r;
            }
        }
    }
}

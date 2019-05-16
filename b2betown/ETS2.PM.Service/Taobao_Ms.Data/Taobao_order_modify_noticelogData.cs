using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.Taobao_Ms.Model;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Taobao_Ms.Data.Internal;

namespace ETS2.PM.Service.Taobao_Ms.Data
{
    public class Taobao_order_modify_noticelogData
    {
        public int Editnoticelog(Taobao_order_modify_noticelog noticelog)
        {
            using (var helper = new SqlHelper())
            {
                var r = new InternalTaobao_order_modify_noticelog(helper).Editnoticelog(noticelog);
                return r;
            }
        }

        public int GetNoticeNum(string taobao_orderid, string method)
        {
            using (var helper = new SqlHelper())
            {
                var r = new InternalTaobao_order_modify_noticelog(helper).GetNoticeNum(taobao_orderid, method);
                return r;
            }
        }
    }
}

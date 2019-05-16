using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.Taobao_Ms.Model;
using ETS2.PM.Service.Taobao_Ms.Data.Internal;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.Taobao_Ms.Data
{
    public class Taobao_resend_noticelogData
    {
        public int Editnoticelog(Taobao_resend_noticelog noticelog)
        {
            using (var helper = new SqlHelper())
            {
                var r = new InternalTaobao_resend_noticelog(helper).Editnoticelog(noticelog);
                return r;
            }
        }

        //public int GetNoticeNum(string taobao_orderid, string method)
        //{
        //    using (var helper = new SqlHelper())
        //    {
        //        var r = new InternalTaobao_resend_noticelog(helper).GetNoticeNum(taobao_orderid, method);
        //        return r;
        //    }
        //}
        public int GetNoticeNum(string token)
        {
            using (var helper = new SqlHelper())
            {
                var r = new InternalTaobao_resend_noticelog(helper).GetNoticeNum(token);
                return r;
            }
        }
    }
}

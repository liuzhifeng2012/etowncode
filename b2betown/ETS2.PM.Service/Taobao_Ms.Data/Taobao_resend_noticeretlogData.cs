using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.Taobao_Ms.Model;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Taobao_Ms.Data.Internal;

namespace ETS2.PM.Service.Taobao_Ms.Data
{
    public class Taobao_resend_noticeretlogData
    {
        public int Editnoticeretlog(Taobao_resend_noticeretlog mretlog)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalTaobao_resend_noticeretlog(helper).Editnoticeretlog(mretlog);
                return r;
            }
        }
    }
}

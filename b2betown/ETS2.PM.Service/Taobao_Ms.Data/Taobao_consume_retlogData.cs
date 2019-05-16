using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.Taobao_Ms.Model;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Taobao_Ms.Data.Internal;

namespace ETS2.PM.Service.Taobao_Ms.Data
{
    public class Taobao_consume_retlogData
    {
        public int Editcomsumeretlog(Taobao_consume_retlog mretlog)
        {
             using(var helper=new SqlHelper())
             {
                 int r = new InternalTaobao_consume_retlog(helper).Editcomsumeretlog(mretlog);
                 return r;
             }
        }

        public Taobao_consume_retlog GetTaobao_consume_retlog(string qrcode, string num, string randomid)
        {
            using (var helper = new SqlHelper())
            {
                Taobao_consume_retlog r = new InternalTaobao_consume_retlog(helper).GetTaobao_consume_retlog(qrcode,num,randomid);
                return r;
            }
        }
    }
}

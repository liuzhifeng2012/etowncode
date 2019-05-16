using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Taobao_Ms.Model;
using ETS2.PM.Service.Taobao_Ms.Data.Internal;

namespace ETS2.PM.Service.Taobao_Ms.Data
{
    public class Taobao_reverse_retlogData
    {
        public int Editretlog(Taobao_reverse_retlog log)
        {
             using(var helper=new SqlHelper())
             {
                 int r = new InternalTaobao_reverse_retlog(helper).EditRetLog(log);
                 return r;
             }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Taobao_Ms.Data.Internal;
using ETS2.PM.Service.Taobao_Ms.Model;

namespace ETS2.PM.Service.Taobao_Ms.Data
{
    public class Taobao_ms_requestlogData
    {
        public int EditLog(Taobao_ms_requestlog log)
        {
            using (var helper=new  SqlHelper())
            {
                int r = new InternalTaobao_ms_requestlog(helper).EditLog(log);
                return r;
            }
        }
    }
}

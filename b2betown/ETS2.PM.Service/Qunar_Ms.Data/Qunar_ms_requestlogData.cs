using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.Qunar_Ms.Model;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Qunar_Ms.Data.Internal;

namespace ETS2.PM.Service.Qunar_Ms.Data
{
    public class Qunar_ms_requestlogData
    {
        public int EditQunar_ms_requestlog(Qunar_ms_requestlog rlog)
        {
            using (var helper = new SqlHelper())
            {
                int id = new InternalQunar_ms_requestlog(helper).EditQunar_ms_requestlog(rlog);
                return id;
            }
        }

        public bool IsHasNotice(string qunar_orderId, string method)
        {
            using (var helper = new SqlHelper())
            {
                bool r = new InternalQunar_ms_requestlog(helper).IsHasNotice(qunar_orderId, method);
                return r;
            }
        }



        public bool IsHasSuc(string method, string qunar_orderid)
        {
            using (var helper = new SqlHelper())
            {
                bool r = new InternalQunar_ms_requestlog(helper).IsHasSuc(method, qunar_orderid);
                return r;
            }
        }

        public Qunar_ms_requestlog GetLog(int id)
        {
             using(var helper=new SqlHelper())
             {
               Qunar_ms_requestlog log=  new InternalQunar_ms_requestlog(helper).GetLog(id);
                 return log;
             }
        }
    }
}

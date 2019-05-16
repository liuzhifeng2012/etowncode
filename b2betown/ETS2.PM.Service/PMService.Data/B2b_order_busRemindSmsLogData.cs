using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2b_order_busRemindSmsLogData
    {
        public B2b_order_busRemindSmsLog GetB2b_order_busRemindSmsSucLog(int proid, string gooutdate)
        {
            using(var helper=new SqlHelper())
            {
                B2b_order_busRemindSmsLog m = new Internalb2b_order_busRemindSmsLog(helper).GetB2b_order_busRemindSmsSucLog(proid, gooutdate);
                return m;
            }
        }

        public int EditRemindSmsLog(B2b_order_busRemindSmsLog m)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internalb2b_order_busRemindSmsLog(helper).EditRemindSmsLog(m);
                return r;
            }
        }

        public int UpRemindSmsLogState(string gooutdate, int proid, int issuc)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internalb2b_order_busRemindSmsLog(helper).UpRemindSmsLogState(gooutdate,proid,issuc);
                return r;
            }
        }
    }

    
}

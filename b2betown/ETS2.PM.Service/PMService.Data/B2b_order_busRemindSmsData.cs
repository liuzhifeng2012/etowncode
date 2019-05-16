using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2b_order_busRemindSmsData
    {

        public B2b_order_busRemindSms GetB2b_order_busRemindSms(int proid, string gooutdate)
        {
            using(var helper=new SqlHelper())
            {
                B2b_order_busRemindSms m = new Internalb2b_order_busRemindSms(helper).GetB2b_order_busRemindSms(proid,gooutdate);
                return m;
            }
        }

        public int EditRemindSms(B2b_order_busRemindSms m)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internalb2b_order_busRemindSms(helper).EditRemindSms(m);
                return r;
            }
        }
    }
}

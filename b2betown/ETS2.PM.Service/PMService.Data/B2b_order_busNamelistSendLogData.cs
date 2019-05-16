using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2b_order_busNamelistSendLogData
    {
        public int EditLog(B2b_order_busNamelistSendLog m)
        {
          using(var helper=new SqlHelper())
          {
              int r = new InternalB2b_order_busNamelistSendLog(helper).EditLog(m);
              return r;
          } 
        }

        public B2b_order_busNamelistSendLog GetB2b_order_busNamelistSendSucLog(int proid, string gooutdate)
        {
            using (var helper = new SqlHelper())
            {
                B2b_order_busNamelistSendLog r = new InternalB2b_order_busNamelistSendLog(helper).GetB2b_order_busNamelistSendSucLog(proid,gooutdate);
                return r;
            } 
        }
    }
}

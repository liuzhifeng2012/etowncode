using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2b_Rentserver_RefundLogData
    {
        public int EditServer_Refundlog(B2b_Rentserver_RefundLog server_refundlog)
        {
            using(var helper=new SqlHelper())
            {
                int r = new InternalB2b_Rentserver_RefundLog(helper).EditServer_Refundlog(server_refundlog);
                return r;
            }
        }

        public B2b_Rentserver_RefundLog GetServerRefundlog(int orderid, int rentserverid, int b2b_eticket_Depositid, int refundstate = -1)
        {
            using (var helper = new SqlHelper())
            {
                B2b_Rentserver_RefundLog r = new InternalB2b_Rentserver_RefundLog(helper).GetServerRefundlog(orderid, rentserverid,b2b_eticket_Depositid, refundstate);
                return r;
            }
        }

        public int Upb2b_Rentserver_RefundLogState(int logid, int refundstate)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalB2b_Rentserver_RefundLog(helper).Upb2b_Rentserver_RefundLogState(logid,refundstate);
                return r;
            }
        }
    }
}

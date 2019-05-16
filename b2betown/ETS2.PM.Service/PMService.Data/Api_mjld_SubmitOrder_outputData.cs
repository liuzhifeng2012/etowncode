using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class Api_mjld_SubmitOrder_outputData
    {
        public int EditApi_Mjld_SubmitOrder_output(Api_Mjld_SubmitOrder_output m)
        {
            using(var helper=new SqlHelper())
            {
                int r = new Internalapi_mjld_SubmitOrder_output(helper).EditApi_Mjld_SubmitOrder_output(m);
                return r;
            }
        }

        public Api_Mjld_SubmitOrder_output GetApi_Mjld_SubmitOrder_output(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                Api_Mjld_SubmitOrder_output r = new Internalapi_mjld_SubmitOrder_output(helper).GetApi_Mjld_SubmitOrder_output(orderid);
                return r;
            }
        }

        public string GetMjldinsureNo(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                string r = new Internalapi_mjld_SubmitOrder_output(helper).GetMjldinsureNo(orderid);
                return r;
            }
        }
    }
}

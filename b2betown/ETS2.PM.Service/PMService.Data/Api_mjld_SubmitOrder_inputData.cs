using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class Api_mjld_SubmitOrder_inputData
    {
        public int EditApi_mjld_SubmitOrder_input(Api_Mjld_SubmitOrder_input minput)
        {
             using(var helper=new SqlHelper())
             {
                 int id = new Internalapi_mjld_SubmitOrder_input(helper).EditApi_mjld_SubmitOrder_input(minput);
                 return id;
             }
        }

        public Api_Mjld_SubmitOrder_input GetApi_Mjld_SubmitOrder_input(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                Api_Mjld_SubmitOrder_input id = new Internalapi_mjld_SubmitOrder_input(helper).GetApi_Mjld_SubmitOrder_input(orderid);
                return id;
            }
        }
    }
}

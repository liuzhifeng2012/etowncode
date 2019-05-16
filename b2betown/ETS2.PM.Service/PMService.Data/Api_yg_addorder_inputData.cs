using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class Api_yg_addorder_inputData
    {
        public int EditApi_yg_addorder_input(Api_yg_addorder_input m)
        {
             using(var helper=new SqlHelper())
             {
                 int r = new Internalapi_yg_addorder_input(helper).EditApi_yg_addorder_input(m);
                 return r;
             }
        }

        public Api_yg_addorder_input Getapi_yg_addorder_input(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                Api_yg_addorder_input r = new Internalapi_yg_addorder_input(helper).Getapi_yg_addorder_input(orderid);
                return r;
            }
        }
    }
}

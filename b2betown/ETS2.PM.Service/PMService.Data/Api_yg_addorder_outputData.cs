using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class Api_yg_addorder_outputData
    {
        public int EditApi_yg_addorder_output(Api_yg_addorder_output mout)
        {
            using(var helper=new SqlHelper())
            {
                int r = new Internalapi_yg_addorder_output(helper).EditApi_yg_addorder_output(mout);
                return r;
            }
        }

        public string GetApi_yg_ordernum(int sysorderid)
        {
            using (var helper = new SqlHelper())
            {
                string r = new Internalapi_yg_addorder_output(helper).GetApi_yg_ordernum(sysorderid);
                return r;
            }
        }

        public Api_yg_addorder_output Getapi_yg_addorder_output(int sysorderid)
        {
            using (var helper = new SqlHelper())
            {
                Api_yg_addorder_output r = new Internalapi_yg_addorder_output(helper).Getapi_yg_addorder_output(sysorderid);
                return r;
            }
        }
    }
}

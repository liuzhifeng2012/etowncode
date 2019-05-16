using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class Api_yg_SyncordernoticeData
    {
        public int EditNotice(Api_yg_Syncordernotice notice)
        {
           using(var helper=new SqlHelper())
           {
               int r = new InternalApi_yg_Syncordernotice(helper).EditNotice(notice);
               return r;
           }
        }

        public Api_yg_Syncordernotice GetSucNotice(string platform_req_seq)
        {
            using (var helper = new SqlHelper())
            {
                Api_yg_Syncordernotice r = new InternalApi_yg_Syncordernotice(helper).GetSucNotice(platform_req_seq);
                return r;
            }
        }
    }
}

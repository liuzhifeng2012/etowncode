using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
    public class Wx_kfinsert_recordData
    {
        internal int Insertkfinsert_record(Wx_kfinsert_record kfinsert_record)
        {
             using(var helper=new SqlHelper())
             {
                 int r = new InternalWx_kfinsert_record(helper).Insertkfinsert_record(kfinsert_record);
                 return r;
             }
        }

        internal Wx_kfinsert_record GetLastRecord(string openid, int comid)
        {
            using (var helper = new SqlHelper())
            {
                Wx_kfinsert_record r = new InternalWx_kfinsert_record(helper).GetLastRecord(openid,comid);
                return r;
            }
        }
    }
}

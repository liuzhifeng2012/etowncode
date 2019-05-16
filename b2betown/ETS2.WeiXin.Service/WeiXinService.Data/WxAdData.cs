using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
    public class WxAdData
    {
        public int Editwxad(Wxad adinfo)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalWxAd(helper).Editwxad(adinfo);
                return r;
            }
        }

        public IList<Wxad> Getwxadpagelist(int pageindex, int pagesize, int comid, out int totalcount,string key="", int applystate = 0)
        {
            using (var helper = new SqlHelper())
            {
                List<Wxad> list = new InternalWxAd(helper).Getwxadpagelist(pageindex, pagesize, comid, out totalcount,key, applystate);
                return list;
            }
        }

        public int DelWxad(int id, int comid)
        {
            using (var helper = new SqlHelper())
            {
                int d = new InternalWxAd(helper).DelWxad(id, comid);
                return d;
            }
        }

        public int Wxadaddcount(int id, int comid,int vadd,int ladd)
        {
            using (var helper = new SqlHelper())
            {
                int d = new InternalWxAd(helper).Wxadaddcount(id, comid, vadd, ladd);
                return d;
            }
        }

        public Wxad Getwxad(int id, int comid)
        {
            using (var helper = new SqlHelper())
            {
                Wxad d = new InternalWxAd(helper).Getwxad(id, comid);
                return d;
            }
        }

    }
}

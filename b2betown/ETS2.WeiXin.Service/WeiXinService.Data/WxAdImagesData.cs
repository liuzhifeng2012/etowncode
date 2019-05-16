using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
    public class WxAdImagesData
    {
        public int Editwxadimage(WxAdImages adinfo)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalWxAdImages(helper).Editwxadimage(adinfo);
                return r;
            }
        }

        public IList<WxAdImages> Getwxadimagespagelist(int pageindex, int pagesize, int comid,int adid, out int totalcount, string key = "")
        {
            using (var helper = new SqlHelper())
            {
                List<WxAdImages> list = new InternalWxAdImages(helper).Getwxadimagespagelist(pageindex, pagesize, comid, adid, out totalcount, key);
                return list;
            }
        }

        public int DelWxadimages(int id, int adid)
        {
            using (var helper = new SqlHelper())
            {
                int d = new InternalWxAdImages(helper).DelWxadimages(id, adid);
                return d;
            }
        }


        public int upWxadimages_sort(int id, int sort)
        {
            using (var helper = new SqlHelper())
            {
                int d = new InternalWxAdImages(helper).upWxadimages_sort(id, sort);
                return d;
            }
        }

        public WxAdImages Getwxadimages(int id, int aid)
        {
            using (var helper = new SqlHelper())
            {
                WxAdImages d = new InternalWxAdImages(helper).Getwxadimages(id, aid);
                return d;
            }
        }
    }
}

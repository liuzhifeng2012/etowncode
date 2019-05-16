using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
    public class WxdelivernotifyData
    {
        public int EditWxdelivernotify(Wxdelivernotify m_d)
        {
             using(var helper=new  SqlHelper())
             {
                 int r = new InternalWxdelivernotify(helper).EditWxdelivernotify(m_d);
                 return r;
             }
        }
    }
}

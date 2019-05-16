using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
    public class WxMessageLogData
    {
        public int EditWxMessageLog(WxMessageLog log)
        {
            using (var helper = new SqlHelper())
            {
                int id = new InternalWxMessageLog(helper).EditWxMessageLog(log);
                return id;

            }
        }

        public int GetWxMessageLogSendTime(int comid,string weixin)
        {
            using (var helper = new SqlHelper())
            {
                int id = new InternalWxMessageLog(helper).GetWxMessageLogSendTime(comid,weixin);
                return id;

            }
        }

    }
}

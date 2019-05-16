using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Data.InternalData;

namespace ETS2.WeiXin.Service.WeiXinService.Data
{
    public class WXAccessTokenData
    {
        public  WXAccessToken GetLaststWXAccessToken(DateTime fitcreatetime,int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalWXAccessToken(sql);
                    WXAccessToken result = internalData.GetLaststWXAccessToken(fitcreatetime,comid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #region 编辑凭证信息
        public int EditAccessToken(WXAccessToken accesstoken)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalWXAccessToken(sql);
                    int result = internalData.EditAccessToken(accesstoken);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion 
    }
}

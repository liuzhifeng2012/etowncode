using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class Api_mjld_AsyncUsenoticeData
    {
        /// <summary>
        /// 根据平台验证id 得到验证通知
        /// </summary>
        /// <param name="exchangeId"></param>
        /// <returns></returns>
        public Api_mjld_AsyncUsenotice GetSucUseNoticeByExchangeId(string exchangeId)
        {
            using(var helper=new SqlHelper())
            {
                Api_mjld_AsyncUsenotice m = new Internalapi_mjld_AsyncUsenotice(helper).GetSucUseNoticeByExchangeId(exchangeId);
                return m;
            }
        }

        public int EditUsenotice(Api_mjld_AsyncUsenotice usenotice)
        {
            using (var helper = new SqlHelper())
            {
                int m = new Internalapi_mjld_AsyncUsenotice(helper).EditUsenotice(usenotice);
                return m;
            }
        }
    }
}

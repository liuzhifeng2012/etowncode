using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class B2bEticketSendLogData
    {
        #region 发送电子记录
        public int InsertOrUpdate(B2b_eticket_send_log eticket)
        {

            using (var helper = new SqlHelper())
            {
                return new InternalB2bEticketSendLog(helper).InsertOrUpdate(eticket);
            }
        }
        #endregion

    }
}

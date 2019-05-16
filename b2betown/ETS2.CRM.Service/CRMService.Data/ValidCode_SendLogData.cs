using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Data.InternalData;

namespace ETS2.CRM.Service.CRMService.Data
{
    public class ValidCode_SendLogData
    {
        public ValidCode_SendLog GetLasterLogByMobile(string mobile, string source)
        {
            using (var helper = new SqlHelper())
            {
                ValidCode_SendLog r = new InternalValidCode_SendLog(helper).GetLasterLogByMobile(mobile,source);
                return r;
            }
        }

        public int InsertLog(ValidCode_SendLog log)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalValidCode_SendLog(helper).InsertLog(log);
                return r;
            }
        }

        public int GetSendNumIn30ByMobile(string mobile, string source)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalValidCode_SendLog(helper).GetSendNumIn30ByMobile(mobile,source);
                return r;
            }
        }

        public DateTime GetFirstsendtimeByCode(string mobile, string source, decimal code)
        {
            using (var helper = new SqlHelper())
            {
                DateTime r = new InternalValidCode_SendLog(helper).GetFirstsendtimeByCode(mobile, source,code);
                return r;
            }
        }
    }
}

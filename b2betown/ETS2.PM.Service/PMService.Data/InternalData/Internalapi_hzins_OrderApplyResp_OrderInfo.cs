using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalapi_hzins_OrderApplyResp_OrderInfo
    {
        public SqlHelper sqlHelper;
        public Internalapi_hzins_OrderApplyResp_OrderInfo(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal int EditOrderApplyResp_OrderInfo(Api_hzins_OrderApplyResp_OrderInfo m)
        {
            if (m.id > 0)
            {
                return 0;
            }
            else
            {
                string sql = @"INSERT INTO  [api_hzins_OrderApplyResp_OrderInfo]
                                   ([orderid]
                                   ,[insureNum]
                                   ,[policyNum]
                                   ,[cName]
                                   ,[cardCode]
                                   ,[issueState])
                             VALUES
                                   (@orderid 
                                   ,@insureNum 
                                   ,@policyNum 
                                   ,@cName 
                                   ,@cardCode 
                                   ,@issueState);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@orderid", m.orderid);
                cmd.AddParam("@insureNum", m.insureNum);
                cmd.AddParam("@policyNum", m.policyNum);
                cmd.AddParam("@cName", m.cName);
                cmd.AddParam("@cardCode", m.cardCode);
                cmd.AddParam("@issueState", m.issueState);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalB2bEticketSendLog
    {
        private SqlHelper sqlHelper;
        public InternalB2bEticketSendLog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        #region 发送电子票电子票信息
        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateEticketsend";//调用存储过程，
        internal int InsertOrUpdate(B2b_eticket_send_log model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);

            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@eticket_id", model.Eticket_id);
            cmd.AddParam("@pnotext", model.Pnotext);
            cmd.AddParam("@phone", model.Phone);
            cmd.AddParam("@sendstate", model.Sendstate);
            cmd.AddParam("@sendtype", model.Sendtype);
            cmd.AddParam("@senddate", model.Senddate);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion

    }
}

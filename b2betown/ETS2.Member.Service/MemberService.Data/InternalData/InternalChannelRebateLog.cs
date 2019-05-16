using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.Member.Service.MemberService.Model;
using ETS.Data.SqlHelper;

namespace ETS2.Member.Service.MemberService.Data.InternalData
{
    class InternalChannelRebateLog
    {
        private SqlHelper sqlHelper;
        public InternalChannelRebateLog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        #region 添加或编辑卡号管理

        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateChannelRebateLog";
        internal int EditChannelRebateLog(ChannelRebateLog model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);

            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Channelid", model.Channelid);
            cmd.AddParam("@Type", model.Type);
            cmd.AddParam("@Rebatemoney", model.Rebatemoney);
            cmd.AddParam("@Summoney", model.Summoney);
            cmd.AddParam("@Execdate", model.Execdate);
            cmd.AddParam("@Remark", model.Remark);


            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion
    }
}

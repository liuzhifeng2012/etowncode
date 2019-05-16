using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{

    public class InternalPosversionrenewlog
    {
        private SqlHelper sqlHelper;
        public InternalPosversionrenewlog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        #region 添加或者编辑pos机版本更新日志信息

        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdatePosversionrenewlog";

        public int InsertOrUpdate(Posversionrenewlog model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@PosId", model.Posid);
            cmd.AddParam("@Initversionno", model.Initversionno);
            cmd.AddParam("@Newversionno", model.Newversionno);
            cmd.AddParam("@Updatefileurl", model.Updatefileurl);
            cmd.AddParam("@Updatetype", model.Updatetype);
            cmd.AddParam("@Updatetime", model.Updatetime);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion
    }
}

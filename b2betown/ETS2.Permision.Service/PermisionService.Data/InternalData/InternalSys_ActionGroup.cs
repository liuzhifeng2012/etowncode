using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;

namespace ETS2.Permision.Service.PermisionService.Data.InternalData
{
    public class InternalSys_ActionGroup
    {
        private SqlHelper sqlHelper;
        public InternalSys_ActionGroup(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        private static readonly string SQLInsertOrUpdate2 = "usp_DistributeAction";
        internal int DistributeAction(int groupid, string selednodeid, int createuserid, string createusername, DateTime createdate)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate2);
            cmd.AddParam("@GroupId", groupid);
            cmd.AddParam("@SeledNodeIdArr", selednodeid);
            cmd.AddParam("@CreateMasterId", createuserid);
            cmd.AddParam("@CreateMasterName", createusername);
            cmd.AddParam("@CreateDate", createdate);


            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class InternalWxqunfa_wxno
    {
        private SqlHelper sqlHelper;
        public InternalWxqunfa_wxno(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal int InsWxno(int logid, string weixin)
        {
            string sql = "insert into  wxqunfa_wxno(weixin,logid) values(@weixin,@logid)";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@logid", logid);
            cmd.AddParam("@weixin", weixin);


            return cmd.ExecuteNonQuery();


        }
    }
}

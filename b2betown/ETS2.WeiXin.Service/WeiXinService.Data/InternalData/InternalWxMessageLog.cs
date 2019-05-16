using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.Member.Service.MemberService.Data;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class InternalWxMessageLog
    {
        private SqlHelper sqlHelper;
        public InternalWxMessageLog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        //插入发送日志
        internal int EditWxMessageLog(WxMessageLog log)
        {
            string sql = @"INSERT [dbo].[WxMessageLog]
           ([comid],[weixin])
     VALUES
           (@comid
           ,@weixin
          );select @@IDENTITY;";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", log.Comid);
            cmd.AddParam("@weixin", log.Weixin);
           
            object o = cmd.ExecuteScalar();
            return o == null ? 0 : int.Parse(o.ToString());
        }


        internal int GetWxMessageLogSendTime(int comid,string weixin)
        {
            try
            {
                string sql = "select TOP 1 id from WxMessageLog where  [comid]=" + comid + " and weixin='" + weixin + "' and sendtime > dateadd(\"hh\",-2,getdate())";

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            catch 
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

    }
}

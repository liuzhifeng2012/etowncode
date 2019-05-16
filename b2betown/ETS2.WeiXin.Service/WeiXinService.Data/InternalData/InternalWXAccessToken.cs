using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.WeiXin.Service.WeiXinService.Model;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class InternalWXAccessToken
    {
        private SqlHelper sqlHelper;
        public InternalWXAccessToken(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal WXAccessToken GetLaststWXAccessToken(DateTime fitcreatetime,int comid)
        {

            const string sqltxt = @"SELECT  top 1  [id]
                                      ,[ACCESS_TOKEN]
                                      ,[createdate]
                                      ,[comid]
                                  FROM [EtownDB].[dbo].[WXAccessToken] where createdate>@fitcreatetime and comid=@comid order by createdate desc";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@fitcreatetime", fitcreatetime);
            cmd.AddParam("@comid", comid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new WXAccessToken
                    {
                        Id = reader.GetValue<int>("id"),
                        ACCESS_TOKEN = reader.GetValue<string>("ACCESS_TOKEN"),
                        CreateDate = reader.GetValue<DateTime>("createdate"),


                    };
                }
                return null;
            }
        }
        #region 编辑微信凭证信息
        internal int EditAccessToken(WXAccessToken accesstoken)
        {
            string sql = "INSERT INTO [EtownDB].[dbo].[WXAccessToken]([ACCESS_TOKEN],[createdate],[comid])VALUES('" + accesstoken.ACCESS_TOKEN + "','" + accesstoken.CreateDate + "',"+accesstoken.Comid+")";
            if (accesstoken.Id > 0)
            {
                sql = "UPDATE [EtownDB].[dbo].[WXAccessToken]   SET [ACCESS_TOKEN] = '"+accesstoken.ACCESS_TOKEN+"',[createdate] = '"+accesstoken.CreateDate+"',comid="+accesstoken.Comid+"  WHERE id="+accesstoken.Id;            
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return (int)cmd.ExecuteNonQuery();
        }

        #endregion
    }
}

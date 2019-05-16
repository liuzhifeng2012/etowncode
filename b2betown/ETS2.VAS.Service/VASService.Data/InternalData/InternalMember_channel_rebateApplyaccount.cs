using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.VAS.Service.VASService.Modle;

namespace ETS2.VAS.Service.VASService.Data.InternalData
{
    public class InternalMember_channel_rebateApplyaccount
    {
        public SqlHelper sqlHelper;
        public InternalMember_channel_rebateApplyaccount(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal Member_channel_rebateApplyaccount GetchanelrebateApplyaccount(int channelid)
        {
            string sql = "select * from Member_channel_rebateApplyaccount where channelid=" + channelid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                Member_channel_rebateApplyaccount m = null;
                if (reader.Read())
                {
                    m = new Member_channel_rebateApplyaccount
                    {
                        id = reader.GetValue<int>("id"),
                        channelid = reader.GetValue<int>("channelid"),
                        truename = reader.GetValue<string>("truename"),
                        alipayaccount = reader.GetValue<string>("alipayaccount"),
                        alipayphone = reader.GetValue<string>("alipayphone"),
                        accountstatus = reader.GetValue<int>("accountstatus"),
                        comid = reader.GetValue<int>("comid"),
                    };
                }
                return m;
            }
        }

        internal int Upchannelrebateaccount(int channelid, string truename, string account, string newphone,int comid)
        {
            string sql = "select count(1) from Member_channel_rebateApplyaccount where channelid>0 and channelid=" + channelid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            int num = int.Parse(o.ToString());

            if (num > 0)
            {
                //编辑
                string sql2 = @"UPDATE  [Member_channel_rebateApplyaccount]
                               SET  [truename] = @truename 
                                  ,[alipayaccount] = @alipayaccount 
                                  ,[alipayphone] = @alipayphone 
                                  ,comid=@comid 
                             WHERE channelid=@channelid";
                var cmd2 = sqlHelper.PrepareTextSqlCommand(sql2);
                cmd2.AddParam("@truename", truename);
                cmd2.AddParam("@alipayaccount", account);
                cmd2.AddParam("@alipayphone", newphone);
                cmd2.AddParam("@comid", comid);
                cmd2.AddParam("@channelid", channelid);
                return cmd2.ExecuteNonQuery();
            }
            else
            {
                //增加
                string sql2 = @"INSERT INTO  [Member_channel_rebateApplyaccount]
                                   ([channelid]
                                   ,[truename]
                                   ,[alipayaccount]
                                   ,[alipayphone]
                                   ,[accountstatus]
                                    ,comid)
                             VALUES
                                   (@channelid 
                                   ,@truename 
                                   ,@alipayaccount 
                                   ,@alipayphone 
                                   ,1
                                   ,@comid)";
                var cmd2 = sqlHelper.PrepareTextSqlCommand(sql2);
                cmd2.AddParam("@channelid", channelid);
                cmd2.AddParam("@truename", truename);
                cmd2.AddParam("@alipayaccount", account);
                cmd2.AddParam("@alipayphone", newphone);
                cmd2.AddParam("@comid", comid);
                return cmd2.ExecuteNonQuery();
            }
        }
    }
}

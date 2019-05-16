using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS.Data.SqlHelper;

namespace ETS2.WeiXin.Service.WeiXinService.Data.InternalData
{
    public class InternalWeixin_templatemsg_sendlog
    {
        public SqlHelper sqlHelper;
        public InternalWeixin_templatemsg_sendlog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal int EditTmplLog(Weixin_templatemsg_sendlog m)
        {
            if (m.Id == 0)
            {
                string sql = @"INSERT INTO [EtownDB].[dbo].[weixin_templatemsg_sendlog](
            [msg_send_content],
            [touser],
            [template_id],
            [url],
            [msg_send_createtime],
            [msg_call_content],
            [msg_call_errcode],
            [msg_call_errmsg],
            [msgid]
           ,[msg_push_content]
           ,[msg_push_CreateTime]
           ,[msg_push_CreateTime_format]
           ,[msg_push_status]
           ,[orderid]
           ,[public_account]
           ,[comid]
           ,[remark]
           ,infotype)
     VALUES
           (@msg_send_content
           ,@touser
           ,@template_id
           ,@url
           ,@msg_send_createtime
           ,@msg_call_content
           ,@msg_call_errcode
           ,@msg_call_errmsg
           ,@msgid
           ,@msg_push_content
           ,@msg_push_CreateTime
           ,@msg_push_CreateTime_format
           ,@msg_push_status
           ,@orderid
           ,@public_account
           ,@comid
           ,@remark
           ,@infotype);select @@IDENTITY;";

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@msg_send_content", m.Msg_send_content);
                cmd.AddParam("@touser", m.Touser);
                cmd.AddParam("@template_id", m.Template_id);
                cmd.AddParam("@url", m.Url);
                cmd.AddParam("@msg_send_createtime", m.Msg_send_createtime);
                cmd.AddParam("@msg_call_content", m.Msg_call_content);
                cmd.AddParam("@msg_call_errcode", m.Msg_call_errcode);
                cmd.AddParam("@msg_call_errmsg", m.Msg_call_errmsg);
                cmd.AddParam("@msgid", m.Msgid);
                cmd.AddParam("@msg_push_content", m.Msg_push_content);
                cmd.AddParam("@msg_push_CreateTime", m.Msg_push_CreateTime);
                cmd.AddParam("@msg_push_CreateTime_format", m.Msg_push_CreateTime_format);
                cmd.AddParam("@msg_push_status", m.Msg_push_status);
                cmd.AddParam("@orderid", m.Orderid);
                cmd.AddParam("@public_account", m.Public_account);
                cmd.AddParam("@comid", m.Comid);
                cmd.AddParam("@remark", m.Remark);
                cmd.AddParam("@infotype", m.Infotype);

                object o = cmd.ExecuteScalar();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            else
            {
                string sql = @"UPDATE [EtownDB].[dbo].[weixin_templatemsg_sendlog]
   SET [msg_send_content] = @msg_send_content 
      ,[touser] = @touser 
      ,[template_id] = @template_id 
      ,[url] = @url 
      ,[msg_send_createtime] = @msg_send_createtime 
      ,[msg_call_content] = @msg_call_content 
      ,[msg_call_errcode] = @msg_call_errcode 
      ,[msg_call_errmsg] = @msg_call_errmsg 
      ,[msgid] = @msgid 
      ,[msg_push_content] = @msg_push_content 
      ,[msg_push_CreateTime] = @msg_push_CreateTime 
      ,[msg_push_CreateTime_format] = @msg_push_CreateTime_format 
      ,[msg_push_status] = @msg_push_status 
      ,[orderid] = @orderid 
      ,[public_account] = @public_account 
      ,[comid] = @comid 
      ,[remark] = @remark 
      ,infotype=@infotype
 WHERE  id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", m.Id);
                cmd.AddParam("@msg_send_content", m.Msg_send_content);
                cmd.AddParam("@touser", m.Touser);
                cmd.AddParam("@template_id", m.Template_id);
                cmd.AddParam("@url", m.Url);
                cmd.AddParam("@msg_send_createtime", m.Msg_send_createtime);
                cmd.AddParam("@msg_call_content", m.Msg_call_content);
                cmd.AddParam("@msg_call_errcode", m.Msg_call_errcode);
                cmd.AddParam("@msg_call_errmsg", m.Msg_call_errmsg);
                cmd.AddParam("@msgid", m.Msgid);
                cmd.AddParam("@msg_push_content", m.Msg_push_content);
                cmd.AddParam("@msg_push_CreateTime", m.Msg_push_CreateTime);
                cmd.AddParam("@msg_push_CreateTime_format", m.Msg_push_CreateTime_format);
                cmd.AddParam("@msg_push_status", m.Msg_push_status);
                cmd.AddParam("@orderid", m.Orderid);
                cmd.AddParam("@public_account", m.Public_account);
                cmd.AddParam("@comid", m.Comid);
                cmd.AddParam("@remark", m.Remark);
                cmd.AddParam("@infotype", m.Infotype);

                cmd.ExecuteNonQuery();
                return m.Id;
            }
        }

        internal bool Issendsuc(int orderid, string Template_id)
        {
            string sql = "select count(1) from weixin_templatemsg_sendlog where msg_push_status='success' and  orderid=" + orderid + "  and template_id='" + Template_id + "'";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            if (int.Parse(o.ToString()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal Weixin_templatemsg_sendlog GetTmplLogByMsgId(string msgid)
        {
            string sql = @"SELECT [id]
      ,[msg_send_content]
      ,[touser]
      ,[template_id]
      ,[url]
      ,[msg_send_createtime]
      ,[msg_call_content]
      ,[msg_call_errcode]
      ,[msg_call_errmsg]
      ,[msgid]
      ,[msg_push_content]
      ,[msg_push_CreateTime]
      ,[msg_push_CreateTime_format]
      ,[msg_push_status]
      ,[orderid]
      ,[public_account]
      ,[comid]
      ,[remark]
      ,[infotype]
  FROM [EtownDB].[dbo].[weixin_templatemsg_sendlog] where msgid=@msgid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@msgid", msgid);

            using (var reader = cmd.ExecuteReader())
            {
                Weixin_templatemsg_sendlog m = null;
                if (reader.Read())
                {
                    m = new Weixin_templatemsg_sendlog
                    {
                        Id = reader.GetValue<int>("id"),
                        Msg_send_content = reader.GetValue<string>("msg_send_content"),
                        Touser = reader.GetValue<string>("touser"),
                        Template_id = reader.GetValue<string>("template_id"),
                        Url = reader.GetValue<string>("url"),
                        Msg_send_createtime = reader.GetValue<DateTime>("msg_send_createtime"),
                        Msg_call_content = reader.GetValue<string>("msg_call_content"),
                        Msg_call_errcode = reader.GetValue<int>("msg_call_errcode").ToString(),
                        Msg_call_errmsg = reader.GetValue<string>("msg_call_errmsg"),
                        Msgid = reader.GetValue<string>("msgid"),
                        Msg_push_content = reader.GetValue<string>("msg_push_content"),
                        Msg_push_CreateTime = reader.GetValue<string>("msg_push_CreateTime"),
                        Msg_push_CreateTime_format = reader.GetValue<DateTime>("msg_push_CreateTime_format"),
                        Msg_push_status = reader.GetValue<string>("msg_push_status"),
                        Orderid = reader.GetValue<int>("orderid"),
                        Public_account = reader.GetValue<string>("public_account"),
                        Comid = reader.GetValue<int>("comid"),
                        Remark = reader.GetValue<string>("remark"),
                        Infotype = reader.GetValue<string>("infotype"),

                    };
                }
                return m;
            }
        }
    }
}

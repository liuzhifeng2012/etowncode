using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Taobao_Ms.Model;

namespace ETS2.PM.Service.Taobao_Ms.Data.Internal
{
    public class InternalTaobao_resend_noticelog
    {
         public SqlHelper sqlHelper;
         public InternalTaobao_resend_noticelog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

         internal int Editnoticelog(Taobao_resend_noticelog log)
         {
             if (log.id == 0)
             {
                 string sql = @"INSERT INTO  [taobao_resend_noticelog]
           ([timestamp]
           ,[sign]
           ,[order_id]
           ,[mobile]
           ,[num]
,[left_num]
           ,[method]
           ,[taobao_sid]
           ,[seller_nick]
           ,[item_title]
           ,[send_type]
           ,[consume_type]
           ,[sms_template]
           ,[valid_start]
           ,[valid_ends]
           ,[num_iid]
           ,[outer_iid]
           ,[sub_outer_iid]
           ,[sku_properties]
           ,[token]
           
           ,[subtime]
           ,[responsecode]
           ,[responsetime]
           ,[self_order_id]
           ,[agentid]
           ,[errmsg]
           ,type
           ,encrypt_mobile
           ,md5_mobile)
     VALUES
           (@timestamp 
           ,@sign 
           ,@order_id 
           ,@mobile 
           ,@num
,@left_num
           ,@method 
           ,@taobao_sid 
           ,@seller_nick 
           ,@item_title 
           ,@send_type 
           ,@consume_type 
           ,@sms_template 
           ,@valid_start 
           ,@valid_ends 
           ,@num_iid 
           ,@outer_iid 
           ,@sub_outer_iid 
           ,@sku_properties 
           ,@token 
            
           ,@subtime 
           ,@responsecode 
           ,@responsetime 
           ,@self_order_id 
           ,@agentid 
           ,@errmsg
           ,@type
           ,@encrypt_mobile
           ,@md5_mobile);select @@identity;";
                 var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                 cmd.AddParam("@timestamp", log.timestamp);
                 cmd.AddParam("@sign", log.sign);
                 cmd.AddParam("@order_id", log.order_id);
                 cmd.AddParam("@mobile", log.mobile);
                 cmd.AddParam("@num", log.num);
                 cmd.AddParam("@left_num",log.left_num);
                 cmd.AddParam("@method", log.method);
                 cmd.AddParam("@taobao_sid", log.taobao_sid);
                 cmd.AddParam("@seller_nick", log.seller_nick);
                 cmd.AddParam("@item_title", log.item_title);
                 cmd.AddParam("@send_type", log.send_type);
                 cmd.AddParam("@consume_type", log.consume_type);
                 cmd.AddParam("@sms_template", log.sms_template);
                 cmd.AddParam("@valid_start", log.valid_start);
                 cmd.AddParam("@valid_ends", log.valid_ends);
                 cmd.AddParam("@num_iid", log.num_iid);
                 cmd.AddParam("@outer_iid", log.outer_iid);
                 cmd.AddParam("@sub_outer_iid", log.sub_outer_iid);
                 cmd.AddParam("@sku_properties", log.sku_properties);
                 cmd.AddParam("@token", log.token);
            
                 cmd.AddParam("@subtime", log.subtime);
                 cmd.AddParam("@responsecode", log.responsecode);
                 cmd.AddParam("@responsetime", log.responsetime);
                 cmd.AddParam("@self_order_id", log.self_order_id);
                 cmd.AddParam("@agentid", log.agentid);
                 cmd.AddParam("@errmsg", log.errmsg);
                 cmd.AddParam("@type", log.type);
                 cmd.AddParam("@encrypt_mobile", log.encrypt_mobile);
                 cmd.AddParam("@md5_mobile", log.md5_mobile);
                 try
                 {
                     object o = cmd.ExecuteScalar();
                     return int.Parse(o.ToString());
                 }
                 catch
                 {
                     sqlHelper.Dispose();
                     return 0;
                 }
             }
             else
             {
                 string sql = @"UPDATE  [taobao_resend_noticelog]
   SET [timestamp] = @timestamp 
      ,[sign] = @sign 
      ,[order_id] = @order_id 
      ,[mobile] = @mobile 
      ,[num] = @num 
,left_num=@left_num
      ,[method] = @method 
      ,[taobao_sid] = @taobao_sid 
      ,[seller_nick] = @seller_nick 
      ,[item_title] = @item_title 
      ,[send_type] = @send_type 
      ,[consume_type] = @consume_type 
      ,[sms_template] = @sms_template 
      ,[valid_start] = @valid_start 
      ,[valid_ends] = @valid_ends 
      ,[num_iid] = @num_iid 
      ,[outer_iid] = @outer_iid 
      ,[sub_outer_iid] = @sub_outer_iid 
      ,[sku_properties] = @sku_properties 
      ,[token] = @token 
 
      ,[subtime] = @subtime 
      ,[responsecode] = @responsecode 
      ,[responsetime] = @responsetime 
      ,[self_order_id] = @self_order_id 
      ,[agentid] = @agentid 
      ,[errmsg] = @errmsg 
      ,type=@type
      ,encrypt_mobile=@encrypt_mobile
      ,md5_mobile=@md5_mobile 
 WHERE id=@id";
                 var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                 cmd.AddParam("@id", log.id);
                 cmd.AddParam("@timestamp", log.timestamp);
                 cmd.AddParam("@sign", log.sign);
                 cmd.AddParam("@order_id", log.order_id);
                 cmd.AddParam("@mobile", log.mobile);
                 cmd.AddParam("@num", log.num);
                 cmd.AddParam("@left_num", log.left_num);
                 cmd.AddParam("@method", log.method);
                 cmd.AddParam("@taobao_sid", log.taobao_sid);
                 cmd.AddParam("@seller_nick", log.seller_nick);
                 cmd.AddParam("@item_title", log.item_title);
                 cmd.AddParam("@send_type", log.send_type);
                 cmd.AddParam("@consume_type", log.consume_type);
                 cmd.AddParam("@sms_template", log.sms_template);
                 cmd.AddParam("@valid_start", log.valid_start);
                 cmd.AddParam("@valid_ends", log.valid_ends);
                 cmd.AddParam("@num_iid", log.num_iid);
                 cmd.AddParam("@outer_iid", log.outer_iid);
                 cmd.AddParam("@sub_outer_iid", log.sub_outer_iid);
                 cmd.AddParam("@sku_properties", log.sku_properties);
                 cmd.AddParam("@token", log.token);
          
                 cmd.AddParam("@subtime", log.subtime);
                 cmd.AddParam("@responsecode", log.responsecode);
                 cmd.AddParam("@responsetime", log.responsetime);
                 cmd.AddParam("@self_order_id", log.self_order_id);
                 cmd.AddParam("@agentid", log.agentid);
                 cmd.AddParam("@errmsg", log.errmsg);
                 cmd.AddParam("@type", log.type);
                 cmd.AddParam("@encrypt_mobile", log.encrypt_mobile);
                 cmd.AddParam("@md5_mobile", log.md5_mobile);
                 cmd.ExecuteNonQuery();
                 return log.id;
             }
         }

         //internal int GetNoticeNum(string taobao_orderid, string method)
         //{
         //    string sql = "select count(1) from taobao_resend_noticelog where order_id='" + taobao_orderid + "' and method='" + method + "'";
         //    var cmd = sqlHelper.PrepareTextSqlCommand(sql);
         //    object o = cmd.ExecuteScalar();
         //    return int.Parse(o.ToString());
         //}
         internal int GetNoticeNum(string token)
         {
             string sql = "select count(1) from taobao_resend_noticelog where token='" + token + "'";
             var cmd = sqlHelper.PrepareTextSqlCommand(sql);
             object o = cmd.ExecuteScalar();
             return int.Parse(o.ToString());
         }
    }
}

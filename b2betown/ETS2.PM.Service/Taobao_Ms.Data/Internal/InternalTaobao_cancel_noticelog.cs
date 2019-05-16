using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.Taobao_Ms.Data.Internal
{
    public class InternalTaobao_cancel_noticelog
    {
          public SqlHelper sqlHelper;
          public InternalTaobao_cancel_noticelog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }


          internal int Editnoticelog(Model.Taobao_cancel_noticelog log)
          {
              if (log.id == 0)
              {
                  string sql = @"INSERT INTO   taobao_cancel_noticelog
           ([timestamp]
           ,[sign]
           ,[order_id]
           ,[mobile]
           ,[num]
,cancel_num
           ,[method]
           ,[taobao_sid]
           ,[seller_nick]
           ,[item_title]
           ,[send_type]
           ,[consume_type]
           
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
           ,[errmsg])
     VALUES
           (@timestamp 
           ,@sign 
           ,@order_id 
           ,@mobile 
           ,@num
,@cancel_num
           ,@method 
           ,@taobao_sid 
           ,@seller_nick 
           ,@item_title 
           ,@send_type 
           ,@consume_type 
 
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
           ,@errmsg );select @@identity;";
                  var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                  cmd.AddParam("@timestamp", log.timestamp);
                  cmd.AddParam("@sign", log.sign);
                  cmd.AddParam("@order_id", log.order_id);
                  cmd.AddParam("@mobile", log.mobile);
                  cmd.AddParam("@num", log.num);
                  cmd.AddParam("@cancel_num", log.cancel_num);
                  cmd.AddParam("@method", log.method);
                  cmd.AddParam("@taobao_sid", log.taobao_sid);
                  cmd.AddParam("@seller_nick", log.seller_nick);
                  cmd.AddParam("@item_title", log.item_title);
                  cmd.AddParam("@send_type", log.send_type);
                  cmd.AddParam("@consume_type", log.consume_type);
   
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
                  string sql = @"UPDATE  taobao_cancel_noticelog
   SET [timestamp] = @timestamp 
      ,[sign] = @sign 
      ,[order_id] = @order_id 
      ,[mobile] = @mobile 
      ,[num] = @num 
,cancel_num=@cancel_num
      ,[method] = @method 
      ,[taobao_sid] = @taobao_sid 
      ,[seller_nick] = @seller_nick 
      ,[item_title] = @item_title 
      ,[send_type] = @send_type 
      ,[consume_type] = @consume_type 
   
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
 WHERE id=@id";
                  var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                  cmd.AddParam("@id", log.id);
                  cmd.AddParam("@timestamp", log.timestamp);
                  cmd.AddParam("@sign", log.sign);
                  cmd.AddParam("@order_id", log.order_id);
                  cmd.AddParam("@mobile", log.mobile);
                  cmd.AddParam("@num", log.num);
                  cmd.AddParam("@cancel_num", log.cancel_num);
                  cmd.AddParam("@method", log.method);
                  cmd.AddParam("@taobao_sid", log.taobao_sid);
                  cmd.AddParam("@seller_nick", log.seller_nick);
                  cmd.AddParam("@item_title", log.item_title);
                  cmd.AddParam("@send_type", log.send_type);
                  cmd.AddParam("@consume_type", log.consume_type);
          
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
                  cmd.ExecuteNonQuery();
                  return log.id;
              }
          }

          internal int GetNoticeNum(string token)
          {
              string sql = "select count(1) from taobao_cancel_noticelog where token='"+token+"'";
              var cmd = sqlHelper.PrepareTextSqlCommand(sql);
              object o = cmd.ExecuteScalar();
              return int.Parse(o.ToString());
          }
    }
}

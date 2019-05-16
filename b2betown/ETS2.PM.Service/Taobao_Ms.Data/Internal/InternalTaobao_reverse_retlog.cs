using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Taobao_Ms.Model;

namespace ETS2.PM.Service.Taobao_Ms.Data.Internal
{
    public  class InternalTaobao_reverse_retlog
    {
        public SqlHelper sqlHelper;
        public InternalTaobao_reverse_retlog(SqlHelper sqlHelper) 
        {
            this.sqlHelper = sqlHelper;
        }
        internal int EditRetLog(Taobao_reverse_retlog log)    
        {
            if (log.id == 0)
            {
                string sql = @"INSERT INTO  [taobao_reverse_retlog]
           ([order_id]
           ,[reverse_code]
           ,[reverse_num]
           ,[consume_secial_num]
           ,[verify_codes]
           ,[qr_images]
           ,[token]
           ,[codemerchant_id]
           ,[posid]
           ,[ret_code]
           ,[item_title]
           ,[left_num]
           ,[ret_time])
     VALUES
           (@order_id 
           ,@reverse_code 
           ,@reverse_num 
           ,@consume_secial_num 
           ,@verify_codes 
           ,@qr_images 
           ,@token 
           ,@codemerchant_id 
           ,@posid 
           ,@ret_code 
           ,@item_title 
           ,@left_num 
           ,@ret_time );select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@order_id",log.order_id);
                cmd.AddParam("@reverse_code",log.reverse_code);
                cmd.AddParam("@reverse_num",log.reverse_num);
                cmd.AddParam("@consume_secial_num",log.consume_secial_num);
                cmd.AddParam("@verify_codes",log.verify_codes);
                cmd.AddParam("@qr_images",log.qr_images);
                cmd.AddParam("@token",log.token);
                cmd.AddParam("@codemerchant_id",log.codemerchant_id);
                cmd.AddParam("@posid",log.posid);
                cmd.AddParam("@ret_code",log.ret_code);
                cmd.AddParam("@item_title",log.item_title);
                cmd.AddParam("@left_num",log.left_num);
                cmd.AddParam("@ret_time", log.ret_time);

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
                 string sql = @"UPDATE  [taobao_reverse_retlog]
                               SET [order_id] = @order_id 
                                  ,[reverse_code] = @reverse_code 
                                  ,[reverse_num] = @reverse_num 
                                  ,[consume_secial_num] = @consume_secial_num
                                  ,[verify_codes] = @verify_codes
                                  ,[qr_images] = @qr_images 
                                  ,[token] = @token 
                                  ,[codemerchant_id] = @codemerchant_id 
                                  ,[posid] = @posid 
                                  ,[ret_code] = @ret_code 
                                  ,[item_title] = @item_title 
                                  ,[left_num] = @left_num 
                                  ,[ret_time] = @ret_time 
                             WHERE id=@id;";
                 var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                 cmd.AddParam("@id", log.id);
                 cmd.AddParam("@order_id", log.order_id);
                 cmd.AddParam("@reverse_code", log.reverse_code);
                 cmd.AddParam("@reverse_num", log.reverse_num);
                 cmd.AddParam("@consume_secial_num", log.consume_secial_num);
                 cmd.AddParam("@verify_codes", log.verify_codes);
                 cmd.AddParam("@qr_images", log.qr_images);
                 cmd.AddParam("@token", log.token);
                 cmd.AddParam("@codemerchant_id", log.codemerchant_id);
                 cmd.AddParam("@posid", log.posid);
                 cmd.AddParam("@ret_code", log.ret_code);
                 cmd.AddParam("@item_title", log.item_title);
                 cmd.AddParam("@left_num", log.left_num);
                 cmd.AddParam("@ret_time", log.ret_time);

                 cmd.ExecuteNonQuery();
                 return log.id;

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Taobao_Ms.Model;

namespace ETS2.PM.Service.Taobao_Ms.Data.Internal
{
    public class InternalTaobao_resend_noticeretlog
    {
         public SqlHelper sqlHelper;
         public InternalTaobao_resend_noticeretlog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

         internal int Editnoticeretlog(Taobao_resend_noticeretlog  log)
         {
             if (log.id == 0)
             {
                 var sql = @"INSERT INTO  [taobao_resend_noticeretlog]
           ([order_id]
           ,[verify_codes]
           ,[token]
           ,[codemerchant_id]
           ,[qr_images]
           ,[ret_code]
           ,[ret_time])
     VALUES
           (@order_id
           ,@verify_codes
           ,@token
           ,@codemerchant_id
           ,@qr_images
           ,@ret_code
           ,@ret_time);select @@identity;";

                 var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                 cmd.AddParam("@order_id", log.order_id);
                 cmd.AddParam("@verify_codes", log.verify_codes);
                 cmd.AddParam("@token", log.token);
                 cmd.AddParam("@codemerchant_id", log.codemerchant_id);
                 cmd.AddParam("@qr_images", log.qr_images);
                 cmd.AddParam("@ret_code", log.ret_code);
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
                 string sql = @"UPDATE  [taobao_resend_noticeretlog]
                           SET [order_id] = @order_id
                              ,[verify_codes] = @verify_codes 
                              ,[token] = @token 
                              ,[codemerchant_id] = @codemerchant_id 
                              ,[qr_images] = @qr_images 
                              ,[ret_code] = @ret_code 
                              ,[ret_time] = @ret_time 
                         WHERE id=@id;";
                 var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                 cmd.AddParam("@id", log.id);
                 cmd.AddParam("@order_id", log.order_id);
                 cmd.AddParam("@verify_codes", log.verify_codes);
                 cmd.AddParam("@token", log.token);
                 cmd.AddParam("@codemerchant_id", log.codemerchant_id);
                 cmd.AddParam("@qr_images", log.qr_images);
                 cmd.AddParam("@ret_code", log.ret_code);
                 cmd.AddParam("@ret_time", log.ret_time);
                 cmd.ExecuteNonQuery();
                 return log.id;
             }
         }
    }
}

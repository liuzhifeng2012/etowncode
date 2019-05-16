using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Taobao_Ms.Model;

namespace ETS2.PM.Service.Taobao_Ms.Data.Internal
{
    public class InternalTaobao_consume_retlog
    {
        public SqlHelper sqlHelper;
        public InternalTaobao_consume_retlog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int Editcomsumeretlog(Taobao_consume_retlog log)
        {
            if (log.id == 0)
            {
                string sql = @"INSERT INTO  [taobao_consume_retlog]
           ([order_id]
           ,[verify_code]
           ,[consume_num]
           ,[token]
           ,[codemerchant_id]
           ,[posid]
           ,[mobile]
           ,[new_code]
           ,[serial_num]
           ,[qr_images]
           ,[ret_code]
           ,[item_title]
           ,[left_num]
           ,[sms_tpl]
           ,[print_tpl]
           ,[consume_secial_num]
           ,[code_left_num]
           ,[ret_time]
           ,ycSystemRandomid)
     VALUES
           (@order_id 
           ,@verify_code 
           ,@consume_num 
           ,@token 
           ,@codemerchant_id 
           ,@posid 
           ,@mobile 
           ,@new_code 
           ,@serial_num 
           ,@qr_images 
           ,@ret_code 
           ,@item_title 
           ,@left_num 
           ,@sms_tpl 
           ,@print_tpl 
           ,@consume_secial_num 
           ,@code_left_num 
           ,@ret_time 
           ,@ycSystemRandomid);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@order_id", log.order_id);
                cmd.AddParam("@verify_code", log.verify_code);
                cmd.AddParam("@consume_num", log.consume_num);
                cmd.AddParam("@token", log.token);
                cmd.AddParam("@codemerchant_id", log.codemerchant_id);
                cmd.AddParam("@posid", log.posid);
                cmd.AddParam("@mobile", log.mobile);
                cmd.AddParam("@new_code", log.new_code);
                cmd.AddParam("@serial_num", log.serial_num);
                cmd.AddParam("@qr_images", log.qr_images);
                cmd.AddParam("@ret_code", log.ret_code);
                cmd.AddParam("@item_title", log.item_title);
                cmd.AddParam("@left_num", log.left_num);
                cmd.AddParam("@sms_tpl", log.sms_tpl);
                cmd.AddParam("@print_tpl", log.print_tpl);
                cmd.AddParam("@consume_secial_num", log.consume_secial_num);
                cmd.AddParam("@code_left_num", log.code_left_num);
                cmd.AddParam("@ret_time", log.ret_time);
                cmd.AddParam("@ycSystemRandomid", log.ycSystemRandomid);

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
                string sql = @"UPDATE [taobao_consume_retlog]
                               SET [order_id] = @order_id
                                  ,[verify_code] = @verify_code
                                  ,[consume_num] = @consume_num
                                  ,[token] = @token
                                  ,[codemerchant_id] = @codemerchant_id
                                  ,[posid] = @posid
                                  ,[mobile] = @mobile
                                  ,[new_code] = @new_code
                                  ,[serial_num] = @serial_num
                                  ,[qr_images] = @qr_images
                                  ,[ret_code] = @ret_code
                                  ,[item_title] = @item_title
                                  ,[left_num] = @left_num
                                  ,[sms_tpl] = @sms_tpl
                                  ,[print_tpl] = @print_tpl
                                  ,[consume_secial_num] = @consume_secial_num
                                  ,[code_left_num] = @code_left_num
                                  ,[ret_time] = @ret_time
                                  ,ycSystemRandomid=@ycSystemRandomid
                             WHERE id=@id;";

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", log.id);
                cmd.AddParam("@order_id", log.order_id);
                cmd.AddParam("@verify_code", log.verify_code);
                cmd.AddParam("@consume_num", log.consume_num);
                cmd.AddParam("@token", log.token);
                cmd.AddParam("@codemerchant_id", log.codemerchant_id);
                cmd.AddParam("@posid", log.posid);
                cmd.AddParam("@mobile", log.mobile);
                cmd.AddParam("@new_code", log.new_code);
                cmd.AddParam("@serial_num", log.serial_num);
                cmd.AddParam("@qr_images", log.qr_images);
                cmd.AddParam("@ret_code", log.ret_code);
                cmd.AddParam("@item_title", log.item_title);
                cmd.AddParam("@left_num", log.left_num);
                cmd.AddParam("@sms_tpl", log.sms_tpl);
                cmd.AddParam("@print_tpl", log.print_tpl);
                cmd.AddParam("@consume_secial_num", log.consume_secial_num);
                cmd.AddParam("@code_left_num", log.code_left_num);
                cmd.AddParam("@ret_time", log.ret_time);
                cmd.AddParam("@ycSystemRandomid", log.ycSystemRandomid);

                cmd.ExecuteNonQuery();
                return log.id;
            }
        }

        internal Taobao_consume_retlog GetTaobao_consume_retlog(string qrcode, string num, string randomid)
        {
            string sql = "select top 1 *  from taobao_consume_retlog where verify_code='" + qrcode + "' and consume_num=" + num + " and ycSystemRandomid='" + randomid + "' order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                Taobao_consume_retlog log = null;
                if (reader.Read())
                {
                    log = new Taobao_consume_retlog
                    {
                        id = reader.GetValue<int>("id"),
                        order_id = reader.GetValue<string>("order_id"),
                        verify_code = reader.GetValue<string>("verify_code"),
                        consume_num = reader.GetValue<int>("consume_num"),
                        token = reader.GetValue<string>("token"),
                        codemerchant_id = reader.GetValue<string>("codemerchant_id"),
                        posid = reader.GetValue<string>("posid"),
                        mobile = reader.GetValue<string>("mobile"),
                        new_code = reader.GetValue<string>("new_code"),
                        serial_num = reader.GetValue<string>("serial_num"),
                        qr_images = reader.GetValue<string>("qr_images"),
                        ret_code = reader.GetValue<string>("ret_code"),
                        item_title = reader.GetValue<string>("item_title"),
                        left_num = reader.GetValue<int>("left_num"),
                        sms_tpl = reader.GetValue<string>("sms_tpl"),
                        print_tpl = reader.GetValue<string>("print_tpl"),
                        consume_secial_num = reader.GetValue<string>("consume_secial_num"),
                        code_left_num = reader.GetValue<int>("code_left_num"),
                        ret_time = reader.GetValue<DateTime>("ret_time"),
                        ycSystemRandomid = reader.GetValue<string>("ycSystemRandomid")
                    };
                }
                return log;
            }
        }
    }
}

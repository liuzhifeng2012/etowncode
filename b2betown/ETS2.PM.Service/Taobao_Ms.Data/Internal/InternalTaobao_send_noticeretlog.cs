using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Taobao_Ms.Model;

namespace ETS2.PM.Service.Taobao_Ms.Data.Internal
{
    public class InternalTaobao_send_noticeretlog
    {
        public SqlHelper sqlHelper;
        public InternalTaobao_send_noticeretlog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int Editsendnoticeretlog(Taobao_send_noticeretlog log)
        {
            if (log.id == 0)
            {
                var sql = @"INSERT INTO  [taobao_send_noticeretlog]
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

                 
                    object o = cmd.ExecuteScalar();
                    return int.Parse(o.ToString());
             
            }
            else
            {
                string sql = @"UPDATE  [taobao_send_noticeretlog]
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

        internal string GetVerifyCodesByTborderid(string tboid)
        {
            string sql = "select top 1   verify_codes from taobao_send_noticeretlog where order_id='" + tboid + "' order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                string r = "";
                if (reader.Read())
                {
                    r = reader.GetValue<string>("verify_codes").ToString();
                }
                return r;
            }
        }

        internal Taobao_send_noticeretlog GetSendRetLogByQrcode(string qrcode)
        {
            string sql = "SELECT top 1  *  FROM [taobao_send_noticeretlog] where verify_codes like '%"+qrcode+"%' order by id desc";


            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            using (var reader = cmd.ExecuteReader())
            {
                Taobao_send_noticeretlog log = null;
                if (reader.Read())
                {
                    log = new Taobao_send_noticeretlog
                    {
                        id = reader.GetValue<int>("id"),
                        order_id = reader.GetValue<string>("order_id"),
                        verify_codes = reader.GetValue<string>("verify_codes"),
                        token = reader.GetValue<string>("token"),
                        codemerchant_id = reader.GetValue<string>("codemerchant_id"),
                        qr_images = reader.GetValue<string>("qr_images"),
                        ret_code = reader.GetValue<string>("ret_code"),
                        ret_time = reader.GetValue<DateTime>("ret_time")
                    };
                }
                return log;
            }
        }

        internal int GetInvokeNum(string tborderid)
        {
            string sql = "select count(1) from taobao_send_noticeretlog where order_id='"+tborderid+"'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
      
            object o = cmd.ExecuteScalar();
           
            return int.Parse(o.ToString());
        }
        /// <summary>
        /// 24h内 提单成功 但是 发送淘宝发码回调失败的订单;如果 成功了，则不再(无需)可以回调
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        internal int GetIscantaobo_sendret(int orderid)
        {
            sqlHelper.BeginTrancation();

            try
            {
                int iscan = 0;
                //判断淘宝回调是否成功了,成功了则不需要再次回调
                string sq = "select count(1) from taobao_send_noticeretlog where (convert(nvarchar,ret_code) = '1' or  ret_code like '%已经发码%')  and order_id in (select order_id from taobao_send_noticelog where  self_order_id=" + orderid + " )";
                var cmd = sqlHelper.PrepareTextSqlCommand(sq);
                object o = cmd.ExecuteScalar();
                if (int.Parse(o.ToString()) > 0)
                {
                    iscan = 0;
                }
                else 
                {
                    //淘宝发码通知没有记录，则不是淘宝订单，不需要淘宝回调
                    string sqlaa = "select count(1) from taobao_send_noticelog where  self_order_id="+orderid;
                    cmd = sqlHelper.PrepareTextSqlCommand(sqlaa);

                    object q = cmd.ExecuteScalar();
                    if (int.Parse(q.ToString()) > 0)
                    {
                        //判断淘宝回调是否有记录(如果订单是失败订单则没有淘宝回调记录)，没有记录则需要淘宝回调 
                        string sqlbb = "select count(1) from taobao_send_noticeretlog where   order_id in (select order_id from taobao_send_noticelog where  self_order_id=" + orderid + " )";
                        cmd = sqlHelper.PrepareTextSqlCommand(sqlbb);

                        object qbb = cmd.ExecuteScalar();
                        if (int.Parse(qbb.ToString()) > 0)
                        {
                            //判断是否有 24h内 提单成功 但是 发送淘宝发码回调失败的订单
                            string sqlcc = "select count(1) from taobao_send_noticeretlog where convert(nvarchar,ret_code) != '1' and ret_time> '" + DateTime.Now.AddHours(-24) + "' and order_id in (select order_id from taobao_send_noticelog where  self_order_id=" + orderid + " )";
                            cmd = sqlHelper.PrepareTextSqlCommand(sqlcc);

                            object qcc = cmd.ExecuteScalar();
                            if (int.Parse(qcc.ToString()) > 0)
                            {
                                iscan = 1;
                            }
                            else
                            {
                                iscan = 0;
                            } 
                        }
                        else
                        {
                            iscan = 1;
                        } 

                    }
                    else
                    {
                        iscan = 0;
                    }  
                  
                }
                 
                sqlHelper.Commit();
                sqlHelper.Dispose();
                return iscan;
            }
            catch 
            {
               
                sqlHelper.Rollback();
                sqlHelper.Dispose();
                return 0;
            }
        }
    }
}

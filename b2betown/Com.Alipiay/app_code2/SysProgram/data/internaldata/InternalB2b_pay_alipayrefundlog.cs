using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using Com.Alipiay.app_code2.SysProgram.model;

namespace Com.Alipiay.app_code2.SysProgram.data.internaldata
{
    public class InternalB2b_pay_alipayrefundlog
    {
        public SqlHelper sqlHelper;
        public InternalB2b_pay_alipayrefundlog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

//        internal B2b_pay_alipayrefundlog GetLastestrefundsuclog(int orderid)
//        {
//            string sql = @"SELECT top 1 [id]
//                              ,[orderid]
//                              ,[service]
//                              ,[partner]
//                              ,[notify_url]
//                              ,[seller_email]
//                              ,[seller_user_id]
//                              ,[refund_date]
//                              ,[batch_no]
//                              ,[batch_num]
//                              ,[detail_data]
//                              ,[notify_time]
//                              ,[notify_type]
//                              ,[notify_id]
//                              ,[success_num]
//                              ,[result_details]
//                              ,[error_code]
//                              ,[error_desc]
//                          FROM  [b2b_pay_alipayrefundlog] where orderid=@orderid and error_code='SUCCESS' order by id desc";
//            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
//            cmd.AddParam("@orderid", orderid);
//            using (var reader = cmd.ExecuteReader())
//            {
//                B2b_pay_alipayrefundlog m = null;
//                if (reader.Read())
//                {
//                    m = new B2b_pay_alipayrefundlog
//                    {
//                        id = reader.GetValue<int>("id"),
//                        orderid = reader.GetValue<int>("orderid"),
//                        service = reader.GetValue<string>("service"),
//                        partner = reader.GetValue<string>("partner"),
//                        notify_url = reader.GetValue<string>("notify_url"),
//                        seller_email = reader.GetValue<string>("seller_email"),
//                        seller_user_id = reader.GetValue<string>("seller_user_id"),
//                        refund_date = reader.GetValue<DateTime>("refund_date"),
//                        batch_no = reader.GetValue<string>("batch_no"),
//                        batch_num = reader.GetValue<int>("batch_num"),
//                        detail_data = reader.GetValue<string>("detail_data"),
//                        notify_time = reader.GetValue<DateTime>("notify_time"),
//                        notify_type = reader.GetValue<string>("notify_type"),
//                        notify_id = reader.GetValue<string>("notify_id"),
//                        success_num = reader.GetValue<int>("success_num"),
//                        result_details = reader.GetValue<string>("result_details"),
//                        error_code = reader.GetValue<string>("error_code"),
//                        error_desc = reader.GetValue<string>("error_desc"),

//                    };
//                }
//                return m;
//            }

//        }

        internal int Editalipayrefundlog(B2b_pay_alipayrefundlog nowlog)
        {
            if (nowlog.id == 0)
            {
                string sql = @"INSERT INTO  [b2b_pay_alipayrefundlog]
                                   ([orderid]
                                   ,[service]
                                   ,[partner]
                                   ,[notify_url]
                                   ,[seller_email]
                                   ,[seller_user_id]
                                   ,[refund_date]
                                   ,[batch_no]
                                   ,[batch_num]
                                   ,[detail_data]
                                   ,[notify_time]
                                   ,[notify_type]
                                   ,[notify_id]
                                   ,[success_num]
                                   ,[result_details]
                                   ,[error_code]
                                   ,[error_desc]
                                   ,refund_fee,rentserver_refundlogid)
                             VALUES
                                   (@orderid
                                   ,@service
                                   ,@partner
                                   ,@notify_url
                                   ,@seller_email
                                   ,@seller_user_id
                                   ,@refund_date
                                   ,@batch_no
                                   ,@batch_num
                                   ,@detail_data
                                   ,@notify_time
                                   ,@notify_type
                                   ,@notify_id
                                   ,@success_num
                                   ,@result_details
                                   ,@error_code
                                   ,@error_desc
                                   ,@refund_fee,@rentserver_refundlogid);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@orderid", nowlog.orderid);
                cmd.AddParam("@service", nowlog.service);
                cmd.AddParam("@partner", nowlog.partner);
                cmd.AddParam("@notify_url", nowlog.notify_url);
                cmd.AddParam("@seller_email", nowlog.seller_email);
                cmd.AddParam("@seller_user_id", nowlog.seller_user_id);
                cmd.AddParam("@refund_date", nowlog.refund_date);
                cmd.AddParam("@batch_no", nowlog.batch_no);
                cmd.AddParam("@batch_num", nowlog.batch_num);
                cmd.AddParam("@detail_data", nowlog.detail_data);
                cmd.AddParam("@notify_time", nowlog.notify_time);
                cmd.AddParam("@notify_type", nowlog.notify_type);
                cmd.AddParam("@notify_id", nowlog.notify_id);
                cmd.AddParam("@success_num", nowlog.success_num);
                cmd.AddParam("@result_details", nowlog.result_details);
                cmd.AddParam("@error_code", nowlog.error_code);
                cmd.AddParam("@error_desc", nowlog.error_desc);
                cmd.AddParam("@refund_fee", nowlog.refund_fee); 
                cmd.AddParam("@rentserver_refundlogid", nowlog.rentserver_refundlogid);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
            else
            {
                string sql = @"UPDATE  [b2b_pay_alipayrefundlog]
                                   SET [orderid] = @orderid
                                      ,[service] = @service
                                      ,[partner] = @partner
                                      ,[notify_url] = @notify_url
                                      ,[seller_email] = @seller_email
                                      ,[seller_user_id] = @seller_user_id
                                      ,[refund_date] = @refund_date
                                      ,[batch_no] = @batch_no
                                      ,[batch_num] = @batch_num
                                      ,[detail_data] = @detail_data
                                      ,[notify_time] = @notify_time
                                      ,[notify_type] = @notify_type
                                      ,[notify_id] = @notify_id
                                      ,[success_num] = @success_num
                                      ,[result_details] = @result_details
                                      ,[error_code] = @error_code
                                      ,[error_desc] = @error_desc
                                      ,refund_fee=@refund_fee,
                                      rentserver_refundlogid=@rentserver_refundlogid
                                 WHERE id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", nowlog.id);
                cmd.AddParam("@orderid", nowlog.orderid);
                cmd.AddParam("@service", nowlog.service);
                cmd.AddParam("@partner", nowlog.partner);
                cmd.AddParam("@notify_url", nowlog.notify_url);
                cmd.AddParam("@seller_email", nowlog.seller_email);
                cmd.AddParam("@seller_user_id", nowlog.seller_user_id);
                cmd.AddParam("@refund_date", nowlog.refund_date);
                cmd.AddParam("@batch_no", nowlog.batch_no);
                cmd.AddParam("@batch_num", nowlog.batch_num);
                cmd.AddParam("@detail_data", nowlog.detail_data);
                cmd.AddParam("@notify_time", nowlog.notify_time);
                cmd.AddParam("@notify_type", nowlog.notify_type);
                cmd.AddParam("@notify_id", nowlog.notify_id);
                cmd.AddParam("@success_num", nowlog.success_num);
                cmd.AddParam("@result_details", nowlog.result_details);
                cmd.AddParam("@error_code", nowlog.error_code);
                cmd.AddParam("@error_desc", nowlog.error_desc);
                cmd.AddParam("@refund_fee", nowlog.refund_fee);
                cmd.AddParam("@rentserver_refundlogid", nowlog.rentserver_refundlogid);

                cmd.ExecuteNonQuery();
                return nowlog.id;

            }
        }

        internal int Uprefundlog(string batch_no, string success_num, string result_details, string error_code, string error_desc, string notify_id, string notify_type, string notify_time)
        {
            string sql = @"UPDATE  [b2b_pay_alipayrefundlog]
                                   SET  [notify_time] = @notify_time
                                      ,[notify_type] = @notify_type
                                      ,[notify_id] = @notify_id
                                      ,[success_num] = @success_num
                                      ,[result_details] = @result_details
                                      ,[error_code] = @error_code
                                      ,[error_desc] = @error_desc
                                 WHERE [batch_no] = @batch_no";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            cmd.AddParam("@batch_no", batch_no);
            cmd.AddParam("@notify_time", notify_time);
            cmd.AddParam("@notify_type", notify_type);
            cmd.AddParam("@notify_id", notify_id);
            cmd.AddParam("@success_num", success_num);
            cmd.AddParam("@result_details", result_details);
            cmd.AddParam("@error_code", error_code);
            cmd.AddParam("@error_desc", error_desc);

            return cmd.ExecuteNonQuery();
        }

        internal decimal Gettotalquitfeebyoid(int orderid)
        {
            try
            {
                string sql = "select  sum(refund_fee) as total_refund_fee from b2b_pay_alipayrefundlog  where orderid=" + orderid + " and error_code='SUCCESS'";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    decimal r = 0;
                    if (reader.Read())
                    {
                        r = reader.GetValue<decimal>("total_refund_fee");
                    }
                    return r;
                }
            }
            catch
            {
                return 0;
            }
        }

        internal int IsHasAlipayRefund(int orderid)
        {
            string sql = "select count(1) from b2b_pay_alipayrefundlog where orderid="+orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            if (int.Parse(o.ToString()) == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        internal string AlipayRefundDetailStr(int orderid)
        {
            string sql = "select top 1  * from b2b_pay_alipayrefundlog  where orderid='" + orderid + "' order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                string r = "";
                if (reader.Read())
                {

                    r = reader.GetValue<string>("error_code") == "SUCCESS" ? "退款成功(" + reader.GetValue<decimal>("refund_fee") + "元)" : "退款失败(" +  reader.GetValue<decimal>("refund_fee")  + "元):" + reader.GetValue<string>("error_code") + "(" + reader.GetValue<string>("error_desc") + ")";
                }
                return r;
            }
        }

        internal B2b_pay_alipayrefundlog Getrefundlogbybatch_no(string batch_no)
        {
          
            string sql = @"SELECT   [id]
                                          ,[orderid]
                                          ,[service]
                                          ,[partner]
                                          ,[notify_url]
                                          ,[seller_email]
                                          ,[seller_user_id]
                                          ,[refund_date]
                                          ,[batch_no]
                                          ,[batch_num]
                                          ,[detail_data]
                                          ,[notify_time]
                                          ,[notify_type]
                                          ,[notify_id]
                                          ,[success_num]
                                          ,[result_details]
                                          ,[error_code]
                                          ,[error_desc]
                                          ,refund_fee 
                                          ,rentserver_refundlogid
                                      FROM  [b2b_pay_alipayrefundlog] where batch_no=@batch_no";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@batch_no", batch_no);
            using (var reader = cmd.ExecuteReader())
            {
                B2b_pay_alipayrefundlog m = null;
                if (reader.Read())
                {
                    m = new B2b_pay_alipayrefundlog
                    {
                        id = reader.GetValue<int>("id"),
                        orderid = reader.GetValue<int>("orderid"),
                        service = reader.GetValue<string>("service"),
                        partner = reader.GetValue<string>("partner"),
                        notify_url = reader.GetValue<string>("notify_url"),
                        seller_email = reader.GetValue<string>("seller_email"),
                        seller_user_id = reader.GetValue<string>("seller_user_id"),
                        refund_date = reader.GetValue<DateTime>("refund_date"),
                        batch_no = reader.GetValue<string>("batch_no"),
                        batch_num = reader.GetValue<int>("batch_num"),
                        detail_data = reader.GetValue<string>("detail_data"),
                        notify_time = reader.GetValue<DateTime>("notify_time"),
                        notify_type = reader.GetValue<string>("notify_type"),
                        notify_id = reader.GetValue<string>("notify_id"),
                        success_num = reader.GetValue<int>("success_num"),
                        result_details = reader.GetValue<string>("result_details"),
                        error_code = reader.GetValue<string>("error_code"),
                        error_desc = reader.GetValue<string>("error_desc"),
                        refund_fee = reader.GetValue<decimal>("refund_fee"),
                        rentserver_refundlogid = reader.GetValue<int>("rentserver_refundlogid"),
                    };
                }
                return m;
            }

        }
    }
}

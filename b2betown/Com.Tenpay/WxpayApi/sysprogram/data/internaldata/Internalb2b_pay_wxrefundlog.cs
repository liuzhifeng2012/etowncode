using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using Com.Tenpay.WxpayApi.sysprogram.model;

namespace Com.Tenpay.WxpayApi.sysprogram.data.internaldata
{
    public class Internalb2b_pay_wxrefundlog
    {
        public SqlHelper sqlHelper;
        public Internalb2b_pay_wxrefundlog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int Editwxrefundlog(B2b_pay_wxrefundlog m)
        {
            if (m.id == 0)
            {
                string sql = @"INSERT INTO [b2b_pay_wxrefundlog]
                                       ([out_refund_no]
                                       ,[out_trade_no]
                                       ,[transaction_id]
                                       ,[total_fee]
                                       ,[refund_fee]
                                       ,[send_xml]
                                       ,[send_time]
                                       ,[return_code]
                                       ,[return_msg]
                                       ,[err_code]
                                       ,[err_code_des]
                                       ,[refund_id]
                                       ,[return_xml]
                                       ,[return_time])
                                 VALUES
                                       (@out_refund_no 
                                       ,@out_trade_no 
                                       ,@transaction_id 
                                       ,@total_fee 
                                       ,@refund_fee 
                                       ,@send_xml 
                                       ,@send_time 
                                       ,@return_code 
                                       ,@return_msg 
                                       ,@err_code 
                                       ,@err_code_des 
                                       ,@refund_id 
                                       ,@return_xml 
                                       ,@return_time);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@out_refund_no", m.out_refund_no);
                cmd.AddParam("@out_trade_no", m.out_trade_no);
                cmd.AddParam("@transaction_id", m.transaction_id);
                cmd.AddParam("@total_fee", m.total_fee);
                cmd.AddParam("@refund_fee", m.refund_fee);
                cmd.AddParam("@send_xml", m.send_xml);
                cmd.AddParam("@send_time", m.send_time);
                cmd.AddParam("@return_code", m.return_code);
                cmd.AddParam("@return_msg", m.return_msg);
                cmd.AddParam("@err_code", m.err_code);
                cmd.AddParam("@err_code_des", m.err_code_des);
                cmd.AddParam("@refund_id", m.refund_id);
                cmd.AddParam("@return_xml", m.return_xml);
                cmd.AddParam("@return_time", m.return_time);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
            else
            {
                string sql = @"UPDATE [b2b_pay_wxrefundlog]
                               SET [out_refund_no] = @out_refund_no 
                                  ,[out_trade_no] = @out_trade_no 
                                  ,[transaction_id] = @transaction_id 
                                  ,[total_fee] = @total_fee 
                                  ,[refund_fee] = @refund_fee 
                                  ,[send_xml] = @send_xml 
                                  ,[send_time] = @send_time 
                                  ,[return_code] = @return_code
                                  ,[return_msg] = @return_msg 
                                  ,[err_code] = @err_code
                                  ,[err_code_des] = @err_code_des 
                                  ,[refund_id] = @refund_id 
                                  ,[return_xml] = @return_xml 
                                  ,[return_time] = @return_time 
                             WHERE  id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", m.id);
                cmd.AddParam("@out_refund_no", m.out_refund_no);
                cmd.AddParam("@out_trade_no", m.out_trade_no);
                cmd.AddParam("@transaction_id", m.transaction_id);
                cmd.AddParam("@total_fee", m.total_fee);
                cmd.AddParam("@refund_fee", m.refund_fee);
                cmd.AddParam("@send_xml", m.send_xml);
                cmd.AddParam("@send_time", m.send_time);
                cmd.AddParam("@return_code", m.return_code);
                cmd.AddParam("@return_msg", m.return_msg);
                cmd.AddParam("@err_code", m.err_code);
                cmd.AddParam("@err_code_des", m.err_code_des);
                cmd.AddParam("@refund_id", m.refund_id);
                cmd.AddParam("@return_xml", m.return_xml);
                cmd.AddParam("@return_time", m.return_time);

                cmd.ExecuteNonQuery();
                return m.id;
            }
        }

        internal int Upreturnxml(string returnxml, int id)
        {
            string sql = "update b2b_pay_wxrefundlog set return_xml='" + returnxml + "',return_time='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where id=" + id;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 是否有微信退款
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        internal int IsHasWxRefund(int orderid)
        {
            string sql = "select  count(1) from B2b_pay_wxrefundlog  where out_trade_no='" + orderid + "'";
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

        internal string WxRefundDetailStr(int orderid)
        {
            string sql = "select  * from B2b_pay_wxrefundlog  where out_trade_no='" + orderid + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                string r = "";
                if (reader.Read())
                {

                    r = reader.GetValue<string>("err_code") == "" ? "退款成功(" + decimal.Parse(reader.GetValue<int>("refund_fee").ToString()) / 100 + "元)" : "退款失败(" + decimal.Parse(reader.GetValue<int>("refund_fee").ToString()) / 100 + "元):" + reader.GetValue<string>("err_code") + "(" + reader.GetValue<string>("err_code_des") + ")";
                }
                return r;
            }
        }

        internal decimal Gettotalquitfeebyoid(int orderid)
        {
            try
            {
                string sql = "select  sum(refund_fee) as total_refund_fee from b2b_pay_wxrefundlog  where out_trade_no=" + orderid + " and err_code=''";
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
    }
}

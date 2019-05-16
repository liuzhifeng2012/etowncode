using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;

namespace ETS2.VAS.Service.VASService.Data.InternalData
{
    public class InternalB2bPay
    {
        private SqlHelper sqlHelper;
        public InternalB2bPay(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        #region 支付插入数据库
        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdatePay";
        internal int InsertOrUpdate(B2b_pay model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Oid", model.Oid);
            cmd.AddParam("@Pay_com", model.Pay_com);
            cmd.AddParam("@Pay_name", model.Pay_name);
            cmd.AddParam("@Pay_phone", model.Pay_phone);
            cmd.AddParam("@Total_fee", model.Total_fee);
            cmd.AddParam("@Trade_no", model.Trade_no);
            cmd.AddParam("@Trade_status", model.Trade_status);
            cmd.AddParam("@Uip", model.Uip);
            cmd.AddParam("@Wxtransaction_id", model.Wxtransaction_id);
            cmd.AddParam("@comid", model.comid);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion

        #region 根据订单号Oid获取支付信息
        internal B2b_pay GetPayById(int orderid)
        {
            string sqltxt = "SELECT   [id] ,[oid] ,[pay_com] ,[pay_name],[pay_phone] ,[total_fee],[trade_no] ,[trade_status] ,[uip] ,wxtransaction_id,comid,subtime   FROM b2b_pay where [Oid]=" + orderid ;
            //判断订单是否是购物车订单
            int shopcartid = new B2bOrderData().GetShopcartidbyoid(orderid);
            if(shopcartid>0)
            {
                sqltxt = "SELECT   [id] ,[oid] ,[pay_com] ,[pay_name],[pay_phone] ,[total_fee],[trade_no] ,[trade_status] ,[uip] ,wxtransaction_id,comid,subtime    FROM  [b2b_pay] where oid in (select id from b2b_order where shopcartid=" + shopcartid + ")";
            }


            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_pay
                    {
                        Id = reader.GetValue<int>("Id"),
                        Oid = reader.GetValue<int>("oid"),
                        Pay_com = reader.GetValue<string>("pay_com"),
                        Pay_name = reader.GetValue<string>("pay_name"),
                        Pay_phone = reader.GetValue<string>("pay_phone"),
                        Total_fee = reader.GetValue<decimal>("total_fee"),
                        Trade_no = reader.GetValue<string>("trade_no"),
                        Trade_status = reader.GetValue<string>("trade_status"),
                        Uip = reader.GetValue<string>("uip"),
                        Wxtransaction_id = reader.GetValue<string>("wxtransaction_id"),
                        comid = reader.GetValue<int>("comid"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                    };
                }
                return null;
            }
        }
        #endregion

        #region 根据订单号Oid获取支付信息,只查询成功
        internal B2b_pay GetSUCCESSPayById(int orderid)
        {
            string sqltxt = "SELECT   [id] ,[oid] ,[pay_com] ,[pay_name],[pay_phone] ,[total_fee],[trade_no] ,[trade_status] ,[uip] ,wxtransaction_id,comid,subtime   FROM b2b_pay where [Oid]=" + orderid +" and [trade_status]='TRADE_SUCCESS'";
            //判断订单是否是购物车订单
            int shopcartid = new B2bOrderData().GetShopcartidbyoid(orderid);
            if (shopcartid > 0)
            {
                sqltxt = "SELECT   [id] ,[oid] ,[pay_com] ,[pay_name],[pay_phone] ,[total_fee],[trade_no] ,[trade_status] ,[uip] ,wxtransaction_id,comid,subtime    FROM  [b2b_pay] where oid in (select id from b2b_order where shopcartid=" + shopcartid + ") and [trade_status]='TRADE_SUCCESS' ";
            }


            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_pay
                    {
                        Id = reader.GetValue<int>("Id"),
                        Oid = reader.GetValue<int>("oid"),
                        Pay_com = reader.GetValue<string>("pay_com"),
                        Pay_name = reader.GetValue<string>("pay_name"),
                        Pay_phone = reader.GetValue<string>("pay_phone"),
                        Total_fee = reader.GetValue<decimal>("total_fee"),
                        Trade_no = reader.GetValue<string>("trade_no"),
                        Trade_status = reader.GetValue<string>("trade_status"),
                        Uip = reader.GetValue<string>("uip"),
                        Wxtransaction_id = reader.GetValue<string>("wxtransaction_id"),
                        comid = reader.GetValue<int>("comid"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                    };
                }
                return null;
            }
        }
        #endregion

        #region 根据订单号获取订单支付详情
        internal string GetOrderPay(int orderid)
        {
            string sqltxt = "";
            string returnstr = "";

            sqltxt = @"SELECT  *
  FROM b2b_pay where [Oid]=@orderid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@orderid", orderid);

            using (var reader1 = cmd.ExecuteReader())
            {
                if (reader1.Read())
                {
                    if (reader1.GetValue<string>("trade_status") == "TRADE_SUCCESS")
                    {
                        returnstr = "网上支付(" + reader1.GetValue<string>("pay_com") + ")：";
                        if (reader1.GetValue<string>("pay_com") == "wx")
                        {
                            returnstr = "网上支付(微信支付)：";
                        }
                        if (reader1.GetValue<string>("pay_com") == "malipay" || reader1.GetValue<string>("pay_com") == "alipay")
                        {
                            returnstr = "网上支付(支付宝)：";
                        }
                        if (reader1.GetValue<string>("pay_com") == "mtenpay" || reader1.GetValue<string>("pay_com") == "tenpay")
                        {
                            returnstr = "网上支付(财付通)：";
                        }


                        returnstr += reader1.GetValue<decimal>("total_fee").ToString("0.00");

                        if (reader1.GetValue<string>("pay_com") == "wx")
                        {
                            returnstr += "  交易号:" + reader1.GetValue<string>("wxtransaction_id");
                        }
                        else
                        {
                            returnstr += "  交易号:" + reader1.GetValue<string>("trade_no");
                        }
                    }
                }
            }

            //先查询订单
            sqltxt = @"SELECT  *
  FROM [b2b_order] where [id]=@orderid ";
            cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@orderid", orderid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (reader.GetValue<int>("order_state") > 1)
                    {
                        var integral = reader.GetValue<Decimal>("integral");
                        var imprest = reader.GetValue<Decimal>("imprest");
                        if (integral != 0)
                        {
                            returnstr = returnstr + "使用积分抵: " + integral + " 元";
                        }

                        if (imprest != 0)
                        {
                            returnstr = returnstr + "使用预付款: " + imprest + " 元";
                        }

                    }
                }

            }



            return returnstr;

        }
        #endregion

        internal decimal GetPaytotalfee(B2b_order m)
        {
          
            string sql = "SELECT  [total_fee]  FROM  [b2b_pay] where oid=" + m.Id + " and [trade_status]='TRADE_SUCCESS'";

            if (m.Shopcartid > 0)//购物车订单，则需要查询购物车订单的支付金额
            {
                sql = "SELECT  [total_fee]   FROM  [b2b_pay] where oid in (select id from b2b_order where shopcartid=" + m.Shopcartid + ") and [trade_status]='TRADE_SUCCESS'";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<decimal>("total_fee");
                }
                return 0;
            }
        }

        internal string Getpaycombyorder(B2b_order order)
        {
            string sql = "select pay_com from b2b_pay where oid=" + order.Id + "  and trade_status='TRADE_SUCCESS'";
            if(order.Shopcartid>0)
            {
                sql = "select pay_com from b2b_pay where oid in (select id from b2b_order where shopcartid =" + order.Shopcartid + ")  and trade_status='TRADE_SUCCESS'";
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using(var reader=cmd.ExecuteReader())
            {
                string paycom = "";
                if(reader.Read())
                {
                    paycom = reader.GetValue<string>("pay_com");
                }
                return paycom;
            }
        }

        internal string GetOrderPaydesc(int orderid)
        {
            //判断是否有订单 绑定传入的订单
            B2b_order d_loldorder = new B2bOrderData().GetOldorderBybindingId(orderid);
            if (d_loldorder != null)
            {
                orderid = d_loldorder.Id;
            }
            string desc=GetOrderPay(orderid);
            return desc;
        }

        //internal B2b_pay GetYYYYB2bPay(int orderid, int comid)
        //{
        //    string sql = "select * from b2b_pay where oid="+orderid+" and comid="+comid;
        //    var cmd = this.sqlHelper.PrepareTextSqlCommand(sql);


        //    using (var reader = cmd.ExecuteReader())
        //    {
        //        if (reader.Read())
        //        {
        //            return new B2b_pay
        //            {
        //                Id = reader.GetValue<int>("Id"),
        //                Oid = reader.GetValue<int>("oid"),
        //                Pay_com = reader.GetValue<string>("pay_com"),
        //                Pay_name = reader.GetValue<string>("pay_name"),
        //                Pay_phone = reader.GetValue<string>("pay_phone"),
        //                Total_fee = reader.GetValue<decimal>("total_fee"),
        //                Trade_no = reader.GetValue<string>("trade_no"),
        //                Trade_status = reader.GetValue<string>("trade_status"),
        //                Uip = reader.GetValue<string>("uip"),
        //                Wxtransaction_id = reader.GetValue<string>("wxtransaction_id"),
        //                comid = reader.GetValue<int>("comid"),
        //                subtime = reader.GetValue<DateTime>("subtime"),
        //            };
        //        }
        //        return null;
        //    }
        //}
    }
}

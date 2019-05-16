using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.VAS.Service.VASService.Modle;
using ETS.Data.SqlHelper;
using ETS2.VAS.Service.VASService.Data.InternalData;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.VAS.Service.VASService.Data
{
    public class B2bPayData
    {

        #region 插入支付记录
        public int InsertOrUpdate(B2b_pay payinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2bPay(sql);
                    int result = internalData.InsertOrUpdate(payinfo);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 根据oid获得支付信息，成功支付
        public B2b_pay GetPayByoId(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                var order = new InternalB2bPay(helper).GetPayById(orderid);
                return order;
            }
        }
        #endregion

        #region 根据oid获得支付信息，不判断支付是否成功
        public B2b_pay GetSUCCESSPayById(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                var order = new InternalB2bPay(helper).GetSUCCESSPayById(orderid);
                return order;
            }
        }
        #endregion

        #region 根据订单号获取订单详细支付情况
        public string GetOrderPay(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                var order = new InternalB2bPay(helper).GetOrderPay(orderid);
                return order;
            }
        }
        #endregion

        /// <summary>
        /// 得到支付总金额
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public decimal GetPaytotalfee(B2b_order m)
        {
            using (var helper = new SqlHelper())
            {
                decimal r = new InternalB2bPay(helper).GetPaytotalfee(m);
                return r;
            }
        }


        /// <summary>
        /// 得到支付来源
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public string GetPaycomByorder(B2b_order order)
        {
            using(var helper=new SqlHelper())
            {
                string pay_com = new InternalB2bPay(helper).Getpaycombyorder(order);
                return pay_com;
            }
        }

        public string GetOrderPaydesc(int orderid)
        {
            using (var helper = new SqlHelper())
            {
                string desc = new InternalB2bPay(helper).GetOrderPaydesc(orderid);
                return desc;
            }
        }

        ///// <summary>
        ///// 这个方法只是因为20160121去哪儿录入产品 引起产生了很多错误订单 而建立的，请无论如何不要修改
        ///// </summary>
        ///// <param name="orderid"></param>
        ///// <param name="comid"></param>
        ///// <returns></returns>
        //public B2b_pay GetYYYYB2bPay(int orderid, int comid)
        //{
        //    using (var helper = new SqlHelper())
        //    {
        //        B2b_pay r = new InternalB2bPay(helper).GetYYYYB2bPay(orderid,comid);
        //        return r;
        //    }
        //}
    }
}

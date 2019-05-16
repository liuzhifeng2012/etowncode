using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.Meituan.Model;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Meituan.Data.Internal;

namespace ETS2.PM.Service.Meituan.Data
{
    public class Meituan_reqlogData
    {
        public int EditReqlog(Meituan_reqlog m)
        {
            using (var helper = new SqlHelper())
            {
                int id = new InternalMeituan_reqlog(helper).EditReqlog(m);
                return id;
            }
        }
        /// <summary>
        /// 根据美团订单号得到美团订单支付成功日志
        /// </summary>
        /// <param name="mtorderid"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public Meituan_reqlog GetMeituan_Orderpayreqlog(string mtorderid, string code)
        {
            using (var helper = new SqlHelper())
            {
                Meituan_reqlog r = new InternalMeituan_reqlog(helper).GetMeituan_Orderpayreqlog(mtorderid,code);
                return r;
            }
        }
        /// <summary>
        /// 根据本系统订单号得到美团订单支付成功日志
        /// </summary>
        /// <param name="a_orderid"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public Meituan_reqlog GetMt_OrderpayreqlogBySelforderid(int a_orderid, string code)
        {
            using (var helper = new SqlHelper())
            {
                Meituan_reqlog r = new InternalMeituan_reqlog(helper).GetMt_OrderpayreqlogBySelforderid(a_orderid, code);
                return r;
            }
        }
        /// <summary>
        /// 根据美团订单号得到订单创建成功日志
        /// </summary>
        /// <param name="mtorderid"></param>
        /// <returns></returns>
        public Meituan_reqlog GetMtOrderCrateLogByMtorder(string mtorderid,string code)
        {
            using (var helper = new SqlHelper())
            {
                Meituan_reqlog r = new InternalMeituan_reqlog(helper).GetMtOrderCrateLogByMtorder(mtorderid, code);
                return r;
            }
        }

        /// <summary>
        /// 根据美团订单号得到系统订单号
        /// </summary>
        /// <param name="mtorderid"></param>
        /// <returns></returns>
        public int GetSysorderidByMtorderid(string mtorderid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new InternalMeituan_reqlog(helper).GetSysorderidByMtorderid(mtorderid);
                return r;
            }
        }
    }
}

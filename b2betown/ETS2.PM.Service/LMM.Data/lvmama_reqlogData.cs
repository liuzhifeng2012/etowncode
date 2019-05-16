using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.LMM.Model;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.LMM.Data.Internal;

namespace ETS2.PM.Service.LMM.Data
{
    public class lvmama_reqlogData
    {


        public int EditReqlog(Lvmama_reqlog m)
        {
            using (var helper = new SqlHelper())
            {
                int id = new Internallvmama_reqlogData(helper).EditReqlog(m);
                return id;
            }
        }
        /// <summary>
        /// 根据驴妈妈订单号得到驴妈妈订单支付成功日志
        /// </summary>
        /// <param name="mtorderid"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public Lvmama_reqlog GetLvmama_Orderpayreqlog(string mtorderid, string code)
        {
            using (var helper = new SqlHelper())
            {
                Lvmama_reqlog r = new Internallvmama_reqlogData(helper).GetLvmama_Orderpayreqlog(mtorderid, code);
                return r;
            }
        }
        /// <summary>
        /// 根据本系统订单号得到驴妈妈订单支付成功日志
        /// </summary>
        /// <param name="a_orderid"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public Lvmama_reqlog GetLvmama_OrderpayreqlogBySelforderid(int a_orderid, string code)
        {
            using (var helper = new SqlHelper())
            {
                Lvmama_reqlog r = new Internallvmama_reqlogData(helper).GetLvmama_OrderpayreqlogBySelforderid(a_orderid, code);
                return r;
            }
        }
        /// <summary>
        /// 根据驴妈妈订单号得到订单创建成功日志
        /// </summary>
        /// <param name="mtorderid"></param>
        /// <returns></returns>
        public Lvmama_reqlog GetLvmamaOrderCreateLogByLvmamaorder(string mtorderid, string code)
        {
            using (var helper = new SqlHelper())
            {
                Lvmama_reqlog r = new Internallvmama_reqlogData(helper).GetLvmamaOrderCreateLogByLvmamaorder(mtorderid, code);
                return r;
            }
        }

        /// <summary>
        /// 根据驴妈妈订单号得到系统订单号
        /// </summary>
        /// <param name="mtorderid"></param>
        /// <returns></returns>
        public int GetSysorderidByLvmamaorderid(string mtorderid)
        {
            using (var helper = new SqlHelper())
            {
                int r = new Internallvmama_reqlogData(helper).GetSysorderidByLvmamaorderid(mtorderid);
                return r;
            }
        }
    }
}

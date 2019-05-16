using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.VAS.Service.VASService.Modle;
using ETS.Data.SqlHelper;
using ETS2.VAS.Service.VASService.Data.InternalData;

namespace ETS2.VAS.Service.VASService.Data
{
    public class B2bFinanceData
    {
        #region 插入财务记录
        public int InsertOrUpdate(B2b_Finance payinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2BFinance(sql);
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


        #region 提现申请
        public int InsertFinanceWithdraw(B2b_Finance payinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2BFinance(sql);
                    int result = internalData.InsertFinanceWithdraw(payinfo);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion
        #region 插入或修改财务收款方式
        public int InsertOrUpdateFinancePayType(B2b_Finance payinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2BFinance(sql);
                    int result = internalData.InsertOrUpdateFinancePayType(payinfo);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 插入或修改财务收款银行
        public int InsertOrUpdateFinancePayBank(B2b_finance_paytype payinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2BFinance(sql);
                    int result = internalData.InsertOrUpdateFinancePayBank(payinfo);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 插入或修改微信支付
        public int InsertOrUpdateFinancePayWX(B2b_finance_paytype payinfo)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2BFinance(sql);
                    int result = internalData.InsertOrUpdateFinancePayWX(payinfo);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion


        #region 获取财务记录
        public List<B2b_Finance> FinancePageList(string comid, int pageindex, int pagesize, string key, out int totalcount, int channelcomid = 0, int oid = 0, string payment_type = "", string money_come = "", string starttime = "", string endtime = "")
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2BFinance(helper).FinancePageList(comid, pageindex, pagesize, key, out totalcount, channelcomid, oid, payment_type, money_come, starttime, endtime);

                return list;
            }
        }
        #endregion


        #region 获取财务记录
        public List<B2b_Finance> FinanceallList(int comid, string stardate, string enddate)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2BFinance(helper).FinanceallList(comid, stardate, enddate);

                return list;
            }
        }
        #endregion

        #region 获取财务记录
        public List<B2b_Finance> Financecount(string comid, string stardate, string enddate)
        {
            using (var helper = new SqlHelper())
            {

                var list = new InternalB2BFinance(helper).Financecount(comid, stardate, enddate);

                return list;
            }
        }
        #endregion

        #region 获取支付方式
        public Modle.B2b_finance_paytype FinancePayType(int comid)
        {
            using (var helper = new SqlHelper())
            {

                var pro = new InternalB2BFinance(helper).FinancePayType(comid);

                return pro;
            }
        }
        #endregion


        #region 插入或修改财务收款方式
        public int WithdrawConf(int id, int comid, string remarks)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2BFinance(sql);
                    int result = internalData.WithdrawConf(id, comid, remarks);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region 插入或修改财务收款方式
        public int WithdrawConf(int id, int comid, string remarks,int printscreen)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2BFinance(sql);
                    int result = internalData.WithdrawConf(id, comid, remarks,printscreen);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion
        public decimal GetFinanceAmount(int comid, string payment_type)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2BFinance(sql);
                    decimal result = internalData.GetFinanceAmount(comid,payment_type);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        //查询支付金额
        public decimal GetShouruAmount(int orderid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2BFinance(sql);
                    decimal result = internalData.GetShouruAmount(orderid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        //查询手续费金额
        public decimal GetShouxufeiAmount(int orderid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2BFinance(sql);
                    decimal result = internalData.GetShouxufeiAmount(orderid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }


        //购物车订单 查询 支付订单 编号
        public int GetPayidbyorderid(int Shopcartid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2BFinance(sql);
                    int result = internalData.GetPayidbyorderid(Shopcartid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public decimal GetChannelFinanceCount(int comid, int channelcomid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2BFinance(sql);
                    decimal result = internalData.GetChannelFinanceCount(comid, channelcomid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        //获取分销商名称
        public string GetAgentNamebyorderid(int orderid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2BFinance(sql);
                    var result = internalData.GetAgentNamebyorderid(orderid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

                #region 积分获得使用详情
        public List<Member_Integral> IntegralList(int pageindex, int pagesize, int comid, out int totalcount,int mid=0,string key="")
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2BFinance(helper).IntegralList(pageindex, pagesize, comid,out totalcount,mid,key);

                return list;
            }
        }
        #endregion

        #region 积分获得使用详情
        public List<Member_Imprest> ImprestList(int pageindex, int pagesize, int comid, out int totalcount, int mid = 0,string key="")
        {
            using (var helper = new SqlHelper())
            {
                var list = new InternalB2BFinance(helper).ImprestList(pageindex, pagesize, comid, out totalcount,mid,key);

                return list;
            }
        }
        #endregion

        public decimal IntegralCount(int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2BFinance(sql);
                    decimal result = internalData.IntegralCount(comid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public decimal ImprestCount(int comid)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2BFinance(sql);
                    decimal result = internalData.ImprestCount(comid);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        //获取支付订单是否正确
        public int huoqupayorder(int orderid,decimal price)
        {
            using (var sql = new SqlHelper())
            {
                try
                {
                    var internalData = new InternalB2BFinance(sql);
                    var result = internalData.huoqupayorder(orderid, price);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }


        public IList<string> Selpayment_type()
        {
           using(var helper=new SqlHelper())
           {
               List<string> list = new InternalB2BFinance(helper).Selpayment_type();
               return list;
           }

        }

        public IList<string> Selmoney_come()
        {
            using (var helper = new SqlHelper())
            {
                List<string> list = new InternalB2BFinance(helper).Selmoney_come();
                return list;
            }
        }
    }
}

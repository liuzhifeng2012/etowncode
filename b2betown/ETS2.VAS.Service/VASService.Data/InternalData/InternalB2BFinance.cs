using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.VAS.Service.VASService.Modle;
using System.Data;

namespace ETS2.VAS.Service.VASService.Data.InternalData
{
    public class InternalB2BFinance
    {
        private SqlHelper sqlHelper;
        public InternalB2BFinance(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        #region 支付插入数据库
        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateFinance";
        internal int InsertOrUpdate(B2b_Finance model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Com_id", model.Com_id);
            cmd.AddParam("@Agent_id", model.Agent_id);
            cmd.AddParam("@Eid", model.Eid);
            cmd.AddParam("@Order_id", model.Order_id);
            cmd.AddParam("@Servicesname", model.Servicesname);
            cmd.AddParam("@SerialNumber", model.SerialNumber);
            cmd.AddParam("@Money", model.Money);
            cmd.AddParam("@Money_come", model.Money_come);
            cmd.AddParam("@Over_money", model.Over_money);
            cmd.AddParam("@Payment", model.Payment);
            cmd.AddParam("@Payment_type", model.Payment_type);
            cmd.AddParam("@Paychannelstate", model.Paychannelstate);
            cmd.AddParam("@Channelid", model.Channelid);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            //修改预付款+
            UpdateImprest(model.Com_id, model.Money);

            return (int)parm.Value;
        }
        #endregion


        #region 修改指定商的预付款
        internal int UpdateImprest(int comid, decimal money)
        {
            const string sqlTxt = "update dbo.b2b_company set imprest=imprest+@money where id=@comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@money", money);

            return cmd.ExecuteNonQuery();
        }
        #endregion


        #region 提现申请
        private static readonly string SQLInsertWite = "usp_InsertFinanceWithdraw";
        internal int InsertFinanceWithdraw(B2b_Finance model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertWite);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Com_id", model.Com_id);
            cmd.AddParam("@Money", model.Money);
            cmd.AddParam("@Servicesname", model.Servicesname);
            cmd.AddParam("@SerialNumber", model.SerialNumber);
            cmd.AddParam("@Money_come", model.Money_come);
            cmd.AddParam("@Over_money", model.Over_money);
            cmd.AddParam("@Payment", model.Payment);
            cmd.AddParam("@Payment_type", model.Payment_type);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            //修改预付款-
            UpdateImprest(model.Com_id, model.Money);
            return (int)parm.Value;


        }
        #endregion


        #region 修改或插入支付银行
        private static readonly string SQLInsertOrUpdatePayBank = "usp_InsertOrUpdateFinancePayBank";
        internal int InsertOrUpdateFinancePayBank(B2b_finance_paytype model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdatePayBank);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Com_id", model.Com_id);
            cmd.AddParam("@paytype", model.Paytype);
            cmd.AddParam("@bank_account", model.Bank_account);
            cmd.AddParam("@bank_card", model.Bank_card);
            cmd.AddParam("@bank_name", model.Bank_name);
            cmd.AddParam("@alipay_account", model.Alipay_account);
            cmd.AddParam("@alipay_id", model.Alipay_id);
            cmd.AddParam("@alipay_key", model.Alipay_key);
            cmd.AddParam("@Uptype", model.Uptype);
            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion

        #region 修改或插入支付银行

        internal int InsertOrUpdateFinancePayWX(B2b_finance_paytype model)
        {
            string sqlTxt = "";
            if (model.Id == 0)
            {
                sqlTxt = "insert dbo.b2b_finance_paytype (com_id,Wx_appid,Wx_appkey,Wx_paysignkey ,Wx_partnerid,Wx_partnerkey,wx_SSLCERT_PATH,wx_SSLCERT_PASSWORD)values(@com_id,@Wx_appid,@Wx_appkey,@Wx_paysignkey ,@Wx_partnerid,@Wx_partnerkey,@wx_SSLCERT_PATH,@wx_SSLCERT_PASSWORD)";
            }
            else
            {
                sqlTxt = "update dbo.b2b_finance_paytype set Wx_appid=@Wx_appid,Wx_appkey=@Wx_appkey,Wx_paysignkey=@Wx_paysignkey,Wx_partnerid=@Wx_partnerid,Wx_partnerkey=@Wx_partnerkey,wx_SSLCERT_PATH=@wx_SSLCERT_PATH,wx_SSLCERT_PASSWORD=@wx_SSLCERT_PASSWORD  where id=@id";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            if (model.Id > 0)
            {
                cmd.AddParam("@id", model.Id);
            }
            cmd.AddParam("@com_id", model.Com_id);
            cmd.AddParam("@Wx_appid", model.Wx_appid);
            cmd.AddParam("@Wx_appkey", model.Wx_appkey);
            cmd.AddParam("@Wx_paysignkey", model.Wx_paysignkey);
            cmd.AddParam("@Wx_partnerid", model.Wx_partnerid);
            cmd.AddParam("@Wx_partnerkey", model.Wx_partnerkey);
            cmd.AddParam("@wx_SSLCERT_PATH", model.wx_SSLCERT_PATH);
            cmd.AddParam("@wx_SSLCERT_PASSWORD", model.wx_SSLCERT_PASSWORD);

            return cmd.ExecuteNonQuery();

        }
        #endregion

        #region 修改或插入支付方式
        private static readonly string SQLInsertOrUpdatePayType = "usp_InsertOrUpdateFinancePayType";
        internal int InsertOrUpdateFinancePayType(B2b_Finance model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdatePayType);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Com_id", model.Com_id);
            cmd.AddParam("@Paytype", model.Paytype);
            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion

        #region 修改或插入支付方式
        internal int WithdrawConf(int id, int comid, string remarks)
        {
            const string sqlTxt = "update dbo.b2b_finance set remarks=@remarks,con_state=1 where id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@id", id);
            cmd.AddParam("@remarks", remarks);
            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region 修改或插入支付方式
        internal int WithdrawConf(int id, int comid, string remarks, int printscreen)
        {
            const string sqlTxt = "update dbo.b2b_finance set remarks=@remarks,con_state=1,printscreen=@printscreen where id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@id", id);
            cmd.AddParam("@remarks", remarks);
            cmd.AddParam("@printscreen", printscreen);
            return cmd.ExecuteNonQuery();
        }
        #endregion


        #region  获得订单分页列表
        internal List<B2b_Finance> FinancePageList(string comid, int pageindex, int pagesize, string key, out int totalcount, int channelcomid = 0, int oid = 0,string payment_type="",string money_come="",string starttime="",string endtime="")
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_finance";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = "";
            if (comid != "999999")
            {
                condition = " com_id=" + comid;
            }
            else
            {
                condition = " com_id ! =1";
            }
            if (oid != 0)
            {
                condition += " and order_id =" + oid;
            }
            if (channelcomid != 0)
            {
                condition = condition + " and Channelid=" + channelcomid + "";
            } 
            if (key != "")
            {
                condition = condition + " and (payment_type='" + key + "' or order_id in ( select oid from b2b_pay where pay_name='" + key + "' or pay_phone='" + key + "' or trade_no='" + key + "'))";
            }

            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);

            List<B2b_Finance> list = new List<B2b_Finance>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_Finance
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Agent_id = reader.GetValue<int>("Agent_id"),
                        Eid = reader.GetValue<int>("Eid"),
                        Order_id = reader.GetValue<int>("Order_id"),
                        Money = reader.GetValue<decimal>("Money"),
                        Servicesname = reader.GetValue<string>("Servicesname"),
                        SerialNumber = reader.GetValue<string>("SerialNumber"),
                        Payment = reader.GetValue<int>("Payment"),
                        Payment_type = reader.GetValue<string>("Payment_type"),
                        Money_come = reader.GetValue<string>("Money_come"),
                        Over_money = reader.GetValue<decimal>("Over_money"),
                        Subdate = reader.GetValue<DateTime>("Subdate"),
                        Con_state = reader.GetValue<int>("Con_state"),
                        Remarks = reader.GetValue<string>("Remarks"),
                        Printscreen = reader.GetValue<int>("printscreen"),
                        Channelid = reader.GetValue<int>("Channelid"),
                        Paychannelstate = reader.GetValue<int>("Paychannelstate"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;

        }
        #endregion


        #region  获得订单分页列表
        internal List<B2b_Finance> FinanceallList(int comid, string stardate, string enddate)
        {
            enddate += " 23:59:59";

            string sql = "select * from b2b_finance where com_id=" + comid ;
            if (stardate !=""){
                sql +=" and Subdate>='" + stardate + "'";
            }
            if (stardate !=""){
                sql +=" and Subdate<='" + enddate + "'";
            }

            sql += " order by id desc ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<B2b_Finance> list = new List<B2b_Finance>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_Finance
                    {
                        Id = reader.GetValue<int>("Id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Agent_id = reader.GetValue<int>("Agent_id"),
                        Eid = reader.GetValue<int>("Eid"),
                        Order_id = reader.GetValue<int>("Order_id"),
                        Money = reader.GetValue<decimal>("Money"),
                        Servicesname = reader.GetValue<string>("Servicesname"),
                        SerialNumber = reader.GetValue<string>("SerialNumber"),
                        Payment = reader.GetValue<int>("Payment"),
                        Payment_type = reader.GetValue<string>("Payment_type"),
                        Money_come = reader.GetValue<string>("Money_come"),
                        Over_money = reader.GetValue<decimal>("Over_money"),
                        Subdate = reader.GetValue<DateTime>("Subdate"),
                        Con_state = reader.GetValue<int>("Con_state"),
                        Remarks = reader.GetValue<string>("Remarks"),
                        Printscreen = reader.GetValue<int>("printscreen"),
                        Channelid = reader.GetValue<int>("Channelid"),
                        Paychannelstate = reader.GetValue<int>("Paychannelstate"),
                    });


                }
            }

            return list;

        }
        #endregion


        #region  获得订单分页列表
        internal List<B2b_Finance> Financecount(string comid, string stardate, string enddate)
        {
            enddate += " 23:59:59";

            string sql = "select payment_type,Money_come,sum(money) as money from b2b_finance where com_id=" + comid + " and Subdate>='" + stardate + "' and Subdate<='" + enddate + "' group by payment_type,Money_come order by Money_come,payment_type,sum(money) desc ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);


            List<B2b_Finance> list = new List<B2b_Finance>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_Finance
                    {

                        Money = reader.GetValue<decimal>("Money"),
                        Payment_type = reader.GetValue<string>("Payment_type"),
                        Money_come = reader.GetValue<string>("Money_come"),
                       
                    });

                }
            }

            return list;

        }
        #endregion

        #region 获得商家付款类型
        /// <summary>
        ///  获得商家付款类型
        /// </summary>
        /// <param name="comid"></param>
        /// <returns></returns>
        internal B2b_finance_paytype FinancePayType(int companyid)
        {
            const string sqlTxt = @"SELECT  [ID]
                  ,[com_id]
                  ,[paytype]
                  ,[Bank_account]
                  ,[Bank_card]
                  ,[Bank_name]
                  ,[alipay_account]
                  ,[alipay_id]
                  ,[alipay_key]
                  ,[userBank_account]
                  ,[userBank_card]
                  ,[userBank_name]
                  ,uptype 
                  ,  wx_appid
                  ,  wx_appkey
                  ,  wx_paysignkey
                  ,  wx_partnerid
                  ,  wx_partnerkey
                  ,  tenpay_id
                  , tenpay_key
                  ,wx_SSLCERT_PATH
                  ,wx_SSLCERT_PASSWORD 
              FROM  [dbo].[b2b_finance_paytype] a where a.com_ID=@ID";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@ID", companyid);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_finance_paytype u = null;

                while (reader.Read())
                {
                    u = new B2b_finance_paytype
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Paytype = reader.GetValue<int>("Paytype"),
                        Bank_account = reader.GetValue<string>("Bank_account"),
                        Bank_card = reader.GetValue<string>("Bank_card"),
                        Bank_name = reader.GetValue<string>("Bank_name"),
                        Alipay_account = reader.GetValue<string>("Alipay_account"),
                        Alipay_id = reader.GetValue<string>("Alipay_id"),
                        Alipay_key = reader.GetValue<string>("Alipay_key"),
                        Userbank_account = reader.GetValue<string>("Userbank_account"),
                        Userbank_card = reader.GetValue<string>("Userbank_card"),
                        Userbank_name = reader.GetValue<string>("Userbank_name"),
                        Uptype = reader.GetValue<int>("uptype"),
                        Wx_appid = reader.GetValue<string>("Wx_appid"),
                        Wx_appkey = reader.GetValue<string>("Wx_appkey"),
                        Wx_paysignkey = reader.GetValue<string>("Wx_paysignkey"),
                        Wx_partnerid = reader.GetValue<string>("wx_partnerid"),
                        Wx_partnerkey = reader.GetValue<string>("Wx_partnerkey"),
                        Tenpay_id = reader.GetValue<string>("Tenpay_id"),
                        Tenpay_key = reader.GetValue<string>("Tenpay_key"),
                        wx_SSLCERT_PATH = reader.GetValue<string>("wx_SSLCERT_PATH"),
                        wx_SSLCERT_PASSWORD = reader.GetValue<string>("wx_SSLCERT_PASSWORD"),
                    };

                }
                return u;
            }
        }
        #endregion



        internal decimal GetFinanceAmount(int comid, string payment_type)
        {
            try
            {
                string sql = "select sum(abs(money)) from b2b_finance where payment_type='" + payment_type + "' and com_id=" + comid;

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null || o.ToString() == "" ? 0 : decimal.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal decimal GetShouruAmount(int orderid)
        {
            try
            {
                string sql = "select money from b2b_finance where payment_type !='手续费' and order_id=" + orderid;

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null || o.ToString() == "" ? 0 : decimal.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }



        internal decimal GetShouxufeiAmount(int orderid)
        {
            try
            {
                string sql = "select money from b2b_finance where payment_type='手续费' and order_id=" + orderid;

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null || o.ToString() == "" ? 0 : decimal.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal int GetPayidbyorderid(int Shopcartid)
        {
            try
            {
                string sql = " SELECT  [oid]   FROM  [b2b_pay] where oid in (select id from b2b_order where shopcartid=" + Shopcartid + ") and [trade_status]='TRADE_SUCCESS'";

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null || o.ToString() == "" ? 0 : int.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal decimal GetChannelFinanceCount(int comid, int channelcomid)
        {
            try
            {
                string sql = "select sum(abs(money)) from b2b_finance where channelid='" + channelcomid + "' and com_id=" + comid;

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null || o.ToString() == "" ? 0 : decimal.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal string GetAgentNamebyorderid(int orderid)
        {
            try
            {
                string sql = "select company from agent_company where id in(select agentid from b2b_order where id=" + orderid + ")";

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null || o.ToString() == "" ? "" : o.ToString();
            }
            catch
            {
                sqlHelper.Dispose();
                return "";
            }
        }



        #region 活动加载明细列表
        internal List<Member_Integral> IntegralList(int pageindex, int pagesize, int comid, out int totalcount, int mid = 0, string key = "")
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "Member_Integral_Log";
            var strGetFields = "*";
            var sortKey = "ID";
            var sortMode = "1";

            var condition = " Com_id=" + comid;
            if (mid != 0)
            {
                condition += " and mid=" + mid;
            }

            if (key != "")
            {
                condition += " and mid in (select id from b2b_crm where phone='"+key+"'  )";
            }

            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);

            List<Member_Integral> list = new List<Member_Integral>();

            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Integral
                    {
                        Id = reader.GetValue<int>("ID"),
                        OrderId = reader.GetValue<decimal>("OrderId"),
                        Mid = reader.GetValue<int>("Mid"),
                        Money = reader.GetValue<decimal>("Money"),
                        OrderName = reader.GetValue<string>("OrderName"),
                        Subdate = reader.GetValue<DateTime>("Subdate"),
                        Comid = reader.GetValue<int>("Com_id"),
                        Ptype = reader.GetValue<int>("Ptype"),
                        Admin = reader.GetValue<string>("Admin"),
                        Ip = reader.GetValue<string>("Ip")

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        #endregion

        #region 活动加载明细列表
        internal List<Member_Imprest> ImprestList(int pageindex, int pagesize, int comid, out int totalcount, int mid = 0,string key="")
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "Member_Imprest_Log";
            var strGetFields = "*";
            var sortKey = "ID";
            var sortMode = "1";

            var condition = " Com_id=" + comid;
            if (mid != 0)
            {
                condition += " and mid=" + mid;
            }
            if (key != "")
            {
                condition += " and mid in (select id from b2b_crm where phone='" + key + "')";
            }

            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);

            List<Member_Imprest> list = new List<Member_Imprest>();

            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Member_Imprest
                    {
                        Id = reader.GetValue<int>("ID"),
                        OrderId = reader.GetValue<decimal>("OrderId"),
                        Mid = reader.GetValue<int>("Mid"),
                        Money = reader.GetValue<decimal>("Money"),
                        OrderName = reader.GetValue<string>("OrderName"),
                        Subdate = reader.GetValue<DateTime>("Subdate"),
                        Comid = reader.GetValue<int>("Com_id"),
                        Ptype = reader.GetValue<int>("Ptype"),
                        Admin = reader.GetValue<string>("Admin"),
                        Ip = reader.GetValue<string>("Ip")

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        #endregion

        internal decimal IntegralCount(int comid)
        {
            try
            {
                string sql = "select sum(Integral) from b2b_crm where com_id=" + comid;

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null || o.ToString() == "" ? 0 : decimal.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }
        internal decimal ImprestCount(int comid)
        {
            try
            {
                string sql = "select sum(imprest) from b2b_crm where com_id=" + comid;

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null || o.ToString() == "" ? 0 : decimal.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal int huoqupayorder(int orderid, decimal price)
        {
            try
            {
                string sql = "select id from b2b_pay where oid=" + orderid + " and total_fee=" + price;

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null || o.ToString() == "" ? 0 : int.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }


        internal List<string> Selpayment_type()
        {
            string sql = "select distinct(payment_type) as  payment_type from b2b_finance";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<string> list = new List<string>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(reader.GetValue<string>("payment_type"));
                }
                return list;
            }
        }

        internal List<string> Selmoney_come()
        {
            string sql = "select distinct(money_come) as  money_come from b2b_finance";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<string> list = new List<string>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(reader.GetValue<string>("money_come"));
                }
                return list;
            }
        }
    }
}

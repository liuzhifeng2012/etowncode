using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using Newtonsoft.Json;
using ETS2.PM.Service.PMService.Data;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.CRM.Service.CRMService.Modle;
using System.Collections;
using ETS.Framework;
using System.Data;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Modle;
using FileUpload.FileUpload.Data;
using ETS2.Member.Service.MemberService.Data;
using ETS2.Common.Business;



namespace ETS.JsonFactory
{
    public class FinanceJsonData
    {

        #region 获取财务记录
        public static string FinancePageList(string comid, int pageindex, int pagesize, string key)
        {
            var totalcount = 0;
            try
            {

                var financedata = new B2bFinanceData();
                var orderdata = new B2bOrderData();
                var channeldata = new MemberChannelcompanyData();


                var list = financedata.FinancePageList(comid, pageindex, pagesize, key, out totalcount);
                IEnumerable result = "";
                if (list != null)

                    result = from finance in list
                             select new
                             {

                                 Id = finance.Id,
                                 Com_id = finance.Com_id,
                                 ComName = B2bCompanyData.GetCompany(finance.Com_id) == null ? "" : B2bCompanyData.GetCompany(finance.Com_id).Com_name,
                                 Agent_id = finance.Agent_id,
                                 Eid = finance.Eid,
                                 Order_id = finance.Order_id,
                                 Servicesname = finance.Servicesname,
                                 ShortServicesname = finance.Servicesname.Length > 15 ? finance.Servicesname.Substring(0, 15) + "." : finance.Servicesname,
                                 SerialNumber = finance.SerialNumber,
                                 Money = finance.Money,
                                 Money_come = finance.Money_come,
                                 Over_money = finance.Over_money,
                                 Payment = finance.Payment,
                                 Payment_type = finance.Payment_type,
                                 Subdate = finance.Subdate,
                                 Con_state = finance.Con_state,
                                 Remarks = finance.Remarks,
                                 PrintscreenUrl = FileSerivce.GetImgUrl(finance.Printscreen.ToString().ConvertTo<int>(0)),
                                 Pno = orderdata.GetPnoByOrderId(finance.Order_id),
                                 Channelid = finance.Channelid,
                                 ChannelName = channeldata.GetChannelCompanyNameById(finance.Channelid),
                                 Paychannelstate = finance.Paychannelstate,
                                 Agentname = financedata.GetAgentNamebyorderid(finance.Order_id)
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 获取财务记录
        public static string Financecount(string comid, string stardate, string enddate)
        {
            var totalcount = 0;
            try
            {

                var financedata = new B2bFinanceData();
                var orderdata = new B2bOrderData();
                var channeldata = new MemberChannelcompanyData();


                var list = financedata.Financecount(comid, stardate, enddate);


                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 获取所有
        public static string ComFinancePageList(string comid, int pageindex, int pagesize, string key, int oid = 0, string payment_type = "", string money_come = "", string starttime = "", string endtime = "")
        {
            var totalcount = 0;
            try
            {

                var financedata = new B2bFinanceData();
                var orderdata = new B2bOrderData();
                var channeldata = new MemberChannelcompanyData();

                B2bPayData datapay = new B2bPayData();
                var list = financedata.FinancePageList(comid, pageindex, pagesize, key, out totalcount, 0, oid, payment_type, money_come, starttime, endtime);
                IEnumerable result = "";
                if (list != null)

                    result = from finance in list
                             select new
                             {

                                 Id = finance.Id,
                                 Com_id = finance.Com_id,
                                 ComName = B2bCompanyData.GetCompany(finance.Com_id).Com_name,
                                 Agent_id = finance.Agent_id,
                                 Eid = finance.Eid,
                                 Order_id = finance.Order_id,
                                 Servicesname = finance.Servicesname,
                                 ShortServicesname = finance.Servicesname.Length > 15 ? finance.Servicesname.Substring(0, 15) + "." : finance.Servicesname,
                                 SerialNumber = finance.SerialNumber,
                                 Money = finance.Money,
                                 Money_come = finance.Money_come,
                                 Over_money = finance.Over_money,
                                 Payment = finance.Payment,
                                 Payment_type = finance.Payment_type,
                                 Subdate = finance.Subdate,
                                 Con_state = finance.Con_state,
                                 Remarks = finance.Remarks,
                                 Pno = orderdata.GetPnoByOrderId(finance.Order_id),
                                 Channelid = finance.Channelid,
                                 ChannelName = channeldata.GetChannelCompanyNameById(finance.Channelid),
                                 Paychannelstate = finance.Paychannelstate,
                                 Agentname = financedata.GetAgentNamebyorderid(finance.Order_id),
                                 Payinfo = datapay.GetPayByoId(finance.Order_id)

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 渠道、门市获取财务记录
        public static string ChannelFPageList(string comid, int pageindex, int pagesize, string key, int channelcomid)
        {
            var totalcount = 0;

            if (channelcomid == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "传递参数错误" });
            }

            try
            {




                var financedata = new B2bFinanceData();
                var orderdata = new B2bOrderData();
                var channeldata = new MemberChannelcompanyData();


                var list = financedata.FinancePageList(comid, pageindex, pagesize, key, out totalcount, channelcomid);
                IEnumerable result = "";
                if (list != null)

                    result = from finance in list
                             select new
                             {

                                 Id = finance.Id,
                                 Com_id = finance.Com_id,
                                 ComName = B2bCompanyData.GetCompany(finance.Com_id).Com_name,
                                 Agent_id = finance.Agent_id,
                                 Eid = finance.Eid,
                                 Order_id = finance.Order_id,
                                 Servicesname = finance.Servicesname,
                                 ShortServicesname = finance.Servicesname.Length > 15 ? finance.Servicesname.Substring(0, 15) + "." : finance.Servicesname,
                                 SerialNumber = finance.SerialNumber,
                                 Money = finance.Money,
                                 Money_come = finance.Money_come,
                                 Over_money = finance.Over_money,
                                 Payment = finance.Payment,
                                 Payment_type = finance.Payment_type,
                                 Subdate = finance.Subdate,
                                 Con_state = finance.Con_state,
                                 Remarks = finance.Remarks,
                                 PrintscreenUrl = new FileUploadData().GetFileById(finance.Printscreen.ToString().ConvertTo<int>(0)) == null ? "" : new FileUploadData().GetFileById(finance.Printscreen.ToString().ConvertTo<int>(0)).Relativepath,
                                 Pno = orderdata.GetPnoByOrderId(finance.Order_id),
                                 Channelid = finance.Channelid,
                                 ChannelName = channeldata.GetChannelCompanyNameById(finance.Channelid),
                                 Paychannelstate = finance.Paychannelstate
                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region 获取支付信息
        public static string FinancePayType(int comid)
        {
            try
            {
                B2bFinanceData fdate = new B2bFinanceData();
                B2b_finance_paytype com = fdate.FinancePayType(comid);
                return JsonConvert.SerializeObject(new { type = 100, msg = com });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }
        #endregion

        #region 财务提现确认
        public static string WithdrawConf(int id, int comid, string remarks, int printscreen)
        {
            try
            {
                B2bFinanceData fdate = new B2bFinanceData();
                int com = fdate.WithdrawConf(id, comid, remarks, printscreen);

                if (com < 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, msg = "失败,请重新操作" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { type = 100, msg = "处理成功" });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }
        #endregion



        #region 提现申请
        public static string ModifyFinanceWithdraw(B2b_Finance fdateinfo)
        {

            try
            {
                B2bFinanceData fdate = new B2bFinanceData();
                var id = fdate.InsertFinanceWithdraw(fdateinfo);

                return JsonConvert.SerializeObject(new { type = 100, msg = id });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }
        #endregion


        #region 修改或插入支付信息
        public static string ModifyFinancePayType(B2b_Finance fdateinfo)
        {

            try
            {
                B2bFinanceData fdate = new B2bFinanceData();
                var id = fdate.InsertOrUpdateFinancePayType(fdateinfo);

                return JsonConvert.SerializeObject(new { type = 100, msg = id });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }
        #endregion

        #region 修改或插入支付信息
        public static string ModifyFinancePayBank(B2b_finance_paytype fdateinfo)
        {

            try
            {
                B2bFinanceData fdate = new B2bFinanceData();
                var id = fdate.InsertOrUpdateFinancePayBank(fdateinfo);

                return JsonConvert.SerializeObject(new { type = 100, msg = id });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }
        #endregion

        #region 修改微信支付
        public static string ModifyFinancePayWX(B2b_finance_paytype fdateinfo)
        {

            try
            {
                B2bFinanceData fdate = new B2bFinanceData();
                var id = fdate.InsertOrUpdateFinancePayWX(fdateinfo);

                return JsonConvert.SerializeObject(new { type = 100, msg = id });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }

        }
        #endregion


        #region 获取渠道返佣总金额
        public static string GetChannelFinanceCount(int comid, int channelcomid)
        {
            try
            {
                B2bFinanceData fdate = new B2bFinanceData();
                var com = fdate.GetChannelFinanceCount(comid, channelcomid);
                return JsonConvert.SerializeObject(new { type = 100, msg = com });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
        }
        #endregion



        #region  积分获得使用详情
        public static string IntegralList(int pageindex, int pagesize, int comid,string key="")
        {
            int totalcount = 0;
            try
            {
                B2bFinanceData fdate = new B2bFinanceData();
                var crmdata = new B2bCrmData();
                var list = fdate.IntegralList(pageindex, pagesize, comid, out totalcount,0,key);

                IEnumerable result = "";
                if (list != null)

                    result = from finance in list
                             select new
                             {

                                 Id = finance.Id,
                                 OrderId = finance.OrderId,
                                 Mid = finance.Mid,
                                 Money = finance.Money,
                                 OrderName = finance.OrderName,
                                 Subdate = finance.Subdate,
                                 Comid = finance.Comid,
                                 Ptype = finance.Ptype,
                                 Admin = finance.Admin,
                                 Ip = finance.Ip,
                                 Crm = crmdata.GetB2bCrmById(finance.Mid)

                             };





                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, totalCount = 0, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region  积分获得汇总
        public static string IntegralCount(int comid)
        {
            int totalcount = 0;
            try
            {
                B2bFinanceData fdate = new B2bFinanceData();
                var list = fdate.IntegralCount(comid);

                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region  积分获得使用详情
        public static string ImprestList(int pageindex, int pagesize, int comid, string key = "")
        {
            int totalcount = 0;
            var crmdata = new B2bCrmData();
            try
            {
                B2bFinanceData fdate = new B2bFinanceData();
                var list = fdate.ImprestList(pageindex, pagesize, comid, out totalcount,0,key);
                IEnumerable result = "";
                if (list != null)

                    result = from finance in list
                             select new
                             {

                                 Id = finance.Id,
                                 OrderId = finance.OrderId,
                                 Mid = finance.Mid,
                                 Money = finance.Money,
                                 OrderName = finance.OrderName,
                                 Subdate = finance.Subdate,
                                 Comid = finance.Comid,
                                 Ptype = finance.Ptype,
                                 Admin = finance.Admin,
                                 Ip = finance.Ip,
                                 Crm = crmdata.GetB2bCrmById(finance.Mid)

                             };

                return JsonConvert.SerializeObject(new { type = 100, totalCount = totalcount, msg = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, totalCount = 0, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region  积分获得使用详情
        public static string ImprestCount(int comid)
        {
            int totalcount = 0;
            try
            {
                B2bFinanceData fdate = new B2bFinanceData();
                var list = fdate.ImprestCount(comid);

                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, totalCount = 0, msg = ex.Message });
                throw;
            }
        }
        #endregion

        #region  积分获得使用详情
        public static string shougongqueren(string trade_no, int order_no, decimal total_fee)
        {
            try
            {
                B2bFinanceData fdate = new B2bFinanceData();
                var list = fdate.huoqupayorder(order_no, total_fee);
                if (list == 0)
                {
                    return JsonConvert.SerializeObject(new { type = 1, totalCount = 0, msg = "没有此订单" });
                }
                else
                {
                    string retunstr = new PayReturnSendEticketData().PayReturnSendEticket(trade_no, order_no, total_fee, "TRADE_SUCCESS");

                    return JsonConvert.SerializeObject(new { type = 100, msg = retunstr });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { type = 1, totalCount = 0, msg = ex.Message });
                throw;
            }
        }
        #endregion

        /// <summary>
        /// 得到交易类型列表
        /// </summary>
        /// <returns></returns>
        public static string Selpayment_type()
        {
            IList<string> list = new B2bFinanceData().Selpayment_type();
            if (list.Count == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
        }
        //支付方式
        public static string Selmoney_come()
        {
            IList<string> list = new B2bFinanceData().Selmoney_come();
            if (list.Count == 0)
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
        }

        public static string FinanceStat(int comid, string begindate, string enddate, int projectid, int productid)
        {
            //直销售卖XXX张 退票XXX张  应收XXX元 退款XXX元 实收XXX元  => 直销订单统计
            //分销售卖XXX张: 销售扣款XXX张 退票XXX张   应收XXX元 退款XXX元 实收XXX元    => 分销订单统计1   
            //               验证扣款XXX张 退票XXX张   应收XXX元 退款XXX元 实收XXX元    =>分销订单统计2
            //.................................手续费 和 核销情况(XXX张已经验证，XXX张尚未验证) 点击后才显示.............................

            SqlHelper sqlHelper = new SqlHelper();
            var condition = "";
            if (begindate != "" && enddate != "")
            {
                begindate = DateTime.Parse(begindate).ToString("yyyy-MM-dd 00:00:00");
                enddate = DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59");
                condition = condition + " and u_subdate between '" + begindate + "' and '" + enddate + "'";
            }
            if (projectid != 0)
            {
                condition = condition + " and pro_id in (select id from b2b_com_pro where projectid=" + projectid + ")";
            }
            if (productid != 0)
            {
                condition = condition + " and pro_id =" + productid;
            }

            sqlHelper.BeginTrancation();
            try
            {
                //直销售卖情况
                string sql1 = "select sum(u_num) as totalnum,sum(cancel_ticketnum) as cancelnum,sum(u_num*pay_price) as shouldmoney,sum(cancel_ticketnum*pay_price) as cancelmoney,sum(u_num*pay_price-cancel_ticketnum*pay_price) as factmoney from b2b_order where   comid=" + comid + " and agentid=0 " + condition;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql1);
                DataTable t11 = new DataTable("t11");
                DataColumn dc11 = null;
                dc11 = t11.Columns.Add("totalnum", Type.GetType("System.Int32"));
                dc11 = t11.Columns.Add("cancelnum", Type.GetType("System.Int32"));
                dc11 = t11.Columns.Add("shouldmoney", Type.GetType("System.String"));
                dc11 = t11.Columns.Add("cancelmoney", Type.GetType("System.String"));
                dc11 = t11.Columns.Add("factmoney", Type.GetType("System.String"));
                using (var reader11 = cmd.ExecuteReader())
                {
                    DataRow newRow11;
                    if (reader11.Read())
                    {
                        newRow11 = t11.NewRow();
                        newRow11["totalnum"] = reader11.GetValue<int>("totalnum");
                        newRow11["cancelnum"] = reader11.GetValue<int>("cancelnum");
                        newRow11["shouldmoney"] = reader11.GetValue<decimal>("shouldmoney").ToString();
                        newRow11["cancelmoney"] = reader11.GetValue<decimal>("cancelmoney").ToString();
                        newRow11["factmoney"] = reader11.GetValue<decimal>("factmoney").ToString();
                        t11.Rows.Add(newRow11);
                    }
                    else
                    {
                        t11 = null;
                    }
                }


                //分销(销售扣款)销售情况
                string sql2 = "select sum(u_num) as totalnum,sum(cancel_ticketnum) as cancelnum,sum(u_num*pay_price) as shouldmoney,sum(cancel_ticketnum*pay_price) as cancelmoney,sum(u_num*pay_price-cancel_ticketnum*pay_price) as factmoney from b2b_order where   comid=" + comid + " and agentid>0 and warrant_type=1 " + condition;
                cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                DataTable t22 = new DataTable("t22");
                DataColumn dc22 = null;
                dc22 = t22.Columns.Add("totalnum", Type.GetType("System.Int32"));
                dc22 = t22.Columns.Add("cancelnum", Type.GetType("System.Int32"));
                dc22 = t22.Columns.Add("shouldmoney", Type.GetType("System.String"));
                dc22 = t22.Columns.Add("cancelmoney", Type.GetType("System.String"));
                dc22 = t22.Columns.Add("factmoney", Type.GetType("System.String"));
                using (var reader22 = cmd.ExecuteReader())
                {
                    DataRow newRow22;
                    if (reader22.Read())
                    {
                        newRow22 = t22.NewRow();
                        newRow22["totalnum"] = reader22.GetValue<int>("totalnum");
                        newRow22["cancelnum"] = reader22.GetValue<int>("cancelnum");
                        newRow22["shouldmoney"] = reader22.GetValue<decimal>("shouldmoney").ToString();
                        newRow22["cancelmoney"] = reader22.GetValue<decimal>("cancelmoney").ToString();
                        newRow22["factmoney"] = reader22.GetValue<decimal>("factmoney").ToString();
                        t22.Rows.Add(newRow22);
                    }
                    else
                    {
                        t22 = null;
                    }
                }


                //分销(验证扣款)销售情况 因为是验证扣款，所以没有计算实际收款金额
                string sql3 = "select sum(u_num) as totalnum,sum(cancel_ticketnum) as cancelnum  from b2b_order where   comid=" + comid + " and agentid>0  and warrant_type=2 " + condition;
                cmd = sqlHelper.PrepareTextSqlCommand(sql3);
                DataTable t33 = new DataTable("t33");
                DataColumn dc33 = null;
                dc33 = t33.Columns.Add("totalnum", Type.GetType("System.Int32"));
                dc33 = t33.Columns.Add("cancelnum", Type.GetType("System.Int32"));
                dc33 = t33.Columns.Add("shouldmoney", Type.GetType("System.String"));
                dc33 = t33.Columns.Add("cancelmoney", Type.GetType("System.String"));
                using (var reader33 = cmd.ExecuteReader())
                {
                    DataRow newRow33;
                    if (reader33.Read())
                    {
                        newRow33 = t33.NewRow();
                        newRow33["totalnum"] = reader33.GetValue<int>("totalnum");
                        newRow33["cancelnum"] = reader33.GetValue<int>("cancelnum");
                        newRow33["shouldmoney"] = "0";
                        newRow33["cancelmoney"] = "0";
                        t33.Rows.Add(newRow33);
                    }
                    else
                    {
                        t33 = null;
                    }
                }
                //分销(验证扣款)销售情况：只计算验票收款金额
                string sql4 = "select sum(money) as factmoney from agent_Financial where orderid in (select id from b2b_order where  comid=" + comid + " and agentid>0 and warrant_type=2 " + condition + ")";
                cmd = sqlHelper.PrepareTextSqlCommand(sql4);
                DataTable t33_2 = new DataTable("t33_2");
                DataColumn dc33_2 = null;
                dc33_2 = t33_2.Columns.Add("factmoney", Type.GetType("System.String"));
                using (var reader33_2 = cmd.ExecuteReader())
                {
                    DataRow newRow33_2;
                    if (reader33_2.Read())
                    {
                        newRow33_2 = t33_2.NewRow();
                        newRow33_2["factmoney"] = reader33_2.GetValue<decimal>("factmoney").ToString();
                        t33_2.Rows.Add(newRow33_2);
                    }
                    else
                    {
                        t33_2 = null;
                    }
                }


                sqlHelper.Commit();
                return JsonConvert.SerializeObject(new { type = 100, directDt1 = t11, agentDt2 = t22, agentDt31 = t33, agentDt32 = t33_2 });
            }
            catch (Exception ex)
            {
                sqlHelper.Rollback();
                return JsonConvert.SerializeObject(new { type = 1, msg = ex.Message });
            }
            finally
            {
                sqlHelper.Dispose();
            }
        }

        public static string HotelOrderStat(int comid, string begindate, string enddate, int projectid, int productid, string key, string orderstate)
        {
            var condition = " where  a.order_state in (2,4,8,22)  and a.pro_id in (select id from b2b_com_pro where server_type=9 and comid=" + comid + ") and a.comid=" + comid;
            if (begindate != "" && enddate != "")
            {
                begindate = DateTime.Parse(begindate).ToString("yyyy-MM-dd 00:00:00");
                enddate = DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59");
                condition = condition + " and b.start_date between '" + begindate + "' and '" + enddate + "'";
            }
            if (projectid != 0)
            {
                condition = condition + " and a.pro_id in (select id from b2b_com_pro where projectid=" + projectid + ")";
            }
            if (productid != 0)
            {
                condition = condition + " and a.pro_id =" + productid;
            }
            if (key != "")
            {
                try
                {
                    int intkey = int.Parse(key);
                    condition = condition + " and a.pro_id=" + intkey;
                }
                catch
                {
                    condition = condition + " and a.pro_id in (select id from b2b_com_pro where pro_name='" + key + "' and comid=" + comid + ")";
                }
            }



            string sql = "select  sum(a.u_num*b.bookdaynum) as jianye,a.pro_id ,count(1) as ordertcount,c.pro_name,sum(a.u_num*b.bookdaynum*a.pay_price) as price from b2b_order as a left join b2b_order_hotel as b on a.id=b.orderid left join b2b_com_pro as c on a.pro_id=c.id   " + condition + " group by a.pro_id,c.pro_name  order by a.pro_id";
            DataTable dt = ExcelSqlHelper.ExecuteDataTable(sql);
            if (dt != null)
            {
                DataTable tblDatas = new DataTable("Datas");
                DataColumn dc = null;
                dc = tblDatas.Columns.Add("ID", Type.GetType("System.Int32"));
                dc.AutoIncrement = true;//自动增加
                dc.AutoIncrementSeed = 1;//起始为1
                dc.AutoIncrementStep = 1;//步长为1
                dc.AllowDBNull = false;

                dc = tblDatas.Columns.Add("jianye", Type.GetType("System.Int32"));
                dc = tblDatas.Columns.Add("pro_id", Type.GetType("System.Int32"));
                dc = tblDatas.Columns.Add("pro_name", Type.GetType("System.String"));
                dc = tblDatas.Columns.Add("ordertcount", Type.GetType("System.Int32"));
                dc = tblDatas.Columns.Add("projectname", Type.GetType("System.String"));
                dc = tblDatas.Columns.Add("price", Type.GetType("System.String"));

                DataRow newRow;
                foreach (DataRow row in dt.Rows)
                {
                    newRow = tblDatas.NewRow();

                    newRow["jianye"] = row["jianye"].ToString().ConvertTo<int>(9999);//出错的话间夜返回9999
                    newRow["pro_id"] = int.Parse(row["pro_id"].ToString());
                    newRow["pro_name"] = row["pro_name"].ToString();
                    newRow["ordertcount"] = row["ordertcount"].ToString().ConvertTo<int>(9999);//出错的话间夜返回9999
                    newRow["projectname"] = new B2bComProData().GetProjectNameByProid(row["pro_id"].ToString());
                    newRow["price"] = row["price"].ToString();
                    tblDatas.Rows.Add(newRow);
                }


                return JsonConvert.SerializeObject(new { type = 100, msg = tblDatas });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "订房信息为空" });
            }
        }

        public static string HotelOrderlist(int comid, string begindate, string enddate, int productid, string orderstate)
        {
            DataTable tblDatas = new B2bOrderData().HotelOrderlist(comid, begindate, enddate, productid, orderstate);

            if (tblDatas != null)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = tblDatas });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }

        public static string Selprojectlist(int comid)
        {
            List<B2b_com_project> list = new B2b_com_projectData().Selprojectlist(comid);
            if (list.Count > 0)
            {
                IEnumerable result = "";
                    result = from finance in list
                             select new
                             {
                                 Id = finance.Id,
                                 Projectname = finance.Projectname
                             };

                return JsonConvert.SerializeObject(new { type = 100, msg = result });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }

        public static string Selhotelproductlist(int comid, int projectid)
        {
            List<B2b_com_pro> list = new B2bComProData().Selhotelproductlist(comid, projectid);
            if (list.Count > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }

        public static string Selhotelprojectlist(int comid)
        {
            List<B2b_com_project> list = new B2b_com_projectData().Selhotelprojectlist(comid);
            if (list.Count > 0)
            {
                return JsonConvert.SerializeObject(new { type = 100, msg = list });
            }
            else
            {
                return JsonConvert.SerializeObject(new { type = 1, msg = "" });
            }
        }
    }
}

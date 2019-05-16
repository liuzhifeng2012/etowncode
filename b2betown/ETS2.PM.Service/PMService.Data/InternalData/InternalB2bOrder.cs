using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Modle.Enum;
using System.Data;
using System.Text.RegularExpressions;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalB2bOrder
    {
        private SqlHelper sqlHelper;
        public InternalB2bOrder(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        #region 添加或者编辑订单信息

        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateB2bOrder";

        public int InsertOrUpdate(B2b_order model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Pro_id", model.Pro_id);
            cmd.AddParam("@Order_type", model.Order_type);
            cmd.AddParam("@U_name", model.U_name);
            cmd.AddParam("@U_id", model.U_id);
            cmd.AddParam("@U_phone", model.U_phone);
            cmd.AddParam("@U_idcard", model.U_idcard);
            cmd.AddParam("@U_num", model.U_num);
            cmd.AddParam("@U_subdate", model.U_subdate);
            cmd.AddParam("@Payid", model.Payid);
            cmd.AddParam("@Pay_price", model.Pay_price);
            cmd.AddParam("@Cost", model.Cost);
            cmd.AddParam("@Profit", model.Profit);
            cmd.AddParam("@Order_state", model.Order_state);
            cmd.AddParam("@Pay_state", model.Pay_state);
            cmd.AddParam("@Send_state", model.Send_state);
            cmd.AddParam("@Ticketcode", model.Ticketcode);
            cmd.AddParam("@Rebate", model.Rebate);
            cmd.AddParam("@Ordercome", model.Ordercome);
            cmd.AddParam("@U_traveldate", model.U_traveldate);
            cmd.AddParam("@Dealer", model.Dealer);
            cmd.AddParam("@comid", model.Comid);
            cmd.AddParam("@agentid", model.Agentid);
            cmd.AddParam("@warrantid", model.Warrantid);
            cmd.AddParam("@warrant_type", model.Warrant_type);
            cmd.AddParam("@openid", model.Openid);
            cmd.AddParam("@Integral", model.Integral1);
            cmd.AddParam("@Imprest", model.Imprest1);
            cmd.AddParam("@Pno", model.Pno);
            cmd.AddParam("@ticket", model.Ticket);
            cmd.AddParam("@ticketinfo", model.Ticketinfo);
            cmd.AddParam("@Backtickettime", model.Backtickettime);

            cmd.AddParam("@service_order_num", model.Service_order_num);
            cmd.AddParam("@service_req_seq", model.Service_req_seq);
            cmd.AddParam("@order_remark", model.Order_remark);
            cmd.AddParam("@servicepro_v_state", model.Servicepro_v_state);
            cmd.AddParam("@buyuid", model.Buyuid);
            cmd.AddParam("@tocomid", model.Tocomid);
            cmd.AddParam("@Bindingagentorderid", model.Bindingagentorderid);
            cmd.AddParam("@Speciid", model.Speciid);
            cmd.AddParam("@channelcoachid", model.channelcoachid);
            cmd.AddParam("@Shopcartid", model.Shopcartid);

            cmd.AddParam("@submanagename", model.submanagename);
            cmd.AddParam("@mangefinset", model.mangefinset);
            cmd.AddParam("@payorder", model.payorder);
            

            cmd.AddParam("@baoxiannames", model.baoxiannames);
            cmd.AddParam("@baoxianpinyinnames", model.baoxianpinyinnames);
            cmd.AddParam("@baoxianidcards", model.baoxianidcards);
            cmd.AddParam("@serverid", model.serverid);



            if (model.service_code == null)
            {
                model.service_code = "";
            }
            if (model.service_lastcount == null)
            {
                model.service_lastcount = 0;
            }
            if (model.service_usecount == null)
            {
                model.service_usecount = 0;
            }
            cmd.AddParam("@service_code", model.service_code);
            cmd.AddParam("@service_lastcount", model.service_lastcount);
            cmd.AddParam("@service_usecount", model.service_usecount);


            if (model.Deliverytype == null)
            {
                model.Deliverytype = 0;
            }
            if (model.Province == null)
            {
                model.Province = "";
            }
            if (model.City == null)
            {
                model.City = "";
            }
            if (model.Address == null)
            {
                model.Address = "";
            }
            if (model.Code == null)
            {
                model.Code = "";
            }
            if (model.Express == null)
            {
                model.Express = 0;
            }


            if (model.Child_u_num == null)
            {
                model.Child_u_num = 0;
            }
            if (model.childreduce == null)
            {
                model.childreduce = 0;
            }

            cmd.AddParam("@Deliverytype", model.Deliverytype);
            cmd.AddParam("@Province", model.Province);
            cmd.AddParam("@City", model.City);
            cmd.AddParam("@Address", model.Address);
            cmd.AddParam("@Code", model.Code);
            cmd.AddParam("@Express", model.Express);

            cmd.AddParam("@Child_u_num", model.Child_u_num);

            cmd.AddParam("@pickuppoint", model.pickuppoint);
            cmd.AddParam("@dropoffpoint", model.dropoffpoint);

            cmd.AddParam("@childreduce", model.childreduce);
            cmd.AddParam("@yanzheng_method", model.yanzheng_method);
            cmd.AddParam("@yuyuepno", model.yuyuepno);

            if (model.travelnames == null)
            {
                model.travelnames = "";
            }
            if (model.travelidcards == null)
            {
                model.travelidcards = "";
            }
            if (model.travelnations == null)
            {
                model.travelnations = "";
            }
            if (model.travelphones == null)
            {
                model.travelphones = "";
            }
            if (model.travelremarks == null)
            {
                model.travelremarks = "";
            }
            cmd.AddParam("@travelnames", model.travelnames);
            cmd.AddParam("@travelidcards", model.travelidcards);
            cmd.AddParam("@travelnations", model.travelnations);
            cmd.AddParam("@travelphones", model.travelphones);
            cmd.AddParam("@travelremarks", model.travelremarks);
            if (model.isInterfaceSub == null)
            {
                model.isInterfaceSub = 0;
            }
            cmd.AddParam("@isInterfaceSub", model.isInterfaceSub);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion

        #region 购物车，订单
        public int CartInsertOrUpdate(B2b_order model)
        {
            string sqltxt = @"INSERT b2b_shopcartorder(agentid,comid,userid,openid,phone,name,paymoney,integral,imprest,province,city,code,address,expressfee,expresscom,expresscode)
                  VALUES(@agentid,@com_id,@userid,'',@phone,@name,@paymoney,0,0,@province,@city,@code,@address,@expressfee,'','');select @@identity;";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@com_id", model.Comid);
            cmd.AddParam("@agentid", model.Agentid);
            cmd.AddParam("@userid", model.U_id);
            cmd.AddParam("@name", model.U_name);
            cmd.AddParam("@phone", model.U_phone);
            cmd.AddParam("@province", model.Province);
            cmd.AddParam("@city", model.City);
            cmd.AddParam("@code", model.Code);
            cmd.AddParam("@address", model.Address);
            cmd.AddParam("@expressfee", model.Express);
            cmd.AddParam("@paymoney", model.Pay_price);

            object o = cmd.ExecuteScalar();
            int newId = o == null ? 0 : int.Parse(o.ToString());
            return newId;
        }
        #endregion
        #region  获得订单分页列表
        internal List<B2b_order> OrderPageList(string comid, int pageindex, int pagesize, string key, int order_state, int ordertype, out int totalcount, int userid = 0, int crmid = 0, int orderIsAccurateToPerson = 0, int channelcompanyid = 0, string begindate = "", string enddate = "", int servertype = 0, int datetype=0)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_order";
            var strGetFields = "*";
            var sortKey = "u_subdate";
            var sortMode = "1";
            var condition = "comid = " + comid;

            //订单精确到渠道人
            if (orderIsAccurateToPerson == 1)
            {
                if (userid > 0)
                {
                    condition += " and recommendchannelid in (select id from member_channel where mobile=(select tel from b2b_company_manageuser where id=" + userid + "))";
                }
            }
            else
            {
                if (channelcompanyid > 0)
                {
                    condition += " and  recommendchannelid in (select id from member_channel where companyid=" + channelcompanyid + " ) ";
                }
            }
            //if (channelcompanyid > 0)
            //{
            //    condition += " and u_id in (select  id from b2b_crm where idcard in (select cardcode from member_card where issuecard in (select id from member_channel where companyid=" + channelcompanyid + ")) )";
            //}

            if (ordertype == 1)
            {//直销订单
                condition = condition + " and Agentid =0";
            }

            if (ordertype == 2)
            {//分销后台订单
                condition = condition + " and Agentid !=0 and warrant_type=1 and Order_type=1";
            }
            if (ordertype == 3)
            {//分销倒码订单
                condition = condition + " and Agentid !=0 and warrant_type=2";
            }
            if (ordertype == 4)
            {//分销充值订单
                condition = condition + " and Agentid !=0 and Order_type=2 and warrant_type=1 ";
            }
            if (ordertype == 5)
            {//为了查询和预约相关的订单
                condition = condition + " and Pro_id in(SELECT proid FROM  Bus_Feeticket_pno)";
            }

            //按用户ID查询
            if (crmid != 0)
            {
                condition = condition + " and U_id = " + crmid;
            }


            if (key != "")
            {

                //判断是否为数字，是数字的则可以查询订单号
                Regex regex = new Regex("^[0-9]*$");
                bool isNum = regex.IsMatch(key.Trim());
                if (isNum == false)
                {
                    condition = condition + " and (U_name='" + key + "' or U_phone='" + key + "' or id in (select oid from b2b_eticket where pno='" + key + "') or pro_id in (select id from b2b_com_pro where com_id =" + comid + " and pro_name like '%" + key + "%'))";
                }
                else
                {
                    condition = condition + " and (U_name='" + key + "' or U_phone='" + key + "' or id=" + key + " or  id in (select oid from b2b_eticket where pno='" + key + "'))";
                }
            }
            if (order_state != 0)
            {
                condition = condition + " and order_state=" + order_state;
            }
            if (servertype != 0)
            {
                condition += " and pro_id in (select id from b2b_com_pro where server_type=" + servertype + ")";
            }
           
            if (begindate != "" && enddate != "")
            {
                    begindate = DateTime.Parse(begindate).ToString("yyyy-MM-dd 00:00:00");
                    enddate = DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59");
                    if (datetype == 0)
                    {//按订单查询
                        condition = condition + " and u_subdate between '" + begindate + "' and '" + enddate + "'";
                    }else{//按出游日期查询或按入住日期查询
                        condition = condition + " and U_traveldate between '" + begindate + "' and '" + enddate + "'";
                    }
            }
           


            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_order> list = new List<B2b_order>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_idcard = reader.GetValue<string>("U_idcard"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        U_traveldate = reader.GetValue<DateTime>("U_traveldate"),
                        Dealer = reader.GetValue<string>("dealer"),
                        Comid = reader.GetValue<int>("comid"),
                        Openid = reader.GetValue<string>("openid"),
                        Pno = reader.GetValue<string>("pno"),
                        Integral1 = reader.GetValue<decimal>("integral"),
                        Imprest1 = reader.GetValue<decimal>("imprest"),
                        Ticket = reader.GetValue<decimal>("Ticket"),
                        Ticketinfo = reader.GetValue<string>("ticketinfo") == null ? "" : reader.GetValue<string>("ticketinfo"),
                        Backtickettime = reader.GetValue<DateTime>("Backtickettime"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrantid = reader.GetValue<int>("Warrantid"),
                        Service_order_num = reader.GetValue<string>("Service_order_num"),
                        Service_req_seq = reader.GetValue<string>("Service_req_seq"),
                        Order_remark = reader.GetValue<string>("Order_remark"),
                        Servicepro_v_state = reader.GetValue<string>("Servicepro_v_state"),
                        Tocomid = reader.GetValue<int>("Tocomid"),
                        Buyuid = reader.GetValue<int>("Buyuid"),
                        Child_u_num = reader.GetValue<int>("child_u_num"),
                        Bindingagentorderid = reader.GetValue<int>("Bindingagentorderid"),
                        pickuppoint = reader.GetValue<string>("pickuppoint"),
                        dropoffpoint = reader.GetValue<string>("dropoffpoint"),
                        Cancelnum = reader.GetValue<int>("cancel_ticketnum"),

                        Province = reader.GetValue<string>("Province"),
                        City = reader.GetValue<string>("City"),
                        Address = reader.GetValue<string>("Address"),
                        Code = reader.GetValue<string>("Code"),
                        Express = reader.GetValue<decimal>("Expressfee"),
                        Expresscom = reader.GetValue<string>("Expresscom"),
                        Expresscode = reader.GetValue<string>("expresscode"),
                        Deliverytype = reader.GetValue<int>("Deliverytype"),
                        Shopcartid = reader.GetValue<int>("Shopcartid"),
                        childreduce = reader.GetValue<decimal>("childreduce"),
                        yanzheng_method = reader.GetValue<int>("yanzheng_method"),
                        Handlid = reader.GetValue<int>("Handlid"),
                        qunar_orderid = reader.GetValue<string>("qunar_orderid"),
                        askquitfee = reader.GetValue<decimal>("askquitfee"),
                        recommendchannelid = reader.GetValue<int>("recommendchannelid"),
                        Speciid = reader.GetValue<int>("Speciid"),
                        channelcoachid = reader.GetValue<int>("channelcoachid"),
                        baoxiannames = reader.GetValue<string>("baoxiannames"),
                        baoxianpinyinnames = reader.GetValue<string>("baoxianpinyinnames"),
                        baoxianidcards = reader.GetValue<string>("baoxianidcards"),

                        service_code = reader.GetValue<string>("service_code"),
                        service_usecount = reader.GetValue<int>("service_usecount"),
                        service_lastcount = reader.GetValue<int>("service_lastcount"),

                        bookpro_bindcompany = reader.GetValue<string>("bookpro_bindcompany"),
                        bookpro_bindconfirmtime = reader.GetValue<DateTime>("bookpro_bindconfirmtime"),
                        bookpro_bindname = reader.GetValue<string>("bookpro_bindname"),
                        bookpro_bindphone = reader.GetValue<string>("bookpro_bindphone"),
                        payorder = reader.GetValue<int>("payorder"),

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;

        }
        #endregion


        #region  获得订单分页列表
        internal List<B2b_order> channelcoachOrderPageList(string comid, int channlecoachid)
        {
            int count = 0;
            int totalcount = 0;
            //按天统计
            string sqltxt = @"select CONVERT(varchar(10),U_traveldate,120) as U_traveldate,count(id) as num from b2b_order where comid=@comid and channelcoachid=@channlecoachid and pay_state=2 group by CONVERT(varchar(10),U_traveldate,120)";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@channlecoachid", channlecoachid);

            using (var reader = cmd.ExecuteReader())
            {
                List<B2b_order> list = new List<B2b_order>();
                while (reader.Read())
                {

                    list.Add(new B2b_order
                    {
                        Address = reader.GetValue<string>("U_traveldate"),//用字符串代替时间，否则总报错
                        U_num = reader.GetValue<int>("num"),
                    });
                    count += 1;
                }
                totalcount = count;
                return list;
            }

        }
        #endregion



        #region  获得订单分页列表
        internal List<B2b_order> OrderCartPageList(string comid, int cartid, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_order";
            var strGetFields = "*";
            var sortKey = "u_subdate";
            var sortMode = "1";
            var condition = "comid = " + comid + " and shopcartid=" + cartid;



            cmd.PagingCommand(tblName, strGetFields, 1, 100, sortKey, sortMode, condition);


            List<B2b_order> list = new List<B2b_order>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        U_traveldate = reader.GetValue<DateTime>("U_traveldate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        Integral1 = reader.GetValue<decimal>("Integral"),
                        Imprest1 = reader.GetValue<decimal>("Imprest"),
                        Ticketinfo = reader.GetValue<string>("ticketinfo"),
                        Openid = reader.GetValue<string>("Openid"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrantid = reader.GetValue<int>("Warrantid"),
                        Comid = reader.GetValue<int>("Comid"),
                        Bindingagentorderid = reader.GetValue<int>("Bindingagentorderid"),
                        Pno = reader.GetValue<string>("Pno"),
                        Order_remark = reader.GetValue<string>("Order_remark"),

                        Province = reader.GetValue<string>("Province"),
                        City = reader.GetValue<string>("City"),
                        Address = reader.GetValue<string>("Address"),
                        Code = reader.GetValue<string>("Code"),
                        Express = reader.GetValue<decimal>("Expressfee"),
                        Expresscom = reader.GetValue<string>("Expresscom"),
                        Expresscode = reader.GetValue<string>("expresscode"),
                        Deliverytype = reader.GetValue<int>("Deliverytype"),
                        Shopcartid = reader.GetValue<int>("Shopcartid"),
                        Speciid = reader.GetValue<int>("Speciid"),
                        channelcoachid = reader.GetValue<int>("channelcoachid"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;

        }
        #endregion


        //订单统计未做完
        internal List<B2b_order> OrderCountList(string comid, string startime, string endtime, int searchtype, out int totalcount, int userid = 0)
        {
            int count = 0;
            //按天统计
            string sqltxt = @"select CONVERT(varchar(10),u_subdate,120) as subdate,count(id) as countnum , sum(u_num) as num,sum(u_num*pay_price)as pay_price,sum(integral) as integral,sum(profit) as profit from b2b_order where comid=@comid and agentid=0 and order_state in (2,4,8,22,16)  and u_subdate>=@startime and u_subdate<@endtime group by CONVERT(varchar(10),u_subdate,120) order by CONVERT(varchar(10),u_subdate,120)";
            //按月份统一
            if (searchtype == 2)
            {
                sqltxt = @"select CONVERT(varchar(7),u_subdate,120) as subdate,count(id) as countnum , sum(u_num) as num,sum(u_num*pay_price)as pay_price,sum(integral) as integral,sum(profit) as profit from b2b_order where comid=@comid and agentid=0 and order_state in (2,4,8,22,16) and u_subdate>=@startime and u_subdate<@endtime group by CONVERT(varchar(7),u_subdate,120) order by CONVERT(varchar(7),u_subdate,120)";
            }

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@startime", startime);
            cmd.AddParam("@endtime", endtime);

            using (var reader = cmd.ExecuteReader())
            {
                List<B2b_order> list = new List<B2b_order>();
                while (reader.Read())
                {

                    list.Add(new B2b_order
                   {
                       Datestr = reader.GetValue<string>("subdate"),
                       Countnum = reader.GetValue<int>("Countnum"),
                       Profit = reader.GetValue<decimal>("Profit"),
                       Integral1 = reader.GetValue<decimal>("integral"),
                       Pay_price = reader.GetValue<decimal>("pay_price"),
                       U_num = reader.GetValue<int>("num"),
                   });
                    count += 1;
                }
                totalcount = count;
                return list;
            }
        }

        //订单统计未做完
        internal List<B2b_order> Orderfinset(string comid, string startime, string endtime, int mangefinset,string key, out int totalcount)
        {
            int count = 0;

            endtime = DateTime.Parse(endtime).AddDays(1).ToString();//结束日期加1天


            //按天统计
            string sqltxt = @"select submanagename,count(id) as countnum , sum(u_num) as num,sum(u_num*pay_price)as pay_price from b2b_order where comid=@comid and agentid=0 and mangefinset=@mangefinset and pay_state=2 and order_state in (2,4,8,22) and U_subdate>=@startime and U_subdate<@endtime group by submanagename ";


            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@startime", startime);
            cmd.AddParam("@endtime", endtime);
            cmd.AddParam("@mangefinset", mangefinset);
            cmd.AddParam("@key", key);

            using (var reader = cmd.ExecuteReader())
            {
                List<B2b_order> list = new List<B2b_order>();
                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        Countnum = reader.GetValue<int>("Countnum"),
                        submanagename = reader.GetValue<string>("submanagename"),
                        Pay_price = reader.GetValue<decimal>("pay_price"),
                        U_num = reader.GetValue<int>("num"),
                    });
                    count += 1;
                }
                totalcount = count;
                return list;
            }
        }


        //前台提单 各种支付方式收取金额统计
        internal List<B2b_order> Orderfinset_pay_price(string comid, string startime, string endtime, int mangefinset, string key, string submanagename)
        {
            int count = 0;

            endtime = DateTime.Parse(endtime).AddDays(1).ToString();//结束日期加1天


            //按天统计
            string sqltxt = @"select ticketinfo,count(id) as countnum , sum(u_num) as num,sum(u_num*pay_price)as pay_price from b2b_order where comid=@comid and agentid=0 and mangefinset=@mangefinset and pay_state=2 and order_state in (2,4,8,22) and U_subdate>=@startime and U_subdate<@endtime and submanagename=@submanagename group by ticketinfo ";


            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@startime", startime);
            cmd.AddParam("@endtime", endtime);
            cmd.AddParam("@mangefinset", mangefinset);
            cmd.AddParam("@key", key);
            cmd.AddParam("@submanagename", submanagename);
            

            using (var reader = cmd.ExecuteReader())
            {
                List<B2b_order> list = new List<B2b_order>();
                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        Countnum = reader.GetValue<int>("Countnum"),
                        Ticketinfo = reader.GetValue<string>("ticketinfo"),
                        Pay_price = reader.GetValue<decimal>("pay_price"),
                        U_num = reader.GetValue<int>("num"),
                    });
                    count += 1;
                }
                return list;
            }
        }


        //前台提单 各种支付方式收取金额统计
        internal List<B2b_order> Orderfinset_pay_price_list(string comid, string startime, string endtime, int mangefinset, string key, string submanagename, out int totalcount)
        {
            int count = 0;

            endtime = DateTime.Parse(endtime).AddDays(1).ToString();//结束日期加1天


            //按天统计
            string sqltxt = @"select * from b2b_order where comid=@comid and agentid=0 and mangefinset=@mangefinset and pay_state=2 and order_state in (2,4,8,22) and U_subdate>=@startime and U_subdate<@endtime and submanagename=@submanagename ";


            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@startime", startime);
            cmd.AddParam("@endtime", endtime);
            cmd.AddParam("@mangefinset", mangefinset);
            cmd.AddParam("@key", key);
            cmd.AddParam("@submanagename", submanagename);


            using (var reader = cmd.ExecuteReader())
            {
                List<B2b_order> list = new List<B2b_order>();
                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        U_traveldate = reader.GetValue<DateTime>("U_traveldate"),
                        Dealer = reader.GetValue<string>("dealer"),
                        Comid = reader.GetValue<int>("comid"),
                        Openid = reader.GetValue<string>("openid"),
                        Pno = reader.GetValue<string>("pno"),
                        Integral1 = reader.GetValue<decimal>("integral"),
                        Imprest1 = reader.GetValue<decimal>("imprest"),
                        Ticket = reader.GetValue<decimal>("Ticket"),
                        Ticketinfo = reader.GetValue<string>("ticketinfo") == null ? "" : reader.GetValue<string>("ticketinfo"),
                        Backtickettime = reader.GetValue<DateTime>("Backtickettime"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrantid = reader.GetValue<int>("Warrantid"),
                        Service_order_num = reader.GetValue<string>("Service_order_num"),
                        Service_req_seq = reader.GetValue<string>("Service_req_seq"),
                        Order_remark = reader.GetValue<string>("Order_remark"),
                        Servicepro_v_state = reader.GetValue<string>("Servicepro_v_state"),
                        Tocomid = reader.GetValue<int>("Tocomid"),
                        Buyuid = reader.GetValue<int>("Buyuid"),
                        Child_u_num = reader.GetValue<int>("child_u_num"),
                        Bindingagentorderid = reader.GetValue<int>("Bindingagentorderid"),
                        pickuppoint = reader.GetValue<string>("pickuppoint"),
                        dropoffpoint = reader.GetValue<string>("dropoffpoint"),
                        Cancelnum = reader.GetValue<int>("cancel_ticketnum"),

                        Province = reader.GetValue<string>("Province"),
                        City = reader.GetValue<string>("City"),
                        Address = reader.GetValue<string>("Address"),
                        Code = reader.GetValue<string>("Code"),
                        Express = reader.GetValue<decimal>("Expressfee"),
                        Expresscom = reader.GetValue<string>("Expresscom"),
                        Expresscode = reader.GetValue<string>("expresscode"),
                        Deliverytype = reader.GetValue<int>("Deliverytype"),
                        Shopcartid = reader.GetValue<int>("Shopcartid"),
                        childreduce = reader.GetValue<decimal>("childreduce"),
                        yanzheng_method = reader.GetValue<int>("yanzheng_method"),
                        Handlid = reader.GetValue<int>("Handlid"),
                        qunar_orderid = reader.GetValue<string>("qunar_orderid"),
                        askquitfee = reader.GetValue<decimal>("askquitfee"),
                        recommendchannelid = reader.GetValue<int>("recommendchannelid"),
                        Speciid = reader.GetValue<int>("Speciid"),
                        channelcoachid = reader.GetValue<int>("channelcoachid"),
                        baoxiannames = reader.GetValue<string>("baoxiannames"),
                        baoxianpinyinnames = reader.GetValue<string>("baoxianpinyinnames"),
                        baoxianidcards = reader.GetValue<string>("baoxianidcards"),

                        service_code = reader.GetValue<string>("service_code"),
                        service_usecount = reader.GetValue<int>("service_usecount"),
                        service_lastcount = reader.GetValue<int>("service_lastcount"),

                        bookpro_bindcompany = reader.GetValue<string>("bookpro_bindcompany"),
                        bookpro_bindconfirmtime = reader.GetValue<DateTime>("bookpro_bindconfirmtime"),
                        bookpro_bindname = reader.GetValue<string>("bookpro_bindname"),
                        bookpro_bindphone = reader.GetValue<string>("bookpro_bindphone"),
                        submanagename = reader.GetValue<string>("submanagename"),
                    });
                    count += 1;
                }
                totalcount = count;
                return list;
            }
        }

        //前台提单 各种支付方式收取金额统计
        internal int orderfinset_quren(string comid, string startime, string endtime, int mangefinset, string key, string submanagename)
        {

            endtime = DateTime.Parse(endtime).AddDays(1).ToString();//结束日期加1天

            string sqltxt = @"update b2b_order set mangefinset=2 where comid=@comid and agentid=0 and mangefinset=@mangefinset and pay_state=2 and order_state in (2,4,8,22) and U_subdate>=@startime and U_subdate<@endtime and submanagename=@submanagename ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@startime", startime);
            cmd.AddParam("@endtime", endtime);
            cmd.AddParam("@mangefinset", mangefinset);
            cmd.AddParam("@key", key);
            cmd.AddParam("@submanagename", submanagename);

            return cmd.ExecuteNonQuery();

        }


        #region  合作商获得订单分页列表
        internal List<B2b_order> CoopOrderPageList(string comid, int pageindex, int pagesize, string ordercome, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_order";
            var strGetFields = "*";
            var sortKey = "u_subdate";
            var sortMode = "1";
            var condition = "comid = " + comid;

            condition = condition + " and ordercome='" + ordercome + "'";

            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_order> list = new List<B2b_order>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        U_traveldate = reader.GetValue<DateTime>("U_traveldate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        Integral1 = reader.GetValue<decimal>("Integral"),
                        Imprest1 = reader.GetValue<decimal>("Imprest"),
                        Ticketinfo = reader.GetValue<string>("ticketinfo"),
                        Openid = reader.GetValue<string>("Openid"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrantid = reader.GetValue<int>("Warrantid"),
                        Comid = reader.GetValue<int>("Comid"),
                        Speciid = reader.GetValue<int>("Speciid"),
                        channelcoachid = reader.GetValue<int>("channelcoachid"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;

        }
        #endregion

        #region  合作商获得订单分页列表
        internal List<B2b_order> CoopVerOrderPageList(string comid, int pageindex, int pagesize, string ordercome, out int totalcount)
        {

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "a.com_id=" + comid + " and  c.ordercome='" + ordercome + "' and a.action=1 and a.a_state=1";
            cmd.PagingCommand1("b2b_etcket_log as a left join b2b_eticket as b on a.eticket_id=b.id left join b2b_order as c on b.oid=c.id", "c.id,c.pro_id,c.Speciid,c.channelcoachid,c.order_type,c.u_name,c.u_id,c.u_phone,a.use_pnum as u_num,a.actiondate as U_traveldate,c.U_subdate,c.Payid ,c.Pay_price ,c.Cost ,c.Profit ,c.Order_state,c.Pay_state ,c.Send_state,c.Ticketcode ,c.Rebate ,c.Ordercome ,c.Integral,c.Imprest ,c.Ticketinfo ,c.Openid ,c.Agentid ,c.Warrant_type ,c.Warrantid ,c.Comid,a.pno", "a.id", "", pagesize, pageindex, "0", condition);


            List<B2b_order> list = new List<B2b_order>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        U_traveldate = reader.GetValue<DateTime>("U_traveldate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        Integral1 = reader.GetValue<decimal>("Integral"),
                        Imprest1 = reader.GetValue<decimal>("Imprest"),
                        Ticketinfo = reader.GetValue<string>("ticketinfo"),
                        Openid = reader.GetValue<string>("Openid"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrantid = reader.GetValue<int>("Warrantid"),
                        Comid = reader.GetValue<int>("Comid"),
                        Pno = reader.GetValue<string>("Pno"),
                        Speciid = reader.GetValue<int>("Speciid"),
                        channelcoachid = reader.GetValue<int>("channelcoachid"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;

        }
        #endregion

        #region  分销订单列表
        internal List<B2b_order> AgentOrderPageList(string comid, int agentid, int pageindex, int pagesize, string key, int order_state, out int totalcount, string begindate = "", string enddate = "", int servertype = 0)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_order";
            var strGetFields = "*";
            var sortKey = "u_subdate";
            var sortMode = "1";
            var condition = "agentid=" + agentid;
            if (comid == "" || comid == "0")
            { }
            else
            {
                condition = condition + " and comid = " + comid;
            }
            if (key != "")
            {
                int key_int = 0;
                try
                {
                    key_int =Int32.Parse(key);
                }
                catch
                {
                    key_int = 0;
                }
                condition = condition + " and (U_name='" + key + "' or U_phone='" + key + "' or id=" + key_int + " or '" + key + "' in (pno) or Bindingagentorderid in (select id from b2b_order where pno='" + key + "') or pro_id in (select id from b2b_com_pro where pro_name like '%" + key + "%'))";
            }
            if (order_state != 0)
            {
                condition = condition + " and order_state=" + order_state;
            }
            if (begindate != "" && enddate != "")
            {
                begindate = DateTime.Parse(begindate).ToString("yyyy-MM-dd 00:00:00");
                enddate = DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59");
                condition = condition + " and u_subdate between '" + begindate + "' and '" + enddate + "'";
            }
            if (servertype != 0)
            {
                condition += " and pro_id in (select id from b2b_com_pro where server_type=" + servertype + ")";
            }

            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_order> list = new List<B2b_order>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        U_traveldate = reader.GetValue<DateTime>("U_traveldate"),
                        Dealer = reader.GetValue<string>("dealer"),
                        Comid = reader.GetValue<int>("comid"),
                        Openid = reader.GetValue<string>("openid"),
                        Pno = reader.GetValue<string>("pno"),
                        Integral1 = reader.GetValue<decimal>("integral"),
                        Imprest1 = reader.GetValue<decimal>("imprest"),
                        Ticket = reader.GetValue<decimal>("Ticket"),
                        Ticketinfo = reader.GetValue<string>("ticketinfo") == null ? "" : reader.GetValue<string>("ticketinfo"),
                        Backtickettime = reader.GetValue<DateTime>("Backtickettime"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrantid = reader.GetValue<int>("Warrantid"),
                        Service_order_num = reader.GetValue<string>("Service_order_num"),
                        Service_req_seq = reader.GetValue<string>("Service_req_seq"),
                        Order_remark = reader.GetValue<string>("Order_remark"),
                        Servicepro_v_state = reader.GetValue<string>("Servicepro_v_state"),
                        Tocomid = reader.GetValue<int>("Tocomid"),
                        Buyuid = reader.GetValue<int>("Buyuid"),
                        Child_u_num = reader.GetValue<int>("child_u_num"),
                        Bindingagentorderid = reader.GetValue<int>("Bindingagentorderid"),
                        pickuppoint = reader.GetValue<string>("pickuppoint"),
                        dropoffpoint = reader.GetValue<string>("dropoffpoint"),
                        Cancelnum = reader.GetValue<int>("cancel_ticketnum"),

                        Province = reader.GetValue<string>("Province"),
                        City = reader.GetValue<string>("City"),
                        Address = reader.GetValue<string>("Address"),
                        Code = reader.GetValue<string>("Code"),
                        Express = reader.GetValue<decimal>("Expressfee"),
                        Expresscom = reader.GetValue<string>("Expresscom"),
                        Expresscode = reader.GetValue<string>("expresscode"),
                        Deliverytype = reader.GetValue<int>("Deliverytype"),
                        Shopcartid = reader.GetValue<int>("Shopcartid"),
                        childreduce = reader.GetValue<decimal>("childreduce"),
                        yanzheng_method = reader.GetValue<int>("yanzheng_method"),
                        Handlid = reader.GetValue<int>("Handlid"),
                        qunar_orderid = reader.GetValue<string>("qunar_orderid"),
                        askquitfee = reader.GetValue<decimal>("askquitfee"),
                        recommendchannelid = reader.GetValue<int>("recommendchannelid"),
                        Speciid = reader.GetValue<int>("Speciid"),
                        channelcoachid = reader.GetValue<int>("channelcoachid"),
                        baoxiannames = reader.GetValue<string>("baoxiannames"),
                        baoxianpinyinnames = reader.GetValue<string>("baoxianpinyinnames"),
                        baoxianidcards = reader.GetValue<string>("baoxianidcards"),

                        service_code = reader.GetValue<string>("service_code"),
                        service_usecount = reader.GetValue<int>("service_usecount"),
                        service_lastcount = reader.GetValue<int>("service_lastcount"),
                        bookpro_bindcompany = reader.GetValue<string>("bookpro_bindcompany"),
                        bookpro_bindconfirmtime = reader.GetValue<DateTime>("bookpro_bindconfirmtime"),
                        bookpro_bindname = reader.GetValue<string>("bookpro_bindname"),
                        bookpro_bindphone = reader.GetValue<string>("bookpro_bindphone"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;

        }
        #endregion


        #region  分销订单汇总
        internal List<B2b_order> AgentOrderCount(string comid, int agentid, int pageindex, int pagesize, out int totalcount, string key = "")
        {

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "(oid in (select id from b2b_order as c where c.comid=" + comid + " and c.agentid=" + agentid + " and (c.order_state=4 or c.order_state=8 or c.order_state=16) ) or oid in (select bindingagentorderid from b2b_order as c where c.comid=" + comid + " and c.agentid=" + agentid + " and (c.order_state=4 or c.order_state=8 or c.order_state=16) and bindingagentorderid !=0  ) ) ";

            if (key != "")
            {
                condition += " and e_proname like '%" + key + "%'";
            }

            cmd.PagingCommand1("b2b_eticket ", "pro_id,sum(pnum) as U_num", "pro_id", "pro_id", pagesize, pageindex, "1", condition);


            List<B2b_order> list = new List<B2b_order>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        U_num = reader.GetValue<int>("U_num"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;

        }
        #endregion

        #region  获得退票列表
        internal List<B2b_order> TicketPageList(int pageindex, int pagesize, string key, int order_state, int endstate, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_order";
            var strGetFields = "*";
            var sortKey = "backtickettime";
            var sortMode = "1";
            var condition = "id != 0 ";
            if (order_state == 0)
            {
                condition += " and order_state in(16,17,18) ";
                if (key != "")
                {


                    //判断是否为数字，是数字的则可以查询订单号
                    Regex regex = new Regex("^[0-9]*$");
                    bool isNum = regex.IsMatch(key.Trim());




                    if (isNum == false || key.Length >= 7)
                    {
                        condition += " and ( U_name='" + key + "' or U_phone='" + key + "')  ";
                    }
                    else
                    {
                        condition += " and ( U_name='" + key + "' or U_phone='" + key + "' or Id='" + key + "')  ";
                    }


                }
            }
            else
            {
                if (order_state == 17)//未完成退票，包括 申请和退票中
                {
                    condition = condition + " and order_state in (17,18)";
                }
                else
                {
                    condition += " and order_state = " + order_state;
                }


                if (key != "")
                {
                    //判断是否为数字，是数字的则可以查询订单号
                    Regex regex = new Regex("^[0-9]*$");
                    bool isNum = regex.IsMatch(key.Trim());

                    if (isNum == false || key.Length >= 7)
                    {
                        condition += " and ( U_name='" + key + "' or U_phone='" + key + "')  ";
                    }
                    else
                    {
                        condition += " and ( U_name='" + key + "' or U_phone='" + key + "' or Id='" + key + "')  ";
                    }
                }
            }


            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_order> list = new List<B2b_order>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        Ticket = reader.GetValue<decimal>("Ticket"),
                        Ticketinfo = reader.GetValue<string>("ticketinfo"),
                        Pno = reader.GetValue<string>("pno"),
                        Backtickettime = reader.GetValue<DateTime>("backtickettime"),
                        Comid = reader.GetValue<int>("comid"),
                        Express = reader.GetValue<decimal>("Expressfee"),
                        Agentid = reader.GetValue<int>("agentid"),

                        askquitfee = reader.GetValue<decimal>("askquitfee"),
                        askquitfeereason = reader.GetValue<string>("askquitfeereason"),
                        askquitfeeexplain = reader.GetValue<string>("askquitfeeexplain"),
                        askquitnum = reader.GetValue<int>("askquitnum"),
                        Speciid = reader.GetValue<int>("Speciid"),
                        channelcoachid = reader.GetValue<int>("channelcoachid"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;

        }
        #endregion

        internal B2b_order GetOrderById(int orderid)
        {
            const string sqltxt = @"SELECT   *
  FROM b2b_order where [Id]=@orderid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@orderid", orderid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_idcard = reader.GetValue<string>("U_idcard"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        U_traveldate = reader.GetValue<DateTime>("U_traveldate"),
                        Dealer = reader.GetValue<string>("dealer"),
                        Comid = reader.GetValue<int>("comid"),
                        Openid = reader.GetValue<string>("openid"),
                        Pno = reader.GetValue<string>("pno"),
                        Integral1 = reader.GetValue<decimal>("integral"),
                        Imprest1 = reader.GetValue<decimal>("imprest"),
                        Ticket = reader.GetValue<decimal>("Ticket"),
                        Ticketinfo = reader.GetValue<string>("ticketinfo") == null ? "" : reader.GetValue<string>("ticketinfo"),
                        Backtickettime = reader.GetValue<DateTime>("Backtickettime"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrantid = reader.GetValue<int>("Warrantid"),
                        Service_order_num = reader.GetValue<string>("Service_order_num"),
                        Service_req_seq = reader.GetValue<string>("Service_req_seq"),
                        Order_remark = reader.GetValue<string>("Order_remark"),
                        Servicepro_v_state = reader.GetValue<string>("Servicepro_v_state"),
                        Tocomid = reader.GetValue<int>("Tocomid"),
                        Buyuid = reader.GetValue<int>("Buyuid"),
                        Child_u_num = reader.GetValue<int>("child_u_num"),
                        Bindingagentorderid = reader.GetValue<int>("Bindingagentorderid"),
                        pickuppoint = reader.GetValue<string>("pickuppoint"),
                        dropoffpoint = reader.GetValue<string>("dropoffpoint"),
                        Cancelnum = reader.GetValue<int>("cancel_ticketnum"),

                        Province = reader.GetValue<string>("Province"),
                        City = reader.GetValue<string>("City"),
                        Address = reader.GetValue<string>("Address"),
                        Code = reader.GetValue<string>("Code"),
                        Express = reader.GetValue<decimal>("Expressfee"),
                        Expresscom = reader.GetValue<string>("Expresscom"),
                        Expresscode = reader.GetValue<string>("expresscode"),
                        Deliverytype = reader.GetValue<int>("Deliverytype"),
                        Shopcartid = reader.GetValue<int>("Shopcartid"),
                        childreduce = reader.GetValue<decimal>("childreduce"),
                        yanzheng_method = reader.GetValue<int>("yanzheng_method"),
                        Handlid = reader.GetValue<int>("Handlid"),
                        qunar_orderid = reader.GetValue<string>("qunar_orderid"),
                        askquitfee = reader.GetValue<decimal>("askquitfee"),
                        recommendchannelid = reader.GetValue<int>("recommendchannelid"),
                        Speciid = reader.GetValue<int>("Speciid"),
                        channelcoachid = reader.GetValue<int>("channelcoachid"),
                        baoxiannames = reader.GetValue<string>("baoxiannames"),
                        baoxianpinyinnames = reader.GetValue<string>("baoxianpinyinnames"),
                        baoxianidcards = reader.GetValue<string>("baoxianidcards"),

                        service_code = reader.GetValue<string>("service_code"),
                        service_usecount = reader.GetValue<int>("service_usecount"),
                        service_lastcount = reader.GetValue<int>("service_lastcount"),
                        yuyuepno = reader.GetValue<string>("yuyuepno"),
                        yuyueweek = reader.GetValue<int>("yuyueweek"),
                        givebaoxianorderid = reader.GetValue<int>("givebaoxianorderid"),
                        payserverpno = reader.GetValue<string>("payserverpno"),
                        serverid = reader.GetValue<string>("serverid"),

                        travelnames = reader.GetValue<string>("travelnames"),
                        travelidcards = reader.GetValue<string>("travelidcards"),
                        travelnations = reader.GetValue<string>("travelnations"),
                        travelphones = reader.GetValue<string>("travelphones"),
                        travelremarks = reader.GetValue<string>("travelremarks"),
                        isInterfaceSub = reader.GetValue<int>("isInterfaceSub"),
                        bookpro_bindcompany = reader.GetValue<string>("bookpro_bindcompany"),
                        bookpro_bindconfirmtime = reader.GetValue<DateTime>("bookpro_bindconfirmtime"),
                        bookpro_bindname = reader.GetValue<string>("bookpro_bindname"),
                        bookpro_bindphone = reader.GetValue<string>("bookpro_bindphone"),
                        submanagename = reader.GetValue<string>("submanagename"),
                        mangefinset = reader.GetValue<int>("mangefinset"),
                        payorder = reader.GetValue<int>("payorder"),
                    };
                }
                return null;
            }
        }

        //根据绑定id获取原来订单ID
        internal int GetIdOrderBybindingId(int orderid)
        {
            const string sqltxt = @"SELECT id  FROM b2b_order where [bindingagentorderid]=@orderid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@orderid", orderid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("id");
                }
                return orderid;
            }
        }

        //根据weixin,comid获取是否已此订单
        internal int GetOrderIdByWeixin(string openid, int comid, int proid)
        {
            const string sqltxt = @"SELECT id  FROM b2b_order where openid=@openid and comid=@comid and pro_id=@proid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@openid", openid);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@proid", proid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("id");
                }
                return 0;
            }
        }

        internal string GetPnoByOrderId(int orderid)
        {
            const string sqltxt = @"SELECT pno  FROM b2b_order where [Id]=@orderid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@orderid", orderid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<string>("pno");
                }
                return "";
            }
        }


        internal List<B2b_com_pro> SaleCountPageList(string comid, int pageindex, int pagesize, string startdate, string enddate, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = " b2b_com_pro";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = " com_id=" + comid;

            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);

            List<B2b_com_pro> list = new List<B2b_com_pro>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_pro
                    {
                        
                        Pro_name = reader.GetValue<string>("pro_name"),//商品名
                        Face_price = reader.GetValue<decimal>("face_price"),//门市价
                        Advise_price = reader.GetValue<decimal>("advise_price"),//销售价
                        //Totalpay = reader.GetValue<int>("totalpay"),//多少条订单
                        //U_num = reader.GetValue<int>("u_num"),//总共多少张
                        //Totalpay_price = reader.GetValue<decimal>("totalpay_price"),//实际收款金额
                        //Totalprofit = reader.GetValue<decimal>("totalprofit"),//毛利

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }

        internal B2b_com_pro CountOrder(int id, string startdate, string enddate)
        {
            string sqltxt = @"select count(a.id) as totalpay,SUM(a.u_num) as u_num,SUM(a.pay_price*a.u_num-a.integral) as totalpay_price,SUM(a.profit*a.u_num) as totalprofit from b2b_order a
            where  a.pro_id =@id and a.pay_state=@paystate  ";
            if (startdate != "")
            {
                sqltxt = sqltxt + " and a.u_subdate>='" + startdate + "'";
            }
            if (enddate != "")
            {
                enddate = DateTime.Parse(enddate).AddDays(1).ToString();
                sqltxt = sqltxt + " and a.u_subdate<'" + enddate + "'";
            }
            sqltxt = sqltxt + " group by a.pro_id";


            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", id);
            cmd.AddParam("@paystate", (int)PayStatus.HasPay);

            B2b_com_pro list = new B2b_com_pro();
            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    return new B2b_com_pro
                     {
                         Totalpay = reader.GetValue<int>("totalpay"),//多少条订单
                         U_num = reader.GetValue<int>("u_num"),//总共多少张
                         Totalpay_price = reader.GetValue<decimal>("totalpay_price"),//实际收款金额
                         Totalprofit = reader.GetValue<decimal>("totalprofit"),//毛利
                     };
                }
                else
                {
                    return new B2b_com_pro
                    {
                        Totalpay = 0,//多少条订单
                        U_num = 0,//总共多少张
                        Totalpay_price = 0,//实际收款金额
                        Totalprofit = 0,//毛利
                    };
                }
            }
        }


        internal B2b_com_pro Daoma_CountOrder(int id, string startdate, string enddate)
        {
            string sqltxt = @"select count(a.id) as totalpay,SUM(a.u_num) as u_num,SUM(a.pay_price*a.u_num-a.integral) as totalpay_price,SUM(a.profit*a.u_num) as totalprofit from b2b_order a
            where  a.pro_id =@id and a.pay_state=@paystate and warrant_type=2  ";
            if (startdate != "")
            {
                sqltxt = sqltxt + " and a.u_subdate>='" + startdate + "'";
            }
            if (enddate != "")
            {
                enddate = DateTime.Parse(enddate).AddDays(1).ToString();
                sqltxt = sqltxt + " and a.u_subdate<'" + enddate + "'";
            }
            sqltxt = sqltxt + " group by a.pro_id";


            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", id);
            cmd.AddParam("@paystate", (int)PayStatus.HasPay);

            B2b_com_pro list = new B2b_com_pro();
            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    return new B2b_com_pro
                    {
                        Totalpay = reader.GetValue<int>("totalpay"),//多少条订单
                        U_num = reader.GetValue<int>("u_num"),//总共多少张
                        Totalpay_price = reader.GetValue<decimal>("totalpay_price"),//实际收款金额
                        Totalprofit = reader.GetValue<decimal>("totalprofit"),//毛利
                    };
                }
                else
                {
                    return new B2b_com_pro
                    {
                        Totalpay = 0,//多少条订单
                        U_num = 0,//总共多少张
                        Totalpay_price = 0,//实际收款金额
                        Totalprofit = 0,//毛利
                    };
                }
            }
        }

        internal DataSet GetTotaldate(string comid, string startdate, string enddate)
        {
            const string sqltxt = @"select SUM(a.pay_price*a.u_num) as totalpay,SUM(a.u_num) as u_num,SUM(a.profit*a.u_num) as totalprofit from dbo.b2b_order a
  where a.u_subdate>=@startdate and a.u_subdate<=@enddate and a.pay_state=@paystate and a.pro_id in (select b.id from dbo.b2b_com_pro as b where b.com_id=@comid)";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@startdate", DateTime.Parse(startdate));
            cmd.AddParam("@enddate", DateTime.Parse(enddate));
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@paystate", (int)PayStatus.HasPay);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {

                    DataTable dt = new DataTable("salecount");
                    DataColumn dc1 = dt.Columns.Add("totalpay");
                    DataColumn dc2 = dt.Columns.Add("u_num");
                    DataColumn dc3 = dt.Columns.Add("totalprofit");

                    DataRow dr;
                    dr = dt.NewRow();
                    dr["totalpay"] = reader.GetValue<decimal>("totalpay");
                    dr["u_num"] = reader.GetValue<int>("u_num");
                    dr["totalprofit"] = reader.GetValue<decimal>("totalprofit");

                    dt.Rows.Add(dr);

                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);

                    return ds;
                }
                return null;
            }
        }

        internal B2b_order GetOrderByEticketId(int eticketid)
        {
            const string sqltxt = @"SELECT   [id]
      ,[pro_id]
      ,[order_type]
      ,[u_name]
      ,[u_id]
      ,[u_phone]
      ,[u_num]
      ,[u_subdate]
      ,[payid]
      ,[pay_price]
      ,[cost]
      ,[profit]
      ,[order_state]
      ,[pay_state]
      ,[send_state]
      ,[ticketcode]
      ,[rebate]
      ,[ordercome]
  FROM b2b_order where [ticketcode]=@eticketid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@eticketid", eticketid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome")
                    };
                }
                return null;
            }
        }

        internal List<B2b_order> ConsumerOrderPageList(string openid, int pageindex, int pagesize, int accountid, out int totalcount, int servertype = 0, int channelid = 0, string startime = "", string endtime = "")
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_order";
            var strGetFields = "*";
            var sortKey = "u_subdate";
            var sortMode = "1";
            var condition = " ";
            if (accountid != 0 && openid != "")
            {
                //微信手机绑定后新添加此用户在电脑端下的订单
                condition = " (openid='" + openid + "' or u_id =" + accountid + ") and not Order_state in (23) ";
            }
            else if (accountid != 0)
            {
                //微信手机绑定后新添加此用户在电脑端下的订单
                condition = " (u_id =" + accountid + ") and not Order_state in (23) ";

            }
            else
            {

                condition = "openid='" + openid + "' and not Order_state in (23)";
            }

            //如果查询指定类型产品订单，默认 必须成功支付的订单
            if (servertype != 0)
            {
                condition += " and Pro_id in (select id from b2b_com_pro where server_type=" + servertype + ") and Pay_state=2";
            }


            if (channelid != 0)
            { //过渠道不=0，则读取预约教练的订单查询
                condition = " channelcoachid = " + channelid + " and Order_state in (2,4,8,16)";
                if (startime != "")
                {
                    condition += " and u_traveldate>='" + startime + "'";
                }
                if (endtime != "")
                {
                    condition += " and u_traveldate<'" + endtime + "'";
                }

                sortKey = "u_traveldate";
            }




            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_order> list = new List<B2b_order>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        U_traveldate = reader.GetValue<DateTime>("U_traveldate"),
                        Dealer = reader.GetValue<string>("dealer"),
                        Comid = reader.GetValue<int>("comid"),
                        Openid = reader.GetValue<string>("openid"),
                        Pno = reader.GetValue<string>("pno"),
                        Integral1 = reader.GetValue<decimal>("integral"),
                        Imprest1 = reader.GetValue<decimal>("imprest"),
                        Ticket = reader.GetValue<decimal>("Ticket"),
                        Ticketinfo = reader.GetValue<string>("ticketinfo") == null ? "" : reader.GetValue<string>("ticketinfo"),
                        Backtickettime = reader.GetValue<DateTime>("Backtickettime"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrantid = reader.GetValue<int>("Warrantid"),
                        Service_order_num = reader.GetValue<string>("Service_order_num"),
                        Service_req_seq = reader.GetValue<string>("Service_req_seq"),
                        Order_remark = reader.GetValue<string>("Order_remark"),
                        Servicepro_v_state = reader.GetValue<string>("Servicepro_v_state"),
                        Tocomid = reader.GetValue<int>("Tocomid"),
                        Buyuid = reader.GetValue<int>("Buyuid"),
                        Child_u_num = reader.GetValue<int>("child_u_num"),
                        Bindingagentorderid = reader.GetValue<int>("Bindingagentorderid"),
                        pickuppoint = reader.GetValue<string>("pickuppoint"),
                        dropoffpoint = reader.GetValue<string>("dropoffpoint"),
                        Cancelnum = reader.GetValue<int>("cancel_ticketnum"),

                        Province = reader.GetValue<string>("Province"),
                        City = reader.GetValue<string>("City"),
                        Address = reader.GetValue<string>("Address"),
                        Code = reader.GetValue<string>("Code"),
                        Express = reader.GetValue<decimal>("Expressfee"),
                        Expresscom = reader.GetValue<string>("Expresscom"),
                        Expresscode = reader.GetValue<string>("expresscode"),
                        Deliverytype = reader.GetValue<int>("Deliverytype"),
                        Shopcartid = reader.GetValue<int>("Shopcartid"),
                        childreduce = reader.GetValue<decimal>("childreduce"),
                        Speciid = reader.GetValue<int>("Speciid"),
                        channelcoachid = reader.GetValue<int>("channelcoachid"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }


        internal List<B2b_order> ClientOrderquerenPageList(string openid, int pageindex, int pagesize, int accountid, out int totalcount, int servertype = 0)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_order";
            var strGetFields = "*";
            var sortKey = "u_subdate";
            var sortMode = "1";
            var condition = " ";
            if (accountid != 0 && openid != "")
            {
                //微信手机绑定后新添加此用户在电脑端下的订单
                condition = " (openid='" + openid + "' or u_id =" + accountid + ") and not Order_state in (23) ";
            }
            else if (accountid != 0)
            {
                //微信手机绑定后新添加此用户在电脑端下的订单
                condition = " (u_id =" + accountid + ") and not Order_state in (23) ";

            }
            else
            {

                condition = "openid='" + openid + "' and not Order_state in (23)";
            }

            //如果查询指定类型产品订单，默认 必须成功支付的订单
            if (servertype != 0)
            {
                condition += " and Pro_id in (select id from b2b_com_pro where server_type=" + servertype + ") and Pay_state=2";
            }

            condition += " and id in (select oid from b2b_eticket where use_pnum>0 )";//必须有可使用数量


            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_order> list = new List<B2b_order>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        U_traveldate = reader.GetValue<DateTime>("U_traveldate"),
                        Dealer = reader.GetValue<string>("dealer"),
                        Comid = reader.GetValue<int>("comid"),
                        Openid = reader.GetValue<string>("openid"),
                        Pno = reader.GetValue<string>("pno"),
                        Integral1 = reader.GetValue<decimal>("integral"),
                        Imprest1 = reader.GetValue<decimal>("imprest"),
                        Ticket = reader.GetValue<decimal>("Ticket"),
                        Ticketinfo = reader.GetValue<string>("ticketinfo") == null ? "" : reader.GetValue<string>("ticketinfo"),
                        Backtickettime = reader.GetValue<DateTime>("Backtickettime"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrantid = reader.GetValue<int>("Warrantid"),
                        Service_order_num = reader.GetValue<string>("Service_order_num"),
                        Service_req_seq = reader.GetValue<string>("Service_req_seq"),
                        Order_remark = reader.GetValue<string>("Order_remark"),
                        Servicepro_v_state = reader.GetValue<string>("Servicepro_v_state"),
                        Tocomid = reader.GetValue<int>("Tocomid"),
                        Buyuid = reader.GetValue<int>("Buyuid"),
                        Child_u_num = reader.GetValue<int>("child_u_num"),
                        Bindingagentorderid = reader.GetValue<int>("Bindingagentorderid"),
                        pickuppoint = reader.GetValue<string>("pickuppoint"),
                        dropoffpoint = reader.GetValue<string>("dropoffpoint"),
                        Cancelnum = reader.GetValue<int>("cancel_ticketnum"),

                        Province = reader.GetValue<string>("Province"),
                        City = reader.GetValue<string>("City"),
                        Address = reader.GetValue<string>("Address"),
                        Code = reader.GetValue<string>("Code"),
                        Express = reader.GetValue<decimal>("Expressfee"),
                        Expresscom = reader.GetValue<string>("Expresscom"),
                        Expresscode = reader.GetValue<string>("expresscode"),
                        Deliverytype = reader.GetValue<int>("Deliverytype"),
                        Shopcartid = reader.GetValue<int>("Shopcartid"),
                        childreduce = reader.GetValue<decimal>("childreduce"),

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }


        internal int CountOrderServertype12(string openid)
        {
            int count = 0;
            try
            {
                //只查询此openid，已支付，并未验证数量
                string sql = @"SELECT count(id) as count FROM b2b_order where openid=@openid and Pro_id in (select id from b2b_com_pro where server_type=12) and Pay_state=2 and order_state=4 ";

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@openid", openid);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        count = reader.GetValue<int>("count");
                    }
                }
                return count;
            }
            catch
            {
                return 0;
            }
        }

        internal List<B2b_order> ComOrderPageList(int comid, int pageindex, int pagesize, int accountid, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_order";
            var strGetFields = "*";
            var sortKey = "u_subdate";
            var sortMode = "1";
            var condition = "u_id =" + accountid + " and  comid=" + comid;
            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_order> list = new List<B2b_order>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        Pno = reader.GetValue<string>("pno") == null ? "" : reader.GetValue<string>("pno"),
                        Comid = reader.GetValue<int>("comid")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }

        internal B2b_order GetProductByOrderId(int orderid, string openid)
        {
            string sql = @"SELECT * FROM b2b_order where id=@orderid ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@orderid", orderid);
            cmd.AddParam("@openid", openid);


            B2b_order order = null;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    order = new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        Openid = reader.GetValue<string>("openid"),
                        Comid = reader.GetValue<int>("Comid"),
                        Deliverytype = reader.GetValue<int>("Deliverytype"),
                        Expresscom = reader.GetValue<string>("Expresscom"),
                        Expresscode = reader.GetValue<string>("Expresscode"),
                        Backtickettime = reader.GetValue<DateTime>("Backtickettime"),
                    };
                }
            }
            return order;
        }

        internal int ConfirExpress(int comid, int id, string expresscom, string expresscode, int order_state, string expresstext)
        {
            //如果order_state =0 则不处理状态，而只对订单状态支付转成功（2-4）
            string wstr = "update b2b_order set expresscom=@expresscom,expresscode=@expresscode,order_state=4,Order_remark=@expresstext where id=@id and comid=@comid ";
            if (order_state == 0)
            {
                wstr = "update b2b_order set expresscom=@expresscom,expresscode=@expresscode,Order_remark=@expresstext where id=@id and comid=@comid ";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(wstr);
            cmd.AddParam("@expresscom", expresscom);
            cmd.AddParam("@expresscode", expresscode);
            cmd.AddParam("@expresstext", expresstext);
            cmd.AddParam("@id", id);
            cmd.AddParam("@comid", comid);
            return cmd.ExecuteNonQuery();
        }


        internal int guoqi_biaozhu(string comid, int id)
        {
            //如果order_state =0 则不处理状态，而只对订单状态支付转成功（2-4）
            string wstr = "update b2b_order set order_state=25 where id=@id and comid=@comid ";

            var cmd = sqlHelper.PrepareTextSqlCommand(wstr);
            cmd.AddParam("@id", id);
            cmd.AddParam("@comid", comid);
            cmd.ExecuteNonQuery();

            string wstr2 = "update b2b_eticket set v_state=5 where oid=@id  and com_id=@comid ";
            var cmd2 = sqlHelper.PrepareTextSqlCommand(wstr2);
            cmd2.AddParam("@id", id);
            cmd2.AddParam("@comid", comid);
            return cmd2.ExecuteNonQuery();
            
        }
        



        internal int ModifyUidByWeiXin(string weixin, int uid, int comid)
        {
            string wstr = "update b2b_order set u_id=@uid  where openid=@openid and comid=@comid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(wstr);
            cmd.AddParam("@uid", uid);
            cmd.AddParam("@openid", weixin);
            cmd.AddParam("@comid", comid);
            return cmd.ExecuteNonQuery();
        }


        internal DataSet CreateDaoma_Dataset(int id)
        {
            string wstr = "b2b_order set u_id=@id  where openid=@openid and comid=@comid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(wstr);
            cmd.AddParam("@id", id);
            cmd.ExecuteNonQuery();
            return null;
        }

        //internal int ModifyUidByPhone(decimal phone, int uid, int comid)
        //{
        //    string wstr = "update b2b_order set u_id=@uid  where u_phone=@phone and comid=@comid ";
        //    var cmd = sqlHelper.PrepareTextSqlCommand(wstr);
        //    cmd.AddParam("@uid", uid);
        //    cmd.AddParam("@phone", phone);
        //    cmd.AddParam("@comid", comid);
        //    return cmd.ExecuteNonQuery();
        //}



        internal int CoopOrderCount(int comid, string ordercome, out int Allnum, out int Todaynum, out int Yesterdaynum, out int Transactionnum)
        {
            Allnum = 0;
            Todaynum = 0;
            Yesterdaynum = 0;
            Transactionnum = 0;

            string sqltxt = @"select count(id) as num from b2b_order where ordercome=@ordercome and comid=@comid";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@ordercome", ordercome);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Allnum = reader.GetValue<int>("num");
                reader.Close();
            }

            sqltxt = @"select count(id) as num from b2b_order where comid=@comid and ordercome=@ordercome  and u_subdate>='" + DateTime.Now.Date.ToShortDateString() + "'";
            var cmd1 = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd1.AddParam("@comid", comid);
            cmd1.AddParam("@ordercome", ordercome);
            var reader1 = cmd1.ExecuteReader();
            if (reader1.Read())
            {
                Todaynum = reader1.GetValue<int>("num");
                reader1.Close();
            }


            sqltxt = @"select count(id) as num from b2b_order where comid=@comid and ordercome=@ordercome and u_subdate>'" + DateTime.Now.AddDays(-1).ToShortDateString() + "' and u_subdate<'" + DateTime.Now.Date.ToShortDateString() + "'";
            var cmd2 = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd2.AddParam("@comid", comid);
            cmd2.AddParam("@ordercome", ordercome);
            var reader2 = cmd2.ExecuteReader();
            if (reader2.Read())
            {
                Yesterdaynum = reader2.GetValue<int>("num");
                reader2.Close();
            }

            sqltxt = @"select SUM(use_pnum) as num from b2b_etcket_log where eticket_id in (select id from b2b_eticket where oid in (select id from b2b_order where comid=@comid and ordercome=@ordercome)) and com_id =@comid and action=1 and a_state=1 ";
            var cmd3 = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd3.AddParam("@comid", comid);
            cmd3.AddParam("@ordercome", ordercome);
            var reader3 = cmd3.ExecuteReader();
            if (reader3.Read())
            {
                Transactionnum = reader3.GetValue<int>("num");
                reader3.Close();
            }

            return 1;

        }

        internal B2b_order GetOrderByOutPro_OrderNum(string order_num, int serviceid)
        {
            const string sqltxt = @"SELECT   [id]
      ,[pro_id]
      ,[order_type]
      ,[u_name]
      ,[u_id]
      ,[u_phone]
      ,[u_num]
      ,[u_subdate]
      ,[payid]
      ,[pay_price]
      ,[cost]
      ,[profit]
      ,[order_state]
      ,[pay_state]
      ,[send_state]
      ,[ticketcode]
      ,[rebate]
      ,[ordercome]
     ,[u_traveldate]
     ,[dealer]
     ,comid
    ,openid
    ,pno
    ,comid
    ,integral
    ,imprest
    ,Ticket
    ,ticketinfo
,Agentid
,Warrant_type
,Warrantid
,Backtickettime 
,service_order_num
,service_req_seq
,order_remark
,servicepro_v_state

  FROM b2b_order where service_order_num=@service_order_num and  pro_id in (select id from b2b_com_pro where serviceid=@serviceid)";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@service_order_num", order_num);
            cmd.AddParam("@serviceid", serviceid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        U_traveldate = reader.GetValue<DateTime>("U_traveldate"),
                        Dealer = reader.GetValue<string>("dealer"),
                        Comid = reader.GetValue<int>("comid"),
                        Openid = reader.GetValue<string>("openid"),
                        Pno = reader.GetValue<string>("pno"),
                        Integral1 = reader.GetValue<decimal>("integral"),
                        Imprest1 = reader.GetValue<decimal>("imprest"),
                        Ticket = reader.GetValue<decimal>("Ticket"),
                        Ticketinfo = reader.GetValue<string>("ticketinfo") == null ? "" : reader.GetValue<string>("ticketinfo"),
                        Backtickettime = reader.GetValue<DateTime>("Backtickettime"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrantid = reader.GetValue<int>("Warrantid"),
                        Service_order_num = reader.GetValue<string>("Service_order_num"),
                        Service_req_seq = reader.GetValue<string>("Service_req_seq"),
                        Order_remark = reader.GetValue<string>("Order_remark"),

                        Servicepro_v_state = reader.GetValue<string>("Servicepro_v_state")

                    };
                }
                return null;
            }
        }
        /*同一用户对于同一产品是否有未完成订单*/
        internal int Ishasnotpayorder(int uid, int proid)
        {
            string sql = "select count(1) from b2b_order where u_id=" + uid + " and pro_id=" + proid + "  and order_state=1";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            if (int.Parse(o.ToString()) > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }

        internal List<B2b_order> Travelbusorderdetail(string traveldate, int proid, int order_state)
        {

            const string sqltxt = @"SELECT   [id]
      ,[pro_id]
      ,[order_type]
      ,[u_name]
      ,[u_id]
      ,[u_phone]
      ,[u_num]
      ,[u_subdate]
      ,[payid]
      ,[pay_price]
      ,[cost]
      ,[profit]
      ,[order_state]
      ,[pay_state]
      ,[send_state]
      ,[ticketcode]
      ,[rebate]
      ,[ordercome]
     ,[u_traveldate]
     ,[dealer]
     ,comid
    ,openid
    ,pno
    ,comid
    ,integral
    ,imprest
    ,Ticket
    ,ticketinfo
,Agentid
,Warrant_type
,Warrantid
,Backtickettime 
,service_order_num
,service_req_seq
,order_remark
,Servicepro_v_state
,tocomid
,buyuid
,child_u_num
  FROM b2b_order where  u_traveldate=@u_traveldate and pro_id=@pro_id and order_state=@order_state";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@u_traveldate", traveldate);
            cmd.AddParam("@pro_id", proid);
            cmd.AddParam("@order_state", order_state);

            using (var reader = cmd.ExecuteReader())
            {
                List<B2b_order> list = new List<B2b_order>();
                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        U_traveldate = reader.GetValue<DateTime>("U_traveldate"),
                        Dealer = reader.GetValue<string>("dealer"),
                        Comid = reader.GetValue<int>("comid"),
                        Openid = reader.GetValue<string>("openid"),
                        Pno = reader.GetValue<string>("pno"),
                        Integral1 = reader.GetValue<decimal>("integral"),
                        Imprest1 = reader.GetValue<decimal>("imprest"),
                        Ticket = reader.GetValue<decimal>("Ticket"),
                        Ticketinfo = reader.GetValue<string>("ticketinfo") == null ? "" : reader.GetValue<string>("ticketinfo"),
                        Backtickettime = reader.GetValue<DateTime>("Backtickettime"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrantid = reader.GetValue<int>("Warrantid"),
                        Service_order_num = reader.GetValue<string>("Service_order_num"),
                        Service_req_seq = reader.GetValue<string>("Service_req_seq"),
                        Order_remark = reader.GetValue<string>("Order_remark"),
                        Servicepro_v_state = reader.GetValue<string>("Servicepro_v_state"),
                        Tocomid = reader.GetValue<int>("Tocomid"),
                        Buyuid = reader.GetValue<int>("Buyuid"),
                        Child_u_num = reader.GetValue<int>("child_u_num")
                    });
                }
                return list;
            }
        }


        internal int GetPaySucNumByProid(int proid, int comid, int paystate, DateTime daydate, int agentid = 0, string orderstate = "")
        {
            int servertype = 0;//服务类型
            string sql1 = "select server_type from b2b_com_pro where id=" + proid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql1);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    servertype = reader.GetValue<int>("server_type");
                }
            }

            string sql = "";
            //酒店客房
            if (servertype == 9)
            {
                sql = "select SUM(a.u_num)  from b2b_order as a   left join b2b_order_hotel as b on a.id=b.orderid where  a.pro_id=" + proid + " and a.pay_state=" + paystate + " and b.start_date<='" + daydate + "' and b.end_date>'" + daydate + "' ";
                if (agentid > 0)
                {
                    sql = "select SUM(a.u_num)  from b2b_order as a   left join b2b_order_hotel as b on a.id=b.orderid where a.agentid=" + agentid + " and a.pro_id=" + proid + " and a.pay_state=" + paystate + " and b.start_date<='" + daydate + "' and b.end_date>'" + daydate + "' ";
                }

                if (orderstate != "")
                {
                    sql += " and charindex(',' + LTRIM(a.order_state) + ',', ',' + '" + orderstate + "' + ',') > 0  ";
                }
            }
            //非酒店客房
            else
            {
                sql = "select SUM(u_num)  from b2b_order where  pro_id=" + proid + "   and u_traveldate='" + daydate + "' and pay_state=" + paystate;
                if (agentid > 0)
                {
                    sql = "select SUM(u_num)  from b2b_order where agentid=" + agentid + "  and   pro_id=" + proid + "   and u_traveldate='" + daydate + "' and pay_state=" + paystate;
                }

                if (orderstate != "")
                {
                    sql += " and charindex(',' + LTRIM(order_state) + ',', ',' + '" + orderstate + "' + ',') > 0  ";
                }

            }

            try
            {
                cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        internal int GetCloseTeamNumByProid(int proid, int comid, int orderstate, DateTime daydate)
        {
            string sql = "select SUM(u_num)  from b2b_order where pro_id=" + proid + "   and u_traveldate='" + daydate + "' and order_state=" + orderstate + " and comid=" + comid;
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            catch (Exception e)
            {
                sqlHelper.Dispose();
                return 0;
            }
        }



        internal int GetPaysucNumByServertype(DateTime daydate, int servertype, int comid, int paystate, int agentid = 0, string orderstate = "")
        {

            string sql = "select SUM(u_num)  from b2b_order where     pro_id in (select id from b2b_com_pro where server_type=" + servertype + ") and u_traveldate='" + daydate + "' and pay_state=" + paystate + " and comid=" + comid;
            if (agentid > 0)
            {
                sql = "select SUM(u_num)  from b2b_order where     agentid=" + agentid + "  and  pro_id in (select id from b2b_com_pro where server_type=" + servertype + ") and u_traveldate='" + daydate + "' and pay_state=" + paystate + " and comid=" + comid;
            }
            if (orderstate != "")
            {
                sql += " and charindex(',' + LTRIM(order_state) + ',', ',' + '" + orderstate + "' + ',') > 0";
            }
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        internal int GetCloseTeamNumByServertype(DateTime daydate, int servertype, int comid, int orderstate, int agentid = 0)
        {
            string sql = "select SUM(u_num)  from b2b_order where pro_id in (select id from b2b_com_pro where server_type=" + servertype + ") and u_traveldate='" + daydate + "' and order_state=" + orderstate + " and comid=" + comid;
            if (agentid > 0)
            {
                sql = "select SUM(u_num)  from b2b_order agentid=" + agentid + " and  where pro_id in (select id from b2b_com_pro where server_type=" + servertype + ") and u_traveldate='" + daydate + "' and order_state=" + orderstate + " and comid=" + comid;
            }
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            catch (Exception e)
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal IList<B2b_order> travelbusorderdetailByiscloseteam(string gooutdate, int proid, int iscloseteam)
        {
            string sqltxt = "";
            if (iscloseteam == 1)//已经截团，则查询处理成功的订单
            {
                sqltxt = @"SELECT   [id] ,[pro_id],[order_type] ,[u_name] ,[u_id] ,[u_phone]  ,[u_num] ,[u_subdate],[payid] ,[pay_price] ,[cost],[profit] ,[order_state] ,[pay_state] ,[send_state],[ticketcode] ,[rebate] ,[ordercome],[u_traveldate],[dealer],comid,openid ,pno,comid,integral,imprest,Ticket,ticketinfo,Agentid,Warrant_type,Warrantid,Backtickettime ,service_order_num,service_req_seq,order_remark,Servicepro_v_state,tocomid,buyuid,child_u_num
  FROM b2b_order where  u_traveldate=@u_traveldate and pro_id=@pro_id and order_state=@order_state";
            }
            else //没有截团，则查询支付成功的订单
            {
                sqltxt = @"SELECT   [id] ,[pro_id],[order_type] ,[u_name] ,[u_id] ,[u_phone]  ,[u_num] ,[u_subdate],[payid] ,[pay_price] ,[cost],[profit] ,[order_state] ,[pay_state] ,[send_state],[ticketcode] ,[rebate] ,[ordercome],[u_traveldate],[dealer],comid,openid ,pno,comid,integral,imprest,Ticket,ticketinfo,Agentid,Warrant_type,Warrantid,Backtickettime ,service_order_num,service_req_seq,order_remark,Servicepro_v_state,tocomid,buyuid,child_u_num
  FROM b2b_order where  u_traveldate=@u_traveldate and pro_id=@pro_id and pay_state=@pay_state";
            }

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@u_traveldate", gooutdate);
            cmd.AddParam("@pro_id", proid);
            if (iscloseteam == 1)
            {
                cmd.AddParam("@order_state", (int)OrderStatus.HasFin);
            }
            else
            {
                cmd.AddParam("@pay_state", (int)PayStatus.HasPay);
            }


            using (var reader = cmd.ExecuteReader())
            {
                List<B2b_order> list = new List<B2b_order>();
                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        U_traveldate = reader.GetValue<DateTime>("U_traveldate"),
                        Dealer = reader.GetValue<string>("dealer"),
                        Comid = reader.GetValue<int>("comid"),
                        Openid = reader.GetValue<string>("openid"),
                        Pno = reader.GetValue<string>("pno"),
                        Integral1 = reader.GetValue<decimal>("integral"),
                        Imprest1 = reader.GetValue<decimal>("imprest"),
                        Ticket = reader.GetValue<decimal>("Ticket"),
                        Ticketinfo = reader.GetValue<string>("ticketinfo") == null ? "" : reader.GetValue<string>("ticketinfo"),
                        Backtickettime = reader.GetValue<DateTime>("Backtickettime"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrantid = reader.GetValue<int>("Warrantid"),
                        Service_order_num = reader.GetValue<string>("Service_order_num"),
                        Service_req_seq = reader.GetValue<string>("Service_req_seq"),
                        Order_remark = reader.GetValue<string>("Order_remark"),
                        Servicepro_v_state = reader.GetValue<string>("Servicepro_v_state"),
                        Tocomid = reader.GetValue<int>("Tocomid"),
                        Buyuid = reader.GetValue<int>("Buyuid"),
                        Child_u_num = reader.GetValue<int>("child_u_num")
                    });
                }
                return list;
            }
        }

        internal IList<B2b_order> travelbusorderdetailBypaystate(string gooutdate, int proid, int paystate)
        {
            string sqltxt = "";

            sqltxt = @"SELECT   [id] ,[pro_id],[order_type] ,[u_name] ,[u_id] ,[u_phone]  ,[u_num] ,[u_subdate],[payid] ,[pay_price] ,[cost],[profit] ,[order_state] ,[pay_state] ,[send_state],[ticketcode] ,[rebate] ,[ordercome],[u_traveldate],[dealer],comid,openid ,pno,comid,integral,imprest,Ticket,ticketinfo,Agentid,Warrant_type,Warrantid,Backtickettime ,service_order_num,service_req_seq,order_remark,Servicepro_v_state,tocomid,buyuid,child_u_num
  FROM b2b_order where  u_traveldate=@u_traveldate and pro_id=@pro_id and pay_state=@pay_state";


            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@u_traveldate", gooutdate);
            cmd.AddParam("@pro_id", proid);

            cmd.AddParam("@pay_state", paystate);



            using (var reader = cmd.ExecuteReader())
            {
                List<B2b_order> list = new List<B2b_order>();
                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        U_traveldate = reader.GetValue<DateTime>("U_traveldate"),
                        Dealer = reader.GetValue<string>("dealer"),
                        Comid = reader.GetValue<int>("comid"),
                        Openid = reader.GetValue<string>("openid"),
                        Pno = reader.GetValue<string>("pno"),
                        Integral1 = reader.GetValue<decimal>("integral"),
                        Imprest1 = reader.GetValue<decimal>("imprest"),
                        Ticket = reader.GetValue<decimal>("Ticket"),
                        Ticketinfo = reader.GetValue<string>("ticketinfo") == null ? "" : reader.GetValue<string>("ticketinfo"),
                        Backtickettime = reader.GetValue<DateTime>("Backtickettime"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrantid = reader.GetValue<int>("Warrantid"),
                        Service_order_num = reader.GetValue<string>("Service_order_num"),
                        Service_req_seq = reader.GetValue<string>("Service_req_seq"),
                        Order_remark = reader.GetValue<string>("Order_remark"),
                        Servicepro_v_state = reader.GetValue<string>("Servicepro_v_state"),
                        Tocomid = reader.GetValue<int>("Tocomid"),
                        Buyuid = reader.GetValue<int>("Buyuid"),
                        Child_u_num = reader.GetValue<int>("child_u_num")
                    });
                }
                return list;
            }
        }

        internal int Upsendstate(int orderid, int sendstate)
        {
            string sql = "update b2b_order set send_state=" + sendstate + "  where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int Uporderstatesendstate(int orderid, int sendstate,int orderstate)
        {
            string sql = "update b2b_order set send_state=" + sendstate + ",order_state=" + orderstate + "  where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int Insertb2b_order_busNamelist(int orderid, string travelname, string travelidcard, string travelnation, string u_name, string u_phone, DateTime dateTime, string u_num, string u_traveldate, int comid, int agentid, int proid, string pickuppoint, string dropoffpoint, string travelphone, string travelremark, string travelpinyin, string traveladdress, string travelpostcode, string travelemail, string travelcredentialsType)
        {
            string sql = @"INSERT INTO  [b2b_order_busNamelist]
           ([orderid]
           ,[name]
           ,[IdCard]
           ,[Nation]
           ,[yuding_name]
           ,[yuding_phone]
           ,[yuding_time]
           ,[yuding_num]
           ,[travel_time]
           ,[comid]
           ,[agentid]
           ,[proid]
           ,pickuppoint,dropoffpoint,contactphone,contactremark,pinyin,address,postcode,email,credentialsType)
     VALUES
           (@orderid 
           ,@name 
           ,@IdCard 
           ,@Nation 
           ,@yuding_name 
           ,@yuding_phone 
           ,@yuding_time 
           ,@yuding_num 
           ,@travel_time 
           ,@comid 
           ,@agentid 
           ,@proid,@pickuppoint,@dropoffpoint,@contactphone,@contactremark,@pinyin,@address,@postcode,@email,@credentialsType)";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@orderid", orderid);
            cmd.AddParam("@name", travelname);
            cmd.AddParam("@IdCard", travelidcard);
            cmd.AddParam("@Nation", travelnation);
            cmd.AddParam("@yuding_name", u_name);
            cmd.AddParam("@yuding_phone", u_phone);
            cmd.AddParam("@yuding_time", dateTime);
            cmd.AddParam("@yuding_num", u_num);
            cmd.AddParam("@travel_time", u_traveldate);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@agentid", agentid);
            cmd.AddParam("@proid", proid);
            cmd.AddParam("@pickuppoint", pickuppoint);
            cmd.AddParam("@dropoffpoint", dropoffpoint);
            cmd.AddParam("@contactphone", travelphone);
            cmd.AddParam("@contactremark", travelremark);
            cmd.AddParam("@pinyin", travelpinyin);
            cmd.AddParam("@address", traveladdress);
            cmd.AddParam("@postcode", travelpostcode);
            cmd.AddParam("@email", travelemail);
            cmd.AddParam("@credentialsType", travelcredentialsType);

            return cmd.ExecuteNonQuery();
        }

        internal IList<b2b_order_busNamelist> travelbustravelerlistBypaystate(string gooutdate, int proid, int paystate, int agentid = 0, string orderstate = "")
        {
            string sqltxt = "select a.* ,b.company,c.order_remark  from agent_company as b right join [b2b_order_busNamelist] as a on a.agentid=b.id left join b2b_order as c on a.orderid=c.id  where a.orderid in ( select id   FROM b2b_order where  u_traveldate='" + gooutdate + "' and pro_id=" + proid + " and pay_state=" + paystate + ")";

            if (agentid > 0)
            {
                sqltxt = "select a.* ,b.company,c.order_remark  from agent_company as b right join [b2b_order_busNamelist] as a on a.agentid=b.id left join b2b_order as c on a.orderid=c.id  where a.orderid in ( select id  FROM b2b_order where  u_traveldate='" + gooutdate + "' and pro_id=" + proid + " and pay_state=" + paystate + " and agentid=" + agentid + ")";
            }

            if (orderstate != "")
            {
                sqltxt += " and  c.order_state in (" + orderstate + ")  ";
            }
            sqltxt += " order by a.pickuppoint desc";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);



            using (var reader = cmd.ExecuteReader())
            {
                List<b2b_order_busNamelist> list = new List<b2b_order_busNamelist>();
                while (reader.Read())
                {
                    list.Add(new b2b_order_busNamelist
                    {
                        id = reader.GetValue<int>("Id"),
                        orderid = reader.GetValue<int>("orderid"),
                        agentid = reader.GetValue<int>("agentid"),
                        comid = reader.GetValue<int>("comid"),
                        IdCard = reader.GetValue<string>("IdCard"),
                        name = reader.GetValue<string>("name"),
                        Nation = reader.GetValue<string>("Nation"),
                        proid = reader.GetValue<int>("proid"),
                        travel_time = reader.GetValue<DateTime>("travel_time"),
                        yuding_name = reader.GetValue<string>("yuding_name"),
                        yuding_num = reader.GetValue<int>("yuding_num"),
                        yuding_phone = reader.GetValue<string>("yuding_phone"),
                        yuding_time = reader.GetValue<DateTime>("yuding_time"),
                        pickuppoint = reader.GetValue<string>("pickuppoint"),
                        dropoffpoint = reader.GetValue<string>("dropoffpoint"),
                        agentname = reader.GetValue<string>("company"),
                        orderremark = reader.GetValue<string>("order_remark"),
                        contactphone = reader.GetValue<string>("contactphone"),
                        contactremark = reader.GetValue<string>("contactremark"),
                        isaboard = reader.GetValue<int>("isaboard"),
                    });
                }
                return list;
            }
        }

        internal List<b2b_order_busNamelist> travelbusordertravalerdetail(string gooutdate, int proid, int order_state)
        {

            const string sqltxt = @"SELECT  a.*,b.company,c.order_remark
  FROM  agent_company as b right join [b2b_order_busNamelist] as a on a.agentid=b.id left join b2b_order as c on a.orderid=c.id where a.orderid in ( SELECT   [id]
  FROM b2b_order where  u_traveldate=@u_traveldate and pro_id=@pro_id and order_state=@order_state)";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@u_traveldate", gooutdate);
            cmd.AddParam("@pro_id", proid);
            cmd.AddParam("@order_state", order_state);

            using (var reader = cmd.ExecuteReader())
            {
                List<b2b_order_busNamelist> list = new List<b2b_order_busNamelist>();
                while (reader.Read())
                {
                    list.Add(new b2b_order_busNamelist
                    {
                        id = reader.GetValue<int>("Id"),
                        orderid = reader.GetValue<int>("orderid"),
                        agentid = reader.GetValue<int>("agentid"),
                        comid = reader.GetValue<int>("comid"),
                        IdCard = reader.GetValue<string>("IdCard"),
                        name = reader.GetValue<string>("name"),
                        Nation = reader.GetValue<string>("Nation"),
                        proid = reader.GetValue<int>("proid"),
                        travel_time = reader.GetValue<DateTime>("travel_time"),
                        yuding_name = reader.GetValue<string>("yuding_name"),
                        yuding_num = reader.GetValue<int>("yuding_num"),
                        yuding_phone = reader.GetValue<string>("yuding_phone"),
                        yuding_time = reader.GetValue<DateTime>("yuding_time"),
                        pickuppoint = reader.GetValue<string>("pickuppoint"),
                        dropoffpoint = reader.GetValue<string>("dropoffpoint"),
                        agentname = reader.GetValue<string>("company"),
                        orderremark = reader.GetValue<string>("order_remark"),
                        contactphone = reader.GetValue<string>("contactphone"),
                        contactremark = reader.GetValue<string>("contactremark"),
                    });
                }
                return list;
            }
        }




        #region 已消费
        internal decimal Xiaofei_price(int proid, int comid, string startdate, string enddate)
        {
            string sqlTxt = @"select sum(b.use_pnum*a.e_sale_price) as money from b2b_eticket as a left join b2b_etcket_log as b on  a.id=b.eticket_id  where  b.action=1 and b.a_state=1 and (a.oid in (select id from b2b_order where pro_id=@proid and comid=@comid and warrant_type=1 and pay_state=2) or a.oid in (select bindingagentorderid from b2b_order where pro_id=@proid and comid=@comid and warrant_type=1 and pay_state=2))";

            if (startdate != "")
            {
                sqlTxt = sqlTxt + " and a.subdate>='" + startdate + "'";
            }
            if (enddate != "")
            {
                enddate = DateTime.Parse(enddate).AddDays(1).ToString();
                sqlTxt = sqlTxt + " and a.subdate<='" + enddate + "'";
            }


            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@proid", proid);
                cmd.AddParam("@comid", comid);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : decimal.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }
        #endregion

        #region 倒码已消费
        internal decimal Daoma_Xiaofei_price(int proid, int comid, string startdate, string enddate)
        {
            string sqlTxt = @"select sum(b.use_pnum*a.e_sale_price) as money from b2b_eticket as a left join b2b_etcket_log as b on  a.id=b.eticket_id  where a.v_state=2 and b.action=1 and b.a_state=1 and (a.oid in (select id from b2b_order where pro_id=@proid  and comid=@comid and warrant_type=2 and pay_state =2) or a.oid in (select bindingagentorderid from b2b_order where pro_id=@proid and comid=@comid and warrant_type=2 and pay_state=2))";
            if (startdate != "")
            {
                sqlTxt = sqlTxt + " and a.subdate>='" + startdate + "'";
            }
            if (enddate != "")
            {
                enddate = DateTime.Parse(enddate).AddDays(1).ToString();
                sqlTxt = sqlTxt + " and a.subdate<='" + enddate + "'";
            }

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@proid", proid);
                cmd.AddParam("@comid", comid);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : decimal.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }
        #endregion

        #region 沉淀
        internal decimal Chendian_price(int proid, int comid, string startdate, string enddate)
        {
            string sqlTxt = @"select sum(a.use_pnum*a.e_sale_price) as money from b2b_eticket as a where a.v_state=1 and (a.oid in (select id from b2b_order where  pro_id=@proid and comid=@comid and warrant_type=1 and pay_state =2) or a.oid in (select bindingagentorderid from b2b_order where  pro_id=@proid and comid=@comid and warrant_type=1 and pay_state =2))";
            if (startdate != "")
            {
                sqlTxt = sqlTxt + " and a.subdate>='" + startdate + "'";
            }
            if (enddate != "")
            {
                enddate = DateTime.Parse(enddate).AddDays(1).ToString();
                sqlTxt = sqlTxt + " and a.subdate<='" + enddate + "'";
            }

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@proid", proid);
                cmd.AddParam("@comid", comid);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : decimal.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }
        #endregion

        #region 退票
        internal decimal Tuipiao_price(int proid, int comid, string startdate, string enddate)
        {
            string sqlTxt = @"select oid from b2b_eticket as a where a.v_state=4 and (a.oid in (select id from b2b_order where  pro_id=@proid and comid=@comid and warrant_type=1 and pay_state =2) or a.oid in (select bindingagentorderid from b2b_order where  pro_id=@proid and comid=@comid and warrant_type=1 and pay_state =2))";
            if (startdate != "")
            {
                sqlTxt = sqlTxt + " and a.subdate>='" + startdate + "'";
            }
            if (enddate != "")
            {
                enddate = DateTime.Parse(enddate).AddDays(1).ToString();
                sqlTxt = sqlTxt + " and a.subdate<='" + enddate + "'";
            }


            sqlTxt = "select sum(cancel_ticketnum*pay_price) as money  from b2b_order where id in(" + sqlTxt + ")";

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@proid", proid);
                cmd.AddParam("@comid", comid);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : decimal.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }
        #endregion

        #region 直销订单退款
        internal decimal BackPrice(string comid, string stardate, string enddate)
        {

            string sqlTxt = @"select sum(cancel_ticketnum*pay_price) as cancel_ticketnum from b2b_order where comid=@comid and pay_state=2 and agentid=0  and backtickettime>=@stardate and backtickettime<@enddate";

            try
            {
                if (stardate.Length == 7)
                {
                    stardate = stardate + "-01";
                    enddate = DateTime.Parse(enddate + "-01").AddMonths(1).AddDays(-1).ToString();
                }

                if (stardate.Length == 10)
                {
                    enddate = DateTime.Parse(enddate).AddDays(1).ToString();
                }
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@stardate", stardate);
                cmd.AddParam("@enddate", enddate);
                cmd.AddParam("@comid", comid);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : decimal.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }
        #endregion


        #region 直销订单 微信
        internal decimal WeixinSale(string comid, string stardate, string enddate)
        {

            string sqlTxt = @"select sum(u_num*pay_price) as u_num from b2b_order where comid=@comid and agentid=0 and openid !='' and pay_state=2 and u_subdate>=@stardate and u_subdate<@enddate";

            try
            {
                if (stardate.Length == 7)
                {
                    stardate = stardate + "-01";
                    enddate = DateTime.Parse(enddate + "-01").AddMonths(1).AddDays(-1).ToString();
                }

                if (stardate.Length == 10)
                {
                    enddate = DateTime.Parse(enddate).AddDays(1).ToString();
                }
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@stardate", stardate);
                cmd.AddParam("@enddate", enddate);
                cmd.AddParam("@comid", comid);
                object o = cmd.ExecuteScalar();

                sqlHelper.Dispose();
                return o == null ? 0 : decimal.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }
        #endregion

        #region 直销订单 网站
        internal decimal WebSale(string comid, string stardate, string enddate)
        {

            string sqlTxt = @"select sum(u_num*pay_price) as u_num from b2b_order where comid=@comid and agentid=0 and openid ='' and pay_state=2 and u_subdate>=@stardate and u_subdate<@enddate";

            try
            {
                if (stardate.Length == 7)
                {
                    stardate = stardate + "-01";
                    enddate = DateTime.Parse(enddate + "-01").AddMonths(1).AddDays(-1).ToString();
                }

                if (stardate.Length == 10)
                {
                    enddate = DateTime.Parse(enddate).AddDays(1).ToString();
                }
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@stardate", stardate);
                cmd.AddParam("@enddate", enddate);
                cmd.AddParam("@comid", comid);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : decimal.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }
        #endregion

        #region 直销订单 已消费
        internal decimal UseState(string comid, string stardate, string enddate)
        {

            string sqlTxt = @"select sum(b.use_pnum*a.e_sale_price) as money from b2b_eticket as a left join b2b_etcket_log as b on a.id=b.eticket_id  where b.action=1 and b.a_state=1 and (a.oid in (select id from b2b_order where comid=@comid and agentid=0 and backtickettime>=@stardate and backtickettime<@enddate) or a.oid in (select bindingagentorderid from b2b_order where comid=@comid and agentid=0 and backtickettime>=@stardate and backtickettime<@enddate and bindingagentorderid !=0))";

            try
            {
                if (stardate.Length == 7)
                {
                    stardate = stardate + "-01";
                    enddate = DateTime.Parse(enddate + "-01").AddMonths(1).AddDays(-1).ToString();
                }

                if (stardate.Length == 10)
                {
                    enddate = DateTime.Parse(enddate).AddDays(1).ToString();
                }
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@stardate", stardate);
                cmd.AddParam("@enddate", enddate);
                cmd.AddParam("@comid", comid);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : decimal.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }
        #endregion

        #region 直销订单 未消费
        internal decimal UnUseState(string comid, string stardate, string enddate)
        {

            string sqlTxt = @"select sum(a.use_pnum*a.e_sale_price) as money from b2b_eticket as a where (a.oid in (select id  from b2b_order where comid=@comid and agentid=0 and backtickettime>=@stardate and backtickettime<@enddate) or a.oid in (select bindingagentorderid  from b2b_order where comid=@comid and agentid=0 and backtickettime>=@stardate and backtickettime<@enddate and bindingagentorderid !=0))";

            try
            {
                if (stardate.Length == 7)
                {
                    stardate = stardate + "-01";
                    enddate = DateTime.Parse(enddate + "-01").AddMonths(1).AddDays(-1).ToString();
                }

                if (stardate.Length == 10)
                {
                    enddate = DateTime.Parse(enddate).AddDays(1).ToString();
                }
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@stardate", stardate);
                cmd.AddParam("@enddate", enddate);
                cmd.AddParam("@comid", comid);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : decimal.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }
        #endregion

        internal string GetPickuppointByorderid(int order_no)
        {
            try
            {
                string sql = "select top 1  pickuppoint from b2b_order_busNamelist where orderid=" + order_no;

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o.ToString();
            }
            catch
            {
                sqlHelper.Dispose();
                return "";
            }
        }

        internal int IsHasLvyoubusProOrder(int comid, int servertype)
        {
            string sql = "select count(1) from b2b_order where pro_id in (select id from  b2b_com_pro where server_type=" + servertype + "  and com_id=" + comid + " )";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            int r = o == null ? 0 : int.Parse(o.ToString());
            if (r > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        internal int InsertCancleTicketNum(int orderid, int num)
        {
            string sql = "update b2b_order set cancel_ticketnum=" + num + "  where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();

        }

        internal B2b_order GetOrderByAgentRequestReqSeq(string req_seq)
        {
            const string sqltxt = @"SELECT   [id]
      ,[pro_id]
      ,[order_type]
      ,[u_name]
      ,[u_id]
      ,[u_phone]
      ,[u_num]
      ,[u_subdate]
      ,[payid]
      ,[pay_price]
      ,[cost]
      ,[profit]
      ,[order_state]
      ,[pay_state]
      ,[send_state]
      ,[ticketcode]
      ,[rebate]
      ,[ordercome]
     ,[u_traveldate]
     ,[dealer]
     ,comid
    ,openid
    ,pno
    ,comid
    ,integral
    ,imprest
    ,Ticket
    ,ticketinfo
,Agentid
,Warrant_type
,Warrantid
,Backtickettime 
,service_order_num
,service_req_seq
,order_remark
,Servicepro_v_state
,tocomid
,buyuid
,child_u_num
,Bindingagentorderid
,pickuppoint
,dropoffpoint
,cancel_ticketnum
,Province
,City
,Address
,Code
,expressfee
,expresscom
,expresscode
,deliverytype
,Shopcartid
,childreduce 
,yanzheng_method 
,Handlid
,qunar_orderid
,askquitfee
,recommendchannelid
,speciid 
,channelcoachid
,baoxiannames
,baoxianpinyinnames
,baoxianidcards
,service_code
,service_usecount
,service_lastcount
,yuyuepno
,yuyueweek
  FROM b2b_order where id=(select top 1 ordernum from agent_requestlog where req_seq=@req_seq and ordernum!='' and request_type='add_order') ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@req_seq", req_seq);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        U_traveldate = reader.GetValue<DateTime>("U_traveldate"),
                        Dealer = reader.GetValue<string>("dealer"),
                        Comid = reader.GetValue<int>("comid"),
                        Openid = reader.GetValue<string>("openid"),
                        Pno = reader.GetValue<string>("pno"),
                        Integral1 = reader.GetValue<decimal>("integral"),
                        Imprest1 = reader.GetValue<decimal>("imprest"),
                        Ticket = reader.GetValue<decimal>("Ticket"),
                        Ticketinfo = reader.GetValue<string>("ticketinfo") == null ? "" : reader.GetValue<string>("ticketinfo"),
                        Backtickettime = reader.GetValue<DateTime>("Backtickettime"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrantid = reader.GetValue<int>("Warrantid"),
                        Service_order_num = reader.GetValue<string>("Service_order_num"),
                        Service_req_seq = reader.GetValue<string>("Service_req_seq"),
                        Order_remark = reader.GetValue<string>("Order_remark"),
                        Servicepro_v_state = reader.GetValue<string>("Servicepro_v_state"),
                        Tocomid = reader.GetValue<int>("Tocomid"),
                        Buyuid = reader.GetValue<int>("Buyuid"),
                        Child_u_num = reader.GetValue<int>("child_u_num"),
                        Bindingagentorderid = reader.GetValue<int>("Bindingagentorderid"),
                        pickuppoint = reader.GetValue<string>("pickuppoint"),
                        dropoffpoint = reader.GetValue<string>("dropoffpoint"),
                        Cancelnum = reader.GetValue<int>("cancel_ticketnum"),

                        Province = reader.GetValue<string>("Province"),
                        City = reader.GetValue<string>("City"),
                        Address = reader.GetValue<string>("Address"),
                        Code = reader.GetValue<string>("Code"),
                        Express = reader.GetValue<decimal>("Expressfee"),
                        Expresscom = reader.GetValue<string>("Expresscom"),
                        Expresscode = reader.GetValue<string>("expresscode"),
                        Deliverytype = reader.GetValue<int>("Deliverytype"),
                        Shopcartid = reader.GetValue<int>("Shopcartid"),
                        childreduce = reader.GetValue<decimal>("childreduce"),
                        yanzheng_method = reader.GetValue<int>("yanzheng_method"),
                        Handlid = reader.GetValue<int>("Handlid"),
                        qunar_orderid = reader.GetValue<string>("qunar_orderid"),
                        askquitfee = reader.GetValue<decimal>("askquitfee"),
                        recommendchannelid = reader.GetValue<int>("recommendchannelid"),
                        Speciid = reader.GetValue<int>("Speciid"),
                        channelcoachid = reader.GetValue<int>("channelcoachid"),
                        baoxiannames = reader.GetValue<string>("baoxiannames"),
                        baoxianpinyinnames = reader.GetValue<string>("baoxianpinyinnames"),
                        baoxianidcards = reader.GetValue<string>("baoxianidcards"),

                        service_code = reader.GetValue<string>("service_code"),
                        service_usecount = reader.GetValue<int>("service_usecount"),
                        service_lastcount = reader.GetValue<int>("service_lastcount"),
                        yuyuepno = reader.GetValue<string>("yuyuepno"),
                        yuyueweek = reader.GetValue<int>("yuyueweek"),
                    };
                }
                return null;
            }
        }

        internal int GetWarrant_typeByOrderid(int orderid)
        {
            try
            {
                string sql = "select warrant_type from b2b_order where id=" + orderid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal int SaveAddress(B2b_order order)
        {
            string wstr = "insert b2b_address (agentid,name,phone,Province,city,address,code) values(@Agentid,@U_name,@U_phone,@Province,@City,@Address,@Code) ";

            var cmd = sqlHelper.PrepareTextSqlCommand(wstr);
            cmd.AddParam("@Agentid", order.Agentid);
            cmd.AddParam("@City", order.City);
            cmd.AddParam("@Province", order.Province);
            cmd.AddParam("@U_name", order.U_name);
            cmd.AddParam("@U_phone", order.U_phone);
            cmd.AddParam("@Address", order.Address);
            cmd.AddParam("@Code", order.Code);
            return cmd.ExecuteNonQuery();
        }

        internal int SaveAddress(B2b_address m)
        {
            try
            {
                if (m.Id == 0)
                {
                    string wstr = "insert b2b_address (agentid,name,phone,Province,city,address,code) values(@Agentid,@U_name,@U_phone,@Province,@City,@Address,@Code);select @@identity; ";

                    var cmd = sqlHelper.PrepareTextSqlCommand(wstr);
                    cmd.AddParam("@Agentid", m.Agentid);
                    cmd.AddParam("@City", m.City);
                    cmd.AddParam("@Province", m.Province);
                    cmd.AddParam("@U_name", m.U_name);
                    cmd.AddParam("@U_phone", m.U_phone);
                    cmd.AddParam("@Address", m.Address);
                    cmd.AddParam("@Code", m.Code);
                    object o = cmd.ExecuteScalar();
                    return int.Parse(o.ToString());
                }
                else
                {
                    string wstr = "update  b2b_address  set agentid=@Agentid,name=@U_name,phone=@U_phone,Province=@Province,city=@City,  address=@Address,code=@Code  where id=@Id";

                    var cmd = sqlHelper.PrepareTextSqlCommand(wstr);
                    cmd.AddParam("@Id", m.Id);
                    cmd.AddParam("@Agentid", m.Agentid);
                    cmd.AddParam("@City", m.City);
                    cmd.AddParam("@Province", m.Province);
                    cmd.AddParam("@U_name", m.U_name);
                    cmd.AddParam("@U_phone", m.U_phone);
                    cmd.AddParam("@Address", m.Address);
                    cmd.AddParam("@Code", m.Code);
                    return cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }


        internal int UpSaveAddress(B2b_order order)
        {
            string wstr = "update b2b_address set name=@U_name,phone=@U_phone,Province=@Province,city=@City,address=@Address,code=@Code where id=@Id ";
            var cmd = sqlHelper.PrepareTextSqlCommand(wstr);
            cmd.AddParam("@Id", order.Id);
            cmd.AddParam("@City", order.City);
            cmd.AddParam("@Province", order.Province);
            cmd.AddParam("@U_name", order.U_name);
            cmd.AddParam("@U_phone", order.U_phone);
            cmd.AddParam("@Address", order.Address);
            cmd.AddParam("@Code", order.Code);
            cmd.AddParam("@Agentid", order.Agentid);
            return cmd.ExecuteNonQuery();
        }


        internal int DelSaveAddress(int id, int agentid)
        {
            string wstr = "delete b2b_address where id=@Id and agentid=@Agentid ";
            var cmd = sqlHelper.PrepareTextSqlCommand(wstr);
            cmd.AddParam("@Id", id);
            cmd.AddParam("@Agentid", agentid);
            return cmd.ExecuteNonQuery();
        }

        internal int SearchCart(int comid, int agentid, int proid, int Speciid)
        {
            try
            {
                string sql = "select id from b2b_SCart where comid=" + comid + " and agentid=" + agentid + " and proid=" + proid + " and Speciid=" + Speciid;
                if (proid == -1)
                {//查询分销商购物车是否有购物
                    sql = "select sum(num) as num from b2b_SCart where comid=" + comid + " and agentid=" + agentid + " and proid in (select id from b2b_com_pro where pro_state=1)";
                }

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }
        internal int SearchCartCount(int comid, int agentid)
        {
            try
            {

                //查询分销商购物车是否有购物
                string sql = "select count(num) as num from b2b_SCart where comid=" + comid + " and agentid=" + agentid + " and proid in (select id from b2b_com_pro where pro_state=1)";


                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal List<B2b_com_pro> SearchCartList(int comid, int agentid, string proid, string cartid = "")
        {
            try
            {
                if (proid != "")
                {
                    if (proid.Substring(proid.Length - 1, 1) == ",")
                    {
                        proid = proid.Substring(0, proid.Length - 1);
                    }
                }
                if (cartid != "")
                {
                    if (cartid.Substring(cartid.Length - 1, 1) == ",")
                    {
                        cartid = cartid.Substring(0, cartid.Length - 1);
                    }
                }



                string sql = "select b.*,a.Speciid,a.num,a.id as Cartid,a.proid from b2b_SCart as a left join  b2b_com_pro as b on a.proid=b.id where a.comid=" + comid + " and a.agentid=" + agentid + " and b.pro_state=1";

                if (proid != "")
                {
                    sql += " and a.proid in (" + proid + ")";
                }
                if (cartid != "")
                {
                    sql += " and a.id in (" + cartid + ")";
                }


                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    List<B2b_com_pro> list = new List<B2b_com_pro>();
                    while (reader.Read())
                    {
                        list.Add(new B2b_com_pro
                        {
                            Id = reader.GetValue<int>("proid"),
                            Com_id = reader.GetValue<int>("com_id"),
                            Pro_name = reader.GetValue<string>("pro_name"),
                            Pro_state = reader.GetValue<int>("pro_state"),
                            Server_type = reader.GetValue<int>("server_type"),
                            Pro_type = reader.GetValue<int>("pro_type"),
                            Source_type = reader.GetValue<int>("Source_type"),
                            Pro_Remark = reader.GetValue<string>("pro_Remark"),
                            Pro_start = reader.GetValue<DateTime>("pro_start"),
                            Pro_end = reader.GetValue<DateTime>("pro_end"),
                            Face_price = reader.GetValue<decimal>("face_price"),
                            Advise_price = reader.GetValue<decimal>("advise_price"),
                            Agent1_price = reader.GetValue<decimal>("Agent1_price"),
                            Agent2_price = reader.GetValue<decimal>("Agent2_price"),
                            Agent3_price = reader.GetValue<decimal>("Agent3_price"),
                            Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),
                            ThatDay_can = reader.GetValue<int>("ThatDay_can"),
                            Thatday_can_day = reader.GetValue<int>("Thatday_can_day"),
                            Service_Contain = reader.GetValue<string>("service_Contain"),
                            Service_NotContain = reader.GetValue<string>("service_NotContain"),
                            Precautions = reader.GetValue<string>("Precautions"),
                            Tuan_pro = reader.GetValue<int>("tuan_pro"),
                            Zhixiao = reader.GetValue<int>("zhixiao"),
                            Agentsale = reader.GetValue<int>("agentsale"),
                            Createtime = reader.GetValue<DateTime>("createtime"),
                            Createuserid = reader.GetValue<int>("createuserid"),

                            Imgurl = reader.GetValue<int>("imgurl"),
                            Tuipiao = reader.GetValue<int>("TuiPiao"),
                            Tuipiao_guoqi = reader.GetValue<int>("TuiPiao_Guoqi"),
                            Tuipiao_endday = reader.GetValue<int>("TuiPiao_Endday"),
                            Projectid = reader.GetValue<int>("projectid"),
                            Bindingid = reader.GetValue<int>("Bindingid"),
                            Viewmethod = reader.GetValue<int>("Viewmethod"),
                            U_num = reader.GetValue<int>("num"),
                            ishasdeliveryfee = reader.GetValue<int>("ishasdeliveryfee"),
                            Speciid = reader.GetValue<int>("Speciid"),
                            Cartid = reader.GetValue<int>("Cartid"),

                        });
                    }
                    return list;
                }
            }
            catch
            {
                sqlHelper.Dispose();
                return null;
            }
        }

        internal int InsertCart(B2b_order order)
        {
            string wstr = "";
            if (order.Id == 0)
            {
                wstr = "insert b2b_SCart (agentid,comid,proid,speciid,num) values(@Agentid,@Comid,@Pro_id,@speciid,@U_num) ";
            }
            else
            {
                wstr = "update b2b_SCart set num=num+@U_num where id=@Id ";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(wstr);
            cmd.AddParam("@Agentid", order.Agentid);
            cmd.AddParam("@Comid", order.Comid);
            cmd.AddParam("@Pro_id", order.Pro_id);
            cmd.AddParam("@U_num", order.U_num);
            cmd.AddParam("@Id", order.Id);
            cmd.AddParam("@speciid", order.Speciid);

            return cmd.ExecuteNonQuery();
        }

        internal int UpCartNum(B2b_order order)
        {
            string wstr = "update b2b_SCart set num=@U_num where Comid=@Comid and agentid=@Agentid and proid=@Pro_id ";
            if (order.Cartid != 0)
            {
                wstr = "update b2b_SCart set num=@U_num where Comid=@Comid and agentid=@Agentid and id=@Cartid ";
            }



            var cmd = sqlHelper.PrepareTextSqlCommand(wstr);
            cmd.AddParam("@Agentid", order.Agentid);
            cmd.AddParam("@Comid", order.Comid);
            cmd.AddParam("@Pro_id", order.Pro_id);
            cmd.AddParam("@U_num", order.U_num);
            cmd.AddParam("@Cartid", order.Cartid);


            return cmd.ExecuteNonQuery();
        }
        internal int DelCart(B2b_order order, string cartid)
        {
            string wstr = "delete b2b_SCart  where Comid=@Comid and agentid=@Agentid and id in (@cartid) ";

            if (cartid == "")
            {
                wstr = "delete b2b_SCart  where Comid=@Comid and agentid=@Agentid and proid= " + order.Pro_id + " and speciid=" + order.Speciid;
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(wstr);
            cmd.AddParam("@Agentid", order.Agentid);
            cmd.AddParam("@Comid", order.Comid);
            cmd.AddParam("@cartid", cartid);
            return cmd.ExecuteNonQuery();
        }




        internal int SearchUserCart(int comid, string userid, int proid, int Speciid)
        {
            try
            {
                string sql = "select id from b2b_SCart where comid=" + comid + " and userid='" + userid + "' and proid=" + proid + " and Speciid=" + Speciid;
                if (proid == -1)
                {//查询分销商购物车是否有购物
                    sql = "select sum(num) as num from b2b_SCart where comid=" + comid + " and userid='" + userid + "' and proid in (select id from b2b_com_pro where pro_state=1)";
                }

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal List<B2b_com_pro> SearchUserCartList(int comid, string userid, string proid, string speciid = "")
        {
            try
            {
                //如果最右侧为空格则去掉
                if (proid != "")
                {
                    if (proid.Substring(proid.Length - 1, 1) == ",")
                    {
                        proid = proid.Substring(0, proid.Length - 1);
                    }
                }

                if (speciid != "")
                {
                    if (speciid.Substring(speciid.Length - 1, 1) == ",")
                    {
                        speciid = speciid.Substring(0, speciid.Length - 1);
                    }
                }


                string sql = "select b.*,a.num,a.id as cartid,a.speciid,a.proid from b2b_SCart as a left join  b2b_com_pro as b on a.proid=b.id where a.comid=" + comid + " and a.userid='" + userid + "' and b.pro_state=1";


                if (proid != "")
                {
                    sql += " and a.proid in (" + proid + ")";
                }

                if (speciid != "")
                {
                    sql += " and a.speciid in (" + speciid + ")";

                }

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    List<B2b_com_pro> list = new List<B2b_com_pro>();
                    while (reader.Read())
                    {
                        list.Add(new B2b_com_pro
                        {
                            Id = reader.GetValue<int>("proid"),
                            Com_id = reader.GetValue<int>("com_id"),
                            Pro_name = reader.GetValue<string>("pro_name"),
                            Pro_state = reader.GetValue<int>("pro_state"),
                            Server_type = reader.GetValue<int>("server_type"),
                            Pro_type = reader.GetValue<int>("pro_type"),
                            Source_type = reader.GetValue<int>("Source_type"),
                            Pro_Remark = reader.GetValue<string>("pro_Remark"),
                            Pro_start = reader.GetValue<DateTime>("pro_start"),
                            Pro_end = reader.GetValue<DateTime>("pro_end"),
                            Face_price = reader.GetValue<decimal>("face_price"),
                            Advise_price = reader.GetValue<decimal>("advise_price"),
                            Agent1_price = reader.GetValue<decimal>("Agent1_price"),
                            Agent2_price = reader.GetValue<decimal>("Agent2_price"),
                            Agent3_price = reader.GetValue<decimal>("Agent3_price"),
                            Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),
                            ThatDay_can = reader.GetValue<int>("ThatDay_can"),
                            Thatday_can_day = reader.GetValue<int>("Thatday_can_day"),
                            Service_Contain = reader.GetValue<string>("service_Contain"),
                            Service_NotContain = reader.GetValue<string>("service_NotContain"),
                            Precautions = reader.GetValue<string>("Precautions"),
                            Tuan_pro = reader.GetValue<int>("tuan_pro"),
                            Zhixiao = reader.GetValue<int>("zhixiao"),
                            Agentsale = reader.GetValue<int>("agentsale"),
                            Createtime = reader.GetValue<DateTime>("createtime"),
                            Createuserid = reader.GetValue<int>("createuserid"),

                            Imgurl = reader.GetValue<int>("imgurl"),
                            Tuipiao = reader.GetValue<int>("TuiPiao"),
                            Tuipiao_guoqi = reader.GetValue<int>("TuiPiao_Guoqi"),
                            Tuipiao_endday = reader.GetValue<int>("TuiPiao_Endday"),
                            Projectid = reader.GetValue<int>("projectid"),
                            Bindingid = reader.GetValue<int>("Bindingid"),
                            Viewmethod = reader.GetValue<int>("Viewmethod"),
                            U_num = reader.GetValue<int>("num"),
                            Cartid = reader.GetValue<int>("cartid"),
                            Speciid = reader.GetValue<int>("Speciid"),
                        });
                    }
                    return list;
                }
            }
            catch
            {
                sqlHelper.Dispose();
                return null;
            }
        }


        internal List<B2b_com_pro> SearchCartListBycartid(string cartid)
        {
            try
            {
                //如果最右侧为空格则去掉
                if (cartid != "")
                {
                    if (cartid.Substring(cartid.Length - 1, 1) == ",")
                    {
                        cartid = cartid.Substring(0, cartid.Length - 1);
                    }
                }




                string sql = "select b.*,a.num,a.id as cartid,a.speciid,a.proid from b2b_SCart as a left join  b2b_com_pro as b on a.proid=b.id where b.pro_state=1";

                sql += " and a.id in (" + cartid + ")";

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    List<B2b_com_pro> list = new List<B2b_com_pro>();
                    while (reader.Read())
                    {
                        list.Add(new B2b_com_pro
                        {
                            Id = reader.GetValue<int>("proid"),
                            Com_id = reader.GetValue<int>("com_id"),
                            Pro_name = reader.GetValue<string>("pro_name"),
                            Pro_state = reader.GetValue<int>("pro_state"),
                            Server_type = reader.GetValue<int>("server_type"),
                            Pro_type = reader.GetValue<int>("pro_type"),
                            Source_type = reader.GetValue<int>("Source_type"),
                            Pro_Remark = reader.GetValue<string>("pro_Remark"),
                            Pro_start = reader.GetValue<DateTime>("pro_start"),
                            Pro_end = reader.GetValue<DateTime>("pro_end"),
                            Face_price = reader.GetValue<decimal>("face_price"),
                            Advise_price = reader.GetValue<decimal>("advise_price"),
                            Agent1_price = reader.GetValue<decimal>("Agent1_price"),
                            Agent2_price = reader.GetValue<decimal>("Agent2_price"),
                            Agent3_price = reader.GetValue<decimal>("Agent3_price"),
                            Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),
                            ThatDay_can = reader.GetValue<int>("ThatDay_can"),
                            Thatday_can_day = reader.GetValue<int>("Thatday_can_day"),
                            Service_Contain = reader.GetValue<string>("service_Contain"),
                            Service_NotContain = reader.GetValue<string>("service_NotContain"),
                            Precautions = reader.GetValue<string>("Precautions"),
                            Tuan_pro = reader.GetValue<int>("tuan_pro"),
                            Zhixiao = reader.GetValue<int>("zhixiao"),
                            Agentsale = reader.GetValue<int>("agentsale"),
                            Createtime = reader.GetValue<DateTime>("createtime"),
                            Createuserid = reader.GetValue<int>("createuserid"),

                            Imgurl = reader.GetValue<int>("imgurl"),
                            Tuipiao = reader.GetValue<int>("TuiPiao"),
                            Tuipiao_guoqi = reader.GetValue<int>("TuiPiao_Guoqi"),
                            Tuipiao_endday = reader.GetValue<int>("TuiPiao_Endday"),
                            Projectid = reader.GetValue<int>("projectid"),
                            Bindingid = reader.GetValue<int>("Bindingid"),
                            Viewmethod = reader.GetValue<int>("Viewmethod"),
                            U_num = reader.GetValue<int>("num"),
                            Cartid = reader.GetValue<int>("cartid"),
                            Speciid = reader.GetValue<int>("Speciid"),
                        });
                    }
                    return list;
                }
            }
            catch
            {
                sqlHelper.Dispose();
                return null;
            }
        }



        internal string SearchUserCartSpeciNameList(int comid, int cartid, int speciid, string viewtype)
        {
            try
            {


                string sql = "select * from b2b_SCart as a left join b2b_com_pro_Speci as b on a.speciid=b.id where a.comid=" + comid + " and a.id=" + cartid;


                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        if (viewtype == "speci_name")
                        {
                            return reader.GetValue<string>("speci_name");
                        }
                        if (viewtype == "speci_face_price")
                        {
                            return reader.GetValue<decimal>("speci_face_price").ToString();
                        }
                        if (viewtype == "speci_advise_price")
                        {
                            return reader.GetValue<decimal>("speci_advise_price").ToString();
                        }
                        if (viewtype == "speci_agent1_price")
                        {
                            return reader.GetValue<decimal>("speci_agent1_price").ToString();
                        }
                        if (viewtype == "speci_agent2_price")
                        {
                            return reader.GetValue<decimal>("speci_agent2_price").ToString();
                        }
                        if (viewtype == "speci_agent3_price")
                        {
                            return reader.GetValue<decimal>("speci_agent3_price").ToString();
                        }
                        if (viewtype == "speci_agentsettle_price")
                        {
                            return reader.GetValue<decimal>("speci_agentsettle_price").ToString();
                        }
                    }
                    return "";
                }
            }
            catch
            {
                sqlHelper.Dispose();
                return "";
            }
        }


        internal decimal SearchUserCartSpeciPrcieList(int comid, int cartid, int speciid, string viewtype)
        {
            try
            {


                string sql = "select * from b2b_SCart as a left join b2b_com_pro_Speci as b on a.speciid=b.id where a.comid=" + comid + " and a.id=" + cartid;


                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {

                        if (viewtype == "speci_face_price")
                        {
                            return reader.GetValue<decimal>("speci_face_price");
                        }
                        if (viewtype == "speci_advise_price")
                        {
                            return reader.GetValue<decimal>("speci_advise_price");
                        }
                        if (viewtype == "speci_agent1_price")
                        {
                            return reader.GetValue<decimal>("speci_agent1_price");
                        }
                        if (viewtype == "speci_agent2_price")
                        {
                            return reader.GetValue<decimal>("speci_agent2_price");
                        }
                        if (viewtype == "speci_agent3_price")
                        {
                            return reader.GetValue<decimal>("speci_agent3_price");
                        }
                        if (viewtype == "speci_agentsettle_price")
                        {
                            return reader.GetValue<decimal>("speci_agentsettle_price");
                        }
                    }
                    return 0;
                }
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal List<B2b_com_pro> SearchUserOrderList(int comid, int cartid, string proid)
        {
            try
            {
                //如果最右侧为空格则去掉
                if (proid != "")
                {
                    if (proid.Substring(proid.Length - 1, 1) == ",")
                    {
                        proid = proid.Substring(0, proid.Length - 1);
                    }
                }

                string sql = "select * from b2b_order as a left join  b2b_com_pro as b on a.pro_id=b.id where a.comid=" + comid + " and a.shopcartid='" + cartid + "'";

                if (proid != "" && cartid == 0)
                {
                    sql = " select * from b2b_order as a left join  b2b_com_pro as b on a.pro_id=b.id where a.comid=" + comid + " and a.pro_id in (" + proid + ")";
                }

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    List<B2b_com_pro> list = new List<B2b_com_pro>();
                    while (reader.Read())
                    {
                        list.Add(new B2b_com_pro
                        {
                            Id = reader.GetValue<int>("pro_id"),
                            Com_id = reader.GetValue<int>("comid"),
                            Pro_name = reader.GetValue<string>("pro_name"),
                            Pro_state = reader.GetValue<int>("pro_state"),
                            Server_type = reader.GetValue<int>("server_type"),
                            Pro_type = reader.GetValue<int>("pro_type"),
                            Source_type = reader.GetValue<int>("Source_type"),
                            Pro_Remark = reader.GetValue<string>("pro_Remark"),
                            Pro_start = reader.GetValue<DateTime>("pro_start"),
                            Pro_end = reader.GetValue<DateTime>("pro_end"),
                            Face_price = reader.GetValue<decimal>("face_price"),
                            Advise_price = reader.GetValue<decimal>("advise_price"),
                            Agent1_price = reader.GetValue<decimal>("Agent1_price"),
                            Agent2_price = reader.GetValue<decimal>("Agent2_price"),
                            Agent3_price = reader.GetValue<decimal>("Agent3_price"),
                            Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),
                            ThatDay_can = reader.GetValue<int>("ThatDay_can"),
                            Thatday_can_day = reader.GetValue<int>("Thatday_can_day"),
                            Service_Contain = reader.GetValue<string>("service_Contain"),
                            Service_NotContain = reader.GetValue<string>("service_NotContain"),
                            Precautions = reader.GetValue<string>("Precautions"),
                            Tuan_pro = reader.GetValue<int>("tuan_pro"),
                            Zhixiao = reader.GetValue<int>("zhixiao"),
                            Agentsale = reader.GetValue<int>("agentsale"),
                            Createtime = reader.GetValue<DateTime>("createtime"),
                            Createuserid = reader.GetValue<int>("createuserid"),

                            Imgurl = reader.GetValue<int>("imgurl"),
                            Tuipiao = reader.GetValue<int>("TuiPiao"),
                            Tuipiao_guoqi = reader.GetValue<int>("TuiPiao_Guoqi"),
                            Tuipiao_endday = reader.GetValue<int>("TuiPiao_Endday"),
                            Projectid = reader.GetValue<int>("projectid"),
                            Bindingid = reader.GetValue<int>("Bindingid"),
                            Viewmethod = reader.GetValue<int>("Viewmethod"),
                            U_num = reader.GetValue<int>("u_num"),
                        });
                    }
                    return list;
                }
            }
            catch
            {
                sqlHelper.Dispose();
                return null;
            }
        }

        internal decimal SumCartPrice(int comid, string userid, string cartid)
        {
            try
            {
                decimal money = 0;
                string sql = "select  CASE WHEN a.speciid = 0 THEN sum (a.num*b.advise_price) ELSE sum (a.num*c.speci_advise_price) END as money from b2b_SCart as a left join  b2b_com_pro as b on a.proid=b.id left join b2b_com_pro_Speci as c on a.speciid=c.id where a.comid=" + comid + " and a.userid='" + userid + "' and b.pro_state=1";
                sql += " and a.id in (" + cartid + ") group by a.speciid";

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    List<B2b_com_pro> list = new List<B2b_com_pro>();
                    while (reader.Read())
                    {
                        money += reader.GetValue<decimal>("money");
                    }
                    return money;
                }
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal decimal SumCartSpeciPrice(int comid, string userid, string Speciid)
        {
            try
            {
                decimal money = 0;
                string sql = "select  sum (a.num*b.speci_advise_price) as money from b2b_SCart as a left join  b2b_com_pro_Speci as b on a.Speciid=b.id where a.comid=" + comid + " and speciid!=0 and a.userid='" + userid + "'";
                sql += " and a.Speciid in (" + Speciid + ")";

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    List<B2b_com_pro> list = new List<B2b_com_pro>();
                    while (reader.Read())
                    {
                        money += reader.GetValue<decimal>("money");
                    }
                    return money;
                }
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal int InsertUserCart(B2b_order order)
        {
            string wstr = "";
            if (order.Id == 0)
            {
                wstr = "insert b2b_SCart (userid,comid,proid,Speciid,num) values(@userid,@Comid,@Pro_id,@Speciid,@U_num) ";
            }
            else
            {
                wstr = "update b2b_SCart set num=num+@U_num where id=@Id ";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(wstr);
            cmd.AddParam("@userid", order.Openid);
            cmd.AddParam("@Comid", order.Comid);
            cmd.AddParam("@Pro_id", order.Pro_id);
            cmd.AddParam("@U_num", order.U_num);
            cmd.AddParam("@Id", order.Id);
            cmd.AddParam("@Speciid", order.Speciid);
            return cmd.ExecuteNonQuery();
        }

        internal int UpUserCartNum(B2b_order order)
        {
            string wstr = "update b2b_SCart set num=@U_num where Comid=@Comid and userid=@userid and proid=@Pro_id and Speciid=@Speciid ";
            if (order.Cartid != 0)
            {
                wstr = "update b2b_SCart set num=@U_num where id=@cartid ";
            }


            var cmd = sqlHelper.PrepareTextSqlCommand(wstr);
            cmd.AddParam("@userid", order.Openid);
            cmd.AddParam("@Comid", order.Comid);
            cmd.AddParam("@Pro_id", order.Pro_id);
            cmd.AddParam("@U_num", order.U_num);
            cmd.AddParam("@Speciid", order.Speciid);
            cmd.AddParam("@cartid", order.Cartid);
            return cmd.ExecuteNonQuery();
        }
        internal int DelUserCart(B2b_order order, string cartid)
        {
            string wstr = "delete b2b_SCart  where Comid=@Comid and userid=@userid and id in (@cartid) ";

            if (cartid == "")//针对没有购物车ID，主要应用在提交订单成功后，删除此购物车产品
            {
                wstr = "delete b2b_SCart where Comid=@Comid and userid=@userid and proid= " + order.Pro_id + " and speciid=" + order.Speciid;
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(wstr);
            cmd.AddParam("@userid", order.Openid);
            cmd.AddParam("@Comid", order.Comid);
            cmd.AddParam("@cartid", cartid);
            return cmd.ExecuteNonQuery();
        }





        #region  分销订单列表
        internal List<B2b_order> SaveAddressPageList(int agentid, int pageindex, int pagesize, string key, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_address";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = "agentid=" + agentid;
            if (key != "")
            {
                condition = condition + " and (name='" + key + "' or phone='" + key + "'or address  like '%" + key + "%')";
            }

            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_order> list = new List<B2b_order>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        U_name = reader.GetValue<string>("name"),
                        U_phone = reader.GetValue<string>("phone"),
                        Province = reader.GetValue<string>("Province"),
                        City = reader.GetValue<string>("City"),
                        Address = reader.GetValue<string>("Address"),
                        Code = reader.GetValue<string>("Code"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;

        }
        #endregion



        internal B2b_address Getagentaddrbyid(int addrid)
        {
            var sql = "SELECT [id]   ,[agentid]    ,[name]  ,[phone]  ,[province]  ,[city]  ,[address]  ,[code]   FROM  [b2b_address] where id=" + addrid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                B2b_address m = null;
                if (reader.Read())
                {
                    m = new B2b_address()
                    {
                        Id = reader.GetValue<int>("id"),
                        Address = reader.GetValue<string>("address"),
                        Agentid = reader.GetValue<int>("agentid"),
                        City = reader.GetValue<string>("city"),
                        Province = reader.GetValue<string>("province"),
                        Code = reader.GetValue<string>("code"),
                        U_name = reader.GetValue<string>("name"),
                        U_phone = reader.GetValue<string>("phone"),

                    };
                }
                return m;
            }
        }

        internal int Deladdr(int addrid)
        {
            string sql = "delete B2b_address  where id=" + addrid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal decimal GetAgentCartTotalPrice(int comid, int agentid, string proid, int Agentlevel)
        {
            try
            {
                string sql = "";
                if (Agentlevel == 1)
                {
                    sql = "select sum(b.Agent1_price*a.num) from b2b_SCart as a left join  b2b_com_pro as b on a.proid=b.id where a.comid=" + comid + " and a.agentid=" + agentid + " and b.pro_state=1";
                }
                if (Agentlevel == 2)
                {
                    sql = "select sum(b.Agent2_price*a.num) from b2b_SCart as a left join  b2b_com_pro as b on a.proid=b.id where a.comid=" + comid + " and a.agentid=" + agentid + " and b.pro_state=1";

                }
                if (Agentlevel == 3)
                {
                    sql = "select sum(b.Agent3_price*a.num) from b2b_SCart as a left join  b2b_com_pro as b on a.proid=b.id where a.comid=" + comid + " and a.agentid=" + agentid + " and b.pro_state=1";

                }


                if (sql != "")
                {
                    if (proid != "")
                    {
                        sql += " and a.proid in (" + proid + ")";
                    }
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    object o = cmd.ExecuteScalar();
                    sqlHelper.Dispose();
                    return decimal.Parse(o.ToString());
                }
                else
                {
                    return 0;
                }

            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }


        internal decimal GetCartOrderMoneyById(int orderid)
        {
            const string sqltxt = @"SELECT (pay_price*u_num-integral-imprest+expressfee) as money from b2b_order where shopcartid in ( select shopcartid FROM b2b_order where [Id]=@orderid)
  ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@orderid", orderid);
            decimal money = 0;
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    money += reader.GetValue<decimal>("money");
                }
                return money;
            }
        }

        internal decimal GetCartOrderExpressMoneyById(int orderid)
        {
            const string sqltxt = @"SELECT (expressfee) as money from b2b_order where shopcartid in ( select shopcartid FROM b2b_order where [Id]=@orderid)
  ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@orderid", orderid);
            decimal money = 0;
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    money += reader.GetValue<decimal>("money");
                }
                return money;
            }
        }

        internal string GetCartOrderProById(int orderid)
        {
            const string sqltxt = @"SELECT * from b2b_order as a left join b2b_com_pro as b on a.pro_id=b.id where a.shopcartid in ( select shopcartid FROM b2b_order where [Id]=@orderid)
  ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@orderid", orderid);
            string proname = "";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    proname += reader.GetValue<string>("pro_name") + "*" + reader.GetValue<int>("u_num").ToString() + ",";
                }
                return proname;
            }
        }


        internal IList<B2b_order> shopcartorder(int shopcartid)
        {
            string sqltxt = "";

            sqltxt = @"SELECT  *  FROM b2b_order where shopcartid=@shopcartid";


            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@shopcartid", shopcartid);


            using (var reader = cmd.ExecuteReader())
            {
                List<B2b_order> list = new List<B2b_order>();
                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        U_traveldate = reader.GetValue<DateTime>("U_traveldate"),
                        Dealer = reader.GetValue<string>("dealer"),
                        Comid = reader.GetValue<int>("comid"),
                        Openid = reader.GetValue<string>("openid"),
                        Pno = reader.GetValue<string>("pno"),
                        Integral1 = reader.GetValue<decimal>("integral"),
                        Imprest1 = reader.GetValue<decimal>("imprest"),
                        Ticket = reader.GetValue<decimal>("Ticket"),
                        Ticketinfo = reader.GetValue<string>("ticketinfo") == null ? "" : reader.GetValue<string>("ticketinfo"),
                        Backtickettime = reader.GetValue<DateTime>("Backtickettime"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrantid = reader.GetValue<int>("Warrantid"),
                        Service_order_num = reader.GetValue<string>("Service_order_num"),
                        Service_req_seq = reader.GetValue<string>("Service_req_seq"),
                        Order_remark = reader.GetValue<string>("Order_remark"),
                        Servicepro_v_state = reader.GetValue<string>("Servicepro_v_state"),
                        Tocomid = reader.GetValue<int>("Tocomid"),
                        Buyuid = reader.GetValue<int>("Buyuid"),
                        Child_u_num = reader.GetValue<int>("child_u_num"),
                        Bindingagentorderid = reader.GetValue<int>("Bindingagentorderid"),
                        pickuppoint = reader.GetValue<string>("pickuppoint"),
                        dropoffpoint = reader.GetValue<string>("dropoffpoint"),
                        Cancelnum = reader.GetValue<int>("cancel_ticketnum"),

                        Province = reader.GetValue<string>("Province"),
                        City = reader.GetValue<string>("City"),
                        Address = reader.GetValue<string>("Address"),
                        Code = reader.GetValue<string>("Code"),
                        Express = reader.GetValue<decimal>("Expressfee"),
                        Expresscom = reader.GetValue<string>("Expresscom"),
                        Expresscode = reader.GetValue<string>("expresscode"),
                        Deliverytype = reader.GetValue<int>("Deliverytype"),
                        Shopcartid = reader.GetValue<int>("Shopcartid"),
                    });
                }
                return list;
            }
        }


        internal int Uporderstate(int orderid, int orderstate)
        {
            string sql = "update b2b_order set order_state=" + orderstate + " where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int UpB2borderPhone(int orderid, string phone)
        {
            string sql = "update b2b_order set u_phone='" + phone + "' where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int GetBindOrderIdByOrderid(int orderid)
        {
            string sql = "select Bindingagentorderid from b2b_order where  id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int bindorderid = 0;
                if (reader.Read())
                {
                    bindorderid = reader.GetValue<int>("Bindingagentorderid");
                }
                return bindorderid;
            }
        }


        internal int UpHandlid(int orderid, int Handlid)
        {
            string sql = "update b2b_order set Handlid=" + Handlid + " where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal bool Ishas_qunarorder(string qunar_orderId)
        {
            string sql = "select count(1) from b2b_order where qunar_orderid='" + qunar_orderId + "' and qunar_orderid!=''";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            if (int.Parse(o.ToString()) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal int InsertQunar_Orderid(int parterorderid, string qunar_orderid)
        {
            try
            {
                if (parterorderid > 0 && qunar_orderid != "")
                {
                    string sql = "update b2b_order set qunar_orderid='" + qunar_orderid + "' where id=" + parterorderid;
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    return cmd.ExecuteNonQuery();
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }

        }

        internal int GetParterOrderId(string qunar_orderId)
        {
            string sql = "select id from b2b_order where qunar_orderid='" + qunar_orderId + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int id = 0;
                if (reader.Read())
                {
                    id = reader.GetValue<int>("id");
                }
                return id;
            }
        }
        /// <summary>
        /// 得到已经消费的数量
        /// </summary>
        /// <param name="pno"></param>
        /// <returns></returns>
        internal int GetHasConsumeNumByPno(string pno)
        {
            try
            {
                string sql = "select  (pnum-use_pnum) as consumenum from b2b_eticket where pno='" + pno + "'";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    int consumenum = 0;
                    if (reader.Read())
                    {
                        consumenum = reader.GetValue<int>("consumenum");
                    }
                    return consumenum;
                }
            }
            catch
            {
                return 0;
            }
        }

        internal int GetOrderState(string partnerorderId)
        {
            string sql = "select order_state from b2b_order where id=" + partnerorderId;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int orderstate = 0;
                if (reader.Read())
                {
                    orderstate = reader.GetValue<int>("order_state");
                }
                return orderstate;
            }
        }

        internal int UpdateOrder_contactperson(string partnerorderId, string c_name, string c_mobile)
        {
            string sql = "update b2b_order set u_name='" + c_name + "' ,u_phone='" + c_mobile + "' where id=" + partnerorderId;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int GetHasConsumeNumByOrderId(int orderid)
        {
            try
            {
                //判断订单是否有绑定订单，有绑定订单 根据绑定订单查询 已经消费的数量
                string sq = "select Bindingagentorderid from b2b_order where id=" + orderid;
                var cm = sqlHelper.PrepareTextSqlCommand(sq);
                using (var reader = cm.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader.GetValue<int>("Bindingagentorderid") > 0)
                        {
                            orderid = reader.GetValue<int>("Bindingagentorderid");
                        }
                    }
                }


                string sql = "select  sum(pnum-use_pnum) as consumenum from b2b_eticket where oid=" + orderid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    int num = 0;
                    if (reader.Read())
                    {
                        num = reader.GetValue<int>("consumenum");
                    }
                    return num;
                }
            }
            catch
            {
                return 0;
            }
        }

        internal bool CancelOrder(string orderid)
        {
            string sql = "update b2b_order set order_state=" + (int)OrderStatus.Timeout + " where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.ExecuteNonQuery();
            return true;
        }

        internal int GetSurplusNum(int orderid)
        {
            try
            {
                //判断订单是否含有绑定订单，含有绑定订单则求绑定订单的可用数量
                string sql = "select bindingagentorderid  from b2b_order where id=" + orderid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader.GetValue<int>("bindingagentorderid") > 0)
                        {
                            orderid = reader.GetValue<int>("bindingagentorderid");
                        }
                    }
                }

                string sql2 = "select sum(use_pnum) as canusenum from b2b_eticket where oid=" + orderid;

                cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                using (var reader = cmd.ExecuteReader())
                {
                    int dealid = 0;
                    if (reader.Read())
                    {
                        dealid = reader.GetValue<int>("canusenum");
                    }

                    return dealid;
                }

            }
            catch
            {
                return 0;
            }
        }

        internal int Insquitnum(int orderid, int refundNum)
        {
            string sql = "update b2b_order set  cancel_ticketnum=cancel_ticketnum+" + refundNum + " where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int Upqunarcashback(string partnerorderId, string orderCashBackMoney)
        {
            string sql = "update b2b_order set  qunar_CashBack='" + orderCashBackMoney + "' where id=" + partnerorderId;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int InsNuomiDealid(int orderid, int nuomidealid)
        {
            string sql = "update b2b_order set nuomi_dealid='" + nuomidealid + "' where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal string GetNuomi_dealid(int orderid)
        {
            string sql = "select nuomi_dealid from b2b_order where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                string nuomi_dealid = "";
                if (reader.Read())
                {
                    nuomi_dealid = reader.GetValue<string>("nuomi_dealid");
                }
                return nuomi_dealid;
            }
        }

        internal int Getinitorderid(int importorderid)
        {
            string sql = "select top 1 id from  b2b_order where Bindingagentorderid=" + importorderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int orderid = 0;
                if (reader.Read())
                {
                    orderid = reader.GetValue<int>("id");
                }
                return orderid;
            }
        }

        internal int GetConsumeNum(int orderid)
        {

            try
            {
                //判断订单是否含有绑定订单，含有绑定订单则求绑定订单的可用数量
                string sql = "select bindingagentorderid  from b2b_order where id=" + orderid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader.GetValue<int>("bindingagentorderid") > 0)
                        {
                            orderid = reader.GetValue<int>("bindingagentorderid");
                        }
                    }
                }

                string sql2 = "select sum(pnum-use_pnum) as consumenum from b2b_eticket where oid=" + orderid;

                cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                using (var reader = cmd.ExecuteReader())
                {
                    int dealid = 0;
                    if (reader.Read())
                    {
                        dealid = reader.GetValue<int>("consumenum");
                    }

                    return dealid;
                }

            }
            catch
            {

                return 0;
            }


        }

        internal B2b_order GetOldorderBybindingId(int bindingorderid)
        {

            string sqltxt = @"SELECT   *
  FROM b2b_order where Bindingagentorderid=@bindingorderid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@bindingorderid", bindingorderid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        U_traveldate = reader.GetValue<DateTime>("U_traveldate"),
                        Dealer = reader.GetValue<string>("dealer"),
                        Comid = reader.GetValue<int>("comid"),
                        Openid = reader.GetValue<string>("openid"),
                        Pno = reader.GetValue<string>("pno"),
                        Integral1 = reader.GetValue<decimal>("integral"),
                        Imprest1 = reader.GetValue<decimal>("imprest"),
                        Ticket = reader.GetValue<decimal>("Ticket"),
                        Ticketinfo = reader.GetValue<string>("ticketinfo") == null ? "" : reader.GetValue<string>("ticketinfo"),
                        Backtickettime = reader.GetValue<DateTime>("Backtickettime"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrantid = reader.GetValue<int>("Warrantid"),
                        Service_order_num = reader.GetValue<string>("Service_order_num"),
                        Service_req_seq = reader.GetValue<string>("Service_req_seq"),
                        Order_remark = reader.GetValue<string>("Order_remark"),
                        Servicepro_v_state = reader.GetValue<string>("Servicepro_v_state"),
                        Tocomid = reader.GetValue<int>("Tocomid"),
                        Buyuid = reader.GetValue<int>("Buyuid"),
                        Child_u_num = reader.GetValue<int>("child_u_num"),
                        Bindingagentorderid = reader.GetValue<int>("Bindingagentorderid"),
                        pickuppoint = reader.GetValue<string>("pickuppoint"),
                        dropoffpoint = reader.GetValue<string>("dropoffpoint"),
                        Cancelnum = reader.GetValue<int>("cancel_ticketnum"),

                        Province = reader.GetValue<string>("Province"),
                        City = reader.GetValue<string>("City"),
                        Address = reader.GetValue<string>("Address"),
                        Code = reader.GetValue<string>("Code"),
                        Express = reader.GetValue<decimal>("Expressfee"),
                        Expresscom = reader.GetValue<string>("Expresscom"),
                        Expresscode = reader.GetValue<string>("expresscode"),
                        Deliverytype = reader.GetValue<int>("Deliverytype"),
                        Shopcartid = reader.GetValue<int>("Shopcartid"),
                        childreduce = reader.GetValue<decimal>("childreduce"),
                        yanzheng_method = reader.GetValue<int>("yanzheng_method"),
                        Handlid = reader.GetValue<int>("Handlid"),
                        qunar_orderid = reader.GetValue<string>("qunar_orderid"),
                        askquitfee = reader.GetValue<decimal>("askquitfee"),
                        recommendchannelid = reader.GetValue<int>("recommendchannelid"),
                        Speciid = reader.GetValue<int>("Speciid"),
                        channelcoachid = reader.GetValue<int>("channelcoachid"),
                        baoxiannames = reader.GetValue<string>("baoxiannames"),
                        baoxianpinyinnames = reader.GetValue<string>("baoxianpinyinnames"),
                        baoxianidcards = reader.GetValue<string>("baoxianidcards"),

                        service_code = reader.GetValue<string>("service_code"),
                        service_usecount = reader.GetValue<int>("service_usecount"),
                        service_lastcount = reader.GetValue<int>("service_lastcount"),
                        yuyuepno = reader.GetValue<string>("yuyuepno"),
                        yuyueweek = reader.GetValue<int>("yuyueweek"),
                        givebaoxianorderid = reader.GetValue<int>("givebaoxianorderid"),
                        payserverpno = reader.GetValue<string>("payserverpno"),
                        serverid = reader.GetValue<string>("serverid"),

                        travelnames = reader.GetValue<string>("travelnames"),
                        travelidcards = reader.GetValue<string>("travelidcards"),
                        travelnations = reader.GetValue<string>("travelnations"),
                        travelphones = reader.GetValue<string>("travelphones"),
                        travelremarks = reader.GetValue<string>("travelremarks"),
                        isInterfaceSub = reader.GetValue<int>("isInterfaceSub"),
                        bookpro_bindcompany = reader.GetValue<string>("bookpro_bindcompany"),
                        bookpro_bindconfirmtime = reader.GetValue<DateTime>("bookpro_bindconfirmtime"),
                        bookpro_bindname = reader.GetValue<string>("bookpro_bindname"),
                        bookpro_bindphone = reader.GetValue<string>("bookpro_bindphone"),
                    };
                }
                return null;
            }
        }



        internal int GetOrderidbypno(string pno)
        {
            string sql = "select id from b2b_order where pno ='" + pno + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int r = 0;
                if (reader.Read())
                {
                    r = reader.GetValue<int>("id");
                }
                return r;
            }
        }

        internal IList<B2b_order> Getclientlistbyagentid(int Pageindex, int Pagesize, string Key, int Agentid, out int totalcount)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            string condition = "";
            if (Key != "")
            {
                condition = "u_phone!='' and u_name!='' and charindex('" + Key + "', u_phone)>0 or charindex('" + Key + "',u_name)>0 and  agentid=" + Agentid;
            }
            else
            {
                condition = "u_phone!='' and u_name!=''   and  agentid=" + Agentid;
            }


            cmd.PagingCommand1("b2b_order", "u_phone,u_name", "u_phone", "u_phone,u_name", Pagesize, Pageindex, "1", condition);
            IList<B2b_order> list = new List<B2b_order>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        U_name = reader.GetValue<string>("u_name"),
                        U_phone = reader.GetValue<string>("u_phone"),
                    });
                }


            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;

        }

        internal int GetShopcartidbyoid(int orderid)
        {
            string sql = "select   shopcartid  from b2b_order where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int shopcartid = 0;
                if (reader.Read())
                {
                    shopcartid = reader.GetValue<int>("shopcartid");
                }
                return shopcartid;
            }
        }

        internal int InsAskquitfee(int orderid, int quit_num, decimal quit_fee, string quit_Reason, string quit_info)
        {
            string sql = "update b2b_order set askquitfee=" + quit_fee + " ,askquitfeereason='" + quit_Reason + "' ,askquitfeeexplain='" + quit_info + "',askquitnum=" + quit_num + "  where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int GetOrderBookNum(int orderid)
        {
            string sql = "select u_num from b2b_order where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int r = 0;
                if (reader.Read())
                {
                    r = reader.GetValue<int>("u_num");
                }
                return r;
            }
        }
        internal int UporderRecommendMannelid(int crmid, int orderid)
        {
            try
            {
                string sql = "update b2b_order set recommendchannelid=(select IssueCard  from Member_Card where Cardcode=(select IDcard from b2b_crm where id=" + crmid + " )) where id=" + orderid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                return cmd.ExecuteNonQuery();
            }
            catch
            {
                return 0;
            }
        }
        internal int Getishaseticket(int orderid)
        {

            try
            {
                //判断订单是否含有绑定订单，含有绑定订单则求绑定订单的可用数量
                string sql = "select bindingagentorderid  from b2b_order where id=" + orderid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                if (o.ToString() == "0" || o.ToString().ToLower() == "null")
                { }
                else
                {
                    orderid = int.Parse(o.ToString());
                }

                string sql2 = "select count(1) as countnum from b2b_eticket where oid=" + orderid;

                cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                using (var reader = cmd.ExecuteReader())
                {
                    int r = 0;
                    if (reader.Read())
                    {
                        r = reader.GetValue<int>("countnum");
                    }

                    return r;
                }

            }
            catch
            {

                return 0;
            }
        }


        internal int GetevaluateidByoid(int oid, int evatype)
        {
            string sql = "select id from b2b_order_evaluate where oid=" + oid + " and evatype=" + evatype + "";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int id = 0;
                if (reader.Read())
                {
                    id = reader.GetValue<int>("id");
                }
                return id;
            }
        }


        internal int Insertevaluateid(int comid, int uid, int oid, int channelid, int annonymous, int star, int evatype, string area)
        {

            string sql = "insert b2b_order_evaluate (oid,uid,comid,channelid,anonymous,text,evatype,starnum) values(" + oid + "," + uid + "," + comid + "," + channelid + "," + annonymous + ",'" + area + "'," + evatype + "," + star + ")";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();

        }


        internal int updateevaluateid(int id, int annonymous, int star, string area)
        {
            string sql = "update b2b_order_evaluate set anonymous=" + annonymous + ",starnum=" + star + ",text= '" + area + "' where id=" + id + "";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();

        }


        internal List<B2b_order_evaluate> EvaluatePageList(int comid, int oid, int uid, int channelid, int evatype, int Pageindex, int Pagesize, out int totalcount)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            string condition = " comid =" + comid;
            if (oid != 0)//根据订单查
            {
                condition += " and oid=" + oid;
            }
            if (uid != 0)//根据会员查
            {
                condition += " and uid=" + uid;
            }

            if (channelid != 0)//根据教练查
            {
                condition += " and channelid=" + channelid;
            }

            if (evatype != 0)//查询类型，会员评价教练、教练评价会员
            {
                condition += " and evatype=" + evatype;
            }

            cmd.PagingCommand1("b2b_order_evaluate", "*", "id desc", "", Pagesize, Pageindex, "", condition);
            List<B2b_order_evaluate> list = new List<B2b_order_evaluate>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_order_evaluate
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        anonymous = reader.GetValue<int>("anonymous"),
                        channelid = reader.GetValue<int>("channelid"),
                        evatype = reader.GetValue<int>("evatype"),
                        oid = reader.GetValue<int>("oid"),
                        starnum = reader.GetValue<int>("starnum"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                        text = reader.GetValue<string>("text"),
                        uid = reader.GetValue<int>("uid")

                    });
                }


            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;

        }

        internal List<B2b_order_evaluate> EvaluatePageinfo(int id, int evatype)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            string condition = "oid= " + id + " and evatype=" + evatype;

            cmd.PagingCommand1("b2b_order_evaluate", "*", "id", "", 1, 1, "", condition);
            List<B2b_order_evaluate> list = new List<B2b_order_evaluate>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_order_evaluate
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        anonymous = reader.GetValue<int>("anonymous"),
                        channelid = reader.GetValue<int>("channelid"),
                        evatype = reader.GetValue<int>("evatype"),
                        oid = reader.GetValue<int>("oid"),
                        starnum = reader.GetValue<int>("starnum"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                        text = reader.GetValue<string>("text"),
                        uid = reader.GetValue<int>("uid")

                    });
                }
            }
            return list;
        }

        //查询是否有评价
        internal int Evaluateyesno(int oid)
        {
            string sql = "select id from b2b_order_evaluate where oid=" + oid + " and evatype=0";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int id = 0;
                if (reader.Read())
                {
                    id = reader.GetValue<int>("id");
                }
                return id;
            }
        }


        internal int InsertSmsback(int type, string mobile, string state, string con, string time)
        {
            string wstr = "insert sms_log (type,mobile,state,con,time) values(@type,@mobile,@state,@con,@time) ";

            var cmd = sqlHelper.PrepareTextSqlCommand(wstr);
            cmd.AddParam("@type", type);
            cmd.AddParam("@mobile", mobile);
            cmd.AddParam("@state", state);
            cmd.AddParam("@con", con);
            cmd.AddParam("@time", time);

            return cmd.ExecuteNonQuery();
        }


        internal int GetProIdbyorderid(int orderid)
        {
            string sql = "select pro_id from b2b_order where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int proid = 0;
                if (reader.Read())
                {
                    proid = reader.GetValue<int>("pro_id");
                }
                return proid;
            }
        }

        internal int UpOrderRemark(int orderid, string orderremark)
        {
            string sql = "update b2b_order set order_remark ='" + orderremark + "' where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int UpOrderStateAndRemark(int orderid, int orderstate, string orderremark)
        {
            string sql = "update b2b_order set order_state=" + orderstate + " , order_remark ='" + orderremark + "' where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int GetOrderidByServiceOrderNum(string serviceorder_num)
        {
            string sql = "select id from b2b_order where service_order_num='" + serviceorder_num + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int orderid = 0;
                if (reader.Read())
                {
                    return reader.GetValue<int>("id");
                }
                return orderid;
            }
        }

        internal int GetServiceLastcount(int orderid)
        {
            try
            {
                string sql = "select service_lastcount from b2b_order where id=" + orderid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    int r = 0;
                    if (reader.Read())
                    {
                        return reader.GetValue<int>("service_lastcount");
                    }
                    return r;
                }
            }
            catch
            {
                return 0;
            }
        }

        internal string GetService_code(int orderid)
        {
            try
            {
                string sql = "select service_code from b2b_order where id=" + orderid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    string r = "";
                    if (reader.Read())
                    {
                        return reader.GetValue<string>("service_code");
                    }
                    return r;
                }
            }
            catch
            {
                return "";
            }
        }

        // yuyueweek 1=周末,0=平日 2=不限制
        internal int GetOrderyuyuepnoshulaingBypno(string pno, int yuyueweek)
        {
            try
            {
                string sql = "select sum(u_num) as u_num from b2b_order where yuyuepno='" + pno + "' and order_state in (2,4,8,22)";

                if (yuyueweek == 1)
                {
                    sql = " and yuyueweek=1";
                }
                if (yuyueweek == 2)
                {
                    sql = " and yuyueweek=0";
                }

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    int r = 0;
                    if (reader.Read())
                    {
                        return reader.GetValue<int>("u_num");
                    }
                    return r;
                }
            }
            catch
            {
                return 0;
            }
        }


        internal int GedangriyuyueBypno(string pno, int proid, DateTime traveldate)
        {
            try
            {
                string sql = "select sum(u_num) as u_num from b2b_order where  pro_id=" + proid + " and yuyuepno='" + pno + "' and order_state in (2,4,8,22)and u_traveldate='" + traveldate + "'";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    int r = 0;
                    if (reader.Read())
                    {
                        return reader.GetValue<int>("u_num");
                    }
                    return r;
                }
            }
            catch
            {
                return 0;
            }
        }


        internal int UpGivebxorderid(int orderid, int bxorderid)
        {
            string sql = "update b2b_order set givebaoxianorderid=" + bxorderid + " where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }


        internal int Upyuyueweekid(int orderid, int yuyueweek)
        {
            string sql = "update b2b_order set yuyueweek=" + yuyueweek + " where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int GuanlianBxorder(int orderid, int givebaoxianorderid)
        {
            string sql = "update b2b_order set givebaoxianorderid=" + givebaoxianorderid + " where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int GetSamedayBaoxianOrderNum(string idcard, DateTime dateTime)
        {
            string sql = "select count(1) as cNum from b2b_order where baoxianidcards like '%" + idcard + "%' and convert(varchar(10),u_traveldate,120)='" + dateTime.ToString("yyyy-MM-dd") + "' and pro_id in (select id from b2b_com_pro where server_type=14) and order_state in (4)";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("cNum");
                }
                return 0;
            }
        }

        internal IList<b2b_order_busNamelist> GetTravelBusNamelist(int orderid)
        {
            string sql = "select * from b2b_order_busNamelist where orderid =" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                List<b2b_order_busNamelist> list = new List<b2b_order_busNamelist>();
                while (reader.Read())
                {
                    list.Add(new b2b_order_busNamelist
                    {
                        id = reader.GetValue<int>("Id"),
                        orderid = reader.GetValue<int>("orderid"),
                        agentid = reader.GetValue<int>("agentid"),
                        comid = reader.GetValue<int>("comid"),
                        IdCard = reader.GetValue<string>("IdCard"),
                        name = reader.GetValue<string>("name"),
                        Nation = reader.GetValue<string>("Nation"),
                        proid = reader.GetValue<int>("proid"),
                        travel_time = reader.GetValue<DateTime>("travel_time"),
                        yuding_name = reader.GetValue<string>("yuding_name"),
                        yuding_num = reader.GetValue<int>("yuding_num"),
                        yuding_phone = reader.GetValue<string>("yuding_phone"),
                        yuding_time = reader.GetValue<DateTime>("yuding_time"),
                        pickuppoint = reader.GetValue<string>("pickuppoint"),
                        dropoffpoint = reader.GetValue<string>("dropoffpoint"),
                        contactphone = reader.GetValue<string>("contactphone"),
                        contactremark = reader.GetValue<string>("contactremark"),
                        pinyin = reader.GetValue<string>("pinyin"),
                        address = reader.GetValue<string>("address"),
                        email = reader.GetValue<string>("email"),
                        postcode = reader.GetValue<string>("postcode"),
                        credentialsType = reader.GetValue<int>("credentialsType"),
                    });
                }
                return list;
            }
        }

        internal int Changetraveldate(int aorderid, string testpro, string traveldate, string oldtraveldate, out string message)
        {
            sqlHelper.BeginTrancation();
            try
            {
                //根据a订单得到b订单
                string sql2 = "select Bindingagentorderid from b2b_order where id=" + aorderid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                int borderid = 0;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        borderid = reader.GetValue<int>("Bindingagentorderid");
                    }
                }

                #region b 订单存在，修改b订单出游日期；修改b订单大巴附属表出游日期；修改b订单产品库存
                if (borderid > 0)
                {
                    //得到b订单预订数量
                    int order_number = 0;
                    int pro_id = 0;
                    string sql5 = "select u_num,pro_id from b2b_order where id=" + borderid;
                    cmd = sqlHelper.PrepareTextSqlCommand(sql5);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            pro_id = reader.GetValue<int>("pro_id");
                            order_number = reader.GetValue<int>("u_num");
                        }
                    }
                    //得到产品新出游当日的库存
                    string sql_kucun = "select emptynum  from B2b_com_LineGroupDate where lineid=" + pro_id + " and daydate='" + traveldate + "'";
                    cmd = sqlHelper.PrepareTextSqlCommand(sql_kucun);
                    int kucun = 0;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            kucun = reader.GetValue<int>("emptynum");
                        }
                    }
                    if (kucun <= order_number)
                    {
                        message = "库存不足，" + traveldate + "现有库存:" + kucun;
                        sqlHelper.Commit();
                        return 0;
                    }
                    else
                    {
                        //修改b订单出游日期修改
                        string sql31 = "update b2b_order set u_traveldate='" + traveldate + "' where id=" + borderid;
                        cmd = sqlHelper.PrepareTextSqlCommand(sql31);
                        cmd.ExecuteNonQuery();

                        //修改a订单出游日期修改
                        string sql3 = "update b2b_order set u_traveldate='" + traveldate + "' where id=" + aorderid;
                        cmd = sqlHelper.PrepareTextSqlCommand(sql3);
                        cmd.ExecuteNonQuery();

                        //修改b订单大巴附属表出游日期
                        string sql4 = "update b2b_order_busNamelist set travel_time='" + traveldate + "' where orderid=" + borderid;
                        cmd = sqlHelper.PrepareTextSqlCommand(sql4);
                        cmd.ExecuteNonQuery();



                        //b订单 产品库存修改 原日期库存增加；新日期库存减少
                        string sql6 = "update B2b_com_LineGroupDate set emptynum=emptynum+" + order_number + " where daydate='" + oldtraveldate + "' and lineid=" + pro_id;
                        cmd = sqlHelper.PrepareTextSqlCommand(sql6);
                        cmd.ExecuteNonQuery();

                        string sql7 = "update B2b_com_LineGroupDate set emptynum=emptynum-" + order_number + " where daydate='" + traveldate + "' and lineid=" + pro_id;
                        cmd = sqlHelper.PrepareTextSqlCommand(sql7);
                        cmd.ExecuteNonQuery();

                        string sql11 = "update b2b_order set order_remark=order_remark+'" + testpro + "' where id=" + aorderid;
                        cmd = sqlHelper.PrepareTextSqlCommand(sql11);
                        cmd.ExecuteNonQuery();

                        message = "改期成功";
                        sqlHelper.Commit();
                        return 1;
                    }
                }
                #endregion
                #region b订单不存在，修改a订单出游日期修改;修改a订单大巴附属表出游日期；修改a订单产品库存
                else
                {
                    //得到a订单预订数量
                    int order_number = 0;
                    int pro_id = 0;
                    string sql5 = "select u_num,pro_id from b2b_order where id=" + aorderid;
                    cmd = sqlHelper.PrepareTextSqlCommand(sql5);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            pro_id = reader.GetValue<int>("pro_id");
                            order_number = reader.GetValue<int>("u_num");
                        }
                    }
                    //得到产品新出游当日的库存
                    string sql_kucun = "select emptynum  from B2b_com_LineGroupDate where lineid=" + pro_id + " and daydate='" + traveldate + "'";
                    cmd = sqlHelper.PrepareTextSqlCommand(sql_kucun);
                    int kucun = 0;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            kucun = reader.GetValue<int>("emptynum");
                        }
                    }
                    if (kucun <= order_number)
                    {
                        message = "库存不足，" + traveldate + "现有库存:" + kucun;
                        sqlHelper.Commit();
                        return 0;
                    }
                    else
                    {
                        //修改a订单出游日期修改
                        string sql3 = "update b2b_order set u_traveldate='" + traveldate + "' where id=" + aorderid;
                        cmd = sqlHelper.PrepareTextSqlCommand(sql3);
                        cmd.ExecuteNonQuery();

                        //修改a订单大巴附属表出游日期
                        string sql4 = "update b2b_order_busNamelist set travel_time='" + traveldate + "' where orderid=" + aorderid;
                        cmd = sqlHelper.PrepareTextSqlCommand(sql4);
                        cmd.ExecuteNonQuery();



                        //a订单 产品库存修改 原日期库存增加；新日期库存减少
                        string sql6 = "update B2b_com_LineGroupDate set emptynum=emptynum+" + order_number + " where daydate='" + oldtraveldate + "' and lineid=" + pro_id;
                        cmd = sqlHelper.PrepareTextSqlCommand(sql6);
                        cmd.ExecuteNonQuery();

                        string sql7 = "update B2b_com_LineGroupDate set emptynum=emptynum-" + order_number + " where daydate='" + traveldate + "' and lineid=" + pro_id;
                        cmd = sqlHelper.PrepareTextSqlCommand(sql7);
                        cmd.ExecuteNonQuery();

                        string sql11 = "update b2b_order set order_remark=order_remark+'" + testpro + "' where id=" + aorderid;
                        cmd = sqlHelper.PrepareTextSqlCommand(sql11);
                        cmd.ExecuteNonQuery();

                        message = "改期成功";
                        sqlHelper.Commit();
                        return 1;
                    }
                }
                #endregion

            }
            catch (Exception err)
            {
                message = err.Message;
                sqlHelper.Rollback();
                return 0;
            }
            finally
            {
                sqlHelper.Dispose();
            }

        }

        internal int TagBusride(int orderbusrideid)
        {
            string sql = "update b2b_order_busNamelist set isaboard=1 where id=" + orderbusrideid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int Getgivebaoxinorderid(int orderid)
        {
            string sql = "select givebaoxianorderid from b2b_order where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int baoxinorderid = 0;
                if (reader.Read())
                {
                    baoxinorderid = reader.GetValue<int>("givebaoxianorderid");
                }
                return baoxinorderid;
            }
        }

        internal List<B2b_order> GetAgentOrderList(int comid, int agentid, string key, int order_state, string begindate, string enddate, int servertype)
        {
            var condition = "agentid=" + agentid;
            if (comid == 0)
            { }
            else
            {
                condition = condition + " and comid = " + comid;
            }
            if (key != "")
            {
                int key_int = 0;
                try
                {
                    key_int = int.Parse(key);
                }
                catch
                {
                    key_int = 0;
                }
                condition = condition + " and (U_name='" + key + "' or U_phone='" + key + "' or id=" + key_int + " or '" + key + "' in (pno) or pro_id in (select id from b2b_com_pro where pro_name like '%" + key + "%'))";
            }
            if (order_state != 0)
            {
                condition = condition + " and order_state=" + order_state;
            }
            if (begindate != "" && enddate != "")
            {
                begindate = DateTime.Parse(begindate).ToString("yyyy-MM-dd 00:00:00");
                enddate = DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59");
                condition = condition + " and u_subdate between '" + begindate + "' and '" + enddate + "'";
            }
            if (servertype != 0)
            {
                condition += " and pro_id in (select id from b2b_com_pro where server_type=" + servertype + ")";
            }
            string sql = "select * from b2b_order where " + condition;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<B2b_order> list = new List<B2b_order>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        U_traveldate = reader.GetValue<DateTime>("U_traveldate"),
                        Dealer = reader.GetValue<string>("dealer"),
                        Comid = reader.GetValue<int>("comid"),
                        Openid = reader.GetValue<string>("openid"),
                        Pno = reader.GetValue<string>("pno"),
                        Integral1 = reader.GetValue<decimal>("integral"),
                        Imprest1 = reader.GetValue<decimal>("imprest"),
                        Ticket = reader.GetValue<decimal>("Ticket"),
                        Ticketinfo = reader.GetValue<string>("ticketinfo") == null ? "" : reader.GetValue<string>("ticketinfo"),
                        Backtickettime = reader.GetValue<DateTime>("Backtickettime"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrantid = reader.GetValue<int>("Warrantid"),
                        Service_order_num = reader.GetValue<string>("Service_order_num"),
                        Service_req_seq = reader.GetValue<string>("Service_req_seq"),
                        Order_remark = reader.GetValue<string>("Order_remark"),
                        Servicepro_v_state = reader.GetValue<string>("Servicepro_v_state"),
                        Tocomid = reader.GetValue<int>("Tocomid"),
                        Buyuid = reader.GetValue<int>("Buyuid"),
                        Child_u_num = reader.GetValue<int>("child_u_num"),
                        Bindingagentorderid = reader.GetValue<int>("Bindingagentorderid"),
                        pickuppoint = reader.GetValue<string>("pickuppoint"),
                        dropoffpoint = reader.GetValue<string>("dropoffpoint"),
                        Cancelnum = reader.GetValue<int>("cancel_ticketnum"),

                        Province = reader.GetValue<string>("Province"),
                        City = reader.GetValue<string>("City"),
                        Address = reader.GetValue<string>("Address"),
                        Code = reader.GetValue<string>("Code"),
                        Express = reader.GetValue<decimal>("Expressfee"),
                        Expresscom = reader.GetValue<string>("Expresscom"),
                        Expresscode = reader.GetValue<string>("expresscode"),
                        Deliverytype = reader.GetValue<int>("Deliverytype"),
                        Shopcartid = reader.GetValue<int>("Shopcartid"),
                        childreduce = reader.GetValue<decimal>("childreduce"),
                        yanzheng_method = reader.GetValue<int>("yanzheng_method"),
                        Handlid = reader.GetValue<int>("Handlid"),
                        qunar_orderid = reader.GetValue<string>("qunar_orderid"),
                        askquitfee = reader.GetValue<decimal>("askquitfee"),
                        recommendchannelid = reader.GetValue<int>("recommendchannelid"),
                        Speciid = reader.GetValue<int>("Speciid"),
                        channelcoachid = reader.GetValue<int>("channelcoachid"),
                        baoxiannames = reader.GetValue<string>("baoxiannames"),
                        baoxianpinyinnames = reader.GetValue<string>("baoxianpinyinnames"),
                        baoxianidcards = reader.GetValue<string>("baoxianidcards"),

                        service_code = reader.GetValue<string>("service_code"),
                        service_usecount = reader.GetValue<int>("service_usecount"),
                        service_lastcount = reader.GetValue<int>("service_lastcount"),
                    });

                }
            }


            return list;
        }

        internal string GetPhoneByOrderid(int orderid)
        {
            string sql = "select u_phone from b2b_order where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            string phone = "";
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    phone = reader.GetValue<string>("u_phone");
                }
                return phone;
            }
        }

        internal int upprobindinfo(int orderid, string bindname, string bindcompany, string bindphone)
        {
            string sql = "update b2b_order set bookpro_bindname='" + bindname + "',bookpro_bindcompany='" + bindcompany + "',bookpro_bindphone='" + bindphone + "',bookpro_bindconfirmtime='" + DateTime.Now + "' where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }



        internal List<B2b_order> GetOrderList(int comid, int order_state, int servertype, string begindate, string enddate, string key, int userid, int orderIsAccurateToPerson, int channelcompanyid, int ordertype)
        {
            var condition = "comid = " + comid;

            //订单精确到渠道人
            if (orderIsAccurateToPerson == 1)
            {
                if (userid > 0)
                {
                    condition += " and recommendchannelid in (select id from member_channel where mobile=(select tel from b2b_company_manageuser where id=" + userid + "))";
                }
            }
            else
            {
                if (channelcompanyid > 0)
                {
                    condition += " and  recommendchannelid in (select id from member_channel where companyid=" + channelcompanyid + " ) ";
                }
            }

            if (ordertype == 1)
            {//直销订单
                condition = condition + " and Agentid =0";
            }

            if (ordertype == 2)
            {//分销后台订单
                condition = condition + " and Agentid !=0 and warrant_type=1 and Order_type=1";
            }
            if (ordertype == 3)
            {//分销倒码订单
                condition = condition + " and Agentid !=0 and warrant_type=2";
            }
            if (ordertype == 4)
            {//分销充值订单
                condition = condition + " and Agentid !=0 and Order_type=2 and warrant_type=1 ";
            }

            if (key != "")
            {
                //判断是否为数字，是数字的则可以查询订单号
                Regex regex = new Regex("^[0-9]*$");
                bool isNum = regex.IsMatch(key.Trim());
                if (isNum == false)
                {
                    condition = condition + " and (U_name='" + key + "' or U_phone='" + key + "' or id in (select oid from b2b_eticket where pno='" + key + "') or Bindingagentorderid in (select id from b2b_order where pno='" + key + "')  or pro_id in (select id from b2b_com_pro where com_id =" + comid + " and pro_name like '%" + key + "%'))";
                }
                else
                {
                    condition = condition + " and (U_name='" + key + "' or U_phone='" + key + "' or id=" + key + " or  id in (select oid from b2b_eticket where pno='" + key + "') or Bindingagentorderid in (select id from b2b_order where pno='" + key + "'))";
                }
            }
            if (order_state != 0)
            {
                condition = condition + " and order_state=" + order_state;
            }
            if (servertype != 0)
            {
                condition += " and pro_id in (select id from b2b_com_pro where server_type=" + servertype + ")";
            }
            if (begindate != "" && enddate != "")
            {
                begindate = DateTime.Parse(begindate).ToString("yyyy-MM-dd 00:00:00");
                enddate = DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59");
                condition = condition + " and u_subdate between '" + begindate + "' and '" + enddate + "'";
            }

            string sql = "select * from b2b_order where " + condition;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<B2b_order> list = new List<B2b_order>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_idcard=  reader.GetValue<string>("U_idcard"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        U_traveldate = reader.GetValue<DateTime>("U_traveldate"),
                        Dealer = reader.GetValue<string>("dealer"),
                        Comid = reader.GetValue<int>("comid"),
                        Openid = reader.GetValue<string>("openid"),
                        Pno = reader.GetValue<string>("pno"),
                        Integral1 = reader.GetValue<decimal>("integral"),
                        Imprest1 = reader.GetValue<decimal>("imprest"),
                        Ticket = reader.GetValue<decimal>("Ticket"),
                        Ticketinfo = reader.GetValue<string>("ticketinfo") == null ? "" : reader.GetValue<string>("ticketinfo"),
                        Backtickettime = reader.GetValue<DateTime>("Backtickettime"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrantid = reader.GetValue<int>("Warrantid"),
                        Service_order_num = reader.GetValue<string>("Service_order_num"),
                        Service_req_seq = reader.GetValue<string>("Service_req_seq"),
                        Order_remark = reader.GetValue<string>("Order_remark"),
                        Servicepro_v_state = reader.GetValue<string>("Servicepro_v_state"),
                        Tocomid = reader.GetValue<int>("Tocomid"),
                        Buyuid = reader.GetValue<int>("Buyuid"),
                        Child_u_num = reader.GetValue<int>("child_u_num"),
                        Bindingagentorderid = reader.GetValue<int>("Bindingagentorderid"),
                        pickuppoint = reader.GetValue<string>("pickuppoint"),
                        dropoffpoint = reader.GetValue<string>("dropoffpoint"),
                        Cancelnum = reader.GetValue<int>("cancel_ticketnum"),

                        Province = reader.GetValue<string>("Province"),
                        City = reader.GetValue<string>("City"),
                        Address = reader.GetValue<string>("Address"),
                        Code = reader.GetValue<string>("Code"),
                        Express = reader.GetValue<decimal>("Expressfee"),
                        Expresscom = reader.GetValue<string>("Expresscom"),
                        Expresscode = reader.GetValue<string>("expresscode"),
                        Deliverytype = reader.GetValue<int>("Deliverytype"),
                        Shopcartid = reader.GetValue<int>("Shopcartid"),
                        childreduce = reader.GetValue<decimal>("childreduce"),
                        yanzheng_method = reader.GetValue<int>("yanzheng_method"),
                        Handlid = reader.GetValue<int>("Handlid"),
                        qunar_orderid = reader.GetValue<string>("qunar_orderid"),
                        askquitfee = reader.GetValue<decimal>("askquitfee"),
                        recommendchannelid = reader.GetValue<int>("recommendchannelid"),
                        Speciid = reader.GetValue<int>("Speciid"),
                        channelcoachid = reader.GetValue<int>("channelcoachid"),
                        baoxiannames = reader.GetValue<string>("baoxiannames"),
                        baoxianpinyinnames = reader.GetValue<string>("baoxianpinyinnames"),
                        baoxianidcards = reader.GetValue<string>("baoxianidcards"),

                        service_code = reader.GetValue<string>("service_code"),
                        service_usecount = reader.GetValue<int>("service_usecount"),
                        service_lastcount = reader.GetValue<int>("service_lastcount"),
                    });

                }
            }
            return list;
        }


        internal int GetNeedVisitDateOrderPaysucNum(DateTime daydate, int servertype, int comid, int paystate, int agentid, string orderstate)
        {
            string sql = "select SUM(u_num)  from b2b_order where     pro_id in (select id from b2b_com_pro where server_type=" + servertype + "  and isSetVisitDate=1) and u_traveldate='" + daydate + "' and pay_state=" + paystate + " and comid=" + comid;
            if (agentid > 0)
            {
                sql = "select SUM(u_num)  from b2b_order where     agentid=" + agentid + "  and  pro_id in (select id from b2b_com_pro where server_type=" + servertype + "  and isSetVisitDate=1) and u_traveldate='" + daydate + "' and pay_state=" + paystate + " and comid=" + comid;
            }
            if (orderstate != "")
            {
                sql += " and charindex(',' + LTRIM(order_state) + ',', ',' + '" + orderstate + "' + ',') > 0";
            }
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        internal List<B2b_order> getNeedvisitdatePaysucorderlist(DateTime gooutdate, int proid, int paystate, string orderstate)
        {
            string sql = "select * from b2b_order where u_traveldate='" + gooutdate + "' and pro_id=" + proid + " and pay_state=" + paystate + " and order_state in (" + orderstate + ")";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<B2b_order> list = new List<B2b_order>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pro_id = reader.GetValue<int>("Pro_id"),
                        Order_type = reader.GetValue<int>("Order_type"),
                        U_name = reader.GetValue<string>("U_name"),
                        U_id = reader.GetValue<int>("U_id"),
                        U_phone = reader.GetValue<string>("U_phone"),
                        U_num = reader.GetValue<int>("U_num"),
                        U_subdate = reader.GetValue<DateTime>("U_subdate"),
                        Payid = reader.GetValue<int>("Payid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        Cost = reader.GetValue<decimal>("Cost"),
                        Profit = reader.GetValue<decimal>("Profit"),
                        Order_state = reader.GetValue<int>("Order_state"),
                        Pay_state = reader.GetValue<int>("Pay_state"),
                        Send_state = reader.GetValue<int>("Send_state"),
                        Ticketcode = reader.GetValue<int>("Ticketcode"),
                        Rebate = reader.GetValue<decimal>("Rebate"),
                        Ordercome = reader.GetValue<string>("Ordercome"),
                        U_traveldate = reader.GetValue<DateTime>("U_traveldate"),
                        Dealer = reader.GetValue<string>("dealer"),
                        Comid = reader.GetValue<int>("comid"),
                        Openid = reader.GetValue<string>("openid"),
                        Pno = reader.GetValue<string>("pno"),
                        Integral1 = reader.GetValue<decimal>("integral"),
                        Imprest1 = reader.GetValue<decimal>("imprest"),
                        Ticket = reader.GetValue<decimal>("Ticket"),
                        Ticketinfo = reader.GetValue<string>("ticketinfo") == null ? "" : reader.GetValue<string>("ticketinfo"),
                        Backtickettime = reader.GetValue<DateTime>("Backtickettime"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrantid = reader.GetValue<int>("Warrantid"),
                        Service_order_num = reader.GetValue<string>("Service_order_num"),
                        Service_req_seq = reader.GetValue<string>("Service_req_seq"),
                        Order_remark = reader.GetValue<string>("Order_remark"),
                        Servicepro_v_state = reader.GetValue<string>("Servicepro_v_state"),
                        Tocomid = reader.GetValue<int>("Tocomid"),
                        Buyuid = reader.GetValue<int>("Buyuid"),
                        Child_u_num = reader.GetValue<int>("child_u_num"),
                        Bindingagentorderid = reader.GetValue<int>("Bindingagentorderid"),
                        pickuppoint = reader.GetValue<string>("pickuppoint"),
                        dropoffpoint = reader.GetValue<string>("dropoffpoint"),
                        Cancelnum = reader.GetValue<int>("cancel_ticketnum"),

                        Province = reader.GetValue<string>("Province"),
                        City = reader.GetValue<string>("City"),
                        Address = reader.GetValue<string>("Address"),
                        Code = reader.GetValue<string>("Code"),
                        Express = reader.GetValue<decimal>("Expressfee"),
                        Expresscom = reader.GetValue<string>("Expresscom"),
                        Expresscode = reader.GetValue<string>("expresscode"),
                        Deliverytype = reader.GetValue<int>("Deliverytype"),
                        Shopcartid = reader.GetValue<int>("Shopcartid"),
                        childreduce = reader.GetValue<decimal>("childreduce"),
                        yanzheng_method = reader.GetValue<int>("yanzheng_method"),
                        Handlid = reader.GetValue<int>("Handlid"),
                        qunar_orderid = reader.GetValue<string>("qunar_orderid"),
                        askquitfee = reader.GetValue<decimal>("askquitfee"),
                        recommendchannelid = reader.GetValue<int>("recommendchannelid"),
                        Speciid = reader.GetValue<int>("Speciid"),
                        channelcoachid = reader.GetValue<int>("channelcoachid"),
                        baoxiannames = reader.GetValue<string>("baoxiannames"),
                        baoxianpinyinnames = reader.GetValue<string>("baoxianpinyinnames"),
                        baoxianidcards = reader.GetValue<string>("baoxianidcards"),

                        service_code = reader.GetValue<string>("service_code"),
                        service_usecount = reader.GetValue<int>("service_usecount"),
                        service_lastcount = reader.GetValue<int>("service_lastcount"),

                        bookpro_bindcompany = reader.GetValue<string>("bookpro_bindcompany"),
                        bookpro_bindconfirmtime = reader.GetValue<DateTime>("bookpro_bindconfirmtime"),
                        bookpro_bindname = reader.GetValue<string>("bookpro_bindname"),
                        bookpro_bindphone = reader.GetValue<string>("bookpro_bindphone"),
                        payorder = reader.GetValue<int>("payorder"),

                    });
                }

            }
            return list;
        }

        internal int HasFinOrder(int orderid)
        {
            string sql = "update b2b_order set order_state=" + (int)OrderStatus.HasFin + " where id=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal List<B2b_order> prosaleordercount(int comid, string begindate, string enddate, int projectid, int productid, string key, string orderstate, out int totalcount)
        {
            int count=0;
            var condition = "comid = " + comid;

            condition = condition + " and order_state in (" + orderstate+")";

            if (projectid != 0)
            {
                condition += " and pro_id in (select id from b2b_com_pro where Projectid=" + projectid + ")";
            }

            if (productid != 0)
            {
                condition += " and pro_id=" + productid;
            }

            if (begindate != "" && enddate != "")
            {
                begindate = DateTime.Parse(begindate).ToString("yyyy-MM-dd 00:00:00");
                enddate = DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59");
                condition = condition + " and u_subdate between '" + begindate + "' and '" + enddate + "'";
            }

            string sql = "select agentid,sum(u_num) as u_num,avg(pay_price) as pay_price from b2b_order where " + condition + " group by agentid order by sum(u_num) desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<B2b_order> list = new List<B2b_order>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_order
                    {
                        U_num = reader.GetValue<int>("U_num"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Pay_price = reader.GetValue<decimal>("Pay_price"),
                        
                    });
                    count=count+1;
                }
            }
            totalcount = count;
            return list;
        }

        internal int proyanzhengordercount(int comid, string begindate, string enddate, int projectid, int productid, int agentid)
        {
            var condition = "select id from b2b_order where comid = " + comid + " and agentid=" + agentid;

            if (projectid != 0)
            {
                condition += " and pro_id in (select id from b2b_com_pro where Projectid=" + projectid + ")";
            }

            if (productid != 0)
            {
                condition += " and pro_id=" + productid;
            }

            condition = "select id from b2b_eticket where oid in (" + condition + ")";

            //时间按 验票日志查询
            var condition_time = "";
            if (begindate != "" && enddate != "")
            {
                begindate = DateTime.Parse(begindate).ToString("yyyy-MM-dd 00:00:00");
                enddate = DateTime.Parse(enddate).ToString("yyyy-MM-dd 23:59:59");
                condition_time = " and actiondate between '" + begindate + "' and '" + enddate + "'";
            }

            string sql = "select sum(use_pnum) as use_pnum from b2b_etcket_log where action=1 and a_state=1 and  eticket_id in(" + condition + ") " + condition_time + " group by action";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<B2b_order> list = new List<B2b_order>();
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("use_pnum");
                }
            }
            return 0;
        } 

    }
}

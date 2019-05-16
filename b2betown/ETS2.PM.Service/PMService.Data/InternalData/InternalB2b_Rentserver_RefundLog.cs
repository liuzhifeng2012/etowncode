using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalB2b_Rentserver_RefundLog
    {
        public SqlHelper sqlHelper;
        public InternalB2b_Rentserver_RefundLog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int EditServer_Refundlog(B2b_Rentserver_RefundLog m)
        {
            if (m.id > 0)
            {
                string sql = @"UPDATE  [b2b_Rentserver_RefundLog]
                               SET [orderid] = @orderid
                                   ,proid=@proid
                                   ,proname=@proname
                                   ,comid=@comid
                                   ,comname=@comname
                                  ,[rentserverid] = @rentserverid 
                                  ,[rentservername] = @rentservername 
                                  ,[ordertotalfee] = @ordertotalfee 
                                  ,[refundfee] = @refundfee 
                                  ,[subtime] = @subtime 
                                  ,[pay_com] = @pay_com 
                                  ,[refundstate] = @refundstate 
                                  ,refundremark=@refundremark
                                  ,b2b_eticket_Depositid=@b2b_eticket_Depositid 
                             WHERE id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", m.id);
                cmd.AddParam("@orderid", m.orderid);
                cmd.AddParam("@rentserverid", m.rentserverid);
                cmd.AddParam("@rentservername", m.rentservername);
                cmd.AddParam("@ordertotalfee", m.ordertotalfee);
                cmd.AddParam("@refundfee", m.refundfee);
                cmd.AddParam("@subtime", m.subtime);
                cmd.AddParam("@pay_com", m.pay_com);
                cmd.AddParam("@refundstate", m.refundstate);
                cmd.AddParam("@refundremark", m.refundremark); 
                cmd.AddParam("@proid", m.proid);
                cmd.AddParam("@proname", m.proname);
                cmd.AddParam("@comid", m.comid);
                cmd.AddParam("@comname", m.comname);
                cmd.AddParam("@b2b_eticket_Depositid", m.b2b_eticket_Depositid);

                cmd.ExecuteNonQuery();
                return m.id;
            }
            else
            {
                string sql = @"INSERT  [b2b_Rentserver_RefundLog]
                                   ([orderid]
                                   ,proid
                                   ,proname
                                   ,comid
                                   ,comname
                                   ,[rentserverid]
                                   ,[rentservername]
                                   ,[ordertotalfee]
                                   ,[refundfee]
                                   ,[subtime]
                                   ,[pay_com]
                                   ,[refundstate]
                                  ,refundremark,b2b_eticket_Depositid)
                             VALUES
                                   (@orderid 
                                   ,@proid
                                   ,@proname
                                   ,@comid
                                   ,@comname
                                   ,@rentserverid 
                                   ,@rentservername 
                                   ,@ordertotalfee 
                                   ,@refundfee 
                                   ,@subtime 
                                   ,@pay_com 
                                   ,@refundstate,@refundremark,@b2b_eticket_Depositid);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@orderid", m.orderid);
                cmd.AddParam("@rentserverid", m.rentserverid);
                cmd.AddParam("@rentservername", m.rentservername);
                cmd.AddParam("@ordertotalfee", m.ordertotalfee);
                cmd.AddParam("@refundfee", m.refundfee);
                cmd.AddParam("@subtime", m.subtime);
                cmd.AddParam("@pay_com", m.pay_com);
                cmd.AddParam("@refundstate", m.refundstate);
                cmd.AddParam("@refundremark", m.refundremark);
                cmd.AddParam("@proid", m.proid);
                cmd.AddParam("@proname", m.proname);
                cmd.AddParam("@comid", m.comid);
                cmd.AddParam("@comname", m.comname);
                cmd.AddParam("@b2b_eticket_Depositid", m.b2b_eticket_Depositid);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
        }

        internal B2b_Rentserver_RefundLog GetServerRefundlog(int orderid, int rentserverid, int b2b_eticket_Depositid, int refundstate)
        {
            string sql = "select * from b2b_Rentserver_RefundLog where orderid=" + orderid + " and rentserverid=" + rentserverid + " and b2b_eticket_Depositid=" + b2b_eticket_Depositid + " and refundstate=" + refundstate;
            if (refundstate == -1)
            {
                sql = "select * from b2b_Rentserver_RefundLog where orderid=" + orderid + " and rentserverid=" + rentserverid + " and b2b_eticket_Depositid=" + b2b_eticket_Depositid;
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                B2b_Rentserver_RefundLog m = null;
                if (reader.Read())
                {
                    m = new B2b_Rentserver_RefundLog
                    {
                        id = reader.GetValue<int>("id"),
                        orderid = reader.GetValue<int>("orderid"),
                        rentserverid = reader.GetValue<int>("rentserverid"),
                        rentservername = reader.GetValue<string>("rentservername"),
                        ordertotalfee = reader.GetValue<decimal>("ordertotalfee"),
                        refundfee = reader.GetValue<decimal>("refundfee"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                        pay_com = reader.GetValue<string>("pay_com"),
                        refundstate = reader.GetValue<int>("refundstate"),
                        refundremark = reader.GetValue<string>("refundremark"),
                        proid = reader.GetValue<int>("proid"),
                        proname = reader.GetValue<string>("proname"),
                        comid = reader.GetValue<int>("comid"),
                        comname = reader.GetValue<string>("comname"),
                        b2b_eticket_Depositid = reader.GetValue<int>("b2b_eticket_Depositid"),
                    };
                }
                return m;
            }
        }

        internal List<B2b_Rentserver_RefundLog> GetyajinrefundLoglist(int pageindex, int pagesize, string key, int refundstate, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "";
            if (refundstate != -1)
            {
                condition += "refundstate=" + refundstate;
            }
            if (key != "")
            {
                if (CommonFunc.IsNumber(key))
                {
                    if (CommonFunc.IsMobile(key))
                    {
                        condition += " and orderid in (select id from b2b_order where u_phone='" + key + "')";
                    }
                    else
                    {
                        condition += " and orderid=" + key;
                    }
                }
                else
                {
                    condition += " and orderid in (select id from b2b_order where u_name like '%" + key + "%')";
                }
            }
            cmd.PagingCommand1("b2b_Rentserver_RefundLog", "*", "id desc", "", pagesize, pageindex, "0", condition);
            List<B2b_Rentserver_RefundLog> list = new List<B2b_Rentserver_RefundLog>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_Rentserver_RefundLog
                    {
                        id = reader.GetValue<int>("id"),
                        orderid = reader.GetValue<int>("orderid"),
                        rentserverid = reader.GetValue<int>("rentserverid"),
                        rentservername = reader.GetValue<string>("rentservername"),
                        ordertotalfee = reader.GetValue<decimal>("ordertotalfee"),
                        refundfee = reader.GetValue<decimal>("refundfee"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                        pay_com = reader.GetValue<string>("pay_com"),
                        refundstate = reader.GetValue<int>("refundstate"),
                        refundremark = reader.GetValue<string>("refundremark"),
                        proid = reader.GetValue<int>("proid"),
                        proname = reader.GetValue<string>("proname"),
                        comid = reader.GetValue<int>("comid"),
                        comname = reader.GetValue<string>("comname"),
                        b2b_eticket_Depositid = reader.GetValue<int>("b2b_eticket_Depositid"),
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }

        internal int Upb2b_Rentserver_RefundLogState(int logid, int refundstate)
        {
            string sql = "update b2b_Rentserver_RefundLog set refundstate=" + refundstate + " where id=" + logid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Modle.Enum;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalB2b_com_LineGroupDate
    {
        private SqlHelper sqlHelper;
        public InternalB2b_com_LineGroupDate(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal List<B2b_com_LineGroupDate> GetLineGroupDateByLineid(int lineid, string isvalid = "0,1", int servertype = 0)
        {
            string sql = @"SELECT *
  FROM  [B2b_com_LineGroupDate] where lineid=@lineid";
            if (isvalid != "0,1")
            {
                if (isvalid == "0")
                {
                    sql += " and  CONVERT(varchar(10),daydate,120)<convert(varchar(10),getdate(),120)";
                }
                if (isvalid == "1")
                {
                    sql += " and  CONVERT(varchar(10),daydate,120)>=convert(varchar(10),getdate(),120)";
                }

            }

            if (servertype != 0)
            {
                //如果为旅游大巴 或者 跟团游当地游，得到可以预订的最小日期(当天以后还没有截团的日期)
                if (servertype == 2 || servertype == 8)
                {
                    sql += " and emptynum>0  and CONVERT(varchar(10),daydate,120)>CONVERT(varchar(10),GETDATE(),120)";
                }
                if (servertype == 10)
                {
                    sql += " and emptynum>0 ";
                }
            }


            sql += " order by daydate ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@lineid", lineid);

            List<B2b_com_LineGroupDate> list = new List<B2b_com_LineGroupDate>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_com_LineGroupDate
                    {
                        Id = reader.GetValue<int>("id"),
                        Menprice = reader.GetValue<decimal>("menprice"),
                        Childprice = reader.GetValue<decimal>("childprice"),
                        Oldmenprice = reader.GetValue<decimal>("oldmenprice"),
                        Emptynum = reader.GetValue<int>("emptynum"),
                        Daydate = reader.GetValue<DateTime>("daydate"),
                        Lineid = reader.GetValue<int>("lineid"),
                        agent1_back = reader.GetValue<decimal>("agent1_back"),
                        agent2_back = reader.GetValue<decimal>("agent2_back"),
                        agent3_back = reader.GetValue<decimal>("agent3_back"),
                    });
                }
                return list;
            }


        }

        internal B2b_com_LineGroupDate GetLineDayGroupDate(DateTime daydate, int lineid)
        {
            string sql = @"SELECT  * 
  FROM  [B2b_com_LineGroupDate] where lineid=@lineid and daydate=@daydate";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@lineid", lineid);
            cmd.AddParam("@daydate", daydate);



            B2b_com_LineGroupDate u = null;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    u = new B2b_com_LineGroupDate
                    {
                        Id = reader.GetValue<int>("id"),
                        Menprice = reader.GetValue<decimal>("menprice"),
                        Childprice = reader.GetValue<decimal>("childprice"),
                        Oldmenprice = reader.GetValue<decimal>("oldmenprice"),
                        Emptynum = reader.GetValue<int>("emptynum"),
                        Daydate = reader.GetValue<DateTime>("daydate"),
                        Lineid = reader.GetValue<int>("lineid"),
                        agent1_back = reader.GetValue<decimal>("agent1_back"),
                        agent2_back = reader.GetValue<decimal>("agent2_back"),
                        agent3_back = reader.GetValue<decimal>("agent3_back"),
                    };
                }

            }

            if (u != null)
            {
                //支付成功人数
                u.paysucnum = new B2bOrderData().GetPaySucNumByProid(lineid, 0, (int)PayStatus.HasPay, daydate, 0, "2,4,8,22");
                //待支付人数
                u.waitpaynum = new B2bOrderData().GetPaySucNumByProid(lineid, 0, (int)PayStatus.NotPay, daydate, 0, "1");
                return u;
            }
            else
            {
                return null;
            }
        }

        internal int EditLineGroupDate(B2b_com_LineGroupDate model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand("usp_InsertOrUpdateB2bcomLineGroupDate");

            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Menprice", model.Menprice);
            cmd.AddParam("@Childprice", model.Childprice);
            cmd.AddParam("@Oldmenprice", model.Oldmenprice);
            cmd.AddParam("@Lineid", model.Lineid);
            cmd.AddParam("@Emptynum", model.Emptynum);
            cmd.AddParam("@Daydate", model.Daydate);
            cmd.AddParam("@agent1_back", model.agent1_back);
            cmd.AddParam("@agent2_back", model.agent2_back);
            cmd.AddParam("@agent3_back", model.agent3_back);


            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }

        internal int DelLineGroupDate(int lineid, string daydate)
        {
            string sql = " delete [B2b_com_LineGroupDate] where lineid=@lineid and daydate=@daydate";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@lineid", lineid);
            cmd.AddParam("@daydate", daydate);

            return cmd.ExecuteNonQuery();
        }


        internal string GetMinPrice(int fangxingid, string startdata = "", string enddata = "")
        {
            string sql = "select min(menprice) as minprice from B2b_com_LineGroupDate  where lineid=" + fangxingid;
            if (startdata != "" && enddata != "")
            {
                sql += " and daydate between '" + startdata + "' and '" + enddata + "'";
            }
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                if (o == null || o.ToString() == "")
                {
                    return "0";
                }
                else
                {
                    return o.ToString();
                }

            }
            catch (Exception e)
            {
                sqlHelper.Dispose();
                return "0";
            }
        }

        internal string IsCanBook(int fangxingid, string startdate = "", string enddate = "")
        {
            string sql = "select min(emptynum) as minemptynum from B2b_com_LineGroupDate  where lineid=" + fangxingid;
            if (startdate != "" && enddate != "")
            {
                sql += " and daydate between '" + startdate + "' and '" + enddate + "'";
            }
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                if (o == null || o.ToString() == "")
                {
                    return "0";
                }
                else
                {
                    if (int.Parse(o.ToString()) > 0)
                    {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                }

            }
            catch (Exception e)
            {
                sqlHelper.Dispose();
                return "0";
            }
        }

        internal List<B2b_com_LineGroupDate> GetHouseTypeDayList(int proid, string startdate, string enddate)
        {
            //查询是否为导入产品，如果是导入产品读取主产品的id，读取主产proid品日历
            string sql_f = "select * from B2b_com_pro  where id=" + proid;
            var cmd_f = sqlHelper.PrepareTextSqlCommand(sql_f);
            using (var reader = cmd_f.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (reader.GetValue<int>("bindingid") != 0)
                    {
                        proid = reader.GetValue<int>("bindingid");
                    }
                }
            }

            string sql = @"SELECT *
  FROM  B2b_com_LineGroupDate  where lineid=@proid and daydate>= @startdate and daydate<@enddate";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@proid", proid);
            cmd.AddParam("@startdate", startdate);
            cmd.AddParam("@enddate", enddate);

            List<B2b_com_LineGroupDate> list = new List<B2b_com_LineGroupDate>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_com_LineGroupDate()
                    {
                        Id = reader.GetValue<int>("id"),
                        Menprice = reader.GetValue<decimal>("menprice"),
                        Childprice = reader.GetValue<decimal>("childprice"),
                        Oldmenprice = reader.GetValue<decimal>("oldmenprice"),
                        Emptynum = reader.GetValue<int>("emptynum"),
                        Daydate = reader.GetValue<DateTime>("daydate"),
                        Lineid = reader.GetValue<int>("lineid"),
                        agent1_back = reader.GetValue<decimal>("agent1_back"),
                        agent2_back = reader.GetValue<decimal>("agent2_back"),
                        agent3_back = reader.GetValue<decimal>("agent3_back"),
                    });
                }
            }
            return list;

        }

        internal string GetNowdayPrice(int fangxingid, string startdate)
        {
            //查询是否为导入产品，如果是导入产品读取主产品的id，读取主产品日历
            string sql_f = "select * from B2b_com_pro  where id=" + fangxingid;
            var cmd_f = sqlHelper.PrepareTextSqlCommand(sql_f);
            using (var reader = cmd_f.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (reader.GetValue<int>("bindingid") != 0)
                    {
                        fangxingid = reader.GetValue<int>("bindingid");
                    }
                }
            }





            string sql = "select menprice from B2b_com_LineGroupDate  where lineid=" + fangxingid;

            sql += " and daydate = '" + startdate + "'";

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                if (o == null || o.ToString() == "")
                {
                    return "0";
                }
                else
                {
                    return o.ToString();
                }
            }
            catch (Exception e)
            {
                sqlHelper.Dispose();
                return "0";
            }
        }

        internal string GetNowdayavailablenum(int fangxingid, string startdate)
        {
            string sql = "select emptynum from B2b_com_LineGroupDate  where lineid=" + fangxingid;

            sql += " and daydate = '" + startdate + "'";

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                if (o == null)
                {
                    return "0";
                }
                else
                {
                    if (o.ToString() == "")
                    {
                        return "0";
                    }
                    else
                    {
                        return o.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                return "0";
            }
        }

        internal List<B2b_com_LineGroupDate> GetLineDayGroupDate(string checkindate, string checkoutdate, int proid)
        {
            //查询是否为导入产品，如果是导入产品读取主产品的id，读取主产品日历
            string sql_f = "select * from B2b_com_pro  where id=" + proid;
            var cmd_f = sqlHelper.PrepareTextSqlCommand(sql_f);
            using (var reader = cmd_f.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (reader.GetValue<int>("bindingid") != 0)
                    {
                        proid = reader.GetValue<int>("bindingid");
                    }
                }
            }

            string sql = "SELECT  [id],[menprice],[childprice] ,[oldmenprice],[emptynum],[daydate],[lineid] FROM [EtownDB].[dbo].[B2b_com_LineGroupDate] where daydate>='" + checkindate + "' and daydate<'" + checkoutdate + "' and lineid=" + proid;
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);

                List<B2b_com_LineGroupDate> list = new List<B2b_com_LineGroupDate>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new B2b_com_LineGroupDate()
                        {
                            Id = reader.GetValue<int>("id"),
                            Menprice = reader.GetValue<decimal>("menprice"),
                            Childprice = reader.GetValue<decimal>("childprice"),
                            Oldmenprice = reader.GetValue<decimal>("oldmenprice"),
                            Emptynum = reader.GetValue<int>("emptynum"),
                            Daydate = reader.GetValue<DateTime>("daydate"),
                            Lineid = reader.GetValue<int>("lineid")
                        });
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        internal int ReduceEmptyNum(int proid, int booknum, DateTime start_date)
        {

            string sql = "update B2b_com_LineGroupDate set  emptynum=emptynum-" + booknum + "  where lineid=" + proid + " and daydate='" + start_date + "'";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();

        }

        internal int ReduceEmptyNum(int proid, int booknum, DateTime start_date, DateTime enddate)
        {
            string sql = "update B2b_com_LineGroupDate set  emptynum=emptynum-" + booknum + "  where lineid=" + proid + " and daydate>='" + start_date + "' and daydate<'" + enddate + "'";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();

        }

        internal int GetEmptyNum(int proid, DateTime daydate)
        {
            string sql = "select emptynum from B2b_com_LineGroupDate where lineid=" + proid + " and daydate='" + daydate + "'";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                if (o == null)
                {
                    return 0;
                }
                else
                {
                    if (o.ToString() == "")
                    {
                        return 0;
                    }
                    else
                    {
                        return int.Parse(o.ToString());
                    }
                }
            }
            catch
            {
                return 0;
            }
        }



        internal int CleanEmptyNum(int proid, DateTime daydate, int userid = 0, int comid = 0)
        {

            try
            {

                //注销所有未支付的订单
                string sql2 = "update b2b_order set order_state=23 WHERE pro_id=" + proid + " and u_traveldate='" + daydate + "' and order_state=1";
                var cmd2 = sqlHelper.PrepareTextSqlCommand(sql2);
                cmd2.ExecuteNonQuery();

                //对已支付订单标记为已处理
                string sql3 = "update b2b_order set order_state=22 WHERE pro_id=" + proid + " and u_traveldate='" + daydate + "' and (order_state=4 or (order_state=2 and pay_state=2))";
                var cmd3 = sqlHelper.PrepareTextSqlCommand(sql3);
                cmd3.ExecuteNonQuery();


                //清空控位
                string sql = "update B2b_com_LineGroupDate set  emptynum=0 WHERE lineid=" + proid + " and daydate='" + daydate + "'";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.ExecuteNonQuery();

                //截单记录表 添加结单记录
                sql = "select count(1) from travelbusorder_operlog where proid=" + proid + " and traveldate='" + daydate + "'";
                cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                int r = o == null ? 0 : int.Parse(o.ToString());
                if (r == 0)
                {
                    sql = "insert into travelbusorder_operlog (proid,proname,traveldate,opertime,operremark,opertor,comid) values " +
                        " (" + proid + ",'','" + daydate + "','" + DateTime.Now + "',''," + userid + "," + comid + ")";
                    cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    cmd.ExecuteNonQuery();
                }

                return 1;
            }
            catch
            {
                return 0;
            }
        }
        /*得到一段时间内最小空房数量*/
        internal int GetMinEmptyNum(int proid, DateTime start_date, DateTime end_date)
        {
            //查询是否为导入产品，如果是导入产品读取主产品的id，读取主产品日历
            string sql_f = "select * from B2b_com_pro  where id=" + proid;
            var cmd_f = sqlHelper.PrepareTextSqlCommand(sql_f);
            using (var reader = cmd_f.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (reader.GetValue<int>("bindingid") != 0)
                    {
                        proid = reader.GetValue<int>("bindingid");
                    }
                }
            }


            var r = new B2b_com_LineGroupDateData().GetHouseTypeDayList(proid, start_date.ToString("yyyy-MM-dd"), end_date.ToString("yyyy-MM-dd"));

            if (r.Count == 0)
            {
                return 0;
            }
            else
            {
                //判断是否有房型已满房
                string errmsg = "";
                foreach (B2b_com_LineGroupDate m in r)
                {
                    if (m.Emptynum == 0)
                    {
                        errmsg = m.Daydate.ToString("yyyy-MM-dd") + "已满房";
                        break;
                    }
                }


                if (errmsg == "")
                {
                    //判断是否查询超出了房态范围
                    System.TimeSpan ND = end_date - start_date;
                    int n = ND.Days;   //天数差 
                    if (r.Count < n)
                    {
                        errmsg = end_date.AddDays(r.Count - n).ToString("yyyy-MM-dd") + "房已满";
                        return 0;
                    }
                    else
                    {
                        string sql = "select min(emptynum) as minemptynum from B2b_com_LineGroupDate where lineid=" + proid + " and daydate>='" + start_date + "' and daydate<'" + end_date + "' ";

                        try
                        {
                            var cmd = sqlHelper.PrepareTextSqlCommand(sql);


                            int minm = 0;//最小空位数量
                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    minm = reader.GetValue<int>("minemptynum");
                                }
                            }
                            return minm;
                        }
                        catch
                        {
                            return 0;
                        }
                    }
                }
                else
                {
                    return 0;
                }


            }
        }


        /*得到1个人的价格*/
        internal decimal Gethotelallprice(int proid, DateTime start_date, DateTime end_date, int Agentlevel)
        {

            //产品基本信息中的分销返还
            decimal basic_agentback1 = 0;
            decimal basic_agentback2 = 0;
            decimal basic_agentback3 = 0;
            //查询是否为导入产品，如果是导入产品读取主产品的id，读取主产品日历
            string sql_f = "select * from B2b_com_pro  where id=" + proid;
            var cmd_f = sqlHelper.PrepareTextSqlCommand(sql_f);
            using (var reader = cmd_f.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (reader.GetValue<int>("bindingid") != 0)
                    {
                        proid = reader.GetValue<int>("bindingid");
                    }
                    basic_agentback1 = reader.GetValue<decimal>("agent1_price");
                    basic_agentback2 = reader.GetValue<decimal>("agent2_price");
                    basic_agentback3 = reader.GetValue<decimal>("agent3_price");
                }
            }


            var r = new B2b_com_LineGroupDateData().GetHouseTypeDayList(proid, start_date.ToString("yyyy-MM-dd"), end_date.ToString("yyyy-MM-dd"));

            if (r.Count == 0)
            {
                return 0;
            }
            else
            {
                //判断是否有房型已满房
                string errmsg = "";

                if (errmsg == "")
                {
                    //判断是否查询超出了房态范围
                    System.TimeSpan ND = end_date - start_date;
                    int n = ND.Days;   //天数差 
                    if (r.Count < n)
                    {
                        errmsg = end_date.AddDays(r.Count - n).ToString("yyyy-MM-dd") + "房已满";
                        return 0;
                    }
                    else
                    {
                        //得到产品团期中的分销返还 和 产品基本信息中的分销返还：产品团期没有设置分销返还，则用基本信息中的分销返还
                        if (r[0].agent1_back == 0 && r[0].agent2_back == 0 && r[0].agent3_back == 0)
                        {
                            decimal agent_back = 0;//分销返还
                            if (Agentlevel == 1)
                            {
                                agent_back = basic_agentback1;
                            }
                            if (Agentlevel == 2)
                            {
                                agent_back = basic_agentback2;
                            }
                            if (Agentlevel == 3)
                            {
                                agent_back = basic_agentback3;
                            }

                            string sql = "select sum(Menprice) as Menprice  from B2b_com_LineGroupDate where lineid=" + proid + " and daydate>='" + start_date + "' and daydate<'" + end_date + "' ";

                            try
                            {
                                var cmd = sqlHelper.PrepareTextSqlCommand(sql);


                                decimal Menprice = 0;//总价格 
                                using (var reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        Menprice = reader.GetValue<decimal>("Menprice");
                                    }
                                }
                                return Menprice - agent_back * n;
                            }
                            catch
                            {
                                return 0;
                            }
                        }
                        else
                        {
                            string sql = "select sum(Menprice) as Menprice  from B2b_com_LineGroupDate where lineid=" + proid + " and daydate>='" + start_date + "' and daydate<'" + end_date + "' ";
                            if (Agentlevel == 1)
                            {
                                sql = "select sum(Menprice) as Menprice,sum(agent1_back) as backmoney from B2b_com_LineGroupDate where lineid=" + proid + " and daydate>='" + start_date + "' and daydate<'" + end_date + "' ";
                            }
                            if (Agentlevel == 2)
                            {
                                sql = "select sum(Menprice) as Menprice,sum(agent2_back) as backmoney from B2b_com_LineGroupDate where lineid=" + proid + " and daydate>='" + start_date + "' and daydate<'" + end_date + "' ";
                            }
                            if (Agentlevel == 3)
                            {
                                sql = "select sum(Menprice) as Menprice,sum(agent3_back) as backmoney from B2b_com_LineGroupDate where lineid=" + proid + " and daydate>='" + start_date + "' and daydate<'" + end_date + "' ";
                            }

                            try
                            {
                                var cmd = sqlHelper.PrepareTextSqlCommand(sql);


                                decimal Menprice = 0;//总价格
                                decimal backmoney = 0;//总减免
                                using (var reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        Menprice = reader.GetValue<decimal>("Menprice");
                                        try
                                        {
                                            backmoney = reader.GetValue<decimal>("backmoney");
                                        }
                                        catch { }
                                    }
                                }
                                return Menprice - backmoney;
                            }
                            catch
                            {
                                return 0;
                            }


                        }

                    }
                }
                else
                {
                    return 0;
                }


            }
        }


        internal B2b_com_LineGroupDate GetMinValidByProjectid(int projectid, int comid)
        {
            string sql = @"SELECT top 1 [id]
      ,[menprice]
      ,[childprice]
      ,[oldmenprice]
      ,[emptynum]
      ,[daydate]
      ,[lineid]
  FROM [EtownDB].[dbo].[B2b_com_LineGroupDate] where lineid in (select id from b2b_com_pro where projectid=@projectid and com_id=@comid) and daydate>=@nowdate  order by daydate";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@projectid", projectid);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@nowdate", DateTime.Now.ToString("yyyy-MM-dd"));


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_com_LineGroupDate
                    {
                        Id = reader.GetValue<int>("id"),
                        Menprice = reader.GetValue<decimal>("menprice"),
                        Childprice = reader.GetValue<decimal>("childprice"),
                        Oldmenprice = reader.GetValue<decimal>("oldmenprice"),
                        Emptynum = reader.GetValue<int>("emptynum"),
                        Daydate = reader.GetValue<DateTime>("daydate"),
                        Lineid = reader.GetValue<int>("lineid")
                    };
                }

            }
            return null;
        }


        internal B2b_com_LineGroupDate GetMinValidByProid(int proid, int comid)
        {

            //查询是否为导入产品，如果是导入产品读取主产品的id，读取主产品日历
            string sql_f = "select * from B2b_com_pro  where id=" + proid;
            var cmd_f = sqlHelper.PrepareTextSqlCommand(sql_f);
            using (var reader = cmd_f.ExecuteReader())
            {
                if (reader.Read())
                {
                    if (reader.GetValue<int>("bindingid") != 0)
                    {
                        proid = reader.GetValue<int>("bindingid");
                    }
                }
            }



            string sql = @"SELECT top 1 [id]
      ,[menprice]
      ,[childprice]
      ,[oldmenprice]
      ,[emptynum]
      ,[daydate]
      ,[lineid]
  FROM [EtownDB].[dbo].[B2b_com_LineGroupDate] where lineid =@proid and daydate>=@nowdate  order by daydate";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@proid", proid);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@nowdate", DateTime.Now.ToString("yyyy-MM-dd"));


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_com_LineGroupDate
                    {
                        Id = reader.GetValue<int>("id"),
                        Menprice = reader.GetValue<decimal>("menprice"),
                        Childprice = reader.GetValue<decimal>("childprice"),
                        Oldmenprice = reader.GetValue<decimal>("oldmenprice"),
                        Emptynum = reader.GetValue<int>("emptynum"),
                        Daydate = reader.GetValue<DateTime>("daydate"),
                        Lineid = reader.GetValue<int>("lineid")
                    };
                }

            }
            return null;
        }

        internal int UpGroupdateprice(int proid, decimal price)
        {
            string sql = "update B2b_com_LineGroupDate set menprice=@price ,childprice=@price,oldmenprice=@price where lineid=@proid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@price", price);
            cmd.AddParam("@proid", proid);

            return cmd.ExecuteNonQuery();
        }

        internal int Rollbackemptynum(int proid, DateTime dateTime, int unum)
        {
            string sql = "update B2b_com_LineGroupDate set emptynum=emptynum+" + unum + "  where lineid=" + proid + " and daydate='" + dateTime + "'";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            return cmd.ExecuteNonQuery();
        }

        internal decimal GetMinValidePrice(int proid)
        {
            string sql = "select min(menprice) from B2b_com_LineGroupDate where lineid=" + proid + " and convert(varchar(10),daydate,120)>convert(varchar(10),getdate(),120) and emptynum>0 and menprice>0";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return decimal.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal List<B2b_com_LineGroupDate> GetLineGroupDate(int proid, DateTime startTime, DateTime endTime)
        {
            string sql = @"SELECT [id]
      ,[menprice]
      ,[childprice]
      ,[oldmenprice]
      ,[emptynum]
      ,[daydate]
      ,[lineid]
  FROM [B2b_com_LineGroupDate] where lineid=@lineid and  CONVERT(varchar(10),daydate,120)>=@startTime and  CONVERT(varchar(10),daydate,120)<=@endTime";



            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@lineid", proid);
            cmd.AddParam("@startTime", startTime.ToString("yyyy-MM-dd"));
            cmd.AddParam("@endTime", endTime.ToString("yyyy-MM-dd"));

            List<B2b_com_LineGroupDate> list = new List<B2b_com_LineGroupDate>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_com_LineGroupDate
                    {
                        Id = reader.GetValue<int>("id"),
                        Menprice = reader.GetValue<decimal>("menprice"),
                        Childprice = reader.GetValue<decimal>("childprice"),
                        Oldmenprice = reader.GetValue<decimal>("oldmenprice"),
                        Emptynum = reader.GetValue<int>("emptynum"),
                        Daydate = reader.GetValue<DateTime>("daydate"),
                        Lineid = reader.GetValue<int>("lineid")
                    });
                }
                return list;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalb2b_order_bespeak
    {
        public SqlHelper sqlHelper;
        public Internalb2b_order_bespeak(SqlHelper sqlhelper)
        {
            this.sqlHelper = sqlhelper;
        }

        internal int Subbespeak(Modle.B2b_order_bespeak m)
        {
            string sql = @"INSERT INTO b2b_order_bespeak 
           ([bespeakname]
           ,[phone]
           ,[idcard]
           ,[bespeakdate]
           ,[beaspeaknum]
           ,[beaspeaktype]
           ,[beaspeakstate]
           ,[remark]
           ,[subtime]
           ,[pno]
           ,[orderid]
           ,[proid]
           ,[proname]
           ,[comid])
     VALUES
           (@bespeakname 
           ,@phone 
           ,@idcard 
           ,@bespeakdate 
           ,@beaspeaknum 
           ,@beaspeaktype 
           ,@beaspeakstate 
           ,@remark 
           ,@subtime 
           ,@pno 
           ,@orderid 
           ,@proid 
           ,@proname 
           ,@comid )";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@bespeakname", m.Bespeakname);
            cmd.AddParam("@phone", m.Phone);
            cmd.AddParam("@idcard", m.Idcard);
            cmd.AddParam("@bespeakdate", m.Bespeakdate);
            cmd.AddParam("@beaspeaknum", m.Bespeaknum);
            cmd.AddParam("@beaspeaktype", m.beaspeaktype);
            cmd.AddParam("@beaspeakstate", m.beaspeakstate);
            cmd.AddParam("@remark", m.remark);
            cmd.AddParam("@subtime", m.subtime);
            cmd.AddParam("@pno", m.Pno);
            cmd.AddParam("@orderid", m.Orderid);
            cmd.AddParam("@proid", m.Proid);
            cmd.AddParam("@proname", m.Proname);
            cmd.AddParam("@comid", m.Comid);

            return cmd.ExecuteNonQuery();

        }

        internal int GetBespeaknum(string pno, string bespeakdate)
        {
            try
            {
                string sql = "select count(1) from b2b_order_bespeak where pno='" + pno + "' and bespeakdate='" + bespeakdate + "'";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return int.Parse(o.ToString());
            }
            catch
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal List<B2b_order_bespeak> Getbespeaklist(string comid, int pageindex, int pagesize, string bespeakdate, string key, string bespeaktype, string bespeakstate, out  int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            string condition = "comid=" + comid;

            if (bespeakdate != "")
            {
                condition += " and bespeakdate='" + bespeakdate + "'";
            }
            if (key != "")
            {
                condition += " and ( bespeakname like '%" + key + "%' or phone  like '%" + key + "%'  or idcard  like '%" + key + "%'  or remark  like '%" + key + "%' or pno  like '%" + key + "%' or orderid  like '%" + key + "%' or proid  like '%" + key + "%' or proname   like '%" + key + "%')";
            }
            if (bespeakstate != "-1")
            {
                condition += " and beaspeakstate in (" + bespeakstate + ")";
            }
            if (bespeaktype != "0,1")
            {
                condition += " and beaspeaktype in (" + bespeaktype + ")";
            }

            cmd.PagingCommand1("b2b_order_bespeak", "*", "beaspeakstate ,proid,subtime", "", pagesize, pageindex, "", condition);

            List<B2b_order_bespeak> list = new List<B2b_order_bespeak>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_order_bespeak
                    {
                        Id = reader.GetValue<int>("id"),
                        Bespeakdate = reader.GetValue<DateTime>("Bespeakdate"),
                        beaspeakstate = reader.GetValue<int>("beaspeakstate"),
                        Comid = reader.GetValue<int>("Comid"),
                        Idcard = reader.GetValue<string>("Idcard"),
                        Bespeakname = reader.GetValue<string>("Bespeakname"),
                        beaspeaktype = reader.GetValue<int>("beaspeaktype"),
                        Bespeaknum = reader.GetValue<int>("Beaspeaknum"),
                        Orderid = reader.GetValue<int>("Orderid"),
                        Phone = reader.GetValue<string>("Phone"),
                        Pno = reader.GetValue<string>("Pno"),
                        Proid = reader.GetValue<int>("Proid"),
                        Proname = reader.GetValue<string>("Proname"),
                        remark = reader.GetValue<string>("remark"),
                        Remark = reader.GetValue<string>("Remark"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;

        }

        internal int Operbespeakstate(int id, int bespeakstate)
        {
            string sql = "update b2b_order_bespeak set beaspeakstate =" + bespeakstate + " where id=" + id;
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return 0;
            }

        }

        internal int Gettotalbespeaknum(int proid, DateTime bespeakdate)
        {
            string sql = "select sum(beaspeaknum) from b2b_order_bespeak where proid=" + proid + " and convert(varchar(10), bespeakdate,120)='" + bespeakdate.ToString("yyyy-MM-dd") + "'";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return int.Parse(o.ToString());

            }
            catch (Exception e)
            {
                sqlHelper.Dispose();
                return 0;
            }
        }

        internal B2b_order_bespeak GetbespeakByid(int id)
        {
            string sql = "select * from b2b_order_bespeak where id=" + id;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                B2b_order_bespeak m = null;
                if (reader.Read())
                {
                    m = new B2b_order_bespeak()
                    {
                        Id = reader.GetValue<int>("id"),
                        Bespeakdate = reader.GetValue<DateTime>("Bespeakdate"),
                        beaspeakstate = reader.GetValue<int>("beaspeakstate"),
                        Comid = reader.GetValue<int>("Comid"),
                        Idcard = reader.GetValue<string>("Idcard"),
                        Bespeakname = reader.GetValue<string>("Bespeakname"),
                        beaspeaktype = reader.GetValue<int>("beaspeaktype"),
                        Bespeaknum = reader.GetValue<int>("Beaspeaknum"),
                        Orderid = reader.GetValue<int>("Orderid"),
                        Phone = reader.GetValue<string>("Phone"),
                        Pno = reader.GetValue<string>("Pno"),
                        Proid = reader.GetValue<int>("Proid"),
                        Proname = reader.GetValue<string>("Proname"),
                        remark = reader.GetValue<string>("remark"),
                        Remark = reader.GetValue<string>("Remark"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                    };
                }
                return m;
            }

        }

        internal B2b_order_bespeak Geteffectivebespeak(string pno)
        {
            string sql = "select top 1 * from b2b_order_bespeak where pno='" + pno + "' and beaspeakstate in (0,1)";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                B2b_order_bespeak m = null;
                if (reader.Read())
                {
                    m = new B2b_order_bespeak()
                    {
                        Id = reader.GetValue<int>("id"),
                        Bespeakdate = reader.GetValue<DateTime>("Bespeakdate"),
                        beaspeakstate = reader.GetValue<int>("beaspeakstate"),
                        Comid = reader.GetValue<int>("Comid"),
                        Idcard = reader.GetValue<string>("Idcard"),
                        Bespeakname = reader.GetValue<string>("Bespeakname"),
                        beaspeaktype = reader.GetValue<int>("beaspeaktype"),
                        Bespeaknum = reader.GetValue<int>("Beaspeaknum"),
                        Orderid = reader.GetValue<int>("Orderid"),
                        Phone = reader.GetValue<string>("Phone"),
                        Pno = reader.GetValue<string>("Pno"),
                        Proid = reader.GetValue<int>("Proid"),
                        Proname = reader.GetValue<string>("Proname"),
                        remark = reader.GetValue<string>("remark"),
                        Remark = reader.GetValue<string>("Remark"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                    };
                }
                return m;
            }

        }
    }
}

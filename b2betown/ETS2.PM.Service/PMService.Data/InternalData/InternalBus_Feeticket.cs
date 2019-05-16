using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;
using System.Data;
using System.Data.SqlClient;
using ETS.Framework;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.Common.Business;


namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalBus_Feeticket
    {
        private SqlHelper sqlHelper;
        public InternalBus_Feeticket(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        //编辑大巴车免费券
        internal int Bus_FeeticketInsertOrUpdate(Bus_Feeticket businfo)
        {
            string sql = "insert Bus_Feeticket (comid,Title,Feeday,Stardate,Enddate,Iuse)values(@comid,@Title,@Feeday,@Startime,@Endtime,@Iuse)";
            if (businfo.Id != 0)//如果有ID不为0则为修改
            {
                sql = "update Bus_Feeticket set Title=@Title,Feeday=@Feeday,Stardate=@Startime,Enddate=@Endtime,Iuse=@Iuse where id =@id ";
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", businfo.Id);
            cmd.AddParam("@Title", businfo.Title);
            cmd.AddParam("@Feeday", businfo.Feeday);
            cmd.AddParam("@Startime", businfo.Startime);
            cmd.AddParam("@Endtime", businfo.Endtime);
            cmd.AddParam("@comid", businfo.Comid);
            cmd.AddParam("@Iuse", businfo.Iuse);
            
           return cmd.ExecuteNonQuery();
        }


        #region 查询产品
        internal List<Bus_Feeticket> Bus_Feeticketpagelist(int comid, int pageindex, int pagesize, out int totalcount)
        {


            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "comid=" + comid ;


            cmd.PagingCommand1("Bus_Feeticket", "*", "id", "", pagesize, pageindex, "", condition);

            List<Bus_Feeticket> list = new List<Bus_Feeticket>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Bus_Feeticket
                    {
                        Id = reader.GetValue<int>("id"),
                        Feeday = reader.GetValue<int>("Feeday"),
                        Title = reader.GetValue<string>("Title"),
                        Startime = reader.GetValue<DateTime>("Stardate"),
                        Endtime = reader.GetValue<DateTime>("Enddate"),
                        Comid = reader.GetValue<int>("Comid"),
                        Iuse = reader.GetValue<int>("Iuse"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;

        }
        #endregion


        #region 查询产品
        internal Bus_Feeticket GetBus_FeeticketById(int id, int comid)
        {
            const string sqltxt = @"SELECT * FROM Bus_Feeticket where [id]=@id and comid=@comid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", id);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Bus_Feeticket
                    {

                        Id = reader.GetValue<int>("id"),
                        Feeday = reader.GetValue<int>("Feeday"),
                        Title = reader.GetValue<string>("Title"),
                        Startime = reader.GetValue<DateTime>("Stardate"),
                        Endtime = reader.GetValue<DateTime>("Enddate"),
                        Comid = reader.GetValue<int>("Comid"),
                        Iuse = reader.GetValue<int>("Iuse"),

                    };
                }
                return null;
            }
        }
        #endregion


        //删除
        internal int DeleteBus_FeeticketById(int id,int comid)
        {
            
            string sql = "delete Bus_Feeticket where id =@id and comid=@comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);
            cmd.AddParam("@comid", comid);

            return cmd.ExecuteNonQuery();
        }



        //导入大巴车免费券
        internal int Bus_Feeticket_pnoInsertOrUpdate(Bus_Feeticket_pno businfo)
        {
            string sql = "insert Bus_Feeticket_pno (Busid,Pno,Num)values(@Busid,@Pno,@Num)";
            if (businfo.Id != 0)//如果有ID不为0则为修改
            {
                sql = "update Bus_Feeticket_pno set Busid=@Busid,Pno=@Pno,Num=@Num where id =@id ";
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", businfo.Id);
            cmd.AddParam("@Busid", businfo.Busid);
            cmd.AddParam("@Pno", businfo.Pno);
            cmd.AddParam("@Num", businfo.Num);

            return cmd.ExecuteNonQuery();
        }




        #region 查询产品
        internal List<Bus_feeticket_Pro> BusFeeticketpropagelist(int busid, int pageindex, int pagesize, out int totalcount)
        {
            int i = 0;
            string sql = "select a.id,a.busid,a.proid,b.pro_name from Bus_Feeticket_pro as a left join b2b_com_pro as b on a.proid=b.id where a.busid=" + busid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<Bus_feeticket_Pro> list = new List<Bus_feeticket_Pro>();
             using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Bus_feeticket_Pro
                    {
                        Id = reader.GetValue<int>("id"),
                        Busid = reader.GetValue<int>("Busid"),
                        Proid = reader.GetValue<int>("proid"),
                        proname = reader.GetValue<string>("pro_name"),
                    });
                    i++;
                }
            }
            totalcount = i;

            return list;

        }
        #endregion


        #region 查询产品
        internal List<Bus_Feeticket_pno> BusFeeticketpnopagelist(int busid, int pageindex, int pagesize, out int totalcount)
        {
            int i = 0;
            string sql = "select a.id,a.busid,a.proid,b.pro_name from Bus_Feeticket_pno as a left join b2b_com_pro as b on a.proid=b.id where a.busid=" + busid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<Bus_Feeticket_pno> list = new List<Bus_Feeticket_pno>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Bus_Feeticket_pno
                    {
                        Id = reader.GetValue<int>("id"),
                        Busid = reader.GetValue<int>("Busid"),
                        Proid = reader.GetValue<int>("proid"),
                        proname = reader.GetValue<string>("pro_name"),
                    });
                    i++;
                }
            }
            totalcount = i;

            return list;

        }
        #endregion



        #region 查询产品
        internal List<Bus_Feeticket_pno> Bus_Feeticket_pnopagelist(int busid, int pageindex, int pagesize, out int totalcount)
        {


            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "a.busid=" + busid;


            cmd.PagingCommand1("Bus_Feeticket_pno as a left join as b on a.proid=b.id", "a.id,a.busid,a.proid,b.pro_name", "a.id", "", pagesize, pageindex, "", condition);

            List<Bus_Feeticket_pno> list = new List<Bus_Feeticket_pno>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Bus_Feeticket_pno
                    {
                        Id = reader.GetValue<int>("id"),
                        Busid = reader.GetValue<int>("Busid"),
                        Proid = reader.GetValue<int>("proid"),
                        proname = reader.GetValue<string>("pro_name"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;

        }
        #endregion


        #region 查询产品            
        internal Bus_Feeticket_pno GetBus_Feeticket_pnoById(int id, int busid,int proid=0)
        {
            string sqltxt = @"SELECT * FROM Bus_Feeticket_pno where [id]=@id and busid=@busid ";
            if (proid != 0) {
                sqltxt = @"SELECT * FROM Bus_Feeticket_pno where [proid]=@proid and busid=@busid ";
            }

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", id);
            cmd.AddParam("@busid", busid);
            cmd.AddParam("@proid", proid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Bus_Feeticket_pno
                    {

                        Id = reader.GetValue<int>("id"),
                        Busid = reader.GetValue<int>("Busid"),
                        Proid = reader.GetValue<int>("proid"),

                    };
                }
                return null;
            }
        }
        #endregion


        //删除
        internal int DeleteBus_Feeticket_pnoById(int id, int busid)
        {

            string sql = "delete Bus_Feeticket_pno where id =@id and busid=@busid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);
            cmd.AddParam("@busid", busid);

            return cmd.ExecuteNonQuery();
        }

        //设定可用的大巴车产品
        internal int Bus_feeticket_ProInsert(Bus_feeticket_Pro businfo)
        {
            string sql = "insert Bus_feeticket_Pro (Busid,Proid)values(@Busid,@Proid)";
            if (businfo.Id != 0)//如果有ID不为0则为修改
            {
                sql = "update Bus_feeticket_Pro set Busid=@Busid,Proid=@Proid where id =@id ";
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", businfo.Id);
            cmd.AddParam("@Busid", businfo.Busid);
            cmd.AddParam("@Proid", businfo.Proid);

            return cmd.ExecuteNonQuery();
        }



        #region 查询产品
        internal List<Bus_feeticket_Pro> Bus_feeticket_Propagelist( int busid, int pageindex, int pagesize, out int totalcount)
        {


            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = " a.busid=" + busid;


            cmd.PagingCommand1("Bus_Feeticket_pno as a left join b2b_com_pro as b  on a.proid=b.id", "a.id,a.proid,a.busid,b.pro_name", "a.id", "", pagesize, pageindex, "", condition);

            List<Bus_feeticket_Pro> list = new List<Bus_feeticket_Pro>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Bus_feeticket_Pro
                    {
                        Id = reader.GetValue<int>("id"),
                        Proid = reader.GetValue<int>("Proid"),
                        Busid = reader.GetValue<int>("Busid"),
                        proname = reader.GetValue<string>("pro_name"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;

        }
        #endregion


        #region 查询产品
        internal Bus_feeticket_Pro GetBus_feeticket_ProById(int id, int busid,int proid=0,string pno="")
        {
            string sqltxt = @"SELECT * FROM Bus_Feeticket_pro where [id]=@id and busid=@busid ";

            if (proid != 0)
            {
                sqltxt = @"SELECT * FROM Bus_Feeticket_pro where [proid]=@proid and busid=@busid ";
            }

            if (pno != "") {
                sqltxt = @"SELECT * FROM Bus_Feeticket_pro where proid=@proid and [busid] in (select busid from Bus_Feeticket_pno where proid in (select pro_id from b2b_eticket where pno=@pno)) ";
            }

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", id);
            cmd.AddParam("@busid", busid);
            cmd.AddParam("@proid", proid);
            cmd.AddParam("@pno", pno);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Bus_feeticket_Pro
                    {
                        Id = reader.GetValue<int>("id"),
                        Proid = reader.GetValue<int>("Proid"),
                        Busid = reader.GetValue<int>("Busid"),
                        limitweek = reader.GetValue<int>("limitweek"),
                        limitweekdaynum = reader.GetValue<int>("limitweekdaynum"),
                        limitweekendnum = reader.GetValue<int>("limitweekendnum"),

                    };
                }
                return null;
            }
        }
        #endregion
        #region 查询产品
        internal int BusSearchpnobyproid(int comid, int proid )
        {
            string   sqltxt = @"SELECT * FROM Bus_Feeticket_pno where [proid]=@proid ";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
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
        #endregion

        #region 查询产品
        internal int BusSearchprobyproid(int comid, int proid)
        {
            string sqltxt = @"SELECT * FROM Bus_Feeticket_pro where [proid]=@proid ";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
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
        #endregion
        


        //删除
        internal int DeleteBus_feeticket_ProById(int id, int busid)
        {

            string sql = "delete Bus_feeticket_Pro where id =@id and busid=@busid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);
            cmd.AddParam("@busid", busid);

            return cmd.ExecuteNonQuery();
        }


        #region 查询产品
        internal List<B2b_com_pro> busfeeticketbindingpropagelist(int pageindex, int pagesize, int busid, int bindingprotype, int comid, out int totalcount)
        {


            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "com_id=" + comid;
            if (bindingprotype == 1)//产品
            {
                condition += " and server_type=10";
            }
            else {
                condition += " and server_type=1 and pro_state=1";
            }
            cmd.PagingCommand1("b2b_com_pro", "id,pro_name,com_id", "id", "", pagesize, pageindex, "", condition);

            List<B2b_com_pro> list = new List<B2b_com_pro>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_pro
                    {
                        Id = reader.GetValue<int>("id"),
                        Pro_name = reader.GetValue<string>("Pro_name"),
                        Com_id = reader.GetValue<int>("Com_id"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;

        }
        #endregion

        //绑定
        internal int Busbindingpro(int busid, int comid, int proid, int type, int subtype, int limitweek, int limitweekdaynum, int limitweekendnum)
        {
            string sql = "";
            if (type == 1)//产品
            {
                if (subtype == 1)//绑定，
                {
                    sql = "insert Bus_Feeticket_pro (busid,proid,limitweek,limitweekdaynum,limitweekendnum)values(@busid,@proid,@limitweek,@limitweekdaynum,@limitweekendnum)";
                }
                else {//解绑
                    sql = "delete Bus_Feeticket_pro where busid =@busid and proid=@proid";
                }
            }
            else {//码
                if (subtype == 1)
                {
                    sql = "insert Bus_Feeticket_pno (busid,proid)values(@busid,@proid)";
                }
                else
                {
                    sql = "delete Bus_Feeticket_pno where busid =@busid and proid=@proid";
                }
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@busid", busid);
            cmd.AddParam("@proid", proid);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@limitweek", limitweek);
            cmd.AddParam("@limitweekdaynum", limitweekdaynum);
            cmd.AddParam("@limitweekendnum", limitweekendnum);

            return cmd.ExecuteNonQuery();
        }



        #region 查询产品
        internal int BusSearchpno_propipei(string pno, int proid)
        {
            string sqltxt = @"select id from bus_feeticket_pro where busid in ( SELECT busid FROM Bus_Feeticket_pno where [proid] in (select pro_id from b2b_eticket where pno=@pno)) and proid=@proid ";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@pno", pno);
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
        #endregion

    }
}

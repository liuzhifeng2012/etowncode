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
    public class InternalRentserver
    {
        private SqlHelper sqlHelper;
        public InternalRentserver(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        //修改
        internal int upRentserver(int id, int comid, string servername, int WR, int num, int posid, decimal saleprice, decimal serverDepositprice, string renttype, int mustselect, int servertype, int printticket, int Fserver)
        {
            string sql = "";

            if (id == 0)
            {
                sql = "insert b2b_Rentserver (comid,servername,wr,num,posid,saleprice,serverDepositprice,renttype,mustselect,servertype,printticket,Fserver)values(@comid,@servername,@WR,@num,@posid,@saleprice,@serverDepositprice,@renttype,@mustselect,@servertype,@printticket,@Fserver)";
            }
            else
            {
                sql = "update b2b_Rentserver set servername =@servername,WR=@WR,num=@num,posid=@posid,saleprice=@saleprice,serverDepositprice=@serverDepositprice,renttype=@renttype,mustselect=@mustselect,servertype=@servertype,printticket=@printticket,Fserver=@Fserver where id=@id";
            }


            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);
            cmd.AddParam("@servername", servername);
            cmd.AddParam("@renttype", renttype);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@WR", WR);
            cmd.AddParam("@num", num);
            cmd.AddParam("@posid", posid);
            cmd.AddParam("@saleprice", saleprice);
            cmd.AddParam("@serverDepositprice", serverDepositprice);
            cmd.AddParam("@mustselect", mustselect);
            cmd.AddParam("@servertype", servertype);
            cmd.AddParam("@printticket", printticket);
            cmd.AddParam("@Fserver", Fserver);

            return cmd.ExecuteNonQuery();
        }

        //删除
        internal int delRentserver(int id, int comid)
        {
            string sql = "";
            sql = "delete b2b_Rentserver where comid=@comid and id=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);
            cmd.AddParam("@comid", comid);

            return cmd.ExecuteNonQuery();
        }


        #region 服务查询
        internal List<B2b_Rentserver> Rentserverpagelist(int pageindex, int pagesize, int comid, out int totalcount, int proid = 0, string pno = "")
        {

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "comid=" + comid;

            if (proid != 0)
            {
                condition += " and id in(select sid from b2b_pro_rentserver where proid=" + proid + ") and servertype=0";//只读需要服务验证的判断

            }
            if (pno != "")
            {

                condition += " and not id in( select sid from b2b_eticket_Deposit where eticketid in( select id from b2b_eticket where pno='" + pno + "')) and servertype=0";//只读需要服务验证的判断
            }


            cmd.PagingCommand1("b2b_Rentserver", "*", "renttype,id", "", pagesize, pageindex, "", condition);

            List<B2b_Rentserver> list = new List<B2b_Rentserver>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_Rentserver
                    {
                        id = reader.GetValue<int>("id"),
                        servername = reader.GetValue<string>("servername"),
                        renttype = reader.GetValue<string>("renttype"),
                        comid = reader.GetValue<int>("comid"),
                        WR = reader.GetValue<int>("WR"),
                        num = reader.GetValue<int>("num"),
                        posid = reader.GetValue<int>("posid"),
                        saleprice = reader.GetValue<decimal>("saleprice"),
                        serverDepositprice = reader.GetValue<decimal>("serverDepositprice"),
                        mustselect = reader.GetValue<int>("mustselect"),
                        servertype = reader.GetValue<int>("servertype"),
                        printticket = reader.GetValue<int>("printticket"),
                        Fserver = reader.GetValue<int>("Fserver"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;

        }
        #endregion


        #region 服务查询
        internal B2b_Rentserver Rentserverbyid(int id, int comid)
        {

            string sqltxt = @"select * from b2b_Rentserver where id=@id and comid=@comid ";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", id);
            cmd.AddParam("@comid", comid);
            B2b_Rentserver list = null;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    list = new B2b_Rentserver
                    {
                        id = reader.GetValue<int>("id"),
                        servername = reader.GetValue<string>("servername"),
                        renttype = reader.GetValue<string>("renttype"),
                        comid = reader.GetValue<int>("comid"),
                        WR = reader.GetValue<int>("WR"),
                        num = reader.GetValue<int>("num"),
                        posid = reader.GetValue<int>("posid"),
                        saleprice = reader.GetValue<decimal>("saleprice"),
                        serverDepositprice = reader.GetValue<decimal>("serverDepositprice"),
                        mustselect = reader.GetValue<int>("mustselect"),
                        servertype = reader.GetValue<int>("servertype"),
                        printticket = reader.GetValue<int>("printticket"),
                        Fserver = reader.GetValue<int>("Fserver"),
                    };

                }
                return list;
            }

        }
        #endregion

        #region 服务查询
        internal B2b_Rentserver RentserverbyFid(int Fid, int comid)
        {

            string sqltxt = @"select * from b2b_Rentserver where Fserver=@id and comid=@comid ";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", Fid);
            cmd.AddParam("@comid", comid);
            B2b_Rentserver list = null;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    list = new B2b_Rentserver
                    {
                        id = reader.GetValue<int>("id"),
                        servername = reader.GetValue<string>("servername"),
                        renttype = reader.GetValue<string>("renttype"),
                        comid = reader.GetValue<int>("comid"),
                        WR = reader.GetValue<int>("WR"),
                        num = reader.GetValue<int>("num"),
                        posid = reader.GetValue<int>("posid"),
                        saleprice = reader.GetValue<decimal>("saleprice"),
                        serverDepositprice = reader.GetValue<decimal>("serverDepositprice"),
                        mustselect = reader.GetValue<int>("mustselect"),
                        servertype = reader.GetValue<int>("servertype"),
                        printticket = reader.GetValue<int>("printticket"),
                        Fserver = reader.GetValue<int>("Fserver"),
                    };

                }
                return list;
            }

        }
        #endregion

        #region 服务查询
        internal B2b_Rentserver Rentserverbyuserinfoid(int id, int comid)
        {

            string sqltxt = @"select * from b2b_Rentserver where comid=@comid and id in(select sid from b2b_eticket_Deposit where id=@id) ";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", id);
            cmd.AddParam("@comid", comid);
            B2b_Rentserver list = null;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    list = new B2b_Rentserver
                    {
                        id = reader.GetValue<int>("id"),
                        servername = reader.GetValue<string>("servername"),
                        renttype = reader.GetValue<string>("renttype"),
                        comid = reader.GetValue<int>("comid"),
                        WR = reader.GetValue<int>("WR"),
                        num = reader.GetValue<int>("num"),
                        posid = reader.GetValue<int>("posid"),
                        saleprice = reader.GetValue<decimal>("saleprice"),
                        serverDepositprice = reader.GetValue<decimal>("serverDepositprice"),
                        mustselect = reader.GetValue<int>("mustselect"),
                        servertype = reader.GetValue<int>("servertype"),
                        printticket = reader.GetValue<int>("printticket"),
                        Fserver = reader.GetValue<int>("Fserver"),
                    };

                }
                return list;
            }

        }
        #endregion




        #region 服务查询
        internal B2b_Rentserver Rentserverby_user_Info_id(int id)
        {

            string sqltxt = @"select * from b2b_Rentserver where id in (select sid from b2b_eticket_Deposit where id in(select rentserverid from b2b_Rentserver_User_info where id=@id)) ";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", id);
            B2b_Rentserver list = null;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    list = new B2b_Rentserver
                    {
                        id = reader.GetValue<int>("id"),
                        servername = reader.GetValue<string>("servername"),
                        renttype = reader.GetValue<string>("renttype"),
                        comid = reader.GetValue<int>("comid"),
                        WR = reader.GetValue<int>("WR"),
                        num = reader.GetValue<int>("num"),
                        posid = reader.GetValue<int>("posid"),
                        saleprice = reader.GetValue<decimal>("saleprice"),
                        serverDepositprice = reader.GetValue<decimal>("serverDepositprice"),
                        mustselect = reader.GetValue<int>("mustselect"),
                        servertype = reader.GetValue<int>("servertype"),
                        printticket = reader.GetValue<int>("printticket"),
                        Fserver = reader.GetValue<int>("Fserver"),
                    };

                }
                return list;
            }

        }
        #endregion

        #region 服务查询
        internal B2b_Rentserver Rentserverbyidandproid(int id, int proid)
        {

            string sqltxt = @"select * from b2b_Rentserver where id=@id and id in( select sid from  b2b_pro_rentserver where proid=@proid) ";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", id);
            cmd.AddParam("@proid", proid);
            B2b_Rentserver list = null;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    list = new B2b_Rentserver
                    {
                        id = reader.GetValue<int>("id"),
                        servername = reader.GetValue<string>("servername"),
                        renttype = reader.GetValue<string>("renttype"),
                        comid = reader.GetValue<int>("comid"),
                        WR = reader.GetValue<int>("WR"),
                        num = reader.GetValue<int>("num"),
                        posid = reader.GetValue<int>("posid"),
                        saleprice = reader.GetValue<decimal>("saleprice"),
                        serverDepositprice = reader.GetValue<decimal>("serverDepositprice"),
                        mustselect = reader.GetValue<int>("mustselect"),
                        servertype = reader.GetValue<int>("servertype"),
                        printticket = reader.GetValue<int>("printticket"),
                        Fserver = reader.GetValue<int>("Fserver"),
                    };

                }
                return list;
            }

        }
        #endregion
        #region 跟产品id查询所有绑定的服务
        internal B2b_Rentserver Rentserverproid(int proid)
        {

            string sqltxt = @"select * from b2b_Rentserver where id in( select sid from  b2b_pro_rentserver where proid=@proid) ";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@proid", proid);
            B2b_Rentserver list = null;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    list = new B2b_Rentserver
                    {
                        id = reader.GetValue<int>("id"),
                        servername = reader.GetValue<string>("servername"),
                        renttype = reader.GetValue<string>("renttype"),
                        comid = reader.GetValue<int>("comid"),
                        WR = reader.GetValue<int>("WR"),
                        num = reader.GetValue<int>("num"),
                        posid = reader.GetValue<int>("posid"),
                        saleprice = reader.GetValue<decimal>("saleprice"),
                        serverDepositprice = reader.GetValue<decimal>("serverDepositprice"),
                        mustselect = reader.GetValue<int>("mustselect"),
                        servertype = reader.GetValue<int>("servertype"),
                        printticket = reader.GetValue<int>("printticket"),
                        Fserver = reader.GetValue<int>("Fserver"),
                    };

                }
                return list;
            }

        }
        #endregion

        #region 通过pos查询服务
        internal B2b_Rentserver Rentserverbyposid(int posid)
        {

            string sqltxt = @"select * from b2b_Rentserver where posid=@posid";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@posid", posid);
            B2b_Rentserver list = null;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    list = new B2b_Rentserver
                    {
                        id = reader.GetValue<int>("id"),
                        servername = reader.GetValue<string>("servername"),
                        renttype = reader.GetValue<string>("renttype"),
                        comid = reader.GetValue<int>("comid"),
                        WR = reader.GetValue<int>("WR"),
                        num = reader.GetValue<int>("num"),
                        posid = reader.GetValue<int>("posid"),
                        saleprice = reader.GetValue<decimal>("saleprice"),
                        serverDepositprice = reader.GetValue<decimal>("serverDepositprice"),
                        mustselect = reader.GetValue<int>("mustselect"),
                        servertype = reader.GetValue<int>("servertype"),
                        printticket = reader.GetValue<int>("printticket"),
                        Fserver = reader.GetValue<int>("Fserver"),
                    };
                }
                return list;

            }

        }
        #endregion


        #region 通过pos查询服务
        internal List<B2b_Rentserver> RentserverListbyposid(int posid)
        {

            string sqltxt = @"select * from b2b_Rentserver where posid=@posid";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@posid", posid);
            List<B2b_Rentserver> list = new List<B2b_Rentserver>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_Rentserver
                    {
                        id = reader.GetValue<int>("id"),
                        servername = reader.GetValue<string>("servername"),
                        renttype = reader.GetValue<string>("renttype"),
                        comid = reader.GetValue<int>("comid"),
                        WR = reader.GetValue<int>("WR"),
                        num = reader.GetValue<int>("num"),
                        posid = reader.GetValue<int>("posid"),
                        saleprice = reader.GetValue<decimal>("saleprice"),
                        serverDepositprice = reader.GetValue<decimal>("serverDepositprice"),
                        mustselect = reader.GetValue<int>("mustselect"),
                        servertype = reader.GetValue<int>("servertype"),
                        printticket = reader.GetValue<int>("printticket"),
                        Fserver = reader.GetValue<int>("Fserver"),
                    });

                }
            }

            return list;

        }
        #endregion

        #region 服务查询
        internal int Rentserverbinding(int id, int proid)
        {

            string sqltxt = @"select * from b2b_pro_rentserver where sid=@id and proid=@proid ";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", id);
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


        //插入产品与服务绑定
        internal int inpro_rentserver(int proid, int sid, int comid)
        {
            string sql = "";

            sql = "insert b2b_pro_rentserver(proid,sid)values(" + proid + "," + sid + ")";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        //先删除产品与服务绑定
        internal int deletepro_rentserver(int proid, int comid)
        {
            string sql = "";
            sql = "delete b2b_pro_rentserver where proid=" + proid;
            var cmd1 = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd1.ExecuteNonQuery();


        }

        //插入或修改用户表
        internal int inb2b_Rentserver_User(B2b_Rentserver_User Rentserver_User)
        {
            string sql = "";
            if (Rentserver_User.id == 0)
            {
                sql = "insert b2b_Rentserver_User(oid,comid,cardid,Depositstate,serverstate,Depositprice,Depositorder,Depositcome,subtime,eticketid,pname,endtime,usernum)values(" + Rentserver_User.oid + "," + Rentserver_User.comid + ",'" + Rentserver_User.cardid + "'," + Rentserver_User.Depositstate + "," + Rentserver_User.serverstate + "," + Rentserver_User.Depositprice + "," + Rentserver_User.Depositorder + ",'" + Rentserver_User.Depositcome + "','" + DateTime.Now + "'," + Rentserver_User.eticketid + ",'" + Rentserver_User.pname + "','" + Rentserver_User.endtime + "',"+Rentserver_User.usenum+");select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
            else
            {
                sql = "update b2b_Rentserver_User set cardid='" + Rentserver_User.cardid + "',Depositstate=" + Rentserver_User.Depositstate + ",serverstate=" + Rentserver_User.serverstate + ",Depositprice=" + Rentserver_User.Depositprice + ",Depositorder=" + Rentserver_User.Depositorder + ",Depositcome='" + Rentserver_User.Depositcome + "',eticketid='" + Rentserver_User.eticketid + "',pname='" + Rentserver_User.pname + "',sendnum=" + Rentserver_User.sendnum + ",sendstate=" + Rentserver_User.sendstate + ",endtime='" + Rentserver_User.endtime + "',usernum=" + Rentserver_User.usenum + " where id=" + Rentserver_User.id;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                return cmd.ExecuteNonQuery();
            }
        }

        //插入或修改用户表
        internal int jianb2b_Rentserver_User(int id)
        {
            string sql = "";

            sql = "update b2b_Rentserver_User set usernum=usernum-1 where id=" + id;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                return cmd.ExecuteNonQuery();
        }

        //修改用户卡序号id
        internal int upRentserver_User_kaid(string cardid, string cardchipid)
        {

            //对现有发卡记录 赋值芯片id
            string sql = "update b2b_Rentserver_User set cardchipid='" + cardchipid + "',cardchipid_notes='" + cardchipid + "' where cardid='" + cardid + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }


        //清空已有芯片id
        internal int clearRentserver_User_kaid(string cardid, string cardchipid)
        {
            //先清除所有 本卡所使用的 账户
            string sql1 = "update b2b_Rentserver_User set cardchipid='0' where cardchipid='" + cardchipid + "'";
            var cmd1 = sqlHelper.PrepareTextSqlCommand(sql1);
            return cmd1.ExecuteNonQuery();
        }


         //修改发送状态
        internal int upRentserver_User_sendstate_str(int sendstate, string cardchipid, string reuntstr)
        {
            //先清除所有 本卡所使用的 账户
            string sql1 = "update b2b_Rentserver_User set sendstate=" + sendstate + ",sendnum=sendnum+1,sendstr='" + reuntstr + "',sendtime='"+DateTime.Now+"' where cardchipid='" + cardchipid + "'";
            var cmd1 = sqlHelper.PrepareTextSqlCommand(sql1);
            return cmd1.ExecuteNonQuery();
        }
        

        #region 服务查询用户表
        internal B2b_Rentserver_User SearchRentserver_User(int id, string cardchipid)
        {

            string sqltxt = @"select * from b2b_Rentserver_User where id=@id ";

            if (id == 0)
            {
                sqltxt = @"select * from b2b_Rentserver_User where cardchipid=@cardchipid ";
            }

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            if (id != 0)
            {
                cmd.AddParam("@id", id);
            }
            else
            {
                cmd.AddParam("@cardchipid", cardchipid);
            }


            using (var reader = cmd.ExecuteReader())
            {
                B2b_Rentserver_User m = null;
                if (reader.Read())
                {
                    m = new B2b_Rentserver_User
                    {

                        id = reader.GetValue<int>("id"),
                        cardid = reader.GetValue<string>("cardid"),
                        Depositcome = reader.GetValue<string>("Depositcome"),
                        Depositorder = reader.GetValue<int>("Depositorder"),
                        Depositprice = reader.GetValue<decimal>("Depositprice"),
                        Depositstate = reader.GetValue<int>("Depositstate"),
                        oid = reader.GetValue<int>("oid"),
                        serverstate = reader.GetValue<int>("serverstate"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                        cardchipid = reader.GetValue<string>("cardchipid"),
                        eticketid = reader.GetValue<int>("eticketid"),
                        comid = reader.GetValue<int>("comid"),
                        pname = reader.GetValue<string>("pname"),
                        sendnum = reader.GetValue<int>("sendnum"),
                        sendstate = reader.GetValue<int>("sendstate"),
                        endtime = reader.GetValue<DateTime>("endtime"),
                    };

                }
                return m;
            }


        }
        #endregion
        #region 服务查询用户表
        internal List<B2b_Rentserver_User> SearchRentserver_Userbypno(string pno, out int count)
        {

            string sqltxt = @"select * from b2b_Rentserver_User where eticketid in (select id from b2b_eticket where oid in (select oid from b2b_eticket where pno=@pno)) ";
            //string sqltxt = @"select * from b2b_Rentserver_User where eticketid in (select id from b2b_eticket where pno=@pno) ";
            int i = 0;
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@pno", pno);
            List<B2b_Rentserver_User> list = new List<B2b_Rentserver_User>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_Rentserver_User
                    {
                        id = reader.GetValue<int>("id"),
                        cardid = reader.GetValue<string>("cardid"),
                        Depositcome = reader.GetValue<string>("Depositcome"),
                        Depositorder = reader.GetValue<int>("Depositorder"),
                        Depositprice = reader.GetValue<decimal>("Depositprice"),
                        Depositstate = reader.GetValue<int>("Depositstate"),
                        oid = reader.GetValue<int>("oid"),
                        serverstate = reader.GetValue<int>("serverstate"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                        cardchipid = reader.GetValue<string>("cardchipid"),
                        eticketid = reader.GetValue<int>("eticketid"),
                    });
                    i = i + 1;
                }

            }
            count = i;
            return list;
        }
        #endregion



        #region 服务查询用户表
        internal B2b_Rentserver_User SearchRentserver_UserbyCardID(string cardchipid)
        {

            string sqltxt = @"select * from b2b_Rentserver_User where cardchipid=@cardchipid ";
            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@cardchipid", cardchipid);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_Rentserver_User m = null;
                if (reader.Read())
                {
                    m = new B2b_Rentserver_User
                    {

                        id = reader.GetValue<int>("id"),
                        cardid = reader.GetValue<string>("cardid"),
                        Depositcome = reader.GetValue<string>("Depositcome"),
                        Depositorder = reader.GetValue<int>("Depositorder"),
                        Depositprice = reader.GetValue<decimal>("Depositprice"),
                        Depositstate = reader.GetValue<int>("Depositstate"),
                        oid = reader.GetValue<int>("oid"),
                        serverstate = reader.GetValue<int>("serverstate"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                        cardchipid = reader.GetValue<string>("cardchipid"),
                        eticketid = reader.GetValue<int>("eticketid"),
                        comid = reader.GetValue<int>("comid"),
                        pname = reader.GetValue<string>("pname"),
                        sendnum = reader.GetValue<int>("sendnum"),
                        sendstate = reader.GetValue<int>("sendstate"),
                        endtime = reader.GetValue<DateTime>("endtime"),
                    };

                }
                return m;
            }
        }
        #endregion

        #region 服务查询用户表
        internal B2b_Rentserver_User SearchRentserver_bypno(string pno)
        {

            string sqltxt = @"select * from b2b_Rentserver_User where eticketid in (select id from b2b_eticket where pno=@pno) ";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@pno", pno);
            B2b_Rentserver_User list = null;
            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    list = new B2b_Rentserver_User
                    {
                        id = reader.GetValue<int>("id"),
                        cardid = reader.GetValue<string>("cardid"),
                        Depositcome = reader.GetValue<string>("Depositcome"),
                        Depositorder = reader.GetValue<int>("Depositorder"),
                        Depositprice = reader.GetValue<decimal>("Depositprice"),
                        Depositstate = reader.GetValue<int>("Depositstate"),
                        oid = reader.GetValue<int>("oid"),
                        serverstate = reader.GetValue<int>("serverstate"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                        cardchipid = reader.GetValue<string>("cardchipid"),
                        eticketid = reader.GetValue<int>("eticketid"),
                    };

                }

            }

            return list;
        }
        #endregion



        #region 服务查询用户表
        internal List<B2b_Rentserver_User> Rentserver_Userpagelist(int pageindex, int pagesize, int oid, out int totalcount)
        {

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "oid=" + oid;

            cmd.PagingCommand1("b2b_Rentserver_User", "*", "id", "", pagesize, pageindex, "", condition);

            List<B2b_Rentserver_User> list = new List<B2b_Rentserver_User>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_Rentserver_User
                    {
                        id = reader.GetValue<int>("id"),
                        cardid = reader.GetValue<string>("cardid"),
                        Depositcome = reader.GetValue<string>("Depositcome"),
                        Depositorder = reader.GetValue<int>("Depositorder"),
                        Depositprice = reader.GetValue<int>("Depositprice"),
                        Depositstate = reader.GetValue<int>("Depositstate"),
                        oid = reader.GetValue<int>("oid"),
                        serverstate = reader.GetValue<int>("serverstate"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;

        }
        #endregion




        //插入修改用户服务表
        internal int inb2b_Rentserver_User_info(B2b_Rentserver_User_info Rentserver_User_info)
        {
            string sql = "";
            if (Rentserver_User_info.id == 0)
            {
                sql = "insert b2b_Rentserver_User_info(Userid,Rentserverid,Verstate,num)values(" + Rentserver_User_info.Userid + "," + Rentserver_User_info.Rentserverid + "," + Rentserver_User_info.Verstate + "," + Rentserver_User_info.num + ")";
            }
            else
            {
                sql = "update b2b_Rentserver_User_info set Userid=" + Rentserver_User_info.Userid + ",Rentserverid=" + Rentserver_User_info.Rentserverid + ",Verstate=" + Rentserver_User_info.Verstate + ",num=" + Rentserver_User_info.num + " ,Vertime='" + Rentserver_User_info.Vertime + "',Rettime='" + Rentserver_User_info.Rettime + "',Remarks='" + Rentserver_User_info.Remarks + "' where id=" + Rentserver_User_info.id;
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        #region 服务查询用户服务表
        internal B2b_Rentserver_User_info SearchRentserver_User_info(int Userid, int Rentserverid)
        {

            string sqltxt = @"select * from b2b_Rentserver_User_info where Userid=@Userid and Rentserverid in (select id from b2b_eticket_Deposit where sid=@Rentserverid) ";


            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@Userid", Userid);
            cmd.AddParam("@Rentserverid", Rentserverid);
            B2b_Rentserver_User_info list = null;
            using (var reader = cmd.ExecuteReader())
            {



                if (reader.Read())
                {
                    list = new B2b_Rentserver_User_info
                    {

                        id = reader.GetValue<int>("id"),
                        Userid = reader.GetValue<int>("Userid"),
                        Rentserverid = reader.GetValue<int>("Rentserverid"),
                        Verstate = reader.GetValue<int>("Verstate"),
                        num = reader.GetValue<int>("num"),
                        Vertime = reader.GetValue<DateTime>("Vertime"),
                        Rettime = reader.GetValue<DateTime>("Rettime"),
                    };


                }
                return list;
            }

        }
        #endregion
        #region 服务查询超时
        internal B2b_Rentserver_User_info SearchRentserver_User_outtime(int Userid)
        {

            string sqltxt = @"select * from b2b_Rentserver_User_info where Userid=@Userid and Rentserverid in (select id from b2b_eticket_Deposit where sid in (select id from b2b_Rentserver where printticket=1 )) ";


            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@Userid", Userid);
            B2b_Rentserver_User_info list = null;
            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    list = new B2b_Rentserver_User_info
                    {

                        id = reader.GetValue<int>("id"),
                        Userid = reader.GetValue<int>("Userid"),
                        Rentserverid = reader.GetValue<int>("Rentserverid"),
                        Verstate = reader.GetValue<int>("Verstate"),
                        num = reader.GetValue<int>("num"),
                        Vertime = reader.GetValue<DateTime>("Vertime"),
                        Rettime = reader.GetValue<DateTime>("Rettime"),
                    };

                }



                return list;
            }

        }
        #endregion




        #region 服务查询用户服务表
        internal List<B2b_Rentserver_User_info> SearchRentserver_User_list_state(int Userid, string Rentserverlistid)
        {

            string sqltxt = @"select * from b2b_Rentserver_User_info where Userid=@Userid and Rentserverid in (select id from b2b_eticket_Deposit where sid in(" + Rentserverlistid + ") ) ";


            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@Userid", Userid);
            List<B2b_Rentserver_User_info> list = new List<B2b_Rentserver_User_info>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_Rentserver_User_info
                    {
                        id = reader.GetValue<int>("id"),
                        Userid = reader.GetValue<int>("Userid"),
                        Rentserverid = reader.GetValue<int>("Rentserverid"),
                        Verstate = reader.GetValue<int>("Verstate"),
                        num = reader.GetValue<int>("num"),
                        Vertime = reader.GetValue<DateTime>("Vertime"),
                        Rettime = reader.GetValue<DateTime>("Rettime"),
                        Remarks = reader.GetValue<string>("Remarks"),
                    });

                }
            }

            return list;

        }
        #endregion

        #region 服务查询用户服务表
        internal List<B2b_Rentserver> SearchRentserver_User_list(int Userid, string Rentserverlistid)
        {

            string sqltxt = @"select * from b2b_Rentserver where id in (select sid from b2b_eticket_Deposit where sid in(" + Rentserverlistid + ") and id in (select Rentserverid from b2b_Rentserver_User_info where Userid=@Userid )) ";


            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@Userid", Userid);
            List<B2b_Rentserver> list = new List<B2b_Rentserver>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_Rentserver
                    {
                        id = reader.GetValue<int>("id"),
                        servername = reader.GetValue<string>("servername"),
                        renttype = reader.GetValue<string>("renttype"),
                        comid = reader.GetValue<int>("comid"),
                        WR = reader.GetValue<int>("WR"),
                        num = reader.GetValue<int>("num"),
                        posid = reader.GetValue<int>("posid"),
                        saleprice = reader.GetValue<decimal>("saleprice"),
                        serverDepositprice = reader.GetValue<decimal>("serverDepositprice"),
                        mustselect = reader.GetValue<int>("mustselect"),
                        servertype = reader.GetValue<int>("servertype")
                    });

                }
            }

            return list;

        }
        #endregion


        #region 服务查询用户服务表，查询此用户购买的服务
        internal List<B2b_Rentserver_User_info> SearchRentserver_User_alllist_state(int Userid)
        {

            string sqltxt = @"select a.id,a.verstate,c.servername from b2b_Rentserver_User_info as a left join b2b_eticket_Deposit as b on a.rentserverid=b.id left join b2b_Rentserver as c on b.sid=c.id where a.Userid=@Userid  ";


            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@Userid", Userid);
            List<B2b_Rentserver_User_info> list = new List<B2b_Rentserver_User_info>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_Rentserver_User_info
                    {
                        id = reader.GetValue<int>("id"),
                        servername = reader.GetValue<string>("servername"),
                        Verstate = reader.GetValue<int>("Verstate"),
                       
                    });

                }
            }

            return list;

        }
        #endregion


        #region 查询所有服务归还情况
        internal int SearchRentserver_count_state(int sid, string startime, string endtime)
        {

            string sqltxt = @"select  count(verstate) as verstatecount from b2b_eticket_Deposit as a left join b2b_Rentserver_User_info as b on a.id=b.Rentserverid where a.sid=@sid and a.subtime>='"+startime+"' and a.subtime<='"+endtime+"' ";


            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@sid", sid);
            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                       return  reader.GetValue<int>("verstatecount");//统计数量
                }
            }

            return 0;

        }
        #endregion

        #region 查询所有服务归还情况
        internal int SearchRentserver_weiguihuancount_state(int sid,string startime,string endtime)
        {

            string sqltxt = @"select count(verstate) as verstatecount from b2b_eticket_Deposit as a left join b2b_Rentserver_User_info as b on a.id=b.Rentserverid where a.sid=@sid and verstate=1 and a.subtime>='" + startime + "' and a.subtime<='" + endtime + "' ";


            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@sid", sid);
           
            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {

                    return reader.GetValue<int>("verstatecount");//统计数量

                }
            }

            return 0;

        }
        #endregion


        #region 服务查询用户服务表.购买服务
        internal List<B2b_Rentserver> SearchRentserverList_User_info(int Userid, string rentserverlist)
        {

            string sqltxt = @"select * from b2b_Rentserver_User_info where Userid=@Userid and Rentserverid in (select id from b2b_eticket_Deposit where sid=@Rentserverid) ";


            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@Userid", Userid);
            List<B2b_Rentserver> list = new List<B2b_Rentserver>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_Rentserver
                    {
                        id = reader.GetValue<int>("id"),
                        servername = reader.GetValue<string>("servername"),
                        renttype = reader.GetValue<string>("renttype"),
                        comid = reader.GetValue<int>("comid"),
                        WR = reader.GetValue<int>("WR"),
                        num = reader.GetValue<int>("num"),
                        posid = reader.GetValue<int>("posid"),
                        saleprice = reader.GetValue<decimal>("saleprice"),
                        serverDepositprice = reader.GetValue<decimal>("serverDepositprice"),
                        mustselect = reader.GetValue<int>("mustselect"),
                        servertype = reader.GetValue<int>("servertype")
                    });

                }
            }

            return list;

        }
        #endregion


        #region 服务查询用户服务表
        internal List<B2b_Rentserver_User_info> Rentserver_User_infopagelist(int pageindex, int pagesize, int Userid, out int totalcount)
        {

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "Userid=" + Userid;

            cmd.PagingCommand1("b2b_Rentserver_User_info", "*", "id", "", pagesize, pageindex, "", condition);

            List<B2b_Rentserver_User_info> list = new List<B2b_Rentserver_User_info>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_Rentserver_User_info
                    {
                        id = reader.GetValue<int>("id"),
                        Userid = reader.GetValue<int>("Userid"),
                        Rentserverid = reader.GetValue<int>("Rentserverid"),
                        Verstate = reader.GetValue<int>("Verstate"),
                        num = reader.GetValue<int>("num"),
                        Vertime = reader.GetValue<DateTime>("Vertime"),
                        Rettime = reader.GetValue<DateTime>("Rettime"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;

        }
        #endregion

        //冲正时，清除发卡 数据，以便重新验证发卡
        internal int Reverse_Rentserver_User(string pno)
        {
            //删除发出的卡所绑定的服务
            string sql = "delete b2b_Rentserver_User_info where Userid in (select id from b2b_Rentserver_User where eticketid in (select id from b2b_eticket where pno='" + pno + "' ))";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.ExecuteNonQuery();

            //删除所发出的卡
            string sql2 = "delete b2b_Rentserver_User where eticketid in (select id from b2b_eticket where pno='" + pno + "')";
            var cmd2 = sqlHelper.PrepareTextSqlCommand(sql2);
            return cmd2.ExecuteNonQuery();
        }



        internal string GetChipidByPrintNo(string printno)
        {
            printno = CommonFunc.Clearzero(printno);    //提取信息:去掉数字前的0

            string sql = "select cardchipid from b2b_Rentserver_printid_chipid where cardprintid='" + printno + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                string cardchipid = "";
                if (reader.Read())
                {
                    cardchipid = reader.GetValue<string>("cardchipid");
                }
                return cardchipid;
            }
        }


        internal string GetPrintNoByChipid(string Chipid)
        {
            string sql = "select cardprintid  from b2b_Rentserver_printid_chipid where cardchipid='" + Chipid + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                string cardprintid = "";
                if (reader.Read())
                {
                    cardprintid = reader.GetValue<int>("cardprintid").ToString();
                }
                return cardprintid;
            }
        }


        internal B2b_Rentserver GetRentServerById(int rentserverid)
        {
            string sqltxt = @"select * from b2b_Rentserver where id=@id";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", rentserverid);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_Rentserver r = null;
                if (reader.Read())
                {
                    r = new B2b_Rentserver
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        servername = reader.GetValue<string>("servername"),
                        num = reader.GetValue<int>("num"),
                        posid = reader.GetValue<int>("posid"),
                        WR = reader.GetValue<int>("wr"),
                    };
                }
                return r;
            }

        }




        //修改
        internal int uppro_worktime(int id, int comid, string title, string defaultendtime, string defaultstartime)
        {
            string sql = "";

            if (id == 0)
            {
                sql = "insert b2b_com_pro_worktime (comid,title,defaultendtime,defaultstartime)values(@comid,@title,@defaultendtime,@defaultstartime)";
            }
            else
            {
                sql = "update b2b_com_pro_worktime set comid =@comid,title=@title,defaultendtime=@defaultendtime,defaultstartime=@defaultstartime where id=@id";
            }


            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@title", title);
            cmd.AddParam("@defaultendtime", defaultendtime);
            cmd.AddParam("@defaultstartime", defaultstartime);


            return cmd.ExecuteNonQuery();
        }
        //修改
        internal int insertTimeoutmoney(b2b_Rentserver_user_Timeoutmoney timeout)
        {
            string sql = "insert b2b_Rentserver_user_Timeoutmoney (comid,subdate,subtime,oid,proid,userid,Timeoutmoney,TimeoutMinute)values(@comid,@subdate,@subtime,@oid,@proid,@userid,@Timeoutmoney,@TimeoutMinute)";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            cmd.AddParam("@comid", timeout.comid);
            cmd.AddParam("@subdate", timeout.subdate);
            cmd.AddParam("@subtime", timeout.subtime);
            cmd.AddParam("@oid", timeout.oid);
            cmd.AddParam("@proid", timeout.proid);
            cmd.AddParam("@userid", timeout.userid);
            cmd.AddParam("@Timeoutmoney", timeout.Timeoutmoney);
            cmd.AddParam("@TimeoutMinute", timeout.TimeoutMinute);

            return cmd.ExecuteNonQuery();
        }


        //删除
        internal int delpro_worktime(int id, int comid)
        {
            string sql = "";
            sql = "delete b2b_com_pro_worktime where comid=@comid and id=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);
            cmd.AddParam("@comid", comid);

            return cmd.ExecuteNonQuery();
        }


        #region 服务查询
        internal List<b2b_com_pro_worktime> pro_worktimepagelist(int pageindex, int pagesize, int comid, out int totalcount, int proid = 0)
        {

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "comid=" + comid;

            if (proid != 0)
            {
                //condition += " and id in(select sid from b2b_pro_rentserver where proid=" + proid + ") and servertype=0";//只读需要服务验证的判断

            }


            cmd.PagingCommand1("b2b_com_pro_worktime", "*", "id desc", "", pagesize, pageindex, "", condition);

            List<b2b_com_pro_worktime> list = new List<b2b_com_pro_worktime>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new b2b_com_pro_worktime
                    {
                        id = reader.GetValue<int>("id"),
                        title = reader.GetValue<string>("title"),
                        defaultendtime = reader.GetValue<string>("defaultendtime"),
                        defaultstartime = reader.GetValue<string>("defaultstartime"),
                        comid = reader.GetValue<int>("comid"),
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;

        }
        #endregion


        #region 服务查询
        internal b2b_com_pro_worktime pro_worktimebyid(int id, int comid)
        {

            string sqltxt = @"select * from b2b_com_pro_worktime where id=@id and comid=@comid ";

            var cmd = this.sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", id);
            cmd.AddParam("@comid", comid);
            b2b_com_pro_worktime r = null;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    r = new b2b_com_pro_worktime
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        title = reader.GetValue<string>("title"),
                        defaultendtime = reader.GetValue<string>("defaultendtime"),
                        defaultstartime = reader.GetValue<string>("defaultstartime"),
                    };

                }
                return r;
            }

        }
        #endregion




        internal List<b2b_com_pro_worktime_calendar> GetblackoutdatebyProWorktimeId(int proworktimeid, string datatype = "0")
        {
            string sql = "select * from b2b_com_pro_worktime_calendar where worktimeid=" + proworktimeid;
            if (datatype != "0")
            {
                if (datatype == "1")
                {
                    sql += "  and convert(varchar(10), setdate,120)>='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                }
                if (datatype == "2")
                {
                    sql += "  and convert(varchar(10), setdate,120)<'" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                }
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<b2b_com_pro_worktime_calendar> list = new List<b2b_com_pro_worktime_calendar>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new b2b_com_pro_worktime_calendar
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        setdate = reader.GetValue<DateTime>("setdate"),
                        worktimeid = reader.GetValue<int>("worktimeid"),
                        endtime = reader.GetValue<string>("endtime"),
                        startime = reader.GetValue<string>("startime"),
                    });
                }
            }
            return list;
        }

        internal b2b_com_pro_worktime_calendar GetblackoutdateByWorktimeId(string daydate, int proworktimeid)
        {
            string sql = "select * from b2b_com_pro_worktime_calendar where setdate='" + daydate + "' and worktimeid=" + proworktimeid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                b2b_com_pro_worktime_calendar m = null;
                if (reader.Read())
                {
                    m = new b2b_com_pro_worktime_calendar
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        setdate = reader.GetValue<DateTime>("setdate"),
                        worktimeid = reader.GetValue<int>("worktimeid"),
                        endtime = reader.GetValue<string>("endtime"),
                        startime = reader.GetValue<string>("startime"),
                    };
                }
                return m;
            }
        }

        internal int DelblackoutdateByWorktimeId(string daydate, int proworktimeid)
        {
            string sql = "delete   b2b_com_pro_worktime_calendar where setdate='" + daydate + "' and worktimeid=" + proworktimeid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int Insworktimeblackoutdates(b2b_com_pro_worktime_calendar model)
        {
            string sql = "INSERT INTO b2b_com_pro_worktime_calendar ( comid,worktimeid,setdate,startime,endtime) VALUES(" + model.comid + "," + model.worktimeid + ",'" + model.setdate + "','" + model.startime + "','" + model.endtime + "')";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int Delworktimeusesetbyworktimeid(int proworktimeid)
        {
            string sql = "delete b2b_com_pro_worktime_calendar where worktimeid=" + proworktimeid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }


        internal List<b2b_Rentserver_user_Timeoutmoney> GetserverTimeoutPagelist(int comid, int pageindex, int pagesize, string startime, string endtime, string key, out int totalcount)
        {
            string condition = " comid=" + comid;

            if (startime != "" && endtime != "")
            {
                startime = DateTime.Parse(startime).ToString("yyyy-MM-dd 00:00:00");
                endtime = DateTime.Parse(endtime).ToString("yyyy-MM-dd 23:59:59");
                condition += " and subtime between '" + startime + "' and '" + endtime + "'";
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
                condition += " and oid in (select id from b2b_order where U_name='" + key + "' or U_phone='" + key + "' or id=" + key_int + " or '" + key + "' in (pno) or pro_id in (select id from b2b_com_pro where pro_name like '%" + key + "%'))";
            }

            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            cmd.PagingCommand1("b2b_Rentserver_user_Timeoutmoney", "*", "id desc", "", pagesize, pageindex, "", condition);

            List<b2b_Rentserver_user_Timeoutmoney> list = new List<b2b_Rentserver_user_Timeoutmoney>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new b2b_Rentserver_user_Timeoutmoney
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        subdate = reader.GetValue<DateTime>("subdate"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                        oid = reader.GetValue<int>("oid"),
                        userid = reader.GetValue<int>("userid"),
                        proid = reader.GetValue<int>("proid"),
                        Timeoutmoney = reader.GetValue<decimal>("Timeoutmoney"),
                        TimeoutMinute = reader.GetValue<int>("TimeoutMinute")
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }

        //
        internal List<B2b_com_pro> serverSuodaoPagelist(int comid, string startime, string endtime, string key)
        {
            
            string condition = "select a.pro_name,sum(c.use_pnum) as num from b2b_com_pro as a left join b2b_eticket as b on a.id=b.pro_id left join b2b_etcket_log as c on b.id=c.eticket_id where a.com_id=" + comid + " and a.worktimehour !=0 and  c.Action=1 and c.a_state=1 ";

            if (startime != "")
            {
                condition += " and c.actiondate>='" + startime + "'";
            }

            if (endtime != "")
            {
                condition += " and c.actiondate<'" + endtime + "'";
            }

            condition += " group by a.pro_name";
            var cmd = sqlHelper.PrepareTextSqlCommand(condition);
            List<B2b_com_pro> list = new List<B2b_com_pro>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_com_pro
                    {
                        Pro_name = reader.GetValue<string>("pro_name"),
                        U_num = reader.GetValue<int>("num"),
                    });
                }
            }
            return list;
        }


        internal decimal GetServerTimeoutMoney(int comid, string startime, string endtime, string key)
        {
            string condition = " comid=" + comid;

            if (startime != "" && endtime != "")
            {
                startime = DateTime.Parse(startime).ToString("yyyy-MM-dd 00:00:00");
                endtime = DateTime.Parse(endtime).ToString("yyyy-MM-dd 23:59:59");
                condition += " and subtime between '" + startime + "' and '" + endtime + "'";
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
                condition += " and oid in (select id from b2b_order where U_name='" + key + "' or U_phone='" + key + "' or id=" + key_int + " or '" + key + "' in (pno) or pro_id in (select id from b2b_com_pro where pro_name like '%" + key + "%'))";
            }

            string sql = "select sum(Timeoutmoney) as summoney from b2b_Rentserver_user_Timeoutmoney where " + condition;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                decimal r = 0;
                if (reader.Read())
                {
                    r = reader.GetValue<decimal>("summoney");
                }
                return r;
            }
        }

        internal List<B2b_Rentserver_User> GetserverfakaPagelist(int comid, int pageindex, int pagesize, string startime, string endtime, string key, int serverstate, int serverid, out int totalcount)
        {
            string condition = " comid=" + comid;

            if (startime != "" && endtime != "")
            {
                startime = DateTime.Parse(startime).ToString("yyyy-MM-dd 00:00:00");
                endtime = DateTime.Parse(endtime).ToString("yyyy-MM-dd 23:59:59");
                condition += " and subtime between '" + startime + "' and '" + endtime + "'";
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
                condition += " and oid in (select id from b2b_order where U_name='" + key + "' or U_phone='" + key + "' or id=" + key_int + " or '" + key + "' in (pno) or pro_id in (select id from b2b_com_pro where pro_name like '%" + key + "%'))";
            }
            if (serverstate == 1)
            {
                condition += " and serverstate=" + serverstate + " and endtime>='" + DateTime.Now.ToString("yyyy-MM-dd") + "'"; ;
            }
            if (serverstate == 2)
            {
                condition += " and serverstate=" + serverstate + " and endtime<'"+DateTime.Now.ToString("yyyy-MM-dd")+"'";
            }


            if (serverid != 0)
            {
                condition += " and oid in (select oid  from b2b_eticket where id in (select eticketid from b2b_eticket_Deposit where sid=" + serverid + "))";
            }


            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            cmd.PagingCommand1("B2b_Rentserver_User", "*", "id desc", "", pagesize, pageindex, "", condition);

            List<B2b_Rentserver_User> list = new List<B2b_Rentserver_User>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_Rentserver_User
                    {
                        id = reader.GetValue<int>("id"),
                        comid = reader.GetValue<int>("comid"),
                        oid = reader.GetValue<int>("oid"),
                        cardid = reader.GetValue<string>("cardid"),
                        Depositstate = reader.GetValue<int>("Depositstate"),
                        serverstate = reader.GetValue<int>("Serverstate"),
                        Depositprice = reader.GetValue<decimal>("Depositprice"),
                        subtime = reader.GetValue<DateTime>("subtime"),
                        Depositorder = reader.GetValue<int>("Depositorder"),
                        Depositcome = reader.GetValue<string>("Depositcome"),
                        eticketid = reader.GetValue<int>("eticketid"),
                        cardchipid = reader.GetValue<string>("cardchipid"),
                        endtime = reader.GetValue<DateTime>("endtime"),
                        pname = reader.GetValue<string>("pname"),
                        sendnum = reader.GetValue<int>("sendnum"),
                        sendstate = reader.GetValue<int>("sendstate"),
                       
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }

        internal int GetServerUsageCount(int comid, string startime, string endtime, string key, int serverstate, int serverid)
        {
            string condition = " comid=" + comid;

            if (startime != "" && endtime != "")
            {
                startime = DateTime.Parse(startime).ToString("yyyy-MM-dd 00:00:00");
                endtime = DateTime.Parse(endtime).ToString("yyyy-MM-dd 23:59:59");
                condition += " and subtime between '" + startime + "' and '" + endtime + "'";
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
                condition += " and oid in (select id from b2b_order where U_name='" + key + "' or U_phone='" + key + "' or id=" + key_int + " or '" + key + "' in (pno) or pro_id in (select id from b2b_com_pro where pro_name like '%" + key + "%'))";
            }
            if (serverstate != -1)
            {
                condition += " and serverstate=" + serverstate;
            }
            if (serverid != 0)
            {
                condition += " and oid in (select oid  from b2b_eticket where id in (select eticketid from b2b_eticket_Deposit where sid=" + serverid + "))";
            }

            string sql = "select count(1) as num from B2b_Rentserver_User where " + condition;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int notlingqunum = 0;
                if (reader.Read())
                {
                    notlingqunum = reader.GetValue<int>("num");
                }
                return notlingqunum;
            }
        }

        #region 服务查询
        internal List<B2b_pro_cost_rili> procostrilipagelist(int pageindex, int pagesize, int comid, int proid, out int totalcount)
        {

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "comid=" + comid + " and proid="+proid;

            cmd.PagingCommand1("B2b_pro_cost_rili", "*", "id desc", "", pagesize, pageindex, "", condition);

            List<B2b_pro_cost_rili> list = new List<B2b_pro_cost_rili>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_pro_cost_rili
                    {
                        id = reader.GetValue<int>("id"),
                        proid = reader.GetValue<int>("proid"),
                        costprice = reader.GetValue<decimal>("costprice"),
                        stardate = reader.GetValue<DateTime>("stardate").ToString("yyyy-MM-dd"),
                        enddate = reader.GetValue<DateTime>("enddate").ToString("yyyy-MM-dd"),
                        comid = reader.GetValue<int>("comid"),
                        admin = reader.GetValue<string>("admin"),
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;

        }
        #endregion

        #region 服务查询
        internal B2b_pro_cost_rili procostrilibyid( int comid, int id)
        {

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "comid=" + comid + " and id=" + id;

            cmd.PagingCommand1("B2b_pro_cost_rili", "*", "id desc", "", 1, 1, "", condition);

            B2b_pro_cost_rili list = null;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    list=new B2b_pro_cost_rili()
                    {
                        id = reader.GetValue<int>("id"),
                        proid = reader.GetValue<int>("proid"),
                        costprice = reader.GetValue<decimal>("costprice"),
                        stardate = reader.GetValue<DateTime>("stardate").ToString("yyyy-MM-dd"),
                        enddate = reader.GetValue<DateTime>("enddate").ToString("yyyy-MM-dd"),
                        comid = reader.GetValue<int>("comid"),
                        admin = reader.GetValue<string>("admin"),
                    };
                }
            }

            return list;

        }
        #endregion

        #region 查询产品日历最后一个日期，必须比此日期大一天
        internal string prolastcostrilibyid(int comid, int proid,int id)
        {
            var sql = "select top 1 * from B2b_pro_cost_rili where proid="+proid+ " and  comid=" + comid ;
            if(id !=0){
                sql += " and not id="+id+" and id<"+id;
            }
            sql += " order by id desc";
            
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                       return reader.GetValue<DateTime>("enddate").ToString("yyyy-MM-dd");
                }
            }

            return "";

        }
        #endregion

        #region 对同一个产品，一个日期不能出现两次
        internal string produibicostrili(int comid, int proid,string stardate,string enddate)
        {
            var sql = "select * from B2b_pro_cost_rili where proid=" + proid + " and  comid=" + comid + " and DATEDIFF(d,'" + stardate + "',stardate)<=0 and DATEDIFF(d,'" + stardate + "',enddate)>0";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return stardate;
                }
            }


            var sql2 = "select * from B2b_pro_cost_rili where proid=" + proid + " and  comid=" + comid + " and DATEDIFF(d,'" + enddate + "',stardate)<=0 and DATEDIFF(d,'" + enddate + "',enddate)>0";

            var cmd2 = sqlHelper.PrepareTextSqlCommand(sql2);
            using (var reader2 = cmd2.ExecuteReader())
            {
                if (reader2.Read())
                {
                    return enddate;
                }
            }

            return "";

        }
        #endregion


        internal int upprocostrili(B2b_pro_cost_rili costrili)
        {
            string sql = "insert B2b_pro_cost_rili (proid,comid,costprice,stardate,enddate,admin)values(" + costrili.proid + "," + costrili.comid + "," + costrili.costprice + ",'" + costrili.stardate + "','" + costrili.enddate + "','" + costrili.admin+ "')";

            if (costrili.id != 0) {
                sql = "update B2b_pro_cost_rili set proid=" + costrili.proid + ",comid=" + costrili.comid + ",costprice=" + costrili.costprice + ",stardate='" + costrili.stardate + "',enddate='" + costrili.enddate + "',admin='" + costrili.admin + "' where id="+costrili.id;
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int delcostrili(int comid,int id)
        {
            string sql = "delete B2b_pro_cost_rili where comid=" + comid +" and id="+id;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }





        #region 服务查询
        internal List<B2b_project_finance> projectfinancepagelist(int pageindex, int pagesize, int comid, int projectid, string startime, string endtime, out int totalcount)
        {

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "comid=" + comid + " and projectid=" + projectid;

            if (startime != "") {
                condition += " and subdate>='" + startime + "'";
            }

            if (endtime != "")
            {
                endtime = endtime + " 23:59:59";
                condition += " and subdate<='" + endtime + "'";
            }

            cmd.PagingCommand1("B2b_project_finance", "*", "id desc", "", pagesize, pageindex, "", condition);

            List<B2b_project_finance> list = new List<B2b_project_finance>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_project_finance
                    {
                        id = reader.GetValue<int>("id"),
                        Projectid = reader.GetValue<int>("Projectid"),
                        Money = reader.GetValue<decimal>("Money"),
                        Subdate = reader.GetValue<DateTime>("Subdate"),
                        Remarks = reader.GetValue<string>("Remarks"),
                        comid = reader.GetValue<int>("comid"),
                        admin = reader.GetValue<string>("admin"),
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;

        }
        #endregion

        #region 服务查询
        internal decimal projectfinancesum(int comid, int projectid, string startime, string endtime)
        {

            var sql = "select sum(money) as money from B2b_project_finance where projectid=" + projectid + " and  comid=" + comid;
            if (startime != "")
            {
                sql += " and subdate>='" + startime + "'";
            }

            if (endtime != "")
            {
                endtime = endtime + " 23:59:59";
                sql += " and subdate<='" + endtime + "'";
            }

            sql += " group by projectid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<decimal>("money");
                }
            }

            return 0;

        }
        #endregion

        internal int upprojectfinance(B2b_project_finance project_finance)
        {
            string sql = "insert B2b_project_finance (Projectid,comid,money,Remarks,subdate,admin)values(" + project_finance.Projectid + "," + project_finance.comid + "," + project_finance.Money + ",'" + project_finance.Remarks + "','" + DateTime.Now + "','" + project_finance.admin + "')";

            if (project_finance.id != 0)
            {
                sql = "update B2b_project_finance set Projectid=" + project_finance.Projectid + ",comid=" + project_finance.comid + ",Money=" + project_finance.Money + ",Remarks='" + project_finance.Remarks + "',subdate='" + DateTime.Now + "',admin='" + project_finance.admin + "' where id=" + project_finance.id;
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;
using System.Data.SqlClient;
using ETS2.CRM.Service.CRMService.Modle;

using ETS.Framework;
using ETS2.Common.Business;
using System.Collections;


namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internal_b2b_com_project
    {
        public SqlHelper sqlHelper;
        public Internal_b2b_com_project(SqlHelper sqlhelper)
        {
            this.sqlHelper = sqlhelper;
        }

        internal int EditProject(Modle.B2b_com_project model)
        {
            if (model.Id == 0)
            {
                string sql = "INSERT INTO [EtownDB].[dbo].[b2b_com_project] " +
           "([projectname] ,[projectimg] ,[province],[city],[industryid],[briefintroduce] ,[address],[mobile],[coordinate] ,[serviceintroduce] ,[onlinestate],comid,createtime,createuserid,hotelset,grade,star,parking,cu)" +
           "  VALUES ('" + model.Projectname + "'," + model.Projectimg + " ,'" + model.Province + "','" + model.City + "' ," + model.Industryid + " ,'" + model.Briefintroduce + "' ,'" + model.Address + "' ,'" + model.Mobile + "','" + model.Coordinate + "' ,'" + model.Serviceintroduce + "','" + model.Onlinestate + "'," + model.Comid + ",'" + model.Createtime.ToString("yyyy-MM-dd HH:mm:ss") + "'," + model.Createuserid + "," + model.hotelset + "," + model.grade + ",'" + model.star + "'," + model.parking + ",'" + model.cu + "');select @@identity;";

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            else
            {
                //如果项目下线，则项目下面的产品下线
                string selsql = "select onlinestate from  b2b_com_project where id=" + model.Id;
                var selcmd = sqlHelper.PrepareTextSqlCommand(selsql);
                object o = selcmd.ExecuteScalar();
                if (o.ToString() == "1")
                {
                    if (model.Onlinestate == "0")
                    {
                        string downlinesql = "update b2b_com_pro set pro_state=0 where projectid=" + model.Id + " and projectid>0";
                        var downlinecmd = sqlHelper.PrepareTextSqlCommand(downlinesql);
                        downlinecmd.ExecuteNonQuery(); 
                    }
                }


                string sql = "UPDATE [EtownDB].[dbo].[b2b_com_project] " +
   "SET [projectname] ='" + model.Projectname + "',[projectimg] = '" + model.Projectimg + "',[province] = '" + model.Province + "',[city] = '" + model.City + "',[industryid] =" + model.Industryid + " ,[briefintroduce] ='" + model.Briefintroduce + "',[address] ='" + model.Address + "',[mobile] ='" + model.Mobile + "',[coordinate] ='" + model.Coordinate + "' ,[serviceintroduce] = '" + model.Serviceintroduce + "',[onlinestate] = '" + model.Onlinestate + "',hotelset= " + model.hotelset + ",grade= " + model.grade + ",star= '" + model.star + "',parking= " + model.parking + ",cu= '" + model.cu + "' WHERE id=" + model.Id;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.ExecuteNonQuery();
                sqlHelper.Dispose();
                return model.Id;
            }
        }

        internal B2b_com_project GetProject(int projectid, int comid)
        {
            string sql = "SELECT * FROM [EtownDB].[dbo].[b2b_com_project] where id=@projectid and comid=@comid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@projectid", projectid);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_com_project u = null;
                if (reader.Read())
                {
                    u = new B2b_com_project()
                    {
                        Id = reader.GetValue<int>("id"),
                        Projectname = reader.GetValue<string>("projectname"),
                        Projectimg = reader.GetValue<int>("projectimg"),
                        Province = reader.GetValue<string>("province"),
                        City = reader.GetValue<string>("city"),
                        Industryid = reader.GetValue<int>("industryid"),
                        Briefintroduce = reader.GetValue<string>("briefintroduce"),
                        Address = reader.GetValue<string>("address"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Coordinate = reader.GetValue<string>("coordinate"),
                        Serviceintroduce = reader.GetValue<string>("serviceintroduce"),
                        Onlinestate = reader.GetValue<string>("onlinestate"),
                        Comid = reader.GetValue<int>("comid"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        hotelset = reader.GetValue<int>("hotelset"),
                        grade = reader.GetValue<decimal>("grade"),
                        star = reader.GetValue<string>("star"),
                        parking = reader.GetValue<int>("parking"),
                        cu = reader.GetValue<string>("cu"),
                    };
                }
                return u;
            }

        }

        internal string GetProjectname(int projectid)
        {
            string sql = "SELECT [projectname] FROM [EtownDB].[dbo].[b2b_com_project] where id=@projectid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@projectid", projectid);

            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    return reader.GetValue<string>("projectname");
                }
                return "";
            }

        }

        internal int GetProjectBindingid(int projectid)
        {
            string sql = "SELECT  bindingprojectid FROM [EtownDB].[dbo].[b2b_com_project] where id=@projectid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@projectid", projectid);

            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    return reader.GetValue<int>("bindingprojectid");
                }
                return 0;
            }

        }

        internal List<B2b_com_project> Projectlist(int pageindex, int pagesize, int agentid, out int totalcount, string key = "")
        {

            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = " id in (select projectid from b2b_project_agent where agentid=" + agentid + " )";

            if (key != "")
            {
                condition += " and Projectname like '%" + key + "%' ";
            }

            cmd.PagingCommand1("b2b_com_project", "*", "id desc", "", pagesize, pageindex, "", condition);

            List<B2b_com_project> list = new List<B2b_com_project>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_project()
                    {
                        Id = reader.GetValue<int>("id"),
                        Projectname = reader.GetValue<string>("projectname"),
                        Projectimg = reader.GetValue<int>("projectimg"),
                        Province = reader.GetValue<string>("province"),
                        City = reader.GetValue<string>("city"),
                        Industryid = reader.GetValue<int>("industryid"),
                        Briefintroduce = reader.GetValue<string>("briefintroduce"),
                        Address = reader.GetValue<string>("address"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Coordinate = reader.GetValue<string>("coordinate"),
                        Serviceintroduce = reader.GetValue<string>("serviceintroduce"),
                        Onlinestate = reader.GetValue<string>("onlinestate"),
                        Comid = reader.GetValue<int>("comid"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        hotelset = reader.GetValue<int>("hotelset"),
                        grade = reader.GetValue<decimal>("grade"),
                        star = reader.GetValue<string>("star"),
                        parking = reader.GetValue<int>("parking"),
                        cu = reader.GetValue<string>("cu"),
                    });
                }

            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;

        }
        internal List<Agent_regiinfo> ProjectAgentlist(int pageindex, int pagesize, int comid, int projectid, out int totalcount)
        {

            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "agent_sort=2 and id in (select agentid from b2b_project_agent where comid=" + comid + " and projectid=" + projectid + ")";

            cmd.PagingCommand1("agent_company", "*", "id desc", "", pagesize, pageindex, "", condition);

            List<Agent_regiinfo> list = new List<Agent_regiinfo>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Agent_regiinfo()
                    {
                        Id = reader.GetValue<int>("id"),
                        Company = reader.GetValue<string>("Company"),
                        Mobile = reader.GetValue<string>("Mobile"),
                        Contentname = reader.GetValue<string>("Name"),

                    });
                }

            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;

        }

        //获取所有分销商电话
        internal string GetAgentPhonelist(int comid)
        {
            string phone = "";
            string sql = "select mobile from agent_company  where id in (select agentid from agent_warrant where warrant_state=1 and comid=" + comid + ") ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<B2b_com_project> list = new List<B2b_com_project>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    phone += reader.GetValue<string>("mobile") + ",";
                }

            }
            return phone;

        }

        internal IList<B2b_com_project> Projectpagelist(string comid, int pageindex, int pagesize, string projectstate, out  int totalcount, string key = "", int runpro = 0, int Projectpagelist=0, int servertype = 0)
        {

            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "comid=" + comid;
            if (key != "")
            {
                condition += " and projectname like '%" + key + "%'";
            }
            if (projectstate != "0,1")
            {
                condition += " and onlinestate in (" + projectstate + ")";
            }

            if (Projectpagelist != 0) {
                condition += " and id in (" + Projectpagelist + ")";
            }

            if (runpro == 1)
            {
                if (servertype != 0)
                {
                    condition += " and id in ( select projectid from b2b_com_pro where com_id=" + comid + " and pro_state=1 and server_type=" + servertype + ")";
                }
                else
                {
                    condition += " and id in ( select projectid from b2b_com_pro where com_id=" + comid + " and pro_state=1 )";
                }
            }
            else
            {
                if (servertype != 0)
                {
                    condition += " and id in ( select projectid from b2b_com_pro where com_id=" + comid + " and server_type=" + servertype + " )";
                }
            }

            cmd.PagingCommand1("b2b_com_project", "*", "sortid ,id desc", "", pagesize, pageindex, "", condition);

            List<B2b_com_project> list = new List<B2b_com_project>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_project()
                    {
                        Id = reader.GetValue<int>("id"),
                        Projectname = reader.GetValue<string>("projectname"),
                        Projectimg = reader.GetValue<int>("projectimg"),
                        Imgaddress = FileSerivce.GetImgUrl(reader.GetValue<int>("projectimg")),
                        Province = reader.GetValue<string>("province"),
                        City = reader.GetValue<string>("city"),
                        Industryid = reader.GetValue<int>("industryid"),
                        Briefintroduce = reader.GetValue<string>("briefintroduce"),
                        Address = reader.GetValue<string>("address"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Coordinate = reader.GetValue<string>("coordinate"),
                        Serviceintroduce = reader.GetValue<string>("serviceintroduce"),
                        Onlinestate = reader.GetValue<string>("onlinestate"),
                        Comid = reader.GetValue<int>("comid"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        hotelset = reader.GetValue<int>("hotelset"),
                        grade = reader.GetValue<decimal>("grade"),
                        star = reader.GetValue<string>("star"),
                        parking = reader.GetValue<int>("parking"),
                        cu = reader.GetValue<string>("cu"),
                        minprice =new B2b_com_housetypeData().GetHousetypeNowdaypricebyprojectid(reader.GetValue<int>("id")).ToString("0.00")//今天以后的最低价格
                    });
                }



            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }


        internal List<B2b_com_pro> ProlistbyProjectid(int pageindex, int pagesize, int agentid, int projectid, out int totalcount)
        {

            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "projectid in (select projectid from b2b_project_agent where agentid=" + agentid + ")";
            if (projectid != 0)
            {
                condition += " and projectid=" + projectid;
            }

            cmd.PagingCommand1("b2b_com_pro", "*", "id desc", "", pagesize, pageindex, "", condition);

            List<B2b_com_pro> list = new List<B2b_com_pro>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_pro()
                    {
                        Id = reader.GetValue<int>("id"),
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
                    });
                }


            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }


        internal int GetProjectCountByComId(int comid)
        {
            string sql = "select count(1) from b2b_com_project where comid=" + comid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            object o = cmd.ExecuteScalar();
            return o == null ? 0 : int.Parse(o.ToString());
        }

        internal string GetProjectNameByid(int projectid)
        {
            string sql = "SELECT  [projectname]    FROM [EtownDB].[dbo].[b2b_com_project] where id=" + projectid;
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                object o = cmd.ExecuteScalar();
                sqlHelper.Dispose();
                return o == null ? "" : o.ToString();
            }
            catch (Exception e)
            {
                sqlHelper.Dispose();
                return "";
            }
        }
        //根据产品ID得到项目ID
        internal int GetProjectidByproid(int proid)
        {
            string sql = "SELECT  projectid FROM.[b2b_com_pro] where id=" + proid;
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

        //prosort 判断排序方式 是按排序列还是按添加顺序
        internal IList<B2b_com_project> Projectpagelist(string comid, string projectstate, out int totalcount, int prosort = 0)
        {
            string sql = "select * from b2b_com_project  where comid=" + comid + " and onlinestate in (" + projectstate + ")  ";

            if (prosort == 0)
            {
                sql += " order by sortid";
            }
            else
            {
                sql += " order by Projectname";
            }


            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<B2b_com_project> list = new List<B2b_com_project>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_project()
                    {
                        Id = reader.GetValue<int>("id"),
                        Projectname = reader.GetValue<string>("projectname"),
                        Projectimg = reader.GetValue<int>("projectimg"),
                        Province = reader.GetValue<string>("province"),
                        City = reader.GetValue<string>("city"),
                        Industryid = reader.GetValue<int>("industryid"),
                        Briefintroduce = reader.GetValue<string>("briefintroduce"),
                        Address = reader.GetValue<string>("address"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Coordinate = reader.GetValue<string>("coordinate"),
                        Serviceintroduce = reader.GetValue<string>("serviceintroduce"),
                        Onlinestate = reader.GetValue<string>("onlinestate"),
                        Comid = reader.GetValue<int>("comid"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Createuserid = reader.GetValue<int>("createuserid")
                    });
                }

            }
            totalcount = list.Count;
            return list;
        }

        internal List<B2b_com_project> ProjectSelectpagelist(int projectstate, int pageindex, int pagesize, string key, out int totalcount, int proclass, string comid, int projectid = 0, int price = 0)
        {





            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            var condition = "comid=" + comid + " and  Onlinestate=" + projectstate;
            if (key != "")
            {
                condition += " and (projectname like '%" + key + "%' or   province like '%" + key + "%' or city  like '%" + key + "%' or id in (select projectid from b2b_com_pro where id in (select lineid from b2b_com_trvalending where ending='" + key + "' )))";
            }
            if (proclass != 0)
            {//按类目查询
                condition += " and id in (select projectid from b2b_com_pro where id in (select proid from b2b_com_pro_class where classid=" + proclass + "  ))";
            }

            if (projectid != 0)
            {
                condition += " and id=" + projectid;
            }
            if (price != 0)
            {
                if (price == 1)
                {
                    condition += "and id in (select projectid from b2b_com_pro where advise_price<=50 and pro_state=1 )";
                }
                else if (price == 2)
                {
                    condition += "and id in (select projectid from b2b_com_pro where advise_price>=50 and  advise_price<=50 and pro_state=1  )";
                }
                else if (price == 3)
                {
                    condition += "and id in (select projectid from b2b_com_pro where advise_price>=100  and pro_state=1  )";
                }
            }



            cmd.PagingCommand1("b2b_com_project", "*", "id desc", "", pagesize, pageindex, "", condition);

            List<B2b_com_project> list = new List<B2b_com_project>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_project()
                    {
                        Id = reader.GetValue<int>("id"),
                        Projectname = reader.GetValue<string>("projectname"),
                        Projectimg = reader.GetValue<int>("projectimg"),
                        Province = reader.GetValue<string>("province"),
                        City = reader.GetValue<string>("city"),
                        Industryid = reader.GetValue<int>("industryid"),
                        Briefintroduce = reader.GetValue<string>("briefintroduce"),
                        Address = reader.GetValue<string>("address"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Coordinate = reader.GetValue<string>("coordinate"),
                        Serviceintroduce = reader.GetValue<string>("serviceintroduce"),
                        Onlinestate = reader.GetValue<string>("onlinestate"),
                        Comid = reader.GetValue<int>("comid"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Createuserid = reader.GetValue<int>("createuserid")
                    });
                }

            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }



        internal int SortMenu(string menuid, int sortid)
        {
            string sql = "update b2b_com_project set sortid=@sortid where id =@id ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", menuid);
            cmd.AddParam("@sortid", sortid);

            return cmd.ExecuteNonQuery();
        }

        internal B2b_com_project GetProject(int projectid)
        {
            string sql = "SELECT createtime,createuserid, [id],[projectname] ,[projectimg],[province],[city],[industryid],[briefintroduce],[address],[mobile],[coordinate],[serviceintroduce],[onlinestate],comid,bindingprojectid FROM  [b2b_com_project] where id=@projectid  ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@projectid", projectid);


            using (var reader = cmd.ExecuteReader())
            {
                B2b_com_project u = null;
                if (reader.Read())
                {
                    u = new B2b_com_project()
                    {
                        Id = reader.GetValue<int>("id"),
                        Projectname = reader.GetValue<string>("projectname"),
                        Projectimg = reader.GetValue<int>("projectimg"),
                        Province = reader.GetValue<string>("province"),
                        City = reader.GetValue<string>("city"),
                        Industryid = reader.GetValue<int>("industryid"),
                        Briefintroduce = reader.GetValue<string>("briefintroduce"),
                        Address = reader.GetValue<string>("address"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Coordinate = reader.GetValue<string>("coordinate"),
                        Serviceintroduce = reader.GetValue<string>("serviceintroduce"),
                        Onlinestate = reader.GetValue<string>("onlinestate"),
                        Comid = reader.GetValue<int>("comid"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Createuserid = reader.GetValue<int>("createuserid"),
                        bindingprojectid = reader.GetValue<int>("bindingprojectid"),
                    };
                }
                return u;
            }

        }

        //判断项目是否含有 预订产品，如果有 列表页面 显示出项目信息
        internal int GetProjectUnyuding(int projectid)
        {
            string sql = "SELECT top 1 id FROM  [b2b_com_project] where id=@projectid and id in (select projectid from b2b_com_pro where server_type=12) ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@projectid", projectid);
            int unyuding = 0;
            try
            {
                using (var reader = cmd.ExecuteReader())
                {
                    B2b_com_project u = null;
                    if (reader.Read())
                    {
                        unyuding = reader.GetValue<int>("id");
                    }
                    return unyuding;
                }
            }
            catch
            {
                return 0;
            }

        }

        internal List<B2b_com_project> GetProjectlistByComid(int comid)
        {
            string sql = "select * from b2b_com_project where comid=" + comid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<B2b_com_project> list = new List<B2b_com_project>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_com_project()
                    {
                        Id = reader.GetValue<int>("id"),
                        Projectname = reader.GetValue<string>("projectname"),
                        Projectimg = reader.GetValue<int>("projectimg"),
                        Province = reader.GetValue<string>("province"),
                        City = reader.GetValue<string>("city"),
                        Industryid = reader.GetValue<int>("industryid"),
                        Briefintroduce = reader.GetValue<string>("briefintroduce"),
                        Address = reader.GetValue<string>("address"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Coordinate = reader.GetValue<string>("coordinate"),
                        Serviceintroduce = reader.GetValue<string>("serviceintroduce"),
                        Onlinestate = reader.GetValue<string>("onlinestate"),
                        Comid = reader.GetValue<int>("comid"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Createuserid = reader.GetValue<int>("createuserid")
                    });
                }
            }
            return list;
        }

        internal List<B2b_com_project> Selhotelprojectlist(int comid)
        {
            string sql = "select * from b2b_com_project where comid=" + comid + " and id in (select projectid from b2b_com_pro where comid=" + comid + " and server_type=9)";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<B2b_com_project> list = new List<B2b_com_project>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_com_project()
                    {
                        Id = reader.GetValue<int>("id"),
                        Projectname = reader.GetValue<string>("projectname"),
                        Projectimg = reader.GetValue<int>("projectimg"),
                        Province = reader.GetValue<string>("province"),
                        City = reader.GetValue<string>("city"),
                        Industryid = reader.GetValue<int>("industryid"),
                        Briefintroduce = reader.GetValue<string>("briefintroduce"),
                        Address = reader.GetValue<string>("address"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Coordinate = reader.GetValue<string>("coordinate"),
                        Serviceintroduce = reader.GetValue<string>("serviceintroduce"),
                        Onlinestate = reader.GetValue<string>("onlinestate"),
                        Comid = reader.GetValue<int>("comid"),
                        Createtime = reader.GetValue<DateTime>("createtime"),
                        Createuserid = reader.GetValue<int>("createuserid")
                    });
                }
            }
            return list;
        }


        internal List<B2b_com_project> Selprojectlist(int comid)
        {
            string sql = "select * from b2b_com_project where comid=" + comid + " and Onlinestate=1 order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<B2b_com_project> list = new List<B2b_com_project>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_com_project()
                    {
                        Id = reader.GetValue<int>("id"),
                        Projectname = reader.GetValue<string>("projectname"),
                    });
                }
            }
            return list;
        }
    }
}

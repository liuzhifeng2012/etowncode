using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;
using System.Data;
using ETS2.PM.Service.PMService.Modle;



namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalAgentCompany
    {

        private SqlHelper sqlHelper;
        public InternalAgentCompany(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }


        #region 授权项目列表


        internal List<B2b_com_project> WarrantProjectlist(int Pageindex, int Pagesize, string Key, int Agentid, int Warrant_type, int comid, out int totalcount, int Agentlevel = 0)
        {

            B2bComProData prodata = new B2bComProData();

            var condition = " id in (select projectid from b2b_com_pro where com_id in (select comid from agent_warrant where agentid = " + Agentid + " and comid = " + comid + " and warrant_state=1)  and pro_state=1";

            if (Warrant_type == 2)
            {
                condition = condition + " and not source_type=3";//如果是倒码授权分销不显示接口产品，因接口产品无法倒码
            }
            if (Agentlevel != 0)
            {//判断分销，根据分销级别，判断这个级别分销价格为0则不显示
                if (Agentlevel == 1)
                {
                    condition = condition + " and (   agent1_price>0 or server_type in (2,8) or id in (select proid from b2b_com_pro_Speci where speci_agent1_price>0) )";//1级分销不能=0
                }
                if (Agentlevel == 2)
                {
                    condition = condition + " and (   agent2_price>0 or server_type in (2,8) or id in (select proid from b2b_com_pro_Speci where speci_agent1_price>0)  )";//2级分销不能=0
                }
                if (Agentlevel == 3)
                {
                    condition = condition + " and (   agent3_price>0 or server_type in (2,8) or id in (select proid from b2b_com_pro_Speci where speci_agent1_price>0) )";//3级分销不能=0
                }
            }
            if (Key != "")
            {
                condition = condition + " and pro_name like '%" + Key + "%'";//3级分销不能=0
            }
            //产品新增加显示设置  1.全部2分销3微信4.官网5.微信和官网
            condition += " and viewmethod in (1,2))";
            if (Key != "")
            {
                condition = condition + " and Projectname like '%" + Key + "%'";//3级分销不能=0
            }


            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            cmd.PagingCommand1("b2b_com_project", "*", "sortid,id desc ", "", Pagesize, Pageindex, "", condition);


            List<B2b_com_project> list = new List<B2b_com_project>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_project
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

        #endregion

        #region 授权产品列表


        internal List<B2b_com_pro> WarrantProlist(int Pageindex, int Pagesize, string Key, int Agentid, int Warrant_type, int comid, out int totalcount, int Agentlevel = 0, int projectid = 0, string viewmethod = "")
        {
            B2bComProData prodata = new B2bComProData();

            var condition = "com_id in (select comid from agent_warrant where agentid = " + Agentid + " and comid = " + comid + " and warrant_state=1) and server_type in (1,2,8,9,10,11,13,14) and pro_state=1 and convert(varchar(10),pro_end,120)>='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";

            if (Warrant_type == 2)
            {
                condition = condition + " and not source_type=3";//如果是倒码授权分销不显示接口产品，因接口产品无法倒码
            }
            if (Agentlevel != 0)
            {//判断分销，根据分销级别，判断这个级别分销价格为0则不显示
                if (Agentlevel == 1)
                {
                    condition = condition + " and (   agent1_price>0 or server_type in (2,8) or id in (select proid from b2b_com_pro_Speci where speci_agent1_price>0) )";//1级分销不能=0
                }
                if (Agentlevel == 2)
                {
                    condition = condition + " and (   agent2_price>0 or server_type in (2,8) or id in (select proid from b2b_com_pro_Speci where speci_agent1_price>0)  )";//2级分销不能=0
                }
                if (Agentlevel == 3)
                {
                    condition = condition + " and (   agent3_price>0 or server_type in (2,8) or id in (select proid from b2b_com_pro_Speci where speci_agent1_price>0) )";//3级分销不能=0
                }
            }
            if (Key != "")
            {

                condition = condition + " and pro_name like '%" + Key + "%'";//3级分销不能=0
            }
            //产品新增加显示设置  1.全部2分销3微信4.官网5.微信和官网
            if (viewmethod != "")
            {
                condition += " and (viewmethod in (1,2) or id in (select proid from b2b_agent_prowarrant where Agentid=" + Agentid + " and comid=" + comid + "))";
            }
            //查询指定项目
            if (projectid != 0)
            {
                condition += " and projectid =" + projectid;
            }
            //如果是 验证扣款分销商 则不能销售 分销导入产品 中原商户设定绑定分销商为 出票扣款
            if (Warrant_type == 2)
            {
                condition += " and not id in( select id from b2b_com_pro where bindingid in (select id from b2b_com_pro where com_id in (select comid from agent_warrant where agentid in (select bindingagent from b2b_company where id=" + comid + ") and warrant_type=1) ))";
            }



            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            cmd.PagingCommand1("b2b_com_pro", "*", "sortid,id desc ", "", Pagesize, Pageindex, "", condition);


            List<B2b_com_pro> list = new List<B2b_com_pro>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_pro
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
                        Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),

                        Agent1_price = reader.GetValue<decimal>("Agent1_price"),
                        Agent2_price = reader.GetValue<decimal>("Agent2_price"),
                        Agent3_price = reader.GetValue<decimal>("Agent3_price"),

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
                        Projectid = reader.GetValue<int>("Projectid"),
                        Imgurl = reader.GetValue<int>("imgurl"),
                        ProValidateMethod = reader.GetValue<string>("ProValidateMethod"),
                        Appointdata = reader.GetValue<int>("Appointdata"),
                        Iscanuseonsameday = reader.GetValue<int>("Iscanuseonsameday"),


                        Pro_youxiaoqi = prodata.GetPro_Youxiaoqi(reader.GetValue<DateTime>("pro_start"), reader.GetValue<DateTime>("pro_end"), reader.GetValue<string>("ProValidateMethod"), reader.GetValue<int>("Appointdata"), reader.GetValue<int>("Iscanuseonsameday")),
                        Pro_explain = reader.GetValue<string>("Pro_explain"),
                        Ispanicbuy = reader.GetValue<int>("ispanicbuy"),
                        Bindingid = reader.GetValue<int>("Bindingid"),
                        Manyspeci = reader.GetValue<int>("Manyspeci"),

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }

        #endregion
        internal List<B2b_com_pro> AllWarrantProlist(int Pageindex, int Pagesize, string Key, int Agentid, out int totalcount, string viewmethod = "", int servertype = 0)
        {
            var condition = " server_type in (1,2,8,10,11,14)";
            if (servertype != 0)
            {
                condition = " server_type =" + servertype;
            }
            condition += " and pro_state=1 and convert(varchar(10),pro_end,120)>='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            //授权给分销的商户
            condition += " and  com_id in (select comid from agent_warrant where agentid =" + Agentid + " and warrant_state=1)";
            //产品不包含 已经授权给分销的商户下的没有设置分销价格的产品
            condition += " and not id in (" +
" select id from b2b_com_pro where server_type not in (2,8) and agent1_price =0 and com_id in (select comid from agent_warrant where agentid =" + Agentid + " and warrant_level=1)" +
" )" +
" and not id in (" +
" select id from b2b_com_pro where  server_type not in (2,8) and  agent2_price =0 and com_id in (select comid from agent_warrant where agentid =" + Agentid + " and warrant_level=2)" +
" )" +
" and not id in (" +
" select id from b2b_com_pro where  server_type not in (2,8) and  agent3_price =0 and com_id in (select comid from agent_warrant where agentid =" + Agentid + " and warrant_level=3)" +
" )";
            //产品不包含 授权给分销是验证扣款的商户下的外部接口产品(现在只有阳光)
            //if (Warrant_type == 2)
            //{
            //    condition = condition + " and not source_type=3";//如果是倒码授权分销不显示接口产品，因接口产品无法倒码
            //}
            condition += " and not id in (select id from b2b_com_pro where source_type=3 and com_id in (select comid from agent_warrant where  agentid =" + Agentid + " and Warrant_type = 2))";

            if (Key != "")
            {
                int nproid = 0;
                try
                {
                    nproid = int.Parse(Key);
                }
                catch
                {
                    nproid = 0;
                }

                condition += " and (pro_name like '%" + Key + "%' or id=" + nproid + ")";

            }
            //产品新增加显示设置  1.全部2分销3微信4.官网5.微信和官网
            if (viewmethod != "")
            {
                condition += "and viewmethod in (" + viewmethod + ")";
                //暂时不考虑特别授权的产品，因为查询用下面包含特别授权产品的语句时可能是因为查询语句过程的原因总是报错
                //condition += " and (viewmethod in (1,2) or id in (select proid from b2b_agent_prowarrant where Agentid=" + Agentid + " and comid in (select comid from agent_warrant where agentid =" + Agentid + " and warrant_state=1)))";
            }

            ////如果是 验证扣款分销商 则不能销售 分销导入产品 中原商户设定绑定分销商为 出票扣款
            //if (Warrant_type == 2)
            //{
            //    condition += " and not id in(    select id from b2b_com_pro where bindingid in (    select id from b2b_com_pro where com_id in ( select comid from agent_warrant where agentid in (   select bindingagent from b2b_company where id=" + comid + ")  and warrant_type=1) ))";
            //}
            //condition += " and not id in( select id from b2b_com_pro where bindingid in (    select id from b2b_com_pro where com_id in ( select comid from agent_warrant where agentid in (   select bindingagent from b2b_company where id  in (select comid from agent_warrant where  agentid =" + Agentid + " and Warrant_type = 2))  and warrant_type=1) ))";


            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            cmd.PagingCommand1("b2b_com_pro", "*", "id desc ", "", Pagesize, Pageindex, "", condition);


            List<B2b_com_pro> list = new List<B2b_com_pro>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_pro
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
                        Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),

                        Agent1_price = reader.GetValue<decimal>("Agent1_price"),
                        Agent2_price = reader.GetValue<decimal>("Agent2_price"),
                        Agent3_price = reader.GetValue<decimal>("Agent3_price"),

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
                        Projectid = reader.GetValue<int>("Projectid"),
                        Imgurl = reader.GetValue<int>("imgurl"),
                        ProValidateMethod = reader.GetValue<string>("ProValidateMethod"),
                        Appointdata = reader.GetValue<int>("Appointdata"),
                        Iscanuseonsameday = reader.GetValue<int>("Iscanuseonsameday"),


                        Pro_youxiaoqi = new B2bComProData().GetPro_Youxiaoqi(reader.GetValue<DateTime>("pro_start"), reader.GetValue<DateTime>("pro_end"), reader.GetValue<string>("ProValidateMethod"), reader.GetValue<int>("Appointdata"), reader.GetValue<int>("Iscanuseonsameday")),
                        Pro_explain = reader.GetValue<string>("Pro_explain"),
                        Ispanicbuy = reader.GetValue<int>("ispanicbuy"),
                        Bindingid = reader.GetValue<int>("Bindingid"),
                        Manyspeci = reader.GetValue<int>("Manyspeci"),

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }


        #region 未授权产品列表


        internal List<B2b_com_pro> UnWarrantProlist(int Pageindex, int Pagesize, string Key, int Agentid, int Warrant_type, int comid, out int totalcount, int Agentlevel = 0, int projectid = 0, string viewmethod = "")
        {
            B2bComProData prodata = new B2bComProData();

            var condition = "com_id in (select id from b2b_company where id = " + comid + " and lp=1) and server_type in (1,2,8,10,11) and pro_state=1 and convert(varchar(10),pro_end,120)>='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";

            if (Agentlevel != 0)
            {//判断分销，根据分销级别，判断这个级别分销价格为0则不显示
                if (Agentlevel == 1)
                {
                    condition = condition + " and (   agent1_price>0 or server_type in (2,8))";//1级分销不能=0
                }
                if (Agentlevel == 2)
                {
                    condition = condition + " and (   agent2_price>0 or server_type in (2,8))";//2级分销不能=0
                }
                if (Agentlevel == 3)
                {
                    condition = condition + " and (   agent3_price>0 or server_type in (2,8))";//3级分销不能=0
                }
            }
            if (Key != "")
            {

                condition = condition + " and pro_name like '%" + Key + "%'";//3级分销不能=0
            }
            //产品新增加显示设置  1.全部2分销3微信4.官网5.微信和官网
            if (viewmethod != "")
            {
                condition += " and viewmethod in (1,2)";
            }
            //查询指定项目
            if (projectid != 0)
            {
                condition += " and projectid =" + projectid;
            }



            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            cmd.PagingCommand1("b2b_com_pro", "*", "id desc ", "", Pagesize, Pageindex, "", condition);


            List<B2b_com_pro> list = new List<B2b_com_pro>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_pro
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
                        Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),

                        Agent1_price = reader.GetValue<decimal>("Agent1_price"),
                        Agent2_price = reader.GetValue<decimal>("Agent2_price"),
                        Agent3_price = reader.GetValue<decimal>("Agent3_price"),

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
                        Projectid = reader.GetValue<int>("Projectid"),
                        Imgurl = reader.GetValue<int>("imgurl"),
                        ProValidateMethod = reader.GetValue<string>("ProValidateMethod"),
                        Appointdata = reader.GetValue<int>("Appointdata"),
                        Iscanuseonsameday = reader.GetValue<int>("Iscanuseonsameday"),


                        Pro_youxiaoqi = prodata.GetPro_Youxiaoqi(reader.GetValue<DateTime>("pro_start"), reader.GetValue<DateTime>("pro_end"), reader.GetValue<string>("ProValidateMethod"), reader.GetValue<int>("Appointdata"), reader.GetValue<int>("Iscanuseonsameday")),
                        Pro_explain = reader.GetValue<string>("Pro_explain"),
                        Ispanicbuy = reader.GetValue<int>("ispanicbuy"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }

        #endregion


        #region 绑定商户授权产品列表
        internal List<B2b_com_pro> BindingWarrantProlist(int Pageindex, int Pagesize, string Key, int Agentid, int Warrant_type, int bindingcomid, int comid, out int totalcount, int Agentlevel = 0)
        {

            B2bComProData prodata = new B2bComProData();

            var condition = "com_id in (select comid from agent_warrant where agentid = " + Agentid + " and comid = " + bindingcomid + " and warrant_state=1) and server_type in (1,11,14,9,10) and pro_state=1 ";

            condition = condition + " and not source_type in (4)";//导入产品，不能再次导入
            if (Warrant_type == 2)
            {
                condition = condition + " and not source_type in (3) and server_type !=11";//倒码产品不能让倒码分销商导入 ,倒码分销不能导入实物产品
            }



            if (Agentlevel != 0)
            {//判断分销，根据分销级别，判断这个级别分销价格为0则不显示
                if (Agentlevel == 1)
                {
                    condition = condition + "  and (   agent1_price>0 or id in (select proid from b2b_com_pro_Speci where speci_agent1_price>0) ) ";//1级分销不能=0
                }
                if (Agentlevel == 2)
                {
                    condition = condition + " and (   agent2_price>0 or id in (select proid from b2b_com_pro_Speci where speci_agent2_price>0) )";//2级分销不能=0
                }
                if (Agentlevel == 3)
                {
                    condition = condition + " and (   agent3_price>0 or id in (select proid from b2b_com_pro_Speci where speci_agent3_price>0) )";//3级分销不能=0
                }
            }
            if (Key != "")
            {

                condition = condition + " and pro_name like '%" + Key + "%'";//3级分销不能=0
            }
            //产品新增加显示设置  1.全部2分销3微信4.官网5.微信和官网
            condition += " and (viewmethod in (1,2) or id in (select proid from b2b_agent_prowarrant where Agentid=" + Agentid + " and comid=" + comid + "))  and not id in(select bindingid from b2b_com_pro where com_id = " + comid + ")";

            var cmd = sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            cmd.PagingCommand1("b2b_com_pro", "*", "id desc ", "", Pagesize, Pageindex, "", condition);


            List<B2b_com_pro> list = new List<B2b_com_pro>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_com_pro
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
                        Agentsettle_price = reader.GetValue<decimal>("agentsettle_price"),

                        Agent1_price = reader.GetValue<decimal>("Agent1_price"),
                        Agent2_price = reader.GetValue<decimal>("Agent2_price"),
                        Agent3_price = reader.GetValue<decimal>("Agent3_price"),

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
                        Projectid = reader.GetValue<int>("Projectid"),
                        Imgurl = reader.GetValue<int>("imgurl"),
                        ProValidateMethod = reader.GetValue<string>("ProValidateMethod"),
                        Appointdata = reader.GetValue<int>("Appointdata"),
                        Iscanuseonsameday = reader.GetValue<int>("Iscanuseonsameday"),


                        Pro_youxiaoqi = prodata.GetPro_Youxiaoqi(reader.GetValue<DateTime>("pro_start"), reader.GetValue<DateTime>("pro_end"), reader.GetValue<string>("ProValidateMethod"), reader.GetValue<int>("Appointdata"), reader.GetValue<int>("Iscanuseonsameday"))

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;
        }
        #endregion

        #region 单个产品授权价格
        internal decimal WarrantProPrice(int proid, int Agentid, int comid)
        {
            string sql = @"select * from b2b_com_pro
as a left join agent_warrant as b on a.com_id=b.comid 
 where a.id in (select id from b2b_com_pro where bindingid=@proid ) and b.agentid=@Agentid and a.com_id=@comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@proid", proid);
            cmd.AddParam("@Agentid", Agentid);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    if (reader.GetValue<int>("warrant_level") == 1)
                    {
                        return reader.GetValue<decimal>("Agent1_price");
                    }
                    if (reader.GetValue<int>("warrant_level") == 2)
                    {
                        return reader.GetValue<decimal>("Agent2_price");
                    }
                    if (reader.GetValue<int>("warrant_level") == 3)
                    {
                        return reader.GetValue<decimal>("Agent3_price");
                    }
                }
            }
            return 0;

        }

        #endregion

        #region 单个产品授权价格
        internal decimal ImprestPro(int comid, int proid, int Agentlevel)
        {
            string aprice = "";
            if (Agentlevel == 1)
            {
                aprice = "agent1_price";
            }
            else if (Agentlevel == 2)
            {
                aprice = "agent2_price";
            }
            else if (Agentlevel == 3)
            {
                aprice = "agent3_price";
            }

            //插入项目并防止重复插入
            string sqlTxt2 = @"insert into b2b_com_project (comid,projectname,projectimg,province,city,industryid,briefintroduce,address,mobile,coordinate,serviceintroduce,onlinestate,createtime,createuserid,sortid,bindingprojectid)
select " + comid + ",projectname,projectimg,province,city,industryid,briefintroduce,address,mobile,coordinate,serviceintroduce,onlinestate,'" + DateTime.Now + "',0,0,id from b2b_com_project where id in (select projectid from b2b_com_pro where id=" + proid + ") and not id in ( select bindingprojectid from b2b_com_project where comid=" + comid + " and  bindingprojectid in (select projectid from b2b_com_pro where id=" + proid + ") ) ";
            var cmd2 = sqlHelper.PrepareTextSqlCommand(sqlTxt2);
            cmd2.CommandType = CommandType.Text;
            cmd2.ExecuteNonQuery();

            //插入产品
            string sqlTxt = @"insert into b2b_com_pro (com_id,pro_name,pro_state,server_type,pro_type,source_type,pro_remark,pro_start,pro_end,face_price,advise_price,agentsettle_price,service_contain,service_notcontain,precautions,createtime,sms,imgurl,pro_number,pro_explain,bindingid,Thatday_can,Thatday_can_day,tuan_pro,zhixiao,agentsale,createuserid  ,[serviceid]  ,[service_proid] ,[realnametype] ,[trval_productid] ,[travel_type]  ,[trval_starting] ,[ispanicbuy] ,[panicbuybegin_time] ,[panicbuyenddate_time]  ,[linepro_booktype]  ,[ProValidateMethod] ,[appointdata]  ,[iscanuseonsameday]  ,[viewmethod]  ,[childreduce]  ,[pickuppoint] ,[dropoffpoint]  ,[pro_note]  ,[QuitTicketMechanism]  ,[daybespeaknum]  ,[isneedbespeak] ,[bespeaksucmsg] ,[bespeakfailmsg] ,[customservicephone] ,[isblackoutdate] ,[etickettype],ishasdeliveryfee,pro_weight,Manyspeci)
select " + comid + ",pro_name,pro_state,server_type,pro_type,4,pro_remark,pro_start,pro_end,face_price,advise_price," + aprice + ",service_contain,service_notcontain,precautions,'" + DateTime.Now + "',sms,imgurl,pro_number,pro_explain,id,1,0,1,1,1,0,[serviceid]  ,[service_proid]   ,[realnametype]  ,[trval_productid]  ,[travel_type]  ,[trval_starting]  ,[ispanicbuy]  ,[panicbuybegin_time]  ,[panicbuyenddate_time]    ,[linepro_booktype] ,[ProValidateMethod] ,[appointdata]  ,[iscanuseonsameday]  ,[viewmethod]  ,[childreduce]  ,[pickuppoint] ,[dropoffpoint]  ,[pro_note]  ,[QuitTicketMechanism]  ,[daybespeaknum] ,[isneedbespeak] ,[bespeaksucmsg] ,[bespeakfailmsg] ,[customservicephone]   ,[isblackoutdate] ,[etickettype],ishasdeliveryfee,pro_weight,Manyspeci  from b2b_com_pro where id=" + proid + " and not id in (select bindingid from b2b_com_pro where com_id=" + comid + ")";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            return cmd.ExecuteNonQuery();


        }

        #endregion


        #region 单个产品授权价格
        internal decimal ImprestProGuige(int comid, int newproid, int oldproid, int Agentlevel)
        {
            string aprice = "";
            if (Agentlevel == 1)
            {
                aprice = "speci_agent1_price";
            }
            else if (Agentlevel == 2)
            {
                aprice = "speci_agent2_price";
            }
            else if (Agentlevel == 3)
            {
                aprice = "speci_agent3_price";
            }


            //插入产品
            string sqlTxt = @"insert into b2b_com_pro_Speci (comid,speci_name,speci_face_price,speci_advise_price,speci_agentsettle_price,speci_pro_weight,speci_totalnum,proid,speci_type_nameid_array,binding_id)
select " + comid + ",speci_name,speci_face_price,speci_advise_price," + aprice + ",speci_pro_weight,speci_totalnum," + newproid + ",speci_type_nameid_array,id  from b2b_com_pro_Speci where proid=" + oldproid + "";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            return cmd.ExecuteNonQuery();


        }

        #endregion


        #region 修改导入产品
        internal decimal UpImprestPro(int comid, int proid, int projectid)
        {
            //修改导入产品
            string sqlTxt = @"update b2b_com_pro set projectid=" + projectid + " where  com_id=" + comid + " and id=" + proid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            return cmd.ExecuteNonQuery();
        }

        #endregion



        #region 查询插入产品新产品ID
        internal int SearchImprestNewProid(int comid, int proid)
        {
            string sql = @"select * from b2b_com_pro where bindingid=@proid and com_id=@comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@proid", proid);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    return reader.GetValue<int>("id");
                }
            }
            return 0;

        }

        #endregion

        #region 查询插入项目新ID
        internal int SearchImprestNewProjectid(int comid, int proid)
        {
            string sql = @"select * from b2b_com_project where comid=@comid and bindingprojectid in (select projectid from b2b_com_pro where id=@proid)";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@proid", proid);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    return reader.GetValue<int>("id");
                }
            }
            return 0;

        }

        #endregion


        internal int ChangeAgentMsgSet(int agentid, int agentmsgset)
        {
            string sql = "update agent_company set agent_messagesetting=" + agentmsgset + " where id=" + agentid;

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal List<string> Getagentprovincelist(int comid = 0)
        {
            string sql = "select distinct(com_province) as province from agent_company where id in (select agentid from agent_warrant where comid=" + comid + ")";
            if (comid == 0)
            {
                sql = "select distinct(com_province) as province from agent_company ";

            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                List<string> list = new List<string>();
                while (reader.Read())
                {
                    if (reader.GetValue<string>("province") != "")
                    {
                        list.Add(reader.GetValue<string>("province"));
                    }
                }
                sqlHelper.Dispose();
                return list;
            }

        }

        internal int GetAgentIdByAccount(string Email)
        {
            string sql = "select agentid from agent_company_manageuser where account='" + Email + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            int agentid = 0;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    agentid = reader.GetValue<int>("agentid");
                }
            }
            return agentid;
        }

        internal int AgentUpPhone(int agentid, string newphone)
        {
            string sql = "update agent_company set mobile='" + newphone + "' where id=" + agentid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int AgentUserUpPhone(int agentuserid, string newphone)
        {
            string sql = "update agent_company_manageuser set mobile='" + newphone + "' where id=" + agentuserid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int UpAgentTaobaoState(int istaobao, int agentid)
        {
            string sql = "update agent_company set istaobao=" + istaobao + " where id=" + agentid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal string GetAgentType(int agentid)
        {
            if (agentid == 0)
            {
                return "";
            }
            string sql = "select agent_type from agent_company where id=" + agentid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                string agenttype = "";
                if (reader.Read())
                {
                    agenttype = reader.GetValue<int>("agent_type").ToString();
                }
                return agenttype;
            }
        }

        internal Agent_warrant GetAgentWarrant(int Agentid, int comid)
        {
            string sql = @"SELECT [id]
                              ,[agentid]
                              ,[comid]
                              ,[warrant_state]
                              ,[warrant_type]
                              ,[warrant_level]
                              ,[warrant_lp]
                              ,[imprest]
                              ,[Credit]
                          FROM  [agent_warrant] where agentid=@agentid and comid=@comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@agentid", Agentid);
            cmd.AddParam("@comid", comid);
            using (var reader = cmd.ExecuteReader())
            {
                Agent_warrant m = null;
                if (reader.Read())
                {
                    m = new Agent_warrant
                    {
                        Id = reader.GetValue<int>("id"),
                        Agentid = reader.GetValue<int>("agentid"),
                        Comid = reader.GetValue<int>("comid"),
                        Warrant_state = reader.GetValue<int>("warrant_state"),
                        Warrant_type = reader.GetValue<int>("warrant_type"),
                        Warrant_level = reader.GetValue<int>("warrant_level"),
                        Warrant_lp = reader.GetValue<int>("warrant_lp"),
                        Imprest = reader.GetValue<decimal>("imprest"),
                        Credit = reader.GetValue<decimal>("Credit"),

                    };
                }
                return m;
            }

        }

        internal string GetAgentName(int agentid)
        {
            string sql = "select company from agent_company where id=" + agentid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                string r = "";
                if (reader.Read())
                {
                    r = reader.GetValue<string>("company");
                }
                return r;
            }
        }

        internal bool IsExistMeituanConfig(Agent_company regiinfo)
        {
            string sql = "select count(1) from agent_company where mt_partnerId='"+regiinfo.mt_partnerId+"' and id!="+regiinfo.Id;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            if (int.Parse(o.ToString()) > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }

        internal Agent_warrant GetAgentWarrantInfo(int agentid, int productid)
        {
            string sql = @"SELECT [id]
                              ,[agentid]
                              ,[comid]
                              ,[warrant_state]
                              ,[warrant_type]
                              ,[warrant_level]
                              ,[warrant_lp]
                              ,[imprest]
                              ,[Credit]
                          FROM  [agent_warrant] where agentid=@agentid and comid=(select com_id from b2b_com_pro where id=@productid)";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@agentid", agentid);
            cmd.AddParam("@productid", productid);
            using (var reader = cmd.ExecuteReader())
            {
                Agent_warrant m = null;
                if (reader.Read())
                {
                    m = new Agent_warrant
                    {
                        Id = reader.GetValue<int>("id"),
                        Agentid = reader.GetValue<int>("agentid"),
                        Comid = reader.GetValue<int>("comid"),
                        Warrant_state = reader.GetValue<int>("warrant_state"),
                        Warrant_type = reader.GetValue<int>("warrant_type"),
                        Warrant_level = reader.GetValue<int>("warrant_level"),
                        Warrant_lp = reader.GetValue<int>("warrant_lp"),
                        Imprest = reader.GetValue<decimal>("imprest"),
                        Credit = reader.GetValue<decimal>("Credit"),

                    };
                }
                return m;
            }
        }
    }
}

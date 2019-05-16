using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalb2b_com_pro_Speci
    {
        private SqlHelper sqlHelper;
        public Internalb2b_com_pro_Speci(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal int EditB2b_com_pro_Speci(B2b_com_pro_Speci m)
        {
            string sql = "select  id  from b2b_com_pro_Speci where proid=" + m.proid + " and speci_type_nameid_Array='" + m.speci_type_nameid_Array + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            var id = 0;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    id = reader.GetValue<int>("id");

                }
            }


            #region 编辑
            if (id > 0)
            {
                string sql1 = "update b2b_com_pro_Speci set speci_name='" + m.speci_name + "',speci_face_price='" + m.speci_face_price + "',speci_advise_price='" + m.speci_advise_price + "',speci_agent1_price='" + m.speci_agent1_price + "',speci_agent2_price='" + m.speci_agent2_price + "',speci_agent3_price='" + m.speci_agent3_price + "',speci_agentsettle_price='" + m.speci_agentsettle_price + "',speci_pro_weight='" + m.speci_pro_weight + "',speci_totalnum='" + m.speci_totalnum + "',speci_type_nameid_Array='" + m.speci_type_nameid_Array + "' where id=" + id;
                var cmd1 = sqlHelper.PrepareTextSqlCommand(sql1);
                cmd1.ExecuteNonQuery();


                #region 同步修改导入产品规格的成本价:获得导入产品id 和 导入产品规格的成本价
                IList<B2b_com_pro> drprolist = new List<B2b_com_pro>();//导入产品列表
                string s1 = "select id,com_id from b2b_com_pro where bindingid=" + m.proid;
                var c1 = sqlHelper.PrepareTextSqlCommand(s1);
                using (var reader = c1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        drprolist.Add(new B2b_com_pro
                        {
                            Id = reader.GetValue<int>("id"),
                            Com_id = reader.GetValue<int>("com_id")
                        });
                    }
                }
                if (drprolist != null)
                {
                    if (drprolist.Count > 0)
                    {
                        foreach (B2b_com_pro drpro in drprolist)
                        {
                            if (drpro != null)
                            {
                                #region 获得导入产品成本价
                                int drbinding_Agentid = 0; //导入产品公司的绑定分销
                                int drbinding_Warrant_type = 0;//导入产品公司的绑定分销的分销类型(验证扣款；出票扣款)
                                int drbinding_Agentlevel = 3;//导入产品公司的绑定分销 在原始公司下的分销级别

                                var comdata = B2bCompanyData.GetCompany(drpro.Com_id);
                                if (comdata != null)
                                {
                                    drbinding_Agentid = comdata.Bindingagent;
                                }

                                var agentmodel = AgentCompanyData.GetAgentWarrant(drbinding_Agentid, m.comid);
                                if (agentmodel != null)
                                {
                                    drbinding_Agentlevel = agentmodel.Warrant_level;
                                    drbinding_Warrant_type = agentmodel.Warrant_type;
                                }

                                decimal aprice = 0;
                                if (drbinding_Agentlevel == 1)
                                {
                                    aprice = m.speci_agent1_price;
                                }
                                else if (drbinding_Agentlevel == 2)
                                {
                                    aprice = m.speci_agent2_price;
                                }
                                else if (drbinding_Agentlevel == 3)
                                {
                                    aprice = m.speci_agent3_price;
                                }
                                //修改绑定规格的成本价格
                                string s2 = "update b2b_com_pro_Speci set speci_agentsettle_price='" + aprice + "' where proid=" + drpro.Id + " and binding_id=" + id;
                                var c2 = sqlHelper.PrepareTextSqlCommand(s2);
                                c2.ExecuteNonQuery();
                                #endregion
                            }
                        }
                    }
                }
                #endregion


                #region 库存变化日志
                try
                {
                    B2b_com_pro_kucunlog kucunlog = new B2b_com_pro_kucunlog
                    {
                        id = 0,
                        orderid = 0,
                        proid = m.proid,
                        servertype = new B2bComProData().GetServertypeByProid(m.proid),
                        daydate = DateTime.Parse("1970-01-01"),
                        proSpeciId = id,
                        surplusnum = m.speci_totalnum,
                        operor = "",
                        opertime = DateTime.Now,
                        opertype = "编辑规格",
                        oper="多规格"
                    };
                    new B2b_com_pro_kucunlogData().Editkucunlog(kucunlog);
                }
                catch { }
                #endregion


                return id;
            }
            #endregion
            #region 添加
            else
            {
                string sql2 = "INSERT INTO  [b2b_com_pro_Speci] (speci_name,speci_face_price,speci_advise_price,speci_agent1_price,speci_agent2_price,speci_agent3_price,speci_agentsettle_price,speci_pro_weight,speci_totalnum,comid,proid,speci_type_nameid_Array)  VALUES  ('" + m.speci_name + "','" + m.speci_face_price + "','" + m.speci_advise_price + "','" + m.speci_agent1_price + "','" + m.speci_agent2_price + "','" + m.speci_agent3_price + "','" + m.speci_agentsettle_price + "','" + m.speci_pro_weight + "','" + m.speci_totalnum + "','" + m.comid + "','" + m.proid + "','" + m.speci_type_nameid_Array + "');select @@identity;";
                var cmd2 = sqlHelper.PrepareTextSqlCommand(sql2);
                object o = cmd2.ExecuteScalar();
                int newspeciid = int.Parse(o.ToString());

                #region 同步修改导入产品规格的成本价:获得导入产品id 和 导入产品规格的成本价
                IList<B2b_com_pro> drprolist = new List<B2b_com_pro>();//导入产品列表
                string s1 = "select id,com_id from b2b_com_pro where bindingid=" + m.proid;
                var c1 = sqlHelper.PrepareTextSqlCommand(s1);
                using (var reader = c1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        drprolist.Add(new B2b_com_pro
                        {
                            Id = reader.GetValue<int>("id"),
                            Com_id = reader.GetValue<int>("com_id")
                        });
                    }
                }
                if (drprolist != null)
                {
                    if (drprolist.Count > 0)
                    {
                        foreach (B2b_com_pro drpro in drprolist)
                        {
                            if (drpro != null)
                            {
                                #region 获得导入产品成本价
                                int drbinding_Agentid = 0; //导入产品公司的绑定分销
                                int drbinding_Warrant_type = 0;//导入产品公司的绑定分销的分销类型(验证扣款；出票扣款)
                                int drbinding_Agentlevel = 3;//导入产品公司的绑定分销 在原始公司下的分销级别

                                var comdata = B2bCompanyData.GetCompany(drpro.Com_id);
                                if (comdata != null)
                                {
                                    drbinding_Agentid = comdata.Bindingagent;
                                }

                                var agentmodel = AgentCompanyData.GetAgentWarrant(drbinding_Agentid, m.comid);
                                if (agentmodel != null)
                                {
                                    drbinding_Agentlevel = agentmodel.Warrant_level;
                                    drbinding_Warrant_type = agentmodel.Warrant_type;
                                }

                                decimal aprice = 0;
                                if (drbinding_Agentlevel == 1)
                                {
                                    aprice = m.speci_agent1_price;
                                }
                                else if (drbinding_Agentlevel == 2)
                                {
                                    aprice = m.speci_agent2_price;
                                }
                                else if (drbinding_Agentlevel == 3)
                                {
                                    aprice = m.speci_agent3_price;
                                }
                                //修改绑定规格的成本价格
                                string sqlTxt = @"insert into b2b_com_pro_Speci (comid,speci_name,speci_face_price,speci_advise_price,speci_agentsettle_price,speci_pro_weight,speci_totalnum,proid,speci_type_nameid_array,binding_id)
select " + drpro.Com_id + ",speci_name,speci_face_price,speci_advise_price," + aprice + ",speci_pro_weight,speci_totalnum," + drpro.Id + ",speci_type_nameid_array,id  from b2b_com_pro_Speci where id=" + newspeciid + "";
                                var c2 = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                                c2.ExecuteNonQuery();

                                #endregion
                            }
                        }
                    }
                }
                #endregion

                #region 库存变化日志
                try
                {
                    B2b_com_pro_kucunlog kucunlog = new B2b_com_pro_kucunlog
                    {
                        id = 0,
                        orderid = 0,
                        proid = m.proid,
                        servertype = new B2bComProData().GetServertypeByProid(m.proid),
                        daydate = DateTime.Parse("1970-01-01"),
                        proSpeciId = newspeciid,
                        surplusnum = m.speci_totalnum,
                        operor = "",
                        opertime = DateTime.Now,
                        opertype = "新加规格",
                        oper="多规格"
                    };
                    new B2b_com_pro_kucunlogData().Editkucunlog(kucunlog);
                }
                catch { }
                #endregion

                return newspeciid;
            }
            #endregion
        }

        internal List<B2b_com_pro_Speci> Getgglist(int proid)
        {
            string sql = "select * from b2b_com_pro_Speci where proid=" + proid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                List<B2b_com_pro_Speci> list = new List<B2b_com_pro_Speci>();
                while (reader.Read())
                {
                    list.Add(new B2b_com_pro_Speci
                    {
                        id = reader.GetValue<int>("id"),
                        speci_name = reader.GetValue<string>("speci_name"),
                        speci_face_price = reader.GetValue<decimal>("speci_face_price"),
                        speci_advise_price = reader.GetValue<decimal>("speci_advise_price"),
                        speci_agent1_price = reader.GetValue<decimal>("speci_agent1_price"),
                        speci_agent2_price = reader.GetValue<decimal>("speci_agent2_price"),
                        speci_agent3_price = reader.GetValue<decimal>("speci_agent3_price"),
                        speci_agentsettle_price = reader.GetValue<decimal>("speci_agentsettle_price"),
                        speci_pro_weight = reader.GetValue<decimal>("speci_pro_weight"),
                        speci_totalnum = reader.GetValue<int>("speci_totalnum"),
                        comid = reader.GetValue<int>("comid"),
                        proid = reader.GetValue<int>("proid"),
                        speci_type_nameid_Array = reader.GetValue<string>("speci_type_nameid_Array"),

                    });
                }
                return list;
            }
        }

        internal B2b_com_pro_Speci Getgginfobyggid(int ggid)
        {
            string sql = "select * from b2b_com_pro_Speci where id=" + ggid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                B2b_com_pro_Speci list = new B2b_com_pro_Speci();
                if (reader.Read())
                {

                    list.id = reader.GetValue<int>("id");
                    list.speci_name = reader.GetValue<string>("speci_name");
                    list.speci_face_price = reader.GetValue<decimal>("speci_face_price");
                    list.speci_advise_price = reader.GetValue<decimal>("speci_advise_price");
                    list.speci_agent1_price = reader.GetValue<decimal>("speci_agent1_price");
                    list.speci_agent2_price = reader.GetValue<decimal>("speci_agent2_price");
                    list.speci_agent3_price = reader.GetValue<decimal>("speci_agent3_price");
                    list.speci_agentsettle_price = reader.GetValue<decimal>("speci_agentsettle_price");
                    list.speci_pro_weight = reader.GetValue<decimal>("speci_pro_weight");
                    list.speci_totalnum = reader.GetValue<int>("speci_totalnum");
                    list.comid = reader.GetValue<int>("comid");
                    list.proid = reader.GetValue<int>("proid");
                    list.speci_type_nameid_Array = reader.GetValue<string>("speci_type_nameid_Array");
                    list.binding_id = reader.GetValue<int>("binding_id");

                }
                return list;
            }
        }

        internal List<B2b_com_pro_Speci> AgentGetgglist(int proid, int Agentlevel)
        {
            string sql = "select * from b2b_com_pro_Speci where proid=" + proid;

            if (Agentlevel == 1)
            {
                sql = "select id,speci_name,speci_face_price,speci_pro_weight,comid,proid,speci_type_nameid_Array,speci_totalnum,speci_advise_price,speci_agent1_price as speci_agentsettle_price from b2b_com_pro_Speci where proid=" + proid + " and speci_agent1_price !=0";
            }
            if (Agentlevel == 2)
            {
                sql = "select id,speci_name,speci_face_price,speci_pro_weight,comid,proid,speci_type_nameid_Array,speci_totalnum,speci_advise_price,speci_agent2_price as speci_agentsettle_price from b2b_com_pro_Speci where proid=" + proid + " and speci_agent2_price !=0";
            }
            if (Agentlevel == 3)
            {
                sql = "select id,speci_name,speci_face_price,speci_pro_weight,comid,proid,speci_type_nameid_Array,speci_totalnum,speci_advise_price,speci_agent3_price as speci_agentsettle_price from b2b_com_pro_Speci where proid=" + proid + " and speci_agent3_price !=0";
            }

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                List<B2b_com_pro_Speci> list = new List<B2b_com_pro_Speci>();
                while (reader.Read())
                {
                    list.Add(new B2b_com_pro_Speci
                    {
                        id = reader.GetValue<int>("id"),
                        speci_name = reader.GetValue<string>("speci_name"),
                        speci_face_price = reader.GetValue<decimal>("speci_face_price"),
                        speci_advise_price = reader.GetValue<decimal>("speci_advise_price"),
                        speci_agentsettle_price = reader.GetValue<decimal>("speci_agentsettle_price"),
                        speci_pro_weight = reader.GetValue<decimal>("speci_pro_weight"),
                        speci_totalnum = reader.GetValue<int>("speci_totalnum"),
                        comid = reader.GetValue<int>("comid"),
                        proid = reader.GetValue<int>("proid"),
                        speci_type_nameid_Array = reader.GetValue<string>("speci_type_nameid_Array"),

                    });
                }
                return list;
            }
        }

        internal string Getspecinamebyid(int speciid)
        {
            string guigename = "";
            if (speciid == 0)
            {
                return "";
            }

            string sql = "select  *  from b2b_com_pro_Speci where id=" + speciid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    guigename = reader.GetValue<string>("speci_name");
                }
            }
            return guigename;
        }


        internal decimal Getspeciminpricebyid(int proid)
        {
            decimal guigename = 0;
            if (proid == 0)
            {
                return 0;
            }

            string sql = "select  min(speci_advise_price) as speci_advise_price  from b2b_com_pro_Speci where proid=" + proid + " and speci_face_price !=0";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    guigename = reader.GetValue<decimal>("speci_advise_price");
                }
            }
            return guigename;
        }




        internal decimal Getspeciminfacepricebyid(int proid)
        {
            decimal guigename = 0;
            if (proid == 0)
            {
                return 0;
            }

            string sql = "select  min(speci_face_price) as speci_face_price from b2b_com_pro_Speci where proid=" + proid + " and speci_face_price !=0";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    guigename = reader.GetValue<decimal>("speci_face_price");
                }
            }
            return guigename;
        }

        internal decimal Gettop1availableprice(int Agentlevel, int proid)
        {
            string sql = "";
            if (Agentlevel == 1)
            {
                sql = "select top 1 speci_agent1_price as agentprice from b2b_com_pro_Speci where proid=" + proid + " and speci_agent1_price>0";
            }
            else if (Agentlevel == 2)
            {
                sql = "select top 1 speci_agent2_price as agentprice from b2b_com_pro_Speci where proid=" + proid + " and speci_agent2_price>0";
            }
            else if (Agentlevel == 3)
            {
                sql = "select top 1 speci_agent3_price as agentprice from b2b_com_pro_Speci where proid=" + proid + " and speci_agent3_price>0";
            }

            if (sql == "")
            {
                return 0;
            }
            else
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetValue<decimal>("agentprice");
                    }
                    return 0;
                }
            }

        }
    }
}

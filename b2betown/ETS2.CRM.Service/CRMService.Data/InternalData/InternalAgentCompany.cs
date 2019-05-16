using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;
using System.Data;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    public class InternalAgentCompany
    {
        private SqlHelper sqlHelper;
        public InternalAgentCompany(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }



        #region 根据授权id 得到分销商基本信息
        internal Agent_company GetAgentCompanyByUid(int warrantid)
        {
            const string sqlTxt = @"SELECT  a.id,b.Credit,b.id as warrantid,a.company,a.name,a.address,a.mobile,a.tel,a.run_state,b.imprest,b.warrant_state,b.warrant_type,b.warrant_level
              FROM [agent_company] as a right join agent_warrant as b on a.id=b.agentid where b.ID =@warrantid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@warrantid", warrantid);

            using (var reader = cmd.ExecuteReader())
            {
                Agent_company u = null;

                while (reader.Read())
                {
                    u = new Agent_company
                    {
                        Id = reader.GetValue<int>("id"),
                        Warrantid = reader.GetValue<int>("warrantid"),
                        Company = reader.GetValue<string>("Company"),
                        Contentname = reader.GetValue<string>("name"),
                        Address = reader.GetValue<string>("Address"),
                        Mobile = reader.GetValue<string>("Mobile"),
                        Tel = reader.GetValue<string>("Tel"),
                        Run_state = reader.GetValue<int>("Run_state"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrant_state = reader.GetValue<int>("Warrant_state"),
                        Imprest = reader.GetValue<decimal>("Imprest"),
                        Credit = reader.GetValue<decimal>("Credit"),
                        Warrant_level = reader.GetValue<int>("warrant_level"),
                    };

                }
                return u;
            }
        }
        #endregion

        #region 根据授权id 得到分销商登陆账户信息
        internal Agent_regiinfo GetAgentAccountByUid(string account, int agentid)
        {
            const string sqlTxt = @"SELECT  * from agent_company_manageuser where account=@account and agentid=@agentid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@agentid", agentid);
            cmd.AddParam("@account", account);

            using (var reader = cmd.ExecuteReader())
            {
                Agent_regiinfo u = null;

                while (reader.Read())
                {
                    u = new Agent_regiinfo
                    {
                        Id = reader.GetValue<int>("id"),
                        Account = reader.GetValue<string>("Account"),
                        Pwd = reader.GetValue<string>("Pwd"),
                        Contentname = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("Mobile"),
                        Amount = reader.GetValue<decimal>("Amount"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        AccountLevel = reader.GetValue<int>("AccountLevel"),
                    };

                }
                return u;
            }
        }
        #endregion


        #region 根据授权id 得到分销商登陆账户信息
        internal Agent_regiinfo GetManageAgentAccountByUid(int id)
        {
            const string sqlTxt = @"SELECT  * from agent_company_manageuser where id=@id ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@id", id);


            using (var reader = cmd.ExecuteReader())
            {
                Agent_regiinfo u = null;

                while (reader.Read())
                {
                    u = new Agent_regiinfo
                    {
                        Id = reader.GetValue<int>("id"),
                        Account = reader.GetValue<string>("Account"),
                        Pwd = reader.GetValue<string>("Pwd"),
                        Contentname = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("Mobile"),
                        Amount = reader.GetValue<decimal>("Amount"),
                        Agentid = reader.GetValue<int>("Agentid"),

                    };

                }
                return u;
            }
        }
        #endregion


        #region 根据分销id 得到分销商基本信息
        internal Agent_company GetAgentByid(int agentid)
        {
            const string sqlTxt = @"SELECT  *
              FROM [agent_company] as a  where a.ID =@agentid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@agentid", agentid);

            using (var reader = cmd.ExecuteReader())
            {
                Agent_company u = null;

                while (reader.Read())
                {
                    u = new Agent_company
                    {
                        istaobao = reader.GetValue<int>("istaobao"),
                        tb_syncurl = reader.GetValue<string>("tb_syncurl"),
                        tb_isret_consumeresult = reader.GetValue<int>("tb_isret_consumeresult"),


                        Id = reader.GetValue<int>("id"),
                        Company = reader.GetValue<string>("Company"),
                        Contentname = reader.GetValue<string>("name"),
                        Address = reader.GetValue<string>("Address"),
                        Mobile = reader.GetValue<string>("Mobile"),
                        Tel = reader.GetValue<string>("Tel"),
                        Run_state = reader.GetValue<int>("Run_state"),
                        Agentsort = reader.GetValue<int>("Agent_sort"),
                        Weixin_img = reader.GetValue<int>("Weixin_img"),
                        Agent_domain = reader.GetValue<string>("Agent_domain"),
                        Agent_messagesetting = reader.GetValue<int>("Agent_messagesetting"),
                        com_province = reader.GetValue<string>("com_province"),
                        com_city = reader.GetValue<string>("com_city"),
                        Agent_type = reader.GetValue<int>("Agent_type"),
                        maxremindmoney = reader.GetValue<int>("maxremindmoney"),

                        ismeituan=reader.GetValue<int>("ismeituan"),
                        mt_partnerId=reader.GetValue<string>("mt_partnerId"),
                        mt_client=reader.GetValue<string>("mt_client"),
                        mt_secret=reader.GetValue<string>("mt_secret"),
                        mt_mark = reader.GetValue<string>("mt_mark"),
                        Lvmama_uid = reader.GetValue<string>("Lvmama_uid"),
                        Lvmama_password = reader.GetValue<string>("Lvmama_password"),
                        Lvmama_Apikey = reader.GetValue<string>("Lvmama_Apikey")
                        
                    };

                }
                return u;
            }
        }
        #endregion


        #region 根据分销id，comid 得到分销商基本信息
        internal Agent_company GetAgentWarrant(int agentid, int comid)
        {
            const string sqlTxt = @"SELECT a.maxremindmoney, a.Agent_sort,a.Agent_domain,a.Weixin_img,a.id,b.id as warrantid,a.company,a.name,a.address,a.mobile,a.tel,a.run_state,b.imprest,b.Credit,b.warrant_state,b.Warrant_level,b.warrant_type
              FROM [agent_company] as a right join agent_warrant as b on a.id=b.agentid where b.agentid=@agentid and b.comid=@comid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@agentid", agentid);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                Agent_company u = null;

                while (reader.Read())
                {
                    u = new Agent_company
                    {
                        maxremindmoney = reader.GetValue<int>("maxremindmoney"),
                        Id = reader.GetValue<int>("id"),
                        Warrantid = reader.GetValue<int>("warrantid"),
                        Company = reader.GetValue<string>("Company"),
                        Contentname = reader.GetValue<string>("name"),
                        Address = reader.GetValue<string>("Address"),
                        Name = reader.GetValue<string>("Name"),
                        Mobile = reader.GetValue<string>("Mobile"),
                        Tel = reader.GetValue<string>("Tel"),
                        Run_state = reader.GetValue<int>("Run_state"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrant_state = reader.GetValue<int>("Warrant_state"),
                        Warrant_level = reader.GetValue<int>("Warrant_level"),
                        Imprest = reader.GetValue<decimal>("Imprest"),
                        Credit = reader.GetValue<decimal>("Credit"),
                        Agentsort = reader.GetValue<int>("Agent_sort"),
                        Agent_domain = reader.GetValue<string>("Agent_domain"),
                        Weixin_img = reader.GetValue<int>("Weixin_img"),
                    };

                }
                return u;
            }
        }
        #endregion

        #region 根据分销id，comid 得到分销商基本信息
        internal int GetAgentWarrantAgentlevel(int agentid, int comid)
        {

            const string sqlTxt = @"SELECT  a.Agent_sort,a.Agent_domain,a.Weixin_img,a.id,b.id as warrantid,a.company,a.name,a.address,a.mobile,a.tel,a.run_state,b.imprest,b.Credit,b.warrant_state,b.Warrant_level,b.warrant_type
              FROM [agent_company] as a right join agent_warrant as b on a.id=b.agentid where b.agentid=@agentid and b.comid=@comid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@agentid", agentid);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("Warrant_level");

                }
                return 3;//默认返回最低级3级
            }
        }
        #endregion

        #region 根据分销id，comid 得到分销商基本信息
        internal int GetAgentWarranttypebyproid(int proid, int comid)
        {
            const string sqlTxt = @"select * from agent_warrant where comid in (select com_id from b2b_com_pro where id in (select bindingid from b2b_com_pro where id=@proid))
and agentid in (select bindingagent from b2b_company where id=@comid)";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@proid", proid);
            cmd.AddParam("@comid", comid);

            using (var reader = cmd.ExecuteReader())
            {


                if (reader.Read())
                {
                    return reader.GetValue<int>("warrant_type");
                }
                return 0;
            }
        }
        #endregion


        #region 后台充入预付款
        internal decimal Hand_Imprest(int agentid, int comid, string startime = "", string endtime = "")
        {


            string sqlTxt = @"select sum(money) as money from agent_Financial where agentid=@agentid and comid=@comid and payment=0 and orderid=0 and payment_type='分销充值' and Rebatetype=0 ";

            if (startime != "")
            {
                sqlTxt += " and subtime>=@startime ";
            }

            if (endtime != "")
            {
                sqlTxt += " and subtime<@endtime";
            }

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@agentid", agentid);
                cmd.AddParam("@comid", comid);
                cmd.AddParam("@startime", startime);
                cmd.AddParam("@endtime", endtime);
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


        #region 分销后台支付预付款
        internal decimal HandOut_Imprest(int agentid, int comid, string startime = "", string endtime = "")
        {


            string sqlTxt = @"select sum(money) as money from agent_Financial where agentid=@agentid and comid=@comid and payment=1 and orderid=0 and payment_type='分销扣款'";


            if (startime != "")
            {
                sqlTxt += " and subtime>=@startime ";
            }

            if (endtime != "")
            {
                sqlTxt += " and subtime<@endtime";
            }
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@agentid", agentid);
                cmd.AddParam("@comid", comid);
                cmd.AddParam("@startime", startime);
                cmd.AddParam("@endtime", endtime);
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


        #region 后台充入返点
        internal decimal Hand_Rebate(int agentid, int comid, string startime = "", string endtime = "")
        {
            string sqlTxt = @"select sum(money) as money from agent_Financial where agentid=@agentid and comid=@comid and payment=0 and orderid=0 and payment_type='分销充值' and Rebatetype=1";

            if (startime != "")
            {
                sqlTxt += " and subtime>=@startime ";
            }

            if (endtime != "")
            {
                sqlTxt += " and subtime<@endtime";
            }
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@agentid", agentid);
                cmd.AddParam("@comid", comid);
                cmd.AddParam("@startime", startime);
                cmd.AddParam("@endtime", endtime);
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

        #region 网上支付预付款
        internal decimal Line_Imprest(int agentid, int comid, string startime = "", string endtime = "")
        {
            string sqlTxt = @"select sum(money) as money from agent_Financial where agentid=@agentid and comid=@comid and payment=0 and orderid!=0 and payment_type='分销充值'";
            if (startime != "")
            {
                sqlTxt += " and subtime>=@startime ";
            }

            if (endtime != "")
            {
                sqlTxt += " and subtime<@endtime";
            }

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@agentid", agentid);
                cmd.AddParam("@comid", comid);
                cmd.AddParam("@startime", startime);
                cmd.AddParam("@endtime", endtime);
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

        #region 销售额
        internal decimal Sale_price(int agentid, int comid, int projectid = 0, string startime = "", string endtime = "")
        {
            string sqlTxt = @"select sum(u_num*pay_price) as money from b2b_order where agentid=@agentid and comid=@comid and warrant_type=1 and order_type=1 and pay_state=2";

            if (projectid != 0)
            {
                sqlTxt += " and pro_id in (select id from b2b_com_pro where projectid=" + projectid + ")";
            }
            if (startime != "")
            {
                sqlTxt += " and u_subdate>= '" + startime + "'";
            }

            if (endtime != "")
            {
                sqlTxt += " and u_subdate< '" + endtime + "'";
            }

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@agentid", agentid);
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


        #region 出票情况
        internal int Outticketnum(int agentid, int comid, int projectid = 0, string startime = "", string endtime = "")
        {
            string sqlTxt = @"select sum(u_num) as money from b2b_order where agentid=@agentid and comid=@comid and pay_state=2";

            if (projectid != 0)
            {
                sqlTxt += " and pro_id in (select id from b2b_com_pro where projectid=" + projectid + ")";
            }
            if (startime != "")
            {
                sqlTxt += " and u_subdate>= '" + startime + "'";
            }

            if (endtime != "")
            {
                sqlTxt += " and u_subdate< '" + endtime + "'";
            }

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@agentid", agentid);
                cmd.AddParam("@comid", comid);
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
        #endregion

        #region 验票情况
        internal int Yanzhengticketnum(int agentid, int comid, int projectid = 0, string startime = "", string endtime = "")
        {


            string sqlTxt = @"select sum(b.use_pnum) as money from b2b_eticket as a left join b2b_etcket_log as b on  a.id=b.eticket_id left join b2b_order as c on a.oid=c.id  where b.action=1 and b.a_state=1 and (a.oid in (select id from b2b_order where agentid=@agentid and comid=@comid and  pay_state=2) or a.oid in (select bindingagentorderid from b2b_order where agentid=@agentid and comid=@comid and  pay_state=2))";
            
            if (projectid != 0)
            {
                sqlTxt += " and a.pro_id in (select id from b2b_com_pro where projectid=" + projectid + ")";
            }
            if (startime != "")
            {
                sqlTxt += " and b.actiondate>= '" + startime + "'";
            }

            if (endtime != "")
            {
                sqlTxt += " and b.actiondate< '" + endtime + "'";
            }

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@agentid", agentid);
                cmd.AddParam("@comid", comid);
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
        #endregion


        #region 毛利
        internal decimal Maoli_price(int agentid, int comid, int projectid = 0, string startime = "", string endtime = "")
        {
            string sqlTxt = @"select sum(u_num*profit) as money from b2b_order where agentid=@agentid and comid=@comid and warrant_type=1 and  pay_state=2";
            if (projectid != 0)
            {
                sqlTxt += " and pro_id in (select id from b2b_com_pro where projectid=" + projectid + ")";
            }
            if (startime != "")
            {
                sqlTxt += " and u_subdate>= '" + startime + "'";
            }

            if (endtime != "")
            {
                sqlTxt += " and u_subdate< '" + endtime + "'";
            }

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@agentid", agentid);
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


        #region 倒码销售额
        internal decimal Daoma_Sale_price(int agentid, int comid, int projectid = 0, string startime = "", string endtime = "")
        {
            string sqlTxt = @"select sum(u_num*pay_price) as money from b2b_order where agentid=@agentid and comid=@comid and warrant_type=2 and  pay_state=2";
            if (projectid != 0)
            {
                sqlTxt += " and pro_id in (select id from b2b_com_pro where projectid=" + projectid + ")";
            }
            if (startime != "")
            {
                sqlTxt += " and u_subdate>= '" + startime + "'";
            }

            if (endtime != "")
            {
                sqlTxt += " and u_subdate< '" + endtime + "'";
            }
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@agentid", agentid);
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

        #region 倒码毛利，按已验证数量
        internal decimal Daoma_Maoli_price(int agentid, int comid, int projectid = 0, string startime = "", string endtime = "")
        {
            string sqlTxt = @"select sum(c.use_pnum*a.profit) as money from b2b_order as a left join b2b_eticket as b on  a.id=b.oid  left join b2b_etcket_log as c on  b.id=c.eticket_id where  b.v_state=2 and c.action=1 and c.a_state=1 and (a.id in(select idfrom b2b_order where agentid=@agentid and comid=@comid and warrant_type=2 and pay_state=2) or a.id in(select bindingagentorderid from b2b_order where agentid=@agentid and comid=@comid and warrant_type=2 and pay_state=2))";

            if (projectid != 0)
            {
                sqlTxt += " and a.pro_id in (select id from b2b_com_pro where projectid=" + projectid + ")";
            }
            if (startime != "")
            {
                sqlTxt += " and u_subdate>= '" + startime + "'";
            }

            if (endtime != "")
            {
                sqlTxt += " and u_subdate< '" + endtime + "'";
            }
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@agentid", agentid);
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


        #region 已消费
        internal decimal Xiaofei_price(int agentid, int comid, int projectid = 0, string startime = "", string endtime = "")
        {
            //const string sqlTxt = @"select sum(money) as money  from agent_financial where comid=@comid and agentid=@agentid  and orderid !=0 ";
            //所有验证数据
            string sqlTxt = @"select sum(b.use_pnum*a.e_sale_price) as money from b2b_eticket as a left join b2b_etcket_log as b on  a.id=b.eticket_id left join b2b_order as c on a.oid=c.id  where b.action=1 and b.a_state=1 and (a.oid in (select id from b2b_order where agentid=@agentid and comid=@comid and  pay_state=2) or a.oid in (select bindingagentorderid from b2b_order where agentid=@agentid and comid=@comid and  pay_state=2))";
            if (projectid != 0)
            {
                sqlTxt += " and c.pro_id in (select id from b2b_com_pro where projectid=" + projectid + ")";
            }
            if (startime != "")
            {
                sqlTxt += " and b.actiondate>= '" + startime + "'";
            }

            if (endtime != "")
            {
                sqlTxt += " and b.actiondate< '" + endtime + "'";
            }


            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@agentid", agentid);
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


        #region 分销导入产品已消费
        internal decimal AgentDaoru_Xiaofei_price(int agentid, int comid, int projectid = 0, string startime = "", string endtime = "")
        {

            //所有验证数据
            string sqlTxt = @"select sum(b.use_pnum*c.pay_price) as money from b2b_eticket as a left join b2b_etcket_log as b on  a.id=b.eticket_id left join b2b_order as c on a.oid=c.bindingagentorderid  where b.action=1 and b.a_state=1 and a.oid in (select bindingagentorderid from b2b_order where agentid=@agentid and comid=@comid and order_state in (4,8,22) and bindingagentorderid!=0)";
            if (projectid != 0)
            {
                sqlTxt += " and c.pro_id in (select id from b2b_com_pro where projectid=" + projectid + ")";
            }
            if (startime != "")
            {
                sqlTxt += " and b.actiondate>= '" + startime + "'";
            }

            if (endtime != "")
            {
                sqlTxt += " and b.actiondate< '" + endtime + "'";
            }

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@agentid", agentid);
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

        #region 已消费 不用了，消费 按财务，统一统计
        internal decimal Daoma_Xiaofei_price(int agentid, int comid, int projectid = 0)
        {
            string sqlTxt = @"select sum(money) as money  from agent_financial where comid=@comid and agentid=@agentid  and orderid !=0 ";

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@agentid", agentid);
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
        internal decimal Chendian_price(int agentid, int comid, int projectid = 0, string startime = "", string endtime = "")
        {
            string sqlTxt = @"select sum(a.use_pnum*a.e_sale_price) as money from b2b_eticket as a left join b2b_order as b on a.oid=b.id where  a.oid in (select id from b2b_order where agentid=@agentid and comid=@comid and warrant_type=1 and  pay_state=2) or  a.oid in (select bindingagentorderid from b2b_order where agentid=@agentid and comid=@comid and warrant_type=1 and  pay_state=2)";
            if (projectid != 0)
            {
                sqlTxt += " and b.pro_id in (select id from b2b_com_pro where projectid=" + projectid + ")";
            }

            if (startime != "")
            {
                sqlTxt += " and a.subdate>= '" + startime + "'";
            }

            if (endtime != "")
            {
                sqlTxt += " and a.subdate< '" + endtime + "'";
            }

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@agentid", agentid);
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

        #region 分销导入产品沉淀
        internal decimal AgentDaoru_Chendian_price(int agentid, int comid, int projectid = 0, string startime = "", string endtime = "")
        {
            string sqlTxt = @"select sum(a.use_pnum*b.pay_price) as money from b2b_eticket as a left join b2b_order as b  on b.bindingagentorderid=a.oid where a.oid in (select bindingagentorderid from b2b_order where agentid=@agentid and comid=@comid and warrant_type=1 and  pay_state=2 and bindingagentorderid!=0)";
            if (projectid != 0)
            {
                sqlTxt += " and b.pro_id in (select id from b2b_com_pro where projectid=" + projectid + ")";
            }
            if (startime != "")
            {
                sqlTxt += " and a.subdate>= '" + startime + "'";
            }

            if (endtime != "")
            {
                sqlTxt += " and a.subdate< '" + endtime + "'";
            }

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@agentid", agentid);
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



        #region 根据分销id，comid 入款
        internal int WriteAgentMoney(string acttype, decimal money, int agentid, int comid, int warrantid)
        {
            string sqlTxt = @"update agent_warrant set imrest=imprest+@money where id=@warrantid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@warrantid", warrantid);
            cmd.AddParam("@money", money);
            return cmd.ExecuteNonQuery();
        }
        #endregion


        #region 子分销扣款
        internal int WriteAgentSunMoney(decimal money, int agentsunid)
        {
            string sqlTxt = @"update agent_company_manageuser set amount=amount-@money where id=@agentsunid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@agentsunid", agentsunid);
            cmd.AddParam("@money", money);
            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region 分销登陆
        internal string Login(string email, string pwd, out Agent_company userinfo)
        {
            string sqlTxt = @"SELECT  b.id,b.company,b.name,b.address,b.mobile,b.tel,b.run_state,a.pwd,a.accountLevel,a.Amount
              FROM  [agent_company_manageuser] as a left join agent_company as b  on a.agentid=b.ID where a.account =@email";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@email", email);

            using (var reader = cmd.ExecuteReader())
            {
                Agent_company u = null;

                if (reader.Read())
                {
                    if (reader.GetValue<string>("pwd") == pwd)
                    {
                        if (reader.GetValue<int>("run_state") == -1)
                        {
                            userinfo = null;
                            return "账户已暂停";
                        }
                        u = new Agent_company
                        {
                            Id = reader.GetValue<int>("id"),
                            Company = reader.GetValue<string>("Company"),
                            Contentname = reader.GetValue<string>("name"),
                            Address = reader.GetValue<string>("Address"),
                            Mobile = reader.GetValue<string>("Mobile"),
                            Tel = reader.GetValue<string>("Tel"),
                            Run_state = reader.GetValue<int>("Run_state"),
                            AccountLevel = reader.GetValue<int>("AccountLevel")
                        };
                    }
                    else
                    {
                        userinfo = null;
                        return "登陆账户与密码匹配错误";
                    }
                }

                if (u != null)
                {
                    userinfo = u;
                    return "OK";
                }
                else
                {
                    userinfo = null;
                    return "没有查询到账户";
                }


            }
        }
        #endregion


        #region 判断邮箱
        internal int GetEmail(string email)
        {
            const string sqlTxt = @"SELECT  *
              FROM  [agent_company_manageuser] where account =@email";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@email", email);

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



        #region 根据域名判断分销商
        internal int DomainGetAgentid(string domain)
        {
            const string sqlTxt = @"SELECT  *
              FROM  [agent_company] where agent_domain =@domain";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@domain", domain);

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


        #region 绑定分销商
        internal int BindingAgent(int comid, int agentid)
        {
            const string sqlTxt = @"update b2b_company set BindingAgent = @agentid Where id=@comid;select @@identity;";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@agentid", agentid);
            return cmd.ExecuteNonQuery();

        }
        #endregion

        #region 解除绑定分销商
        internal int UnBindingAgent(int comid)
        {
            const string sqlTxt = @"update b2b_company set BindingAgent = 0 Where id=@comid;select @@identity;";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);

            cmd.AddParam("@comid", comid);
            return cmd.ExecuteNonQuery();

        }
        #endregion

        #region 判断邮箱
        internal int GetPhone(string phone)
        {
            const string sqlTxt = @"SELECT  *
              FROM  [agent_company_manageuser] where mobile =@phone";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@phone", phone);

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

        #region 查询分销商手机
        internal int Agentsearch(string phone)
        {
            const string sqlTxt = @"SELECT  *
              FROM  [agent_company] where mobile =@phone";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@phone", phone);

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

        #region 分销商注册账户
        internal int RegiAccount(Agent_company model)
        {
            string procsql = "usp_EditAgentComapny";

            var cmd = sqlHelper.PrepareStoredSqlCommand(procsql);
            //注册公司基本信息
            cmd.AddParam("@Agentid", model.Id);
            cmd.AddParam("@Company", model.Company);
            cmd.AddParam("@Tel", model.Tel);
            cmd.AddParam("@Mobile", model.Mobile);
            cmd.AddParam("@Contentname", model.Contentname);
            cmd.AddParam("@Address", model.Address);
            cmd.AddParam("@Run_state", model.Run_state);
            cmd.AddParam("@Agentsort", model.Agentsort);
            //登陆信息
            cmd.AddParam("@Account", model.Account);
            cmd.AddParam("@Pwd", model.Pwd);
            cmd.AddParam("@AccountLevel", model.AccountLevel);
            cmd.AddParam("@Amount", model.Amount);

            cmd.AddParam("@com_province", model.com_province);
            cmd.AddParam("@com_city", model.com_city);

            cmd.AddParam("@agentsourcesort", model.agentsourcesort);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion

        #region 分销商信息修改
        internal int RegiUpCompany(Agent_company model)
        {
            const string sqlTxt = @"update agent_company set  name = @Contentname,Tel =@Tel,Mobile =@Mobile,Address =@Address,Agent_domain=@Agent_domain Where id=@Agentid;select @@identity;";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            //修改授权信息
            cmd.AddParam("@Agentid", model.Id);
            cmd.AddParam("@Contentname", model.Contentname);
            cmd.AddParam("@Tel", model.Tel);
            cmd.AddParam("@Mobile", model.Mobile);
            cmd.AddParam("@Address", model.Address);
            cmd.AddParam("@Agent_domain", model.Agent_domain);

            return cmd.ExecuteNonQuery();

        }
        #endregion

        #region 分销商信息修改
        internal int AdminRegiUpCompany(Agent_company model)
        {
            const string sqlTxt = @"update agent_company set Agent_type=@Agent_type,Lvmama_Apikey=@Lvmama_Apikey,Lvmama_password=@Lvmama_password,Lvmama_uid=@Lvmama_uid,ismeituan=@ismeituan,mt_partnerId=@mt_partnerId,mt_client=@mt_client,mt_secret=@mt_secret,mt_mark=@mt_mark, maxremindmoney=@maxremindmoney, istaobao=@istaobao,tb_syncurl=@tb_syncurl,tb_isret_consumeresult=@tb_isret_consumeresult,Agent_domain=@Agent_domain,  com_province = @com_province, com_city = @com_city, name = @Contentname,Tel =@Tel,Mobile =@Mobile,Address =@Address,Company=@Company,Run_state=@Run_state Where id=@Agentid;select @@identity;";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            //修改授权信息
            cmd.AddParam("@Agentid", model.Id);
            cmd.AddParam("@Contentname", model.Contentname);
            cmd.AddParam("@Tel", model.Tel);
            cmd.AddParam("@Mobile", model.Mobile);
            cmd.AddParam("@Address", model.Address);
            cmd.AddParam("@Company", model.Company);
            cmd.AddParam("@Run_state", model.Run_state);
            cmd.AddParam("@com_province", model.com_province);
            cmd.AddParam("@com_city", model.com_city);
            cmd.AddParam("@Agent_domain", model.Agent_domain);

            cmd.AddParam("@istaobao", model.istaobao);
            cmd.AddParam("@tb_syncurl", model.tb_syncurl);
            cmd.AddParam("@tb_isret_consumeresult", model.tb_isret_consumeresult);

            cmd.AddParam("@maxremindmoney", model.maxremindmoney);

            cmd.AddParam("@ismeituan",model.ismeituan);
            cmd.AddParam("@mt_partnerId",model.mt_partnerId);
            cmd.AddParam("@mt_client",model.mt_client);
            cmd.AddParam("@mt_secret",model.mt_secret);
            cmd.AddParam("@mt_mark",model.mt_mark);

            cmd.AddParam("@Lvmama_uid", model.Lvmama_uid);
            cmd.AddParam("@Lvmama_password", model.Lvmama_password);
            cmd.AddParam("@Lvmama_Apikey", model.Lvmama_Apikey);

            cmd.AddParam("@Agent_type", model.Agent_type);


            return cmd.ExecuteNonQuery();

        }
        #endregion


        #region 分销商登陆账户修改
        internal int ManageUpAccount(Agent_regiinfo model)
        {
            sqlHelper.BeginTrancation();
            try
            {
                string sqlTxt = "";
                if (model.Id != 0)
                {
                    sqlTxt = @"update  agent_company_manageuser set account = @Account,pwd =@Pwd,Mobile =@Mobile,name =@name,amount =@Amount Where id=@Id;";
                    var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                    //修改授权信息
                    cmd.AddParam("@Id", model.Id);
                    cmd.AddParam("@Pwd", model.Pwd);
                    cmd.AddParam("@name", model.Contentname);
                    cmd.AddParam("@Account", model.Account);
                    cmd.AddParam("@Mobile", model.Mobile);
                    cmd.AddParam("@Amount", model.Amount);
                    cmd.AddParam("@agentid", model.Agentid);
                    cmd.ExecuteNonQuery();


                    //判断账户是否是开户账户
                    string sql2 = "select accountlevel from  agent_company_manageuser where id=" + model.Id;
                    cmd = sqlHelper.PrepareTextSqlCommand(sql2);
                    object o = cmd.ExecuteScalar();
                    if (o.ToString() == "0")
                    {
                        //开户账户 修改信息时，公司信息(手机、联系人姓名)需要同步
                        string sql3 = "update agent_company set mobile='" + model.Mobile + "' ,name='" + model.Contentname + "' where id=" + model.Agentid;
                        cmd = sqlHelper.PrepareTextSqlCommand(sql3);
                        cmd.ExecuteNonQuery();
                    }


                }
                else
                {
                    sqlTxt = @"insert agent_company_manageuser (agentid,account,pwd,Mobile,name,amount,accountlevel )values(@agentid,@Account,@Pwd,@Mobile,@name,@Amount,1);select @@identity;";
                    var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                    //修改授权信息
                    cmd.AddParam("@Id", model.Id);
                    cmd.AddParam("@Pwd", model.Pwd);
                    cmd.AddParam("@name", model.Contentname);
                    cmd.AddParam("@Account", model.Account);
                    cmd.AddParam("@Mobile", model.Mobile);
                    cmd.AddParam("@Amount", model.Amount);
                    cmd.AddParam("@agentid", model.Agentid);
                    object o = cmd.ExecuteScalar();
                    model.Id = int.Parse(o.ToString());
                }




                sqlHelper.Commit();
                sqlHelper.Dispose();
                return 1;
            }
            catch
            {
                sqlHelper.Rollback();
                sqlHelper.Dispose();
                return 0;
            }
        }
        #endregion

        #region 分销商登陆账户查询
        internal Agent_regiinfo AgentSearchAccount(string account)
        {
            string sqlTxt = @"select * from agent_company_manageuser where account=@account";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@account", account);
            Agent_regiinfo u = null;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    u = new Agent_regiinfo
                    {
                        Id = reader.GetValue<int>("id"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Account = reader.GetValue<string>("Account"),
                        Mobile = reader.GetValue<string>("Mobile"),
                        Pwd = reader.GetValue<string>("Pwd"),
                        Amount = reader.GetValue<decimal>("Amount"),
                        Contentname = reader.GetValue<string>("name"),
                        AccountLevel = reader.GetValue<int>("AccountLevel"),

                    };
                }
            }
            return u;
        }
        #endregion

        #region 分销商开户账户
        internal string GetAgentAccount(int agentid)
        {
            string sqlTxt = @"select * from agent_company_manageuser where agentid=@agentid  and accountlevel=0";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@agentid", agentid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<string>("account");
                }
            }
            return "";
        }
        #endregion

        #region 分销商开户账户
        internal decimal GetAgentRebatemoney(int agentid, int comid)
        {
            string sqlTxt = @"select SUM(Money) as money  from [agent_Financial] where agentid=@agentid  and comid=@comid and Rebatetype=1 ";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.AddParam("@agentid", agentid);
                cmd.AddParam("@comid", comid);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetValue<decimal>("money");
                    }
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }
        #endregion



        #region 分销商登陆账户修改
        internal int RegiUpAccount(Agent_regiinfo model)
        {
            string sqlTxt = "";
            sqlTxt = @"update  agent_company_manageuser set account = @Account,pwd =@Pwd,Mobile =@Mobile,name =@name,amount =@Amount Where id=@Id;select @@identity;";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            //修改授权信息
            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Pwd", model.Pwd);
            cmd.AddParam("@name", model.Contentname);
            cmd.AddParam("@Account", model.Account);
            cmd.AddParam("@Mobile", model.Mobile);
            cmd.AddParam("@Amount", model.Amount);
            cmd.AddParam("@agentid", model.Agentid);
            return cmd.ExecuteNonQuery();

        }
        #endregion



        #region 授权列表
        internal List<Agent_warrant> Warrantpagelist(int Pageindex, int Pagesize, string Key, int Agentid, out int totalcount, string comstate = "1,2", string offcomids = "")
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = "agentid=" + Agentid + " and warrant_state=1";

            if (Key != "")
            {
                condition += " and comid in (select id from b2b_company where com_name like '%" + Key + "%')";
            }
            if (comstate != "1,2")
            {
                condition += " and comid in (select id from b2b_company where com_state=" + comstate + ")";
            }
            if (offcomids != "")
            {
                condition += " and comid not in (" + offcomids + ")";
            }

            cmd.PagingCommand1("agent_warrant", "*", "comid", "", Pagesize, Pageindex, "", condition);


            List<Agent_warrant> list = new List<Agent_warrant>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Agent_warrant
                    {
                        Id = reader.GetValue<int>("id"),
                        Comid = reader.GetValue<int>("Comid"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_state = reader.GetValue<int>("Warrant_state"),
                        Imprest = reader.GetValue<decimal>("Imprest"),
                        Credit = reader.GetValue<decimal>("Credit"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrant_level = reader.GetValue<int>("Warrant_level"),

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());
            return list;

        }

        #endregion


        #region 更多供应商列表
        internal List<B2b_company> UnWarrantpagelist(int Pageindex, int Pagesize, string Key, int Agentid, out int totalcount, string offcomids = "")
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");

            var condition = "com_state=1 and lp=1 ";
            if (Key != "")
            {
                condition += " and com_name like '%" + Key + "%'";
            }
            if (offcomids != "")
            {
                condition += " and id not in (" + offcomids + ")";
            }
            condition = condition + " and   id not in (select comid from agent_warrant where agentid=" + Agentid + " )";

            cmd.PagingCommand1("b2b_company", "*", "id", "", Pagesize, Pageindex, "", condition);

            List<B2b_company> list = new List<B2b_company>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company
                    {

                        ID = reader.GetValue<int>("Id"),
                        Com_name = reader.GetValue<string>("Com_name"),

                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());
            return list;

        }

        #endregion


        #region 分销商所开通商户列表
        internal List<B2b_company> AgentComlist(int Pageindex, int Pagesize, string Key, int Agentid, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_company";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = "agentid=" + Agentid;

            if (Key != "")
            {
                condition = condition + " and com_name like '%" + Key + "%'";
            }

            cmd.PagingCommand(tblName, strGetFields, Pageindex, Pagesize, sortKey, sortMode, condition);


            List<B2b_company> list = new List<B2b_company>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company
                    {
                        ID = reader.GetValue<int>("id"),
                        Com_name = reader.GetValue<string>("com_name"),
                        Com_state = reader.GetValue<int>("com_state"),
                        Com_type = reader.GetValue<int>("com_type"),
                        Scenic_name = reader.GetValue<string>("Scenic_name"),
                        Imprest = reader.GetValue<decimal>("imprest"),
                        Fee = reader.GetValue<decimal>("fee"),
                        ServiceFee = reader.GetValue<decimal>("Servicefees"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Agentopenstate = reader.GetValue<int>("agentopenstate"),
                        OpenDate = reader.GetValue<DateTime>("Opendate"),
                        EndDate = reader.GetValue<DateTime>("Enddate"),
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());
            return list;

        }

        #endregion


        #region 分销商登陆账户列表
        internal List<Agent_regiinfo> Accountlist(int Pageindex, int Pagesize, int agentid, out int totalcount, int accountid = 0)
        {
            int accountlevel = 0;//判断账户级别:0 开户账户；1 新添加的员工
            if (accountid != 0)
            {
                Agent_regiinfo md = AgentCompanyData.GetAgentAccountByid(accountid);
                if (md == null)
                {
                    totalcount = 0;
                    return new List<Agent_regiinfo>();
                }
                else
                {
                    accountlevel = md.AccountLevel;
                }
            }


            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "agent_company_manageuser";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = "agentid=" + agentid;
            if (accountlevel != 0)
            {
                condition += "  and id=" + accountid;
            }

            cmd.PagingCommand(tblName, strGetFields, Pageindex, Pagesize, sortKey, sortMode, condition);


            List<Agent_regiinfo> list = new List<Agent_regiinfo>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Agent_regiinfo
                    {
                        Id = reader.GetValue<int>("id"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Account = reader.GetValue<string>("Account"),
                        Mobile = reader.GetValue<string>("Mobile"),
                        Pwd = reader.GetValue<string>("Pwd"),
                        Amount = reader.GetValue<decimal>("Amount"),
                        Contentname = reader.GetValue<string>("name"),
                        AccountLevel = reader.GetValue<int>("AccountLevel"),

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());
            return list;

        }

        #endregion


        #region 分销商授权列表
        internal List<Agent_warrant> Agentpagelist(int Pageindex, int Pagesize, string Key, int comid, out int totalcount, string com_province = "", string com_city = "", string warrantstate = "0,1", int agentsourcesort = 0,string orderby="")
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "agent_warrant";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "0";
            var condition = "comid=" + comid;

            if (Key != "")
            {
                condition = condition + " and agentid in (select id from agent_company where company like '%" + Key + "%' or id in (select agentid from agent_company_manageuser where account='" + Key + "' or name='" + Key + "' or mobile='" + Key + "' ))";
            }

            if (com_province != "")
            {
                condition += " and agentid in (select id from agent_company where  com_province='" + com_province + "')";
            }
            if (com_city != "")
            {
                condition += " and agentid in (select id from agent_company where  com_city='" + com_city + "')";
            }

            
            condition += " and warrant_state in (" + warrantstate + ",-1)";



            if (agentsourcesort != 0)
            {
                condition += " and agentid in (select id from agent_company where agent_sourcesort=" + agentsourcesort + ")";
            }


            if (orderby != "") {
                //sortKey = orderby;
            }

            cmd.PagingCommand(tblName, strGetFields, Pageindex, Pagesize, sortKey, sortMode, condition);


            List<Agent_warrant> list = new List<Agent_warrant>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Agent_warrant
                    {
                        Id = reader.GetValue<int>("id"),
                        Comid = reader.GetValue<int>("Comid"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_state = reader.GetValue<int>("Warrant_state"),
                        Imprest = reader.GetValue<decimal>("Imprest"),
                        Credit = reader.GetValue<decimal>("Credit"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrant_level = reader.GetValue<int>("Warrant_level"),
                        Warrant_lp = reader.GetValue<int>("Warrant_lp"),

                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());
            return list;

        }
        #endregion

        #region 分销商特别授权
        internal int SetWarrant(int agentid, int type, int proid, int comid)
        {
            try
            {
                string sqlTxt = @"insert b2b_agent_prowarrant(agentid,comid,proid)values(@Agentid,@Comid,@proid);select @@identity;";
                if (type == 0)
                { //删除授权
                    sqlTxt = @"delete b2b_agent_prowarrant where agentid=@Agentid and comid=@Comid and proid=@proid;select @@identity;";
                }
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                //修改授权信息
                cmd.AddParam("@agentid", agentid);
                cmd.AddParam("@Comid", comid);
                cmd.AddParam("@proid", proid);
                return cmd.ExecuteNonQuery();
            }
            catch
            {
                return 0;
            }

        }

        #endregion


        #region 查询特别授权信息
        internal int SearchSetWarrant(int agentid, int proid, int comid)
        {
            const string sqlTxt = @"select * from b2b_agent_prowarrant Where agentid=@Agentid and comid=@Comid and proid=@proid;select @@identity;";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.AddParam("@Agentid", agentid);
                cmd.AddParam("@Comid", comid);
                cmd.AddParam("@proid", proid);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetValue<int>("id");
                    }
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }
        #endregion



        #region 分销商列表
        internal List<Agent_company> ManageAgentpagelist(int Pageindex, int Pagesize, string Key, out int totalcount, string com_province, string com_city, int agentsourcesort = 0)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "agent_company";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = "1=1 ";

            if (Key != "")
            {
                condition += " and  (company like '%" + Key + "%' or mobile='" + Key + "' or name='" + Key + "') ";
            }
            if (com_province != "")
            {
                condition += " and com_province ='" + com_province + "'";
            }
            if (com_city != "")
            {
                condition += " and com_city ='" + com_city + "'";
            }
            if (agentsourcesort != 0)
            {
                condition += " and agent_sourcesort =" + agentsourcesort;
            }

            cmd.PagingCommand(tblName, strGetFields, Pageindex, Pagesize, sortKey, sortMode, condition);


            List<Agent_company> list = new List<Agent_company>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Agent_company
                    {

                        Id = reader.GetValue<int>("id"),
                        Company = reader.GetValue<string>("Company"),
                        Tel = reader.GetValue<string>("Tel"),
                        Mobile = reader.GetValue<string>("Mobile"),
                        Contentname = reader.GetValue<string>("name"),
                        Address = reader.GetValue<string>("Address"),
                        Run_state = reader.GetValue<int>("Run_state"),
                        Agentsort = reader.GetValue<int>("Agent_sort"),
                        Agent_domain = reader.GetValue<string>("Agent_domain"),
                        Weixin_img = reader.GetValue<int>("Weixin_img"),
                        Agent_messagesetting = reader.GetValue<int>("Agent_messagesetting"),
                        agentsourcesort = reader.GetValue<int>("Agent_sourcesort"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());
            return list;

        }

        #endregion


        #region 修改授权信息（现在只修改状态）
        internal int ModifyAgentInfo(Agent_company model)
        {
            const string sqlTxt = @"update agent_warrant set warrant_state = @Warrant_state,Warrant_type =@Warrant_type,Warrant_level =@Warrant_level Where comid=@Comid and agentid=@Agentid;select @@identity;";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            //修改授权信息
            cmd.AddParam("@Agentid", model.Id);
            cmd.AddParam("@Comid", model.Comid);
            cmd.AddParam("@Warrant_state", model.Warrant_state);
            cmd.AddParam("@Warrant_type", model.Warrant_type);
            cmd.AddParam("@Warrant_level", model.Warrant_level);
            return cmd.ExecuteNonQuery();
        }
        #endregion


        #region 新增授权
        internal int AddAgentInfo(Agent_company model)
        {
            const string sqlTxt = @"insert agent_warrant(agentid,comid,warrant_state,warrant_type,warrant_level,imprest,Warrant_lp)values(@Agentid,@Comid,@Warrant_state,@Warrant_type,@Warrant_level,0,@Warrant_lp);select @@identity;";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            //修改授权信息
            cmd.AddParam("@Agentid", model.Id);
            cmd.AddParam("@Comid", model.Comid);
            cmd.AddParam("@Warrant_state", model.Warrant_state);
            cmd.AddParam("@Warrant_type", model.Warrant_type);
            cmd.AddParam("@Warrant_level", model.Warrant_level);
            cmd.AddParam("@Warrant_lp", model.Warrant_lp);
            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region 项目授权
        internal int AddAgentProject(Agent_company model)
        {
            const string sqlTxt = @"insert b2b_project_agent(agentid,comid,[projectid])values(@Agentid,@Comid,@Projectid);select @@identity;";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            //修改授权信息
            cmd.AddParam("@Agentid", model.Id);
            cmd.AddParam("@Comid", model.Comid);
            cmd.AddParam("@Projectid", model.Projectid);

            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region 查询是否有授权信息
        internal int SearchAgentwarrant(Agent_company model)
        {
            const string sqlTxt = @"select * from  agent_warrant Where comid=@Comid and agentid=@Agentid;select @@identity;";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            //修改授权信息
            cmd.AddParam("@Agentid", model.Id);
            cmd.AddParam("@Comid", model.Comid);

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




        #region 查询未授权分销商列表
        internal List<Agent_regiinfo> Unagentlist(int Pageindex, int Pagesize, string Key, int comid, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "agent_company";
            var strGetFields = "id,company,tel,mobile,name,com_province,com_city,agent_sourcesort";
            var sortKey = "id";
            var sortMode = "1";
            var condition = "";

            if (comid == 106)
            {
                condition = "id !=0";
            }
            else
            {
                condition = "agent_sort=0";
            }

            condition = condition + " and  run_state=0";

            if (Key != "")
            {
                condition = condition + " and (company like '%" + Key + "%' or id in (select agentid from agent_company_manageuser where account='" + Key + "' or mobile='" + Key + "' or name = '" + Key + "')) and not id in (select agentid from agent_warrant where comid=" + comid + ") ";
            }
            else
            {
                condition = condition + " and not id in (select agentid from agent_warrant where comid=" + comid + ") and not id in (1,2,4) ";
            }



            cmd.PagingCommand(tblName, strGetFields, Pageindex, Pagesize, sortKey, sortMode, condition);


            List<Agent_regiinfo> list = new List<Agent_regiinfo>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Agent_regiinfo
                    {
                        Id = reader.GetValue<int>("id"),
                        Company = reader.GetValue<string>("Company"),
                        Tel = reader.GetValue<string>("Tel"),
                        Mobile = reader.GetValue<string>("Mobile"),
                        Contentname = reader.GetValue<string>("name"),
                        Com_province = reader.GetValue<string>("Com_province"),
                        Com_city = reader.GetValue<string>("Com_city"),
                        Agent_sourcesort = reader.GetValue<int>("Agent_sourcesort"),


                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());
            return list;

        }

        #endregion

        #region 查询未授权分销商列表
        internal List<Agent_Financial> AgentFinacelist(int Pageindex, int Pagesize, int agentid, int comid, out int totalcount, int recharge=0)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "agent_Financial";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = " agentid = " + agentid + "and comid=" + comid + " ";

            if (recharge == 1) {
                condition += " and Payment_type='分销充值'";
            }


            cmd.PagingCommand(tblName, strGetFields, Pageindex, Pagesize, sortKey, sortMode, condition);

            List<Agent_Financial> list = new List<Agent_Financial>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Agent_Financial
                    {
                        Id = reader.GetValue<int>("id"),
                        Agentid = reader.GetValue<int>("agentid"),
                        Warrantid = reader.GetValue<int>("Warrantid"),
                        Com_id = reader.GetValue<int>("Comid"),
                        Order_id = reader.GetValue<int>("Orderid"),
                        Servicesname = reader.GetValue<string>("Servicesname"),
                        Money = reader.GetValue<decimal>("Money"),
                        Over_money = reader.GetValue<decimal>("Over_money"),
                        Money_come = reader.GetValue<string>("money_come"),

                        Payment_type = reader.GetValue<string>("payment_type"),
                        Payment = reader.GetValue<int>("Payment"),
                        Remarks = reader.GetValue<string>("Remarks"),
                        Subdate = reader.GetValue<DateTime>("subtime"),
                        Rebatetype = reader.GetValue<int>("rebatetype"),
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());
            return list;

        }

        #endregion

        #region 分销扣款插入数据库
        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateAgentFinance";
        internal int AgentFinancial(Agent_Financial model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);
            cmd.AddParam("@Comid", model.Com_id);
            cmd.AddParam("@Agentid", model.Agentid);
            cmd.AddParam("@Warrantid", model.Warrantid);
            cmd.AddParam("@Orderid", model.Order_id);
            cmd.AddParam("@Servicesname", model.Servicesname);
            cmd.AddParam("@SerialNumber", model.SerialNumber);
            cmd.AddParam("@Money", model.Money);
            cmd.AddParam("@Money_come", model.Money_come);
            cmd.AddParam("@Over_money", model.Over_money);
            cmd.AddParam("@Payment", model.Payment);
            cmd.AddParam("@Payment_type", model.Payment_type);
            cmd.AddParam("@Rebatetype", model.Rebatetype);
            cmd.AddParam("@userid", model.userid);


            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            //扣除分销预付款
            UpdateImprest(model.Warrantid, model.Money);

            return (int)parm.Value;
        }
        #endregion

        #region 修改指定分销商授权的预付款
        public int UpdateImprest(int wid, decimal money)
        {
            const string sqlTxt = "update dbo.agent_warrant set imprest=imprest+@money where id=@wid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@wid", wid);
            cmd.AddParam("@money", money);
            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region 修改信用额度
        public int UpdateCredit(int wid, decimal money)
        {
            const string sqlTxt = "update dbo.agent_warrant set Credit=@money where id=@wid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@wid", wid);
            cmd.AddParam("@money", money);
            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region 修改分销类型
        public int ChangeAgentsort(int agentid, int Agentsort)
        {
            const string sqlTxt = "update dbo.agent_company set Agent_sort=@Agentsort where id=@agentid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@agentid", agentid);
            cmd.AddParam("@Agentsort", Agentsort);
            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region 分销财务编辑
        internal int EditAgentFinancial(Agent_Financial model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);
            cmd.AddParam("@Comid", model.Com_id);
            cmd.AddParam("@Agentid", model.Agentid);
            cmd.AddParam("@Warrantid", model.Warrantid);
            cmd.AddParam("@Orderid", model.Order_id);
            cmd.AddParam("@Servicesname", model.Servicesname);
            cmd.AddParam("@SerialNumber", model.SerialNumber);
            cmd.AddParam("@Money", model.Money);
            cmd.AddParam("@Money_come", model.Money_come);
            cmd.AddParam("@Over_money", model.Over_money);
            cmd.AddParam("@Payment", model.Payment);
            cmd.AddParam("@Payment_type", model.Payment_type);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();

            return (int)parm.Value;
        }
        #endregion


        /// <summary>
        /// 获得分销公司信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal Agent_company GetAgentCompany(int id)
        {
            string sql = @"SELECT *
  FROM  [agent_company] where id=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", id);

            Agent_company u = null;
            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    u = new Agent_company
                    {
                        Id = reader.GetValue<int>("id"),
                        Company = reader.GetValue<string>("company"),
                        Tel = reader.GetValue<string>("tel"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Contentname = reader.GetValue<string>("name"),
                        Address = reader.GetValue<string>("address"),
                        Run_state = reader.GetValue<int>("run_state"),
                        Inter_password = reader.GetValue<string>("inter_password"),
                        Inter_deskey = reader.GetValue<string>("inter_deskey"),
                        Agent_queryurl = reader.GetValue<string>("agent_queryurl"),
                        Agent_updateurl = reader.GetValue<string>("agent_updateurl"),
                        Agent_auth_username = reader.GetValue<string>("agent_auth_username"),
                        Agent_auth_token = reader.GetValue<string>("agent_auth_token"),
                        Agent_type = reader.GetValue<int>("agent_type"),
                        Agent_bindcomname = reader.GetValue<string>("agent_bindcomname"),
                        Agentsort = reader.GetValue<int>("agent_sort"),
                        Agent_messagesetting = reader.GetValue<int>("agent_messagesetting"),
                        inter_sendmethod = reader.GetValue<string>("inter_sendmethod"),

                        ismeituan = reader.GetValue<int>("ismeituan"),
                        mt_partnerId = reader.GetValue<string>("mt_partnerId"),
                        mt_client = reader.GetValue<string>("mt_client"),
                        mt_secret = reader.GetValue<string>("mt_secret"),
                        mt_mark = reader.GetValue<string>("mt_mark"),
                        Lvmama_uid = reader.GetValue<string>("Lvmama_uid"),
                        Lvmama_password = reader.GetValue<string>("Lvmama_password"),
                        Lvmama_Apikey = reader.GetValue<string>("Lvmama_Apikey")
                    };
                }
            }
            return u;
        }

        internal string GetAgentBindIp(int Agentid)
        {
            string ips = "";

            string sql = "select bindip from agent_ip where agentid=" + Agentid;
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ips += reader.GetValue<string>("bindip") + ",";
                    }
                    if (ips.Length > 0)
                    {
                        ips = ips.Substring(0, ips.Length - 1);
                    }

                }

                return ips;
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 获得分销商 授权信息
        /// </summary>
        /// <param name="agentid"></param>
        /// <param name="comid"></param>
        /// <returns></returns>
        internal Agent_warrant GetAgent_Warrant(int agentid, int comid)
        {
            try
            {
                string sql = "SELECT [id] ,[agentid],[comid] ,[warrant_state] ,[warrant_type],[warrant_level] ,[imprest],[Credit] FROM [agent_warrant] where agentid=" + agentid + "  and comid=" + comid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    Agent_warrant m = null;
                    if (reader.Read())
                    {
                        m = new Agent_warrant()
                        {
                            Id = reader.GetValue<int>("id"),
                            Agentid = reader.GetValue<int>("agentid"),
                            Comid = reader.GetValue<int>("comid"),
                            Warrant_state = reader.GetValue<int>("warrant_state"),
                            Warrant_type = reader.GetValue<int>("warrant_type"),
                            Warrant_level = reader.GetValue<int>("warrant_level"),
                            Imprest = reader.GetValue<decimal>("imprest"),
                            Credit = reader.GetValue<decimal>("Credit"),
                        };
                    }
                    return m;
                }
            }
            catch
            {
                return null;
            }
        }

        internal string GetAgentComProvince(int agentid)
        {
            try
            {
                string sql = "select com_province from agent_company where id=" + agentid;
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

        internal string GetAgentComCity(int agentid)
        {
            try
            {
                string sql = "select com_city from agent_company where id=" + agentid;
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

        internal decimal Line_Imprest(int comid, DateTime subdate)
        {
            var endtime = subdate.AddDays(1);//计算出结束日期
            const string sqlTxt = @"select sum(money) as money from agent_Financial where subtime>=@subdate and subtime<@endtime  and    comid=@comid and payment=0 and orderid!=0 and payment_type='分销充值'";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@comid", comid);
                cmd.AddParam("@subdate", subdate.ToString("yyyy-MM-dd"));
                cmd.AddParam("@endtime", endtime.ToString("yyyy-MM-dd"));
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

        internal decimal Hand_Imprest(int comid, DateTime subdate)
        {
            var endtime = subdate.AddDays(1);//计算出结束日期
            const string sqlTxt = @"select sum(money) as money from agent_Financial where  subtime>=@subdate and subtime<@endtime and comid=@comid and payment=0 and orderid=0 and payment_type='分销充值' and Rebatetype=0";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@subdate", subdate.ToString("yyyy-MM-dd"));
                cmd.AddParam("@endtime", endtime.ToString("yyyy-MM-dd"));
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

        internal decimal Hand_Rebate(int comid, DateTime subdate)
        {
            var endtime = subdate.AddDays(1);//计算出结束日期
            const string sqlTxt = @"select sum(money) as money from agent_Financial where  subtime>=@subdate and subtime<@endtime  and comid=@comid and payment=0 and orderid=0 and payment_type='分销充值' and Rebatetype=1";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@subdate", subdate.ToString("yyyy-MM-dd"));
                cmd.AddParam("@endtime", endtime.ToString("yyyy-MM-dd"));
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

        internal decimal TotalImprest(int comid, DateTime subdate)
        {
            return 0;
        }

        internal decimal Daoma_Sale_price(int comid, DateTime subdate)
        {
            var endtime = subdate.AddDays(1);//计算出结束日期
            const string sqlTxt = @"select sum(u_num*pay_price) as money from b2b_order where u_subdate>=@subdate and u_subdate<@endtime and comid=@comid and warrant_type=2  and pay_state =2";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@subdate", subdate.ToString("yyyy-MM-dd"));
                cmd.AddParam("@endtime", endtime.ToString("yyyy-MM-dd"));
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

        internal decimal Daoma_Maoli_price(int comid, DateTime subdate)
        {
            const string sqlTxt = @"select sum(c.use_pnum*a.profit) as money from b2b_order  as a left join b2b_eticket as b on  a.id=b.oid  left join b2b_etcket_log as c on  b.id=c.eticket_id where a.u_subdate=@subdate and a.comid=@comid and a.warrant_type=2 and a.pay_state =2 and b.v_state=2 and c.action=1 and c.a_state=1  ";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@subdate", subdate);
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

        internal decimal Xiaofei_price(int comid, DateTime subdate)
        {
            //const string sqlTxt = @"select sum(money) as money  from agent_financial where comid=@comid and agentid=@agentid  and orderid !=0 ";
            //所有验证数据
            var endtime = subdate.AddDays(1);//计算出结束日期

            const string sqlTxt = @"select sum(b.use_pnum*a.e_sale_price) as money from b2b_eticket as a left join b2b_etcket_log as b on  a.id=b.eticket_id  where b.action=1 and b.a_state=1 and b.actiondate>=@subdate and b.actiondate<@endtime and (a.oid in (select id from b2b_order where comid=@comid and agentid!=0) or a.oid in (select bindingagentorderid from b2b_order where comid=@comid and agentid!=0 and bindingagentorderid !=0))";

            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@subdate", subdate.ToString("yyyy-MM-dd"));
                cmd.AddParam("@endtime", endtime.ToString("yyyy-MM-dd"));
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



        internal decimal Sale_price(int comid, DateTime subdate)
        {
            var endtime = subdate.AddDays(1);//计算出结束日期
            const string sqlTxt = @"select sum(u_num*pay_price) as money from b2b_order where comid=@comid and agentid !=0 and u_subdate>=@subdate and u_subdate<@endtime  and warrant_type=1 and pay_state =2";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@subdate", subdate.ToString("yyyy-MM-dd"));
                cmd.AddParam("@endtime", endtime.ToString("yyyy-MM-dd"));
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

        internal decimal Maoli_price(int comid, DateTime subdate)
        {
            const string sqlTxt = @"select sum(u_num*profit) as money from b2b_order where comid=@comid and agentid!=0  and convert(varchar(10),u_subdate,120)=@subdate and warrant_type=1 and pay_state =2";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@subdate", subdate);
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

        internal decimal Chendian_price(int comid, DateTime subdate)
        {
            const string sqlTxt = @"select sum(a.use_pnum*a.e_sale_price) as money from b2b_eticket as a where  a.oid in (select id from b2b_order where  comid=@comid and agentid!=0 and warrant_type=1  and convert(varchar(10),u_subdate,120)=@subdate ) or a.oid in (select bindingagentorderid from b2b_order where comid=@comid and bindingagentorderid !=0 and agentid!=0 and warrant_type=1 and convert(varchar(10),u_subdate,120)=@subdate ) ";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@subdate", subdate);
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

        //指定分销商本系统总销售量
        internal int Agent_AllSale(int agentid)
        {
            const string sqlTxt = @"select sum(u_num) as u_num from b2b_order where agentid=@agentid and pay_state=2";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
                cmd.CommandType = CommandType.Text;
                cmd.AddParam("@agentid", agentid);
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
        internal decimal OverMoney(int comid, DateTime subtime)
        {


            string sql = "select sum(over_money) as money from agent_Financial where id in(select max(id) from agent_Financial where comid=" + comid + "  and convert(varchar(10),subtime,120)='" + subtime.ToString("yyyy-MM-dd") + "'group by agentid )";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.CommandType = CommandType.Text;

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

        internal int ChangeAgentsourcesort(int agentid, int Agentsourcesort)
        {
            const string sqlTxt = "update dbo.agent_company set Agent_sourcesort=@Agentsourcesort where id=@agentid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@agentid", agentid);
            cmd.AddParam("@Agentsourcesort", Agentsourcesort);
            return cmd.ExecuteNonQuery();
        }

        internal List<Agent_Financial> Agent_Financialpagelist(int pageindex, int pagesize, string key, int agentid, int comid, string payment_type, out int totalcount)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "agent_Financial";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = "comid=" + comid + " and money_come='商家'";

            if (key != "")
            {

            }
            if (agentid != 0)
            {
                condition += " and agentid=" + agentid;
            }
            if (payment_type != "")//分销充值；分销扣款
            {
                condition += " and payment_type=" + payment_type;
            }

            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<Agent_Financial> list = new List<Agent_Financial>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Agent_Financial
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("Comid"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrantid = reader.GetValue<int>("warrantid"),
                        Order_id = reader.GetValue<int>("orderid"),
                        Servicesname = reader.GetValue<string>("servicesname"),
                        Money = reader.GetValue<decimal>("money"),
                        SerialNumber = reader.GetValue<string>("SerialNumber"),
                        Over_money = reader.GetValue<decimal>("over_money"),
                        Money_come = reader.GetValue<string>("money_come"),
                        Payment_type = reader.GetValue<string>("payment_type"),
                        Payment = reader.GetValue<int>("payment"),
                        Rebatetype = reader.GetValue<int>("Rebatetype"),
                        Remarks = reader.GetValue<string>("Remarks"),
                        Subdate = reader.GetValue<DateTime>("subtime"),
                        userid = reader.GetValue<int>("userid")
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());
            return list;

        }

        internal string GetAgentCompanyName(int agentcompanyid)
        {
            try
            {
                string sql = "select company from agent_company  where id=" + agentcompanyid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetValue<string>("company");
                    }
                    return "";
                }
            }
            catch
            {

                return "";
            }
        }

        internal int GetAgentUserCountByMobile(string mobile, string account = "")
        {
            try
            {
                string sql = "select count(1) from agent_company_manageuser where mobile='" + mobile + "'";
                if (account != "")
                {
                    sql += " and account='" + account + "'";
                }
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

        internal int UpAgentUserPass(string mobile, decimal code, string account)
        {
            var sql = "update agent_company_manageuser set pwd='" + code.ToString() + "' where  mobile='" + mobile + "' and account='" + account + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            return cmd.ExecuteNonQuery();
        }

        internal Agent_company GetFreeLandingAgentUserByOpenId(string openid)
        {
            string sqlTxt = @"SELECT  b.id,b.company,b.name,b.address,a.account,b.mobile,b.tel,b.run_state,a.pwd,a.accountLevel,a.Amount
              FROM  [agent_company_manageuser] as a left join agent_company as b  on a.agentid=b.ID where a.account =(select freelandingagentaccount from b2b_crm where weixin=@openid and weixin!='' and freelandingagentaccount!='')";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;
            cmd.AddParam("@openid", openid);

            using (var reader = cmd.ExecuteReader())
            {
                Agent_company u = null;

                if (reader.Read())
                {
                    if (reader.GetValue<int>("run_state") == -1)
                    {
                        return null;
                    }
                    u = new Agent_company
                    {
                        Id = reader.GetValue<int>("id"),
                        Company = reader.GetValue<string>("Company"),
                        Contentname = reader.GetValue<string>("name"),
                        Address = reader.GetValue<string>("Address"),
                        Mobile = reader.GetValue<string>("Mobile"),
                        Tel = reader.GetValue<string>("Tel"),
                        Run_state = reader.GetValue<int>("Run_state"),
                        Account = reader.GetValue<string>("account"),
                        AccountLevel = reader.GetValue<int>("AccountLevel")
                    };
                }
                return u;
            }

        }

        internal int GetAgentCountByMobile(string mobile)
        {
            string sql = "select count(1) from agent_company where mobile='" + mobile + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            return int.Parse(o.ToString());
        }

        internal int GetAgentidByTb_sellerid(string tb_seller_wangwangid)
        {
            string sql = "select agentid from taobao_agent_relation where tb_seller_wangwangid='" + tb_seller_wangwangid + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                int agentid = 0;
                if (reader.Read())
                {
                    agentid = reader.GetValue<int>("agentid");
                }
                return agentid;
            }

        }
        ////判断除了当前分销外 ，该淘宝卖家是否绑定了其他分销
        //internal bool IsbindAgentBytbsellerid(string tbsellerid, int agentid)
        //{
        //    string sql = "select count(1) from   agent_company where tb_seller_id='" + tbsellerid + "'  and id!=" + agentid + " and tb_seller_id!=''";
        //    var cmd = sqlHelper.PrepareTextSqlCommand(sql);
        //    object o = cmd.ExecuteScalar();
        //    if (int.Parse(o.ToString()) == 0)
        //    {
        //        return false;
        //    }
        //    else 
        //    {
        //        return true;
        //    }
        //}

        internal Agent_regiinfo GetAgentAccountByid(int accountid)
        {
            const string sqlTxt = @"SELECT  * from agent_company_manageuser where id=@id";

            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.CommandType = CommandType.Text;

            cmd.AddParam("@id", accountid);

            using (var reader = cmd.ExecuteReader())
            {
                Agent_regiinfo u = null;

                while (reader.Read())
                {
                    u = new Agent_regiinfo
                    {
                        Id = reader.GetValue<int>("id"),
                        Account = reader.GetValue<string>("Account"),
                        Pwd = reader.GetValue<string>("Pwd"),
                        Contentname = reader.GetValue<string>("name"),
                        Mobile = reader.GetValue<string>("Mobile"),
                        Amount = reader.GetValue<decimal>("Amount"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        AccountLevel = reader.GetValue<int>("AccountLevel"),
                    };

                }
                return u;
            }
        }

        internal int GetWarrant_type(int Agentid, int comid)
        {
            string sql = @"SELECT  
                               [warrant_type] 
                          FROM  [agent_warrant] where agentid=@agentid and comid=@comid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@agentid", Agentid);
            cmd.AddParam("@comid", comid);
            using (var reader = cmd.ExecuteReader())
            {
                int m = 0;
                if (reader.Read())
                {
                    m = reader.GetValue<int>("warrant_type");
                }
                return m;
            }
        }

        internal Agent_company GetAgentCompanyByName(string companyname)
        {
            string sql = @"SELECT [id]
      ,[company]
      ,[tel]
      ,[mobile]
      ,[name]
      ,[address]
      ,[run_state]
      ,[inter_password]
      ,[inter_deskey]
      ,[agent_queryurl]
      ,[agent_updateurl]
      ,[agent_auth_username]
      ,[agent_auth_token]
      ,[agent_type]
      ,[agent_bindcomname]
,agent_sort
,agent_messagesetting
  FROM  [agent_company] where company=@company";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@company", companyname.Trim());

            Agent_company u = null;
            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    u = new Agent_company
                    {
                        Id = reader.GetValue<int>("id"),
                        Company = reader.GetValue<string>("company"),
                        Tel = reader.GetValue<string>("tel"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Contentname = reader.GetValue<string>("name"),
                        Address = reader.GetValue<string>("address"),
                        Run_state = reader.GetValue<int>("run_state"),
                        Inter_password = reader.GetValue<string>("inter_password"),
                        Inter_deskey = reader.GetValue<string>("inter_deskey"),
                        Agent_queryurl = reader.GetValue<string>("agent_queryurl"),
                        Agent_updateurl = reader.GetValue<string>("agent_updateurl"),
                        Agent_auth_username = reader.GetValue<string>("agent_auth_username"),
                        Agent_auth_token = reader.GetValue<string>("agent_auth_token"),
                        Agent_type = reader.GetValue<int>("agent_type"),
                        Agent_bindcomname = reader.GetValue<string>("agent_bindcomname"),
                        Agentsort = reader.GetValue<int>("agent_sort"),
                        Agent_messagesetting = reader.GetValue<int>("agent_messagesetting"),
                    };
                }
            }
            return u;
        }

        internal int UpAgentip(int agentid, string deskey)
        {
            string sql = "update agent_company set inter_deskey='" + deskey + "' where id=" + agentid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            return cmd.ExecuteNonQuery();
        }

        internal int Editagentoutinterface(int agentid, string agent_updateurl, string txtagentip, string inter_sendmethod = "post")
        {
            sqlHelper.BeginTrancation();
            try
            {
                //修改分销验证通知地址
                string sql1 = "update agent_company set agent_updateurl='" + agent_updateurl + "',inter_sendmethod='" + inter_sendmethod + "' where id=" + agentid;
                var cmd1 = sqlHelper.PrepareTextSqlCommand(sql1);
                cmd1.ExecuteNonQuery();

                //得到分销绑定的ip列表
                List<string> iplist = new List<string>();
                string sql2 = "select  * from agent_ip where agentid=" + agentid;
                var cmd2 = sqlHelper.PrepareTextSqlCommand(sql2);
                using (var reader = cmd2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        iplist.Add(reader.GetValue<string>("bindip"));
                    }
                }

                if (iplist == null)
                {
                    var iparr = txtagentip.Replace("，", ",").Split(',');
                    foreach (string ip in iparr)
                    {
                        if (ip != "")
                        {
                            string sql3 = "insert into agent_ip(agentid,bindip) values(" + agentid + ",'" + ip.Trim() + "')";
                            var cmd3 = sqlHelper.PrepareTextSqlCommand(sql3);
                            cmd3.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    var iparr = txtagentip.Replace("，", ",").Split(',');

                    //判断数据库中是否有多余的ip
                    foreach (string cip in iplist)
                    {
                        if (cip.Trim() != "")
                        {
                            if (!iparr.Contains(cip.Trim()))
                            {
                                string sql3 = "delete agent_ip where agentid=" + agentid + " and bindip='" + cip.Trim() + "'";
                                var cmd3 = sqlHelper.PrepareTextSqlCommand(sql3);
                                cmd3.ExecuteNonQuery();
                            }
                        }
                    }

                    //把数据库中不包含的ip录入 
                    foreach (string ip in iparr)
                    {
                        if (ip.Trim() != "")
                        {
                            if (!iplist.Contains(ip.Trim()))
                            {
                                string sql3 = "insert into agent_ip(agentid,bindip) values(" + agentid + ",'" + ip.Trim() + "')";
                                var cmd3 = sqlHelper.PrepareTextSqlCommand(sql3);
                                cmd3.ExecuteNonQuery();
                            }
                        }
                    }


                }


                sqlHelper.Commit();
                return 1;
            }
            catch
            {
                sqlHelper.Rollback();
                return 0;
            }
            finally
            {
                sqlHelper.Dispose();
            }
        }

        internal List<Agent_company> Getagentlist(string isapiagent)
        {
            string sql = "select * from agent_company where 1=1 ";
            if (isapiagent == "1")
            {
                sql += " and id in (select agentcomid from agent_asyncsendlog)";
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<Agent_company> list = new List<Agent_company>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new Agent_company
                    {

                        Id = reader.GetValue<int>("id"),
                        Company = reader.GetValue<string>("Company"),
                        Tel = reader.GetValue<string>("Tel"),
                        Mobile = reader.GetValue<string>("Mobile"),
                        Contentname = reader.GetValue<string>("name"),
                        Address = reader.GetValue<string>("Address"),
                        Run_state = reader.GetValue<int>("Run_state"),
                        Agentsort = reader.GetValue<int>("Agent_sort"),
                        Agent_domain = reader.GetValue<string>("Agent_domain"),
                        Weixin_img = reader.GetValue<int>("Weixin_img"),
                        Agent_messagesetting = reader.GetValue<int>("Agent_messagesetting"),
                        agentsourcesort = reader.GetValue<int>("Agent_sourcesort"),
                    });

                }
            }
            return list;
        }

        internal List<Agent_warrant> GetWarrantlist(string Key, int Agentid, string comstate, string containcomids)
        {
            var condition = "agentid=" + Agentid + " and warrant_state=1";
            if (Key != "")
            {
                condition += " and comid in (select id from b2b_company where com_name like '%" + Key + "%')";
            }
            if (comstate != "1,2")
            {
                condition += " and comid in (select id from b2b_company where com_state=" + comstate + ")";
            }
            if (containcomids != "")
            {
                condition += " and comid in (" + containcomids + ")";
            }

            string sql = "select * from agent_warrant where " + condition;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<Agent_warrant> list = new List<Agent_warrant>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Agent_warrant
                    {
                        Id = reader.GetValue<int>("id"),
                        Comid = reader.GetValue<int>("Comid"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_state = reader.GetValue<int>("Warrant_state"),
                        Imprest = reader.GetValue<decimal>("Imprest"),
                        Credit = reader.GetValue<decimal>("Credit"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrant_level = reader.GetValue<int>("Warrant_level"),

                    });

                }
            }
            return list;
        }


        internal List<B2b_company> UnWarrantlist(string Key, int Agentid, string containcomids)
        {
            var condition = "com_state=1 and lp=1 ";
            if (Key != "")
            {
                condition += " and com_name like '%" + Key + "%'";
            }
            if (containcomids != "")
            {
                condition += " and id  in (" + containcomids + ")";
            }
            condition = condition + " and   id not in (select comid from agent_warrant where agentid=" + Agentid + " )";

            string sql = "select * from b2b_company where " + condition;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<B2b_company> list = new List<B2b_company>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_company
                    {

                        ID = reader.GetValue<int>("Id"),
                        Com_name = reader.GetValue<string>("Com_name"),

                    });
                }
            }
            return list;
        }

        internal IList<Agent_company> GetMeituanAgentCompanyList(int comid)
        {
            string sql = "select  id,company from agent_company where ismeituan=1 and  id in (select agentid from agent_warrant where comid="+comid+" and warrant_state=1)";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<Agent_company> list = new List<Agent_company>();
            using(var reader=cmd.ExecuteReader()){
               while(reader.Read())
               {
                   list.Add(new Agent_company { 
                     Id=reader.GetValue<int>("id"),
                     Company=reader.GetValue<string>("company")
                   });
               }
            }
            return list;
        }

        internal Agent_company GetAgentCompanyByMeituanPartnerId(string PartnerId)
        {
            string sql = "select * from agent_company where mt_partnerId=@mt_partnerId";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@mt_partnerId", PartnerId);

            Agent_company u = null;
            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    u = new Agent_company
                    {
                        Id = reader.GetValue<int>("id"),
                        Company = reader.GetValue<string>("company"),
                        Tel = reader.GetValue<string>("tel"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Contentname = reader.GetValue<string>("name"),
                        Address = reader.GetValue<string>("address"),
                        Run_state = reader.GetValue<int>("run_state"),
                        Inter_password = reader.GetValue<string>("inter_password"),
                        Inter_deskey = reader.GetValue<string>("inter_deskey"),
                        Agent_queryurl = reader.GetValue<string>("agent_queryurl"),
                        Agent_updateurl = reader.GetValue<string>("agent_updateurl"),
                        Agent_auth_username = reader.GetValue<string>("agent_auth_username"),
                        Agent_auth_token = reader.GetValue<string>("agent_auth_token"),
                        Agent_type = reader.GetValue<int>("agent_type"),
                        Agent_bindcomname = reader.GetValue<string>("agent_bindcomname"),
                        Agentsort = reader.GetValue<int>("agent_sort"),
                        Agent_messagesetting = reader.GetValue<int>("agent_messagesetting"),

                        ismeituan = reader.GetValue<int>("ismeituan"),
                        mt_partnerId = reader.GetValue<string>("mt_partnerId"),
                        mt_client = reader.GetValue<string>("mt_client"),
                        mt_secret = reader.GetValue<string>("mt_secret"),
                        mt_mark = reader.GetValue<string>("mt_mark")
                    };
                }
            }
            return u;
        }


        internal Agent_company GetAgentCompanyByLvmamaPartnerId(string lvmamauid)
        {
            string sql = "select * from agent_company where Lvmama_uid=@lvmamauid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@lvmamauid", lvmamauid);

            Agent_company u = null;
            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {
                    u = new Agent_company
                    {
                        Id = reader.GetValue<int>("id"),
                        Company = reader.GetValue<string>("company"),
                        Tel = reader.GetValue<string>("tel"),
                        Mobile = reader.GetValue<string>("mobile"),
                        Contentname = reader.GetValue<string>("name"),
                        Address = reader.GetValue<string>("address"),
                        Run_state = reader.GetValue<int>("run_state"),
                        Inter_password = reader.GetValue<string>("inter_password"),
                        Inter_deskey = reader.GetValue<string>("inter_deskey"),
                        Agent_queryurl = reader.GetValue<string>("agent_queryurl"),
                        Agent_updateurl = reader.GetValue<string>("agent_updateurl"),
                        Agent_auth_username = reader.GetValue<string>("agent_auth_username"),
                        Agent_auth_token = reader.GetValue<string>("agent_auth_token"),
                        Agent_type = reader.GetValue<int>("agent_type"),
                        Agent_bindcomname = reader.GetValue<string>("agent_bindcomname"),
                        Agentsort = reader.GetValue<int>("agent_sort"),
                        Agent_messagesetting = reader.GetValue<int>("agent_messagesetting"),

                        ismeituan = reader.GetValue<int>("ismeituan"),
                        mt_partnerId = reader.GetValue<string>("mt_partnerId"),
                        mt_client = reader.GetValue<string>("mt_client"),
                        mt_secret = reader.GetValue<string>("mt_secret"),
                        mt_mark = reader.GetValue<string>("mt_mark"),
                        Lvmama_uid = reader.GetValue<string>("Lvmama_uid"),
                        Lvmama_password = reader.GetValue<string>("Lvmama_password"),
                        Lvmama_Apikey = reader.GetValue<string>("Lvmama_Apikey")

                    };
                }
            }
            return u;
        }

        internal List<Agent_company> GetAllMeituanAgentCompany()
        { 
            string sql = "select  id,company from agent_company where ismeituan=1 ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<Agent_company> list = new List<Agent_company>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Agent_company
                    {
                        Id = reader.GetValue<int>("id"),
                        Company = reader.GetValue<string>("company")
                    });
                }
            }
            return list;
        }

        internal List<Agent_warrant> GetAgentWarrantList(int agentid, string warrantstate)
        {
            string sql = "select  * from agent_warrant where agentid="+agentid;
            if(warrantstate!="0,1")
            {
                sql += " and warrant_state="+warrantstate;
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);

            List<Agent_warrant> list = new List<Agent_warrant>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Agent_warrant
                    {
                        Id = reader.GetValue<int>("id"),
                        Comid = reader.GetValue<int>("Comid"),
                        Agentid = reader.GetValue<int>("Agentid"),
                        Warrant_state = reader.GetValue<int>("Warrant_state"),
                        Imprest = reader.GetValue<decimal>("Imprest"),
                        Credit = reader.GetValue<decimal>("Credit"),
                        Warrant_type = reader.GetValue<int>("Warrant_type"),
                        Warrant_level = reader.GetValue<int>("Warrant_level"), 
                    });
                }
            }
            return list;
        }

        internal bool IsMeituanAgent(int agentid)
        {
            string sql = "select ismeituan from agent_company where id=" + agentid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using(var reader=cmd.ExecuteReader())
            {
              int ismeituan = 0;
              if(reader.Read())
              {
                  ismeituan = reader.GetValue<int>("ismeituan");
              }
              if (ismeituan == 0)
              {
                  return false;
              }
              else 
              {
                  return true;
              }
            }
            
        }
    }
}

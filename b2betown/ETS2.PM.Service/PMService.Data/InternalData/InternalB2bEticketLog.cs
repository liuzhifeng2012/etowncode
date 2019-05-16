using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Modle.Enum;
using System.Text.RegularExpressions;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalB2bEticketLog
    {
        private SqlHelper sqlHelper;
        public InternalB2bEticketLog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        /// <summary>
        /// 编辑电子码验证日志信息
        /// </summary>
        /// <param name="elog"></param>
        /// <returns></returns>
        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateETicketLog";
        internal int InsertOrUpdate(Modle.B2b_eticket_log elog)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);
            cmd.AddParam("@Id", elog.Id);
            cmd.AddParam("@Eticket_id", elog.Eticket_id);
            cmd.AddParam("@Pno", elog.Pno);
            cmd.AddParam("@Action", elog.Action);
            cmd.AddParam("@A_state", elog.A_state);
            cmd.AddParam("@A_remark", elog.A_remark);
            cmd.AddParam("@Use_pnum", elog.Use_pnum);
            cmd.AddParam("@Actiondate", elog.Actiondate);
            cmd.AddParam("@ComId", elog.Com_id);
            cmd.AddParam("@PosId", elog.PosId);
            cmd.AddParam("@RandomId", elog.RandomId);
            cmd.AddParam("@Pcaccount", elog.Pcaccount);

            var para = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)para.Value;
        }

        //电子票记录，其中agent是授权项目账户查看功能
        internal List<B2b_eticket_log> EPageList(string comid, int pageindex, int pagesize, int eclass, int proid, int jsid, out int totalcount, int posid = 0, string key = "", string startime = "", string endtime = "", int agentid = 0, int projectid = 0, int saleagentid=0)
        {
            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = "b2b_etcket_log";
            var strGetFields = "*";
            var sortKey = "actiondate";
            var sortMode = "1";
            var condition = "action=1 and com_id=" + comid + " and a_state=" + (int)ECodeOperStatus.OperSuc + " and pno in (select pno from b2b_eticket where 1=1 ";
            if (projectid != 0)
            {
                condition += " and pro_id in (select id from b2b_com_pro where projectid=" + projectid + ")";
            }
            if (proid != 0)
            {
                condition += " and pro_id=" + proid;
            }
            if (eclass == 0)
            {
                condition += " and agent_id=0";
            }

            if (eclass > 0)
            {
                condition += " and agent_id!=0";
            }
            //这个是对项目账户（利用分销账户做的），看到项目的验票情况
            if (agentid != 0)
            {
                
                    condition += " and pro_id in (select id from b2b_com_pro where projectid in (select projectid from b2b_project_agent where agentid=" + agentid + ") )";
            }

            
            condition += ")";

            //这个是分销验票情况
            if (saleagentid != 0)
            {
                if (saleagentid == -1)
                {
                    condition += " and eticket_id in (select id from b2b_eticket where agent_id=0)";//直销订单
                }
                else
                {
                    condition += " and eticket_id in (select id from b2b_eticket where agent_id=" + saleagentid + ")";
                }
            }

            if (jsid != 0)
            {
                condition += " and jsid=" + jsid;
            }
            if (posid != 0)
            {
                condition += " and posid=" + posid;
            }

            if (key != "")
            {
                Regex regex = new Regex("^[0-9]*$");
                bool isNum = regex.IsMatch(key.Trim());
                if (isNum == false){//如果不是纯数字，则不查询订单
                    condition += " and pno='" + key + "'"; 
                }else{

                    condition += " and (pno='" + key + "' or eticket_id in (select id from b2b_eticket where oid=" + key + "))";
                }
            }
            if (startime != "")
            {
                condition += " and actiondate>='" + startime + "'";
            }
            if (endtime != "")
            {
                endtime = DateTime.Parse(endtime).AddDays(1).ToString();
                condition += " and actiondate<'" + endtime + "'";
            }



            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_eticket_log> list = new List<B2b_eticket_log>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_eticket_log
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pno = reader.GetValue<string>("Pno"),
                        A_remark = reader.GetValue<string>("A_remark"),
                        A_state = reader.GetValue<int>("A_state"),
                        Action = reader.GetValue<int>("Action"),
                        Actiondate = reader.GetValue<DateTime>("Actiondate"),
                        Eticket_id = reader.GetValue<int>("Eticket_id"),
                        Use_pnum = reader.GetValue<int>("Use_pnum"),
                        PosId = reader.GetValue<int>("PosId"),
                        Pcaccount = reader.GetValue<string>("Pcaccount"),
                    });
                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());

            return list;
        }
        internal int ModifyJsid(int JSid, int comid)
        {
            const string sqlTxt = "update dbo.b2b_etcket_log set jsid=@jsid where jsid=0 and com_id=@comid  and action=1 and a_state=1";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@jsid", JSid);
            cmd.AddParam("@comid", comid);

            return cmd.ExecuteNonQuery();
        }

        internal B2b_eticket_log GetFrontNotJS()
        {
            const string sqll = @"SELECT   [id]
      ,[eticket_id]
      ,[pno]
      ,[action]
      ,[a_state]
      ,[a_remark]
      ,[use_pnum]
      ,[actiondate]
      ,[com_id]
      ,[jsid]
,Pcaccount
  FROM [EtownDB].[dbo].[b2b_etcket_log] where  jsid=0 and action=1 and a_state=1  order by actiondate asc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqll);


            return cmd.ExecSingleReader<B2b_eticket_log>(reader => new B2b_eticket_log
            {
                Id = reader.GetValue<int>("id"),
                Eticket_id = reader.GetValue<int>("eticket_id"),
                Pno = reader.GetValue<string>("pno"),
                Action = reader.GetValue<int>("action"),
                A_state = reader.GetValue<int>("a_state"),
                A_remark = reader.GetValue<string>("a_remark"),
                Use_pnum = reader.GetValue<int>("use_pnum"),
                Actiondate = DateTime.Parse(reader.GetValue<DateTime>("actiondate").ToString("yyyy-MM-dd")),
                Com_id = reader.GetValue<int>("com_id"),
                JsId = reader.GetValue<int>("jsid"),
                Pcaccount = reader.GetValue<string>("Pcaccount"),

            });
        }

        internal B2b_eticket_log GetFinalPrintEticketLogByPosid(string pos_id)
        {
            const string sqll = @"SELECT top 1  [id]
      ,[eticket_id]
      ,[pno]
      ,[action]
      ,[a_state]
      ,[a_remark]
      ,[use_pnum]
      ,[actiondate]
      ,[com_id]
      ,[jsid]
,Pcaccount
  FROM [dbo].[b2b_etcket_log] where   action=1 and a_state=1  and posid=@posid order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqll);
            cmd.AddParam("@posid", pos_id);

            return cmd.ExecSingleReader<B2b_eticket_log>(reader => new B2b_eticket_log
            {
                Id = reader.GetValue<int>("id"),
                Eticket_id = reader.GetValue<int>("eticket_id"),
                Pno = reader.GetValue<string>("pno"),
                Action = reader.GetValue<int>("action"),
                A_state = reader.GetValue<int>("a_state"),
                A_remark = reader.GetValue<string>("a_remark"),
                Use_pnum = reader.GetValue<int>("use_pnum"),
                Actiondate = reader.GetValue<DateTime>("actiondate"),
                Com_id = reader.GetValue<int>("com_id"),
                JsId = reader.GetValue<int>("jsid"),
                Pcaccount = reader.GetValue<string>("Pcaccount"),

            });
        }

        internal int ModifyJsidByPosId(int JSid, string pos_id)
        {
            const string sqlTxt = "update dbo.b2b_etcket_log set jsid=@jsid where jsid=0 and posid=@posid  and action=1 and a_state=1";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqlTxt);
            cmd.AddParam("@jsid", JSid);
            cmd.AddParam("@posid", pos_id);

            return cmd.ExecuteNonQuery();
        }

        internal B2b_eticket_log GetTicketLogById(string validateticketlogid)
        {
            const string sqll = @"SELECT    [id]
      ,[eticket_id]
      ,[pno]
      ,[action]
      ,[a_state]
      ,[a_remark]
      ,[use_pnum]
      ,[actiondate]
      ,[com_id]
      ,[jsid]
,Pcaccount
  FROM [EtownDB].[dbo].[b2b_etcket_log] where   id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqll);
            cmd.AddParam("@id", validateticketlogid);

            return cmd.ExecSingleReader<B2b_eticket_log>(reader => new B2b_eticket_log
            {
                Id = reader.GetValue<int>("id"),
                Eticket_id = reader.GetValue<int>("eticket_id"),
                Pno = reader.GetValue<string>("pno"),
                Action = reader.GetValue<int>("action"),
                A_state = reader.GetValue<int>("a_state"),
                A_remark = reader.GetValue<string>("a_remark"),
                Use_pnum = reader.GetValue<int>("use_pnum"),
                Actiondate = reader.GetValue<DateTime>("actiondate"),
                Com_id = reader.GetValue<int>("com_id"),
                JsId = reader.GetValue<int>("jsid"),
                Pcaccount = reader.GetValue<string>("Pcaccount"),

            });
        }

        internal List<B2b_eticket_log> GetPnoConsumeLogList(string pno)
        {
            const string sqll = @"SELECT    [id]
      ,[eticket_id]
      ,[pno]
      ,[action]
      ,[a_state]
      ,[a_remark]
      ,[use_pnum]
      ,[actiondate]
      ,[com_id]
      ,[jsid]
,Pcaccount
  FROM [EtownDB].[dbo].[b2b_etcket_log] where   pno=@pno and a_state=@astate";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqll);
            cmd.AddParam("@pno", pno);
            cmd.AddParam("@astate", 1);

            List<B2b_eticket_log> list = new List<B2b_eticket_log>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_eticket_log
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pno = reader.GetValue<string>("Pno"),
                        A_remark = reader.GetValue<string>("A_remark"),
                        A_state = reader.GetValue<int>("A_state"),
                        Action = reader.GetValue<int>("Action"),
                        Actiondate = reader.GetValue<DateTime>("Actiondate"),
                        Eticket_id = reader.GetValue<int>("Eticket_id"),
                        Use_pnum = reader.GetValue<int>("Use_pnum"),
                        Pcaccount = reader.GetValue<string>("Pcaccount"),

                    });

                }
            }
            return list;

        }
        //        #region 根据随机码id得到验码日志
        //        internal List<B2b_eticket_log> GetElogByRandomid(string randomid)
        //        {
        //            string sqll = @"SELECT  [id]
        //      ,[eticket_id]
        //      ,[pno]
        //      ,[action]
        //      ,[a_state]
        //      ,[a_remark]
        //      ,[use_pnum]
        //      ,[actiondate]
        //      ,[com_id]
        //      ,[jsid]
        //      ,[posid]
        //      ,[randomid]
        //  FROM [EtownDB].[dbo].[b2b_etcket_log] where randomid=@randomid and action=@action";
        //            var cmd = sqlHelper.PrepareTextSqlCommand(sqll);
        //            cmd.AddParam("@randomid", randomid);
        //            cmd.AddParam("@action", (int)ECodeOper.ValidateECode);

        //            try
        //            {
        //                List<B2b_eticket_log> list = new List<B2b_eticket_log>();
        //                using (var reader = cmd.ExecuteReader())
        //                {

        //                    while (reader.Read())
        //                    {
        //                        list.Add(new B2b_eticket_log
        //                        {
        //                            Id = reader.GetValue<int>("Id"),
        //                            Pno = reader.GetValue<string>("Pno"),
        //                            A_remark = reader.GetValue<string>("A_remark"),
        //                            A_state = reader.GetValue<int>("A_state"),
        //                            Action = reader.GetValue<int>("Action"),
        //                            Actiondate = reader.GetValue<DateTime>("Actiondate"),
        //                            Eticket_id = reader.GetValue<int>("Eticket_id"),
        //                            Use_pnum = reader.GetValue<int>("Use_pnum"),
        //                            RandomId = reader.GetValue<string>("randomid"),

        //                            PosId = reader.GetValue<int>("posid"),
        //                            Com_id = reader.GetValue<int>("com_id")
        //                        });
        //                    }

        //                }
        //                return list;
        //            }
        //            catch
        //            {
        //                return null;
        //            }

        //        }
        //        #endregion

        internal bool GetRandomWhetherSame(int action, string randomid)
        {
            string sql = "select count(1) from b2b_etcket_log where   randomid='" + randomid + "' and action=" + action;
            try
            {
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
            catch
            {
                return false;
            }
        }



        #region 分销验证日志
        internal List<B2b_eticket_log> AgentEticketlog(int comid, int agentid, int pageindex, int pagesize, out int totalcount, string key = "", string startime = "", string endtime = "")
        {

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = " ((a.com_id=" + comid + " and b.agent_id=" + agentid + ") or b.oid in (select bindingagentorderid from b2b_order where agentid=" + agentid + " and comid=" + comid + " and bindingagentorderid !=0 ))  and  a.Action=1 and a.a_state=1";

            if (key != "")
            {
                condition += " and (b.pno = '" + key + "' or b.oid in (select ordernum from agent_requestlog where req_seq='" + key + "' and request_type='add_order' and is_dealsuc=1))";
            }
            if (startime != "")
            {
                condition += " and a.actiondate>='" + startime + "'";
            }

            if (endtime != "")
            {
                condition += " and a.actiondate<='" + startime + "'";
            }
            try
            {
                cmd.PagingCommand1("b2b_etcket_log as a left join b2b_eticket as b on a.eticket_id=b.id", "a.id,a.posid,a.A_state,a.action,a.eticket_id,a.pno,a.actiondate,a.use_pnum,b.agent_id,b.e_proname,b.com_id,b.pnum,b.E_sale_price,b.oid,b.e_type", "a.id desc", "", pagesize, pageindex, "", condition);

                List<B2b_eticket_log> list = new List<B2b_eticket_log>();
                using (var reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        list.Add(new B2b_eticket_log
                        {
                            Id = reader.GetValue<int>("Id"),
                            Pno = reader.GetValue<string>("Pno"),
                            //Agent_id = reader.GetValue<int>("agent_id"),
                            PosId = reader.GetValue<int>("PosId"),
                            A_state = reader.GetValue<int>("A_state"),
                            Action = reader.GetValue<int>("Action"),
                            Actiondate = reader.GetValue<DateTime>("Actiondate"),
                            Eticket_id = reader.GetValue<int>("Eticket_id"),
                            Use_pnum = reader.GetValue<int>("Use_pnum"),
                            Agent_id = reader.GetValue<int>("Agent_id"),
                            E_proname = reader.GetValue<string>("E_proname"),
                            Com_id = reader.GetValue<int>("Com_id"),
                            Pnum = reader.GetValue<int>("Pnum"),
                            E_sale_price = reader.GetValue<decimal>("E_sale_price"),
                            Oid = reader.GetValue<int>("Oid"),
                            E_type = reader.GetValue<int>("E_type"),
                        });

                    }
                }
                totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

                return list;
            }
            catch
            {
                totalcount = 0;
                return null;
            }
        }
        #endregion

        #region 按订单分销验证日志
        internal List<B2b_eticket_log> VAgentEticketlog(int comid, int agentid, int orderid, int pageindex, int pagesize, out int totalcount)
        {

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("proc_ListPage");
            var condition = " a.com_id=" + comid + " and b.agent_id=" + agentid + " and b.oid=" + orderid + " and  a.Action=1 and a.a_state=1";
            cmd.PagingCommand1("b2b_etcket_log as a left join b2b_eticket as b on a.eticket_id=b.id", "a.id,a.posid,a.A_state,a.action,a.eticket_id,a.pno,a.actiondate,a.use_pnum,b.agent_id,b.e_proname,b.com_id,b.pnum,b.E_sale_price,b.oid,b.e_type", "a.id desc", "", pagesize, pageindex, "", condition);

            List<B2b_eticket_log> list = new List<B2b_eticket_log>();
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    list.Add(new B2b_eticket_log
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pno = reader.GetValue<string>("Pno"),
                        //Agent_id = reader.GetValue<int>("agent_id"),
                        PosId = reader.GetValue<int>("PosId"),
                        A_state = reader.GetValue<int>("A_state"),
                        Action = reader.GetValue<int>("Action"),
                        Actiondate = reader.GetValue<DateTime>("Actiondate"),
                        Eticket_id = reader.GetValue<int>("Eticket_id"),
                        Use_pnum = reader.GetValue<int>("Use_pnum"),
                        Agent_id = reader.GetValue<int>("Agent_id"),
                        E_proname = reader.GetValue<string>("E_proname"),
                        Com_id = reader.GetValue<int>("Com_id"),
                        Pnum = reader.GetValue<int>("Pnum"),
                        E_sale_price = reader.GetValue<decimal>("E_sale_price"),
                        Oid = reader.GetValue<int>("Oid"),
                        E_type = reader.GetValue<int>("E_type"),
                    });

                }
            }
            totalcount = int.Parse(cmd.Parameters[0].Value.ToString());

            return list;
        }
        #endregion

        internal B2b_eticket_log GetElogByRandomid(string randomid, int action)
        {
            string sqll = @"SELECT  [id]
      ,[eticket_id]
      ,[pno]
      ,[action]
      ,[a_state]
      ,[a_remark]
      ,[use_pnum]
      ,[actiondate]
      ,[com_id]
      ,[jsid]
      ,[posid]
      ,[randomid]
,Pcaccount
  FROM [EtownDB].[dbo].[b2b_etcket_log] where randomid=@randomid and action=@action";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqll);
            cmd.AddParam("@randomid", randomid);
            cmd.AddParam("@action", action);

            try
            {

                using (var reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        return new B2b_eticket_log
                        {
                            Id = reader.GetValue<int>("Id"),
                            Pno = reader.GetValue<string>("Pno"),
                            A_remark = reader.GetValue<string>("A_remark"),
                            A_state = reader.GetValue<int>("A_state"),
                            Action = reader.GetValue<int>("Action"),
                            Actiondate = reader.GetValue<DateTime>("Actiondate"),
                            Eticket_id = reader.GetValue<int>("Eticket_id"),
                            Use_pnum = reader.GetValue<int>("Use_pnum"),
                            RandomId = reader.GetValue<string>("randomid"),

                            PosId = reader.GetValue<int>("posid"),
                            Com_id = reader.GetValue<int>("com_id"),

                            Pcaccount = reader.GetValue<string>("Pcaccount"),
                        };
                    }
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        internal int GetVerifyNum(string pno)
        {
            try
            {
                string sql = "select sum(use_pnum) from b2b_etcket_log where pno='" + pno + "' and action=1 and a_state=1";
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

        internal List<B2b_eticket_log> GeteticketloglistByorderid(int orderid)
        {
            string sqll = @"SELECT  [id]
      ,[eticket_id]
      ,[pno]
      ,[action]
      ,[a_state]
      ,[a_remark]
      ,[use_pnum]
      ,[actiondate]
      ,[com_id]
      ,[jsid]
      ,[posid]
      ,[randomid]
  ,Pcaccount
  FROM  [b2b_etcket_log] where    use_pnum>0
 and action=1 and a_state=1 
 and pno in (select pno from b2b_eticket where oid=@orderid)";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqll);

            cmd.AddParam("@orderid", orderid);


            using (var reader = cmd.ExecuteReader())
            {
                List<B2b_eticket_log> list = new List<B2b_eticket_log>();
                while (reader.Read())
                {
                    list.Add(new B2b_eticket_log
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pno = reader.GetValue<string>("Pno"),
                        A_remark = reader.GetValue<string>("A_remark"),
                        A_state = reader.GetValue<int>("A_state"),
                        Action = reader.GetValue<int>("Action"),
                        Actiondate = reader.GetValue<DateTime>("Actiondate"),
                        Eticket_id = reader.GetValue<int>("Eticket_id"),
                        Use_pnum = reader.GetValue<int>("Use_pnum"),
                        RandomId = reader.GetValue<string>("randomid"),

                        PosId = reader.GetValue<int>("posid"),
                        Com_id = reader.GetValue<int>("com_id"),

                        Pcaccount = reader.GetValue<string>("Pcaccount"),
                    });
                }
                return list;
            }

        }

        internal bool Ishasreversesuclog(string randomid)
        {
            string sql = "select count(1) from  b2b_etcket_log where randomid='" + randomid + "' and action=4 and a_state=1";
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

        internal int GetYanzhenglogCountByPno(string pno)
        {
            string sql = "select count(1) from b2b_etcket_log where pno='" + pno + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            return int.Parse(o.ToString());
        }

        internal B2b_eticket_log GetLasterYueyupnoElog(string pno, int usenum)
        {
            string sql = "select top 1 * from b2b_etcket_log where pno='" + pno + "' and   use_pnum=" + usenum + " order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_eticket_log
                     {
                         Id = reader.GetValue<int>("Id"),
                         Pno = reader.GetValue<string>("Pno"),
                         A_remark = reader.GetValue<string>("A_remark"),
                         A_state = reader.GetValue<int>("A_state"),
                         Action = reader.GetValue<int>("Action"),
                         Actiondate = reader.GetValue<DateTime>("Actiondate"),
                         Eticket_id = reader.GetValue<int>("Eticket_id"),
                         Use_pnum = reader.GetValue<int>("Use_pnum"),
                         RandomId = reader.GetValue<string>("randomid"),

                         PosId = reader.GetValue<int>("posid"),
                         Com_id = reader.GetValue<int>("com_id"),

                         Pcaccount = reader.GetValue<string>("Pcaccount"),
                     };
                }
                return null;
            }
        }

        internal B2b_eticket_log GetlastyanzhengsuclogByPno(string pno)
        {
            string sql = "select top 1 * from b2b_etcket_log where pno='" + pno + "' and   a_state=1 order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_eticket_log
                    {
                        Id = reader.GetValue<int>("Id"),
                        Pno = reader.GetValue<string>("Pno"),
                        A_remark = reader.GetValue<string>("A_remark"),
                        A_state = reader.GetValue<int>("A_state"),
                        Action = reader.GetValue<int>("Action"),
                        Actiondate = reader.GetValue<DateTime>("Actiondate"),
                        Eticket_id = reader.GetValue<int>("Eticket_id"),
                        Use_pnum = reader.GetValue<int>("Use_pnum"),
                        RandomId = reader.GetValue<string>("randomid"),

                        PosId = reader.GetValue<int>("posid"),
                        Com_id = reader.GetValue<int>("com_id"),

                        Pcaccount = reader.GetValue<string>("Pcaccount"),
                    };
                }
                return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalB2bEticket
    {
        private SqlHelper sqlHelper;
        public InternalB2bEticket(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        #region 编辑电子票信息
        private static readonly string SQLInsertOrUpdate = "usp_InsertOrUpdateEticket";
        internal int InsertOrUpdate(B2b_eticket model)
        {
            var cmd = sqlHelper.PrepareStoredSqlCommand(SQLInsertOrUpdate);


            cmd.AddParam("@Id", model.Id);
            cmd.AddParam("@Com_id", model.Com_id);
            cmd.AddParam("@Pro_id", model.Pro_id);
            cmd.AddParam("@Agent_id", model.Agent_id);
            cmd.AddParam("@Pno", model.Pno);
            cmd.AddParam("@E_type", model.E_type);
            cmd.AddParam("@Pnum", model.Pnum);
            cmd.AddParam("@Use_pnum", model.Use_pnum);
            cmd.AddParam("@E_proname", model.E_proname);
            cmd.AddParam("@E_face_price", model.E_face_price);
            cmd.AddParam("@Subdate", model.Subdate);
            cmd.AddParam("@E_sale_price", model.E_sale_price);
            cmd.AddParam("@E_cost_price", model.E_cost_price);
            cmd.AddParam("@V_state", model.V_state);
            cmd.AddParam("@Send_state", model.Send_state);
            cmd.AddParam("@Oid", model.Oid);
            cmd.AddParam("@ishasdeposit", model.ishasdeposit);
            cmd.AddParam("@sendcard", model.sendcard);

            var parm = cmd.AddReturnValueParameter("ReturnValue");
            cmd.ExecuteNonQuery();
            return (int)parm.Value;
        }
        #endregion

        internal B2b_eticket GetEticketDetail(string pno, string posid)
        {
            string GetModelSql = "SELECT  [id],[com_id],[pro_id],[agent_id],[pno],[e_type],[pnum],[use_pnum],[e_proname] ,[e_face_price],[e_sale_price],[e_cost_price],[v_state],[send_state],[subdate],oid,[bindingname],[bindingphone],[bindingcard],sendcard,ishasdeposit  FROM [dbo].[b2b_eticket] where pno='" + pno.Trim() + "'";
            if (posid != "")
            {
                if (posid == "999999999") { }//测试pos
                else
                {
                    GetModelSql += " and com_id in (select com_id from b2b_company_pos where posid='" + posid.Trim() + "')";
                }
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(GetModelSql);


            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_eticket
                   {
                       Id = reader.GetValue<int>("id"),
                       Com_id = reader.GetValue<int>("com_id"),
                       Pro_id = reader.GetValue<int>("pro_id"),
                       Agent_id = reader.GetValue<int>("agent_id"),
                       Pno = reader.GetValue<string>("pno"),
                       E_type = reader.GetValue<int>("e_type"),
                       Pnum = reader.GetValue<int>("pnum"),
                       Use_pnum = reader.GetValue<int>("use_pnum"),
                       E_proname = reader.GetValue<string>("e_proname"),
                       E_face_price = reader.GetValue<decimal>("e_face_price"),
                       E_sale_price = reader.GetValue<decimal>("e_sale_price"),
                       E_cost_price = reader.GetValue<decimal>("e_cost_price"),
                       V_state = reader.GetValue<int>("v_state"),
                       Send_state = reader.GetValue<int>("send_state"),
                       Subdate = reader.GetValue<DateTime>("subdate"),
                       Oid = reader.GetValue<int>("oid"),
                       bindingname = reader.GetValue<string>("bindingname"),
                       bindingphone = reader.GetValue<string>("bindingphone"),
                       bindingcard = reader.GetValue<string>("bindingcard"),
                       sendcard = reader.GetValue<int>("sendcard"),
                       ishasdeposit = reader.GetValue<int>("ishasdeposit"),
                   };
                }
                return null;
            }


        }
        //通过身份证查询此身份证所有有效订单，返回可使用电子票
        internal List<B2b_eticket> GetEticketListbyidcard(string idcard, string posid)
        {
            string GetModelSql = "SELECT  [id],[com_id],[pro_id],[agent_id],[pno],[e_type],[pnum],[use_pnum],[e_proname] ,[e_face_price],[e_sale_price],[e_cost_price],[v_state],[send_state],[subdate],oid,[bindingname],[bindingphone],[bindingcard],sendcard,ishasdeposit  FROM [dbo].[b2b_eticket]";

            GetModelSql += " where Oid in (select id from b2b_order where u_idcard ='" + idcard + "') and Use_pnum>0";


            if (posid != "")
            {
                if (posid == "999999999") { }//测试pos
                else
                {
                    GetModelSql += " and com_id in (select com_id from b2b_company_pos where posid='" + posid.Trim() + "')";
                }
            }
            var cmd = sqlHelper.PrepareTextSqlCommand(GetModelSql);


            List<B2b_eticket> list = new List<B2b_eticket>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_eticket
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Pro_id = reader.GetValue<int>("pro_id"),
                        Agent_id = reader.GetValue<int>("agent_id"),
                        Pno = reader.GetValue<string>("pno"),
                        E_type = reader.GetValue<int>("e_type"),
                        Pnum = reader.GetValue<int>("pnum"),
                        Use_pnum = reader.GetValue<int>("use_pnum"),
                        E_proname = reader.GetValue<string>("e_proname"),
                        E_face_price = reader.GetValue<decimal>("e_face_price"),
                        E_sale_price = reader.GetValue<decimal>("e_sale_price"),
                        E_cost_price = reader.GetValue<decimal>("e_cost_price"),
                        V_state = reader.GetValue<int>("v_state"),
                        Send_state = reader.GetValue<int>("send_state"),
                        Subdate = reader.GetValue<DateTime>("subdate"),
                        Oid = reader.GetValue<int>("oid"),
                        bindingname = reader.GetValue<string>("bindingname"),
                        bindingphone = reader.GetValue<string>("bindingphone"),
                        bindingcard = reader.GetValue<string>("bindingcard"),
                        sendcard = reader.GetValue<int>("sendcard"),
                        ishasdeposit = reader.GetValue<int>("ishasdeposit"),
                    });
                }
            }
            return list;
        }



        internal string GetEticketOrderPno(int order_no, int agentid)
        {
            string GetModelSql = "SELECT  pno  FROM [EtownDB].[dbo].[b2b_eticket] where agent_id='" + agentid + "' and pno='" + order_no + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(GetModelSql);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<string>("pno");
                }
                return null;
            }
        }

        #region 分销退票，使用数量清零
        public int BackAgentEticket(int id, string pno)
        {
            string sqltxt = @"update b2b_eticket set use_pnum=0,v_state=4 where id=@id and pno=@pno ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@id", id);
            cmd.AddParam("@pno", pno);
            return cmd.ExecuteNonQuery();
        }
        #endregion


        public B2b_eticket GetEticketByID(string id)
        {
            const string GetModelSql = @"SELECT   [id]
      ,[com_id]
      ,[pro_id]
      ,[agent_id]
      ,[pno]
      ,[e_type]
      ,[pnum]
      ,[use_pnum]
      ,[e_proname]
      ,[e_face_price]
      ,[e_sale_price]
      ,[e_cost_price]
      ,[v_state]
      ,[send_state]
      ,[subdate]
,oid
,sendcard
,ishasdeposit
  FROM [dbo].[b2b_eticket] where  id=@id ";
            var cmd = sqlHelper.PrepareTextSqlCommand(GetModelSql);
            cmd.AddParam("@id", id);

            return cmd.ExecSingleReader<B2b_eticket>(reader => new B2b_eticket
            {
                Id = reader.GetValue<int>("id"),
                Com_id = reader.GetValue<int>("com_id"),
                Pro_id = reader.GetValue<int>("pro_id"),
                Agent_id = reader.GetValue<int>("agent_id"),
                Pno = reader.GetValue<string>("pno"),
                E_type = reader.GetValue<int>("e_type"),
                Pnum = reader.GetValue<int>("pnum"),
                Use_pnum = reader.GetValue<int>("use_pnum"),
                E_proname = reader.GetValue<string>("e_proname"),
                E_face_price = reader.GetValue<decimal>("e_face_price"),
                E_sale_price = reader.GetValue<decimal>("e_sale_price"),
                E_cost_price = reader.GetValue<decimal>("e_cost_price"),
                V_state = reader.GetValue<int>("v_state"),
                Send_state = reader.GetValue<int>("send_state"),
                Subdate = reader.GetValue<DateTime>("subdate"),
                Oid = reader.GetValue<int>("Oid"),
                sendcard = reader.GetValue<int>("sendcard"),
                ishasdeposit = reader.GetValue<int>("ishasdeposit"),
            });
        }

        internal B2b_eticket GetPnoDetail(string pno)
        {
            const string GetModelSql = @"select a.e_proname,a.use_pnum,a.pno,b.pro_end,b.service_contain,b.service_notcontain,b.precautions,
                c.com_add,d.scenic_name from b2b_eticket a 
                right join b2b_com_pro b on a.pro_id=b.id
                right join b2b_company_info c on b.com_id =c.com_id
                left join b2b_company d on c.com_id=d.id
                where a.pno=@pno";
            var cmd = sqlHelper.PrepareTextSqlCommand(GetModelSql);
            cmd.AddParam("@pno", pno);

            return cmd.ExecSingleReader<B2b_eticket>(reader => new B2b_eticket
            {
                E_proname = reader.GetValue<string>("e_proname"),
                Use_pnum = reader.GetValue<int>("use_pnum"),
                Compro = new B2b_com_pro()
                {
                    Pro_end = reader.GetValue<DateTime>("pro_end"),
                    Service_Contain = reader.GetValue<string>("service_contain"),
                    Service_NotContain = reader.GetValue<string>("service_notcontain"),
                    Precautions = reader.GetValue<string>("precautions")
                },
                Companyinfo = new B2b_company_info()
                {
                    Com_add = reader.GetValue<string>("com_add"),
                },
                Company = new B2b_company()
                {
                    Scenic_name = reader.GetValue<string>("scenic_name"),
                },


            });
        }

        internal B2b_eticket GetEticketSearch(string pno, int comid, int agentid)
        {
            string sql = @"SELECT [id]
      ,[com_id]
      ,[pro_id]
      ,[agent_id]
      ,[pno]
      ,[e_type]
      ,[pnum]
      ,[use_pnum]
      ,[e_proname]
      ,[e_face_price]
      ,[e_sale_price]
      ,[e_cost_price]
      ,[v_state]
      ,[send_state]
      ,[subdate]
,oid
,sendcard
,ishasdeposit
  FROM  [dbo].[b2b_eticket] where pno=@pno and com_id=@comid and oid in (select id from b2b_order where comid=@comid and agentid=@agentid)";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@pno", pno);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@agentid", agentid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_eticket
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Pro_id = reader.GetValue<int>("pro_id"),
                        Agent_id = reader.GetValue<int>("agent_id"),
                        Pno = reader.GetValue<string>("pno"),
                        E_type = reader.GetValue<int>("e_type"),
                        Pnum = reader.GetValue<int>("pnum"),
                        Use_pnum = reader.GetValue<int>("use_pnum"),
                        E_proname = reader.GetValue<string>("e_proname"),
                        E_face_price = reader.GetValue<decimal>("e_face_price"),
                        E_sale_price = reader.GetValue<decimal>("e_sale_price"),
                        E_cost_price = reader.GetValue<decimal>("e_cost_price"),
                        V_state = reader.GetValue<int>("v_state"),
                        Send_state = reader.GetValue<int>("send_state"),
                        Subdate = reader.GetValue<DateTime>("subdate"),
                        Oid = reader.GetValue<int>("Oid"),
                        sendcard = reader.GetValue<int>("sendcard"),
                        ishasdeposit = reader.GetValue<int>("ishasdeposit"),
                    };
                }
                return null;
            }

        }


        internal B2b_eticket GetPnoEticketinfo(string pno)
        {
            string sql = @"SELECT [id]
      ,[com_id]
      ,[pro_id]
      ,[agent_id]
      ,[pno]
      ,[e_type]
      ,[pnum]
      ,[use_pnum]
      ,[e_proname]
      ,[e_face_price]
      ,[e_sale_price]
      ,[e_cost_price]
      ,[v_state]
      ,[send_state]
      ,[subdate]
      ,oid
      ,sendcard
      ,ishasdeposit
  FROM  [dbo].[b2b_eticket] where pno=@pno ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@pno", pno);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_eticket
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Pro_id = reader.GetValue<int>("pro_id"),
                        Agent_id = reader.GetValue<int>("agent_id"),
                        Pno = reader.GetValue<string>("pno"),
                        E_type = reader.GetValue<int>("e_type"),
                        Pnum = reader.GetValue<int>("pnum"),
                        Use_pnum = reader.GetValue<int>("use_pnum"),
                        E_proname = reader.GetValue<string>("e_proname"),
                        E_face_price = reader.GetValue<decimal>("e_face_price"),
                        E_sale_price = reader.GetValue<decimal>("e_sale_price"),
                        E_cost_price = reader.GetValue<decimal>("e_cost_price"),
                        V_state = reader.GetValue<int>("v_state"),
                        Send_state = reader.GetValue<int>("send_state"),
                        Subdate = reader.GetValue<DateTime>("subdate"),
                        Oid = reader.GetValue<int>("Oid"),
                        sendcard = reader.GetValue<int>("sendcard"),
                        ishasdeposit = reader.GetValue<int>("ishasdeposit"),
                    };
                }
                return null;
            }

        }


        internal List<B2b_eticket> GetBackEticketlist(int comid, int agentid, int pageindex, int pagesize, out int totalcount)
        {

            var cmd = this.sqlHelper.PrepareStoredSqlCommand("usp_PagingLarge");
            var tblName = " b2b_eticket";
            var strGetFields = "*";
            var sortKey = "id";
            var sortMode = "1";
            var condition = " V_state=4 and (oid in (select id from b2b_order where comid=" + comid + " and agentid=" + agentid + ") or oid in (select Bindingagentorderid from b2b_order where comid=" + comid + " and agentid=" + agentid + ")) ";

            cmd.PagingCommand(tblName, strGetFields, pageindex, pagesize, sortKey, sortMode, condition);


            List<B2b_eticket> list = new List<B2b_eticket>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_eticket
                   {
                       Id = reader.GetValue<int>("id"),
                       Com_id = reader.GetValue<int>("com_id"),
                       Pro_id = reader.GetValue<int>("pro_id"),
                       Agent_id = reader.GetValue<int>("agent_id"),
                       Pno = reader.GetValue<string>("pno"),
                       E_type = reader.GetValue<int>("e_type"),
                       Pnum = reader.GetValue<int>("pnum"),
                       Use_pnum = reader.GetValue<int>("use_pnum"),
                       E_proname = reader.GetValue<string>("e_proname"),
                       E_face_price = reader.GetValue<decimal>("e_face_price"),
                       E_sale_price = reader.GetValue<decimal>("e_sale_price"),
                       E_cost_price = reader.GetValue<decimal>("e_cost_price"),
                       V_state = reader.GetValue<int>("v_state"),
                       Send_state = reader.GetValue<int>("send_state"),
                       Subdate = reader.GetValue<DateTime>("subdate"),
                       Oid = reader.GetValue<int>("Oid"),
                       sendcard = reader.GetValue<int>("sendcard"),
                       ishasdeposit = reader.GetValue<int>("ishasdeposit"),
                   });
                }
            }
            totalcount = int.Parse(cmd.Parameters[7].Value.ToString());
            return list;

        }

        internal B2b_eticket GetEticketDetail(int orderid, string openid)
        {
            string sql = @"SELECT [id]
      ,[com_id]
      ,[pro_id]
      ,[agent_id]
      ,[pno]
      ,[e_type]
      ,[pnum]
      ,[use_pnum]
      ,[e_proname]
      ,[e_face_price]
      ,[e_sale_price]
      ,[e_cost_price]
      ,[v_state]
      ,[send_state]
      ,[subdate]
  FROM  [dbo].[b2b_eticket] where id in (select ticketcode from b2b_order where id=@orderid )";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@orderid", orderid);
            cmd.AddParam("@openid", openid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_eticket
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Pro_id = reader.GetValue<int>("pro_id"),
                        Agent_id = reader.GetValue<int>("agent_id"),
                        Pno = reader.GetValue<string>("pno"),
                        E_type = reader.GetValue<int>("e_type"),
                        Pnum = reader.GetValue<int>("pnum"),
                        Use_pnum = reader.GetValue<int>("use_pnum"),
                        E_proname = reader.GetValue<string>("e_proname"),
                        E_face_price = reader.GetValue<decimal>("e_face_price"),
                        E_sale_price = reader.GetValue<decimal>("e_sale_price"),
                        E_cost_price = reader.GetValue<decimal>("e_cost_price"),
                        V_state = reader.GetValue<int>("v_state"),
                        Send_state = reader.GetValue<int>("send_state"),
                        Subdate = reader.GetValue<DateTime>("subdate"),
                    };
                }
                return null;
            }

        }

        internal B2b_eticket SelectOrderid(int orderid)
        {
            string sql = @"SELECT [id]
      ,[com_id]
      ,[pro_id]
      ,[agent_id]
      ,[pno]
      ,[e_type]
      ,[pnum]
      ,[use_pnum]
      ,[e_proname]
      ,[e_face_price]
      ,[e_sale_price]
      ,[e_cost_price]
      ,[v_state]
      ,[send_state]
      ,[subdate]
      ,oid
  FROM b2b_eticket where oid=@orderid ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@orderid", orderid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new B2b_eticket
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Pro_id = reader.GetValue<int>("pro_id"),
                        Agent_id = reader.GetValue<int>("agent_id"),
                        Pno = reader.GetValue<string>("pno"),
                        Oid = reader.GetValue<int>("oid"),
                        E_type = reader.GetValue<int>("e_type"),
                        Pnum = reader.GetValue<int>("pnum"),
                        Use_pnum = reader.GetValue<int>("use_pnum"),
                        E_proname = reader.GetValue<string>("e_proname"),
                        E_face_price = reader.GetValue<decimal>("e_face_price"),
                        E_sale_price = reader.GetValue<decimal>("e_sale_price"),
                        E_cost_price = reader.GetValue<decimal>("e_cost_price"),
                        V_state = reader.GetValue<int>("v_state"),
                        Send_state = reader.GetValue<int>("send_state"),
                        Subdate = reader.GetValue<DateTime>("subdate"),
                    };
                }
                return null;
            }

        }

        internal List<B2b_eticket> SelectOrderid_list(int orderid)
        {
            string sql = @"SELECT [id]
      ,[com_id]
      ,[pro_id]
      ,[agent_id]
      ,[pno]
      ,[e_type]
      ,[pnum]
      ,[use_pnum]
      ,[e_proname]
      ,[e_face_price]
      ,[e_sale_price]
      ,[e_cost_price]
      ,[v_state]
      ,[send_state]
      ,[subdate]
      ,oid
  FROM b2b_eticket where oid=@orderid ";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@orderid", orderid);

            List<B2b_eticket> list = new List<B2b_eticket>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new B2b_eticket
                   {
                       Id = reader.GetValue<int>("id"),
                       Com_id = reader.GetValue<int>("com_id"),
                       Pro_id = reader.GetValue<int>("pro_id"),
                       Agent_id = reader.GetValue<int>("agent_id"),
                       Pno = reader.GetValue<string>("pno"),
                       Oid = reader.GetValue<int>("oid"),
                       E_type = reader.GetValue<int>("e_type"),
                       Pnum = reader.GetValue<int>("pnum"),
                       Use_pnum = reader.GetValue<int>("use_pnum"),
                       E_proname = reader.GetValue<string>("e_proname"),
                       E_face_price = reader.GetValue<decimal>("e_face_price"),
                       E_sale_price = reader.GetValue<decimal>("e_sale_price"),
                       E_cost_price = reader.GetValue<decimal>("e_cost_price"),
                       V_state = reader.GetValue<int>("v_state"),
                       Send_state = reader.GetValue<int>("send_state"),
                       Subdate = reader.GetValue<DateTime>("subdate")
                   });
                }
                return list;
            }

        }


        #region 退票，使用数量清零
        internal int SelectEticketUnUsebyOrderid(int orderid)
        {
            string sqltxt = @"select sum(use_pnum) as use_pnum from b2b_eticket where oid=@orderId";
            try
            {
                var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
                cmd.AddParam("@orderId", orderid);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetValue<int>("use_pnum");
                    }
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region 退票，使用数量清零
        internal int Backticket_use_num(int orderid)
        {
            string sqltxt = @"update b2b_eticket set use_pnum=0,v_state=4 where oid=@orderId";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@orderId", orderid);
            return cmd.ExecuteNonQuery();
        }
        #endregion


        #region 退票，使用数量清零
        internal int Backticket_yuyuepno_num(string pno, int num)
        {
            string sqltxt = @"update b2b_eticket set use_pnum=use_pnum+@num where pno=@pno";
            var cmd = sqlHelper.PrepareTextSqlCommand(sqltxt);
            cmd.AddParam("@pno", pno);
            cmd.AddParam("@num", num);
            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region 按产品统计验票数量，分销
        internal int GetEticketVCount(int comid, int agentid, int proid, string startime = "", string endtime = "")
        {
            string sql = @"select sum(use_pnum) as use_pnum from b2b_etcket_log where eticket_id in (SELECT id FROM [b2b_eticket] where oid in (select id from b2b_order where comid=@comid and agentid=@agentid and pro_id=@proid ) or oid in (select bindingagentorderid from b2b_order where comid=@comid and agentid=@agentid and pro_id in (select id from b2b_com_pro where bindingid=@proid ) and bindingagentorderid !=0  ) ) and action=1 and a_state=1";

            if (startime != "")
            {
                sql += " and actiondate>='" + startime + "'";
            }
            if (endtime != "")
            {
                endtime = DateTime.Parse(endtime).AddDays(1).ToString();
                sql += " and actiondate<'" + endtime + "'";
            }


            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@agentid", agentid);
            cmd.AddParam("@proid", proid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("use_pnum");

                }
                return 0;
            }

        }
        #endregion


        #region 按订单统计验票数量，分销
        internal int GetEticketOrderVCount(int comid, int agentid, int orderid)
        {
            string sql = @"select sum(use_pnum) as use_pnum from b2b_etcket_log where eticket_id in (SELECT id FROM [b2b_eticket] where com_id=@comid and oid =@orderid) and action=1 and a_state=1";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@agentid", agentid);
            cmd.AddParam("@orderid", orderid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("use_pnum");

                }
                return 0;
            }

        }
        #endregion

        #region 未使用数量
        internal int GetEticketOrderVoidCount(int comid, int agentid, int orderid)
        {
            string sql = @"SELECT sum(use_pnum) as use_pnum FROM [b2b_eticket] where com_id=@comid and oid =@orderid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@agentid", agentid);
            cmd.AddParam("@orderid", orderid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("use_pnum");
                }
                return 0;
            }

        }
        #endregion

        #region 作废未使用电子码，只针对倒码产品，所以不需要财务记录
        internal int EticketOrderVoid(int comid, int agentid, int orderid)
        {
            string sql = @"update b2b_eticket set use_pnum=0,v_state=4 where oid=@orderid and com_id=@comid and use_pnum>0 ";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@comid", comid);
            cmd.AddParam("@agentid", agentid);
            cmd.AddParam("@orderid", orderid);
            return cmd.ExecuteNonQuery();

        }
        #endregion




        #region 通过电子票id获取电子票详情
        internal int GetEticketbyid(int eticketid)
        {
            string sql = @"SELECT oid FROM [b2b_eticket] where id=@eticketid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@eticketid", eticketid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("oid");
                }
                return 0;
            }

        }
        #endregion
        #region 通过电子票id获取电子票详情
        internal string GetOutcompanybyid(int eticketid)
        {
            string sql = @"select * from agent_company where id in (SELECT agent_id FROM [b2b_eticket] where id=@eticketid)";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@eticketid", eticketid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<string>("company");
                }
                return "";
            }

        }
        #endregion


        #region 通过订单id获取电子票详情
        internal string GetOutcompanybyorderid(int orderid)
        {
            string sql = @"select company from agent_company where id in (SELECT agentid FROM [b2b_order] where id=@orderid)";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@orderid", orderid);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<string>("company");
                }
                return "";
            }

        }
        #endregion



        //查询未打印电子票数量,必须是未使用的电子码
        internal List<B2b_eticket> PrintTicketbyOrderid(int orderid, int size)
        {
            string sql = @"SELECT top " + size + " * FROM b2b_eticket where oid=@orderid and Printstate=0 and use_pnum>0";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@orderid", orderid);
            using (var reader = cmd.ExecuteReader())
            {
                List<B2b_eticket> list = new List<B2b_eticket>();
                while (reader.Read())
                {
                    list.Add(new B2b_eticket
                   {
                       Id = reader.GetValue<int>("id"),
                       Com_id = reader.GetValue<int>("com_id"),
                       Pro_id = reader.GetValue<int>("pro_id"),
                       Agent_id = reader.GetValue<int>("agent_id"),
                       Pno = reader.GetValue<string>("pno"),
                       Oid = reader.GetValue<int>("oid"),
                       E_type = reader.GetValue<int>("e_type"),
                       Pnum = reader.GetValue<int>("pnum"),
                       Use_pnum = reader.GetValue<int>("use_pnum"),
                       E_proname = reader.GetValue<string>("e_proname"),
                       E_face_price = reader.GetValue<decimal>("e_face_price"),
                       E_sale_price = reader.GetValue<decimal>("e_sale_price"),
                       E_cost_price = reader.GetValue<decimal>("e_cost_price"),
                       V_state = reader.GetValue<int>("v_state"),
                       Send_state = reader.GetValue<int>("send_state"),
                       Subdate = reader.GetValue<DateTime>("subdate"),
                       Pagecode = reader.GetValue<decimal>("Pagecode"),
                       Printstate = reader.GetValue<int>("Printstate"),
                   });


                }
                return list;
            }
        }

        //查询已打印电子票列表
        internal List<B2b_eticket> AlreadyPrintbyOrderid(int orderid)
        {
            string sql = @"SELECT *
  FROM b2b_eticket where oid=@orderid and Printstate=1";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@orderid", orderid);
            using (var reader = cmd.ExecuteReader())
            {
                List<B2b_eticket> list = new List<B2b_eticket>();
                if (reader.Read())
                {
                    list.Add(new B2b_eticket
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Pro_id = reader.GetValue<int>("pro_id"),
                        Agent_id = reader.GetValue<int>("agent_id"),
                        Pno = reader.GetValue<string>("pno"),
                        Oid = reader.GetValue<int>("oid"),
                        E_type = reader.GetValue<int>("e_type"),
                        Pnum = reader.GetValue<int>("pnum"),
                        Use_pnum = reader.GetValue<int>("use_pnum"),
                        E_proname = reader.GetValue<string>("e_proname"),
                        E_face_price = reader.GetValue<decimal>("e_face_price"),
                        E_sale_price = reader.GetValue<decimal>("e_sale_price"),
                        E_cost_price = reader.GetValue<decimal>("e_cost_price"),
                        V_state = reader.GetValue<int>("v_state"),
                        Send_state = reader.GetValue<int>("send_state"),
                        Subdate = reader.GetValue<DateTime>("subdate"),
                        Pagecode = reader.GetValue<decimal>("Pagecode"),
                        Printstate = reader.GetValue<int>("Printstate"),
                    });
                }
                return null;
            }
        }

        //查询未打印电子票数量
        internal int AlreadyPrintNumbyOrderid(int orderid)
        {
            string sql = @"SELECT count(id) num
  FROM b2b_eticket where oid=@orderid and Printstate=1";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@orderid", orderid);
            using (var reader = cmd.ExecuteReader())
            {

                if (reader.Read())
                {

                    return reader.GetValue<int>("num");

                }
                return 0;
            }
        }

        //修改电子票打印状态
        internal int PrintStateUp(int eticketid)
        {
            string sql = @"update b2b_eticket set Printstate=1 where id=@eticketid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@eticketid", eticketid);
            return cmd.ExecuteNonQuery();

        }

        //修改电子票票纸号
        internal int PrintStateUpPagecode(int eticketid, string Pagecode)
        {
            string sql = @"update b2b_eticket set Pagecode=@Pagecode where id=@eticketid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@eticketid", eticketid);
            cmd.AddParam("@Pagecode", Pagecode);

            return cmd.ExecuteNonQuery();

        }

        internal B2b_eticket GetEticketByOrderid(int orderid)
        {
            string sql = @"SELECT top 1  *
  FROM b2b_eticket where oid=@orderid order by id desc";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@orderid", orderid);
            using (var reader = cmd.ExecuteReader())
            {
                B2b_eticket r = null;
                if (reader.Read())
                {
                    r = new B2b_eticket
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Pro_id = reader.GetValue<int>("pro_id"),
                        Agent_id = reader.GetValue<int>("agent_id"),
                        Pno = reader.GetValue<string>("pno"),
                        Oid = reader.GetValue<int>("oid"),
                        E_type = reader.GetValue<int>("e_type"),
                        Pnum = reader.GetValue<int>("pnum"),
                        Use_pnum = reader.GetValue<int>("use_pnum"),
                        E_proname = reader.GetValue<string>("e_proname"),
                        E_face_price = reader.GetValue<decimal>("e_face_price"),
                        E_sale_price = reader.GetValue<decimal>("e_sale_price"),
                        E_cost_price = reader.GetValue<decimal>("e_cost_price"),
                        V_state = reader.GetValue<int>("v_state"),
                        Send_state = reader.GetValue<int>("send_state"),
                        Subdate = reader.GetValue<DateTime>("subdate"),
                        Pagecode = reader.GetValue<decimal>("Pagecode"),
                        Printstate = reader.GetValue<int>("Printstate"),
                    };
                }
                return r;
            }
        }

        //internal B2b_eticket GetEticketByTaobaoOrderid(string taobaoorderid)
        //{
        //    string sql = @"select top 1 * from  b2b_eticket where oid=(select top 1 self_order_id from taobao_send_noticelog where order_id=@taobaoorderid order by id desc)";

        //    var cmd = sqlHelper.PrepareTextSqlCommand(sql);
        //    cmd.AddParam("@taobaoorderid", taobaoorderid);
        //    using (var reader = cmd.ExecuteReader())
        //    {
        //        B2b_eticket r = null;
        //        if (reader.Read())
        //        {
        //            r = new B2b_eticket
        //            {
        //                Id = reader.GetValue<int>("id"),
        //                Com_id = reader.GetValue<int>("com_id"),
        //                Pro_id = reader.GetValue<int>("pro_id"),
        //                Agent_id = reader.GetValue<int>("agent_id"),
        //                Pno = reader.GetValue<string>("pno"),
        //                Oid = reader.GetValue<int>("oid"),
        //                E_type = reader.GetValue<int>("e_type"),
        //                Pnum = reader.GetValue<int>("pnum"),
        //                Use_pnum = reader.GetValue<int>("use_pnum"),
        //                E_proname = reader.GetValue<string>("e_proname"),
        //                E_face_price = reader.GetValue<decimal>("e_face_price"),
        //                E_sale_price = reader.GetValue<decimal>("e_sale_price"),
        //                E_cost_price = reader.GetValue<decimal>("e_cost_price"),
        //                V_state = reader.GetValue<int>("v_state"),
        //                Send_state = reader.GetValue<int>("send_state"),
        //                Subdate = reader.GetValue<DateTime>("subdate"),
        //                Pagecode = reader.GetValue<decimal>("Pagecode"),
        //                Printstate = reader.GetValue<int>("Printstate"),
        //            };
        //        }
        //        return r;
        //    }
        //}


        //退票订单，退票数量，剩余数量,如果退票数量大于等于库存数则清0，并且更改状态，如果退票数量小于库存数量只是扣减数量 不更改状态
        internal int UpPnoNumZero(int orderid, int backnum, int libnum)
        {
            try
            {
                string sql = "";
                if (backnum >= libnum)
                {
                    sql = "update b2b_eticket set use_pnum=0,v_state=4 where oid=" + orderid;
                }
                else
                {

                    sql = "update b2b_eticket set use_pnum=use_pnum-" + backnum + " where oid=" + orderid;
                }

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                return cmd.ExecuteNonQuery();
            }
            catch  
            {
                return 0;
            }
        }

        internal List<B2b_eticket> GetEticketListByOrderid(int orderid)
        {
            string sql = @"SELECT [id]
      ,[com_id]
      ,[pro_id]
      ,[agent_id]
      ,[pno]
      ,[e_type]
      ,[pnum]
      ,[use_pnum]
      ,[e_proname]
      ,[e_face_price]
      ,[e_sale_price]
      ,[e_cost_price]
      ,[v_state]
      ,[send_state]
      ,[subdate]
      ,[oid]
      ,[pagecode]
      ,[printstate]
  FROM  [b2b_eticket] where oid=@oid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@oid", orderid);
            using (var reader = cmd.ExecuteReader())
            {
                List<B2b_eticket> m = new List<B2b_eticket>();
                while (reader.Read())
                {
                    m.Add(new B2b_eticket
                    {
                        Id = reader.GetValue<int>("id"),
                        Com_id = reader.GetValue<int>("com_id"),
                        Pro_id = reader.GetValue<int>("pro_id"),
                        Agent_id = reader.GetValue<int>("agent_id"),
                        Pno = reader.GetValue<string>("pno"),
                        E_type = reader.GetValue<int>("e_type"),
                        Pnum = reader.GetValue<int>("pnum"),
                        Use_pnum = reader.GetValue<int>("use_pnum"),
                        E_proname = reader.GetValue<string>("e_proname"),
                        E_face_price = reader.GetValue<decimal>("e_face_price"),
                        E_sale_price = reader.GetValue<decimal>("e_sale_price"),
                        E_cost_price = reader.GetValue<decimal>("e_cost_price"),

                        V_state = reader.GetValue<int>("v_state"),
                        Send_state = reader.GetValue<int>("send_state"),
                        Subdate = reader.GetValue<DateTime>("subdate"),
                        Oid = reader.GetValue<int>("oid"),
                        Pagecode = reader.GetValue<decimal>("pagecode"),
                        Printstate = reader.GetValue<int>("printstate"),
                    });
                }
                return m;
            }

        }



        #region 修改电子票绑定信息
        internal int bindingpnoUpdatepeople(B2b_eticket model)
        {
                string sql = "update b2b_eticket set bindingname='" + model.bindingname + "',bindingphone='" + model.bindingphone + "',bindingcard='" + model.bindingcard + "' where id=" + model.Id;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                return cmd.ExecuteNonQuery();
        }
        #endregion


        //修改电子票票纸号
        internal int InsertOrUpdateB2b_eticket_Deposit(b2b_eticket_Deposit eticketDeposit)
        {
            string sql = "";
            if (eticketDeposit.id == 0)
            {
                sql = "insert b2b_eticket_Deposit (eticketid,sid,saleprice,Depositprice,Depositorder,Backdepositstate) values(@eticketid,@sid,@saleprice,@Depositprice,@Depositorder,@Backdepositstate);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", eticketDeposit.id);
                cmd.AddParam("@eticketid", eticketDeposit.eticketid);
                cmd.AddParam("@sid", eticketDeposit.sid);
                cmd.AddParam("@saleprice", eticketDeposit.saleprice);
                cmd.AddParam("@Depositprice", eticketDeposit.Depositprice);
                cmd.AddParam("@Depositorder", eticketDeposit.Depositorder);
                cmd.AddParam("@Backdepositstate", eticketDeposit.Backdepositstate);

                object o = cmd.ExecuteScalar();
                int newId = o == null ? 0 : int.Parse(o.ToString());
                return newId;
            }
            else {
                sql = "update b2b_eticket_Deposit set eticketid=@eticketid,sid=@sid,saleprice=@saleprice,Depositprice=@Depositprice,Depositorder=@Depositorder,Backdepositstate=@Backdepositstate where id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", eticketDeposit.id);
                cmd.AddParam("@eticketid", eticketDeposit.eticketid);
                cmd.AddParam("@sid", eticketDeposit.sid);
                cmd.AddParam("@saleprice", eticketDeposit.saleprice);
                cmd.AddParam("@Depositprice", eticketDeposit.Depositprice);
                cmd.AddParam("@Depositorder", eticketDeposit.Depositorder);
                cmd.AddParam("@Backdepositstate", eticketDeposit.Backdepositstate);
                return cmd.ExecuteNonQuery();
            }

            

        }
        //通过
        internal List<b2b_eticket_Deposit> GetB2b_eticket_DepositBypno(int eid)
        {
            string sql = @"SELECT *
  FROM  [b2b_eticket_Deposit] where eticketid=@eid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@eid", eid);
            using (var reader = cmd.ExecuteReader())
            {
                List<b2b_eticket_Deposit> m = new List<b2b_eticket_Deposit>();
                while (reader.Read())
                {
                    m.Add(new b2b_eticket_Deposit
                    {
                        id = reader.GetValue<int>("id"),
                        eticketid = reader.GetValue<int>("eticketid"),
                        sid = reader.GetValue<int>("sid"),
                        saleprice = reader.GetValue<decimal>("saleprice"),
                        Depositprice = reader.GetValue<decimal>("Depositprice"),
                        Depositorder = reader.GetValue<int>("Depositorder"),
                        Backdepositstate = reader.GetValue<int>("Backdepositstate")
                        
                    });
                }
                return m;
            }

        }


        //查询押金状态
        internal b2b_eticket_Deposit GetB2b_eticket_DepositByid(int id)
        {
            string sql = @"SELECT *
  FROM  [b2b_eticket_Deposit] where id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@eid", id);
            using (var reader = cmd.ExecuteReader())
            {
                b2b_eticket_Deposit r = null;
                if (reader.Read())
                {
                    r = new b2b_eticket_Deposit
                    {
                        id = reader.GetValue<int>("id"),
                        eticketid = reader.GetValue<int>("eticketid"),
                        sid = reader.GetValue<int>("sid"),
                        saleprice = reader.GetValue<decimal>("saleprice"),
                        Depositprice = reader.GetValue<decimal>("Depositprice"),
                        Depositorder = reader.GetValue<int>("Depositorder"),
                        Backdepositstate = reader.GetValue<int>("Backdepositstate")
                    };
                }
                return r;
            }

        }

        //修改电子票发卡状态
        internal int UpbacketicketDepositstate(int id)
        {
            try
            {
                string sql = "update b2b_eticket_Deposit set Backdepositstate=1 where id=" + id;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                return cmd.ExecuteNonQuery();
            }
            catch
            {
                return 0;
            }
        }


        //修改电子票发卡状态
        internal int Upeticketsencardstate(int eticketid, int state)
        {
            try
            {
                string sql = "update b2b_eticket set sendcard=" + state + " where id=" + eticketid;
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                return cmd.ExecuteNonQuery();
            }
            catch
            {
                return 0;
            }
        }


        internal int ClearPnoNum(string pno)
        {
            try
            {
                string sql ="update b2b_eticket set use_pnum=0 where pno=@pno";
                

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                return cmd.ExecuteNonQuery();
            }
            catch
            {
                return 0;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.LMM.Model;

namespace ETS2.PM.Service.LMM.Data.Internal
{
    public class Internallvmama_reqlogData
    {

        public SqlHelper sqlHelper;
        public Internallvmama_reqlogData(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int EditReqlog(Lvmama_reqlog m)
        {
            if (m.id == 0)
            {
                string sql = @"INSERT INTO  [Lvmama_reqlog]
                           ([reqstr]
                           ,[subtime]
                           ,[respstr]
                           ,[resptime]
                           ,[code]
                           ,[describe]
                           ,[req_type]
                           ,[sendip]
                           ,mtorderid
                           ,ordernum
                           ,issecond_req,stockagentcompanyid)
                     VALUES
                           (@reqstr 
                           ,@subtime 
                           ,@respstr 
                           ,@resptime 
                           ,@code
                           ,@describe 
                           ,@req_type 
                           ,@sendip
                           ,@mtorderid
                           ,@ordernum
                           ,@issecond_req,@stockagentcompanyid);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@reqstr", m.reqstr);
                cmd.AddParam("@subtime", m.subtime);
                cmd.AddParam("@respstr", m.respstr);
                cmd.AddParam("@resptime", m.resptime);
                cmd.AddParam("@code", m.code);
                cmd.AddParam("@describe", m.describe);
                cmd.AddParam("@req_type", m.req_type);
                cmd.AddParam("@sendip", m.sendip);
               
                if (m.mtorderid == null)
                {
                    m.mtorderid = "";
                }
                cmd.AddParam("@mtorderid", m.mtorderid);
                if (m.ordernum == null)
                {
                    m.ordernum = "";
                }
                cmd.AddParam("@ordernum", m.ordernum);
                if (m.issecond_req == null)
                {
                    m.issecond_req = 0;
                }
                cmd.AddParam("@issecond_req", m.issecond_req);
                if(m.stockagentcompanyid==null){
                    m.stockagentcompanyid = 0;
                }
                cmd.AddParam("@stockagentcompanyid", m.stockagentcompanyid);

                object o = cmd.ExecuteScalar();
                return o == null ? 0 : int.Parse(o.ToString());
            }
            else
            {
                string sql = @"UPDATE  [Lvmama_reqlog]
                               SET [reqstr] = @reqstr 
                                  ,[subtime] = @subtime 
                                  ,[respstr] = @respstr 
                                  ,[resptime] = @resptime 
                                  ,[code] = @code 
                                  ,[describe] = @describe 
                                  ,[req_type] = @req_type 
                                  ,[sendip] = @sendip 
                                  ,mtorderid=@mtorderid
                                  ,ordernum=@ordernum
                                  ,issecond_req=@issecond_req
                                  ,stockagentcompanyid=@stockagentcompanyid
                             WHERE id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", m.id);
                cmd.AddParam("@reqstr", m.reqstr);
                cmd.AddParam("@subtime", m.subtime);

                if (m.respstr == null)
                {
                    m.respstr = "";
                }
                cmd.AddParam("@respstr", m.respstr);
                cmd.AddParam("@resptime", m.resptime);
                cmd.AddParam("@code", m.code);
                cmd.AddParam("@describe", m.describe);
                cmd.AddParam("@req_type", m.req_type);
                cmd.AddParam("@sendip", m.sendip); 
                if (m.mtorderid == null)
                {
                    m.mtorderid = "";
                }
                cmd.AddParam("@mtorderid", m.mtorderid);
                if (m.ordernum == null)
                {
                    m.ordernum = "";
                }
                cmd.AddParam("@ordernum", m.ordernum);
                if (m.issecond_req == null)
                {
                    m.issecond_req = 0;
                }
                cmd.AddParam("@issecond_req", m.issecond_req);
                if (m.stockagentcompanyid == null)
                {
                    m.stockagentcompanyid = 0;
                }
                cmd.AddParam("@stockagentcompanyid", m.stockagentcompanyid);

                cmd.ExecuteNonQuery();
                return m.id;
            }
        }

        internal Lvmama_reqlog GetLvmama_Orderpayreqlog(string mtorderid, string code)
        {
            string sql = @"SELECT top 1  *
  FROM  Lvmama_reqlog where   req_type=@req_type and  mtorderid =@mtorderid and ordernum!='' and code=@code order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@req_type", "orderpay.aspx");
            cmd.AddParam("@mtorderid", mtorderid);
            cmd.AddParam("@code", code);
            using (var reader = cmd.ExecuteReader())
            {
                Lvmama_reqlog m = null;
                if (reader.Read())
                {
                    m = new Lvmama_reqlog
                    {
                        id = reader.GetValue<int>("id"),
                        reqstr = reader.GetValue<string>("reqstr"),
                        subtime = reader.GetValue<string>("subtime"),
                        respstr = reader.GetValue<string>("respstr"),
                        resptime = reader.GetValue<string>("resptime"),
                        code = reader.GetValue<string>("code"),
                        describe = reader.GetValue<string>("describe"),
                        req_type = reader.GetValue<string>("req_type"),
                        sendip = reader.GetValue<string>("sendip"),
                        mtorderid = reader.GetValue<string>("mtorderid"),
                        ordernum = reader.GetValue<string>("ordernum"),
                        issecond_req = reader.GetValue<int>("issecond_req"),
                    };
                }
                return m;
            }
        }

        internal Lvmama_reqlog GetLvmama_OrderpayreqlogBySelforderid(int a_orderid, string code)
        {
            string sql = @"SELECT top 1  *
  FROM  Lvmama_reqlog where   req_type=@req_type and ordernum=@ordernum and code=@code order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@req_type", "apply_code");
            cmd.AddParam("@ordernum", a_orderid);
            cmd.AddParam("@code", code);
            using (var reader = cmd.ExecuteReader())
            {
                Lvmama_reqlog m = null;
                if (reader.Read())
                {
                    m = new Lvmama_reqlog
                    {
                        id = reader.GetValue<int>("id"),
                        reqstr = reader.GetValue<string>("reqstr"),
                        subtime = reader.GetValue<string>("subtime"),
                        respstr = reader.GetValue<string>("respstr"),
                        resptime = reader.GetValue<string>("resptime"),
                        code = reader.GetValue<string>("code"),
                        describe = reader.GetValue<string>("describe"),
                        req_type = reader.GetValue<string>("req_type"),
                        sendip = reader.GetValue<string>("sendip"),
                        mtorderid = reader.GetValue<string>("mtorderid"),
                        ordernum = reader.GetValue<string>("ordernum"),
                        issecond_req = reader.GetValue<int>("issecond_req"),
                    };
                }
                return m;
            }
        }

        internal Lvmama_reqlog GetLvmamaOrderCreateLogByLvmamaorder(string mtorderid, string code)
        {
            string sql = @"SELECT top 1  *
  FROM  Lvmama_reqlog where   req_type=@req_type and mtorderid=@mtorderid and code=@code order by id desc";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@req_type", "apply_code");
            cmd.AddParam("@mtorderid", mtorderid);
            cmd.AddParam("@code", code);
            using (var reader = cmd.ExecuteReader())
            {
                Lvmama_reqlog m = null;
                if (reader.Read())
                {
                    m = new Lvmama_reqlog
                    {
                        id = reader.GetValue<int>("id"),
                        reqstr = reader.GetValue<string>("reqstr"),
                        subtime = reader.GetValue<string>("subtime"),
                        respstr = reader.GetValue<string>("respstr"),
                        resptime = reader.GetValue<string>("resptime"),
                        code = reader.GetValue<string>("code"),
                        describe = reader.GetValue<string>("describe"),
                        req_type = reader.GetValue<string>("req_type"),
                        sendip = reader.GetValue<string>("sendip"),
                        mtorderid = reader.GetValue<string>("mtorderid"),
                        ordernum = reader.GetValue<string>("ordernum"),
                        issecond_req = reader.GetValue<int>("issecond_req"),
                    };
                }
                return m;
            }
        }

        internal int GetSysorderidByLvmamaorderid(string mtorderid)
        {
            string sql = "select ordernum from Lvmama_reqlog where mtorderid=@mtorderid and ordernum!=''";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@mtorderid",mtorderid);
            using(var reader=cmd.ExecuteReader())
            {
               if(reader.Read())
               {
                   string ordernum= reader.GetValue<string>("ordernum");
                   return int.Parse(ordernum);
               }
               return 0;
            }
        }

    }
}

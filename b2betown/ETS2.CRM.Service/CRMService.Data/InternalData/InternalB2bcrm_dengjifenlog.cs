using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.CRM.Service.CRMService.Data.InternalData
{
    public class InternalB2bcrm_dengjifenlog
    {
        public SqlHelper sqlHelper;
        public InternalB2bcrm_dengjifenlog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal int InsertOrderUpdate(B2bcrm_dengjifenlog m)
        {
            if (m.id == 0)
            {
                string sql = @"INSERT INTO [EtownDB].[dbo].[b2bcrm_dengjifenlog]
           ([crmid]
           ,[dengjifen]
           ,[ptype]
           ,[opertor]
           ,[opertime]
           ,[orderid]
           ,[ordername]
           ,[remark])
     VALUES
           (@crmid
           ,@dengjifen
           ,@ptype
           ,@opertor
           ,@opertime
           ,@orderid
           ,@ordername
           ,@remark);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@crmid", m.crmid);
                cmd.AddParam("@dengjifen", m.dengjifen);
                cmd.AddParam("@ptype", m.ptype);
                cmd.AddParam("@opertor", m.opertor);
                cmd.AddParam("@opertime", m.opertime);
                cmd.AddParam("@orderid", m.orderid);
                cmd.AddParam("@ordername", m.ordername);
                cmd.AddParam("@remark", m.remark);

                object o = cmd.ExecuteScalar();
                int id = o == null ? 0 : int.Parse(o.ToString());
                return id;
            }
            else
            {
                string sql = @"UPDATE [EtownDB].[dbo].[b2bcrm_dengjifenlog]
                               SET [crmid] = @crmid
                                  ,[dengjifen] = @dengjifen
                                  ,[ptype] =@ptype
                                  ,[opertor] = @opertor
                                  ,[opertime] = @opertime
                                  ,[orderid] = @orderid
                                  ,[ordername] = @ordername
                                  ,[remark] = @remark
                             WHERE id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", m.id);
                cmd.AddParam("@crmid", m.crmid);
                cmd.AddParam("@dengjifen", m.dengjifen);
                cmd.AddParam("@ptype", m.ptype);
                cmd.AddParam("@opertor", m.opertor);
                cmd.AddParam("@opertime", m.opertime);
                cmd.AddParam("@orderid", m.orderid);
                cmd.AddParam("@ordername", m.ordername);
                cmd.AddParam("@remark", m.remark);

                cmd.ExecuteNonQuery();
                return m.id;

            }
        }



        internal List<B2bcrm_dengjifenlog> Getdengjifenlist(int id, int comid, out int totalcount)
        {
            string sql = "select count(1) from b2bcrm_dengjifenlog where crmid=" + id;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            object o = cmd.ExecuteScalar();
            totalcount = o == null ? 0 : int.Parse(o.ToString());

            string sql2 = "select top 10  id,crmid,dengjifen,ptype,opertor,opertime,orderid,ordername,remark from b2bcrm_dengjifenlog where crmid=" + id+"  order by id desc";
            cmd = sqlHelper.PrepareTextSqlCommand(sql2);

            using (var reader = cmd.ExecuteReader())
            {
                List<B2bcrm_dengjifenlog> list = new List<B2bcrm_dengjifenlog>();
                while (reader.Read())
                {
                    list.Add(new B2bcrm_dengjifenlog()
                    {
                        id = reader.GetValue<int>("id"),
                        crmid = reader.GetValue<int>("crmid"),
                        dengjifen = reader.GetValue<decimal>("dengjifen"),
                        ptype = reader.GetValue<int>("ptype"),
                        opertor = reader.GetValue<string>("opertor"),
                        opertime = reader.GetValue<DateTime>("opertime"),
                        orderid = reader.GetValue<int>("orderid"),
                        ordername = reader.GetValue<string>("ordername"),
                        remark = reader.GetValue<string>("remark")
                    });
                }
                return list;
            }
        }
    }
}

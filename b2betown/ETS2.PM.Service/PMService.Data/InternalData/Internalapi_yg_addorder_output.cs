using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalapi_yg_addorder_output
    {
        public SqlHelper sqlHelper;
        public Internalapi_yg_addorder_output(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int EditApi_yg_addorder_output(Modle.Api_yg_addorder_output m)
        {
            if (m.id > 0)
            {
                string sql = @"INSERT  [api_yg_addorder_output]
                                   ([req_seq]
                                   ,[resultid]
                                   ,[resultcomment]
                                   ,[yg_ordernum]
                                   ,[code]
                                   ,[orderId])
                             VALUES
                                   (@req_seq 
                                   ,@resultid 
                                   ,@resultcomment 
                                   ,@yg_ordernum 
                                   ,@code 
                                   ,@orderId);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@req_seq", m.req_seq);
                cmd.AddParam("@resultid", m.resultid);
                cmd.AddParam("@resultcomment", m.resultcomment);
                cmd.AddParam("@yg_ordernum", m.yg_ordernum);
                cmd.AddParam("@code", m.code);
                cmd.AddParam("@orderId", m.orderId);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
            else
            {
                string sql = @"UPDATE [api_yg_addorder_output]
                           SET [req_seq] = @req_seq
                              ,[resultid] = @resultid 
                              ,[resultcomment] = @resultcomment 
                              ,[yg_ordernum] = @yg_ordernum 
                              ,[code] = @code 
                              ,[orderId] = @orderId 
                         WHERE id=@id;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", m.id);
                cmd.AddParam("@req_seq", m.req_seq);
                cmd.AddParam("@resultid", m.resultid);
                cmd.AddParam("@resultcomment", m.resultcomment);
                cmd.AddParam("@yg_ordernum", m.yg_ordernum);
                cmd.AddParam("@code", m.code);
                cmd.AddParam("@orderId", m.orderId);

                cmd.ExecuteNonQuery();
                return m.id;
            }

        }

        internal string GetApi_yg_ordernum(int sysorderid)
        {
            string sql = "select yg_ordernum from api_yg_addorder_output  where  orderId=" + sysorderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                string ygordernum = "";
                if (reader.Read())
                {
                    ygordernum = reader.GetValue<string>("yg_ordernum");
                }
                return ygordernum;
            }
        }

        internal Api_yg_addorder_output Getapi_yg_addorder_output(int sysorderid)
        {
            string sql = "select * from api_yg_addorder_output where orderid=" + sysorderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                Api_yg_addorder_output m = null;
                if (reader.Read())
                {
                    m = new Api_yg_addorder_output
                    {
                        id = reader.GetValue<int>("id"),
                        yg_ordernum = reader.GetValue<string>("yg_ordernum"),
                        code = reader.GetValue<string>("code"),
                        orderId = reader.GetValue<int>("orderId"),
                        req_seq = reader.GetValue<string>("req_seq"),
                        resultcomment = reader.GetValue<string>("resultcomment"),
                        resultid = reader.GetValue<string>("resultid"),
                    };
                }
                return m;
            }
        }
    }
}

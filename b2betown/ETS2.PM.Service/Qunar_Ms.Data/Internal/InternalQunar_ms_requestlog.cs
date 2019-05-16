using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.Qunar_Ms.Model;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.Qunar_Ms.Data.Internal
{
    public class InternalQunar_ms_requestlog
    {
        public SqlHelper sqlHelper;
        public InternalQunar_ms_requestlog(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }
        internal int EditQunar_ms_requestlog(Qunar_ms_requestlog m)
        {
            try
            {
                if (m.id == 0)
                {
                    string sql = @"INSERT INTO  [qunar_ms_requestlog]
                               ([method]
                               ,[requestParam]
                               ,[base64data]
                               ,[securityType]
                               ,[signed]
                               ,[frombase64data]
                               ,[bodyType]
                               ,[createUser]
                               ,[supplierIdentity]
                               ,[createTime]
                               ,qunar_orderId
                               ,msg)
                         VALUES
                               (@method 
                               ,@requestParam
                               ,@base64data
                               ,@securityType
                               ,@signed
                               ,@frombase64data
                               ,@bodyType
                               ,@createUser
                               ,@supplierIdentity
                               ,@createTime
                               ,@qunar_orderId
                               ,@msg);select @@identity;";
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    cmd.AddParam("@method", m.method);
                    cmd.AddParam("@requestParam", m.requestParam);
                    cmd.AddParam("@base64data", m.base64data);
                    cmd.AddParam("@securityType", m.securityType);
                    cmd.AddParam("@signed", m.signed);
                    cmd.AddParam("@frombase64data", m.frombase64data);
                    cmd.AddParam("@bodyType", m.bodyType);
                    cmd.AddParam("@createUser", m.createUser);
                    cmd.AddParam("@supplierIdentity", m.supplierIdentity);
                    cmd.AddParam("@createTime", m.createTime);
                    cmd.AddParam("@qunar_orderId", m.qunar_orderId);
                    cmd.AddParam("@msg", m.msg);

                    object o = cmd.ExecuteScalar();
                    return int.Parse(o.ToString());
                }
                else
                {
                    string sql = @"UPDATE  [qunar_ms_requestlog]
                               SET [method] = @method
                                  ,[requestParam] = @requestParam 
                                  ,[base64data] = @base64data 
                                  ,[securityType] = @securityType 
                                  ,[signed] = @signed 
                                  ,[frombase64data] = @frombase64data 
                                  ,[bodyType] = @bodyType 
                                  ,[createUser] = @createUser 
                                  ,[supplierIdentity] = @supplierIdentity 
                                  ,[createTime] = @createTime 
                                  ,qunar_orderId=@qunar_orderId
                                  ,msg=@msg
                             WHERE id=@id";
                    var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                    cmd.AddParam("@id", m.id);
                    cmd.AddParam("@method", m.method);
                    cmd.AddParam("@requestParam", m.requestParam);
                    cmd.AddParam("@base64data", m.base64data);
                    cmd.AddParam("@securityType", m.securityType);
                    cmd.AddParam("@signed", m.signed);
                    cmd.AddParam("@frombase64data", m.frombase64data);
                    cmd.AddParam("@bodyType", m.bodyType);
                    cmd.AddParam("@createUser", m.createUser);
                    cmd.AddParam("@supplierIdentity", m.supplierIdentity);
                    cmd.AddParam("@createTime", m.createTime);
                    cmd.AddParam("@qunar_orderId", m.qunar_orderId);
                    cmd.AddParam("@msg", m.msg);

                    cmd.ExecuteNonQuery();
                    return m.id;
                }
            }
            catch 
            {
                return 0;
            }
        }

        internal bool IsHasNotice(string qunar_orderId, string method)
        {
            string sql = "select count(1) from qunar_ms_requestlog where qunar_orderId='" + qunar_orderId + "' and method='"+method+"'";
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

        internal bool IsHasSuc(string method, string qunar_orderid)
        {
            string sql = "select count(1) from qunar_ms_requestlog where qunar_orderId='" + qunar_orderid + "' and method='" + method + "' and msg='suc'";
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

        internal Qunar_ms_requestlog GetLog(int id)
        {
            string sql = @"SELECT [id]
      ,[method]
      ,[requestParam]
      ,[base64data]
      ,[securityType]
      ,[signed]
      ,[frombase64data]
      ,[bodyType]
      ,[createUser]
      ,[supplierIdentity]
      ,[createTime]
      ,[qunar_orderId]
      ,[msg]
  FROM  [qunar_ms_requestlog] where id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id",id);
            using(var reader=cmd.ExecuteReader())
            {
                Qunar_ms_requestlog m = null;
                if (reader.Read())
                {
                    m = new Qunar_ms_requestlog
                    {
                        id = reader.GetValue<int>("id"),
                        method = reader.GetValue<string>("method"),
                        requestParam = reader.GetValue<string>("requestParam"),
                        base64data = reader.GetValue<string>("base64data"),
                        securityType = reader.GetValue<string>("securityType"),
                        signed = reader.GetValue<string>("signed"),
                        frombase64data = reader.GetValue<string>("frombase64data"),
                        bodyType = reader.GetValue<string>("bodyType"),
                        createUser = reader.GetValue<string>("createUser"),
                        supplierIdentity = reader.GetValue<string>("supplierIdentity"),
                        createTime = reader.GetValue<DateTime>("createTime"),
                        qunar_orderId = reader.GetValue<string>("qunar_orderId"),
                        msg = reader.GetValue<string>("msg"),
                    };
                }
                return m;
            }
        }
    }
}

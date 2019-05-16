using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.Qunar_Ms.QMRequestDataSchema;

namespace ETS2.PM.Service.Qunar_Ms.Data.Internal
{
    public class InternalQunar_CreateOrderForBeforePaySyncvisitperson
    {
        public SqlHelper sqlHelper;
        public InternalQunar_CreateOrderForBeforePaySyncvisitperson(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int InsQunar_visitperson(CreateOrderForBeforePaySyncRequestBodyorderInfovisitPersonperson mperson, string qunar_orderId)
        {
            try
            {
                string sql = @"INSERT INTO  [qunar_CreateOrderForBeforePaySyncvisitperson]
           ([name]
           ,[namePinyin]
           ,[credentials]
           ,[credentialsType]
           ,[defined1Value]
           ,[defined2Value]
           ,[qunar_orderid])
     VALUES
           (@name 
           ,@namePinyin 
           ,@credentials 
           ,@credentialsType 
           ,@defined1Value 
           ,@defined2Value 
           ,@qunar_orderid);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@name", mperson.name);
                cmd.AddParam("@namePinyin", mperson.namePinyin);
                cmd.AddParam("@credentials", mperson.credentials);
                cmd.AddParam("@credentialsType", mperson.credentialsType);
                cmd.AddParam("@defined1Value", mperson.defined1Value);
                cmd.AddParam("@defined2Value", mperson.defined2Value);
                cmd.AddParam("@qunar_orderid", qunar_orderId);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
            catch
            {
                return 0;
            }
        }

        internal bool Ishasvisitperson(string qunar_orderId)
        {
            string sql = "select count(1) from qunar_CreateOrderForBeforePaySyncvisitperson where qunar_orderId =@qunar_orderid";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@qunar_orderid", qunar_orderId);

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
    }
}

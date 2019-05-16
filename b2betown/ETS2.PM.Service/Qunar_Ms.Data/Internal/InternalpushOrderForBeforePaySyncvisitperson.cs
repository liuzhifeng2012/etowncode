using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.Qunar_Ms.Data.Internal
{
    public class InternalpushOrderForBeforePaySyncvisitperson
    {
        public SqlHelper sqlHelper;
        public InternalpushOrderForBeforePaySyncvisitperson(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int InsRequest(string v_name, string v_namePinyin, string v_credentials, string v_credentialsType, string v_defined1Value, string v_defined2Value, int insrequest1)
        {
            try
            {
                string sql = @"INSERT INTO  [qunar_pushOrderForBeforePaySyncvisitperson]
           ([name]
           ,[namePinyin]
           ,[credentials]
           ,[credentialsType]
           ,[defined1Value]
           ,[defined2Value]
           ,[pushorderlog_id])
     VALUES
           (@name 
           ,@namePinyin 
           ,@credentials 
           ,@credentialsType 
           ,@defined1Value 
           ,@defined2Value 
           ,@pushorderlog_id);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@name", v_name);
                cmd.AddParam("@namePinyin", v_namePinyin);
                cmd.AddParam("@credentials", v_credentials);
                cmd.AddParam("@credentialsType", v_credentialsType);
                cmd.AddParam("@defined1Value", v_defined1Value);
                cmd.AddParam("@defined2Value", v_defined2Value);
                cmd.AddParam("@pushorderlog_id", insrequest1);
                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
            catch 
            {
                return 0;
            }
        }
    }
}

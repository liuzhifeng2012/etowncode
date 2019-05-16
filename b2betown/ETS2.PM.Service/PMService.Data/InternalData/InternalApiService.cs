using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalApiService
    {
        private SqlHelper sqlHelper;
        public InternalApiService(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal ApiService GetServiceDistribution(string servicename)
        {
            string sql = @"SELECT   [id]
                              ,[organization]
                              ,[password]
                              ,[deskey]
                              ,[servicername]
                              ,[remarks]
                              ,[isrun]
                          FROM  [api_service] where servicername=@servicename and isrun=1";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@servicename", servicename);

            using (var reader = cmd.ExecuteReader())
            {
                ApiService u = null;
                if (reader.Read())
                {
                    u = new ApiService
                    {
                        Id = reader.GetValue<int>("id"),
                        Organization = reader.GetValue<string>("organization"),
                        Password = reader.GetValue<string>("Password"),
                        Deskey = reader.GetValue<string>("Deskey"),
                        Servicername = reader.GetValue<string>("Servicername"),
                        Remarks = reader.GetValue<string>("Remarks"),
                        Isrun = reader.GetValue<bool>("isrun")
                    };
                }
                return u;
            }
        }

        internal ApiService GetApiservice(int serviceid)
        {
            string sql = @"SELECT   [id]
                  ,[organization]
                  ,[password]
                  ,[deskey]
                  ,[servicername]
                  ,[remarks]
                  ,[isrun]
              FROM   [api_service] where id=@id";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@id", serviceid);

            using (var reader = cmd.ExecuteReader())
            {
                ApiService u = null;
                if (reader.Read())
                {
                    u = new ApiService
                    {
                        Id = reader.GetValue<int>("id"),
                        Organization = reader.GetValue<string>("organization"),
                        Password = reader.GetValue<string>("Password"),
                        Deskey = reader.GetValue<string>("Deskey"),
                        Servicername = reader.GetValue<string>("Servicername"),
                        Remarks = reader.GetValue<string>("Remarks"),
                        Isrun = reader.GetValue<bool>("isrun")
                    };
                }
                return u;
            }
        }
    }
}

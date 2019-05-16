using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public class ApiServiceData
    {
        public ApiService GetServiceDistribution(string servicename)
        {
            using (var sql = new SqlHelper())
            {
                ApiService model = new InternalApiService(sql).GetServiceDistribution(servicename);
                return model;
            }
        }

        public ApiService GetApiservice(int serviceid)
        {
            using (var sql = new SqlHelper())
            {
                ApiService model = new InternalApiService(sql).GetApiservice(serviceid);
                return model;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data.InternalData;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.PMService.Data
{
    public class ApiAsynOrderData
    {
        public ApiAsynOrder GetAsynOrder(string platform_req_seq)
        {
            using (var sql = new SqlHelper())
            {
                ApiAsynOrder model = new InternalApiAsynOrder(sql).GetAsynOrder(platform_req_seq);
                return model;
            }
        }
        public int EditApiAsynOrder(int id, string req_seq, string platform_req_seq, string order_num, string num, string use_time, int serviceid,int issecondsend,int issuc,int logid)
        {
            using (var sql = new SqlHelper())
            {
                return new InternalApiAsynOrder(sql).EditApiAsynOrder(id, req_seq, platform_req_seq, order_num, num, use_time, serviceid,issecondsend,issuc,logid);
            }
        }

        public int GetTotalAsyncCount(string order_num, int serviceid)
        {
            using (var sql = new SqlHelper())
            {
                return new InternalApiAsynOrder(sql).GetTotalAsyncCount(  order_num,  serviceid);
            }
        }





        public int GetCorrectAsyncOrder(string platform_req_seq)
        {
            using (var sql = new SqlHelper())
            {
                return new InternalApiAsynOrder(sql).GetCorrectAsyncOrder(platform_req_seq);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalapi_hzins_OrderApplyReq_Application
    {
        public SqlHelper sqlHelper;
        public Internalapi_hzins_OrderApplyReq_Application(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int EditOrderApplyReq_Application(Api_hzins_OrderApplyReq_Application m)
        {
            //暂时只有录入保险请求，没有修改保险请求的操作
            if (m.id > 0)
            {
                return 0;
            }
            else
            {
                string sql = @"INSERT INTO  [api_hzins_OrderApplyReq_Application]
                                       ([applicationDate]
                                       ,[startDate]
                                       ,[endDate]
                                       ,[singlePrice]
                                       ,[orderid])
                                 VALUES
                                       (@applicationDate
                                       ,@startDate
                                       ,@endDate
                                       ,@singlePrice
                                       ,@orderid);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@applicationDate", m.applicationDate);
                cmd.AddParam("@startDate", m.startDate);
                cmd.AddParam("@endDate", m.endDate);
                cmd.AddParam("@singlePrice", m.singlePrice);
                cmd.AddParam("@orderid", m.orderid);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
        }

        internal Api_hzins_OrderApplyReq_Application GetOrderApplyReq_Application(int orderid)
        {
            string sql = "select * from api_hzins_OrderApplyReq_Application where orderid=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                Api_hzins_OrderApplyReq_Application m = null;
                if (reader.Read())
                {
                    m = new Api_hzins_OrderApplyReq_Application
                    {
                        id = reader.GetValue<int>("id"),
                        applicationDate = reader.GetValue<string>("applicationDate"),
                        startDate = reader.GetValue<string>("startDate"),
                        endDate = reader.GetValue<string>("endDate"),
                        singlePrice = reader.GetValue<decimal>("singlePrice"),
                        orderid = reader.GetValue<int>("orderid")
                    };
                }
                return m;
            }
        }

        
    }
}

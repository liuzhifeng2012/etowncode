using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalapi_hzins_OrderApplyResp_OrderExt
    {
        public SqlHelper sqlHelper;
        public Internalapi_hzins_OrderApplyResp_OrderExt(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }


        internal int EditOrderApplyResp_OrderExt(Api_hzins_OrderApplyResp_OrderExt m)
        {
            if (m.id > 0)
            {
                return 0;
            }
            else
            {
                string sql = @"INSERT INTO  [api_hzins_OrderApplyResp_OrderExt]
                                       ([orderid]
                                       ,[insureNum]
                                       ,[insurantIds]
                                       ,[priceTotal]
                                       ,[insurantCount])
                                 VALUES
                                       (@orderid 
                                       ,@insureNum 
                                       ,@insurantIds 
                                       ,@priceTotal 
                                       ,@insurantCount);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@orderid", m.orderid);
                cmd.AddParam("@insureNum", m.insureNum);
                cmd.AddParam("@insurantIds", m.insurantIds);
                cmd.AddParam("@priceTotal", m.priceTotal);
                cmd.AddParam("@insurantCount", m.insurantCount);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
        }

        internal int GetorderidbyinsureNum(string insureNum)
        {
            string sql = "select orderid from api_hzins_OrderApplyResp_OrderExt where insureNum='" + insureNum + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<int>("orderid");
                }
                return 0;
            }
        }

        internal string GetinsureNumbyorderid(int orderid)
        {
            string sql = "select insureNum  from api_hzins_OrderApplyResp_OrderExt where orderid='" + orderid + "'";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetValue<string>("insureNum");
                }
                return "";
            }
        }

        internal List<Api_hzins_OrderApplyResp_OrderExt> GetinsureNumsbyorderids(string orderidstr)
        {
            string sql = "select *  from api_hzins_OrderApplyResp_OrderExt where orderid in (" + orderidstr + ")";
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            List<Api_hzins_OrderApplyResp_OrderExt> list = new List<Api_hzins_OrderApplyResp_OrderExt>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Api_hzins_OrderApplyResp_OrderExt
                    {
                        id = reader.GetValue<int>("id"),
                        orderid = reader.GetValue<int>("orderid"),
                        insureNum = reader.GetValue<string>("insureNum"),
                        insurantIds = reader.GetValue<string>("insurantIds"),
                        priceTotal = reader.GetValue<decimal>("priceTotal"),
                        insurantCount = reader.GetValue<int>("insurantCount"),
                         
                    });
                }
                return list;
            }
        }
    }
}

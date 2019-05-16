using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalapi_hzins_OrderApplyReq_insurantInfo
    {
        public SqlHelper sqlHelper;
        public Internalapi_hzins_OrderApplyReq_insurantInfo(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int EditOrderApplyReq_insurantInfo(Api_hzins_OrderApplyReq_insurantInfo m)
        {
            //目前保险产品只有录入操作，没有编辑操作
            if (m.id > 0)
            {
                return 0;
            }
            else
            {
                string sql = @"INSERT INTO  [api_hzins_OrderApplyReq_insurantInfo]
                                   ([insurantId]
                                   ,[cName]
                                   ,[eName]
                                   ,[sex]
                                   ,[cardType]
                                   ,[cardCode]
                                   ,[birthday]
                                   ,[relationId]
                                   ,[count]
                                   ,[singlePrice]
                                   ,[fltNo]
                                   ,[fltDate]
                                   ,[city]
                                   ,[tripPurposeId]
                                   ,[destination]
                                   ,[visaCity]
                                   ,[jobInfo]
                                   ,[mobile]
                                   ,[orderid])
                             VALUES
                                   (@insurantId 
                                   ,@cName 
                                   ,@eName 
                                   ,@sex 
                                   ,@cardType 
                                   ,@cardCode 
                                   ,@birthday 
                                   ,@relationId 
                                   ,@count 
                                   ,@singlePrice 
                                   ,@fltNo 
                                   ,@fltDate 
                                   ,@city 
                                   ,@tripPurposeId 
                                   ,@destination 
                                   ,@visaCity 
                                   ,@jobInfo 
                                   ,@mobile 
                                   ,@orderid);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@insurantId", m.insurantId);
                cmd.AddParam("@cName", m.cName);
                cmd.AddParam("@eName", m.eName);
                cmd.AddParam("@sex", m.sex);
                cmd.AddParam("@cardType", m.cardType);
                cmd.AddParam("@cardCode", m.cardCode);
                cmd.AddParam("@birthday", m.birthday);
                cmd.AddParam("@relationId", m.relationId);
                cmd.AddParam("@count", m.count);
                cmd.AddParam("@singlePrice", m.singlePrice);
                cmd.AddParam("@fltNo", m.fltNo);
                cmd.AddParam("@fltDate", m.fltDate);
                cmd.AddParam("@city", m.city);
                cmd.AddParam("@tripPurposeId", m.tripPurposeId);
                cmd.AddParam("@visaCity", m.visaCity);
                cmd.AddParam("@destination", m.destination);
                cmd.AddParam("@jobInfo", m.jobInfo);
                cmd.AddParam("@mobile", m.mobile);
                cmd.AddParam("@orderid", m.orderid);
                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
        }

        internal List<Api_hzins_OrderApplyReq_insurantInfo> GetOrderApplyReq_insurantInfo(int orderid)
        {
            string sql = "select * from api_hzins_OrderApplyReq_insurantInfo where orderid=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                List<Api_hzins_OrderApplyReq_insurantInfo> list = new List<Api_hzins_OrderApplyReq_insurantInfo>();
                while (reader.Read())
                {
                    list.Add(new Api_hzins_OrderApplyReq_insurantInfo
                    {
                        id = reader.GetValue<int>("id"),
                        insurantId = reader.GetValue<string>("insurantId"),
                        cName = reader.GetValue<string>("cName"),
                        eName = reader.GetValue<string>("eName"),
                        sex = reader.GetValue<int>("sex"),
                        cardType = reader.GetValue<int>("cardType"),
                        cardCode = reader.GetValue<string>("cardCode"),
                        birthday = reader.GetValue<string>("birthday"),
                        relationId = reader.GetValue<int>("relationId"),
                        count = reader.GetValue<int>("count"),
                        singlePrice = reader.GetValue<decimal>("singlePrice"),
                        fltNo = reader.GetValue<string>("fltNo"),
                        fltDate = reader.GetValue<string>("fltDate"),
                        city = reader.GetValue<string>("city"),
                        tripPurposeId = reader.GetValue<int>("tripPurposeId"),
                        destination = reader.GetValue<string>("destination"),
                        visaCity = reader.GetValue<string>("visaCity"),
                        jobInfo = reader.GetValue<string>("jobInfo"),
                        mobile = reader.GetValue<string>("mobile"),
                        orderid = reader.GetValue<int>("orderid"),

                    });
                }
                return list;
            }
        }
    }
}

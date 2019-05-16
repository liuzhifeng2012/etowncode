using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalapi_mjld_SubmitOrder_input
    {
        public SqlHelper sqlHelper;
        public Internalapi_mjld_SubmitOrder_input(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int EditApi_mjld_SubmitOrder_input(Api_Mjld_SubmitOrder_input m)
        {
            //暂时只有录入操作，没有编辑操作
            if (m.id > 0)
            {
                return 0;
            }
            else
            {
                string sql = @"INSERT  [api_mjld_SubmitOrder_input]
           ([timeStamp]
           ,[user]
           ,[password]
           ,[goodsId]
           ,[num]
           ,[phone]
           ,[batch]
           ,[guest_name]
           ,[identityno]
           ,[order_note]
           ,[forecasttime]
           ,[consignee]
           ,[address]
           ,[zipcode]
           ,[orderId])
     VALUES
           (@timeStamp 
           ,@user 
           ,@password 
           ,@goodsId 
           ,@num 
           ,@phone 
           ,@batch 
           ,@guest_name 
           ,@identityno 
           ,@order_note 
           ,@forecasttime 
           ,@consignee 
           ,@address 
           ,@zipcode 
           ,@orderId);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@timeStamp", m.timeStamp);
                cmd.AddParam("@user", m.user);
                cmd.AddParam("@password", m.password);
                cmd.AddParam("@goodsId", m.goodsId);
                cmd.AddParam("@num", m.num);
                cmd.AddParam("@phone", m.phone);
                cmd.AddParam("@batch", m.batch);
                cmd.AddParam("@guest_name", m.guest_name);
                cmd.AddParam("@identityno", m.identityno);
                cmd.AddParam("@order_note", m.order_note);
                cmd.AddParam("@forecasttime", m.forecasttime);
                cmd.AddParam("@consignee", m.consignee);
                cmd.AddParam("@address", m.address);
                cmd.AddParam("@zipcode", m.zipcode);
                cmd.AddParam("@orderId", m.orderId);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
        }

        internal Api_Mjld_SubmitOrder_input GetApi_Mjld_SubmitOrder_input(int orderid)
        {
            string sql = @"SELECT [id]
                          ,[timeStamp]
                          ,[user]
                          ,[password]
                          ,[goodsId]
                          ,[num]
                          ,[phone]
                          ,[batch]
                          ,[guest_name]
                          ,[identityno]
                          ,[order_note]
                          ,[forecasttime]
                          ,[consignee]
                          ,[address]
                          ,[zipcode]
                          ,[orderId]
                      FROM  [api_mjld_SubmitOrder_input] where orderid=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {

                Api_Mjld_SubmitOrder_input m = null;
                if (reader.Read())
                {
                    m = new Api_Mjld_SubmitOrder_input
                    {
                        id = reader.GetValue<int>("id"),
                        timeStamp = reader.GetValue<string>("timeStamp"),
                        user = reader.GetValue<string>("user"),
                        password = reader.GetValue<string>("password"),
                        goodsId = reader.GetValue<string>("goodsId"),
                        num = reader.GetValue<string>("num"),
                        phone = reader.GetValue<string>("phone"),
                        batch = reader.GetValue<string>("batch"),
                        guest_name = reader.GetValue<string>("guest_name"),
                        identityno = reader.GetValue<string>("identityno"),
                        order_note = reader.GetValue<string>("order_note"),
                        forecasttime = reader.GetValue<string>("forecasttime"),
                        consignee = reader.GetValue<string>("consignee"),
                        address = reader.GetValue<string>("address"),
                        zipcode = reader.GetValue<string>("zipcode"),
                        orderId = reader.GetValue<int>("orderId"),
                    };
                }
                return m;
            }
        }
    }
}

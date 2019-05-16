using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class Internalapi_yg_addorder_input
    {
        public SqlHelper sqlHelper;
        public Internalapi_yg_addorder_input(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }


        internal int EditApi_yg_addorder_input(Api_yg_addorder_input m)
        {
            if (m.id > 0)
            {
                string sql = @"INSERT [api_yg_addorder_input]
                                   ([organization]
                                   ,[password]
                                   ,[req_seq]
                                   ,[product_num]
                                   ,[num]
                                   ,[mobile]
                                   ,[use_date]
                                   ,[real_name_type]
                                   ,[real_name]
                                   ,[id_card]
                                   ,[card_type]
                                   ,[orderId])
                             VALUES
                                   (@organization
                                   ,@password 
                                   ,@req_seq 
                                   ,@product_num 
                                   ,@num
                                   ,@mobile 
                                   ,@use_date 
                                   ,@real_name_type 
                                   ,@real_name 
                                   ,@id_card 
                                   ,@card_type 
                                   ,@orderId);select @@identity;";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@organization", m.organization);
                cmd.AddParam("@password", m.password);
                cmd.AddParam("@req_seq", m.req_seq);
                cmd.AddParam("@product_num", m.product_num);
                cmd.AddParam("@num", m.num);
                cmd.AddParam("@mobile", m.mobile);
                cmd.AddParam("@use_date", m.use_date);
                cmd.AddParam("@real_name_type", m.real_name_type);
                cmd.AddParam("@real_name", m.real_name);
                cmd.AddParam("@id_card", m.id_card);
                cmd.AddParam("@card_type", m.card_type);
                cmd.AddParam("@orderId", m.orderId);

                object o = cmd.ExecuteScalar();
                return int.Parse(o.ToString());
            }
            else
            {
                string sql = @"UPDATE  [api_yg_addorder_input]
                               SET [organization] = @organization 
                                  ,[password] = @password 
                                  ,[req_seq] = @req_seq 
                                  ,[product_num] = @product_num 
                                  ,[num] = @num 
                                  ,[mobile] = @mobile 
                                  ,[use_date] = @use_date 
                                  ,[real_name_type] = @real_name_type 
                                  ,[real_name] =@real_name 
                                  ,[id_card] = @id_card 
                                  ,[card_type] = @card_type 
                                  ,[orderId] = @orderId 
                             WHERE id=@id";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", m.id);
                cmd.AddParam("@organization", m.organization);
                cmd.AddParam("@password", m.password);
                cmd.AddParam("@req_seq", m.req_seq);
                cmd.AddParam("@product_num", m.product_num);
                cmd.AddParam("@num", m.num);
                cmd.AddParam("@mobile", m.mobile);
                cmd.AddParam("@use_date", m.use_date);
                cmd.AddParam("@real_name_type", m.real_name_type);
                cmd.AddParam("@real_name", m.real_name);
                cmd.AddParam("@id_card", m.id_card);
                cmd.AddParam("@card_type", m.card_type);
                cmd.AddParam("@orderId", m.orderId);

                cmd.ExecuteNonQuery();
                return m.id;
            }
        }

        internal Api_yg_addorder_input Getapi_yg_addorder_input(int orderid)
        {
            string sql = "select * from api_yg_addorder_input where orderid=" + orderid;
            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            using (var reader = cmd.ExecuteReader())
            {
                Api_yg_addorder_input m = null;
                if (reader.Read())
                {
                    m = new Api_yg_addorder_input
                    {
                        id = reader.GetValue<int>("id"),
                        organization = reader.GetValue<string>("organization"),
                        password = reader.GetValue<string>("password"),
                        req_seq = reader.GetValue<string>("req_seq"),
                        product_num = reader.GetValue<string>("product_num"),
                        num = reader.GetValue<int>("num"),
                        mobile = reader.GetValue<string>("mobile"),
                        use_date = reader.GetValue<string>("use_date"),
                        real_name_type = reader.GetValue<int>("real_name_type"),
                        real_name = reader.GetValue<string>("real_name"),
                        id_card = reader.GetValue<string>("id_card"),
                        card_type = reader.GetValue<int>("card_type"),
                        orderId = reader.GetValue<int>("orderId"),
                    };
                }
                return m;
            }
        }
    }
}

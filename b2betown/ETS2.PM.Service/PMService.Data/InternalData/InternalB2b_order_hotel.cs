using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Modle;

namespace ETS2.PM.Service.PMService.Data.InternalData
{
    public class InternalB2b_order_hotel
    {
        private SqlHelper sqlHelper;
        public InternalB2b_order_hotel(SqlHelper sqlHelper)
        {
            this.sqlHelper = sqlHelper;
        }

        internal int InsertOrUpdate(B2b_order_hotel m_orderhotel)
        {
            if (m_orderhotel.Id == 0)//增加酒店订单
            {
                string sql = @"INSERT INTO [EtownDB].[dbo].[b2b_order_hotel]
           ([orderid]
           ,[start_date]
           ,[end_date]
           ,[bookdaynum]
           ,[lastarrivaltime]
           ,[fangtai])
     VALUES
           (@orderid,@start_date,@end_date,@bookdaynum,@lastarrivaltime,@fangtai)";
                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@orderid", m_orderhotel.Orderid);
                cmd.AddParam("@start_date", m_orderhotel.Start_date);
                cmd.AddParam("@end_date", m_orderhotel.End_date);
                cmd.AddParam("@bookdaynum", m_orderhotel.Bookdaynum);
                cmd.AddParam("@lastarrivaltime", m_orderhotel.Lastarrivaltime);
                cmd.AddParam("@fangtai", m_orderhotel.Fangtai);

                return cmd.ExecuteNonQuery();
            }
            else //编辑酒店订单
            {
                string sql = @"UPDATE [EtownDB].[dbo].[b2b_order_hotel]
   SET [orderid] =  @orderid
      ,[start_date] =  @start_date
      ,[end_date] =  @end_date
      ,[bookdaynum] =@bookdaynum
      ,[lastarrivaltime] = @lastarrivaltime
      ,[fangtai] =@fangtai
 WHERE  id=@id";

                var cmd = sqlHelper.PrepareTextSqlCommand(sql);
                cmd.AddParam("@id", m_orderhotel.Id);
                cmd.AddParam("@orderid", m_orderhotel.Orderid);
                cmd.AddParam("@start_date", m_orderhotel.Start_date);
                cmd.AddParam("@end_date", m_orderhotel.End_date);
                cmd.AddParam("@bookdaynum", m_orderhotel.Bookdaynum);
                cmd.AddParam("@lastarrivaltime", m_orderhotel.Lastarrivaltime);
                cmd.AddParam("@fangtai", m_orderhotel.Fangtai);

                return cmd.ExecuteNonQuery();
            }
        }

        internal B2b_order_hotel GetHotelOrderByOrderId(int orderid)
        {
            string sql = @"SELECT  [id]
      ,[orderid]
      ,[start_date]
      ,[end_date]
      ,[bookdaynum]
      ,[lastarrivaltime]
      ,[fangtai]
  FROM [EtownDB].[dbo].[b2b_order_hotel] where orderid=@orderid";

            var cmd = sqlHelper.PrepareTextSqlCommand(sql);
            cmd.AddParam("@orderid", orderid);

            using (var reader = cmd.ExecuteReader())
            {
                B2b_order_hotel m = null;
                if (reader.Read())
                {
                    m = new B2b_order_hotel
                    {
                        Id=reader.GetValue<int>("id"),
                         Orderid=reader.GetValue<int>("orderid"),
                          Start_date=reader.GetValue<DateTime>("start_date"),
                          End_date=reader.GetValue<DateTime>("end_date"),
                          Bookdaynum=reader.GetValue<int>("bookdaynum"),
                           Lastarrivaltime=reader.GetValue<string>("lastarrivaltime"),
                            Fangtai=reader.GetValue<string>("fangtai")
                    };

                }
                return m;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.PM.Service.PMService.Modle;
using ETS.Data.SqlHelper;
using ETS2.PM.Service.PMService.Data.InternalData;

namespace ETS2.PM.Service.PMService.Data
{
    public  class B2b_order_hotelData
    {
        /*订单扩展表:酒店订单表*/
        public int InsertOrUpdate(B2b_order_hotel m_orderhotel)
        {
             using (var helper=new SqlHelper())
             {
                 int r = new InternalB2b_order_hotel(helper).InsertOrUpdate(m_orderhotel);
                    return r;
             }
        }
        /*得到订单扩展表(酒店订单表)*/
        public B2b_order_hotel GetHotelOrderByOrderId(int orderid)
        {
           using(var helper=new SqlHelper())
           {
               B2b_order_hotel m = new InternalB2b_order_hotel(helper).GetHotelOrderByOrderId(orderid);
               return m;
           }
        }
    }
}

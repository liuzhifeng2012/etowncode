using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
     [Serializable()]
    public class B2b_order_hotel
    {
         private int id;
         private int orderid;
         private DateTime start_date;
         private DateTime end_date;
         private int bookdaynum;
         private string lastarrivaltime;
         private string fangtai;

         public B2b_order_hotel() { }

         public int Id {
             get { return this.id; }
             set { this.id = value; }
         }
         public int Orderid
         {
             get { return this.orderid; }
             set { this.orderid = value; }
         }
         public DateTime Start_date
         {
             get { return this.start_date; }
             set { this.start_date = value; }
         }
         public DateTime  End_date
         {
             get { return this.end_date; }
             set { this.end_date = value; }
         }
         public int Bookdaynum
         {
             get { return this.bookdaynum; }
             set { this.bookdaynum = value; }
         }
         public string  Lastarrivaltime
         {
             get { return this.lastarrivaltime; }
             set { this.lastarrivaltime = value; }
         }
         public string Fangtai
         {
             get { return this.fangtai; }
             set { this.fangtai = value; }
         }
       
    }
}

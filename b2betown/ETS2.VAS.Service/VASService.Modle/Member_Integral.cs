using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.VAS.Service.VASService.Modle
{
     [Serializable()]
    public class Member_Integral
    {
         private int id;
         private int comid;
         private string acttype;
         private decimal money;

         private int mid;
         private int oid;
         private int ptype;
         private string remark;
         private string admin;
         private string ip;
         private DateTime subdate;



         public Member_Integral() { }

         private decimal orderId;

         public decimal OrderId
         {
             get { return orderId; }
             set { orderId = value; }
         }
         private string orderName;

         public string OrderName
         {
             get { return orderName; }
             set { orderName = value; }
         }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public int Comid
        {
            get { return this.comid; }
            set { this.comid = value; }
        }

        public string Acttype
        {
            get { return this.acttype; }
            set { this.acttype = value; }
        }

        public decimal Money
        {
            get { return this.money; }
            set { this.money = value; }
        }

        public int Mid
        {
            get { return this.mid; }
            set { this.mid = value; }
        }

        public int Oid
        {
            get { return this.oid; }
            set { this.oid = value; }
        }

        public int Ptype
        {
            get { return this.ptype; }
            set { this.ptype = value; }
        }


        public string Remark
        {
            get { return this.remark; }
            set { this.remark = value; }
        }

        public string Admin
        {
            get { return this.admin; }
            set { this.admin = value; }
        }
        public string Ip
        {
            get { return this.ip; }
            set { this.ip = value; }
        }
        public DateTime Subdate
        {
            get { return this.subdate; }
            set { this.subdate = value; }
        }

    }
}

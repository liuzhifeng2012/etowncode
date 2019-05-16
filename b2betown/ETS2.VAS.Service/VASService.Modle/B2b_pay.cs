using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Data.SqlHelper;

namespace ETS2.VAS.Service.VASService.Modle
{
    [Serializable()]
    public class B2b_pay
    {

        private int id;
        private int oid;
        private string pay_com;
        private string pay_name;
        private string pay_phone;
        private decimal total_fee;
        private string trade_no;
        private string trade_status;
        private string uip;

        private string wxtransaction_id;//微信支付返回的交易号

        public DateTime subtime { get; set; }//支付时间
        public int comid { get; set; }//收款方公司id

        public B2b_pay() { }

        public string Wxtransaction_id 
        {
            get { return this.wxtransaction_id; }
            set { this.wxtransaction_id = value; }
        }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public int Oid
        {
            get { return this.oid; }
            set { this.oid = value; }
        }

        public string Pay_com
        {
            get { return this.pay_com; }
            set { this.pay_com = value; }
        }

        public string Pay_name
        {
            get { return this.pay_name; }
            set { this.pay_name = value; }
        }

        public string Pay_phone
        {
            get { return this.pay_phone; }
            set { this.pay_phone = value; }
        }

        public decimal Total_fee
        {
            get { return this.total_fee; }
            set { this.total_fee = value; }
        }

        public string Trade_no
        {
            get { return this.trade_no; }
            set { this.trade_no = value; }
        }

        public string Trade_status
        {
            get { return this.trade_status; }
            set { this.trade_status = value; }
        }

        public string Uip
        {
            get { return this.uip; }
            set { this.uip = value; }
        }

    }
}

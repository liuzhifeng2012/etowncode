using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.VAS.Service.VASService.Modle
{
    [Serializable()]
    public class B2b_Finance
    {

        private int id;
        private int com_id;
        private int agent_id;
        private int eid;
        private int order_id;
        private string servicesname;
        private string serialnumber;
        private decimal money;
        private string money_come;
        private decimal over_money;
        private int payment;
        private string payment_type;
        private DateTime subdate;
        private int paytype;
        private string bank_account;
        private string bank_card;
        private string bank_name;
        private string alipay_account;
        private string alipay_id;
        private string alipay_key;
        private string userbank_account;
        private string userbank_card;
        private string userbank_name;
        private int act;
        private int con_state;
        private string remarks;
        private int printscreen;

        private int channelid;
        private int paychannelstate;



        public B2b_Finance() { }

        private int uptype;

        public int Uptype
        {
            get { return uptype; }
            set { uptype = value; }
        }

        public int Printscreen
        {
            get { return this.printscreen; }
            set { this.printscreen = value; }
        }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public int Act
        {
            get { return this.act; }
            set { this.act = value; }
        }
        public int Com_id
        {
            get { return this.com_id; }
            set { this.com_id = value; }
        }
        public int Agent_id
        {
            get { return this.agent_id; }
            set { this.agent_id = value; }
        }
        public int Eid
        {
            get { return this.eid; }
            set { this.eid = value; }
        }
        public int Order_id
        {
            get { return this.order_id; }
            set { this.order_id = value; }
        }
        public string SerialNumber
        {
            get { return this.serialnumber; }
            set { this.serialnumber = value; }
        }
        public decimal Money
        {
            get { return this.money; }
            set { this.money = value; }
        }

        public string Servicesname
        {
            get { return this.servicesname; }
            set { this.servicesname = value; }
        }

        public string Money_come
        {
            get { return this.money_come; }
            set { this.money_come = value; }
        }
        public decimal Over_money
        {
            get { return this.over_money; }
            set { this.over_money = value; }
        }
        public int Payment
        {
            get { return this.payment; }
            set { this.payment = value; }
        }

        public string Payment_type
        {
            get { return this.payment_type; }
            set { this.payment_type = value; }
        }

        public DateTime Subdate
        {
            get { return this.subdate; }
            set { this.subdate = value; }
        }

        public int Paytype
        {
            get { return this.paytype; }
            set { this.paytype = value; }
        }

        public string Bank_account
        {
            get { return this.bank_account; }
            set { this.bank_account = value; }
        }
        public string Bank_card
        {
            get { return this.bank_card; }
            set { this.bank_card = value; }
        }
        public string Bank_name
        {
            get { return this.bank_name; }
            set { this.bank_name = value; }
        }

        public string Userbank_account
        {
            get { return this.userbank_account; }
            set { this.userbank_account = value; }
        }
        public string Userbank_card
        {
            get { return this.userbank_card; }
            set { this.userbank_card = value; }
        }
        public string Userbank_name
        {
            get { return this.userbank_name; }
            set { this.userbank_name = value; }
        }
        public string Alipay_account
        {
            get { return this.alipay_account; }
            set { this.alipay_account = value; }
        }
        public string Alipay_id
        {
            get { return this.alipay_id; }
            set { this.alipay_id = value; }
        }
        public string Alipay_key
        {
            get { return this.alipay_key; }
            set { this.alipay_key = value; }
        }
        public int Con_state
        {
            get { return this.con_state; }
            set { this.con_state = value; }
        }
        public string Remarks
        {
            get { return this.remarks; }
            set { this.remarks = value; }
        }
        public int Channelid
        {
            get { return this.channelid; }
            set { this.channelid = value; }
        }
        public int Paychannelstate
        {
            get { return this.paychannelstate; }
            set { this.paychannelstate = value; }
        }


    }
}

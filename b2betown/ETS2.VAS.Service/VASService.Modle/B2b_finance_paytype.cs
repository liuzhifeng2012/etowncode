using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.VAS.Service.VASService.Modle
{
    public class B2b_finance_paytype
    {
        private int id;
        private int com_id;
        private int paytype;
        private string  bank_account;
        private string bank_card;
        private string bank_name;
        private string alipay_account;
        private string alipay_id;
        private string alipay_key;
        private string userbank_account;
        private string userbank_card;
        private string userbank_name;
        private int uptype;
        private string wx_appid;
        private string wx_appkey;
        private string wx_paysignkey;
        private string wx_partnerid;
        private string wx_partnerkey;

        private string tenpay_id;
        private string tenpay_key;

        public string wx_SSLCERT_PATH { get; set; }
        public string wx_SSLCERT_PASSWORD { get; set; }
      
        public B2b_finance_paytype() { }


        public string Tenpay_id
        {
            get { return tenpay_id; }
            set { tenpay_id = value; }
        }
        public string Tenpay_key
        {
            get { return tenpay_key; }
            set { tenpay_key = value; }
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Com_id
        {
            get { return this.com_id; }
            set { this.com_id = value; }
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
        public int Uptype
        {
            get { return this.uptype; }
            set { this.uptype = value; }
        }
        public string Wx_appid
        {
            get { return this.wx_appid; }
            set { this.wx_appid = value; }
        }

        public string Wx_appkey
        {
            get { return this.wx_appkey; }
            set { this.wx_appkey = value; }
        }

        public string Wx_paysignkey
        {
            get { return this.wx_paysignkey; }
            set { this.wx_paysignkey = value; }
        }

        public string Wx_partnerid
        {
            get { return this.wx_partnerid; }
            set { this.wx_partnerid = value; }
        }

        public string Wx_partnerkey
        {
            get { return this.wx_partnerkey; }
            set { this.wx_partnerkey = value; }
        } 
    }
}

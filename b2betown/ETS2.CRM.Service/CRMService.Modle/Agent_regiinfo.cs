using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{   /// <summary>
    /// 分销商基本信息表
    /// </summary>
    [Serializable()]
    public class  Agent_regiinfo
    {
        private int id;
        private string account;
        private string pwd;
        private string contentname;
        private string tel;
        private string mobile;
        private string company;
        private string address;
        private int agentid;
        private decimal amount;
        private int accounttype;
        private int accountLevel;

        private string com_province;
        private string com_city;
        private int agent_sourcesort;

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string Com_province
        {
            get { return this.com_province; }
            set { this.com_province = value; }
        }
        public string Com_city
        {
            get { return this.com_city; }
            set { this.com_city = value; }
        }
        public int Agent_sourcesort
        {
            get { return this.agent_sourcesort; }
            set { this.agent_sourcesort = value; }
        }




        public string Account
        {
            get { return this.account; }
            set { this.account = value; }
        }
        public string Pwd
        {
            get { return this.pwd; }
            set { this.pwd = value; }
        }
        public string Contentname
        {
            get { return this.contentname; }
            set { this.contentname = value; }
        }
        public string Tel
        {
            get { return this.tel; }
            set { this.tel = value; }
        }
        public string Mobile
        {
            get { return this.mobile; }
            set { this.mobile = value; }
        }
        public string Company
        {
            get { return this.company; }
            set { this.company = value; }
        }
        public string Address
        {
            get { return this.address; }
            set { this.address = value; }
        }
        public int Agentid
        {
            get { return this.agentid; }
            set { this.agentid = value; }
        }
        public decimal Amount
        {
            get { return this.amount; }
            set { this.amount = value; }
        }
        public int Accounttype
        {
            get { return this.accounttype; }
            set { this.accounttype = value; }
        }

        public int AccountLevel
        {
            get { return this.accountLevel; }
            set { this.accountLevel = value; }
        }
    }
}

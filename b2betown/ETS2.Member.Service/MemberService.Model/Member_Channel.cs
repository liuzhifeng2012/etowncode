using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Member.Service.MemberService.Model
{
    [Serializable()]
    public class Member_Channel
    {
        public decimal Rebatemoney { get; set; }
        private int id;
        private int com_id;
        private int issuetype;
        private int companyid;
        private string name = String.Empty;
        private string mobile = String.Empty;
        private decimal cardcode;
        private string chaddress = String.Empty;
        private string chObjects = String.Empty;
        private decimal rebateOpen;
        private decimal rebateConsume;
        private decimal rebateConsume2;
        private int rebatelevel;

        private int opencardnum;
        private int firstdealnum;
        private decimal summoney;

        private int companynum;

        private string lockuserweixin;
        private DateTime lockusertime;
        private int lockuser = 0;


        public int Companynum
        {
            get { return this.companynum; }
            set { this.companynum = value; }
        }

        private int runstate;
        public int Runstate
        {
            get { return this.runstate; }
            set { this.runstate = value; }
        }

        private string channeltypename = String.Empty;



        public Member_Channel() { }

        public string Channeltypename
        {
            get { return this.channeltypename; }
            set { this.channeltypename = value; }
        }

        public int Opencardnum
        {
            get { return this.opencardnum; }
            set { this.opencardnum = value; }
        }

        private int companystate = 1;
        public int Companystate
        {
            get { return this.companystate; }
            set { this.companystate = value; }
        }

        private int whetherdefaultchannel = 0;//默认0(非默认渠道)
        public int Whetherdefaultchannel
        {
            get { return this.whetherdefaultchannel; }
            set { this.whetherdefaultchannel = value; }
        }
        public int Firstdealnum
        {
            get { return this.firstdealnum; }
            set { this.firstdealnum = value; }
        }
        public decimal Summoney
        {
            get { return this.summoney; }
            set { this.summoney = value; }
        }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public int RebateLevel
        {
            get { return this.rebatelevel; }
            set { this.rebatelevel = value; }
        }



        public int Com_id
        {
            get { return this.com_id; }
            set { this.com_id = value; }
        }


        public int Issuetype
        {
            get { return this.issuetype; }
            set { this.issuetype = value; }
        }


        public int Companyid
        {
            get { return this.companyid; }
            set { this.companyid = value; }
        }


        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }


        public string Mobile
        {
            get { return this.mobile; }
            set { this.mobile = value; }
        }


        public decimal Cardcode
        {
            get { return this.cardcode; }
            set { this.cardcode = value; }
        }


        public string Chaddress
        {
            get { return this.chaddress; }
            set { this.chaddress = value; }
        }


        public string ChObjects
        {
            get { return this.chObjects; }
            set { this.chObjects = value; }
        }


        public decimal RebateOpen
        {
            get { return this.rebateOpen; }
            set { this.rebateOpen = value; }
        }


        public decimal RebateConsume
        {
            get { return this.rebateConsume; }
            set { this.rebateConsume = value; }
        }


        public decimal RebateConsume2
        {
            get { return this.rebateConsume2; }
            set { this.rebateConsume2 = value; }
        }

        public DateTime Lockusertime
        {
            get { return this.lockusertime; }
            set { this.lockusertime = value; }
        }

        public string Lockuserweixin
        {
            get { return this.lockuserweixin; }
            set { this.lockuserweixin = value; }
        }
        public int Lockuser
        {
            get { return this.lockuser; }
            set { this.lockuser = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Member.Service.MemberService.Model
{
    /// <summary>
    /// 卡号管理
    /// </summary>
    [Serializable()]
    public class Member_Card
    {
        private int id;
        private int com_id;
        private string cname = String.Empty;
        private int ctype;
        private int printnum;
        private int zhuanzeng;
        private int qrcode;
        private string exchange;
        private string remark;
        private int cardRule;
        private int cardRule_starnum;
        private int cardRule_First;
        private int cardRule_Second;
        private DateTime subdate;
        private bool outstate;
        private bool createstate;
        private int crid;

        private int openstate;
        private DateTime opensubdate;

        private decimal cardcode;
        private decimal issueCard;
        private int issueId;

        //wcl
        private decimal serverCard;

        public decimal ServerCard
        {
            get { return serverCard; }
            set { serverCard = value; }
        }

        public Member_Card() { }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public int IssueId
        {
            get { return this.issueId; }
            set { this.issueId = value; }
        }
        public int Com_id
        {
            get { return this.com_id; }
            set { this.com_id = value; }
        }


        public string Cname
        {
            get { return this.cname; }
            set { this.cname = value; }
        }

        public int Ctype
        {
            get { return this.ctype; }
            set { this.ctype = value; }
        }

        public int Printnum
        {
            get { return this.printnum; }
            set { this.printnum = value; }
        }

        public int Zhuanzeng
        {
            get { return this.zhuanzeng; }
            set { this.zhuanzeng = value; }
        }
        public int Qrcode
        {
            get { return this.qrcode; }
            set { this.qrcode = value; }
        }
        public string Exchange
        {
            get { return this.exchange; }
            set { this.exchange = value; }
        }
        public string Remark
        {
            get { return this.remark; }
            set { this.remark = value; }
        }
        public int CardRule
        {
            get { return this.cardRule; }
            set { this.cardRule = value; }
        }
        public int CardRule_starnum
        {
            get { return this.cardRule_starnum; }
            set { this.cardRule_starnum = value; }
        }

        public int CardRule_First
        {
            get { return this.cardRule_First; }
            set { this.cardRule_First = value; }
        }
        public int CardRule_Second
        {
            get { return this.cardRule_Second; }
            set { this.cardRule_Second = value; }
        }


        public DateTime Subdate
        {
            get { return this.subdate; }
            set { this.subdate = value; }
        }

        public bool Outstate
        {
            get { return this.outstate; }
            set { this.outstate = value; }
        }
        public bool Createstate
        {
            get { return this.createstate; }
            set { this.createstate = value; }
        }

        public int Crid
        {
            get { return this.crid; }
            set { this.crid = value; }
        }

        public decimal Cardcode
        {
            get { return this.cardcode; }
            set { this.cardcode = value; }
        }

        public int Openstate
        {
            get { return this.openstate; }
            set { this.openstate = value; }
        }
        public DateTime Opensubdate
        {
            get { return this.opensubdate; }
            set { this.opensubdate = value; }
        }
        public decimal IssueCard
        {
            get { return this.issueCard; }
            set { this.issueCard = value; }
        }

    }
}

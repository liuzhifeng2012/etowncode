using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    public class B2b_invitecodesendlog
    {
        private int id;
        private string phone;
        private string smscontent;
        private string invitecode;
        private int senduserid;
        private DateTime sendtime;
        private int issendsuc;
        private int isqunfa;
        private string remark;
        private int  comid;

        public B2b_invitecodesendlog() { }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public string Phone
        {
            get { return this.phone; }
            set { this.phone = value; }
        }
        public string Smscontent
        {
            get { return this.smscontent; }
            set { this.smscontent = value; }
        }
        public string Invitecode
        {
            get { return this.invitecode; }
            set { this.invitecode = value; }
        }
        public int Senduserid
        {
            get { return this.senduserid; }
            set { this.senduserid = value; }
        }
        public DateTime Sendtime
        {
            get { return this.sendtime; }
            set { this.sendtime = value; }
        }
        public int Issendsuc
        {
            get { return this.issendsuc; }
            set { this.issendsuc = value; }
        }
        public int Isqunfa
        {
            get { return this.isqunfa; }
            set { this.isqunfa = value; }
        }
        public string Remark
        {
            get { return this.remark; }
            set { this.remark = value; }
        }
        public int Comid
        {
            get { return this.comid; }
            set { this.comid = value; }
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    [Serializable()]
    public class B2b_smsmobilesend
    {

        private int id;
        private string mobile;
        private string content = String.Empty;
        private string pno = String.Empty;
        private string delaysendtime = String.Empty;
        private string text = String.Empty;
        private int flag;
        private int smsid;
        private int oid;
        private int sendnum;
        private int sendeticketid;
        private DateTime realsendtime;


        public B2b_smsmobilesend() { }


        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public int Flag
        {
            get { return this.flag; }
            set { this.flag = value; }
        }
        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }
        public string Pno
        {
            get { return this.pno; }
            set { this.pno = value; }
        }
        public int Smsid
        {
            get { return this.smsid; }
            set { this.smsid = value; }
        }
        public int Oid
        {
            get { return this.oid; }
            set { this.oid = value; }
        }
        public int Sendnum
        {
            get { return this.sendnum; }
            set { this.sendnum = value; }
        }
        public int Sendeticketid
        {
            get { return this.sendeticketid; }
            set { this.sendeticketid = value; }
        }

        public string Mobile
        {
            get { return this.mobile; }
            set { this.mobile = value; }
        }

        public string Content
        {
            get { return this.content; }
            set { this.content = value; }
        }
        public string Delaysendtime
        {
            get { return this.delaysendtime; }
            set { this.delaysendtime = value; }
        }
        public DateTime Realsendtime
        {
            get { return this.realsendtime; }
            set { this.realsendtime = value; }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    [Serializable()]
    public class ObtainGzListSplit
    {
        private int id;
        private int total;
        private int splitcount;
        private string splitopenid = "";
        private int obtainno;
        private DateTime splittime;
        private int splitno;
        private int issuc;
        private int comid;
        private int dealerid;


        public ObtainGzListSplit() { }


        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Total
        {
            get { return this.total; }
            set { this.total = value; }
        }

        public int Splitcount
        {
            get { return this.splitcount; }
            set { this.splitcount = value; }
        }

        public string Splitopenid
        {
            get { return this.splitopenid; }
            set { this.splitopenid = value; }
        }

         
        public int Obtainno
        {
            get { return this.obtainno; }
            set { this.obtainno = value; }
        }


        public DateTime Splittime
        {
            get { return this.splittime; }
            set { this.splittime = value; }
        }


        public int Splitno
        {
            get { return this.splitno; }
            set { this.splitno = value; }
        }


        public int Issuc
        {
            get { return this.issuc; }
            set { this.issuc = value; }
        }


        public int Comid
        {
            get { return this.comid; }
            set { this.comid = value; }
        }


        public int Dealerid
        {
            get { return this.dealerid; }
            set { this.dealerid = value; }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    [Serializable()]
    public class ObtainGzListLog
    {
        private int id;
        private int total;
        private int count;
        private string openid = "";
        private string nextopenid = "";
        private int obtainno;
        private DateTime obtaintime;
        private string errcode = "";
        private string errmsg = "";
        private int comid;
        private int dealerid;


        public ObtainGzListLog() { }


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

        public int Count
        {
            get { return this.count; }
            set { this.count = value; }
        }

        public string Openid
        {
            get { return this.openid; }
            set { this.openid = value; }
        }

        public string Nextopenid
        {
            get { return this.nextopenid; }
            set { this.nextopenid = value; }
        }
        public int Obtainno
        {
            get { return this.obtainno; }
            set { this.obtainno = value; }
        }


        public DateTime Obtaintime
        {
            get { return this.obtaintime; }
            set { this.obtaintime = value; }
        }


        public string Errcode
        {
            get { return this.errcode; }
            set { this.errcode = value; }
        }


        public string Errmsg
        {
            get { return this.errmsg; }
            set { this.errmsg = value; }
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

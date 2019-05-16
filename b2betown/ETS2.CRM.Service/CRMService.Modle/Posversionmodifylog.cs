using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{

    [Serializable()]
    public class Posversionmodifylog
    {

        private int id;
        private int posid;
        private decimal versionNo;
        private string updatefileurl = String.Empty;
        private int updatetype;
        private DateTime updatetime;



        public Posversionmodifylog() { }




        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }





        public int Posid
        {
            get { return this.posid; }
            set { this.posid = value; }
        }


        public decimal VersionNo
        {
            get { return this.versionNo; }
            set { this.versionNo = value; }
        }


        public string Updatefileurl
        {
            get { return this.updatefileurl; }
            set { this.updatefileurl = value; }
        }


        public int Updatetype
        {
            get { return this.updatetype; }
            set { this.updatetype = value; }
        }


        public DateTime Updatetime
        {
            get { return this.updatetime; }
            set { this.updatetime = value; }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{

    [Serializable()]
    public class Posversionrenewlog
    {

        private int id;
        private int posid;
        private decimal initversionno;
        private decimal newversionno;
        private string updatefileurl = String.Empty;
        private int updatetype;
        private DateTime updatetime;



        public Posversionrenewlog() { }




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


        public decimal Initversionno
        {
            get { return this.initversionno; }
            set { this.initversionno = value; }
        }


        public decimal Newversionno
        {
            get { return this.newversionno; }
            set { this.newversionno = value; }
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

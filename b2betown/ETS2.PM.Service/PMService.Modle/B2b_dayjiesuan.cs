using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    [Serializable()]
    public class B2b_dayjiesuan
    {

        private int id;
        private DateTime jsstartdate;
        private DateTime jsenddate;
        private DateTime jstime;
        private int com_id;
        private int posid ;


        public B2b_dayjiesuan() { }




        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public int Com_id
        {
            get { return this.com_id; }
            set { this.com_id = value; }
        }
        public int PosId
        {
            get { return this.posid; }
            set { this.posid = value; }
        }


        public DateTime Jsstartdate
        {
            get { return this.jsstartdate; }
            set { this.jsstartdate = value; }
        }
        public DateTime Jsenddate
        {
            get { return this.jsenddate; }
            set { this.jsenddate = value; }
        }

        public DateTime Jstime
        {
            get { return this.jstime; }
            set { this.jstime = value; }
        }

    }
}

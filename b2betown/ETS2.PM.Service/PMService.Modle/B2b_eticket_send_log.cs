using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    [Serializable()]
    public class B2b_eticket_send_log
    {

        private int id;
        private int eticket_id;
        private string pnotext = String.Empty;
        private string phone = String.Empty;
        private int sendstate;
        private int sendtype;
        private DateTime senddate;



        public B2b_eticket_send_log() { }




        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }





        public int Eticket_id
        {
            get { return this.eticket_id; }
            set { this.eticket_id = value; }
        }


        public string Pnotext
        {
            get { return this.pnotext; }
            set { this.pnotext = value; }
        }


        public string Phone
        {
            get { return this.phone; }
            set { this.phone = value; }
        }


        public int Sendstate
        {
            get { return this.sendstate; }
            set { this.sendstate = value; }
        }


        public int Sendtype
        {
            get { return this.sendtype; }
            set { this.sendtype = value; }
        }


        public DateTime Senddate
        {
            get { return this.senddate; }
            set { this.senddate = value; }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    [Serializable()]
    public class Pos_log
    {

        private int id;
        private string str;
        private DateTime subdate;
        private string uip;
        private string returnstr;
        private DateTime returnsubdate;
       



        public Pos_log() { }




        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }





        public string Str
        {
            get { return this.str; }
            set { this.str = value; }
        }
        public string Uip
        {
            get { return this.uip; }
            set { this.uip = value; }
        }


        public DateTime Subdate
        {
            get { return this.subdate; }
            set { this.subdate = value; }
        }


        public string ReturnStr
        {
            get { return this.returnstr; }
            set { this.returnstr = value; }
        }
       


        public DateTime ReturnSubdate
        {
            get { return this.returnsubdate; }
            set { this.returnsubdate = value; }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class ApiLog
    {
        private int id;
        private int serviceid;
        private string str;
        private DateTime subdate;
        private string errmsg;
        private string returnstr;
        private DateTime returnsubdate;

        public string request_type { get; set; }
         


        public ApiLog() { }




        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }



        public int Serviceid
        {
            get { return this.serviceid; }
            set { this.serviceid = value; }
        }

        public string Str
        {
            get { return this.str; }
            set { this.str = value; }
        }
        public string Errmsg
        {
            get { return this.errmsg; }
            set { this.errmsg = value; }
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

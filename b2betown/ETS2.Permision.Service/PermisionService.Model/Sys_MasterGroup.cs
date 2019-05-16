using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Permision.Service.PermisionService.Model
{
    [Serializable()]
    public class Sys_MasterGroup
    {

        private int id;
        private int masterid;
        private string mastername = String.Empty;
        private int groupid;
        private int masterid2;
        private string mastername2 = String.Empty;
        private DateTime createdate;



        public Sys_MasterGroup() { }




        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }





        public int Masterid
        {
            get { return this.masterid; }
            set { this.masterid = value; }
        }


        public string Mastername
        {
            get { return this.mastername; }
            set { this.mastername = value; }
        }


        public int Groupid
        {
            get { return this.groupid; }
            set { this.groupid = value; }
        }


        public int Masterid2
        {
            get { return this.masterid2; }
            set { this.masterid2 = value; }
        }


        public string Mastername2
        {
            get { return this.mastername2; }
            set { this.mastername2 = value; }
        }


        public DateTime Createdate
        {
            get { return this.createdate; }
            set { this.createdate = value; }
        }

    }
}

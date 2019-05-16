using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Permision.Service.PermisionService.Model
{
    [Serializable()]
    public class Sys_Group
    {
        public int OrderIsAccurateToPerson { get; set; }
        private int groupid;
        private string groupname = String.Empty;
        private string groupinfo = String.Empty;
        private int masterid;
        private string mastername = String.Empty;
        private DateTime createdate;


        private string groupids = String.Empty;
        private bool isviewchannel;

        private bool crmIsAccurateToPerson;


        private int iscanverify;

        public int iscanset_imprest { get; set; }
        public int iscanset_order { get; set; }
        public int validateservertype { get; set; }
        public int canviewpro { get; set; }
        
        public Sys_Group() { }


        public int Iscanverify
        {
            get { return this.iscanverify; }
            set { this.iscanverify = value; }
        }
        public string Groupids
        {
            get { return this.groupids; }
            set { this.groupids = value; }
        }

        public bool Isviewchannel
        {
            get { return this.isviewchannel; }
            set { this.isviewchannel = value; }
        }
        public bool CrmIsAccurateToPerson
        {
            get { return this.crmIsAccurateToPerson; }
            set { this.crmIsAccurateToPerson = value; }
        }




        public int Groupid
        {
            get { return this.groupid; }
            set { this.groupid = value; }
        }





        public string Groupname
        {
            get { return this.groupname; }
            set { this.groupname = value; }
        }


        public string Groupinfo
        {
            get { return this.groupinfo; }
            set { this.groupinfo = value; }
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


        public DateTime Createdate
        {
            get { return this.createdate; }
            set { this.createdate = value; }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Permision.Service.PermisionService.Model
{
    [Serializable()]
    public class Sys_ActionGroup
    {

        private int id;
        private int actionid;
        private int groupid;
        private int masterid;
        private string mastername = String.Empty;
        private DateTime createdate;
        private Sys_Action sysAction;





        public Sys_ActionGroup() { }


        public Sys_Action SysAction
        {
            get { return this.sysAction; }
            set { this.sysAction = value; }
        }


        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }





        public int Actionid
        {
            get { return this.actionid; }
            set { this.actionid = value; }
        }


        public int Groupid
        {
            get { return this.groupid; }
            set { this.groupid = value; }
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

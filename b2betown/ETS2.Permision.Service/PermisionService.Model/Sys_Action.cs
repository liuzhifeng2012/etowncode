using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Permision.Service.PermisionService.Model
{
    [Serializable()]
    public class Sys_Action
    {

        private int actionid;
        private string actionname = String.Empty;
        private int actioncolumnid;
        private string actionurl = String.Empty;
        private bool viewmode;
        private int sortid;
        private string actioncolumnname = String.Empty;


        public Sys_Action() { }




        public int Actionid
        {
            get { return this.actionid; }
            set { this.actionid = value; }
        }

        public string Actioncolumnname
        {
            get { return this.actioncolumnname; }
            set { this.actioncolumnname = value; }
        }




        public string Actionname
        {
            get { return this.actionname; }
            set { this.actionname = value; }
        }


        public int Actioncolumnid
        {
            get { return this.actioncolumnid; }
            set { this.actioncolumnid = value; }
        }


        public string Actionurl
        {
            get { return this.actionurl; }
            set { this.actionurl = value; }
        }


        public bool Viewmode
        {
            get { return this.viewmode; }
            set { this.viewmode = value; }
        }


        public int Sortid
        {
            get { return this.sortid; }
            set { this.sortid = value; }
        }

    }
}

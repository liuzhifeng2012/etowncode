using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Permision.Service.PermisionService.Model
{
    [Serializable()]
    public class Sys_ActionColumn
    {

        private int actioncolumnid;
        private string actioncolumnname = String.Empty;
        private bool viewmode;


        public Sys_ActionColumn() { }




        public int Actioncolumnid
        {
            get { return this.actioncolumnid; }
            set { this.actioncolumnid = value; }
        }


        public bool Viewmode
        {
            get { return this.viewmode; }
            set { this.viewmode = value; }
        }



        public string Actioncolumnname
        {
            get { return this.actioncolumnname; }
            set { this.actioncolumnname = value; }
        }

    }
}

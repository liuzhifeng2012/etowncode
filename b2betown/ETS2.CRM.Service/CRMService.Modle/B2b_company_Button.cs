using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
     [Serializable()]
    public class B2b_company_Button
    {
        
        private int id;
        private int comid;
        private string name = String.Empty;
        private string linkurl = String.Empty;
        private string linkurlname;
        private int sort;
        private int linktype;


        public B2b_company_Button() { }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public string Linkurl
        {
            get { return this.linkurl; }
            set { this.linkurl = value; }
        }
        public string Linkurlname
        {
            get { return this.linkurlname; }
            set { this.linkurlname = value; }
        }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public int Comid
        {
            get { return this.comid; }
            set { this.comid = value; }
        }
        public int Sort
        {
            get { return this.sort; }
            set { this.sort = value; }
        }
        public int Linktype
        {
            get { return this.linktype; }
            set { this.linktype = value; }
        }
         
    }
}

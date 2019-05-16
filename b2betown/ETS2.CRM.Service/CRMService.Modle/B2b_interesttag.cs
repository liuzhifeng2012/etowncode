using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    public class B2b_interesttag
    {
        private int id;
        private string tagname;
        private int tagtypeid;

        public B2b_interesttag() { }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public string TagName
        {
            get { return this.tagname; }
            set { this.tagname = value; }
        }
        public int Tagtypeid
        {
            get { return this.tagtypeid; }
            set { this.tagtypeid = value; }
        }
    }
}

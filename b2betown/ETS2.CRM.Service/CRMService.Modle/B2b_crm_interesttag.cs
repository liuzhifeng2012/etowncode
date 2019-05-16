using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    public class B2b_crm_interesttag
    {
        private int id;
        private int crmid;
        private int tagid;

        public B2b_crm_interesttag() { }
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public int Crmid
        {
            get { return this.crmid; }
            set { this.crmid = value; }
        }
        public int Tagid
        {
            get { return this.tagid; }
            set { this.tagid = value; }
        }
    }
}

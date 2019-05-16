using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    public class B2b_companyindustry
    {
        private int id;
        private string industryname;
        private string remark;

        public B2b_companyindustry() { }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public string Industryname
        {
            get { return this.industryname; }
            set { this.industryname = value; }
        }
        public string Remark
        {
            get { return this.remark; }
            set { this.remark = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    public class B2b_interesttagtype
    {
        private int id;
        private string typename;
        private string remark;
        private DateTime createtime;
        private int industryid;
        private int isselfdefined;
        private IList<B2b_interesttag> b2binteresttag;

        public B2b_interesttagtype() { }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public string Typename
        {
            get { return this.typename; }
            set { this.typename = value; }
        }
        public string Remark
        {
            get { return this.remark; }
            set { this.remark = value; }
        }
        public DateTime Createtime
        {
            get { return this.createtime; }
            set { this.createtime = value; }
        }
        public int Industryid
        {
            get { return this.industryid; }
            set { this.industryid = value; }
        }
        public int Isselfdefined
        {
            get { return this.isselfdefined; }
            set { this.isselfdefined = value; }
        }

        public IList<B2b_interesttag> B2binteresttag
        {
            get { return this.b2binteresttag; }
            set { this.b2binteresttag = value; }
        }
     

    }
}

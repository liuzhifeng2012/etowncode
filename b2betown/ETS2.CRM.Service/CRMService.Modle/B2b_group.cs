using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    public class B2b_group
    {
        private int id;
        private string groupname;
        private string remark;
        private int comid;
        private DateTime createtime;
        private int createuserid;

        public B2b_group() { }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public string Groupname
        {
            get { return this.groupname; }
            set { this.groupname = value; }
        }
        public string Remark
        {
            get { return this.remark; }
            set { this.remark = value; }
        }
        public int Comid
        {
            get { return this.comid; }
            set { this.comid = value; }

        }
        public DateTime Createtime
        {
            get { return this.createtime; }
            set { this.createtime = value; }
        }
        public int Createuserid
        {
            get { return this.createuserid; }
            set { this.createuserid = value; }

        }
    }
}

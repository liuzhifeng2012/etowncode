using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    [Serializable()]
    public class ExcelImportLog
    {
        private int id;
        private int crmid;
        private int comid;
        private string idcard = String.Empty;
        private string name = String.Empty;
        private string sex = String.Empty;
        private string phone = String.Empty;
        private string email = String.Empty;
        private string weixin = String.Empty;
        private DateTime importtime;
        private int age;
        private DateTime birthday;
        private int importnum = 0;////录入计数
        private bool whetherwxfocus = false;
        private bool whetheractivate = false;

        public ExcelImportLog() { }


        public DateTime Birthday
        {
            get { return birthday; }
            set { birthday = value; }
        }

        public bool Whetherwxfocus
        {
            get { return this.whetherwxfocus; }
            set { this.whetherwxfocus = value; }
        }

        public bool Whetheractivate
        {
            get { return this.whetheractivate; }
            set { this.whetheractivate = value; }
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
        public int Crmid
        {
            get { return this.crmid; }
            set { this.crmid = value; }
        }
        public string Idcard
        {
            get { return this.idcard; }
            set { this.idcard = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }


        public string Sex
        {
            get { return this.sex; }
            set { this.sex = value; }
        }


        public string Phone
        {
            get { return this.phone; }
            set { this.phone = value; }
        }


        public string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }


        public string Weixin
        {
            get { return this.weixin; }
            set { this.weixin = value; }
        }




        public DateTime Importtime
        {
            get { return this.importtime; }
            set { this.importtime = value; }
        }


        public int Age
        {
            get { return this.age; }
            set { this.age = value; }
        }

        public int Importnum
        {
            get { return importnum; }
            set { importnum = value; }
        }
    }
}

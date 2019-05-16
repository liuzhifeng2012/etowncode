using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    [Serializable()]
    public class Phone_codemodel
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private int comid;

        public int Comid
        {
            get { return comid; }
            set { comid = value; }
        }
        private decimal phone;

        public decimal Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        private decimal code;

        public decimal Code
        {
            get { return code; }
            set { code = value; }
        }
        private int codenum;

        public int Codenum
        {
            get { return codenum; }
            set { codenum = value; }
        }
        private int codebool;

        public int Codebool
        {
            get { return codebool; }
            set { codebool = value; }
        }
        private DateTime codetime;

        public DateTime Codetime
        {
            get { return codetime; }
            set { codetime = value; }
        }
        private string openid;

        public string Openid
        {
            get { return openid; }
            set { openid = value; }
        }
    }
}

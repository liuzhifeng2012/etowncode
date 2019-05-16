using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{


    [Serializable()]
    public class B2b_company_saleset
    {

        private int id;
        private int com_id;
        private int payto;
        private int model_style;
        private string logo = String.Empty;
        private string title = String.Empty;
        private string service_Phone = String.Empty;
        private string workingHours = String.Empty;
        private string copyright = String.Empty;
        private string tophtml = String.Empty;
        private string bottomHtml = String.Empty;
        private int dealuserid;
        private string smsaccount = String.Empty;
        private string smspass = String.Empty;
        private string smssign = String.Empty;
        private string subid = String.Empty;
        private int smstype;


        private string smalllogo = String.Empty;
        private string compressionlogo = String.Empty;


        public B2b_company_saleset() { }

        public string Smalllogo
        {
            get { return this.smalllogo; }
            set { this.smalllogo = value; }
        }
        public string Compressionlogo
        {
            get { return this.compressionlogo; }
            set { this.compressionlogo = value; }
        }


        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }





        public int Com_id
        {
            get { return this.com_id; }
            set { this.com_id = value; }
        }


        public int Payto
        {
            get { return this.payto; }
            set { this.payto = value; }
        }


        public int Model_style
        {
            get { return this.model_style; }
            set { this.model_style = value; }
        }


        public string Logo
        {
            get { return this.logo; }
            set { this.logo = value; }
        }


        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }


        public string Service_Phone
        {
            get { return this.service_Phone; }
            set { this.service_Phone = value; }
        }


        public string WorkingHours
        {
            get { return this.workingHours; }
            set { this.workingHours = value; }
        }


        public string Copyright
        {
            get { return this.copyright; }
            set { this.copyright = value; }
        }


        public string Tophtml
        {
            get { return this.tophtml; }
            set { this.tophtml = value; }
        }


        public string BottomHtml
        {
            get { return this.bottomHtml; }
            set { this.bottomHtml = value; }
        }


        public int Dealuserid
        {
            get { return this.dealuserid; }
            set { this.dealuserid = value; }
        }

        public string Smsaccount
        {
            get { return this.smsaccount; }
            set { this.smsaccount = value; }
        }

        public string Smspass
        {
            get { return this.smspass; }
            set { this.smspass = value; }
        }


        public string Smssign
        {
            get { return this.smssign; }
            set { this.smssign = value; }
        }

        public int Smstype
        {
            get { return this.smstype; }
            set { this.smstype = value; }
        }

        public string Subid
        {
            get { return this.subid; }
            set { this.subid = value; }
        }
        
    }
}

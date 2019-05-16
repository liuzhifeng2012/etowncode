using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{

    [Serializable()]
    public class B2b_company_pos
    {

        private int id;
        private int com_id;
        private string poscompany = String.Empty;
        private int posid;
        private DateTime bindingTime;
        private string admin = String.Empty;
        private string remark = String.Empty;



        public B2b_company_pos() { }




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


        public string Poscompany
        {
            get { return this.poscompany; }
            set { this.poscompany = value; }
        }


        public int Posid
        {
            get { return this.posid; }
            set { this.posid = value; }
        }


        public DateTime BindingTime
        {
            get { return this.bindingTime; }
            set { this.bindingTime = value; }
        }


        public string Admin
        {
            get { return this.admin; }
            set { this.admin = value; }
        }


        public string Remark
        {
            get { return this.remark; }
            set { this.remark = value; }
        }

    }
}
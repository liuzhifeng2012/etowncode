using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS2.CRM.Service.CRMService.Modle;

namespace ETS2.PM.Service.PMService.Modle
{
    [Serializable()]
    public class B2b_eticket
    {

        private int id;
        private int com_id;
        private int pro_id;
        private int agent_id;
        private string pno;
        private int e_type;
        private int pnum;
        private int use_pnum;
        private string e_proname = String.Empty;
        private decimal e_face_price;
        private decimal e_sale_price;
        private decimal e_cost_price;
        private int v_state;
        private int send_state;
        private int warrant_type;

        private int runstate ;
        private int oid ;
        private DateTime sendtime ;

        private decimal pagecode;
        private int printstate;

        public string Cancelnum { get; set; }
         public string bindingname{ get; set; }
         public string bindingphone{ get; set; }
         public string bindingcard { get; set; }


         public int sendcard { get; set; }
         public int ishasdeposit { get; set; }

        private DateTime subdate;
        private B2b_com_pro compro;
        private B2b_company company;
        private B2b_company_info companyinfo;

        public int VerifyNum { get; set; }


        public B2b_eticket() { }

        public B2b_com_pro Compro
        {
            get { return this.compro; }
            set { this.compro = value; }
        }
        public B2b_company Company
        {
            get { return this.company; }
            set { this.company = value; }
        }
        public B2b_company_info Companyinfo
        {
            get { return this.companyinfo; }
            set { this.companyinfo = value; }
        }
     


        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }


        public decimal Pagecode
        {
            get { return this.pagecode; }
            set { this.pagecode = value; }
        }

        public int Printstate
        {
            get { return this.printstate; }
            set { this.printstate = value; }
        }



        public int Com_id
        {
            get { return this.com_id; }
            set { this.com_id = value; }
        }


        public int Pro_id
        {
            get { return this.pro_id; }
            set { this.pro_id = value; }
        }


        public int Agent_id
        {
            get { return this.agent_id; }
            set { this.agent_id = value; }
        }


        public string Pno
        {
            get { return this.pno; }
            set { this.pno = value; }
        }


        public int E_type
        {
            get { return this.e_type; }
            set { this.e_type = value; }
        }


        public int Pnum
        {
            get { return this.pnum; }
            set { this.pnum = value; }
        }


        public int Use_pnum
        {
            get { return this.use_pnum; }
            set { this.use_pnum = value; }
        }


        public string E_proname
        {
            get { return this.e_proname; }
            set { this.e_proname = value; }
        }


        public decimal E_face_price
        {
            get { return this.e_face_price; }
            set { this.e_face_price = value; }
        }


        public decimal E_sale_price
        {
            get { return this.e_sale_price; }
            set { this.e_sale_price = value; }
        }


        public decimal E_cost_price
        {
            get { return this.e_cost_price; }
            set { this.e_cost_price = value; }
        }


        public int V_state
        {
            get { return this.v_state; }
            set { this.v_state = value; }
        }


        public int Send_state
        {
            get { return this.send_state; }
            set { this.send_state = value; }
        }


        public DateTime Subdate
        {
            get { return this.subdate; }
            set { this.subdate = value; }
        }

        public DateTime Sendtime
        {
            get { return this.sendtime; }
            set { this.sendtime = value; }
        }

        public int Runstate
        {
            get { return this.runstate; }
            set { this.runstate = value; }
        }

        public int Oid
        {
            get { return this.oid; }
            set { this.oid = value; }
        }
        public int Warrant_type
        {
            get { return this.warrant_type; }
            set { this.warrant_type = value; }
        }
        
    }
}

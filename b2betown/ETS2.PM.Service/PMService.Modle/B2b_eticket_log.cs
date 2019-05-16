using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    [Serializable()]
    public class B2b_eticket_log
    {

        private int id;
        private int eticket_id;
        private string pno;
        private int action;
        private int a_state;
        private string a_remark = String.Empty;
        private int use_pnum;
        private DateTime actiondate;
        private int com_id;
        private int jsid;
        private int posid;
        private string randomid = "";//随机id

        private int agent_id;
        private string e_proname;
        private int pnum ;
        private decimal e_sale_price ;
        private int oid ;
        private int e_type;
        private string pcaccount;


        public B2b_eticket_log() { }

        public string RandomId
        {
            get { return this.randomid; }
            set { this.randomid = value; }
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

        public int JsId
        {
            get { return this.jsid; }
            set { this.jsid = value; }
        }

        public int Eticket_id
        {
            get { return this.eticket_id; }
            set { this.eticket_id = value; }
        }


        public string Pno
        {
            get { return this.pno; }
            set { this.pno = value; }
        }


        public int Action
        {
            get { return this.action; }
            set { this.action = value; }
        }


        public int A_state
        {
            get { return this.a_state; }
            set { this.a_state = value; }
        }


        public string A_remark
        {
            get { return this.a_remark; }
            set { this.a_remark = value; }
        }
        public int PosId
        {
            get { return this.posid; }
            set { this.posid = value; }
        }

        public int Use_pnum
        {
            get { return this.use_pnum; }
            set { this.use_pnum = value; }
        }


        public DateTime Actiondate
        {
            get { return this.actiondate; }
            set { this.actiondate = value; }
        }

        public int Agent_id
        {
            get { return this.agent_id; }
            set { this.agent_id = value; }
        }

        public string E_proname
        {
            get { return this.e_proname; }
            set { this.e_proname = value; }
        }

        public int Pnum
        {
            get { return this.pnum; }
            set { this.pnum = value; }
        }

        public decimal E_sale_price
        {
            get { return this.e_sale_price; }
            set { this.e_sale_price = value; }
        }

        public int Oid
        {
            get { return this.oid; }
            set { this.oid = value; }
        }
        public int E_type
        {
            get { return this.e_type; }
            set { this.e_type = value; }
        }

        public string Pcaccount
        {
            get { return this.pcaccount; }
            set { this.pcaccount = value; }
        }
    }
}

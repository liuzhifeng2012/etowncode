using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    /// <summary>
    /// 商家基本信息表
    /// </summary>
    [Serializable()]
    public class B2b_company
    {

        private int iD;
        private string com_name = String.Empty;
        private string scenic_name = String.Empty;
        private int com_type;
        private int com_state;
        private decimal imprest;
        private B2b_company_info b2bcompanyinfo;

        private decimal fee;
        private decimal servicefee;
        private int agentid;


        private DateTime openDate;
        private DateTime endDate;
        private int agentopenstate;
        private int bindingagent;
        private int lp;
        private int lp_agentlevel;
        private int setsearch;
        private string agent_nuomi_bindcomname = "";

        public int isqunar { get; set; }
        public string qunar_username { get; set; }
        public string qunar_pass { get; set; }

        public int micromallimgid { get; set; }
        public string com_scenic_intro { get; set; }

        public B2b_company() { }

        public string Agent_nuomi_bindcomname
        {
            get { return this.agent_nuomi_bindcomname; }
            set { this.agent_nuomi_bindcomname = value; }
        }
        public decimal Fee
        {
            get { return this.fee; }
            set { this.fee = value; }
        }
        public decimal ServiceFee
        {
            get { return this.servicefee; }
            set { this.servicefee = value; }
        }

        public int ID
        {
            get { return this.iD; }
            set { this.iD = value; }
        }

        public int Bindingagent
        {
            get { return this.bindingagent; }
            set { this.bindingagent = value; }
        }

        public string Com_name
        {
            get { return this.com_name; }
            set { this.com_name = value; }
        }


        public string Scenic_name
        {
            get { return this.scenic_name; }
            set { this.scenic_name = value; }
        }


        public int Com_type
        {
            get { return this.com_type; }
            set { this.com_type = value; }
        }


        public int Com_state
        {
            get { return this.com_state; }
            set { this.com_state = value; }
        }


        public decimal Imprest
        {
            get { return this.imprest; }
            set { this.imprest = value; }
        }
        public B2b_company_info B2bcompanyinfo
        {
            get { return this.b2bcompanyinfo; }
            set { this.b2bcompanyinfo = value; }
        }

        public int Agentid
        {
            get { return this.agentid; }
            set { this.agentid = value; }
        }

        public DateTime OpenDate
        {
            get { return this.openDate; }
            set { this.openDate = value; }
        }
        public DateTime EndDate
        {
            get { return this.endDate; }
            set { this.endDate = value; }
        }
        public int Agentopenstate
        {
            get { return this.agentopenstate; }
            set { this.agentopenstate = value; }
        }
        public int Lp
        {
            get { return this.lp; }
            set { this.lp = value; }
        }

        public int Lp_agentlevel
        {
            get { return this.lp_agentlevel; }
            set { this.lp_agentlevel = value; }
        }
        public int Setsearch
        {
            get { return this.setsearch; }
            set { this.setsearch = value; }
        }
    }
}

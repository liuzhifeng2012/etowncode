using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{   /// <summary>
    /// 分销商基本信息表
    /// </summary>
    [Serializable()]
    public class Agent_warrant
    {
        private int id;
        private int agentid;
        private int comid;
        private int warrant_state;
        private int warrant_type;
        private int warrant_level;
        private int warrant_lp;
        private decimal imprest;
        private decimal credit;


        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public int Agentid
        {
            get { return this.agentid; }
            set { this.agentid = value; }
        }

        public int Comid
        {
            get { return this.comid; }
            set { this.comid = value; }
        }

        public int Warrant_state
        {
            get { return this.warrant_state; }
            set { this.warrant_state = value; }
        }

        public int Warrant_type
        {
            get { return this.warrant_type; }
            set { this.warrant_type = value; }
        }
        public int Warrant_level
        {
            get { return this.warrant_level; }
            set { this.warrant_level = value; }
        }

        public int Warrant_lp
        {
            get { return this.warrant_lp; }
            set { this.warrant_lp = value; }
        }

        public decimal Imprest
        {
            get { return this.imprest; }
            set { this.imprest = value; }
        }
        public decimal Credit
        {
            get { return this.credit; }
            set { this.credit = value; }
        }

        
    }

}

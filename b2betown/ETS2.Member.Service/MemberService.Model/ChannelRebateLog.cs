using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Member.Service.MemberService.Model
{

    [Serializable()]
    public class ChannelRebateLog
    {

        private int id;
        private int channelid;
        private int type;
        private decimal rebatemoney;
        private decimal summoney;
        private DateTime execdate;
        private string remark = String.Empty;



        public ChannelRebateLog() { }




        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }





        public int Channelid
        {
            get { return this.channelid; }
            set { this.channelid = value; }
        }


        public int Type
        {
            get { return this.type; }
            set { this.type = value; }
        }


        public decimal Rebatemoney
        {
            get { return this.rebatemoney; }
            set { this.rebatemoney = value; }
        }


        public decimal Summoney
        {
            get { return this.summoney; }
            set { this.summoney = value; }
        }


        public DateTime Execdate
        {
            get { return this.execdate; }
            set { this.execdate = value; }
        }


        public string Remark
        {
            get { return this.remark; }
            set { this.remark = value; }
        }

    }
}

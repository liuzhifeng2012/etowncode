using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Member.Service.MemberService.Model
{
    [Serializable()]

    /// <summary>
    /// 摇奖活动,奖品随机码
    /// </summary>
    public class ERNIE_Awardcode
    {
        private int id;
        private int award_id;
        private int eRNIE_id;
        private int usestate;
        private decimal award_code;
        private int uid;



        public ERNIE_Awardcode() { }


        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public int Award_id
        {
            get { return this.award_id; }
            set { this.award_id = value; }
        }

        public int ERNIE_id
        {
            get { return this.eRNIE_id; }
            set { this.eRNIE_id = value; }
        }

        public int Usestate
        {
            get { return this.usestate; }
            set { this.usestate = value; }
        }

        public decimal Award_code
        {
            get { return this.award_code; }
            set { this.award_code = value; }
        }

        public int Uid
        {
            get { return this.uid; }
            set { this.uid = value; }
        }


    }
}

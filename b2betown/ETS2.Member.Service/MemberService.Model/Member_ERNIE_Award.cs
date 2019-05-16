using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Member.Service.MemberService.Model
{
    [Serializable()]

    /// <summary>
    /// 摇奖活动奖品
    /// </summary>
    public class Member_ERNIE_Award
    {

        private int id;
        private int eRNIE_id;
        private string award_title = String.Empty;
        private int award_class;
        private int award_num;
        private int award_type;
        private int award_Get_Num;
       

        public Member_ERNIE_Award() { }


        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public int ERNIE_id
        {
            get { return this.eRNIE_id; }
            set { this.eRNIE_id = value; }
        }

        public string Award_title
        {
            get { return this.award_title; }
            set { this.award_title = value; }
        }

        public int Award_class
        {
            get { return this.award_class; }
            set { this.award_class = value; }
        }

        public int Award_type
        {
            get { return this.award_type; }
            set { this.award_type = value; }
        }

        public int Award_num
        {
            get { return this.award_num; }
            set { this.award_num = value; }
        }

        public int Award_Get_Num
        {
            get { return this.award_Get_Num; }
            set { this.award_Get_Num = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Member.Service.MemberService.Model
{    [Serializable()]

         /// <summary>
    /// 摇奖活动
    /// </summary>
   public class Member_ERNIE
    {

        private int id;
        private int com_id;
        private string title = String.Empty;
        private int eRNIE_type;
        private DateTime eRNIE_star;
        private DateTime eRNIE_end;
        private int eRNIE_RateNum;
        private int eRNIE_Limit;
        private int limit_Num;
        private int runstate;
        private string remark = String.Empty;
        private int online;


        public Member_ERNIE() { }


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


        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }


        public int ERNIE_type
        {
            get { return this.eRNIE_type; }
            set { this.eRNIE_type = value; }
        }


        public DateTime ERNIE_star
        {
            get { return this.eRNIE_star; }
            set { this.eRNIE_star = value; }
        }


        public DateTime ERNIE_end
        {
            get { return this.eRNIE_end; }
            set { this.eRNIE_end = value; }
        }


        public int ERNIE_RateNum
        {
            get { return this.eRNIE_RateNum; }
            set { this.eRNIE_RateNum = value; }
        }

        public int ERNIE_Limit
        {
            get { return this.eRNIE_Limit; }
            set { this.eRNIE_Limit = value; }
        }
        public int Limit_Num
        {
            get { return this.limit_Num; }
            set { this.limit_Num = value; }
        }
        public int Runstate
        {
            get { return this.runstate; }
            set { this.runstate = value; }
        }


        public string Remark
        {
            get { return this.remark; }
            set { this.remark = value; }
        }

        public int Online
        {
            get { return this.online; }
            set { this.online = value; }
        }
    }
}

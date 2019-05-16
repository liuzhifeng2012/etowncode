using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Member.Service.MemberService.Model
{
    [Serializable()]
    public class Member_Issue_Activity
    {

        private int id;
        private int iSid;
        private int acid;



        public Member_Issue_Activity() { }




        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }





        public int ISid
        {
            get { return this.iSid; }
            set { this.iSid = value; }
        }


        public int Acid
        {
            get { return this.acid; }
            set { this.acid = value; }
        }

    }
}

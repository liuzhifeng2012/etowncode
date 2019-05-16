using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Member.Service.MemberService.Model
{

    [Serializable()]
    public class Member_Card_Activity
    {

        private int id;
        private int cardID;
        private int aCTID;
        private int actnum;
        private int uSEstate;
        private DateTime uSEsubdate;
        private DateTime subdate;


        public Member_Card_Activity() { }




        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }





        public int CardID
        {
            get { return this.cardID; }
            set { this.cardID = value; }
        }


        public int ACTID
        {
            get { return this.aCTID; }
            set { this.aCTID = value; }
        }


        public int Actnum
        {
            get { return this.actnum; }
            set { this.actnum = value; }
        }


        public int USEstate
        {
            get { return this.uSEstate; }
            set { this.uSEstate = value; }
        }


        public DateTime USEsubdate
        {
            get { return this.uSEsubdate; }
            set { this.uSEsubdate = value; }
        }
        public DateTime Subdate
        {
            get { return this.subdate; }
            set { this.subdate = value; }
        }
    }
}

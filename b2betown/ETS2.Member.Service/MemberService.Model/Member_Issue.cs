using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Member.Service.MemberService.Model
{
    [Serializable()]
    public class Member_Issue
    {

        private int id;
        private int com_id;
        private int crid;
        private int chid;
        private string title = String.Empty;
        private int num;
        private bool openyes;
        private int openaddress;



        public Member_Issue() { }




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


        public int Crid
        {
            get { return this.crid; }
            set { this.crid = value; }
        }


        public int Chid
        {
            get { return this.chid; }
            set { this.chid = value; }
        }


        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }


        public int Num
        {
            get { return this.num; }
            set { this.num = value; }
        }


        public bool Openyes
        {
            get { return this.openyes; }
            set { this.openyes = value; }
        }


        public int Openaddress
        {
            get { return this.openaddress; }
            set { this.openaddress = value; }
        }

    }
}

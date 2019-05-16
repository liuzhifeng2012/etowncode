using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Member.Service.MemberService.Model
{
    [Serializable()]
   public  class Member_Activity_Log
    {
        private int iD;

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
        private int cardID;

        public int CardID
        {
            get { return cardID; }
            set { cardID = value; }
        }
        private int aCTID;

        public int ACTID
        {
            get { return aCTID; }
            set { aCTID = value; }
        }
        private string pno;

        public string Pno
        {
            get { return pno; }
            set { pno = value; }
        }
        private int usenum;

        public int Usenum
        {
            get { return usenum; }
            set { usenum = value; }
        }
        private decimal orderId;

        public decimal OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }
        private string serverName;

        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }
        private int num_people;

        public int Num_people
        {
            get { return num_people; }
            set { num_people = value; }
        }
        private decimal per_capita_money;

        public decimal Per_capita_money
        {
            get { return per_capita_money; }
            set { per_capita_money = value; }
        }
        private string sales_admin;

        public string Sales_admin
        {
            get { return sales_admin; }
            set { sales_admin = value; }
        }
        private decimal member_return_money;

        public decimal Member_return_money
        {
            get { return member_return_money; }
            set { member_return_money = value; }
        }
        private int return_money_state;

        public int Return_money_state
        {
            get { return return_money_state; }
            set { return_money_state = value; }
        }
        private string return_money_admin;

        public string Return_money_admin
        {
            get { return return_money_admin; }
            set { return_money_admin = value; }
        }
        private DateTime usesubdate;

        public DateTime Usesubdate
        {
            get { return usesubdate; }
            set { usesubdate = value; }
        }

        private int comid;

        public int Comid
        {
            get { return comid; }
            set { comid = value; }
        }
    }
}

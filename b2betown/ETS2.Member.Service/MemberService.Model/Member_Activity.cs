using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Member.Service.MemberService.Model
{
    /// <summary>
    /// 促销活动
    /// </summary>
    [Serializable()]
    public class Member_Activity
    {

        private int id;
        private int com_id;
        private string title = String.Empty;
        private int acttype;
        private decimal money;
        private double discount;
        private decimal cashFull;
        private decimal cashback;
        private bool useOnce;
        private DateTime actstar;
        private DateTime actend;
        private int faceObjects;
        private int returnact;
        private int repeatissue;
        private bool runstate;
        private int usestate;
        private int actnum;
        private int cardid;
        private int aid;

        private string atitle = String.Empty;
        private string usetitle = String.Empty;
        private string remark = String.Empty;
        private string useremark = String.Empty;
        private string usechannel = String.Empty;

        private int createuserid;
        private DateTime createtime;

        public Member_Activity() { }


        public int CreateUserId
        {
            get { return this.createuserid; }
            set { this.createuserid = value; }
        }
        public DateTime CreateTime
        {
            get { return this.createtime; }
            set { this.createtime = value; }
        }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }


        private bool whethercreateqrcode = false;
        public bool Whethercreateqrcode
        {
            get { return this.whethercreateqrcode; }
            set { this.whethercreateqrcode = value; }
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


        public int Acttype
        {
            get { return this.acttype; }
            set { this.acttype = value; }
        }


        public decimal Money
        {
            get { return this.money; }
            set { this.money = value; }
        }


        public double Discount
        {
            get { return this.discount; }
            set { this.discount = value; }
        }


        public decimal CashFull
        {
            get { return this.cashFull; }
            set { this.cashFull = value; }
        }


        public decimal Cashback
        {
            get { return this.cashback; }
            set { this.cashback = value; }
        }


        public bool UseOnce
        {
            get { return this.useOnce; }
            set { this.useOnce = value; }
        }


        public DateTime Actstar
        {
            get { return this.actstar; }
            set { this.actstar = value; }
        }


        public DateTime Actend
        {
            get { return this.actend; }
            set { this.actend = value; }
        }


        public int FaceObjects
        {
            get { return this.faceObjects; }
            set { this.faceObjects = value; }
        }

        public int ReturnAct
        {
            get { return this.returnact; }
            set { this.returnact = value; }
        }
        public int RepeatIssue
        {
            get { return this.repeatissue; }
            set { this.repeatissue = value; }
        }
        public bool Runstate
        {
            get { return this.runstate; }
            set { this.runstate = value; }
        }

        public int Usestate
        {
            get { return this.usestate; }
            set { this.usestate = value; }
        }
        public int Actnum
        {
            get { return this.actnum; }
            set { this.actnum = value; }
        }

        public int Cardid
        {
            get { return this.cardid; }
            set { this.cardid = value; }
        }
        public int Aid
        {
            get { return this.aid; }
            set { this.aid = value; }
        }

        public string Atitle
        {
            get { return this.atitle; }
            set { this.atitle = value; }
        }

        public string Remark
        {
            get { return this.remark; }
            set { this.remark = value; }
        }

        public string Useremark
        {
            get { return this.useremark; }
            set { this.useremark = value; }
        }
        public string Usetitle
        {
            get { return this.usetitle; }
            set { this.usetitle = value; }
        }

        public string Usechannel
        {
            get { return this.usechannel; }
            set { this.usechannel = value; }
        }


    }
}

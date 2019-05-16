using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Member.Service.MemberService.Model
{
    public class WxSubscribeDetail
    {
        private int id;
        private string openid;
        private DateTime subscribetime;
        private int subscribesourceid;
        private int comid;

        private string createtime = "";


        public string Createtime
        {
            get { return this.createtime; }
            set { this.createtime = value; }
        }

        private int sex = 0;//0未知；1男；2女
        public int Sex
        {
            get { return this.sex; }
            set { this.sex = value; }
        }
        private string city = "";
        public string City
        {
            get { return this.city; }
            set { this.city = value; }
        }


        private string eevent = "";
        public string Eevent
        {
            get { return this.eevent; }
            set { this.eevent = value; }
        }
        private string eventkey = "";
        public string Eventkey
        {
            get { return this.eventkey; }
            set { this.eventkey = value; }
        }
        public WxSubscribeDetail() { }

        private string nickname = "";
        public string Nickname
        {
            get { return this.nickname; }
            set { this.nickname = value; }
        }


        private int channelcompanyid;
        public int Channelcompanyid
        {
            get { return this.channelcompanyid; }
            set { this.channelcompanyid = value; }
        }
        private int activityid;
        public int Activityid
        {
            get { return this.activityid; }
            set { this.activityid = value; }
        }



        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public string Openid
        {
            get { return this.openid; }
            set { this.openid = value; }
        }
        public DateTime Subscribetime
        {
            get { return this.subscribetime; }
            set { this.subscribetime = value; }
        }
        public int Subscribesourceid
        {
            get { return this.subscribesourceid; }
            set { this.subscribesourceid = value; }
        }
        public int Comid
        {
            get { return this.comid; }
            set { this.comid = value; }
        }
    }
}

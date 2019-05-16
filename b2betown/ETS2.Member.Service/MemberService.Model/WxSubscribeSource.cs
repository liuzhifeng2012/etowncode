using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Member.Service.MemberService.Model
{
    public class WxSubscribeSource
    {
        public int choujiangactid { get; set; }

        private int id;
        private int sourcetype;
        private int channelcompanyid;
        private int activityid;
        private int comid;

        public int Wxmaterialid { get; set; }

        private int productid;
        public int Productid 
        {
            get { return this.productid; }
            set { this.productid = value; }
        }
        private string title;
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }
        private string qrcodeurl;
        public string Qrcodeurl
        {
            get { return this.qrcodeurl; }
            set { this.qrcodeurl = value; }
        }
        private DateTime createtime = DateTime.Now;
        public DateTime Createtime
        {
            get { return this.createtime; }
            set { this.createtime = value; }
        }

        public WxSubscribeSource() { }
        private bool onlinestatus;
        public bool Onlinestatus
        {
            get { return this.onlinestatus; }
            set { this.onlinestatus = value; }
        }
        private string ticket = "";
        public string Ticket
        {
            get { return this.ticket; }
            set { this.ticket = value; }
        }
        private int channelid = 0;
        public int Channelid
        {
            get { return this.channelid; }
            set { this.channelid = value; }
        }
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public int Sourcetype
        {
            get { return this.sourcetype; }
            set { this.sourcetype = value; }
        }
        public int Channelcompanyid
        {
            get { return this.channelcompanyid; }
            set { this.channelcompanyid = value; }
        }
        public int Activityid
        {
            get { return this.activityid; }
            set { this.activityid = value; }
        }
        public int Comid
        {
            get { return this.comid; }
            set { this.comid = value; }
        }

        public int qrcodeviewtype { get; set; }
        public int projectid { get; set; }
        public int wxmaterialtypeid { get; set; }
        public int viewchannelcompanyid { get; set; }
    }
}

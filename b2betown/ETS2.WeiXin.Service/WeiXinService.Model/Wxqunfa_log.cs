using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{

    [Serializable()]
    public class Wxqunfa_log
    {
        private int id;
        private string msgtype;
        private string media_id = String.Empty;
        private string content = String.Empty;
        private DateTime sendtime;
        private int errcode;
        private string errmsg;
        private string msg_id;
        private int userid;
        private int channelcompanyid;
        private int comid;
        private string yearmonth;
        private string yearmonthday;
        private string weixins;

        public Wxqunfa_log() { }


        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public string Msgtype
        {
            get { return this.msgtype; }
            set { this.msgtype = value; }
        }
        public string Media_id
        {
            get { return this.media_id; }
            set { this.media_id = value; }
        }
        public string Content
        {
            get { return this.content; }
            set { this.content = value; }
        }
        public DateTime Sendtime
        {
            get { return this.sendtime; }
            set { this.sendtime = value; }
        }
        public int Errcode
        {
            get { return this.errcode; }
            set { this.errcode = value; }
        }
        public string Errmsg
        {
            get { return this.errmsg; }
            set { this.errmsg = value; }
        }
        public string Msg_id
        {
            get { return this.msg_id; }
            set { this.msg_id = value; }
        }
        public int Userid
        {
            get { return this.userid; }
            set { this.userid = value; }
        }
        public int Channelcompanyid
        {
            get { return this.channelcompanyid; }
            set { this.channelcompanyid = value; }
        }
        public int Comid
        {
            get { return this.comid; }
            set { this.comid = value; }
        }
        public string Yearmonth
        {
            get { return this.yearmonth; }
            set { this.yearmonth = value; }
        }
        public string Yearmonthday
        {
            get { return this.yearmonthday; }
            set { this.yearmonthday = value; }
        }
        public string Weixins
        {
            get { return this.weixins; }
            set { this.weixins = value; }
        }
    }

}

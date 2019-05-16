using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
    public class WxMemberBasic
    {
        private int id;
        private int subscribe;
        private string openid = String.Empty;
        private string nickname = String.Empty;
        private int sex = 0;
        private string language = String.Empty;
        private string city = String.Empty;
        private string province = String.Empty;
        private string country = String.Empty;
        private string headimgurl = "";
        private DateTime subscribe_time = DateTime.Now;
        private int comid;
        //更新时间
        public DateTime renewtime { get; set; }


        public WxMemberBasic() { }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public int Subscribe
        {
            get { return this.subscribe; }
            set { this.subscribe = value; }
        }
        public string Openid
        {
            get { return this.openid; }
            set { this.openid = value; }
        }
        public string Nickname
        {
            get { return this.nickname; }
            set { this.nickname = value; }
        }
        public int Sex
        {
            get { return this.sex; }
            set { this.sex = value; }
        }
        public string Language
        {
            get { return this.language; }
            set { this.language = value; }
        }
        public string City
        {
            get { return this.city; }
            set { this.city = value; }
        }
        public string Province
        {
            get { return this.province; }
            set { this.province = value; }
        }
        public string Country
        {
            get { return this.country; }
            set { this.country = value; }
        }
        public string Headimgurl
        {
            get { return this.headimgurl; }
            set { this.headimgurl = value; }
        }
        public DateTime Subscribe_time
        {
            get { return this.subscribe_time; }
            set { this.subscribe_time = value; }
        }
        public int Comid
        {
            get { return this.comid; }
            set { this.comid = value; }
        }

    }
}

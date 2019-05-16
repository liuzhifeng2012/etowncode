using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    [Serializable()]
    public class B2b_crm
    {

        private int id;
        private int com_id;
        private decimal idcard;
        private int cardid;
        private string name = String.Empty;
        private string sex = String.Empty;
        private string phone = String.Empty;
        private string email = String.Empty;
        private string weixin = String.Empty;
        private string laiyuan = String.Empty;
        private string password1 = String.Empty;
        private decimal servercard;
        private decimal imprest;
        private decimal integral;

        private DateTime regidate;
        private int age;

        private string crmlevel = "0";//会员级别,默认0(网站注册会员)

        private int groupid;

        private B2b_crm_location Crmlocation;


        private DateTime birthday;

        public DateTime Birthday
        {
            get { return birthday; }
            set { birthday = value; }
        }

        private DateTime wxlastinteracttime;

        public DateTime Wxlastinteracttime
        {
            get { return wxlastinteracttime; }
            set { wxlastinteracttime = value; }
        }


        public string CrmLevel
        {
            get { return crmlevel; }
            set { crmlevel = value; }
        }

        public int Groupid
        {
            get { return groupid; }
            set { groupid = value; }
        }
        public B2b_crm() { }

        private decimal dengjifen;
        public decimal Dengjifen 
        {
            get { return this.dengjifen; }
            set { this.dengjifen = value; }
        }

        private string wxheadimgurl;
        public string WxHeadimgurl
        {
            get { return this.wxheadimgurl; }
            set { this.wxheadimgurl = value; }
        }

        private bool whetherwxfocus = false;
        public bool Whetherwxfocus
        {
            get { return this.whetherwxfocus; }
            set { this.whetherwxfocus = value; }
        }
        private bool whetheractivate = false;
        public bool Whetheractivate
        {
            get { return this.whetheractivate; }
            set { this.whetheractivate = value; }
        }

        private string wxnickname;
        public string WxNickname
        {
            get { return this.wxnickname; }
            set { this.wxnickname = value; }
        }


        private string address;
        public string Address
        {
            get { return this.address; }
            set { this.address = value; }
        }

        private string wxcountry;
        public string Wxcountry
        {
            get { return this.wxcountry; }
            set { this.wxcountry = value; }
        }
        private string wxprovince;
        public string WxProvince
        {
            get { return this.wxprovince; }
            set { this.wxprovince = value; }
        }
        private string wxcity;
        public string WxCity
        {
            get { return this.wxcity; }
            set { this.wxcity = value; }
        }
        private int wxsex;
        public int WxSex
        {
            get { return this.wxsex; }
            set { this.wxsex = value; }
        }




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
        public decimal Idcard
        {
            get { return this.idcard; }
            set { this.idcard = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }


        public string Sex
        {
            get { return this.sex; }
            set { this.sex = value; }
        }


        public string Phone
        {
            get { return this.phone; }
            set { this.phone = value; }
        }


        public string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }


        public string Weixin
        {
            get { return this.weixin; }
            set { this.weixin = value; }
        }


        public string Laiyuan
        {
            get { return this.laiyuan; }
            set { this.laiyuan = value; }
        }
        public string Password1
        {
            get { return this.password1; }
            set { this.password1 = value; }
        }


        public DateTime Regidate
        {
            get { return this.regidate; }
            set { this.regidate = value; }
        }


        public int Age
        {
            get { return this.age; }
            set { this.age = value; }
        }

        public int Cardid
        {
            get { return this.cardid; }
            set { this.cardid = value; }
        }
        public decimal Servercard
        {
            get { return this.servercard; }
            set { this.servercard = value; }
        }


        public decimal Imprest
        {
            get { return this.imprest; }
            set { this.imprest = value; }
        }
        public decimal Integral
        {
            get { return this.integral; }
            set { this.integral = value; }
        }

        private int num;//登陆计数

        public int Num
        {
            get { return num; }
            set { num = value; }
        }

    }
}

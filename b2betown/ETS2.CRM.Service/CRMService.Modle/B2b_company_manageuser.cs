using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    /// <summary>
    /// 商家用户信息表
    /// </summary>
    [Serializable()]
    public class B2b_company_manageuser
    {

        private int id;
        private int com_id;
        private string accounts = String.Empty;
        private string passwords = String.Empty;
        private int atype;

        private string employeename = String.Empty;
        private string job = String.Empty;
        private string tel = String.Empty;
        private int viewtel = 1;

        private string oldtel = String.Empty;
        private int employeestate;
        private int createuserid;

        private int? channelcompanyid;
        private int? channelsource;

        private int peoplelistview;

        private int isdefaultkf;


        public double Distance{get;set;}
        public int IsCanZixun { get; set; }

        public int worktimestar { get; set; }
        public int worktimeend { get; set; }
        public int workendtimestar { get; set; }
        public int workendtimeend { get; set; }

        public int bindingproid { get; set; }

        private string selfbrief;
        public string Selfbrief
        {
            get { return this.selfbrief; }
            set { this.selfbrief = value; }
        }
        private int headimg;
        public int Headimg
        {
            get { return this.headimg; }
            set { this.headimg = value; }
        }
        private int workingyears;
        public int Workingyears
        {
            get { return this.workingyears; }
            set { this.workingyears = value; }
        }
        private string workdays;
        public string Workdays
        {
            get { return this.workdays; }
            set { this.workdays = value; }
        }
        private string workdaystime;
        public string Workdaystime
        {
            get { return this.workdaystime; }
            set { this.workdaystime = value; }
        }
        private string workendtime;
        public string Workendtime
        {
            get { return this.workendtime; }
            set { this.workendtime = value; }
        }
        private string fixphone;
        public string Fixphone
        {
            get { return this.fixphone; }
            set { this.fixphone = value; }
        }
        private string email;
        public string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }
        private string homepage;
        public string Homepage
        {
            get { return this.homepage; }
            set { this.homepage = value; }
        }
        private string weibopage;
        public string Weibopage
        {
            get { return this.weibopage; }
            set { this.weibopage = value; }
        }
        private string qq;
        public string QQ
        {
            get { return this.qq; }
            set { this.qq = value; }
        }
        private string weixin;
        public string Weixin
        {
            get { return this.weixin; }
            set { this.weixin = value; }
        }
        private string selfhomepage_qrcordurl;
        public string Selfhomepage_qrcordurl
        {
            get { return this.selfhomepage_qrcordurl; }
            set { this.selfhomepage_qrcordurl = value; }
        }

        public B2b_company_manageuser() { }

        public int? Channelcompanyid
        {
            get { return this.channelcompanyid; }
            set { this.channelcompanyid = value; }
        }
        public int? Channelsource
        {
            get { return this.channelsource; }
            set { this.channelsource = value; }
        }

        public string Employeename
        {
            get { return this.employeename; }
            set { this.employeename = value; }
        }
        public string Job
        {
            get { return this.job; }
            set { this.job = value; }
        }
        public string Tel
        {
            get { return this.tel; }
            set { this.tel = value; }
        }
        public string OldTel
        {
            get { return this.oldtel; }
            set { this.oldtel = value; }
        }



        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public int Createuserid
        {
            get { return this.createuserid; }
            set { this.createuserid = value; }
        }

        public int Employeestate
        {
            get { return this.employeestate; }
            set { this.employeestate = value; }
        }





        public int Com_id
        {
            get { return this.com_id; }
            set { this.com_id = value; }
        }
        public int Viewtel
        {
            get { return this.viewtel; }
            set { this.viewtel = value; }
        }

        public string Accounts
        {
            get { return this.accounts; }
            set { this.accounts = value; }
        }


        public string Passwords
        {
            get { return this.passwords; }
            set { this.passwords = value; }
        }


        public int Atype
        {
            get { return this.atype; }
            set { this.atype = value; }
        }
        public int Peoplelistview
        {
            get { return this.peoplelistview; }
            set { this.peoplelistview = value; }
        }
        public int Isdefaultkf
        {
            get { return this.isdefaultkf; }
            set { this.isdefaultkf = value; }
        }

        
    }
}

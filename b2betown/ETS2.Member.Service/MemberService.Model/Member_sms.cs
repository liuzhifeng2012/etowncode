using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Member.Service.MemberService.Model
{
    public class Member_sms
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private string sms_key;

        public string Sms_key
        {
            get { return sms_key; }
            set { sms_key = value; }
        }
        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        private string remark;

        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        private bool openstate;

        public bool Openstate
        {
            get { return openstate; }
            set { openstate = value; }
        }
        private DateTime subdate;

        public DateTime Subdate
        {
            get { return subdate; }
            set { subdate = value; }
        }
        private string ip;

        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }
    }
}

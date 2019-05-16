using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.Member.Service.MemberService.Model
{
    [Serializable()]
    public class Member_Channel_company
    {

        private int id;
        private int com_id;
        private int issuetype;
        private string companyname = String.Empty;
        private bool whethercreateqrcode = false;
        private int companystate = 1;//默认渠道公司为开通状态
        private int whetherdepartment = 0;//默认 非内部部门
        private string bookurl = "";
        private string companyaddress = String.Empty;
        private string companyphone = String.Empty;
        private decimal companyCoordinate = 0;
        private string companyLocate = "";
        private int companyimg = 0;
        private string companyintro = String.Empty;
        private string companyproject = String.Empty;
        private string city = "";
        private string province = "";
        private int selectstate = 0;
        private int outshop = 0;

        public Member_Channel_company() { }

        public bool Whethercreateqrcode
        {
            get { return this.whethercreateqrcode; }
            set { this.whethercreateqrcode = value; }
        }



        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public int Outshop
        {
            get { return this.outshop; }
            set { this.outshop = value; }
        }

        public int Companystate
        {
            get { return this.companystate; }
            set { this.companystate = value; }
        }

        public int Whetherdepartment
        {
            get { return this.whetherdepartment; }
            set { this.whetherdepartment = value; }
        }


        public string Bookurl
        {
            get { return this.bookurl; }
            set { this.bookurl = value; }
        }


        public int Com_id
        {
            get { return this.com_id; }
            set { this.com_id = value; }
        }


        public int Issuetype
        {
            get { return this.issuetype; }
            set { this.issuetype = value; }
        }


        public string Companyname
        {
            get { return this.companyname; }
            set { this.companyname = value; }
        }


        public string Companyaddress
        {
            get { return this.companyaddress; }
            set { this.companyaddress = value; }
        }

        public string Companyphone
        {
            get { return this.companyphone; }
            set { this.companyphone = value; }
        }

        public string CompanyLocate
        {
            get { return this.companyLocate; }
            set { this.companyLocate = value; }
        }

        public decimal CompanyCoordinate
        {
            get { return this.companyCoordinate; }
            set { this.companyCoordinate = value; }
        }



        public int Companyimg
        {
            get { return this.companyimg; }
            set { this.companyimg = value; }
        }

        public string Companyintro
        {
            get { return this.companyintro; }
            set { this.companyintro = value; }
        }

        public string Companyproject
        {
            get { return this.companyproject; }
            set { this.companyproject = value; }
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
        public int Selectstate
        {
            get { return this.selectstate; }
            set { this.selectstate = value; }
        }
    }
}

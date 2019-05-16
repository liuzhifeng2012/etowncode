using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    [Serializable()]
    public class B2b_company_menu
    {

        private int id;
        private int com_id;
        private int imgurl;
        private string name = String.Empty;
        private string linkurl = String.Empty;
        private int linktype;
        private string imgurl_address = String.Empty;
        private string fonticon = "";
        private int outdata;
        private int usetype;
        private int usestyle;
        private int projectlist;
        private string prolist;
        private int menutype;
        private int channelid;
        public int menuindex { get; set; }
        public int menuviewtype { get; set; }


        public B2b_company_menu() { }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public string Linkurl
        {
            get { return this.linkurl; }
            set { this.linkurl = value; }
        }
        public int Linktype
        {
            get { return this.linktype; }
            set { this.linktype = value; }
        }

        public string Imgurl_address
        {
            get { return this.imgurl_address; }
            set { this.imgurl_address = value; }
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
        public int Imgurl
        {
            get { return this.imgurl; }
            set { this.imgurl = value; }
        }
        public string Fonticon
        {
            get { return this.fonticon; }
            set { this.fonticon = value; }
        }
        public int Outdata
        {
            get { return this.outdata; }
            set { this.outdata = value; }
        }
        public int Usetype
        {
            get { return this.usetype; }
            set { this.usetype = value; }
        }
        public int Usestyle
        {
            get { return this.usestyle; }
            set { this.usestyle = value; }
        }
        public int Projectlist
        {
            get { return this.projectlist; }
            set { this.projectlist = value; }
        }
        public string Prolist
        {
            get { return this.prolist; }
            set { this.prolist = value; }
        }
        public int Menutype
        {
            get { return this.menutype; }
            set { this.menutype = value; }
        }
        public int Channelid
        {
            get { return this.channelid; }
            set { this.channelid = value; }
        }
    }
}

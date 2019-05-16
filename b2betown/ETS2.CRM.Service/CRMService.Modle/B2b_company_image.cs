using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{

      [Serializable()]
    public class B2b_company_image
    {

        private int id;
        private int com_id;
        private int typeid;
        private int imgurl;
        private string title = String.Empty;
        private string linkurl = String.Empty;
        private string imgurl_address = String.Empty;
        private int state;
        private int channelcompanyid;



        public B2b_company_image() { }

        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }
        public string Linkurl
        {
            get { return this.linkurl; }
            set { this.linkurl = value; }
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
           public int Channelcompanyid
        {
            get { return this.channelcompanyid; }
            set { this.channelcompanyid = value; }
        }
        public int State
        {
            get { return this.state; }
            set { this.state = value; }
        }
        public int Com_id
        {
            get { return this.com_id; }
            set { this.com_id = value; }
        }
        public int Typeid
        {
            get { return this.typeid; }
            set { this.typeid = value; }
        }
        public int Imgurl
        {
            get { return this.imgurl; }
            set { this.imgurl = value; }
        }

    }
}

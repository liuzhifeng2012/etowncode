using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
    [Serializable()]
    public class WeiXinBasic
    {

        private int id;
        private int comid;
        private string domain = String.Empty;
        private string url = String.Empty;
        private string token = String.Empty;
        private string appId = String.Empty;
        private string appSecret = String.Empty;
        private string attentionautoreply = String.Empty;
        private string leavemsgautoreply = String.Empty;
        private string weixinno = "";
        private int weixintype;

        private bool whethervertify = false;




        public WeiXinBasic() { }

        public bool Whethervertify
        {
            get { return this.whethervertify; }
            set { this.whethervertify = value; }
        }
        public string Weixinno
        {
            get { return this.weixinno; }
            set { this.weixinno = value; }
        }
        public int Weixintype
        {
            get { return this.weixintype; }
            set { this.weixintype = value; }
        }
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }





        public int Comid
        {
            get { return this.comid; }
            set { this.comid = value; }
        }

        public string Url
        {
            get { return this.url; }
            set { this.url = value; }
        }

        public string Domain
        {
            get { return this.domain; }
            set { this.domain = value; }
        }


        public string Token
        {
            get { return this.token; }
            set { this.token = value; }
        }


        public string AppId
        {
            get { return this.appId; }
            set { this.appId = value; }
        }


        public string AppSecret
        {
            get { return this.appSecret; }
            set { this.appSecret = value; }
        }


        public string Attentionautoreply
        {
            get { return this.attentionautoreply; }
            set { this.attentionautoreply = value; }
        }


        public string Leavemsgautoreply
        {
            get { return this.leavemsgautoreply; }
            set { this.leavemsgautoreply = value; }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
    [Serializable()]
    public class WxMessageLog
    {
        private int comid;
        private int menuid;
        private string weixin = String.Empty;
        private DateTime sendtime;

        public WxMessageLog() { }

        public int Comid
        {
            get { return this.comid; }
            set { this.comid = value; }
        }


        public int Menuid
        {
            get { return this.menuid; }
            set { this.menuid = value; }
        }
        public string Weixin
        {
            get { return this.weixin; }
            set { this.weixin = value; }
        }

        public DateTime Sendtime
        {
            get { return this.sendtime; }
            set { this.sendtime = value; }
        }
    }
}

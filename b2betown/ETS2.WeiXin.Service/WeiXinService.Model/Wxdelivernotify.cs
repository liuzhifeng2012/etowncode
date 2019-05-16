using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
    public class Wxdelivernotify
    {
        public int Id { get; set; }
        public string Out_trade_no { get; set; }
        public string Appid { get; set; }
        public string Openid { get; set; }
        public string Transid { get; set; }
        public string Deliver_timestamp { get; set; }
        public DateTime Timeformat { get; set; }
        public int Deliver_status { get; set; }

        public string Deliver_msg { get; set; }
        public string Requestxml { get; set; }
        public string Responsexml { get; set; }
        public string Errcode { get; set; }
        public string Errmsg { get; set; }
        public int Comid { get; set; }

    }
}

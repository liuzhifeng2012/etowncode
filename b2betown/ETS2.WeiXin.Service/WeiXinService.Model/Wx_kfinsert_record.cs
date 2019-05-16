using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
    public class Wx_kfinsert_record
    {
        private int id;
        public int Id { get; set; }

        private string openid;
        public string Openid { get; set; }

        private int kf_id;
        public int Kf_id { get; set; }

        private string kf_account;
        public string Kf_account { get; set; }

        private int comid;
        public int Comid { get; set; }

        private DateTime lastinserttime;
        public DateTime Lastinserttime { get; set; }
    }
}

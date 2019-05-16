using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
    public class Wx_onlinekf_huoqurecord
    {
        private int id;
        public int Id { get; set; }

        private int comid;
        public int Comid { get; set; }

        private DateTime huoqu_time;
        public DateTime Huoqu_time { get; set; }

        private bool huoqu_issuc;
        public bool Huoqu_issuc { get; set; }

        private string huoqu_content;
        public string Huoqu_content { get; set; }
    }
}

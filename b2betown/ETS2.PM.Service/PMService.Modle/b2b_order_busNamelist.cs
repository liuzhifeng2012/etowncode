using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public  class b2b_order_busNamelist
    {
        public b2b_order_busNamelist() { }

        public int id { get; set; }
        public int orderid { get; set; }
        public string name { get; set; }
        public string IdCard { get; set; }
        public string Nation { get; set; }
        public string yuding_name { get; set; }
        public string yuding_phone { get; set; }
        public DateTime yuding_time { get; set; }
        public int yuding_num { get; set; }
        public DateTime  travel_time { get; set; }
        public int comid { get; set; }
        public int agentid { get; set; }
        public int proid { get; set; }
        public string pickuppoint { get; set; }
        public string dropoffpoint { get; set; }
        public string agentname { get; set; }
        public string orderremark { get; set; }

        public string contactphone { get; set; }
        public string contactremark { get; set; }

        public string pinyin { get; set; }
        public string address { get; set; }
        public string postcode { get; set; }
        public string email { get; set; }
        public int credentialsType { get; set; }
        //乘车人是否上车
        public int isaboard { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class Api_Mjld_SubmitOrder_input
    {
        public Api_Mjld_SubmitOrder_input(){ }
        public int id { get; set; }
        public string timeStamp { get; set; }
        public string user { get; set; }
        public string password { get; set; }
        public string goodsId { get; set; }
        public string num { get; set; }
        public string phone { get; set; }
        public string batch { get; set; }
        public string guest_name { get; set; }
        public string identityno { get; set; }
        public string order_note { get; set; }
        public string forecasttime { get; set; }
        public string consignee { get; set; }
        public string address { get; set; }
        public string zipcode { get; set; }
        public int orderId { get; set; } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class Api_yg_addorder_input
    {
        public Api_yg_addorder_input() { }
        public int id { get; set; }
        public string organization { get; set; }
        public string password { get; set; }
        public string req_seq { get; set; }
        public string product_num { get; set; }
        public int num { get; set; }
        public string mobile { get; set; }
        public string use_date { get; set; }
        public int real_name_type { get; set; }
        public string  real_name { get; set; }
        public string id_card { get; set; }
        public int card_type { get; set; }
        public int orderId { get; set; }
    }
}

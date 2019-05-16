using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    public class Taobao_agent_relation
    {
        public Taobao_agent_relation() { }
        public int serialnum { get; set; }
        public string tb_id { get; set; }
        public string tb_seller_wangwangid { get; set; }
        public string tb_seller_wangwang { get; set; }
        public string tb_shop_name { get; set; }
        public string tb_shop_url { get; set; }
        public int tb_shop_state { get; set; }
        public int agentid { get; set; }
    }
}

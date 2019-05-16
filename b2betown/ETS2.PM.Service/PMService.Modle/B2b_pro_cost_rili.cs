using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    /// <summary>
    /// 商家产品成本日历
    /// </summary>
    [Serializable()]
    public class B2b_pro_cost_rili
    {
        public int id { get; set; }
        public int comid { get; set; }
        public int proid { get; set; }
        public decimal costprice { get; set; }
        public string stardate { get; set; }
        public string enddate { get; set; }
        public string admin { get; set; }

    }

}

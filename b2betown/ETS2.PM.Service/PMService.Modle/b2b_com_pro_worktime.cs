using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    /// <summary>
    /// 商家产品每日工作截止时间
    /// </summary>
    [Serializable()]
    public class b2b_com_pro_worktime
    {
        public int id { get; set; }
        public int comid { get; set; }
        public string title { get; set; }
        public string defaultendtime { get; set; }
        public string defaultstartime { get; set; }
        
    }
}

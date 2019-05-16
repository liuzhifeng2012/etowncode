using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    /// <summary>
    /// 商家产品每日工作时间 特殊时间设置
    /// </summary>
    [Serializable()]
    public class b2b_com_pro_worktime_calendar
    {
        public int id { get; set; }
        public int comid { get; set; }
        public int worktimeid { get; set; }
        public string startime { get; set; }
        public string endtime { get; set; }
        public DateTime setdate { get; set; }
    }
}

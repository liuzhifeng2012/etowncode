using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    /// <summary>
    /// 用户服务验证超时
    /// </summary>
    [Serializable()]
    public class b2b_Rentserver_user_Timeoutmoney
    {
        public int id { get; set; }
        public int comid { get; set; }
        public DateTime subdate { get; set; }
        public DateTime subtime { get; set; }
        public int oid { get; set; }
        public int proid { get; set; }
        public int userid { get; set; }
        public decimal Timeoutmoney { get; set; }
        public int TimeoutMinute { get; set; }
    }
}

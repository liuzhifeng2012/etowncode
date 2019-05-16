using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    /// <summary>
    /// 项目结算
    /// </summary>
    [Serializable()]
    public class B2b_project_finance
    {
        public int id { get; set; }
        public int comid { get; set; }
        public int Projectid { get; set; }
        public decimal Money { get; set; }
        public DateTime Subdate { get; set; }
        public string Remarks { get; set; }
        public string admin { get; set; }

    }

}

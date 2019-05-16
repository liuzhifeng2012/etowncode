using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.CRM.Service.CRMService.Modle
{
    /// <summary>
    /// 商家用户工作时间信息表
    /// </summary>
    [Serializable()]
    public class B2b_company_manageuser_useworktime
    {
         public int id { get; set; }
         public int comid { get; set; }
         public int MasterId { get; set; }
         public DateTime useDate { get; set; }
         public int Hournum { get; set; }
         public int oid { get; set; }
         public string text { get; set; }
    }
}

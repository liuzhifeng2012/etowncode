using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
      [Serializable()]
    public class B2b_order_evaluate
    {
          public int id { get; set; }
           public int comid { get; set; }
           public int oid { get; set; }
           public int uid { get; set; }
           public int channelid { get; set; }
           public int starnum { get; set; }
           public int anonymous { get; set; }
           public int evatype { get; set; }
           public string text { get; set; }
           public DateTime subtime { get; set; }
    }
}

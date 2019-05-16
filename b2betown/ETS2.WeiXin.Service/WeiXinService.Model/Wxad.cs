using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
   public class Wxad
    {
       public int Id { get; set; }
       public int Comid { get; set; }
       public string Title { get; set; }
       public int Adtype { get; set; }
       public string Link { get; set; }
       public string Author { get; set; }
       public string Keyword { get; set; }
       public int Applystate { get; set; }
       public int Votecount { get; set; }
       public int Lookcount { get; set; }
       public int Musicid { get; set; }
    }
}

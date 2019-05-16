using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
    public class Wxqunfa_news_addrecord
    {
        public int id { get; set; }
        public int is_singlenews { get; set; }
        public string type { get; set; }
        public string media_id { get; set; }
        public string created_at { get; set; }
        public DateTime createtime { get; set; }
        public int createuserid { get; set; }
        public int comid { get; set; }

        //public List<Wxqunfa_news> list_Wxqunfa_news { get; set; } 
    }
}

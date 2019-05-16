using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
    public class Wxmedia_updownlog
    {
        public Wxmedia_updownlog() { }
        public int id { get; set; }
        public string mediaid { get; set; }
        public string mediatype { get; set; }
        public string savepath { get; set; }
        public string created_at { get; set; }
        public DateTime  createtime { get; set; }
        public string opertype { get; set; }
        public string operweixin { get; set; }
        public int clientuptypemark { get; set; }
        public int  comid { get; set; }
        public string relativepath{ get; set; }
        public string txtcontent { get; set; }
        public int isfinish { get; set; }
        public string remarks { get; set; }
        public int materialid { get; set; }
    }
}

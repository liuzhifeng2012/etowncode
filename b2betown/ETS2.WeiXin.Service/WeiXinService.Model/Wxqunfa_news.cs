using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WeiXinService.Model
{
    public class Wxqunfa_news
    {
        public int id { get; set; }
        public string thumb_media_id { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        public string content_source_url { get; set; }
        public string content { get; set; }
        public string digest { get; set; }
        public int show_cover_pic { get; set; }
        public int newsrecordid { get; set; }
        public string thumb_url { get; set; }
        public int wxmaterialid { get; set; }
    }
}

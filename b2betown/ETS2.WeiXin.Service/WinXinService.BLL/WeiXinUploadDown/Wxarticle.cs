using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WinXinService.BLL.WeiXinUploadDown
{
    /// <summary>
    /// 微信图文消息素材
    /// </summary>
    public class Wxarticle
    {
        public Wxarticle(){}
        //图文消息缩略图的media_id，可以在基础支持-上传多媒体文件接口中获得
        public string thumb_media_id { get; set; }
        //图文消息的作者
        public string author { get; set; }
        //图文消息的标题
        public string title { get; set; }
        //在图文消息页面点击“阅读原文”后的页面
        public string content_source_url { get; set; }
        //图文消息页面的内容，支持HTML标签
        public string content { get; set; }
        //图文消息的描述
        public string digest { get; set; }
        //是否显示封面，1为显示，0为不显示
        public string show_cover_pic { get; set; }

    }
    public class Wxarticles 
    {
        public List<Wxarticle> articles { get; set; }
    }
}

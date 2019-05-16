using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.WeiXin.Service.WinXinService.BLL.WeiXinUploadDown
{
    /// <summary>
    /// 微信返回消息实体类
    /// </summary>
    public class Wxmessage
    {
        public Wxmessage() { }
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string MsgType { get; set; }
        public string EventName { get; set; }
        public string Content { get; set; }
        public string Recognition { get; set; }
        public string MediaId { get; set; }
        public string EventKey { get; set; }
    }
}

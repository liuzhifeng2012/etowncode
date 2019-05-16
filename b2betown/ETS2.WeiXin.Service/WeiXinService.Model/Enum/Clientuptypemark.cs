using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.WeiXin.Service.WeiXinService.Model.Enum
{
    /// <summary>
    /// 下载 微信客户端上传类型标记
    /// </summary>
    [Flags]
    public enum Clientuptypemark
    { 
        [EnumAttribute("向微信上传多媒体信息")]
        UpMedia = 0,
        [EnumAttribute("下载上传的问候语音")]
        DownGreetVoice = 1,
        [EnumAttribute("下载上传的语音")]
        DownVoice = 2,
        [EnumAttribute("下载上传的图片")]
        DownImg = 3,
        [EnumAttribute("下载文字留言")]
        DownTxt = 4,
        [EnumAttribute("下载上传的文章语音")]
        DownMaterialVoice = 5,
        [EnumAttribute("下载咨询语音")]
        DownConsultVoice = 6,
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
   
    /// <summary>
    /// 电子码发送状态（（0未发送,1已发送，2已下载，3，已重发））
    /// </summary>
    [Flags]
    public enum EticketCodeSendStatus
    {
        [EnumAttribute("未发送")]
        NotSend =0,
        [EnumAttribute("已发送")]
        HasSend = 1,
        [EnumAttribute("已下载")]
        HasDownLoad = 2,
        [EnumAttribute("已重发")]
        HasResend = 3,
    }
}

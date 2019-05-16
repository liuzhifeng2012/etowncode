using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    /// <summary>
    /// 发码状态
    /// </summary>
    [Flags]
    public enum SendCodeStatus
    {

        [EnumAttribute("未发码")]
        NotSend = 0x01,


        [EnumAttribute("已发码")]
        HasSend= 0x02,

        [EnumAttribute("发送中")]
        SendLoading = 0x03,
    }
}

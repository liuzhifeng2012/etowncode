using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    /// <summary>
    /// 电子票扫码方式
    /// </summary>
    [Flags]
    public enum ECodeVerifyMode
    {
        [EnumAttribute("手动输入")]
        HandInput = 0,
        [EnumAttribute("扫二维码")]
        ScanCode = 1,
        [EnumAttribute("后台验证")]
        BackStageVerify = 2,
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    /// <summary>
    /// 电子码类型 （1数字码2二维码3微护照4银河）
    /// </summary>
    [Flags]
    public enum EticketCodeType
    {
        [EnumAttribute("数字码")]
        ShuZiMa = 1,
        [EnumAttribute("二维码")]
        ErWeiMa = 2,
        [EnumAttribute("微护照")]
        WeiHuZhao = 3,
        [EnumAttribute("银河")]
        YinHe = 4,
    }
}

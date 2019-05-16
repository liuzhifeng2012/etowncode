using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    
    /// <summary>
    /// 电子码状态（1未验证，2已验证,3部分验证,4作废，）
    /// </summary>
    [Flags]
    public enum EticketCodeStatus
    {
        [EnumAttribute("未验证")]
        NotValidate = 1,
        [EnumAttribute("已验证")]
        HasValidate = 2,
        [EnumAttribute("部分验证")]
        PartValidate = 3,
        [EnumAttribute("作废")]
        HasZuoFei = 4,
        [EnumAttribute("已过期")]
        HasGuoqi = 5,
    }
}

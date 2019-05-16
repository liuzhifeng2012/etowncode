using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    /// <summary>
    /// 慧择网 与投保人关系
    /// </summary>
     
    [Flags]
    public enum Hzins_relationId
    {
        [EnumAttribute("本人")]
        Benren = 1,
        [EnumAttribute("配偶")]
        Peiou = 2,
        [EnumAttribute("子女")]
        Zinv = 3,
        [EnumAttribute("空")]
        Kong = 4,
        [EnumAttribute("父母")]
        Fumu = 5,
        [EnumAttribute("其他")]
        Qita = 6,
    }
}

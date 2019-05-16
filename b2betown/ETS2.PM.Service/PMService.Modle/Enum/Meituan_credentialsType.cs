using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{ 
    [Flags]
    public enum Meituan_credentialsType
    {
        [EnumAttribute("身份证")]
        Shenfen = 0,
        [EnumAttribute("护照")]
        Huzhao = 1,
        [EnumAttribute("军官证")]
        Junguan = 2,
        [EnumAttribute("台胞证")]
        Taibao = 3,
        [EnumAttribute("港澳证")]
        Gangao = 4, 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    [Flags]
    public enum ProRealNameType
    {
        [EnumAttribute("非实名制")]
        ValidateECode = 0,
        [EnumAttribute("一张一人")]
        ToVoidEcode = 1,
        [EnumAttribute("一单一人")]
        RePrintEcode = 2,
        [EnumAttribute("一单一人+身份证")]
        Reverse = 3,
    }
}

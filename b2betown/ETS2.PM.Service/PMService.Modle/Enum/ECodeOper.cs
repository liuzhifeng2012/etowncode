using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{


    [Flags]
    public enum ECodeOper
    {
        [EnumAttribute("验码")]
        ValidateECode = 1,
        [EnumAttribute("作废")]
        ToVoidEcode = 2,
        [EnumAttribute("重打")]
        RePrintEcode = 3,
        [EnumAttribute("冲正")]
        Reverse = 4,
    }
}

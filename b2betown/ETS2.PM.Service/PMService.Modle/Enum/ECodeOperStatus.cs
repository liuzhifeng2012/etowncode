using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{

    [Flags]
    public enum ECodeOperStatus
    {
        [EnumAttribute("成功")]
        OperSuc = 1,
        [EnumAttribute("失败")]
        OperFail = 2,
        
    }
}

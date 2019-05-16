using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    [Flags]
    public enum Meituan_code
    {
        [EnumAttribute("合作方成功状态")]
        Suc = 200,
        [EnumAttribute("合作方错误统一状态，错误描述可以不同")]
        Fail = 300,
    }
}

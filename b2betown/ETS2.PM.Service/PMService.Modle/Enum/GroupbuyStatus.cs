using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    [Flags]
    public enum GroupbuyStatus
    {
        [EnumAttribute("未上线")]
        NotOnline=0,
        [EnumAttribute("已上线")]
        Online=1,
    }
}

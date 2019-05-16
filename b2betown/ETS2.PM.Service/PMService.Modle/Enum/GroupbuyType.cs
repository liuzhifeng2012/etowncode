using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{ 
    [Flags]
    public enum GroupbuyType
    {
        [EnumAttribute("美团")]
        Meituan = 1,
        
    }
}

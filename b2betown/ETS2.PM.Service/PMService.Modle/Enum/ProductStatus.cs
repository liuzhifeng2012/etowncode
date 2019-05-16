using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
   

    [Flags]
    public enum ProductStatus
    {

        [EnumAttribute("运行")]
        Run = 0x01,


        [EnumAttribute("暂停")]
        Stop = 0x02,
    }
}

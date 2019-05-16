using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{ 
    [Flags]
    public enum Meituan_ProStatus
    {
        [EnumAttribute("合作方产品下架")]
        Xiajia = 0,
        [EnumAttribute("合作方产品上架")]
        Shangjia = 1,
        [EnumAttribute("合作方产品信息发生变化")]
        Change = 2,
        [EnumAttribute("合作方产品价格日历变化")]
        PriceChange = 5,
    }
}

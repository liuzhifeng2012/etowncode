using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.VAS.Service.VASService.Modle.Enum
{
    [Flags] //外部接口产品验证状态
    public enum Product_V_State
    {
        [EnumAttribute("未验证")]
        NotValidate = 0,
        [EnumAttribute("全部验证")]
        AllValidate = 1,
        [EnumAttribute("部分验证")]
        PartValidate = 2,

    }
}

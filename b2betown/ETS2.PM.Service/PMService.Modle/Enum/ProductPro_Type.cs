using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
 

    /// <summary>
    /// 验证类型
    /// </summary>
    [Flags]
    public enum ProductPro_Type
    {

        [EnumAttribute("电子凭证")]
        EticketVerify = 1,
    }
}

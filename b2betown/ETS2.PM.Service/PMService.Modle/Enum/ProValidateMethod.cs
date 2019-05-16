using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    /// <summary>
    /// 产品验证方式
    /// </summary>
    [Flags]
    public enum ProValidateMethod
    {
        [EnumAttribute("按产品有效期")]
        ProYouXiaoData = 1,
        [EnumAttribute("按指定有效期")]
        AppointData = 2,
    }
}

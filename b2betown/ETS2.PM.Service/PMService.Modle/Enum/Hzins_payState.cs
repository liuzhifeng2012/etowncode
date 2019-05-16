using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{ 
    /// <summary>
    /// 慧择网支付状态
    /// </summary>
    [Flags]
    public enum Hzins_payState
    {
        [EnumAttribute("待完成")]
        WaitingComplete = 2,
        [EnumAttribute("未支付")]
        NotPay = 3,
        [EnumAttribute("已支付")]
        HasPay = 4,

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    /// <summary>
    /// 被保人出单状态
    /// </summary>
    [Flags]
    public enum Hzins_issueState
    {
        [EnumAttribute("出单失败")]
        Fail = -1,
        [EnumAttribute("未支付")]
        NotPay = 1,
        [EnumAttribute("待出单")]
        WaitOrder = 10,
        [EnumAttribute("已出单")]
        HasOrder = 20,
        [EnumAttribute("已过期")]
        Overdue = 30,
        [EnumAttribute("退保中")]
        Canceling = 40,
        [EnumAttribute("已退保")]
        HasCancel = 50, 
    }
}

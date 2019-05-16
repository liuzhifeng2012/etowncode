using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    /// <summary>
    /// 被保人生效状态
    /// </summary>
    [Flags]
    public enum Hzins_effectiveState
    {
        [EnumAttribute("未知")]
        Unknown = -1,
        [EnumAttribute("未生效")]
        Noteffect = 1,
        [EnumAttribute("已生效")]
        Haseffect = 2,
        [EnumAttribute("已退保")]
        Hascancel = 3,
        [EnumAttribute("已删除")]
        HasDel = 4,
        [EnumAttribute("待核保")]
        WaitCheck = 11,
        [EnumAttribute("拒保")]
        Refuse = 12,
        [EnumAttribute("核保已通过")]
        Passed = 18,
        [EnumAttribute("退保中")]
        Canceling = 21,
    }
}

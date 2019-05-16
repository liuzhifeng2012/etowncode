using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    /// <summary>
    /// 退票机制:0有效期内可退票 1永久可退票  2不可退票 
    /// </summary>
    [Flags]
    public enum QuitTicketMechanism
    {
        [EnumAttribute("有效期内可退票")]
        QuitInValid = 0,

        [EnumAttribute("有效期内/外 均可退票")]
        QuitInAnyTime = 1,

        [EnumAttribute("不可退票")]
        QuitNoTime = 2,
    }
}

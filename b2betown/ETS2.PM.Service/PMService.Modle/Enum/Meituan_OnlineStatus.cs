using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    //美团产品上单状态
    [Flags]
    public enum Meituan_OnlineStatus
    {
        [EnumAttribute("待上单")]
        WaitLine = 0,
        [EnumAttribute("已上单")]
        Online = 1,
        [EnumAttribute("上单失败")]
        Failline = 2,
        [EnumAttribute("已经下线")]
        Downline = 3,
        [EnumAttribute("未上线")]
        Noline = 4,
      
        
    }
}

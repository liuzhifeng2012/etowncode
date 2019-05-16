using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.VAS.Service.VASService.Modle.Enum
{

    [Flags] //分销商更新结果
    public enum AgentUpdateStatus
    {
        [EnumAttribute("消费成功")]
        Suc = 0,
        [EnumAttribute("消费失败")]
        Fail = 1,


    }
}

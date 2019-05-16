using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.CRM.Service.CRMService.Modle.Enum
{
    [Flags]
    public enum Agentsort
    {
        [EnumAttribute("票务分销")]
        TicketAgent = 0,
        [EnumAttribute("微站渠道")]
        MicroStation = 1,
        [EnumAttribute("商家项目账户(查看验证数据)")]
        ProjectAccount = 2,
    }
}

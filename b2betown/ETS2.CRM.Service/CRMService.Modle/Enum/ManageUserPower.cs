using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.CRM.Service.CRMService.Modle.Enum
{
 
    [Flags]
    public enum ManageUserPower
    {

        [EnumAttribute("管理员")]
        Admin = 1,

        [EnumAttribute("验票员")]
        Ticket = 2,

        [EnumAttribute("经理")]
        Manager = 3,
        
    }
}

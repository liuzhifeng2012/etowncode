using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.CRM.Service.CRMService.Modle.Enum
{
    [Flags]
    public enum AgentCompanyType
    {
        [EnumAttribute("一般分销")]
        Common = 1,
        [EnumAttribute("糯米分销")]
        NuoMi = 2,
        [EnumAttribute("驴妈妈")]
        Lvmama =3,

    }
}

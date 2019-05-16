using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.CRM.Service.CRMService.Modle.Enum
{

    [Flags]
    public enum CrmInterestType
    {
        [EnumAttribute("旅游目的地")]
        TourismDestination = 0,

        [EnumAttribute("年龄段")]
        AgeGroup = 1,

        [EnumAttribute("收入段")]
        IncomeGroup = 2,


    }
}

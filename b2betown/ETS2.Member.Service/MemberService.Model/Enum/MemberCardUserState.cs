using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ETS.Framework;

namespace ETS2.Member.Service.MemberService.Model.Enum
{

    [Flags]
    public enum MemberCardUserState
    {

        [EnumAttribute("未使用")]
        NotUse = 1,

        [EnumAttribute("已使用")]
        HasUsed = 2,

        [EnumAttribute("部分使用")]
        PartUsed = 3,
        [EnumAttribute("作废此次活动")]
        Cancel = 4,
    }
}

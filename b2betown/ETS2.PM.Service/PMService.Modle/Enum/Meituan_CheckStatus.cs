using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    //美团产品上单审核状态
    [Flags]
    public enum Meituan_CheckStatus
    {
        [EnumAttribute("待审核")]
        WailCheck = 0,
        [EnumAttribute("已通过")]
        Checked = 1,
        [EnumAttribute("已驳回")]
        Rebut = 2,
        [EnumAttribute("免审核")]
        IgnoreCheck = 3,
        [EnumAttribute("黑名单")]
        BlackList = 4,
        [EnumAttribute("未提交审核")]
        NoSubCheck = 5,
    }
}

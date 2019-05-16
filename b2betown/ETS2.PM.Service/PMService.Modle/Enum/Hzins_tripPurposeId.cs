using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    /// <summary>
    /// 慧择网 出行目的
    /// </summary> 
     [Flags]
    public enum Hzins_tripPurposeId
    {
        [EnumAttribute("旅游")]
        Lvyou = 1,
        [EnumAttribute("商务")]
        Shangwu = 2,
        [EnumAttribute("探亲")]
        Tanqin = 3,
        [EnumAttribute("留学")]
        Liuxue = 4,
        [EnumAttribute("务工")]
        Wugong = 5,
        [EnumAttribute("其他")]
        Qita = 6,
    }
}

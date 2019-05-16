using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    /// <summary>
    /// 慧择网 被保险人证件类型
    /// </summary> 
    [Flags]
    public enum Hzins_cardType
    {
        [EnumAttribute("身份证")]
        Shenfen = 1,
        [EnumAttribute("军官证")]
        Junguan = 2,
        [EnumAttribute("护照")]
        Huzhao = 3,
        [EnumAttribute("港澳通行证")]
        Gangao = 4,
        [EnumAttribute("驾照")]
        Jiazhao = 5,
        [EnumAttribute("空")]
        Kong = 6,
        [EnumAttribute("台胞证")]
        Taibao = 7,
        [EnumAttribute("出生证")]
        Chusheng = 8,
        [EnumAttribute("其他")]
        Qita =9,

    }
}

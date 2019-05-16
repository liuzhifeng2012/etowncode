using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    /// <summary>
    /// 使用限制(1.当天出票可用；0.当天出票不可用；2.2小时内出票不可用；)
    /// </summary>
    [Flags]
    public enum Iscanuseonsameday
    {
        [EnumAttribute("2小时内出票不可用")]
        TwoHours_NotCanUse = 2,

        [EnumAttribute("当天出票不可用")]
        SameDay_CanUse = 0,

        [EnumAttribute("当天出票可用")]
        SameDay_NotCanUse = 1,
     
     
       
    }
   
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    /// <summary>
    /// 产品制定日期方式
    /// </summary>
    [Flags]
    public enum ProAppointdata
    {
        [EnumAttribute("未指定")]
        NotAppoint = 0,
        [EnumAttribute("一星期")]
        OneWeek = 1,
        [EnumAttribute("一个月")]
        OneMonth = 2,
        [EnumAttribute("三个月")]
        ThreeMonth = 3,
        [EnumAttribute("六个月")]
        SixMonth = 4,
        [EnumAttribute("一 年")]
        OneYear = 5,
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
 

    /// <summary>
    /// 产品来源类型
    /// </summary>
    [Flags]
    public enum ProductSource_Type
    {

        [EnumAttribute("系统自动生成")]
        SysCreateCode = 1,


        [EnumAttribute("从外部倒码")]
        ImportOutCode = 2,


        [EnumAttribute("外来产品接口")]
        OutProductCode = 3,

        [EnumAttribute("系统内商户产品接口")]
        InProductCode = 4,
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    [Flags]
    public enum Meituan_voucherType
    {
        //[EnumAttribute("短信验证码")]
        //Message = 0,
        //[EnumAttribute("二维码")]
        //Qrcode = 1,

        //1	商家一码一验码_不绑定身份（一码一验：一个券一个码）
        //2	商家一码多验码（一码多验，一个订单一个码，码只能验一次或码可以验多次）
        [EnumAttribute("一码一验")]
        singleuse = 1,
        [EnumAttribute("一码多验")]
        multiuse = 2,
    }
}

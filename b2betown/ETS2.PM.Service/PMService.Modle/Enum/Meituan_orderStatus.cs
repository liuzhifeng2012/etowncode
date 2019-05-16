using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    [Flags]
    public enum Meituan_orderStatus
    {
        //2	创建订单成功
        //3	创建订单失败
        //4	支付成功
        //5	支付中 不能明确支付状态时使用该状态
        //6	支付失败
        [EnumAttribute("创建订单成功")]
        CreateSuc = 2,
        [EnumAttribute("创建订单失败")]
        CreateFail = 3,
        [EnumAttribute("支付订单成功")]
        PaySuc = 4,
        [EnumAttribute("支付中 不能明确支付状态时使用该状态")]
        Paying = 5,
        [EnumAttribute("支付失败")]
        PayFailed = 6
    }
}

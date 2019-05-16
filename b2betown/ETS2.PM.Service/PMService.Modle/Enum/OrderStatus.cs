using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    /// <summary>
    /// 订单状态
    /// </summary>
    [Flags]
    public enum OrderStatus
    {

        [EnumAttribute("等待对方付款")]
        WaitPay = 1,
        [EnumAttribute("已付款")]
        HasPay = 2,
        [EnumAttribute("已发码")]
        HasSendCode = 4,
        [EnumAttribute("已充值")]
        HasRecharge = 6,
        [EnumAttribute("已消费")]
        HasUsed = 8,
        [EnumAttribute("订单退票")]
        QuitOrder = 16,
        [EnumAttribute("申请退票中")]
        WaitQuitOrder = 17,
        [EnumAttribute("退票处理中")]
        HandleQuitOrder = 18,
        [EnumAttribute("作废")]
        InvalidOrder = 19,
        [EnumAttribute("发码出错")]
        SendCodeErr = 20,
        [EnumAttribute("重新发码出错")]
        RepeatSendCodeErr = 21,
        [EnumAttribute("已处理")]
        HasFin = 22,
        [EnumAttribute("超时订单已取消")]
        Timeout = 23,
        [EnumAttribute("订房取消")]
        Hotecannel = 24,
        [EnumAttribute("已过期")]
        Olddate = 25
    }
}

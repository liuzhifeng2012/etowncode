using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    /// <summary>
    /// 酒店预订类型
    /// </summary>
    [Flags]
    public enum Hotel_ReserveType
    {
        [EnumAttribute("不用支付直接发送预订短信")]
        NotNeedPay = 1,
        [EnumAttribute("预付发送短信")]
        NeedPaySendSms = 2,
        [EnumAttribute("预付发送电子码")]
        NeedPaySendEticket = 3,
    }
}

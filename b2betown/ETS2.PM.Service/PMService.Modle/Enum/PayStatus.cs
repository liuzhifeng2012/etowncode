using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    /// <summary>
    /// 支付状态
    /// </summary>
    [Flags] 
   public enum PayStatus
    {
       
        [EnumAttribute("等待确认订单，未支付")]
        WaitPay = 0,

        [EnumAttribute("未支付")]
        NotPay = 1,
      
        [EnumAttribute("已支付")]
        HasPay = 2,
    }
}

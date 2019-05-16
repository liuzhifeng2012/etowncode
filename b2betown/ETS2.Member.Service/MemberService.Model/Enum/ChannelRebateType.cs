using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.Member.Service.MemberService.Model.Enum
{
    /// <summary>
    /// 渠道返佣类型
    /// </summary>
  
    [Flags]
    public enum ChannelRebateType
    {

        [EnumAttribute("开卡")]
        OpenCard = 1,

        [EnumAttribute("第一次消费")]
        FirstDeal = 2,

    
    }
}

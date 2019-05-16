using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.WeiXin.Service.WeiXinService.Model.Enum
{
    /// <summary>
    /// 微信模板消息类型
    /// </summary>
    [Flags]
    public enum WxTmplType
    {
        [EnumAttribute("新订单生成通知")]
        OrderNewCreate = 1,

        [EnumAttribute("订单状态变更通知")]
        OrderStatusChange = 2,

        [EnumAttribute("门票订单预订成功通知")]
        OrderSendCodeSuc = 3,

        [EnumAttribute("酒店预订确认通知")]
        OrderHotelConfirm = 4,

        [EnumAttribute("会员充值通知")]
        CrmRecharge = 5,

        [EnumAttribute("会员消费通知 ")]
        CrmConsume = 6,

        [EnumAttribute("积分奖励提醒 ")]
        CrmIntegralReward = 7,

        [EnumAttribute("订阅活动开始提醒 ")]
        SubscribeActReward = 8,
    }
}

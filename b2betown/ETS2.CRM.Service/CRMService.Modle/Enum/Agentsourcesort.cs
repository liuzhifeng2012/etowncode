using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.CRM.Service.CRMService.Modle.Enum
{
    [Flags]
    public enum Agentsourcesort
    {
        [EnumAttribute("类 别")]
        NotSeled = 0,

        [EnumAttribute("在线旅游(OTA)")]
        OTA = 3,
        [EnumAttribute("团购网")]
        GroupPurchase = 4,
        [EnumAttribute("平台电商")]
        Platform = 5,
        [EnumAttribute("垂直网站及社区论坛")]
        VerticalWebsite = 6,
        [EnumAttribute("票务系统(公司)")]
        TicketSystem = 7,
        [EnumAttribute("旅行社")]
        TravelAgency = 8,
        [EnumAttribute("酒店服务")]
        HotelService = 9,
        [EnumAttribute("俱乐部")]
        Club = 10,
        [EnumAttribute("户外实体店")]
        OutdoorStore = 11,
        [EnumAttribute("礼品特产专卖店")]
        GiftShop = 12,
        [EnumAttribute("会议公司")]
        MeetingCompany = 13,
        [EnumAttribute("其 他")]
        Others = 14,
    }
}

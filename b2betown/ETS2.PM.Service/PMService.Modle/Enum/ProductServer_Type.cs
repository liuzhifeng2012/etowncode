using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{

    /// <summary>
    /// 服务类型
    /// </summary>
    [Flags]
    public enum ProductServer_Type
    {

        [EnumAttribute("电子凭证")]
        Eticket = 1,
        [EnumAttribute("跟团游")]
        TourGroup = 2,
        [EnumAttribute("当地游")]
        LocalTour = 8,
        [EnumAttribute("酒店客房")]
        HotelRooms = 9,
        [EnumAttribute("旅游大巴")]
        LvyouBus = 10,
        [EnumAttribute("实物产品")]
        PhysicalPro = 11,
        [EnumAttribute("预订服务")]
        YudingPro = 12,
        [EnumAttribute("教练预约")]
        CoachYudingPro = 13,
        [EnumAttribute("保险产品")]
        Baoxian = 14,
    }
}

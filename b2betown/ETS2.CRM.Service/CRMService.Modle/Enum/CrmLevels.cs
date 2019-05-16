using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.CRM.Service.CRMService.Modle.Enum
{
    [Flags]//只有导入的会员才有会员级别，网站/微信过来的会员没有级别
    public enum CrmLevels
    {
        [EnumAttribute("网站注册会员")]
        WebsiteReg = 0,

        [EnumAttribute("微信一般会员")]
        WechatGeneral = 1,

        [EnumAttribute("微信高级会员:含有手机或者邮箱")]
        WechatHigh = 2,

        [EnumAttribute("VIP会员")]
        Vip = 3,
    }
}

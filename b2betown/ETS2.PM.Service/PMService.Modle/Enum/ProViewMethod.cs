using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.PM.Service.PMService.Modle.Enum
{
    /// <summary>
    /// 产品展示方式
    /// </summary>
    [Flags]
    public enum ProViewMethod
    {
        [EnumAttribute("全部显示")]
        AllView = 1,
        [EnumAttribute("分销后台显示")]
        AgentView = 2,
        [EnumAttribute("官方微信显示")]
        WeixinView = 3,
        [EnumAttribute("官方网站显示")]
        WebsitView = 4,
        [EnumAttribute("微信和官网显示")]
        WxWebsitView = 5,

    }
}

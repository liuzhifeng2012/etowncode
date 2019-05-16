using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.CRM.Service.CRMService.Modle.Enum
{

    [Flags]
    public enum ModelStyle
    {

        [EnumAttribute("易城默认模板")]
        DefaultModel = 1,

        [EnumAttribute("使用编辑器上传自己头部模板和页面底部模板")]
        SelfModel = 2,


    }
}

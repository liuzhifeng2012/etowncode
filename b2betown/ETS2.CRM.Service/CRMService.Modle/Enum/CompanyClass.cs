using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
using System.ComponentModel;
using ETS.Framework;

namespace ETS2.CRM.Service.CRMService.Modle.Enum
{

    [Flags]
    public enum CompanyClass
    {
        /// <summary>
        /// 全部
        /// </summary>      
        [EnumAttribute("全部")]
        All = 0x01,
        /// <summary>
        /// 旅游
        /// </summary> 
        [EnumAttribute("旅游")]
        Traval = 0x02,
        /// <summary>
        /// 票务
        /// </summary> 
        [EnumAttribute("票务")]
        Ticket = 0x03,
    }
}

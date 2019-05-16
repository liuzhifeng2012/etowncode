using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
using System.ComponentModel;
using ETS.Framework;

namespace ETS2.CRM.Service.CRMService.Modle.Enum
{
    [Flags]
    public enum CompanyType
    {
        /// <summary>
        /// 全部
        /// </summary>      
        [EnumAttribute("全部")]
        All = 0x01,
        /// <summary>
        /// 供应商
        /// </summary> 
        [EnumAttribute("供应商")]
        Privider = 0x02,
        /// <summary>
        /// 分销商
        /// </summary> 
        [EnumAttribute("分销商")]
        Sales = 0x04,
    }
}

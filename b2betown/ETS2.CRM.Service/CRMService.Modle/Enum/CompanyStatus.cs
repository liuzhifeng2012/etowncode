using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using ETS.Framework;

namespace ETS2.CRM.Service.CRMService.Modle.Enum
{

    [Flags]
    public enum CompanyStatus
    {

        /// <summary>
        /// 正常营业
        /// </summary>      
        [EnumAttribute("正常营业")]
        InBusiness = 0x01,
        /// <summary>
        /// 暂停营业
        /// </summary> 
        [EnumAttribute("暂停营业")]
        SuspensionOfBusiness = 0x02,
        /// <summary>
        /// 终止合约
        /// </summary> 
        [EnumAttribute("终止合约")]
        CancelOfBusiness = 0x04,
    }
}

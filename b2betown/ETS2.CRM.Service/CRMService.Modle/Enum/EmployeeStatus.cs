using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.CRM.Service.CRMService.Modle.Enum
{
  
    [Flags]
    public enum EmployeeStatus
    {
         
        [EnumAttribute("开通")]
        Open = 0x01,
        
        [EnumAttribute("关闭")]
        Close = 0x02,
        
    }
}

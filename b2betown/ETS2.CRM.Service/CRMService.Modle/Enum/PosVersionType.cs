using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace ETS2.CRM.Service.CRMService.Modle.Enum
{
     [Flags]
    public enum PosVersionType
    {
         
        [EnumAttribute("不需要更新")]
        NotRenew = 1,
        
        [EnumAttribute("exe更新")]
        ExeRenew = 2,
       
        [EnumAttribute("xml更新")]
        XmlRenew = 3,
          [EnumAttribute("exe和xml同时更新")]
        AllRenew = 4 ,
    }
}

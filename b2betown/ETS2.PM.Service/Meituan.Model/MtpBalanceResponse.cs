using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    public class MtpBalanceResponse
    {
        public int code { get; set; }
        public string describe { get; set; }
        public int partnerId { get; set; }
        public List<MtpBalanceResponseBody> body { get; set; }
    }
    public class MtpBalanceResponseBody 
    {
        //required	预付款账户余额	单位为分, 非预付款商家, 此值返回-1不要传0
        public int prepaidAccountBalance { get; set; }
        //授信账户余额	单位为分, 无授信账户, 此值返回-1不要传0
        public int creditAccountBalance { get; set; }
        //合作方产品id
        public string partnerDealId { get; set; }
    }
}

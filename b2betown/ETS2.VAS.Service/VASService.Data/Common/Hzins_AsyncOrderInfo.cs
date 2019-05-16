using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.VAS.Service.VASService.Data.Common
{
    /// <summary>
    /// 慧择网 异步 出单通知
    /// </summary>
    public class Hzins_AsyncOrderInfo
    {
        public string insureNum { get; set; }
        public int partnerId { get; set; }
        public int resultCode { get; set; }
        public List<AsyncOrderInfo_policyList> policyList { get; set; }
    }
    public class AsyncOrderInfo_policyList 
    {
        public int productId { get; set; }
        public string productName { get; set; }
        public int planId { get; set; }
        public string planName { get; set; }
        public string applicant { get; set; }
        public string insurant { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int totalnum { get; set; }
        public int issueState { get; set; }
        public string signKey { get; set; }
    }
}

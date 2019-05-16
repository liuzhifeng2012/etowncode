using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.VAS.Service.VASService.Data.Common
{
    public class Hzins_SearchInsureResp
    {
        public int respCode { get; set; }
        public string respMsg { get; set; }
        public Hzins_SearchInsureResp_data data { get; set; }
    }
    public class Hzins_SearchInsureResp_data
    {
        public int partnerId { get; set; }
        public string transNo { get; set; } 
        public List<Hzins_SearchInsureResp_OrderDetailInfo> orderDetailInfos { get; set; }
    }
    public class Hzins_SearchInsureResp_OrderDetailInfo
    {
        public string insureNum { get; set; }
        public int payState { get; set; }
        public int issueState { get; set; }
        public int effectiveState { get; set; }
        public DateTime insureTime { get; set; }
        public int totalNum { get; set; }
        public decimal price { get; set; }
        public string planName { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string deadline { get; set; }
        public string companyName { get; set; }
        public string applicant { get; set; }
        public string insurant { get; set; }
        //生效状态描述
        public string effectiveStateStr { get; set; }
    }
}

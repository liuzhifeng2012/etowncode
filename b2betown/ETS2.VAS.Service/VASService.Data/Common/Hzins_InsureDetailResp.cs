using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.VAS.Service.VASService.Data.Common
{

    public class Hzins_InsureDetailResp
    {
        public int respCode { get; set; }
        public string respMsg { get; set; }
        public Hzins_InsureDetailResp_data data { get; set; }
    }
    public class Hzins_InsureDetailResp_data
    {
        public ApplicantInfo applicantInfo { get; set; }
        public List<InsurantInfos> insurantInfos { get; set; }
        public PolicyDetailInfo policyDetailInfo { get; set; }

        public string contact { get; set; }
        public string contactMob { get; set; }
        public string companyName { get; set; }
        public string prodName { get; set; }
        public string transNo { get; set; }
        public string partnerId { get; set; }
        public string insureNum { get; set; }
    }
    public class ApplicantInfo
    {
        public string cName { get; set; }
        public string eName { get; set; }
        public int cardType { get; set; }
        public string cardCode { get; set; }
        public int sex { get; set; }
        public string birthday { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
    }
    public class InsurantInfos
    {
        public string cName { get; set; }
        public string eName { get; set; }
        public int cardType { get; set; }
        public string cardCode { get; set; }
        public int sex { get; set; }
        public string birthday { get; set; }
        public int tripPurposeId { get; set; }
        public int relationId { get; set; }
        public int count { get; set; }
        public string mobile { get; set; }
        public string singlePrice { get; set; }
    }
    public class PolicyDetailInfo
    {
        public int totalNum { get; set; }
        public string deallineText { get; set; }
        public string buySinglePrice { get; set; }
        public string settlementPrice { get; set; }

    }
}

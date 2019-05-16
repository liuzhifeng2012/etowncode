using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    /// <summary>
    /// 被保险人信息
    /// </summary>
    public class Api_hzins_OrderApplyReq_insurantInfo
    {
        public Api_hzins_OrderApplyReq_insurantInfo() { }
        public int id { get; set; }
        public string insurantId { get; set; }
        public string cName { get; set; }
        public string eName { get; set; }
        public int sex { get; set; }
        public int  cardType{get;set;}
        public string cardCode{get;set;}
        public string birthday{get;set;}
        public int relationId{get;set;}
        public int count{get;set;}
        public decimal singlePrice{get;set;}
        public string fltNo{get;set;}
        public string fltDate{get;set;}
        public string city{get;set;}
        public int tripPurposeId{get;set;}
        public string destination{get;set;}
        public string visaCity{get;set;}
        public string jobInfo{get;set;}
        public string mobile{get;set;}
        public int orderid{get;set;}
    }
}

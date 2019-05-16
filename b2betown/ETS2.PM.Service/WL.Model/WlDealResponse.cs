using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.WL.Model
{
     public class WlDealResponse
    {
         public int code { get; set; }
         public string describe { get; set; }
         public int partnerId { get; set; }


        public List<WlDealResponseBody> body { get; set; }
    }
     public class WlDealResponseBody
     {
         public WlDealResponseBody() { }

         public int id { get; set; }
         public int comid { get; set; }
         public DateTime subtime { get; set; }

         public int useDateMode { get; set; }
         public int visitorInfoType { get; set; }
         public UserInfoRule visitorInfoRule { get; set; }

         public string proID { get; set; }
         public string scheduleOnlineTime { get; set; }
         public string scheduleOfflineTime { get; set; }

         public List<ServicePhone> servicePhones { get; set; }

         //public string[] getInAddresses { get; set; }
         public bool needTicket { get; set; }

         public GetTicketRule getTicketRule { get; set; }

         public string[] ticketGetAddress { get; set; }

         public int orderCancelTime { get; set; }
         public string otherNote { get; set; }
         public Int64 stock { get; set; }
         public double marketPrice { get; set; }
         public double wlPrice { get; set; }
         public double settlementPrice { get; set; }

         public int voucherTimeBegin { get; set; }
         public int voucherTimeEnd { get; set; }
         public int minBuyCount { get; set; }

         public string title { get; set; }
         public string subTitle { get; set; }
         public string include { get; set; }

         public string exclude { get; set; }

         public string partnerId { get; set; }
         public List<DealImageInfo> dealImageInfos { get; set; }
         public string voucherDateBegin { get; set; }

         public string voucherDateEnd { get; set; }
         public int stockMode { get; set; }
         public int state { get; set; }
         
        
         
         
         
     }

     public class UserInfoRule
     {
         public UserInfoRule() { }
         public bool name { get; set; }
         public bool pinyin { get; set; }
         public bool mobile { get; set; }
         public bool address { get; set; }
         public bool email { get; set; }
         public bool hkmlp { get; set; }
         public bool tlp { get; set; }
         public bool mtp { get; set; }
         public bool passport { get; set; }
         public bool credentials { get; set; }

     }
     public class ServicePhone
     {
         public ServicePhone() { }
         public string phone { get; set; }
         public string startHour { get; set; }
         public string startMin { get; set; }
         public string endHour { get; set; }
         public string endMin { get; set; }
     }
     public class GetTicketRule
     {
         public GetTicketRule() { }
         public bool effectiveCertificate { get; set; }
         public int voucherLoaders { get; set; }
         public bool needCertificateSupplement { get; set; }
         public string certificateSupplement { get; set; }
         


     }
     public class DealImageInfo
     {
         public DealImageInfo() { }
         public string imageName { get; set; }
         public string imageUrl { get; set; }
         public bool frontImage { get; set; }
     }

}

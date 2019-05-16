using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.WL.Model
{
    public class wlOrderCreateRequest
    {
        public string partnerId { get; set; }
        public wlOrderCreateRequestBody body { get; set; }
    }

   


    public class wlOrderCreateRequestBody
     {
         public wlOrderCreateRequestBody() { }
         public int id { get; set; }
         public int comid { get; set; }
         public int OrderId { get; set; }
         public string partnerOrderId { get; set; }
         public int voucherLoaders { get; set; }
         public bool needCertificateSupplement { get; set; }
         public string certificateSupplement { get; set; }
         public string partnerDealId { get; set; }
         public string wlDealId { get; set; }
         public double buyPrice { get; set; }
         public double unitPrice { get; set; }
         public double totalPrice { get; set; }
         public int quantity { get; set; }
         public string travelDate { get; set; }

         public wlPerson contactPerson { get; set; }
         public List<wlVisitor> visitors { get; set; }
         
     }
    public class wlPerson
    {
        public wlPerson() { }
        public string name { get; set; }
        public string mobile { get; set; }
    }
    public class wlVisitor
    {
        public wlVisitor() { }
        public string name { get; set; }
        public string mobile { get; set; }
    }


}

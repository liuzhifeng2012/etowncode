using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.WL.Model
{
   public  class wlOrderPayRequest
    {
       public string partnerId { get; set; }
       public wlOrderPayRequestBody body { get; set; }
    }

   public class wlOrderPayRequestBody
   {
       public wlOrderPayRequestBody() { }
       public string wlOrderId { get; set; }
       public string partnerOrderId { get; set; }
   }
}

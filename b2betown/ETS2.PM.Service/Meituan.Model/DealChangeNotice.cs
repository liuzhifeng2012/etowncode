using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    public class DealChangeNotice
    {
        public DealChangeNotice() { }
         
        public string partnerId { get; set; }
        public List<DealChangeNoticeBody> body { get; set; }
    }
    public class DealChangeNoticeBody 
    {
        public DealChangeNoticeBody() { }
        public string partnerDealId { get; set; }
        public int status { get; set; }
        
    }
}

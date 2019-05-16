using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    public class Api_hzins_orderCancel
    {
        public Api_hzins_orderCancel() { }
        public int id { get; set; }
        public int orderid { get; set; }
        public string insureNo { get; set; }//投保单号
        public int respCode { get; set; }
        public string respMsg { get; set; }
    }
}

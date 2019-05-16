using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    /// <summary>
    /// 投保人信息
    /// </summary>
    public class Api_hzins_OrderApplyReq_applicantInfo
    {
        public Api_hzins_OrderApplyReq_applicantInfo() { }
        public int id { get; set; }
        public string cName { get; set; }
        public string eName { get; set; }
        public int cardType { get; set; }
        public string cardCode { get; set; }
        public int sex { get; set; }
        public string birthday { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string jobInfo { get; set; }
        public int orderid { get; set; }
    }
}

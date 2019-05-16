using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.VAS.Service.VASService.Data.Common
{
    /// <summary>
    /// 慧择网 同步返回信息
    /// </summary>
    public class Hzins_OrderApplyResp
    {
        public int respCode { get; set; }
        public string respMsg { get; set; }
        public OrderApplyResp_data data { get; set; }
    }
    public class OrderApplyResp_data
    {
        public string transNo { get; set; }
        public int partnerId { get; set; }
        public string insureNum { get; set; }
        public List<OrderApplyResp_orderInfos> orderInfos { get; set; }
        public List<OrderApplyResp_orderExts> orderExts { get; set; }
    }
    public class OrderApplyResp_orderInfos
    {
        public string insureNum { get; set; }
        public string policyNum { get; set; }
        public string cName { get; set; }
        public string cardCode { get; set; }
    }
    public class OrderApplyResp_orderExts
    {
        public string insureNum { get; set; }
        public List<string> insurantIds { get; set; }
        public int insurantCount { get; set; }
        public decimal priceTotal { get; set; }
    }

}

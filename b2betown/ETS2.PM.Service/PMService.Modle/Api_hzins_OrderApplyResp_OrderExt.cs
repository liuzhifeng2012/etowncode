using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    /// <summary>
    /// 保单与被保人关联信息  
    /// </summary>
    public class Api_hzins_OrderApplyResp_OrderExt
    {
        public Api_hzins_OrderApplyResp_OrderExt() { }
        public int id { get; set; }
        public int orderid { get; set; }
        public string insureNum { get; set; }//投保单号
        public string insurantIds { get; set; }//被保险人id集合
        public decimal priceTotal { get; set; }//该投保单总价格
        public int insurantCount { get; set; } //被保人数量
    }
}

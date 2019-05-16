using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.PMService.Modle
{
    /// <summary>
    /// 保单列表
    /// </summary>
    public class Api_hzins_OrderApplyResp_OrderInfo
    {
        public Api_hzins_OrderApplyResp_OrderInfo() { }
        public int id { get; set; }
        public int orderid { get; set; }
        public string insureNum { get; set; }//投保单号
        public string policyNum { get; set; }//保险公司保单号
        public string cName { get; set; }//被保人姓名
        public string cardCode { get; set; }//被保人证件号码
        public int issueState { get; set; }//出单状态
    }
}

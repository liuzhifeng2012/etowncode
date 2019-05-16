using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    public class MtpDealSendNoticeRequest
    {
        public int partnerId { get; set; }
        public List<MtpDealSendNoticeRequestBody> body { get; set; }
    }
    public class MtpDealSendNoticeRequestBody 
    {
        //required	合作方产品ID
        public string partnerDealId { get; set; }
        //required 产品上线状态	0 待上单 这个是审核通过后的中间状态， 这个状态的单子会等待被上线处理程序处理。成功 ONLINE 失败后则置为FAIL1 已上单 2 上单失败3 已经下线4 未上线 这个是新单子的默认状态
        public int status { get; set; }
        //required 产品审核状态 0 待审核1 已通过2 已驳回3 免审核4 黑名单5 未提交审核9 放弃该任务, 这个状态不会出现在partner_deal check_status中
        public int checkStatus { get; set; }
        //required 产品费率审核状态 0 待审核1 已通过2 已驳回3 免审核4 黑名单5 未提交审核
        public int msRatioCheckStatus { get; set; }
    }
}

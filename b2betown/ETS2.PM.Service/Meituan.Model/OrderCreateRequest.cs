using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
   // 支付前创建订单 Requst
    public class OrderCreateRequest
    {
        public OrderCreateRequest(){}
        public int partnerId { get; set; }
        public OrderCreateRequestBody body { get; set; }
    }
    public class OrderCreateRequestBody
    {
        public OrderCreateRequestBody(){}
        //required	美团订单Id
        public Int64 orderId { get; set; }
        //required	第三方dealId
        public string partnerDealId { get; set; }
        //required	美团DealId
        public Int64 mtDealId { get; set; }
        //required	商品结算单价	单位元
        public double buyPrice { get; set; }
        //required	商品单价(美团售价)	单位元
        public double unitPrice { get; set; }
        //required	订单总金额	单位元
        public double totalPrice{get;set;}
        //required	购买数量
        public int quantity{get;set;}
        //optional	游玩人列表	不需要填写的时候有可能为空
        public List<BaseVisitor> visitors{get;set;}
        //optional	预约游玩日期	格式yyyy-MM-dd, 有效期模式下可为空
        public string travelDate{get;set;}
    }
    public class BaseVisitor
    {
      public BaseVisitor(){}
      //optional	游客姓名	
      public string name { get; set; }
      //optional	游客姓名拼音
      public string pinyin { get; set; }
      //optional	游客手机号
      public string mobile { get; set; }
      //optional	地址
      public string address { get; set; }
      //optional	邮编
      public string postCode { get; set; }
      //optional	邮箱
      public string email { get; set; }
      /*****************************************
       * optional	证件号码	json 对象,属性key是证件类型, value 是证件号码 如 {0=xxxxx} 表示, 身份证号为 'xxxxx', 证件类型 请参考 证件类型映射表
        0	身份证
        1	护照
        2	军官证
        3	台胞证
        4	港澳通行证
        5	大陆居民往来台湾通行证
       * *****************************************/
      public object credentials { get; set; }
      //optional	性别 1	男2	女
      public int gender { get; set; }
    }
}

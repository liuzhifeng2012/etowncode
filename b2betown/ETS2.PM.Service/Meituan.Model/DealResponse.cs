using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETS2.PM.Service.Meituan.Model
{
    /// <summary>
    /// Push Deal 请求
    /// </summary>
    public class DealResponse
    {
        public DealResponse() { }
        public int code { get; set; }
        public string describe { get; set; }
        public int partnerId { get; set; }
        public int totalSize { get; set; }
        public List<DealResponseBody> body { get; set; }
    }
    /// <summary>
    /// 封装拉取或者推送到美团的Deal信息
    /// </summary>
    public class DealResponseBody
    {
        public DealResponseBody() { }

        //required	指定产品预约有效期	 0: 不需预约的期票形式, n(n>0) : 预约日期n天内有效
        public int useDateMode { get; set; }
        // required	游玩人信息类型 1	不需要游客信息2	只需要一位游客信息3	需要每一位游客信息4	每几个人需要一位游客信息
        public int visitorInfoType { get; set; }
        //required	每多少个人共用一个游客信息	当visitorInfoType 为4的时候设置
        public int visitorInfoGroupSize { get; set; }
        //required	游玩人信息要求
        public UserInfoRule visitorInfoRule { get; set; }
        //optional	自动退款时间
        public int autoRefundTime { get; set; }
        /******************************
         * required	退款方式	见映射表<退款类型>:
         *  1	随时退 未消费（包含过期）订单随时可退，无退款手续费，订单可多次退款，退款状态异步获取（人工审核）
         *  3	不可退 订单不可退款
         *  4	有条件退 未消费订单具有时间节点限制、手续费限制、只能整单退款
       *********************************/
        public int refundType { get; set; }
        //required	是否支持部分退款	如果不支持部分退, 则退款类型必须为有条件退. refundType=4
        public bool partlyRefund { get; set; }
        //optional	退款说明
        public string refundNote { get; set; }
        //optional	具体的退款规则，可以有多个	退款类型为4时该字段不能为空
        public List<RefundStairsRule> refundStairsRules { get; set; }
        //required	上架时间	格式yyyy-MM-dd
        public string scheduleOnlineTime { get; set; }
        //required	下架时间	格式yyyy-MM-dd, 注意：下架时间是7月13日代表7月13日00：00分开始产品就下架。
        public string scheduleOfflineTime { get; set; }
       //required	服务电话
        public List<ServicePhone> servicePhones { get; set; }
       //required	入园地址
        public string[] getInAddresses { get; set; }
        //required	是否需要换票	无需换正式纸质门票的都是，无需换票。需要换正式纸质门票入园的，是需换票。需换票的必须传「换票凭证」和「取票地址」字段，否则无法审核上线
        public bool needTicket { get; set; }
        //optional	换票方式
        public GetTicketRule getTicketRule { get; set; } 
        //optional	取票地址
        public string[] ticketGetAddress { get; set; }
        //optional	下单多久后自动取消订单	单位分钟, 不传或者传(30, 120]区间的值
        public int orderCancelTime { get; set; }
        //required	其他注意事项	必须是纯文本，不能有格式代码
        public string otherNote { get; set; }
       //required	用户下单时是否回传凭证	该字段会影响订单核销，必须保证与实际回传情况相符。如无法确认，请传false。要求合作方在对接时具备回传凭证的能力，这一点会在联调和测试时进行验收
        public bool returnVoucher { get; set; }
       //optional	不回传凭证时，显示的文案	不回传凭证时，该字段不能为空, 不得多于20个字
        public string voucherText { get; set; }
       //required	凭证获取形式
        public GetVoucherRule getVoucherRule { get; set; }
        //optional	总库存	-1 表示不限库存，注：useDateMode = 0 时 该字段必填
        public int stock { get; set; }
        //optional	市场价	useDateMode 为0时必填
        public decimal marketPrice { get; set; }
        //optional	美团价	useDateMode 为0时必填
        public decimal mtPrice { get; set; }
        //optional	结算价	useDateMode 为0时必填
        public decimal settlementPrice { get; set; }
        //optional	景区当日开始入园时间，目前该字段已经废弃，请使用voucherTimes	单位分钟,从凌晨开始计算时间
        public int voucherTimeBegin { get; set; }
        //optional	景区当日停止入园时间，目前该字段已经废弃,请使用voucherTimes	单位分钟，从凌晨开始计算时间
        public int voucherTimeEnd { get; set; }
        //optional	景区入园时间,可以写多个(最多3个)时间段，多时间段之间不允许重复，支持跨天
        public List<VoucherTime> voucherTimes { get; set; }
        /***********************************
         * optional	提前预约类型	见映射表<提前预约类型>:
         * 1	任意时刻都可下单
         * 2	用户需要提前多少分钟购买才可以生效。如用户需要提前2小时购买才生效，aheadMinutes=120.
         * 3	截止到(从预定日期晚上24点往前计算)A天B点C分 则aheadMinutes = A x 24 x 60 +(24 x 60 - B x 60 - C)，如预定1.19号游玩，1.18号16点34止预定，则A＝1，B＝16，C＝34
         ************************************/
        public int aheadHoursType { get; set; }
        //optional	提前预约分钟数
        public int aheadMinutes { get; set; }
        //optional	当产品有多个提前购买规则，对接这个字段，仅支持两个规则，且aheadHoursType必须是2和3组合。当aheadHoursType=2时，aheadMinutes不能大于24小时。且提前购买规则算出的最晚可入园时间不能晚于产品中最晚的入园时间。	例如，需要当天15:00前购买，下单后3小时可使用，入园时间7:00～17:00。15:00+3=18:00晚于当天最晚入园时间17:00，校验不通过。
        public List<AheadTimeRule> aheadTimeRules { get; set; }
        //required	身份证几天内	和idLimitCount联合使用, 0 表示不限制
        public int idLimitDays { get; set; }
        //required	每身份证idLimitDays天内最多可购买数量	和idLimitDays联合使用, 值为 0 表示不限制购买数量
        public int idLimitCount { get; set; }
        //required	每手机号几天内	和phoneLimitCount联合使用, 0 表示不限制
        public int phoneLimitDays { get; set; }
        //required	每手机号phoneLimitDays天内最多可购买数量	和phoneLimitDays联合使用, 0 表示不限制
        public int phoneLimitCount { get; set; }
        //required	最多购买数量	不能少于最小购买数量, 0为不限制
        public int maxBuyCount { get; set; }
        //required	最少购买数量	最小购买数量为1
        public int minBuyCount { get; set; }
        //required	产品标题
        public string title { get; set; }
        //optional	产品副标题	切记 副标题长度不能大于512字节
        public string subTitle { get; set; }
        //required	费用包含补充说明	所有产品必须都包含该字段，否则无法上线。必须是纯文本，不能有格式代码
        public string include { get; set; }
        //optional	费用不包含	必须是纯文本，不能有格式代码
        public string exclude { get; set; }
        //required	供应商id
        public int partnerId { get; set; }
        //required	合作方产品id
        public string partnerDealId { get; set; }
        //optional	产品对应的景点id	对应多个景点，用,分割
        public string partnerPoiId { get; set; }
        //optional	产品图片	推荐商家传此字段，有产品图片会提升产品上线速度(最多只能提供5张产品图片)
        public List<DealImageInfo> dealImageInfos { get; set; }
        //optional	有效期开始日期 格式yyyy-MM-dd	useDateMode = 0 时 该字段必填
        public string voucherDateBegin { get; set; }
        //optional	有效期结束日期 格式yyyy-MM-dd	useDateMode = 0 时 该字段必填，注意：结束时间是7月13日代表7月13日23：59分结束。
        public string voucherDateEnd { get; set; }
        //optional	有效期星期设置(可用星期，指每周都哪几天可以使用，默认都不可以使用	useDateMode = 0 时 该字段必填)
        public ValidWeekRule validWeekRule { get; set; }
        //optional	不可用日期	useDateMode = 0 时 该字段如有需要请传入相应数值。如：["2016-07-01", "2016-07-25"]
        public string[] unavailableDates { get; set; } 
    }

    public class UserInfoRule 
    {
        public UserInfoRule() { }
        //optional	是否需要名称	true 是, false 否
        public bool name { get; set; }
        //optional	是否需要拼音	true 是, false 否
        public bool pinyin { get; set; }
        //optional	是否需要电话	true 是, false 否
        public bool mobile { get; set; }
        //optional	是否需要地址	true 是, false 否
        public bool address { get; set; }
        //optional	是否需要邮件	true 是, false 否
        public bool email { get; set; }
        //optional	是否需要港澳通行证	true 是, false 否
        public bool hkmlp { get; set; }
        //optional	大陆居民往来台湾通行证	true 是, false 否
        public bool tlp { get; set; }
        //optional	是否需要台胞证	true 是, false 否
        public bool mtp { get; set; }
        //optional	是否需要护照	true 是, false 否
        public bool passport { get; set; }
        //optional	是否需要身份证号	true 是, false 否
        public bool credentials { get; set; }
    }

    public class RefundStairsRule 
    {
        public RefundStairsRule() { }
        /***********************
         * required	退款时间限制类型,该字段与refundLimitMinutes配合使用，具体参见退款时间类型表	见映射表<退款时间类型>
         * 注意：useDateMode = 0 为有效期模式 useDateMode > 0 为预约模式，即有价格日历。两种模式退款时间计算方式不同，且有效期模式不支持阶梯退款。

            当useDateMode = 0 时

            取值	说明
            0	退款时间无限制 refundLimitMinutes 填写默认值0
            2	退款时间截至到有效期结束前 refundLimitMinutes不能为空。从有效期结束当天24点计算，往前递推。假设时间点为提前A天的B点C分，则refundLimitMinutes = (A + 1) x 24 x 60 -(B x 60 + C) 假设有效期结束日期为1.19号,如果1.18号16点24分截止退款 则 A＝1 B＝16 C＝24
            3	退款时间在截止到有效期结束后 refundLimitMinutes不能为空。从有效期结束日期后一天0点计算，往后递推。假设时间点为截止后A天的B点C分，则refundLimitMinutes＝(A - 1) x 24 x 60 + B x 60 + C 假设预定日期1.19,1.20号10点33分截止退款 则 A＝1 B＝10 C＝33
            当useDateMode > 0 时

            取值	说明
            0	退款时间无限制 refundLimitMinutes 填写默认值0
            1	退款时间在使用开始日期前 refundLimitMinutes不能为空。从预定日期当天0点计算，往前递推。假设时间点为提前A天的B点C分，则refundLimitMinutes = A x 24 x 60 - (B x 60 + C) 假设预定日期为1.19号，退款截止时间为1.18号16点24分 则 A＝1 B＝16 C＝24
            2	退款时间在使用截止日期前 refundLimitMinutes不能为空。从预定截止日期当天24点计算，往前递推。假设时间点为提前A天的B点C分，则refundLimitMinutes = (A + 1) x 24 x 60 -(B x 60 + C) 假设预定日期1.19号 预约有效期(useDateMode)为3, 则预定截止日期为21号24点, 如果1.18号16点24分截止退款 则 A＝3 (18号为21号的前3天) B＝16 C＝24
            3	退款时间在使用截止日期后 refundLimitMinutes不能为空。从使用截至日期后一天0点计算，往后递推。假设时间点为截止后A天的B点C分，则refundLimitMinutes＝(A - 1) x 24 x 60 + B x 60 + C 假设预定日期1.19号 预约有效期(useDateMode)为1 , 1.20号10点33分截止退款 则 A＝1 B＝10 C＝33
         * ********************/
        public int refundTimeType { get; set; }
        //required	退款时间限制	单位分钟,必须大于0
        public int refundLimitMinutes { get; set; }
        /**************************************
        * required	退款手续费类型，与refundFee结合使用	见映射表<退款手续费类型>
         *  1	无手续费
            2	每张票需要手续费, 每张票扣除多少手续费，精确到分。如每张扣除1.12 此时refundFee=1.12
            3	每笔订单需要手续费,每笔订单扣除多少手续费，精确到分。如每笔扣除10.25 此时refundFee = 10.25
            4	销售价格的百分比,小数点后保留两位。如收取12%手续费，此时refundFee=0.12
        ***************************************/
        public int refundFeeMode { get; set; }
        //required	退款手续费	当refundFeeMode＝1 的时候 refundFee ＝0.00 当refundFeeMode =2 或者3 精确到分, 当refundFeeMode =4 单位是1,支持两位小数. 0.1 表示10%的退款手续费
        public double refundFee { get; set; }
        //optional	退款说明
        public string refundNote { get; set; }
    }
    public class ServicePhone 
    {
        public ServicePhone() { }
        //required	服务电话
        public string phone { get; set; }
        //required	服务开始小时	若不填写，默认00 可使用范围00~23
        public string startHour { get; set; }
        //required	服务开始分钟	若不填写，默认00 可使用的范围 00~59
        public string startMin { get; set; }
        //required	服务结束小时	若不填写，默认23 可使用的范围 00~23
        public string endHour { get; set; }
        //required	服务结束分钟	若不填写，默认59 可使用的范围 00~59
        public string endMin { get; set; }
    }
    public class GetTicketRule 
    {
        public GetTicketRule() { }
        //optional	是否需要有效证件	true:需要有效证件, false: 不需要有效证件
        public bool effectiveCertificate { get; set; }
        //optional	换票凭证	2 商家短信,3 商家邮件,4 商家电子码,5 二维码,6 身份证,7 商家订单号,8 手机号,可以多个值并存
        public int[] voucherLoaders { get; set; }
        //optional	是否需要凭证补充信息	true:需要凭证补充信息, false: 不需要凭证补充信息
        public bool needCertificateSupplement { get; set; }
        //optional	凭证补充信息说明	学生证信息
        public string certificateSupplement { get; set; }
    }
    public class GetVoucherRule 
    {
        public GetVoucherRule() { }
        //optional	是否需要有效证件	true:需要有效证件, false: 不需要有效证件
        public bool effectiveCertificate { get; set; }
        //optional	换票凭证	2 商家短信,3 商家邮件,4 商家电子码,5 二维码,6 身份证,7 商家订单号,8 手机号,可以多个值并存
        public int[] voucherLoaders { get; set; }
        //optional	是否需要凭证补充信息	true:需要凭证补充信息, false: 不需要凭证补充信息
        public bool needCertificateSupplement { get; set; }
        //optional	凭证补充信息说明	学生证信息
        public string certificateSupplement { get; set; }
    }

    public class VoucherTime
    {
        public VoucherTime() { }
        //required	景区当日开始入园时间	单位分钟,从凌晨开始计算时间
        public int voucherStartTime { get; set; }
        //required	景区当日停止入园时间	单位分钟，从凌晨开始计算时间
        public int voucherEndTime { get; set; }
        //boolean	optional	是否需要换票时间/入园时间补充信息
        public bool needVoucherOrGetInTimeSupplement { get; set; }
        //string	optional	换票时间/入园时间补充信息说明
        public string voucherOrGetInTimeSupplement { get; set; }
    }
    public class AheadTimeRule 
    {
        public AheadTimeRule() { }
        /*******************************
         * required	提前预约类型	见映射表<提前预约类型>
         *  1	任意时刻都可下单
            2	用户需要提前多少分钟购买才可以生效。如用户需要提前2小时购买才生效，aheadMinutes=120.
            3	截止到(从预定日期晚上24点往前计算)A天B点C分 则aheadMinutes = A x 24 x 60 +(24 x 60 - B x 60 - C)，如预定1.19号游玩，1.18号16点34止预定，则A＝1，B＝16，C＝34
         * ****************************/
        public int aheadHoursType { get; set; }
        //required	提前预约分钟数,当aheadHoursType为1时,aheadMinutes可以传0	
        public int aheadMinutes { get; set; }
    }
    public class DealImageInfo 
    {
        public DealImageInfo() { }
        //optional	图片名称
        public string imageName { get; set; }
        //required	图片地址
        public string imageUrl { get; set; }
        //required	是否是首图	首图frontImage为true，其他为false
        public bool frontImage { get; set; }
    }
    public class ValidWeekRule 
    {
        public ValidWeekRule() { }
        //optional	星期一	true:该天有效, false: 该天无效
        public bool monday { get; set; }
        //optional	星期二	true:该天有效, false: 该天无效
        public bool tuesday { get; set; }
        //optional	星期三	true:该天有效, false: 该天无效
        public bool wednesday { get; set; }
        //optional	星期四	true:该天有效, false: 该天无效
        public bool thursday { get; set; }
        //optional	星期五	true:该天有效, false: 该天无效
        public bool friday { get; set; }
        //optional	星期六	true:该天有效, false: 该天无效
        public bool saturday { get; set; }
        //optional	星期天	true:该天有效, false: 该天无效
        public bool sunday { get; set; }
    }

}

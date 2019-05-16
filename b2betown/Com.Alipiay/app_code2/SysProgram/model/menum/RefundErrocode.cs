using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETS.Framework;

namespace Com.Alipiay.app_code2.SysProgram.model.menum
{
    public enum RefundErrocode
    {
        [EnumAttribute("退款成功")]
        SUCCESS,
        [EnumAttribute("退款失败")]
        REFUND_TRADE_FEE_ERROR,

        [EnumAttribute("签名不正确")]
        ILLEGAL_SIGN,
        [EnumAttribute("动态密钥信息错误")]
        ILLEGAL_DYN_MD5_KEY,
        [EnumAttribute("加密不正确")]
        ILLEGAL_ENCRYPT,

        [EnumAttribute("参数不正确")]
        ILLEGAL_ARGUMENT,
        [EnumAttribute("Service参数不正确")]
        ILLEGAL_SERVICE,
        [EnumAttribute("用户ID不正确")]
        ILLEGAL_USER,
        [EnumAttribute("合作伙伴ID不正确")]
        ILLEGAL_PARTNER,
        [EnumAttribute("接口配置不正确")]
        ILLEGAL_EXTERFACE,
        [EnumAttribute("合作伙伴接口信息不正确")]
        ILLEGAL_PARTNER_EXTERFACE,
        [EnumAttribute("未找到匹配的密钥配置")]
        ILLEGAL_SECURITY_PROFILE,
        [EnumAttribute("代理ID不正确")]
        ILLEGAL_AGENT,
        [EnumAttribute("签名类型不正确")]
        ILLEGAL_SIGN_TYPE,
        [EnumAttribute("字符集不合法")]
        ILLEGAL_CHARSET,
        [EnumAttribute("客户端IP地址无权访问服务")]
        ILLEGAL_CLIENT_IP,
        [EnumAttribute("无权访问")]
        HAS_NO_PRIVILEGE,
        [EnumAttribute("session超时")]
        SESSION_TIMEOUT,
        [EnumAttribute("摘要类型不正确")]
        ILLEGAL_DIGEST_TYPE, 
        [EnumAttribute("文件摘要不正确")]
        ILLEGAL_DIGEST,
        [EnumAttribute("文件格式不正确")]
        ILLEGAL_FILE_FORMAT,
        [EnumAttribute("错误的target_service")]
        ILLEGAL_TARGET_SERVICE,
        [EnumAttribute("partner不允许访问该类型的系统")]
        ILLEGAL_ACCESS_SWITCH_SYSTEM,
        [EnumAttribute("不支持该编码类型")]
        ILLEGAL_ENCODING,
        [EnumAttribute("接口已关闭")]
        EXTERFACE_IS_CLOSED,


        [EnumAttribute("防钓鱼检查不支持该请求来源")]
        ILLEGAL_REQUEST_REFERER,
        [EnumAttribute("防钓鱼检查非法时间戳参数")]
        ILLEGAL_ANTI_PHISHING_KEY,
        [EnumAttribute("防钓鱼检查时间戳超时")]
        ANTI_PHISHING_KEY_TIMEOUT,
        [EnumAttribute("防钓鱼检查非法调用IP")]
        ILLEGAL_EXTER_INVOKE_IP,
        [EnumAttribute("总比数大于1000")]
        BATCH_NUM_EXCEED_LIMIT,
        [EnumAttribute("错误的退款时间")]
        REFUND_DATE_ERROR,
        [EnumAttribute("传入的总笔数格式错误")]
        BATCH_NUM_ERROR,
        [EnumAttribute("传入的退款条数不等于数据集解析出的退款条数")]
        BATCH_NUM_NOT_EQUAL_TOTAL,
        [EnumAttribute("单笔退款明细超出限制")]
        SINGLE_DETAIL_DATA_EXCEED_LIMIT,
        [EnumAttribute("不是当前卖家的交易")]
        NOT_THIS_SELLER_TRADE,
        [EnumAttribute("同一批退款中存在两条相同的退款记录")]
        DUBL_TRADE_NO_IN_SAME_BATCH,
        [EnumAttribute("重复的批次号")]
        DUPLICATE_BATCH_NO,
        [EnumAttribute("交易状态不允许退款")]
        TRADE_STATUS_ERROR,

        [EnumAttribute("批次号格式错误")]
        BATCH_NO_FORMAT_ERROR,
        [EnumAttribute("卖家信息不存在")]
        SELLER_INFO_NOT_EXIST,
        [EnumAttribute("平台商未签署协议")]
        PARTNER_NOT_SIGN_PROTOCOL,
        [EnumAttribute("退款明细非本合作伙伴的交易")]
        NOT_THIS_PARTNERS_TRADE,
        [EnumAttribute("数据集参数格式错误")]
        DETAIL_DATA_FORMAT_ERROR,
        [EnumAttribute("有密接口不允许退分润")]
        PWD_REFUND_NOT_ALLOW_ROYALTY,
        [EnumAttribute("退票面价金额不合法")]
        NANHANG_REFUND_CHARGE_AMOUNT_ERROR,
        [EnumAttribute("退款金额不合法")]
        REFUND_AMOUNT_NOT_VALID,
        [EnumAttribute("交易类型不允许退交易")]
        TRADE_PRODUCT_TYPE_NOT_ALLOW_REFUND,
        [EnumAttribute("退款票面价不能大于支付票面价")]
        RESULT_FACE_AMOUNT_NOT_VALID,
        [EnumAttribute("退收费金额不合法")]
        REFUND_CHARGE_FEE_ERROR,
        [EnumAttribute("退收费失败")]
        REASON_REFUND_CHARGE_ERR,
        [EnumAttribute("退收费金额错误")]
        RESULT_AMOUNT_NOT_VALID,
        [EnumAttribute("账号无效")]
        RESULT_ACCOUNT_NO_NOT_VALID,
        [EnumAttribute("退款金额错误")]
        REASON_TRADE_REFUND_FEE_ERR,
        [EnumAttribute("已退款金额错误")]
        REASON_HAS_REFUND_FEE_NOT_MATCH,
        [EnumAttribute("账户状态无效")]
        TXN_RESULT_ACCOUNT_STATUS_NOT_VALID,
        [EnumAttribute("账户余额不足")]
        TXN_RESULT_ACCOUNT_BALANCE_NOT_ENOUGH,
        [EnumAttribute("红包无法部分退款")]
        REASON_REFUND_AMOUNT_LESS_THAN_COUPON_FEE,
         

    }
}

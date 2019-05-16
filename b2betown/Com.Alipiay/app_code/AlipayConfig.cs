using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.Common.Business;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Modle;


namespace Com.Alipay
{
    /// <summary>
    /// 类名：Config
    /// 功能：基础配置类
    /// 详细：设置帐户有关信息及返回路径
    /// 版本：3.2
    /// 日期：2011-03-17
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
    /// 
    /// 如何获取安全校验码和合作身份者ID
    /// 1.用您的签约支付宝账号登录支付宝网站(www.alipay.com)
    /// 2.点击“商家服务”(https://b.alipay.com/order/myOrder.htm)
    /// 3.点击“查询合作者身份(PID)”、“查询安全校验码(Key)”
    /// </summary>
    internal class Config : System.Web.UI.Page
    {
        #region 字段
        private string partner = "";
        private string key = "";
        private string seller_email = "";
        private string return_url = "";
        private string notify_url = "";
        private string input_charset = "";
        private string sign_type = "";
        #endregion

        #region 辨别商家参数
        public string RequestUrl = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Trim().ToLower();
        public string Requestfile = HttpContext.Current.Request.ServerVariables["Url"].ToLower();
        public int comid = 0;
        #endregion


        public Config()
        {
            //↓↓↓↓↓↓↓↓↓↓请在这里配置您的基本信息↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓


            ////----------------------------------------易城默认支付宝参数-----------------------------------------------
            //合作身份者ID，以2088开头由16位纯数字组成的字符串
            partner = "2088601265635235";
            //交易安全检验码，由数字和字母组成的32位字符串
            key = "ebkuy760nslctp2kd1ixcuvxn1c4e2vy";
            //签约支付宝账号或卖家支付宝帐户
            seller_email = "wesley@etown.cn";
            //页面跳转同步返回页面文件路径 要用 http://格式的完整路径，不允许加?id=123这类自定义参数

            if (Requestfile == "/ui/vasui/alipay/subpay.aspx")
            {
                return_url = "http://" + RequestUrl + "/ui/vasui/PaySuc.aspx";
            }else{
                return_url = "http://" + RequestUrl + "/H5/pay_by/call_back_kuaijie.aspx";
                
            }


            //服务器通知的页面文件路径 要用 http://格式的完整路径，不允许加?id=123这类自定义参数
            notify_url = "http://" + RequestUrl + "/ui/vasui/alipay/notify_url.aspx";
            //notify_url = "";


            //支付宝账户
            //根据域名读取商户ID,如果没有绑定域名直接跳转后台
            B2b_company_info companyinfo = B2bCompanyData.GetComId(RequestUrl);
            if (companyinfo != null)
            {
                B2bFinanceData comfindate = new B2bFinanceData();
                var comfinaceinfo = comfindate.FinancePayType(companyinfo.Com_id);
                if (comfinaceinfo != null)
                {
                    if (comfinaceinfo.Paytype == 2)
                    {
                        //合作身份者ID，以2088开头由16位纯数字组成的字符串
                        partner = comfinaceinfo.Alipay_id.ToString().Trim().ToLower();
                        //交易安全检验码，由数字和字母组成的32位字符串
                        key = comfinaceinfo.Alipay_key.ToString().Trim().ToLower();
                        //签约支付宝账号或卖家支付宝帐户
                        seller_email = comfinaceinfo.Alipay_account.ToString().Trim().ToLower();
                    }
                }
                else {
                    ////合作身份者ID，以2088开头由16位纯数字组成的字符串
                    partner = "2088601265635235";
                    ////交易安全检验码，由数字和字母组成的32位字符串
                    key = "ebkuy760nslctp2kd1ixcuvxn1c4e2vy";
                    ////签约支付宝账号或卖家支付宝帐户
                    seller_email = "wesley@etown.cn";
                }
            }
            else
            {
                // 判定是否为自助域名规则按 shop1.etown.cn
                if (Domain_def.Domain_yanzheng(RequestUrl))
                {

                    B2bFinanceData comfindate = new B2bFinanceData();
                    var comfinaceinfo = comfindate.FinancePayType(Int32.Parse(Domain_def.Domain_Huoqu(RequestUrl)));
                    if (comfinaceinfo != null)
                    {
                        if (comfinaceinfo.Paytype == 2)
                        {
                            //合作身份者ID，以2088开头由16位纯数字组成的字符串
                            partner = comfinaceinfo.Alipay_id.ToString().Trim().ToLower();
                            //交易安全检验码，由数字和字母组成的32位字符串
                            key = comfinaceinfo.Alipay_key.ToString().Trim().ToLower();
                            //签约支付宝账号或卖家支付宝帐户
                            seller_email = comfinaceinfo.Alipay_account.ToString().Trim().ToLower();
                        }
                    }
                    else
                    {
                        ////合作身份者ID，以2088开头由16位纯数字组成的字符串
                        partner = "2088601265635235";
                        ////交易安全检验码，由数字和字母组成的32位字符串
                        key = "ebkuy760nslctp2kd1ixcuvxn1c4e2vy";
                        ////签约支付宝账号或卖家支付宝帐户
                        seller_email = "wesley@etown.cn";
                    }
                }

            }

            ////----------------------------------------易城默认支付宝参数-----------------------------------------------
            ////合作身份者ID，以2088开头由16位纯数字组成的字符串
            //partner = "2088601265635235";
            ////交易安全检验码，由数字和字母组成的32位字符串
            //key = "ebkuy760nslctp2kd1ixcuvxn1c4e2vy";
            ////签约支付宝账号或卖家支付宝帐户
            //seller_email = "wesley@etown.cn";
            ////页面跳转同步返回页面文件路径 要用 http://格式的完整路径，不允许加?id=123这类自定义参数
            //return_url = "http://shop.etown.cn/ui/vasui/PaySuc.aspx";
            ////服务器通知的页面文件路径 要用 http://格式的完整路径，不允许加?id=123这类自定义参数
            //notify_url = "http://shop.etown.cn/ui/vasui/alipay/notify_url.aspx";


            //↑↑↑↑↑↑↑↑↑↑请在这里配置您的基本信息↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑

            //字符编码格式 目前支持 gbk 或 utf-8
            input_charset = "utf-8";

            //签名方式 不需修改
            sign_type = "MD5";
        }

        #region 属性
        /// <summary>
        /// 获取或设置合作者身份ID
        /// </summary>
        public string Partner
        {
            get { return partner; }
            set { partner = value; }
        }

        /// <summary>
        /// 获取或设置交易安全检验码
        /// </summary>
        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// 获取或设置签约支付宝账号或卖家支付宝帐户
        /// </summary>
        public string Seller_email
        {
            get { return seller_email; }
            set { seller_email = value; }
        }

        /// <summary>
        /// 获取页面跳转同步通知页面路径
        /// </summary>
        public string Return_url
        {
            get { return return_url; }
        }

        /// <summary>
        /// 获取服务器异步通知页面路径
        /// </summary>
        public string Notify_url
        {
            get { return notify_url; }
        }

        /// <summary>
        /// 获取字符编码格式
        /// </summary>
        public string Input_charset
        {
            get { return input_charset; }
        }

        /// <summary>
        /// 获取签名方式
        /// </summary>
        public string Sign_type
        {
            get { return sign_type; }
        }
        #endregion
    }
}
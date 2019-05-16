using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.VAS.Service.VASService.Data;
using Com.Alipiay.app_code2;
using Com.Alipiay.app_code2.SysProgram.model;
using Com.Alipiay.app_code2.SysProgram.data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle.Enum;


namespace ETS2.WebApp.UI.VASUI.alipay
{
    public partial class refund_subpay : System.Web.UI.Page
    {
        private static object lockobj = new object();
        protected void Page_Load(object sender, EventArgs e)
        {
            int orderid = Request["orderid"].ConvertTo<int>(0);
            decimal quit_fee = Request["quit_fee"].ConvertTo<decimal>(0);
            int rentserverid = Request["rentserverid"].ConvertTo<int>(0);
            int b2b_eticket_Depositid = Request["b2b_eticket_Depositid"].ConvertTo<int>(0);
            
            int rentserver_refundlogid = 0; 
            if (orderid == 0 || quit_fee == 0)
            {
                lblresult.InnerText = "参数传递错误";
                return;
            }

            B2b_order oldorder = new B2bOrderData().GetOrderById(orderid);

            if (oldorder != null)
            {
                if (rentserverid > 0)
                { //退押金
                    B2b_Rentserver_RefundLog refundlog = new B2b_Rentserver_RefundLogData().GetServerRefundlog(orderid, rentserverid, b2b_eticket_Depositid, 2);
                    if (refundlog == null)
                    {
                        lblresult.InnerText = "押金状态需要是 退押金处理中 才可退款";
                        return;
                    }
                    rentserver_refundlogid = refundlog.id;
                }
                else
                {
                    //退订单
                    if (oldorder.Order_state == 17 || oldorder.Order_state == 18)
                    { }
                    else
                    {
                        lblresult.InnerText = "订单状态有误:" + EnumUtils.GetName((OrderStatus)oldorder.Order_state); ;
                        return;
                    }
                }
            }
            else
            {
                lblresult.InnerText = "订单获取出错";
                return;
            }

            //得到订单的支付信息
            B2b_pay mpay = new B2bPayData().GetSUCCESSPayById(orderid);
            if (mpay == null)
            {
                lblresult.InnerText = "支付信息获取不到";
                return;
            }
            if (mpay.Trade_status != "TRADE_SUCCESS")
            {
                string r = "订单支付没有成功，不可退款";
                //Response.Write(r);
                lblresult.InnerText = r;
                return;
            }
            #region 支付宝退款
            B2b_finance_paytype model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(mpay.comid);
            if (model == null)
            {

                string data2 = "支付宝设置信息不存在";
                //Response.Write(data2);
                lblresult.InnerText = data2;

                return;
            }
            lock (lockobj)
            {
                ////////////////////////////////////////////请求参数////////////////////////////////////////////
                int nowbatch_num = 0;//退款笔数
                string nowdetail_data = "";//退款详细数据


                nowbatch_num = 1;
                nowdetail_data = mpay.Trade_no + "^" + quit_fee.ToString("F2") + "^" + "协商退款";//原付款支付宝交易号^退款总金额^退款理由

                //服务器异步通知页面路径
                string notify_url = "http://shop.etown.cn/ui/vasui/alipay/refund_notify_url.aspx";
                //需http://格式的完整路径，不允许加?id=123这类自定义参数

                //卖家支付宝帐户
                string seller_email = Config.Seller_email.ToString().Trim();
                //必填

                //退款当天日期
                string refund_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //必填，格式：年[4位]-月[2位]-日[2位] 小时[2位 24小时制]:分[2位]:秒[2位]，如：2007-10-01 13:13:13

                //批次号
                string batch_no = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                //必填，格式：当天日期[8位]+序列号[3至24位]，如：201008010000001

                //退款笔数
                string batch_num = nowbatch_num.ToString();
                //必填，参数detail_data的值中，“#”字符出现的数量加1，最大支持1000笔（即“#”字符出现的数量999个）

                //退款详细数据
                string detail_data = nowdetail_data;
                //必填，具体格式请参见接口技术文档


                ////////////////////////////////////////////////////////////////////////////////////////////////

                //把请求参数打包成数组
                SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                sParaTemp.Add("partner", Config.Partner);
                sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
                sParaTemp.Add("service", "refund_fastpay_by_platform_pwd");
                sParaTemp.Add("notify_url", notify_url);
                sParaTemp.Add("seller_email", seller_email);
                sParaTemp.Add("refund_date", refund_date);
                sParaTemp.Add("batch_no", batch_no);
                sParaTemp.Add("batch_num", batch_num);
                sParaTemp.Add("detail_data", detail_data);

                B2b_pay_alipayrefundlog nowlog = new B2b_pay_alipayrefundlog
                {
                    id = 0,
                    orderid = orderid,
                    service = "refund_fastpay_by_platform_pwd",
                    partner = Config.Partner,
                    notify_url = notify_url,
                    seller_email = Config.Seller_email,
                    seller_user_id = Config.Partner,
                    refund_date = DateTime.Parse(refund_date),
                    batch_no = batch_no,
                    batch_num = int.Parse(batch_num),
                    detail_data = detail_data,
                    notify_time = DateTime.Parse("1970-01-01"),
                    notify_type = "",
                    notify_id = "",
                    success_num = 0,
                    result_details = "",
                    error_code = "",
                    error_desc = "",
                    refund_fee = quit_fee,
                    rentserver_refundlogid = rentserver_refundlogid
                };
                int ee = new B2b_pay_alipayrefundlogData().Editalipayrefundlog(nowlog);
                if (ee > 0)
                {
                    //建立请求
                    string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
                    Response.Write(sHtmlText);
                    //lblresult.InnerText = sHtmlText;
                    return;
                }
                else
                {
                    //Response.Write("{\"type\":\"100\",\"msg\":\"退款操作完成(款项需手动退款).\"}");
                    lblresult.InnerText = "意外错误";
                    return;
                }
            }
            #endregion
        }
    }
}
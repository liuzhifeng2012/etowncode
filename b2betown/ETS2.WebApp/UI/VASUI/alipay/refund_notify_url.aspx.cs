using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Collections.Generic;
using Com.Alipay;
using Com.Alipiay.app_code2.SysProgram.data;
using ETS.Framework;
using Com.Alipiay.app_code2.SysProgram.model.menum;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.VAS.Service.VASService.Data;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS.JsonFactory;
using Com.Alipiay.app_code2.SysProgram.model;
using System.Xml;
using Newtonsoft.Json;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.CRM.Service.CRMService.Data;

namespace ETS2.WebApp.UI.VASUI.alipay
{
    /// <summary>
    /// 功能：服务器异步通知页面
    /// 版本：3.3
    /// 日期：2012-07-10
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
    /// 
    /// ///////////////////页面功能说明///////////////////
    /// 创建该页面文件时，请留心该页面文件中无任何HTML代码及空格。
    /// 该页面不能在本机电脑测试，请到服务器上做测试。请确保外部可以访问该页面。
    /// 该页面调试工具请使用写文本函数logResult。
    /// 如果没有收到该页面返回的 success 信息，支付宝会在24小时内按一定的时间策略重发通知
    /// </summary>
    public partial class refund_notify_url : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> sPara = GetRequestPost();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);

                if (verifyResult)//验证成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码


                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                    //批次号

                    string batch_no = Request.Form["batch_no"];

                    //批量退款数据中转账成功的笔数

                    string success_num = Request.Form["success_num"];

                    //批量退款数据中的详细信息
                    string result_details = Request.Form["result_details"];


                    //判断是否在商户网站中已经做过了这次通知返回的处理
                    //如果没有做过处理，那么执行商户的业务程序
                    //如果有做过处理，那么不执行商户的业务程序

                    string error_code = "";
                    string error_desc = "";

                    try
                    {
                        error_code = result_details.IndexOf("SUCCESS") > -1 ? "SUCCESS" : result_details;

                        string[] arr = error_code.Split('^');
                        foreach (string carr in arr)
                        {
                            if (carr != "")
                            {
                                if (carr.IndexOf("$") > -1)
                                {
                                    error_code = carr.Substring(0, carr.IndexOf("$"));
                                }
                            }
                        }

                        error_desc = EnumUtils.GetName((RefundErrocode)Enum.Parse(typeof(RefundErrocode), error_code, false));
                    }
                    catch(Exception ex)
                    {
                        error_desc = "获取枚举描述错误("+ex.Message+")";
                    }
                    string notify_id = Request.Form["notify_id"];
                    string notify_type = Request.Form["notify_type"];
                    string notify_time = Request.Form["notify_time"];


                    int uprefundlog = new B2b_pay_alipayrefundlogData().Uprefundlog(batch_no, success_num, result_details, error_code, error_desc, notify_id, notify_type, notify_time);
                
                    if (error_code == "SUCCESS")
                    {
                        if (uprefundlog == 0)
                        {
                            TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\alipayrefund_severeerrLog.txt", "退款批次" + batch_no + ";支付宝已经退款，录入退款日志出错,抓紧处理，防止重复给客户退款");
                        }

                        //根据batch_no(退款批次)得到 退款日志
                        B2b_pay_alipayrefundlog malipayrefundlog = new B2b_pay_alipayrefundlogData().Getrefundlogbybatch_no(batch_no);
                        if (malipayrefundlog == null)
                        {
                            //加txt文档记录
                            TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\alipayrefund_severeerrLog.txt", "退款批次" + batch_no + ";支付宝已经退款，根据batch_no(退款批次)得到 退款日志出错,抓紧处理，防止重复给客户退款");

                        }
                        else
                        {
                            //退押金
                            if (malipayrefundlog.rentserver_refundlogid > 0)
                            {
                                //修改退押金日志表中 退押金状态
                                new B2b_Rentserver_RefundLogData().Upb2b_Rentserver_RefundLogState(malipayrefundlog.rentserver_refundlogid,1);

                                //需要在易城给特定商户(钱支付到易城的)做一笔支出操作；
                                B2b_order a_orderinfo = new B2bOrderData().GetOrderById(malipayrefundlog.orderid);
                                B2b_pay mpay = new B2bPayData().GetSUCCESSPayById(malipayrefundlog.orderid);
                               string  proname = new B2bComProData().GetProName(a_orderinfo.Pro_id);
                                var company = B2bCompanyData.GetCompany(a_orderinfo.Comid);

                                OrderJsonData.ZhichuFromYicheng(mpay, a_orderinfo, proname, company, malipayrefundlog.refund_fee);

                            }
                            //订单退款
                            else 
                            {
                                B2b_order oldorder = new B2bOrderData().GetOrderById(malipayrefundlog.orderid);

                                if (oldorder != null)
                                {
                                    if (oldorder.Order_state == 17 || oldorder.Order_state == 18)
                                    {
                                        oldorder.Order_state = (int)OrderStatus.QuitOrder;//订单退票
                                        oldorder.Ticketinfo = oldorder.askquitfeereason + "-" + oldorder.askquitfeeexplain;
                                        oldorder.Backtickettime = DateTime.Now;
                                        oldorder.Ticket = malipayrefundlog.refund_fee;

                                        new B2bOrderData().InsertOrUpdate(oldorder);
                                    }
                                }

                                string data2 = OrderJsonData.Upticket(oldorder);
                                data2 = "{\"root\":" + data2 + "}";
                                XmlDocument xxd2 = JsonConvert.DeserializeXmlNode(data2);
                                string type2 = xxd2.SelectSingleNode("root/type").InnerText;
                                string msg2 = xxd2.SelectSingleNode("root/msg").InnerText;


                                if (type2 == "100")
                                {
                                    //data2 = "{\"type\":100,\"msg\":\"退款成功，款项已自动退回给用户!\"}";
                                    Response.Write("success");  //请不要修改或删除 
                                    return;
                                }
                                else
                                {
                                    TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\alipayrefund_severeerrLog.txt", oldorder.Id + "支付宝已经退款，可是出现严重意外错误(" + msg2 + "),抓紧处理，防止重复给客户退款");
                                    Response.Write("fail");  //请不要修改或删除
                                    return;
                                }
                            } 
                        }

                    }
                    else 
                    {
                        
                        Response.Write("success");  //请不要修改或删除 
                    }



                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else//验证失败
                {
                    Response.Write("fail");
                }
            }
            else
            {
                Response.Write("无通知参数");
            }
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }
    }
}
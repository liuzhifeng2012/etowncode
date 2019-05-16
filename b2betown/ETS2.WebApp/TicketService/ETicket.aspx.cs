using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Com.Alipiay.app_code2;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.VAS.Service.VASService.Data;
using Com.Alipiay.app_code2.SysProgram.model;
using Com.Alipiay.app_code2.SysProgram.data;
using ETS.Framework;
using Com.Alipiay.app_code2.SysProgram.model.menum;
using ETS.JsonFactory;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;


namespace ETS2.WebApp.TicketService
{
    public partial class ETicket : System.Web.UI.Page
    {
        //////----------正式参数---------------
        ////public string organization = "1000000438";//机构号
        ////public string password = "mvjHC6yllZ";//密码
        ////public string skey = "oiszFLRc";//DES密钥

        ////-----------测试参数----------------
        //public string organization = "1000000286";//机构号
        //public string password = "7Udu8O5m6r";//密码
        //public string skey = "vSK5eIRC";//des密钥

        private static object lockobj = new object();
        protected void Page_Load(object sender, EventArgs e)
        {
            //string error_code = "2015070900001000950057205486^5.00^REFUND_TRADE_FEE_ERROR$wesley@etown.cn^2088601265635235^0.00^";
            //string[] arr = error_code.Split('^');
            //foreach (string carr in arr)
            //{
            //    if (carr != "")
            //    {
            //        if (carr.IndexOf("$") > -1)
            //        {
            //            error_code = carr.Substring(0, carr.IndexOf("$"));
            //        }
            //    }
            //}
            //string error_desc = EnumUtils.GetName((RefundErrocode)Enum.Parse(typeof(RefundErrocode), error_code, false));

            //lblresult.InnerText = error_code + "---" + error_desc;
        }
        protected void BtnAlipay_Click(object sender, EventArgs e)
        {

            int orderid = Orderid.Text.Trim().ConvertTo<int>(0);
            decimal quit_fee = Quitfee.Text.Trim().ConvertTo<decimal>(0);


            //得到订单的支付信息
            B2b_pay mpay = new B2bPayData().GetSUCCESSPayById(orderid);
            if (mpay.Trade_status != "TRADE_SUCCESS")
            {
                string r = "{\"type\":1,\"msg\":\"订单支付没有成功，不可退款\"}";
                //Response.Write(r);
                lblresult.InnerText = r;
                return;
            }
            #region 支付宝退款
            B2b_finance_paytype model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(mpay.comid);
            if (model == null)
            {

                string data2 = "{\"type\":1,\"msg\":\"支付宝设置信息不存在!\"}";
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
                    refund_fee = quit_fee
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
                    lblresult.InnerText = "{\"type\":\"100\",\"msg\":\"退款操作完成(款项需手动退款).\"}";
                    return;
                }
            }
            #endregion

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string qunarorderid = qunar_order.Text.Trim();
            B2b_order ordermodel=new B2bOrderData().GetOrderById(qunarorderid.ConvertTo<int>(0));

            EticketJsonData.AsyncSend_qunar(ordermodel);
        }
        ///// <summary>
        ///// 发送电子票
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    //string req_seq = Request["req_seq"];
        //    //string project_num = Request["project_num"];
        //    //string num = Request["num"];
        //    //string mobile = Request["mobile"];
        //    //string use_date = Request["use_date"];
        //    //string real_name_type = Request["real_name_type"];
        //    //string real_name = Request["real_name"];
        //    //string id_card = Request["id_card"];
        //    //string card_type = Request["card_type"];

        //    //Add_Order(req_seq, project_num, num, mobile, use_date, real_name_type, real_name, id_card, card_type);//发送电子票
        //    Add_Order();
        //}
        ///// <summary>
        ///// 查询电子票
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void Button2_Click(object sender, EventArgs e)
        //{
        //    //string req_seq_sel = Request["req_seq_sel"];
        //    string req_seq_sel = "100000028620140508042243333031";

        //    query_order(req_seq_sel);//查询电子票
        //}
        ///// <summary>
        ///// 撤销电子票
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void Button3_Click(object sender, EventArgs e)
        //{
        //    ////string order_num = Request["order_num"];
        //    ////string num = Request["num"];
        //    //string order_num = "D10968103";
        //    //string num = "1";
        //    //cancel_order(order_num, num);//撤销电子票
        //    Literal3.Text = "撤销电子票";
        //}
        ///// <summary>
        ///// 重新发送电子票
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void Button4_Click(object sender, EventArgs e)
        //{
        //    //string order_num = Request["order_num"];
        //    string order_num = "D10968103";
        //    repeat_order(order_num);//重新发送电子票
        //}
        ///// <summary>
        ///// 转发电子票
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void Button5_Click(object sender, EventArgs e)
        //{
        //    //string order_num = Request["order_num"];
        //    //string old_mobile = Request["old_mobile"];
        //    //string new_mobiel = Request["new_mobile"];
        //    string order_num = "D10968103";
        //    string old_mobile = "13522401292";
        //    string new_mobiel = "13488761102";

        //    sendto_order(order_num, old_mobile, new_mobiel);//转发电子票
        //}

        ///// <summary>
        ///// 发送电子票
        ///// </summary>
        ////public void Add_Order(string req_seq, string project_num, string num, string mobile, string use_date, string real_name_type, string real_name, string id_card, string card_type)
        //public void Add_Order()
        //{
        //    string req_seq = organization + DateTime.Now.ToString("yyyyMMddhhssmm") + CreateNum();//请求流水号
        //    string project_num = "15000000004";//产品编码
        //    string num = "1";//购买数量
        //    string mobile = "13522401292";//电话
        //    string use_date = "";//指定电子票的使用时间，默认为发票日期起30内有效
        //    string real_name_type = "";//实名制类型，服务商提供（根据所卖产品而定）。 
        //    string real_name = "yc测试";//姓名，如为实名制产品，则此项为必填项，最多3个名字，以逗号分隔。

        //    StringBuilder builderOrder = new StringBuilder();
        //    builderOrder.Append(@"<?xml version=""1.0"" encoding=""utf-8"" ?>");
        //    builderOrder.Append(@"<business_trans version=""1.0"">");
        //    builderOrder.AppendFormat("<request_type>{0}</request_type>", "add_order111");
        //    builderOrder.AppendFormat("<organization>{0}</organization>", organization);//<!-机构号-->
        //    builderOrder.AppendFormat("<password>{0}</password>", password);//<!-- 接口使用密码  y-->
        //    builderOrder.AppendFormat("<req_seq>{0}</req_seq>", req_seq);//<!--请求流水号 y-->
        //    builderOrder.AppendFormat("<order>");//<!--订单信息-->
        //    builderOrder.AppendFormat("<product_num>{0}</product_num>", project_num);//<!--产品编码 y-->
        //    builderOrder.AppendFormat("<num>{0}</num>", num);//<!--购买数量 y-->
        //    builderOrder.AppendFormat("<mobile>{0}</mobile>", mobile);//<!-- 手机号码 y-->
        //    builderOrder.AppendFormat("<use_date>{0}</use_date>", use_date);//<!-- 使用时间 -->
        //    builderOrder.AppendFormat("<real_name_type>{0}</real_name_type>", real_name_type);//<!-- 实名制类型：0无需实名 1一张一人,2一单一人,3一单一人+身份证-->
        //    builderOrder.AppendFormat("<real_name>{0}</real_name>", real_name);//<!--真是姓名  ,隔开 最多3个名字 <=3 -->
        //    builderOrder.AppendFormat("<id_card>{0}</id_card>", "");//<!--证件号码 -->
        //    builderOrder.AppendFormat("<card_type>{0}</card_type>", "");//<!--证件类型0身份证；1其他证件 -->
        //    builderOrder.AppendFormat("</order>");
        //    builderOrder.AppendFormat("</business_trans>");

        //    try
        //    {
        //        TicketServicePortTypeClient its10 = new TicketServicePortTypeClient();

        //        string en = DESEnCode(builderOrder.ToString(), skey);
        //        string retxmls = its10.getEleInterface(organization, en);

        //        string de = DESDeCode(retxmls, skey);
        //        Literal1.Text = de;

        //        //Response.Write();

        //        //Response.Flush();
        //        //Response.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        //Response.Write(e.Message);
        //        //Response.Flush();
        //        //Response.Close();
        //        Literal1.Text = e.Message;
        //    }



        //}

        //private void query_order(string req_seq_sel)
        //{

        //    string req_seq = organization + DateTime.Now.ToString("yyyyMMddhhssmm") + CreateNum();//请求流水号

        //    StringBuilder builderOrder = new StringBuilder();
        //    builderOrder.Append(@"<?xml version=""1.0"" encoding=""utf-8""?>");
        //    builderOrder.Append(@"<business_trans version=""1.0"">");
        //    builderOrder.Append("<request_type>query_order</request_type>");//<!--查询-->
        //    builderOrder.AppendFormat("<organization>{0}</organization>", organization);//<!--机构号-->
        //    builderOrder.AppendFormat("<password>{0}</password>", password);//<!-- 接口使用密码  -->
        //    builderOrder.AppendFormat("<req_seq>{0}</req_seq>", req_seq);//<!--请求流水号-->
        //    builderOrder.Append("<order>");
        //    builderOrder.AppendFormat("<order_num>{0}</order_num>", req_seq_sel);//<!-- 订单号 y-->
        //    builderOrder.Append("</order>");
        //    builderOrder.Append("</business_trans>");

        //    try
        //    {
        //        TicketServicePortTypeClient its11 = new TicketServicePortTypeClient();
        //        string en = DESEnCode(builderOrder.ToString(), skey);
        //        string retxmls = its11.getEleInterface(organization, en);

        //        string de = DESDeCode(retxmls, skey);
        //        Literal2.Text = de;
        //        //Response.Write(DESDeCode(retxmls, skey));
        //        //Response.Flush();
        //        //Response.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        Literal2.Text = e.Message;
        //        //Response.Write(e.Message);
        //        //Response.Flush();
        //        //Response.Close();
        //    }

        //}


        //private void repeat_order(string order_num)
        //{
        //    string req_seq = organization + DateTime.Now.ToString("yyyyMMddhhssmm") + CreateNum();//请求流水号
        //    StringBuilder buildOrder = new StringBuilder();
        //    buildOrder.Append(@"<?xml version=""1.0"" encoding=""utf-8""?>");
        //    buildOrder.Append(@"<business_trans version=""1.0"">");
        //    buildOrder.Append("<request_type>repeat_order</request_type>");//<!--撤销-->
        //    buildOrder.AppendFormat("<organization>{0}</organization>", organization);//<!--机构号-->
        //    buildOrder.AppendFormat("<password>{0}</password>", password);//<!-- 接口使用密码  -->
        //    buildOrder.AppendFormat("<req_seq>{0}</req_seq>", req_seq);//<!--请求流水号-->
        //    buildOrder.Append("<order>");
        //    buildOrder.AppendFormat("<order_num>{0}</order_num>", order_num);//<!-- 订单号 y-->    
        //    buildOrder.Append("</order>");
        //    buildOrder.Append("</business_trans>");

        //    try
        //    {
        //        TicketServicePortTypeClient its12 = new TicketServicePortTypeClient();
        //        string en = DESEnCode(buildOrder.ToString(), skey);
        //        string retxmls = its12.getEleInterface(organization, en);
        //        string de = DESDeCode(retxmls, skey);
        //        Literal4.Text = de;
        //        //Response.Write(DESDeCode(retxmls, skey));
        //        //Response.Flush();
        //        //Response.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        Literal4.Text = e.Message;
        //        //Response.Write(e.Message);
        //        //Response.Flush();
        //        //Response.Close();
        //    }
        //}

        //private void cancel_order(string order_num, string num)
        //{
        //    string req_seq = organization + DateTime.Now.ToString("yyyyMMddhhssmm") + CreateNum();//请求流水号

        //    StringBuilder buildOrder = new StringBuilder();
        //    buildOrder.Append(@"<?xml version=""1.0"" encoding=""utf-8""?>");
        //    buildOrder.Append(@"<business_trans version=""1.0"">");
        //    buildOrder.Append("<request_type>cancel_order</request_type>");//<!--撤销-->
        //    buildOrder.AppendFormat("<organization>{0}</organization>", organization);//<!--机构号-->
        //    buildOrder.AppendFormat("<password>{0}</password>", password);//<!-- 接口使用密码  -->
        //    buildOrder.AppendFormat("<req_seq>{0}</req_seq>", req_seq);//<!--请求流水号-->
        //    buildOrder.Append("<order>");
        //    buildOrder.AppendFormat("<order_num>{0}</order_num>", order_num);//<!-- 订单号 y-->
        //    buildOrder.AppendFormat("<num>{0}</num>", num);//<!-- 张数 y-->
        //    buildOrder.Append("</order>");
        //    buildOrder.Append("</business_trans>");

        //    try
        //    {
        //        TicketServicePortTypeClient its12 = new TicketServicePortTypeClient();
        //        string en = DESEnCode(buildOrder.ToString(), skey);
        //        string retxmls = its12.getEleInterface(organization, en);
        //        string de = DESDeCode(retxmls, skey);
        //        Literal3.Text = de;
        //        //Response.Write();
        //        //Response.Flush();
        //        //Response.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        Literal3.Text = e.Message;
        //        //Response.Write(e.Message);
        //        //Response.Flush();
        //        //Response.Close();
        //    }
        //}




        //private void sendto_order(string order_num, string old_mobile, string new_mobile)
        //{
        //    string req_seq = organization + DateTime.Now.ToString("yyyyMMddhhssmm") + CreateNum();//请求流水号

        //    StringBuilder builderOrder = new StringBuilder();
        //    builderOrder.Append(@"<?xml version=""1.0"" encoding=""utf-8"" ?>");
        //    builderOrder.Append(@"<business_trans version=""1.0"">");
        //    builderOrder.AppendFormat("<request_type>{0}</request_type>", "sendto_order");
        //    builderOrder.AppendFormat("<organization>{0}</organization>", organization);//<!-机构号-->
        //    builderOrder.AppendFormat("<password>{0}</password>", password);//<!-- 接口使用密码  y-->
        //    builderOrder.AppendFormat("<req_seq>{0}</req_seq>", req_seq);//<!--请求流水号 y-->
        //    builderOrder.AppendFormat("<order>");//<!--订单信息-->
        //    builderOrder.AppendFormat("<order_num>{0}</order_num>", order_num);//<!--订单号 y-->
        //    builderOrder.AppendFormat("<old_mobile>{0}</old_mobile>", old_mobile);//<!-- 原手机号 y-->
        //    builderOrder.AppendFormat("<new_mobile>{0}</new_mobile>", new_mobile);//<!-- 新手机号 y -->
        //    builderOrder.AppendFormat("</order>");
        //    builderOrder.AppendFormat("</business_trans>");

        //    try
        //    {
        //        TicketServicePortTypeClient its10 = new TicketServicePortTypeClient();
        //        string en = DESEnCode(builderOrder.ToString(), skey);
        //        string retxmls = its10.getEleInterface(organization, en);
        //        string de = DESDeCode(retxmls, skey);
        //        Literal5.Text = de;
        //        //Response.Write(DESDeCode(retxmls, skey));
        //        //Response.Flush();
        //        //Response.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        Literal5.Text = e.Message;
        //        //Response.Write(e.Message);
        //        //Response.Flush();
        //        //Response.Close();
        //    }


        //}

        ///// <summary>
        ///// 产生6位随机数
        ///// </summary>
        ///// <returns></returns>
        //public string CreateNum()
        //{
        //    string result = "";
        //    Random r = new Random();
        //    StringBuilder sb;

        //    sb = new StringBuilder();
        //    for (int j = 0; j < 6; j++)
        //    {
        //        sb.Append(r.Next(10));
        //    }
        //    result = sb.ToString();
        //    return result;

        //}
        ////#region DESEnCode DES加密
        //public string DESEnCode(string pToEncrypt, string sKey)
        //{
        //    pToEncrypt = HttpContext.Current.Server.UrlEncode(pToEncrypt);
        //    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //    byte[] inputByteArray = Encoding.GetEncoding("UTF-8").GetBytes(pToEncrypt);
        //    des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
        //    des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
        //    des.Mode = CipherMode.ECB;
        //    des.Padding = PaddingMode.PKCS7;
        //    MemoryStream ms = new MemoryStream();
        //    CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
        //    cs.Write(inputByteArray, 0, inputByteArray.Length);
        //    cs.FlushFinalBlock();
        //    StringBuilder ret = new StringBuilder();
        //    foreach (byte b in ms.ToArray())
        //    {
        //        ret.AppendFormat("{0:x2}", b);
        //    }

        //    return ret.ToString();
        //}


        ////#region DESDeCode DES解密
        //public string DESDeCode(string pToDecrypt, string sKey)
        //{
        //    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //    byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
        //    for (int x = 0; x < pToDecrypt.Length / 2; x++)
        //    {

        //        int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
        //        inputByteArray[x] = (byte)i;
        //    }
        //    des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
        //    des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
        //    des.Mode = CipherMode.ECB;
        //    des.Padding = PaddingMode.PKCS7;
        //    MemoryStream ms = new MemoryStream();
        //    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
        //    cs.Write(inputByteArray, 0, inputByteArray.Length);
        //    cs.FlushFinalBlock();
        //    StringBuilder ret = new StringBuilder();
        //    return HttpContext.Current.Server.UrlDecode(System.Text.Encoding.Default.GetString(ms.ToArray()));
        //}
        ////#endregion

    }
}
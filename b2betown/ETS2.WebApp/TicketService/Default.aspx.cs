using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETS.Framework;
using ETS2.PM.Service.PMService.Modle.Enum;
using System.Text;
using System.Security.Cryptography;
using ETS.JsonFactory;
using ETS2.VAS.Service.VASService.Data;
using ETS2.VAS.Service.VASService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.PM.Service.PMService.Modle;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using System.Text.RegularExpressions;
using WxPayAPI;
using Com.Tenpay.WxpayApi.sysprogram.model;
using Com.Tenpay.WxpayApi.sysprogram.data;
using ETS2.CRM.Service.CRMService.Modle.Enum;
using System.Xml;
using ETS2.VAS.Service.VASService.Modle.Enum;
using Newtonsoft.Json;
using ETS2.WeiXin.Service.WinXinService.BLL;
using ETS2.WeiXin.Service.WeiXinService.Model;
using ETS2.WeiXin.Service.WeiXinService.Data;



namespace ETS2.WebApp.TicketService
{

    public partial class Default : System.Web.UI.Page
    {
        public string SERCERT = "513536f17bce192a9419c68583c172b0";   //电子凭证下发的验证秘钥

        public string CHARSET = "GBK";   //使用的字符集

        public string param = "consume_type=0&item_title=城市海景水上乐园测试产品，不要拍&method=send&mobile=13522401292&num=1&num_iid=44898039857&order_id=927413999524762&seller_nick=快乐票务&send_type=2&sign=53E8EAC10D514E60CEABBC60592ADC8C&sku_properties=门票种类:儿童票;门票类型:电子票&sms_template=验证码$code.您已成功订购快乐票务提供的城市海景水上乐园测试产品，不要拍,有效期2015/04/22至2015/04/30,消费时请出示本短信以验证.如有疑问,请联系卖家.&sub_method=1&sub_outer_iid=2503&taobao_sid=51061508&timestamp=2015-04-22 13:49:29&token=fad5ab5ef8a9adc131fd54d476dbb778&type=0&valid_ends=2015-04-30 23:59:59&valid_start=2015-04-22 00:00:00&weeks=[1,2,3,4,5,6,7]";

        private static object lockobj = new object();

        protected void Page_Load(object sender, EventArgs e)
        {
            //string entrystr = CreateSign(param, SERCERT);
            //Label1.Text = entrystr;

            //string s = new GetUrlData().HttpPost("http://test.etown.cn/taobao_ms/notice.ashx", "consume_type=0&encrypt_mobile=135****1292&item_title=测 shi用&md5_mobile=6822345AB56EF852B8642667C00BDB2D&method=send&num=1&num_iid=45314406703&order_id=962887495594762&seller_nick=快乐票务&send_type=2&sign=AF6853A9CB546199D2C8511A19C3C6C7&sku_properties=门票种类:成人票;门票类型:电子票&sms_template=验证码$code.您已成功订购快乐票务提供的测 shi用,有效期2015/06/10至2015/06/10,消费时请出示本短信以验证.如有疑问,请联系卖家.&sub_method=1&sub_outer_iid=5100&taobao_sid=51061508&timestamp=2015-06-10 10:56:08&token=bfbcff3b07ede0e9e3aef0f5e7570594&type=1&valid_ends=2015-06-10 23:59:59&valid_start=2015-06-10 00:00:00&weeks=[1,2,3,4,5,6,7]");
            //var orderdata = OrderJsonData.insertyuohuiquan(4495, "osaHEjsHg4_hZUHVHPvT4aRUV67M");

            //测试 分销订单
            //int orderid = 0;
            //string data = OrderJsonData.AgentOrder(11, "1078", "1", "3", "test", "13488761102", "2015-5-12", "etowncs", 0, out orderid, 0,1, "", "", "", 0, "", "", "", "", 0,0,0,1);


           //  string RequestDomin = "shop106.etown.cn";
           // string Requestfile = System.Web.HttpContext.Current.Request.ServerVariables["Url"].ToLower();
            //根据访问的域名获得公司信息
           // WeiXinBasic basicc = new WeiXinBasicData().GetWeiXinBasicByDomain(RequestDomin);


           // 微信端模板消息发送测试
            //new Weixin_tmplmsgManage().WxTmplMsg_OrderNewCreate(21736);
            ////2时间差测试
            //TimeSpan ts = DateTime.Parse("2014-08-05 20:20:20") - DateTime.Parse("2014-08-03 20:20:20");
            //Label1.Text = ts.TotalSeconds.ToString();

            ////3微信端接收消息测试
            WeiXinBasic basic = new WeiXinBasicData().GetWxBasicByComId(2571);
            string postStr = "<xml>" +
                       "<ToUserName><![CDATA[gh_d1adc6b97854]]></ToUserName>" +
                       "<FromUserName><![CDATA[o3XEUwpl8H0NeDkxwARaxuNllgi4]]></FromUserName> " +
                       "<CreateTime>" + new WeiXinManage().ConvertDateTimeInt(DateTime.Now) + "</CreateTime>" +
                       "<MsgType><![CDATA[text]]></MsgType>" +
                       "<Content><![CDATA[123132123132312313212313123]]></Content>" +
                       "<MsgId>1234567890123456</MsgId>" +
                       "</xml>";

            //string postStr = "<xml><ToUserName><![CDATA[gh_262af05c61ba]]></ToUserName>" +
            //                    "<FromUserName><![CDATA[osaHEjsHg4_hZUHVHPvT4aRUV67M]]></FromUserName>" +
            //                    "<CreateTime>" + new WeiXinManage().ConvertDateTimeInt(DateTime.Now) + "</CreateTime>" +
            //                    "<MsgType><![CDATA[event]]></MsgType>" +
            //                    "<Event><![CDATA[scancode_push]]></Event>" +
            //                    "<EventKey><![CDATA[1375]]></EventKey>" +
            //                    "<ScanCodeInfo><ScanType><![CDATA[qrcode]]></ScanType>" +
            //                    "<ScanResult><![CDATA[1]]></ScanResult>" +
            //                    "</ScanCodeInfo>" +
            //                    "</xml>";
            new WeiXinManage().Handle(postStr, basic);


            //查询支付，并修改为支付成功
            //basicc.Weixintype = 4;
            //int order_no = 0;
            //string retunstr = new PayReturnSendEticketData().PayReturnSendEticket("1006800634201505130128130325", order_no, 0, "TRADE_SUCCESS1");
            //var testweixin = new WeiXinManage();
            //var test = testweixin.WeixinMessageChannel(0, 106, "osaHEjoMhcIIjzUjcbCl0O8BhpDI", "test", 1, "", basicc);
            //var retmessage = WeixinMessageChannel(0, basic.Comid, requestXML.FromUserName, requestXML.Content, 1, "", basic);
        }
        public static string CreateSign(string paramToMerchant, string secret)
        {
            string[] paramList = paramToMerchant.Split('&');
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            for (int i = 1; i < paramList.Length; i++)
            {
                string[] paramKeyValue = paramList[i].Split('=');
                if (paramKeyValue.Length == 2)
                {
                    parameters.Add(paramKeyValue[0], paramKeyValue[1]);
                }
            }



            parameters.Remove("sign");

            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            StringBuilder query = new StringBuilder(secret);
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = GBKUrlEncode(dem.Current.Value);
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append(value);
                }
            }
            //query.Append(secret);

            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
            //byte[] bytes = md5.ComputeHash(Encoding.GetEncoding("GBK").GetBytes(query.ToString()));

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                string hex = bytes[i].ToString("X");
                if (hex.Length == 1)
                {
                    result.Append("0");
                }
                result.Append(hex);
            }

            return result.ToString();
        }


        /// <summary>
        ///  枚举处理
        /// </summary>
        public void EnumProcess()
        {
            Dictionary<string, string> list = EnumUtils.GetValueName(typeof(PayStatus));
            string s = "";
            foreach (var item in list)
            {
                s += item.Key + "-" + item.Value + "<br>";
            }
        }

        //地址编码 gbk
        public static string GBKUrlEncode(string dataStr)
        {
            return HttpUtility.UrlEncode(dataStr, Encoding.GetEncoding("GBK"));
        }

        //地址解码 gbk
        public static string GBKUrlDecode(string dataStr)
        {
            return HttpUtility.UrlDecode(dataStr, Encoding.GetEncoding("GBK"));
        }

        /// <summary>
        /// 测试去哪消费通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            string pno = TextBox1.Text.Trim();
            var eticketinfo = new B2bEticketData().GetEticketDetail(pno);
            B2b_order ordermodel = new B2bOrderData().GetOrderById(eticketinfo.Oid);
            EticketJsonData.AsyncSend_qunar(ordermodel);

        }

        protected void Unnamed1_Click(object sender, EventArgs e)
        {
            string phone = TextBox1.Text.Trim();
            string fphone = @"^1\d{2}\*\*\*\*\d{4}$";
            Regex dReg = new Regex(fphone);

            if (dReg.IsMatch(phone))
            {
                Label1.Text = "成功";
            }
            else
            {
                Label1.Text = "失败";
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            string transaction_id = Transaction_id.Text.Trim();
            string out_trade_no = Out_trade_no.Text.Trim();
            string out_refund_no = Out_refund_no.Text.Trim();
            string total_fee = Total_fee.Text.Trim();
            string refund_fee = Refund_fee.Text.Trim();


            //根据订单id得到支付信息，进而得到公司的微信支付设置信息
            B2b_pay modelb2pay = new B2bPayData().GetPayByoId(out_trade_no.ConvertTo<int>(0));
            if (modelb2pay == null)
            {
                Label1.Text = "支付信息不存在";
                return;
            }

            B2b_finance_paytype model = new B2b_finance_paytypeData().GetFinancePayTypeByComid(modelb2pay.comid);
            if (model == null)
            {
                Label1.Text = "微信支付设置信息不存在";
                return;
            }


            WxPayConfig config = new WxPayConfig
            {
                APPID = model.Wx_appid,
                APPSECRET = model.Wx_appkey,
                KEY = model.Wx_paysignkey,
                MCHID = model.Wx_partnerid,
                IP = CommonFunc.GetRealIP(),
                SSLCERT_PATH = model.wx_SSLCERT_PATH,
                SSLCERT_PASSWORD = model.wx_SSLCERT_PASSWORD,
                PROXY_URL = "http://115.28.38.65:80",
                LOG_LEVENL = 3,//日志级别
                REPORT_LEVENL = 0//上报信息配置
            };


            lock (lockobj)
            {
                ////根据商户退款单号 判断退款是否已经申请成功
                //bool issuc = new B2b_pay_wxrefundlogData().JudgerefundByout_refund_no(out_refund_no);

                //string r = "";
                //if (issuc)
                //{
                //    r = "退款已经申请成功";
                //}
                //else
                //{
                //r = Refund.Run(transaction_id, out_trade_no, total_fee, refund_fee, out_refund_no, config);
                //}
                B2b_pay_wxrefundlog refundlog = new B2b_pay_wxrefundlog
                {
                    id = 0,
                    out_refund_no = out_refund_no,
                    out_trade_no = out_trade_no,
                    transaction_id = transaction_id,
                    total_fee = int.Parse(total_fee),
                    refund_fee = int.Parse(refund_fee),
                    send_xml = "",
                    send_time = DateTime.Parse("1970-01-01 00:00:00"),
                    return_code = "",
                    return_msg = "",
                    err_code = "",
                    err_code_des = "",
                    refund_id = "",
                    return_xml = "",
                    return_time = DateTime.Parse("1970-01-01 00:00:00"),

                };
                try
                {

                    WxPayData data = new WxPayData();

                    data.SetValue("transaction_id", transaction_id);
                    data.SetValue("out_trade_no", out_trade_no);
                    data.SetValue("total_fee", int.Parse(total_fee));//订单总金额
                    data.SetValue("refund_fee", int.Parse(refund_fee));//退款金额
                    data.SetValue("out_refund_no", out_refund_no);//随机生成商户退款单号
                    data.SetValue("op_user_id", config.MCHID);//操作员，默认为商户号
                    data.SetValue("appid", config.APPID);//公众账号ID
                    data.SetValue("mch_id", config.MCHID);//商户号
                    data.SetValue("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
                    data.SetValue("sign", data.MakeSign(config));//签名

                    string xml = data.ToXml(config);
                    var start = DateTime.Now;

                    refundlog.send_xml = xml;
                    refundlog.send_time = start;

                    int ee = new B2b_pay_wxrefundlogData().Editwxrefundlog(refundlog);
                    refundlog.id = ee;
                    if (ee == 0)
                    {
                        Label1.Text = "记录微信退款日志出错";
                        return;
                    }

                    string r = Refund.Run(transaction_id, out_trade_no, total_fee, refund_fee, out_refund_no, config);



                    string[] str = r.Replace("<br>", ",").Split(',');
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    foreach (string s in str)
                    {
                        if (s != "")
                        {
                            dic.Add(s.Split('=')[0], s.Split('=')[1]);
                        }
                    }
                    string return_code = dic["return_code"];
                    refundlog.return_code = return_code;
                    if (dic.ContainsKey("return_msg"))
                    {
                        string return_msg = dic["return_msg"];
                        refundlog.return_msg = return_msg;
                    }
                    if (dic.ContainsKey("err_code"))
                    {
                        string err_code = dic["err_code"];
                        refundlog.err_code = err_code;
                    }
                    if (dic.ContainsKey("err_code_des"))
                    {
                        string err_code_des = dic["err_code_des"];
                        refundlog.err_code_des = err_code_des;
                    }
                    if (dic.ContainsKey("refund_id"))
                    {
                        string refund_id = dic["refund_id"];
                        refundlog.refund_id = refund_id;
                    }
                    refundlog.return_xml = r;
                    refundlog.return_time = DateTime.Now;
                    new B2b_pay_wxrefundlogData().Editwxrefundlog(refundlog);
                    Label1.Text = r;
                }
                catch (Exception ef)
                {
                    Label1.Text = "记录微信退款日志出错:" + ef.Message;
                    return;

                }


            }




        }

        //验证通知
        protected void Button4_Click(object sender, EventArgs e)
        {

            string pno= TextBox2.Text.Trim();
            if (pno =="")
            {
                Label1.Text = "电子码不可为空";
                return;

            }
            if (pno.Length == 12 || pno.Length == 13)
            {

            }
            else 
            {
                Label1.Text = "电子码格式有误";
                return;

            }
            int bindorderid = new B2bOrderData().GetOrderidbypno(pno);
            if (bindorderid == 0)
            {
                Label1.Text = "电子码输入有误";
                return;

            }

            //a订单:原分销向指定商户提交的订单;b订单:指定商户下的绑定分销向拥有产品的商户提交的订单
            //电子票表中记录的Oid是b订单
            //判断b订单 是否 属于某个a订单 
           

            B2b_order loldorder = new B2bOrderData().GetOldorderBybindingId(bindorderid);
            if (loldorder != null)
            {
                //得到a订单的分销信息
                if (loldorder.Agentid > 0)
                {
                    Agent_company acompany = new AgentCompanyData().GetAgentCompany(loldorder.Agentid);
                    if (acompany != null)
                    {
                        string agent_updateurl = acompany.Agent_updateurl;
                        string agent_Inter_deskey = acompany.Inter_deskey;

                        int a_comid = loldorder.Comid;
                        int a_orderid = loldorder.Id;
                        int a_agentid = loldorder.Agentid;

                        if (agent_updateurl.Trim() != "" && agent_Inter_deskey != "")
                        {
                            //根据bingorderid 得到验证成功日志列表

                            List<B2b_eticket_log> loglist = new B2bEticketLogData().GeteticketloglistByorderid(bindorderid);
                            if (loglist.Count > 0)
                            {
                                foreach (B2b_eticket_log log in loglist)
                                {
                                    if (log != null)
                                    {
                                        AsyncSend(agent_updateurl, log.Pno, log.Use_pnum, log.Actiondate.ToString("yyyy-MM-dd HH:mm:ss"), a_agentid, a_comid, log.Id);

                                    }
                                }
                                Label1.Text = "验证通知发送成功";
                                return;
                            }
                            else
                            {
                                Label1.Text = "根据订单得到验证日志出错";
                                return;
                            }

                        }
                    }
                    else
                    {
                        Label1.Text = "分销订单查询出错";
                        return;
                    }
                }
                else
                {
                    Label1.Text = "不是分销订单";
                    return;
                }

            }
        }
        public void AsyncSend(string updateurl, string pno, int confirmnum, string confirmtime, int agentcomid, int comidd1, int validateticketlogid)
        {
            Agent_asyncsendlog log = new Agent_asyncsendlog
            {
                Id = 0,
                Pno = pno,
                Num = confirmnum,
                Sendtime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                Confirmtime = DateTime.Parse(confirmtime),
                Issendsuc = 0,//0失败；1成功
                Agentupdatestatus = (int)AgentUpdateStatus.Fail,
                Agentcomid = agentcomid,
                Comid = comidd1,
                Remark = "",
                Issecondsend = 0,
                Platform_req_seq = (1000000000 + agentcomid).ToString() + DateTime.Now.ToString("yyyyMMddhhssmm") + CommonFunc.CreateNum(6),//请求流水号
                Request_content = "",
                Response_content = "",
                b2b_etcket_logid = validateticketlogid
            };
            int inslog = new Agent_asyncsendlogData().EditLog(log);
            log.Id = inslog;
            try
            {


                //获得分销商信息
                Agent_company agentinfo = new AgentCompanyData().GetAgentCompany(agentcomid);
                if (agentinfo != null)
                {
                    string agent_updateurl = agentinfo.Agent_updateurl;

                    #region 糯米分销
                    if (agentinfo.Agent_type == (int)AgentCompanyType.NuoMi)//糯米分销
                    {

                        string re = new GetUrlData().HttpPost(updateurl, "");

                        XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode("{\"root\":" + re + "}");
                        XmlElement root = doc.DocumentElement;
                        string info = root.SelectSingleNode("info").InnerText;

                        //只要返回了数据，则是发送成功
                        log.Id = inslog;
                        log.Issendsuc = 1;
                        log.Request_content = updateurl;
                        log.Response_content = re;

                        if (info == "success")
                        {

                            string errCode = root.SelectSingleNode("res/errCode").InnerText;//分销商更新结果
                            if (errCode == "0")
                            {
                                log.Agentupdatestatus = (int)AgentUpdateStatus.Suc;
                                new Agent_asyncsendlogData().EditLog(log);
                            }
                            else
                            {
                                log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                                new Agent_asyncsendlogData().EditLog(log);
                            }
                        }
                        else
                        {
                            log.Remark = info;
                            log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                            new Agent_asyncsendlogData().EditLog(log);
                        }
                    }
                    #endregion
                    #region 一般分销
                    else //一般分销
                    {
                        var eticketinfo = new B2bEticketData().GetEticketDetail(pno);
                        if (eticketinfo == null)
                        {
                            log.Remark = "获得电子票信息失败";//单引号替换为'',双引号不用处理;
                            log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                            new Agent_asyncsendlogData().EditLog(log);
                            return;
                        }

                        //a订单:原分销向指定商户提交的订单;b订单:指定商户下的绑定分销向拥有产品的商户提交的订单
                        //电子票表中记录的Oid是b订单
                        //判断b订单 是否 属于某个a订单 
                        B2b_order loldorder = new B2bOrderData().GetOldorderBybindingId(eticketinfo.Oid);
                        if (loldorder != null)
                        {
                            eticketinfo.Oid = loldorder.Id;
                        }

                        if (eticketinfo.Oid == 0)
                        {
                            log.Remark = "电子票对应的订单号为0";//单引号替换为'',双引号不用处理;
                            log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                            new Agent_asyncsendlogData().EditLog(log);
                            return;
                        }


                        /*根据订单号得到分销商订单请求记录(当前需要得到原订单请求流水号)*/
                        List<Agent_requestlog> listagent_rlog = new Agent_requestlogData().GetAgent_requestlogByOrdernum(eticketinfo.Oid.ToString(), "add_order", "1");
                        if (listagent_rlog == null)
                        {
                            log.Remark = "根据订单号得到分销商订单请求记录失败";//单引号替换为'',双引号不用处理;
                            log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                            new Agent_asyncsendlogData().EditLog(log);
                            return;
                        }
                        if (listagent_rlog.Count == 0)
                        {
                            log.Remark = "根据订单号得到分销商订单请求记录失败.";//单引号替换为'',双引号不用处理;
                            log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                            new Agent_asyncsendlogData().EditLog(log);
                            return;
                        }


                        /*验证通知发送内容*/
                        string sbuilder = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                        "<business_trans>" +
                        "<request_type>sync_order</request_type>" +//<!--验证同步-->
                        "<req_seq>{0}</req_seq>" +//<!--原订单请求流水号-->
                        "<platform_req_seq>{1}</platform_req_seq>" +//<!--平台请求流水号-->
                        "<order>" +//<!--订单信息-->
                        "<order_num>{2}</order_num>" +//<!-- 订单号 y-->
                        "<num>{3}</num>" +//<!-- 使用张数 -->
                        "<use_time>{4}</use_time>" +//<!--  使用时间  -->
                        "</order>" +
                        "</business_trans>", listagent_rlog[0].Req_seq, log.Platform_req_seq, eticketinfo.Oid, confirmnum, ConvertDateTimeInt(DateTime.Parse(confirmtime)));

                        updateurl += "?xml=" + sbuilder;


                        string re = new GetUrlData().HttpPost(updateurl, "");

                        //只要返回了数据，则是发送成功
                        log.Id = inslog;
                        log.Issendsuc = 1;
                        log.Request_content = updateurl;
                        log.Response_content = re;

                        log.Remark = re.Replace("'", "''");//单引号替换为'',双引号不用处理;
                        if (re.Length >= 7)
                        {
                            re = re.Substring(0, 7);
                        }
                        if (re == "success")
                        {
                            log.Agentupdatestatus = (int)AgentUpdateStatus.Suc;
                            new Agent_asyncsendlogData().EditLog(log);

                        }
                        else
                        {

                            log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                            new Agent_asyncsendlogData().EditLog(log);
                        }

                    }
                    #endregion
                }
                else
                {
                    log.Remark = "分销商获取失败";
                    log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                    new Agent_asyncsendlogData().EditLog(log);
                }



            }
            catch (Exception e)
            {
                log.Id = inslog;
                log.Remark = e.Message.Replace("'", "''");//单引号替换为'',双引号不用处理
                new Agent_asyncsendlogData().EditLog(log);
            }
        }
        private static int ConvertDateTimeInt(System.DateTime time)
        {

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

            return (int)(time - startTime).TotalSeconds;

        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            string proid = txtproid.Text ;
            string num = txtnum.Text ;
            string province = txtprovince.Text;
            string city = txtcity.Text;

            string feedetail="";
            decimal fee= new B2b_delivery_costData().Getdeliverycost_ShopCart(proid, city, num, out feedetail);
            Label1.Text = fee.ToString();
        }



    }
}
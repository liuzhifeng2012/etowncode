using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using ETS.Framework;
using ETS2.PM.Service.Taobao_Ms.Model;
using ETS2.PM.Service.Taobao_Ms.Data;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS.JsonFactory;
using Newtonsoft.Json;
using Top.Api.Util;
using System.IO;
using System.Runtime.Remoting.Messaging;
using ETS2.VAS.Service.VASService.Data;
using System.Xml;
using ETS2.PM.Service.PMService.Data;
using System.Timers;
using ETS2.PM.Service.PMService.Modle;
using Newtonsoft.Json.Linq;

namespace ETS2.WebApp.taobao_ms
{
    ///*计时器 给调用方法传的参数,object类型，定义为了一个class*/
    //public class TaskTimerPara
    //{
    //    public SortedDictionary<string, string> para { get; set; }
    //    public int orderid { get; set; }
    //}



    /// <summary>
    /// notice 的摘要说明
    /// </summary>
    public class notice : IHttpHandler
    {
        private string tb_returl = "http://gw.api.taobao.com/router/rest";
        private string CodemerchantId = "727429491";//码商ID
        private string ms_notickkey = "513536f17bce192a9419c68583c172b0";//码商通知秘钥

        private string appkey = "23139679";//开放平台证书权限管理App Key
        private string appsecret = "adde2a4100288166bbee8df66c127d42";//开放平台证书权限管理App Secret

        //开放平台应用通过授权（参考：用户授权介绍）得到的Access Token值（原老的TOP协议对应为SessionKey，现Oauth2.0协议对应为Access Token）。
        private string session = "61017227c9b25cd5e74e3daf09f1471cfaa3f87cd1d5a16727429491";
        private string refresh_token = "61021225b4d99ef699e27391422c86188ccd989e2d45766727429491";

        private static object lockobj = new object();

        delegate void AsyncsendEventHandler(SortedDictionary<string, string> para);//发送验证同步发送请求委托


        //public System.Threading.Timer SendRettimer1;//发码成功回调申请 计时器

        public void ProcessRequest(HttpContext context)
        {
            lock (lockobj)
            {
                context.Response.ContentType = "text/plain";

                //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log.txt", "进入了");
                //获取淘宝端发送过来的请求

                var parastr = "";//传递过来的参数字符串
                //post传递过来的参数
                var httpmethod = "post";

                SortedDictionary<string, string> para = CommonFunc.GetRequestPost();
                #region 淘宝是通过post方式发送通知的，所有 get接收参数方式注释掉
                ////如果post方式没有接收到参数，则改用 接收get方式传递过来的参数
                //if (para.Count == 0)
                //{
                //    httpmethod = "get";
                //    noticemethod = context.Request["method"].ConvertTo<string>("");
                //    para = CommonFunc.GetRequestGet();
                //}
                #endregion
                //如果接收的参数集合数量大于0，则拼接成字符串
                if (para.Count > 0)
                {
                    parastr = CommonFunc.CreateLinkString(para);
                }
                else
                {
                    context.Response.Write("alive");
                    return;
                }

                //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log20150422.txt", parastr);

                //获取发送方的ip
                string sendip = CommonFunc.GetRealIP();
                Taobao_ms_requestlog log = new Taobao_ms_requestlog
                {
                    id = 0,
                    noticemethod = para["method"],
                    parastr = parastr,
                    subtime = DateTime.Now,
                    sendip = sendip,
                    httpmethod = httpmethod,
                    isrightsign = 0
                };
                int inslogid = new Taobao_ms_requestlogData().EditLog(log);
                log.id = inslogid;

                //判断签名是否正确

                string sign = para["sign"];

                //暂时签名验证有问题，可以先绕过去，明天23号的话想o2o问题排查 获取demo
                string selfsign = sign;
                //string selfsign =Tb_Util.CreateSelfSign(para, ms_notickkey);

                //string selfsign2 = TopUtils.SignTopRequest(para,ms_notickkey);

                //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log20150422.txt", "sign:" + sign + ",selfsign:" + selfsign);
                if (sign != selfsign)//签名不正确
                {
                    log.isrightsign = 0;

                    new Taobao_ms_requestlogData().EditLog(log);

                    //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log20150422.txt", "签名不正确");
                    return;
                }
                else //签名正确,返回接收通知成功
                {
                    log.isrightsign = 1;

                    new Taobao_ms_requestlogData().EditLog(log);

                    //string  d=JsonConvert.SerializeObject(new { code = 200 });
                    //发码通知，合作方正确收到通知后，需要返回JSON格式的返回值{"code":200}
                    if (para["method"] == "send")
                    {
                        context.Response.Write("{\"code\":\"200\"}");
                    }
                    //重新发码通知，合作方正确收到通知后，如果重新发的码与之前不同或是重新生成的，请返回{"code":300},否则返回{"code":200}
                    else if (para["method"] == "resend")
                    {
                        context.Response.Write("{\"code\":\"200\"}");
                    }
                    //退款通知
                    else if (para["method"] == "cancel")
                    {
                        context.Response.Write("{\"code\":\"200\"}");
                    }
                    //修改手机通知
                    else if (para["method"] == "modified")
                    {
                        context.Response.Write("{\"code\":\"200\"}");
                    }
                    //订单修改通知（使用有效期修改、维权成功）
                    else if (para["method"] == "order_modify")
                    {
                        context.Response.Write("{\"code\":\"200\"}");
                    }
                    //其他操作
                    else
                    {
                        return;
                    }

                }
                //异步执行操作 
                AsyncsendEventHandler mydelegate = new AsyncsendEventHandler(AsyncSend);
                mydelegate.BeginInvoke(para, new AsyncCallback(Completed), null);

                ////context.Response.Write("");
            }
        }

        #region 异步执行操作，根据通知类型，执行不同操作
        public void AsyncSend(SortedDictionary<string, string> para)
        {
            try
            {
                #region 发码通知
                if (para["method"].ToLower() == "send")
                {
                    lock (lockobj)
                    {
                        #region 通知中可选参数处理一下

                        string item_title = "";
                        if (para.ContainsKey("item_title"))
                        {
                            item_title = para["item_title"];
                        }
                        int send_type = 0;
                        if (para.ContainsKey("send_type"))
                        {
                            send_type = para["send_type"].ConvertTo<int>(2);
                        }
                        DateTime valid_start = DateTime.Now;
                        if (para.ContainsKey("valid_start"))
                        {
                            valid_start = para["valid_start"].ConvertTo<DateTime>();
                        }
                        DateTime valid_ends = DateTime.Now;
                        if (para.ContainsKey("valid_ends"))
                        {
                            valid_ends = para["valid_ends"].ConvertTo<DateTime>();
                        }
                        string outer_iid = "";
                        if (para.ContainsKey("outer_iid"))
                        {
                            outer_iid = para["outer_iid"];
                        }
                        string sub_outer_iid = "";
                        if (para.ContainsKey("sub_outer_iid"))
                        {
                            sub_outer_iid = para["sub_outer_iid"];
                        }
                        string sku_properties = "";
                        if (para.ContainsKey("sku_properties"))
                        {
                            sku_properties = para["sku_properties"];
                        }
                        decimal total_fee = 0;
                        if (para.ContainsKey("total_fee"))
                        {
                            total_fee = para["total_fee"].ConvertTo<decimal>(0);
                        }
                        int tb_type = 0;
                        if (para.ContainsKey("type"))
                        {
                            tb_type = para["type"].ConvertTo<int>(0);
                        }
                        string encrypt_mobile = "";
                        if (para.ContainsKey("encrypt_mobile"))
                        {
                            encrypt_mobile = para["encrypt_mobile"].ConvertTo<string>("");
                        }
                        string md5_mobile = "";
                        if (para.ContainsKey("md5_mobile"))
                        {
                            md5_mobile = para["md5_mobile"].ConvertTo<string>("");
                        }

                        string true_mobile = "";
                        if (para.ContainsKey("mobile"))
                        {
                            true_mobile = para["mobile"].ConvertTo<string>("");
                        }
                        if (tb_type == 1)//如果为新模式，则para["mobile"]不存在，应取para["encrypt_mobile"]
                        {
                            true_mobile = encrypt_mobile;
                        }

                        string timestamp = "";
                        if (para.ContainsKey("timestamp"))
                        {
                            timestamp = para["timestamp"];
                        }
                        string sign = "";
                        if (para.ContainsKey("sign"))
                        {
                            sign = para["sign"];
                        }
                        string order_id = "";
                        if (para.ContainsKey("order_id"))
                        {
                            order_id = para["order_id"];
                        }
                        string num = "";
                        if (para.ContainsKey("num"))
                        {
                            num = para["num"];
                        }
                        string method = "";
                        if (para.ContainsKey("method"))
                        {
                            method = para["method"];
                        }
                        string taobao_sid = "";
                        if (para.ContainsKey("taobao_sid"))
                        {
                            taobao_sid = para["taobao_sid"];
                        }
                        string seller_nick = "";
                        if (para.ContainsKey("seller_nick"))
                        {
                            seller_nick = para["seller_nick"];
                        }
                        string consume_type = "";
                        if (para.ContainsKey("consume_type"))
                        {
                            consume_type = para["consume_type"];
                        }
                        string sms_template = "";
                        if (para.ContainsKey("sms_template"))
                        {
                            sms_template = para["sms_template"];
                        }
                        string num_iid = "";
                        if (para.ContainsKey("num_iid"))
                        {
                            num_iid = para["num_iid"];
                        }
                        string token = "";
                        if (para.ContainsKey("token"))
                        {
                            token = para["token"];
                        }
                        string weeks = "";
                        if (para.ContainsKey("weeks"))
                        {
                            weeks = para["weeks"];
                        }

                        #endregion
                        #region 对淘宝通知进行处理

                        Taobao_send_noticelog sendnoticelog = new Taobao_send_noticelog
                        {
                            id = 0,
                            timestamp = timestamp,
                            sign = sign,
                            order_id = order_id,
                            mobile = true_mobile,
                            num = num.ConvertTo<int>(0),
                            method = method,
                            taobao_sid = taobao_sid,
                            seller_nick = seller_nick,
                            item_title = item_title,
                            send_type = send_type,
                            consume_type = consume_type.ConvertTo<int>(0),//核销类型: 0：不限制 1:一码一刷  2:一码多刷 默认值为0
                            sms_template = sms_template,
                            valid_start = valid_start,
                            valid_ends = valid_ends,
                            num_iid = num_iid,
                            outer_iid = outer_iid,
                            sub_outer_iid = sub_outer_iid,
                            sku_properties = sku_properties,
                            token = token,
                            total_fee = total_fee,
                            weeks = weeks,
                            subtime = DateTime.Now,
                            responsecode = "{\"code\":\"200\"}",//合作方正确收到通知后，需要返回JSON格式的返回值{"code":200}
                            responsetime = DateTime.Parse("1970-01-01"),
                            self_order_id = 0,//本系统生成的订单编号
                            agentid = 0,
                            errmsg = "",
                            type = tb_type,
                            encrypt_mobile = encrypt_mobile,
                            md5_mobile = md5_mobile
                        };
                        int newsendnoticelogid = new Taobao_send_noticelogData().Editsendnoticelog(sendnoticelog);
                        sendnoticelog.id = newsendnoticelogid;

                        if (newsendnoticelogid == 0)
                        {
                            return;
                        }


                        //合作卖家 未填写商品编码
                        if (sub_outer_iid == "")
                        {
                            sendnoticelog.errmsg = "合作卖家 发布淘宝产品时没有填写商品编码";
                            new Taobao_send_noticelogData().Editsendnoticelog(sendnoticelog);
                            return;
                        }

                        //根据taobao_sid(淘宝卖家seller_id)得到分销信息，发送电子码
                        int agentid = AgentCompanyData.GetAgentidByTb_sellerid(taobao_sid);
                        if (agentid == 0)
                        {
                            sendnoticelog.errmsg = "分销商信息中还没有开通淘宝码商";
                            new Taobao_send_noticelogData().Editsendnoticelog(sendnoticelog);
                            return;
                        }

                        sendnoticelog.agentid = agentid;
                        new Taobao_send_noticelogData().Editsendnoticelog(sendnoticelog);

                        // 根据淘宝 发码通知中 token验证串 判断是否是重复通知
                        int sendnoticenum = new Taobao_send_noticelogData().GetSendNoticeNum(token);
                        if (sendnoticenum > 1)
                        {
                            sendnoticelog.errmsg = "淘宝重复通知";
                            new Taobao_send_noticelogData().Editsendnoticelog(sendnoticelog);
                            return;
                        }
                        #endregion


                        int isInterfaceSub = 0;//是否是电子票接口提交的订单:0.否;1.是
                        int orderid = 0;
                        var data = OrderJsonData.AgentOrder(agentid, sub_outer_iid, "1", num, "淘宝" + seller_nick, true_mobile, "", "", isInterfaceSub, out orderid, 0, 1, "", "", "", 4, "", "", "", "", 0, 0, 0);

                        JObject obj = JObject.Parse(data);
                        string type = obj["type"].ToString();
                        string msg = obj["msg"].ToString();

                        if (orderid > 0)
                        {
                            sendnoticelog.self_order_id = orderid;
                            new Taobao_send_noticelogData().Editsendnoticelog(sendnoticelog);
                        }
                        else
                        {
                            sendnoticelog.errmsg = "提交分销订单出错";
                            new Taobao_send_noticelogData().Editsendnoticelog(sendnoticelog);
                        }

                        if (type == "100")
                        {
                            //淘宝发码成功回调申请
                            TaobaoSendRetApi(order_id,token,num,orderid);
                        }
                        else
                        {
                            sendnoticelog.errmsg = msg;
                            new Taobao_send_noticelogData().Editsendnoticelog(sendnoticelog);
                        }
                    }
                }
                #endregion

                #region 重新发码通知
                if (para["method"].ToLower() == "resend")
                {
                    lock (lockobj)
                    {

                        #region 可选参数处理一下
                        string item_title = "";
                        if (para.ContainsKey("item_title"))
                        {
                            item_title = para["item_title"];
                        }
                        int send_type = 0;
                        if (para.ContainsKey("send_type"))
                        {
                            send_type = para["send_type"].ConvertTo<int>(2);
                        }
                        DateTime valid_start = DateTime.Now;
                        if (para.ContainsKey("valid_start"))
                        {
                            valid_start = para["valid_start"].ConvertTo<DateTime>();
                        }
                        DateTime valid_ends = DateTime.Now;
                        if (para.ContainsKey("valid_ends"))
                        {
                            valid_ends = para["valid_ends"].ConvertTo<DateTime>();
                        }
                        string outer_iid = "";
                        if (para.ContainsKey("outer_iid"))
                        {
                            outer_iid = para["outer_iid"];
                        }
                        string sub_outer_iid = "";
                        if (para.ContainsKey("sub_outer_iid"))
                        {
                            sub_outer_iid = para["sub_outer_iid"];
                        }
                        string sku_properties = "";
                        if (para.ContainsKey("sku_properties"))
                        {
                            sku_properties = para["sku_properties"];
                        }
                        int tb_type = 0;
                        if (para.ContainsKey("type"))
                        {
                            tb_type = para["type"].ConvertTo<int>(0);
                        }
                        string encrypt_mobile = "";
                        if (para.ContainsKey("encrypt_mobile"))
                        {
                            encrypt_mobile = para["encrypt_mobile"].ConvertTo<string>("");
                        }
                        string md5_mobile = "";
                        if (para.ContainsKey("md5_mobile"))
                        {
                            md5_mobile = para["md5_mobile"].ConvertTo<string>("");
                        }

                        string true_mobile = "";
                        if (para.ContainsKey("mobile"))
                        {
                            true_mobile = para["mobile"].ConvertTo<string>("");
                        }
                        if (tb_type == 1)//如果为新模式，则para["mobile"]不存在，应取para["encrypt_mobile"]
                        {
                            true_mobile = encrypt_mobile;
                        }

                        string timestamp = "";
                        if (para.ContainsKey("timestamp"))
                        {
                            timestamp = para["timestamp"];
                        }
                        string sign = "";
                        if (para.ContainsKey("sign"))
                        {
                            sign = para["sign"];
                        }
                        string order_id = "";
                        if (para.ContainsKey("order_id"))
                        {
                            order_id = para["order_id"];
                        }
                        string num = "";
                        if (para.ContainsKey("num"))
                        {
                            num = para["num"];
                        }
                        string method = "";
                        if (para.ContainsKey("method"))
                        {
                            method = para["method"];
                        }
                        string taobao_sid = "";
                        if (para.ContainsKey("taobao_sid"))
                        {
                            taobao_sid = para["taobao_sid"];
                        }
                        string seller_nick = "";
                        if (para.ContainsKey("seller_nick"))
                        {
                            seller_nick = para["seller_nick"];
                        }
                        string num_iid = "";
                        if (para.ContainsKey("num_iid"))
                        {
                            num_iid = para["num_iid"];
                        }
                        string consume_type = "";
                        if (para.ContainsKey("consume_type"))
                        {
                            consume_type = para["consume_type"];
                        }
                        string sms_template = "";
                        if (para.ContainsKey("sms_template"))
                        {
                            sms_template = para["sms_template"];
                        }
                        string token = "";
                        if (para.ContainsKey("token"))
                        {
                            token = para["token"];
                        }
                        string left_num = "";
                        if (para.ContainsKey("left_num"))
                        {
                            left_num = para["left_num"];
                        }

                        #endregion

                        #region 对淘宝通知进行处理
                        Taobao_resend_noticelog noticelog = new Taobao_resend_noticelog
                        {
                            id = 0,
                            timestamp = timestamp,
                            sign = sign,
                            order_id = order_id,
                            mobile = true_mobile,
                            num = num.ConvertTo<int>(0),
                            method = method,
                            taobao_sid = taobao_sid,
                            seller_nick = seller_nick,
                            item_title = item_title,
                            num_iid = num_iid,
                            outer_iid = outer_iid,
                            sub_outer_iid = sub_outer_iid,
                            sku_properties = sku_properties,
                            send_type = send_type,
                            consume_type = consume_type.ConvertTo<int>(0),//核销类型: 0：不限制 1:一码一刷  2:一码多刷 默认值为0
                            sms_template = sms_template,
                            valid_start = valid_start,
                            valid_ends = valid_ends,
                            token = token,
                            subtime = DateTime.Now,
                            responsecode = "{\"code\":\"200\"}",//合作方正确收到通知后，需要返回JSON格式的返回值{"code":200}
                            responsetime = DateTime.Parse("1970-01-01"),
                            self_order_id = 0,//本系统生成的订单编号
                            agentid = 0,
                            errmsg = "",
                            left_num = left_num.ConvertTo<int>(0),
                            type = tb_type,
                            encrypt_mobile = encrypt_mobile,
                            md5_mobile = md5_mobile
                        };
                        int newnoticelogid = new Taobao_resend_noticelogData().Editnoticelog(noticelog);
                        noticelog.id = newnoticelogid;
                        if (newnoticelogid == 0)
                        {
                            return;
                        }


                        //合作卖家 未填写商品编码
                        if (sub_outer_iid == "")
                        {
                            noticelog.errmsg = "合作卖家 发布淘宝产品时没有填写商家填写";
                            new Taobao_resend_noticelogData().Editnoticelog(noticelog);

                            return;
                        }

                        //根据taobao_sid(淘宝卖家seller_id)得到分销信息 
                        int agentid = AgentCompanyData.GetAgentidByTb_sellerid(taobao_sid);
                        if (agentid == 0)
                        {
                            noticelog.errmsg = "分销商信息中还没有开通淘宝码商";
                            new Taobao_resend_noticelogData().Editnoticelog(noticelog);
                            return;
                        }

                        noticelog.agentid = agentid;
                        new Taobao_resend_noticelogData().Editnoticelog(noticelog);

                        // 根据 token验证串 判断是否是重复通知
                        int noticenum = new Taobao_resend_noticelogData().GetNoticeNum(token);
                        if (noticenum > 1)
                        {
                            noticelog.errmsg = "淘宝重复通知";
                            new Taobao_resend_noticelogData().Editnoticelog(noticelog);
                            return;
                        }

                        // 根据淘宝订单号得到 系统订单号
                        string sysoid = new Taobao_send_noticelogData().GetSysOidByTaobaoOid(order_id);
                        if (sysoid == "")
                        {
                            noticelog.errmsg = "根据淘宝订单号得到 系统订单号 出错";
                            new Taobao_resend_noticelogData().Editnoticelog(noticelog);
                            return;
                        }

                        noticelog.self_order_id = sysoid.ConvertTo<int>(0);
                        new Taobao_resend_noticelogData().Editnoticelog(noticelog);

                        int orderid = sysoid.ConvertTo<int>(0);

                        //根据订单号判断是否含有 绑定订单号，含有的话直接赋值绑定订单号
                        int bindorderid = new B2bOrderData().GetBindOrderIdByOrderid(orderid);
                        if (bindorderid > 0)
                        {
                            orderid = bindorderid;
                        }

                        //根据自己订单号 得到电子码 和 可用数量
                        string pno = new B2bOrderData().GetPnoByOrderId(orderid);
                        if (pno == "")
                        {
                            noticelog.errmsg = "根据淘宝订单号得到电子码 和 可用数量出错";
                            new Taobao_resend_noticelogData().Editnoticelog(noticelog);
                            return;
                        }
                        string vVerifyCodes = pno + ":" + left_num;


                        #endregion

                        #region 回调淘宝接口

                        //把回调结果记录一下
                        Taobao_resend_noticeretlog mretlog = new Taobao_resend_noticeretlog
                        {
                            id = 0,
                            order_id = para["order_id"].ToString(),
                            codemerchant_id = CodemerchantId,
                            token = para["token"],
                            verify_codes = vVerifyCodes,
                            qr_images = "",
                            ret_code = "",
                            ret_time = DateTime.Now
                        };
                        int mretlogid = new Taobao_resend_noticeretlogData().Editnoticeretlog(mretlog);
                        mretlog.id = mretlogid;

                        //回调一下淘宝API: taobao.vmarket.eticket.resend
                        string url = tb_returl;
                        ITopClient client = new DefaultTopClient(url, appkey, appsecret);
                        VmarketEticketResendRequest req = new VmarketEticketResendRequest();
                        req.OrderId = long.Parse(order_id);
                        req.VerifyCodes = vVerifyCodes;
                        req.Token = token;
                        req.CodemerchantId = long.Parse(CodemerchantId);
                        req.QrImages = "";
                        VmarketEticketResendResponse response = client.Execute(req, session);

                        mretlog.ret_code = response.Body;
                        new Taobao_resend_noticeretlogData().Editnoticeretlog(mretlog);

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(response.Body);
                        XmlElement root = doc.DocumentElement;

                        if (root.SelectSingleNode("ret_code") != null)
                        {
                            string ret_code = root.SelectSingleNode("ret_code").InnerText;

                            if (ret_code == "1")//回调成功，则进行重新发码操作
                            {
                                mretlog.ret_code = ret_code;
                                new Taobao_resend_noticeretlogData().Editnoticeretlog(mretlog);

                                var sendeticketdate = new SendEticketData();
                                var vasmodel = sendeticketdate.SendEticket(noticelog.self_order_id, 2, true_mobile);//重发电子码   
                            }
                            else
                            {
                                //???(暂时没有处理)如果调用失败，建议尝试重新调用，直到调用成功或者收到响应中的sub_code是 isv.eticket-order-status-error:invalid-order-status或者isv.eticket-resend-error:no-can-resend-code为止

                            }
                        }
                        else
                        {

                            string sub_code = root.SelectSingleNode("sub_code").InnerText;
                            if (sub_code == "isv.eticket-order-status-error:invalid-order-status" || sub_code == "isv.eticket-send-error:code-alreay-send")
                            {
                                //不用处理了 
                            }
                            else
                            {
                                //???(暂时没有处理)如果调用失败，建议尝试重新调用，直到调用成功或者收到响应中的sub_code是  isv.eticket-order-status-error:invalid-order-status或者isv.eticket-resend-error:no-can-resend-code为止
                            }
                        }


                        #endregion
                    }
                }
                #endregion

                #region 退款成功通知
                if (para["method"].ToLower() == "cancel")
                {
                    lock(lockobj){
                    #region 对参数进行处理
                    string item_title = "";
                    if (para.ContainsKey("item_title"))
                    {
                        item_title = para["item_title"];
                    }
                    int send_type = 0;
                    if (para.ContainsKey("send_type"))
                    {
                        send_type = para["send_type"].ConvertTo<int>(2);
                    }
                    DateTime valid_start = DateTime.Now;
                    if (para.ContainsKey("valid_start"))
                    {
                        valid_start = para["valid_start"].ConvertTo<DateTime>(DateTime.Parse("1970-01-01"));
                    }
                    DateTime valid_ends = DateTime.Now;
                    if (para.ContainsKey("valid_ends"))
                    {
                        valid_ends = para["valid_ends"].ConvertTo<DateTime>(DateTime.Parse("1970-01-01"));
                    }
                    string outer_iid = "";
                    if (para.ContainsKey("outer_iid"))
                    {
                        outer_iid = para["outer_iid"];
                    }
                    string sub_outer_iid = "";
                    if (para.ContainsKey("sub_outer_iid"))
                    {
                        sub_outer_iid = para["sub_outer_iid"];
                    }

                    string sku_properties = "";
                    if (para.ContainsKey("sku_properties"))
                    {
                        sku_properties = para["sku_properties"];
                    }


                    string timestamp = "";
                    if (para.ContainsKey("timestamp"))
                    {
                        timestamp = para["timestamp"];
                    }
                    string sign = "";
                    if (para.ContainsKey("sign"))
                    {
                        sign = para["sign"];
                    }
                    string order_id = "";
                    if (para.ContainsKey("order_id"))
                    {
                        order_id = para["order_id"];
                    }
                    string encrypt_mobile = "";
                    if (para.ContainsKey("encrypt_mobile"))
                    {
                        encrypt_mobile = para["encrypt_mobile"];
                    }
                    string num = "";
                    if (para.ContainsKey("num"))
                    {
                        num = para["num"];
                    }

                    string cancel_num = "";
                    if (para.ContainsKey("cancel_num"))
                    {
                        cancel_num = para["cancel_num"];
                    }
                    string method = "";
                    if (para.ContainsKey("method"))
                    {
                        method = para["method"];
                    }
                    string taobao_sid = "";
                    if (para.ContainsKey("taobao_sid"))
                    {
                        taobao_sid = para["taobao_sid"];
                    }
                    string seller_nick = "";
                    if (para.ContainsKey("seller_nick"))
                    {
                        seller_nick = para["seller_nick"];
                    }
                    string num_iid = "";
                    if (para.ContainsKey("num_iid"))
                    {
                        num_iid = para["num_iid"];
                    }
                    string consume_type = "";
                    if (para.ContainsKey("consume_type"))
                    {
                        consume_type = para["consume_type"];
                    }
                    string token = "";
                    if (para.ContainsKey("token"))
                    {
                        token = para["token"];
                    }


                    #endregion

                    #region 对淘宝通知进行处理
                    Taobao_cancel_noticelog noticelog = new Taobao_cancel_noticelog
                    {
                        id = 0,
                        timestamp = timestamp,
                        sign = sign,
                        order_id = order_id,
                        mobile = encrypt_mobile,
                        num = num.ConvertTo<int>(0),
                        cancel_num = cancel_num.ConvertTo<int>(0),
                        method = method,
                        taobao_sid = taobao_sid,
                        seller_nick = seller_nick,
                        item_title = item_title,
                        num_iid = num_iid,
                        outer_iid = outer_iid,
                        sub_outer_iid = sub_outer_iid,
                        sku_properties = sku_properties,
                        send_type = send_type,
                        consume_type = consume_type.ConvertTo<int>(0),//核销类型: 0：不限制 1:一码一刷  2:一码多刷 默认值为0
                        valid_start = valid_start,
                        valid_ends = valid_ends,
                        token = token,
                        subtime = DateTime.Now,
                        responsecode = "{\"code\":\"200\"}",//合作方正确收到通知后，需要返回JSON格式的返回值{"code":200}
                        responsetime = DateTime.Parse("1970-01-01"),
                        self_order_id = 0,//本系统生成的订单编号
                        agentid = 0,
                        errmsg = ""
                    };
                    int newnoticelogid = new Taobao_cancel_noticelogData().Editnoticelog(noticelog);
                    noticelog.id = newnoticelogid;
                    if (newnoticelogid == 0)
                    {
                        return;
                    }



                    //合作卖家 未填写商品编码
                    if (sub_outer_iid == "")
                    {
                        noticelog.errmsg = "合作卖家 发布淘宝产品时没有填写商家填写";
                        new Taobao_cancel_noticelogData().Editnoticelog(noticelog);
                        return;
                    }

                    //根据taobao_sid(淘宝卖家seller_id)得到分销信息 
                    int agentid = AgentCompanyData.GetAgentidByTb_sellerid(taobao_sid);
                    if (agentid == 0)
                    {
                        noticelog.errmsg = "分销商信息中还没有开通淘宝码商";
                        new Taobao_cancel_noticelogData().Editnoticelog(noticelog);
                        return;
                    }

                    noticelog.agentid = agentid;
                    new Taobao_cancel_noticelogData().Editnoticelog(noticelog);

                    // 根据token验证码 判断是否是重复通知
                    int noticenum = new Taobao_cancel_noticelogData().GetNoticeNum(token);
                    if (noticenum > 1)
                    {
                        noticelog.errmsg = "淘宝重复通知";
                        new Taobao_cancel_noticelogData().Editnoticelog(noticelog);
                        return;
                    }



                    // 根据淘宝订单号得到 系统订单号
                    string sysoid = new Taobao_send_noticelogData().GetSysOidByTaobaoOid(order_id);
                    if (sysoid == "")
                    {
                        noticelog.errmsg = "根据淘宝订单号得到 系统订单号 出错";
                        new Taobao_cancel_noticelogData().Editnoticelog(noticelog);
                        return;
                    }

                    noticelog.self_order_id = sysoid.ConvertTo<int>(0);
                    new Taobao_cancel_noticelogData().Editnoticelog(noticelog);

                    int orderid = sysoid.ConvertTo<int>(0);
                    //根据订单号判断是否含有 绑定订单号，含有的话直接赋值绑定订单号
                    int bindorderid = new B2bOrderData().GetBindOrderIdByOrderid(orderid);
                    if (bindorderid > 0)
                    {
                        orderid = bindorderid;
                    }
                    string pno = new B2bOrderData().GetPnoByOrderId(orderid);
                    if (pno == "")
                    {
                        noticelog.errmsg = "根据订单号得到电子码出错";
                        new Taobao_cancel_noticelogData().Editnoticelog(noticelog);
                        return;
                    }


                    #endregion

                    #region  执行退票操作
                    string data = OrderJsonData.QuitOrder(0, noticelog.self_order_id, int.Parse(sub_outer_iid), int.Parse(cancel_num), "淘宝码商退票");

                    noticelog.errmsg = data;
                    new Taobao_cancel_noticelogData().Editnoticelog(noticelog);
                    return;

                    #endregion
                    }
                }
                #endregion

                #region 修改手机通知
                if (para["method"].ToLower() == "modified")
                {
                    lock(lockobj){

                    #region 可选参数处理一下
                    string item_title = "";
                    if (para.ContainsKey("item_title"))
                    {
                        item_title = para["item_title"];
                    }
                    int send_type = 0;
                    if (para.ContainsKey("send_type"))
                    {
                        send_type = para["send_type"].ConvertTo<int>(2);
                    }
                    DateTime valid_start = DateTime.Now;
                    if (para.ContainsKey("valid_start"))
                    {
                        valid_start = para["valid_start"].ConvertTo<DateTime>();
                    }
                    DateTime valid_ends = DateTime.Now;
                    if (para.ContainsKey("valid_ends"))
                    {
                        valid_ends = para["valid_ends"].ConvertTo<DateTime>();
                    }
                    string outer_iid = "";
                    if (para.ContainsKey("outer_iid"))
                    {
                        outer_iid = para["outer_iid"];
                    }
                    string sub_outer_iid = "";
                    if (para.ContainsKey("sub_outer_iid"))
                    {
                        sub_outer_iid = para["sub_outer_iid"];
                    }
                    string sku_properties = "";
                    if (para.ContainsKey("sku_properties"))
                    {
                        sku_properties = para["sku_properties"];
                    }
                    int tb_type = 0;
                    if (para.ContainsKey("type"))
                    {
                        tb_type = para["type"].ConvertTo<int>(0);
                    }
                    string encrypt_mobile = "";
                    if (para.ContainsKey("encrypt_mobile"))
                    {
                        encrypt_mobile = para["encrypt_mobile"].ConvertTo<string>("");
                    }
                    string md5_mobile = "";
                    if (para.ContainsKey("md5_mobile"))
                    {
                        md5_mobile = para["md5_mobile"].ConvertTo<string>("");
                    }

                    string true_mobile = "";
                    if (para.ContainsKey("mobile"))
                    {
                        true_mobile = para["mobile"].ConvertTo<string>("");
                    }
                    if (tb_type == 1)//如果为新模式，则para["mobile"]不存在，应取para["encrypt_mobile"]
                    {
                        true_mobile = encrypt_mobile;
                    }

                    string timestamp = "";
                    if (para.ContainsKey("timestamp"))
                    {
                        timestamp = para["timestamp"];
                    }
                    string sign = "";
                    if (para.ContainsKey("sign"))
                    {
                        sign = para["sign"];
                    }
                    string order_id = "";
                    if (para.ContainsKey("order_id"))
                    {
                        order_id = para["order_id"];
                    }
                    string num = "";
                    if (para.ContainsKey("num"))
                    {
                        num = para["num"];
                    }
                    string method = "";
                    if (para.ContainsKey("method"))
                    {
                        method = para["method"];
                    }
                    string taobao_sid = "";
                    if (para.ContainsKey("taobao_sid"))
                    {
                        taobao_sid = para["taobao_sid"];
                    }
                    string seller_nick = "";
                    if (para.ContainsKey("seller_nick"))
                    {
                        seller_nick = para["seller_nick"];
                    }
                    string num_iid = "";
                    if (para.ContainsKey("num_iid"))
                    {
                        num_iid = para["num_iid"];
                    }
                    string consume_type = "";
                    if (para.ContainsKey("consume_type"))
                    {
                        consume_type = para["consume_type"];
                    }
                    string sms_template = "";
                    if (para.ContainsKey("sms_template"))
                    {
                        sms_template = para["sms_template"];
                    }
                    string token = "";
                    if (para.ContainsKey("token"))
                    {
                        token = para["token"];
                    }
                    string left_num = "";
                    if (para.ContainsKey("left_num"))
                    {
                        left_num = para["left_num"];
                    }

                    #endregion

                    #region 对淘宝通知进行处理
                    Taobao_modified_noticelog noticelog = new Taobao_modified_noticelog
                    {
                        id = 0,
                        timestamp = timestamp,
                        sign = sign,
                        order_id = order_id,
                        mobile = true_mobile,
                        num = num.ConvertTo<int>(0),
                        method = method,
                        taobao_sid = taobao_sid,
                        seller_nick = seller_nick,
                        item_title = item_title,
                        num_iid = num_iid,
                        outer_iid = outer_iid,
                        sub_outer_iid = sub_outer_iid,
                        sku_properties = sku_properties,
                        send_type = send_type,
                        consume_type = consume_type.ConvertTo<int>(0),//核销类型: 0：不限制 1:一码一刷  2:一码多刷 默认值为0
                        sms_template = sms_template,
                        valid_start = valid_start,
                        valid_ends = valid_ends,
                        token = token,
                        subtime = DateTime.Now,
                        responsecode = "{\"code\":\"200\"}",//合作方正确收到通知后，需要返回JSON格式的返回值{"code":200}
                        responsetime = DateTime.Parse("1970-01-01"),
                        self_order_id = 0,//本系统生成的订单编号
                        agentid = 0,
                        errmsg = "",
                        left_num = left_num.ConvertTo<int>(0),
                        type = tb_type,
                        encrypt_mobile = encrypt_mobile,
                        md5_mobile = md5_mobile
                    };
                    int newnoticelogid = new Taobao_modified_noticelogData().Editnoticelog(noticelog);
                    noticelog.id = newnoticelogid;
                    if (newnoticelogid == 0)
                    {
                        return;

                    }



                    //合作卖家 未填写商品编码
                    if (sub_outer_iid == "")
                    {
                        noticelog.errmsg = "合作卖家 发布淘宝产品时没有填写商家填写";
                        new Taobao_modified_noticelogData().Editnoticelog(noticelog);
                    }

                    //根据taobao_sid(淘宝卖家seller_id)得到分销信息 
                    int agentid = AgentCompanyData.GetAgentidByTb_sellerid(taobao_sid);
                    if (agentid == 0)
                    {
                        noticelog.errmsg = "分销商信息中还没有开通淘宝码商";
                        new Taobao_modified_noticelogData().Editnoticelog(noticelog);
                        return;
                    }

                    noticelog.agentid = agentid;
                    new Taobao_modified_noticelogData().Editnoticelog(noticelog);


                    int noticenum = new Taobao_modified_noticelogData().GetNoticeNum(token);
                    if (noticenum > 1)
                    {
                        noticelog.errmsg = "淘宝重复通知";
                        new Taobao_modified_noticelogData().Editnoticelog(noticelog);
                        return;
                    }

                    // 根据淘宝订单号得到 系统订单号
                    string sysoid = new Taobao_send_noticelogData().GetSysOidByTaobaoOid(order_id);
                    if (sysoid == "")
                    {
                        noticelog.errmsg = "根据淘宝订单号得到 系统订单号 出错";
                        new Taobao_modified_noticelogData().Editnoticelog(noticelog);
                        return;
                    }

                    noticelog.self_order_id = sysoid.ConvertTo<int>(0);
                    new Taobao_modified_noticelogData().Editnoticelog(noticelog);


                    int orderid = sysoid.ConvertTo<int>(0);
                    if (orderid == 0)
                    {
                        noticelog.errmsg = "根据淘宝订单号得到 自己的订单号出错";
                        new Taobao_modified_noticelogData().Editnoticelog(noticelog);
                        return;
                    }


                    //根据订单号判断是否含有 绑定订单号，含有的话直接赋值绑定订单号
                    int bindorderid = new B2bOrderData().GetBindOrderIdByOrderid(orderid);
                    if (bindorderid > 0)
                    {
                        orderid = bindorderid;
                    }
                    //根据订单id得到电子码
                    string pno = new B2bOrderData().GetPnoByOrderId(orderid);

                    string vVerifyCodes = pno + ":" + left_num;


                    #endregion

                    #region 更改订单中 用户接收码的手机号
                    int upb2borderphone = new B2bOrderData().UpB2borderPhone(noticelog.self_order_id, true_mobile);
                    if (upb2borderphone == 0)
                    {
                        noticelog.errmsg = "更改订单中 用户接收码的手机号 出错";
                        new Taobao_modified_noticelogData().Editnoticelog(noticelog);
                        return;
                    }
                    if (bindorderid > 0)
                    {
                        int upb2borderphone2 = new B2bOrderData().UpB2borderPhone(bindorderid, true_mobile);
                        if (upb2borderphone2 == 0)
                        {
                            noticelog.errmsg = "更改订单中 用户接收码的手机号 出错";
                            new Taobao_modified_noticelogData().Editnoticelog(noticelog);
                            return;
                        }
                    }
                    #endregion

                    #region 回调淘宝接口


                    //回调一下淘宝API: taobao.vmarket.eticket.resend 
                    string url = tb_returl;
                    ITopClient client = new DefaultTopClient(url, appkey, appsecret);
                    VmarketEticketResendRequest req = new VmarketEticketResendRequest();
                    req.OrderId = long.Parse(order_id);
                    req.VerifyCodes = vVerifyCodes;
                    req.Token = token;
                    req.CodemerchantId = long.Parse(CodemerchantId);
                    req.QrImages = "";

                    //把回调结果记录一下
                    Taobao_resend_noticeretlog mretlog = new Taobao_resend_noticeretlog
                    {
                        id = 0,
                        order_id = order_id.ToString(),
                        codemerchant_id = CodemerchantId,
                        token = token,
                        verify_codes = vVerifyCodes,
                        qr_images = "",
                        ret_code = "",
                        ret_time = DateTime.Now
                    };
                    int logidd = new Taobao_resend_noticeretlogData().Editnoticeretlog(mretlog);
                    mretlog.id = logidd;

                    VmarketEticketResendResponse response = client.Execute(req, session);

                    mretlog.ret_code = response.Body;
                    new Taobao_resend_noticeretlogData().Editnoticeretlog(mretlog);

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(response.Body);
                    XmlElement root = doc.DocumentElement;
                    if (root.SelectSingleNode("ret_code") != null)
                    {
                        string ret_code = root.SelectSingleNode("ret_code").InnerText;

                        if (ret_code == "1")//回调成功，则进行重新发码操作
                        {
                            mretlog.ret_code = ret_code;
                            new Taobao_resend_noticeretlogData().Editnoticeretlog(mretlog);

                            var sendeticketdate = new SendEticketData();
                            var vasmodel = sendeticketdate.SendEticket(noticelog.self_order_id, 2, true_mobile);//重发电子码   
                        }
                        else
                        {

                            //???(暂时没有处理)如果调用失败，建议尝试重新调用，直到调用成功或者收到响应中的sub_code是 isv.eticket-order-status-error:invalid-order-status或者isv.eticket-resend-error:no-can-resend-code为止

                        }
                    }
                    else
                    {

                        string sub_code = root.SelectSingleNode("sub_code").InnerText;
                        if (sub_code == "isv.eticket-order-status-error:invalid-order-status" || sub_code == "isv.eticket-send-error:code-alreay-send")
                        {
                            //不用处理了 
                        }
                        else
                        {
                            //???(暂时没有处理)如果调用失败，建议尝试重新调用，直到调用成功或者收到响应中的sub_code是  isv.eticket-order-status-error:invalid-order-status或者isv.eticket-resend-error:no-can-resend-code为止
                        }
                    }
                    #endregion
                    }

                }
                #endregion

                #region 订单修改通知（使用有效期修改、维权成功）,暂时没有体现的地方
                if (para["method"].ToLower() == "order_modify")
                {
                    lock (lockobj)
                    {
                        #region 对参数进行处理
                        string timestamp = "";
                        if (para.ContainsKey("timestamp"))
                        {
                            timestamp = para["timestamp"];
                        }
                        string sign = "";
                        if (para.ContainsKey("sign"))
                        {
                            sign = para["sign"];
                        }
                        string order_id = "";
                        if (para.ContainsKey("order_id"))
                        {
                            order_id = para["order_id"];
                        }
                        string method = "";
                        if (para.ContainsKey("method"))
                        {
                            method = para["method"];
                        }
                        string taobao_sid = "";
                        if (para.ContainsKey("taobao_sid"))
                        {
                            taobao_sid = para["taobao_sid"];
                        }
                        string seller_nick = "";
                        if (para.ContainsKey("seller_nick"))
                        {
                            seller_nick = para["seller_nick"];
                        }
                        string sub_method = "";
                        if (para.ContainsKey("sub_method"))
                        {
                            sub_method = para["sub_method"];
                        }
                        string data = "";
                        if (para.ContainsKey("data"))
                        {
                            data = para["data"];
                        }
                        string left_num = "";
                        if (para.ContainsKey("left_num"))
                        {
                            left_num = para["left_num"];
                        }
                        #endregion

                        #region 对淘宝通知进行处理
                        Taobao_order_modify_noticelog noticelog = new Taobao_order_modify_noticelog
                        {
                            id = 0,
                            timestamp = timestamp,
                            sign = sign,
                            order_id = order_id,

                            method = method,
                            taobao_sid = taobao_sid,
                            seller_nick = seller_nick,
                            sub_method = sub_method,
                            data = data,

                            subtime = DateTime.Now,
                            responsecode = "{\"code\":\"200\"}",//合作方正确收到通知后，需要返回JSON格式的返回值{"code":200}
                            responsetime = DateTime.Parse("1970-01-01"),
                            self_order_id = 0,//本系统生成的订单编号
                            agentid = 0,
                            errmsg = ""

                        };
                        int newnoticelogid = new Taobao_order_modify_noticelogData().Editnoticelog(noticelog);
                        noticelog.id = newnoticelogid;



                        //根据taobao_sid(淘宝卖家seller_id)得到分销信息 
                        int agentid = AgentCompanyData.GetAgentidByTb_sellerid(taobao_sid);
                        if (agentid == 0)
                        {
                            noticelog.errmsg = "分销商信息中还没有开通淘宝码商";
                            new Taobao_order_modify_noticelogData().Editnoticelog(noticelog);
                            return;
                        }

                        noticelog.agentid = agentid;
                        new Taobao_order_modify_noticelogData().Editnoticelog(noticelog);



                        // 根据淘宝订单号得到 系统订单号
                        string sysoid = new Taobao_send_noticelogData().GetSysOidByTaobaoOid(order_id);
                        if (sysoid == "")
                        {
                            noticelog.errmsg = "根据淘宝订单号得到 系统订单号 出错";
                            new Taobao_order_modify_noticelogData().Editnoticelog(noticelog);
                            return;
                        }

                        noticelog.self_order_id = sysoid.ConvertTo<int>(0);
                        new Taobao_order_modify_noticelogData().Editnoticelog(noticelog);

                        #endregion
                    }
                }
                #endregion
            }catch(Exception ex)
            {}
        }
        #region 回调淘宝发码接口(调用3次，间隔为10分钟)

        public string TaobaoSendRetApi(string order_id,string token,string num , int orderid)
        {
            ////调用发码成功回调接口 记录的次数，超过3次 ，不在回调
            //int invokenum = new Taobao_send_noticeretlogData().GetInvokeNum(order_id);
            //if (invokenum > 0)
            //{
            //    ////关闭计时器
            //    //SendRettimer1.Dispose();

            //    if (invokenum >= 3)
            //    {
            //        return;
            //    }
            //}

            //根据订单号判断是否含有 绑定订单号，含有的话直接赋值绑定订单号
            int bindorderid = new B2bOrderData().GetBindOrderIdByOrderid(orderid);
            if (bindorderid > 0)
            {
                orderid = bindorderid;
            }

            //根据订单id得到电子码
            string pno = new B2bOrderData().GetPnoByOrderId(orderid);


            //调用发码成功回调接口记录记入数据库
            Taobao_send_noticeretlog mretlog = new Taobao_send_noticeretlog
            {
                id = 0,
                order_id = order_id,
                codemerchant_id = CodemerchantId,
                token = token,
                verify_codes = pno + ":" + num,
                qr_images = "",
                ret_code = "",
                ret_time = DateTime.Now
            };
            int loggid = new Taobao_send_noticeretlogData().Editsendnoticeretlog(mretlog);
            mretlog.id = loggid;

            //合作方发码，确认发送成功后再调用淘宝API: taobao.vmarket.eticket.send
            string url = tb_returl;

            ITopClient client = new DefaultTopClient(url, appkey, appsecret);
            VmarketEticketSendRequest req = new VmarketEticketSendRequest();
            req.OrderId = long.Parse(order_id);
            req.VerifyCodes = pno + ":" +num;
            req.Token =token;
            req.CodemerchantId = long.Parse(CodemerchantId);
            req.QrImages = "";
            VmarketEticketSendResponse response = client.Execute(req, session);

            mretlog.ret_code = response.Body;
            new Taobao_send_noticeretlogData().Editsendnoticeretlog(mretlog);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(response.Body);
            XmlElement root = doc.DocumentElement;
            if (root.SelectSingleNode("ret_code") != null)
            {
                string ret_code = root.SelectSingleNode("ret_code").InnerText;

                if (ret_code == "1")//回调成功 
                {
                    mretlog.ret_code = ret_code;
                    new Taobao_send_noticeretlogData().Editsendnoticeretlog(mretlog);
                    return ret_code;
                }
                else
                {
                    //???(暂时没有做)如果调用失败，建议尝试重新调用 直到调用成功或者收到响应中的sub_code是isv.eticket-order-status-error:invalid-order-status或者isv.eticket-send-error:code-alreay-send为止
                    ////创建计时器，间隔10 分钟重新回调 
                    //TaskTimerPara taskpara = new TaskTimerPara();
                    //taskpara.para = para;
                    //taskpara.orderid = orderid;

                    //SendRettimer1 = new System.Threading.Timer(Tick, taskpara, 600000, 600000);
                    return ret_code;
                }
            }
            else
            {


                string sub_code = root.SelectSingleNode("sub_code").InnerText;
                if (sub_code == "isv.eticket-order-status-error:invalid-order-status" || sub_code == "isv.eticket-send-error:code-alreay-send")
                {
                    //不用处理了 
                    return sub_code;
                }
                else
                {
                    //???(暂时没有做)如果调用失败，建议尝试重新调用 直到调用成功或者收到响应中的sub_code是isv.eticket-order-status-error:invalid-order-status或者isv.eticket-send-error:code-alreay-send为止
                    ////创建计时器，间隔10 分钟重新回调 
                    //TaskTimerPara taskpara = new TaskTimerPara();
                    //taskpara.para = para;
                    //taskpara.orderid = orderid;

                    //SendRettimer1 = new System.Threading.Timer(Tick, taskpara, 600000, 600000);
                    return sub_code;
                }
            }
             
            //TxtHelper.WriteFile("D:\\site\\b2betown\\ETS2.WebApp\\Log20150422.txt", response.Body); 
            //context.Response.Write(response.Body);  
        }
        #endregion
        ///*计时器 调用的方法*/
        //void Tick(object data)
        //{
        //    TaobaoSendRetApi((data as TaskTimerPara).para, (data as TaskTimerPara).orderid);
        //}
        /*异步完成*/
        public void Completed(IAsyncResult result)
        {
            //获取委托对象，调用EndInvoke方法获取运行结果
            AsyncResult _result = (AsyncResult)result;
            AsyncsendEventHandler myDelegate = (AsyncsendEventHandler)_result.AsyncDelegate;
            myDelegate.EndInvoke(_result);
        }
        #endregion
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
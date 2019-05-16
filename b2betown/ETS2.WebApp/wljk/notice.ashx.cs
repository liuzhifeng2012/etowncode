using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web.Script.Serialization;
using ETS2.CRM.Service.CRMService.Modle;
using ETS2.CRM.Service.CRMService.Data;
using ETS2.PM.Service.WL.Data;
using Newtonsoft.Json;
using ETS2.PM.Service.WL.Model;
using ETS2.PM.Service.PMService.Modle;
using ETS2.PM.Service.PMService.Data;
using ETS2.VAS.Service.VASService.Modle.Enum;
using ETS2.PM.Service.PMService.Modle.Enum;
using ETS2.VAS.Service.VASService.Data;
using ETS.Framework;
using System.Xml;
using ETS2.CRM.Service.CRMService.Modle.Enum;
using ETS2.VAS.Service.VASService.Modle;
using System.Runtime.Remoting.Messaging;
using ETS2.PM.Service.LMM.Data;
using ETS2.PM.Service.LMM.Model;
using ETS2.PM.Service.Meituan.Model;
using ETS2.PM.Service.Meituan.Data;

namespace ETS2.WebApp.wljk
{
    /// <summary>
    /// notice1 的摘要说明
    /// </summary>
    public class notice1 : IHttpHandler
    {
        delegate void AsyncsendEventHandler(string updateurl, string pno, int confirmnum, string confirmtime, int agentcomid, int comid, int validateticketlogid, int aorderid);//发送验证同步发送请求委托
        
        public void ProcessRequest(HttpContext context)
        {

            //B2b_company commanage = B2bCompanyData.GetAllComMsg(112);
            WlGetProInfoDealRequestData wldata = new WlGetProInfoDealRequestData("1","key");//核销数据时不需要读取商户
            int comid = 0;

           context.Response.ContentType = "application/x-www-form-urlencoded";
           Stream stream = context.Request.InputStream;
           if (stream.Length != 0)
           {
               try
               {
                   StreamReader streamreader = new StreamReader(stream);
                   string json = streamreader.ReadToEnd();
                   //var OnlineMsg = StreamToString(stream);
                   // context.Response.Write(OnlineMsg);
                   // context.Response.End();
                   var json_hexiao = (wlhexiao)JsonConvert.DeserializeObject(json, typeof(wlhexiao));
                   int partnerId = json_hexiao.partnerId;
                   string wlOrderId = json_hexiao.body.wlOrderId;
                   int usedQuantity = json_hexiao.body.usedQuantity;
                   int quantity = json_hexiao.body.quantity;
                   int refundedQuantity = json_hexiao.body.refundedQuantity;
                   int lastCount = 0;
                   string credence = "";

                   var orderdata = wldata.getWlOrderidData(wlOrderId);
                   if (orderdata != null)
                   {
                       comid = orderdata.comid;
                       credence = orderdata.vouchers;
                       int orderid = orderdata.orderid;//orderid
                       int partnerdealid = orderdata.partnerdealid;//产品id
                       int shengyuq=orderdata.quantity-orderdata.usedQuantity;
                       lastCount = shengyuq - usedQuantity;//剩余数量看是否大于0


                       //插入万龙核销日志插入
                       var wluselog = wldata.InsertWlUseLog(comid, wlOrderId, usedQuantity, partnerId, quantity, orderid,partnerdealid);


                       if (shengyuq >= usedQuantity)
                       {
                           orderdata.usedQuantity = orderdata.usedQuantity + usedQuantity;//库里的剩余数量加上传递过来使用数量

                           var hexiao = wldata.UpdateWlOrderPaySC(orderdata);

                           var wlhexiaore = wldata.wlhexiaotongzhi_json(200, "成功");//
                           context.Response.Write(wlhexiaore);



                           #region 验证后订单及通知发放
                           //易城系统订单 处理
                           B2b_order morder = new B2bOrderData().GetOrderById(orderid);
                           if (morder != null)
                           {
                               if (lastCount > 0)
                               {

                                   morder.service_usecount = usedQuantity;
                                   morder.service_lastcount = lastCount;
                                   morder.Servicepro_v_state = ((int)Product_V_State.PartValidate).ToString();//验证部分

                                   morder.Order_state = (int)OrderStatus.HasUsed;//已消费

                                   new B2bOrderData().InsertOrUpdate(morder);
                               }
                               else
                               {
                                   //如果剩余数量为0就，使都使用了，剩余数量为0
                                   morder.service_usecount = usedQuantity;
                                   morder.service_lastcount = 0;
                                   morder.Servicepro_v_state = ((int)Product_V_State.AllValidate).ToString();//验证全部

                                   morder.Order_state = (int)OrderStatus.HasUsed;//已消费

                                   new B2bOrderData().InsertOrUpdate(morder);
                               }

                               //根据b单判断是否有a单存在
                               B2b_order aorder = new B2bOrderData().GetOldorderBybindingId(morder.Id);
                               if (aorder != null)
                               {
                                   aorder.Order_state = morder.Order_state;

                                   new B2bOrderData().InsertOrUpdate(aorder);
                               }


                               //如果是分销订单，则需要给分销发送验证通知
                               //判断b订单 是否 属于某个a订单  
                               if (aorder != null)
                               {
                                   //得到a订单的分销信息
                                   if (aorder.Agentid > 0)
                                   {
                                       Agent_company acompany = new AgentCompanyData().GetAgentCompany(aorder.Agentid);
                                       if (acompany != null)
                                       {
                                           //异步发送验证同步请求
                                           AsyncsendEventHandler mydelegate = new AsyncsendEventHandler(AsyncSend);
                                           mydelegate.BeginInvoke(acompany.Agent_updateurl.Trim(), credence, usedQuantity, DateTime.Now.ToString(), aorder.Agentid, aorder.Comid, 0, aorder.Id, new AsyncCallback(Completed), null);
                                       }
                                   }
                               }
                               else
                               {
                                   if (morder.Agentid > 0)
                                   {
                                       Agent_company acompany = new AgentCompanyData().GetAgentCompany(morder.Agentid);
                                       if (acompany != null)
                                       {
                                           //异步发送验证同步请求
                                           AsyncsendEventHandler mydelegate = new AsyncsendEventHandler(AsyncSend);
                                           mydelegate.BeginInvoke(acompany.Agent_updateurl.Trim(), credence, usedQuantity, DateTime.Now.ToString(), morder.Agentid, morder.Comid, 0, morder.Id, new AsyncCallback(Completed), null);
                                       }
                                   }
                               }

                           }

                           #endregion


                       }
                       else {
                           var wlhexiaore = wldata.wlhexiaotongzhi_json(1, "剩余数量不符");//
                           context.Response.Write(wlhexiaore);
                       }

                   }
                   else {
                       //context.Response.Write("{code:200,describe:\"成功\"}");
                       var wlhexiaore = wldata.wlhexiaotongzhi_json(1, "未查询到订单" );//
                       context.Response.Write(wlhexiaore);
                   }

               }
               catch  (Exception ex){
                   var wlhexiaore = wldata.wlhexiaotongzhi_json(1, "异常" + ex.Message);//
                   context.Response.Write(wlhexiaore);
               }
           }
           else {
               var wlhexiaore = wldata.wlhexiaotongzhi_json(2, "未接收到数据");//
               context.Response.Write(wlhexiaore);
           }

           
            

        }


        #region 发送验证同步请求:回调函数和验证同步方法
        public static void Completed(IAsyncResult result)
        {
            //获取委托对象，调用EndInvoke方法获取运行结果
            AsyncResult _result = (AsyncResult)result;
            AsyncsendEventHandler myDelegate = (AsyncsendEventHandler)_result.AsyncDelegate;
            myDelegate.EndInvoke(_result);
        }
        public static void AsyncSend(string updateurl, string pno, int confirmnum, string confirmtime, int agentcomid, int comidd1, int validateticketlogid, int aorderid)
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
                    #region 驴妈妈
                    else if (agentinfo.Agent_type == (int)AgentCompanyType.Lvmama)
                    {
                        /*根据订单号得到分销商订单请求记录(当前需要得到原订单请求流水号)*/
                       

                        Lvmama_reqlog LvmamaOrderCrateSucLog = new lvmama_reqlogData().GetLvmama_OrderpayreqlogBySelforderid(aorderid, "0");


                        if (LvmamaOrderCrateSucLog == null)
                        {
                            log.Remark = "根据订单号得到分销商订单请求记录失败";//单引号替换为'',双引号不用处理;
                            log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                            new Agent_asyncsendlogData().EditLog(log);
                            return;
                        }

                        B2b_order morder = new B2bOrderData().GetOrderById(aorderid);
                        if(morder ==null){
                            log.Remark = "根据订单号查询订单失败.";//单引号替换为'',双引号不用处理;
                            log.Agentupdatestatus = (int)AgentUpdateStatus.Fail;
                            new Agent_asyncsendlogData().EditLog(log);
                            return;
                        }

                        string state="1";
                        if(morder.U_num>confirmnum){
                            state="2";
                        }else if(confirmnum==morder.U_num){
                            state="3";
                        } else{
                            state="1";
                        }



                        var lvmamadata = new LVMAMA_Data(agentinfo.Lvmama_uid, agentinfo.Lvmama_password, agentinfo.Lvmama_Apikey);
                        //初始的时候没有sign值，等组合后下面生成加密文件
                        var hexiaojson = lvmamadata.usedticketscallback_json(LvmamaOrderCrateSucLog.mtorderid, agentinfo.Lvmama_uid, agentinfo.Lvmama_password, state, "", DateTime.Now.ToString("yyyyMMddHHmmss"), confirmnum.ToString());

                        #region 签名验证
                        string Md5Sign = lvmamadata.usedticketscallbackmd5(hexiaojson);
                        string afterSign = lvmamadata.lumamasign(Md5Sign, agentinfo.Lvmama_Apikey);
                        #endregion
                        hexiaojson.sign = afterSign;

                        var asynnoticecall = lvmamadata.useConsumeNotify(hexiaojson, agentinfo.Id);
                        return;

                    }
                    #endregion
                    #region 一般分销
                    else //一般分销
                    {

                        #region 如果是美团分销，则向美团发送验证通知
                        Agent_company mtagentcompany = new AgentCompanyData().GetAgentCompany(agentinfo.Id);
                        if (mtagentcompany != null)
                        {
                            if (mtagentcompany.mt_partnerId != "")
                            {

                                B2b_order ordermodel = new B2bOrderData().GetOrderById(aorderid);
                                Meituan_reqlog meituanlogg = new Meituan_reqlogData().GetMt_OrderpayreqlogBySelforderid(ordermodel.Id, "200");
                                if (meituanlogg == null)
                                {
                                    LogHelper.RecordSelfLog("Erro", "meituan", "美团验证通知发送失败：查询美团支付成功订单失败");
                                }
                                else
                                {
                                    #region  订单电子票使用情况
                                    string all_pno = "";//全部电子码
                                    string keyong_pno = "";//可用电子码 
                                    int buy_num = 0;
                                    int keyong_num = 0;
                                    int consume_num = 0;
                                    int tuipiao_num = ordermodel.Cancelnum;
                                    #endregion


                                    string meituancontent = "{" +
                                    "\"partnerId\": " + mtagentcompany.mt_partnerId + "," +
                                    "\"body\": " +
                                    "{" +
                                        "\"orderId\": " + meituanlogg.mtorderid + "," +
                                        "\"partnerOrderId\": \"" + aorderid + "\"," +
                                        "\"quantity\": " + ordermodel.U_num + "," +
                                        "\"usedQuantity\": " + confirmnum + "," +
                                        "\"refundedQuantity\": " + tuipiao_num + "," +
                                        "\"voucherList\": " +
                                                         "[{" +
                                                             "\"voucher\":\"" + pno + "\"," +
                                                             "\"voucherPics\":\"\"," +
                                                             "\"voucherInvalidTime\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"," +
                                                             "\"quantity\":" + confirmnum + "," +
                                                             "\"status\":1" +//4.10 凭证码状态:0	未使用;1	已使用;2	已退款;3	已废弃 对应的门票还未消费，但是此凭证码作废了
                                                         "}]" +
                                    "}" +
                                "}";


                                    OrderConsumeNotice mrequest = (OrderConsumeNotice)JsonConvert.DeserializeObject(meituancontent, typeof(OrderConsumeNotice));

                                    ETS2.PM.Service.Meituan.Data.ReturnResult r = new MeiTuanInter(mtagentcompany.mt_partnerId, mtagentcompany.mt_secret, mtagentcompany.mt_client).ConsumeNotify(mrequest, mtagentcompany.Id);
                                }

                            }
                        }
                        #endregion


                        /*根据订单号得到分销商订单请求记录(当前需要得到原订单请求流水号)*/
                        List<Agent_requestlog> listagent_rlog = new Agent_requestlogData().GetAgent_requestlogByOrdernum(aorderid.ToString(), "add_order", "1");
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
                        "<code>{5}</code>" +//<!-- 验证电子码 y-->
                        "<order_num>{2}</order_num>" +//<!-- 订单号 y-->
                        "<num>{3}</num>" +//<!-- 使用张数 -->
                        "<use_time>{4}</use_time>" +//<!--  使用时间  -->
                        "</order>" +
                        "</business_trans>", listagent_rlog[0].Req_seq, log.Platform_req_seq, aorderid, confirmnum, CommonFunc.ConvertDateTimeInt(DateTime.Parse(confirmtime)), pno);

                        #region 分销通知发送方式post
                        if (agentinfo.inter_sendmethod.ToLower() == "post")
                        {
                            string re = "";
                            if (agentcomid == 6490)
                            {
                                re = new GetUrlData().HttpPost(updateurl, sbuilder);

                                updateurl += "?xml=" + sbuilder;//只为记录
                            }
                            else
                            {
                                updateurl += "?xml=" + sbuilder;
                                re = new GetUrlData().HttpPost(updateurl, "");
                            }


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
                        #region 分销通知发送方式get
                        else
                        {
                            if (updateurl.IndexOf('?') > -1)
                            {
                                updateurl += "&xml=" + sbuilder;
                            }
                            else
                            {
                                updateurl += "?xml=" + sbuilder;
                            }

                            string re = new GetUrlData().HttpGet(updateurl);
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
        #endregion 



        ///<summary>
        /// 字符流转换成Jsno对象
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static JObject StreamToString(Stream s)
        {
            //创建流的对象
            var sr = new StreamReader(s);
            //读取request的流：Json字符
            var stream = sr.ReadToEnd().ToString();
            //讲读取到的字符用字典存储
            Dictionary<string, object> str = (Dictionary<string, object>)new JavaScriptSerializer().DeserializeObject(stream);
             JObject jo = new JObject();
            foreach (var item in str)
            {
                //把字典转换成Json对象
                jo.Add(item.Key, item.Value.ToString());
 
            }
            return jo;

        }
        

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}